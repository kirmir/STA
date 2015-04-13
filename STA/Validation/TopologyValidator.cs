using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using STA.MineTopology;

namespace STA.Validation
{
    /// <summary>
    /// Validates mine topology.
    /// </summary>
    internal class TopologyValidator
    {
        /// <summary>
        /// Mine to validate.
        /// </summary>
        private readonly Mine _mine;

        /// <summary>
        /// List of validation errors.
        /// </summary>
        private ErrorsLog _errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopologyValidator"/> class.
        /// </summary>
        /// <param name="mine">The mine to validate.</param>
        public TopologyValidator(Mine mine)
        {
            _mine = mine;
        }

        /// <summary>
        /// Validates mine topology.
        /// </summary>
        /// <returns>List of validation errors.</returns>
        public ErrorsLog Validate()
        {
            _errors = new ErrorsLog();

            // Run validations.
            ValidateWrongConnections();
            ValidateConnectivity();

            return _errors;
        }

        /// <summary>
        /// Validates the wrong connections. Checks that there are no tunnels which has the same
        /// source and target nodes.
        /// </summary>
        private void ValidateWrongConnections()
        {
            // Check if any edge has the same target node as the source node.
            var errors = from e in _mine.Topology.Structures.First().Tunnels
                         where e.SourceNode == e.TargetNode
                         select new ValidationLogRecord
                                {
                                    Severity = XmlSeverityType.Error,
                                    Message = string.Format("Tunnel {0} has equal source and target nodes.", e.Name)
                                };

            foreach (var record in errors)
                _errors.Log.Add(record);
        }

        /// <summary>
        /// Validates the connectivity of the topology graph. Checks that any tunnel is connected
        /// with any other tunnel in the graph.
        /// </summary>
        private void ValidateConnectivity()
        {
            // Place all edges to additional list.
            var edges = _mine.Topology.Structures.First().Tunnels.ToList();

            // Set of visited nodes.
            var nodes = new HashSet<string>();

            // Process first edge.
            var first = edges.First();
            nodes.Add(first.SourceNode);
            nodes.Add(first.TargetNode);
            edges.Remove(first);

            // Validation for connectivity of all edges.
            while (edges.Any())
            {
                var connected = (from e in edges
                                 where nodes.Contains(e.SourceNode) || nodes.Contains(e.TargetNode)
                                 select e).ToList();

                if (!connected.Any())
                    break;

                foreach (var edge in connected)
                {
                    nodes.Add(edge.SourceNode);
                    nodes.Add(edge.TargetNode);
                    edges.Remove(edge);
                }
            }

            if (!edges.Any()) return;

            foreach (var edge in edges)
            {
                var record = new ValidationLogRecord
                                 {
                                     Severity = XmlSeverityType.Error,
                                     Message = string.Format(
                                         "Tunnel {0} isn't connected with the rest tunnels of mine topology.", edge.Name)
                                 };
                _errors.Log.Add(record);
            }
        }
    }
}

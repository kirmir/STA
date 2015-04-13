using System;
using System.Xml.Serialization;

namespace STA.MineTopology
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents mine with all it parameters and topology.
    /// </summary>
    [Serializable]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Mine
    {
        /// <summary>
        /// Type of the mine wit initial topology.
        /// </summary>
        public const string DefaultMineType = "Initial";

        /// <summary>
        /// Type for the mine with approximated and/or partitioned (secondary) topology.
        /// </summary>
        public const string SecondaryMineType = "Secondary";

        /// <summary>
        /// Default format version for the future improvements.
        /// </summary>
        public const string DefaultFormatVersion = "1.0.0.0";

        /// <summary>
        /// Initializes a new instance of the <see cref="Mine"/> class.
        /// </summary>
        public Mine()
        {
            Type = DefaultMineType;
            FormatVersion = DefaultFormatVersion;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [XmlElement(IsNullable = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [XmlElement(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        [XmlElement(IsNullable = true)]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the sound speed.
        /// </summary>
        /// <value>
        /// The sound speed.
        /// </value>
        [XmlElement(IsNullable = true)]
        public double? SoundSpeed { get; set; }

        /// <summary>
        /// Gets or sets the topology.
        /// </summary>
        /// <value>
        /// The topology.
        /// </value>
        public Topology Topology { get; set; }

        /// <summary>
        /// Gets or sets the format version.
        /// </summary>
        /// <value>
        /// The format version.
        /// </value>
        [XmlAttribute("formatVersion")]
        public string FormatVersion { get; set; }

        /// <summary>
        /// Gets all nodes from topology.
        /// </summary>
        /// <param name="tunnels">The tunnels.</param>
        /// <returns>All nodes.</returns>
        public static IEnumerable<string> GetNodes(List<Tunnel> tunnels)
        {
            return (from t in tunnels
                    select t.SourceNode).Union(from t in tunnels
                                               select t.TargetNode).Distinct();
        }

        /// <summary>
        /// Gets the local degree of the specified topology node.
        /// </summary>
        /// <param name="tunnels">The topology tunnels.</param>
        /// <param name="node">The node.</param>
        /// <returns>The local degree of the node.</returns>
        public static int GetLocalDegree(IEnumerable<Tunnel> tunnels, string node)
        {
            return (from t in tunnels
                    where t.SourceNode == node || t.TargetNode == node
                    select t).Count();
        }

        /// <summary>
        /// Gets the interlinks count for the tunnels set.
        /// </summary>
        /// <param name="tunnels">The tunnels.</param>
        /// <param name="nodesDegrees">The node's local degrees in whole graph.</param>
        /// <returns>The count of interlinks.</returns>
        public static int GetInterlinksCount(List<Tunnel> tunnels, IDictionary<string, int> nodesDegrees)
        {
            return (from n in GetNodes(tunnels)
                    select nodesDegrees[n] - GetLocalDegree(tunnels, n)).Sum();
        }
    }
}
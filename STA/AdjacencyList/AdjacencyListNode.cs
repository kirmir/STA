using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace STA.AdjacencyList
{
    /// <summary>
    /// The node of the modified adjacency list of topology graph.
    /// </summary>
    [Serializable]
    public class AdjacencyListNode
    {
        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tunnels.
        /// </summary>
        /// <value>
        /// The tunnels.
        /// </value>
        [XmlArrayItem("Tunnel")]
        public List<AdjacencyListItem> Tunnels { get; set; }
    }
}

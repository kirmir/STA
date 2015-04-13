using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using STA.AdjacencyList;

namespace STA.MineTopology
{
    /// <summary>
    /// Mine topology that contains tunnels structures and adjacency list.
    /// </summary>
    [Serializable]
    public class Topology
    {
        /// <summary>
        /// Gets or sets the topology structures.
        /// </summary>
        /// <value>
        /// The topology structures.
        /// </value>
        [XmlArrayItem("Structure", IsNullable = false)]
        public List<Structure> Structures { get; set; }

        /// <summary>
        /// Gets or sets the adjacency list.
        /// </summary>
        /// <value>
        /// The adjacency list.
        /// </value>
        [XmlArrayItem("Node", IsNullable = true)]
        public List<AdjacencyListNode> AdjacencyList { get; set; }
    }
}

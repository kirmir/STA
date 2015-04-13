using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace STA.MineTopology
{
    /// <summary>
    /// Mine topology structure, description of all tunnels (edges of graph).
    /// </summary>
    [Serializable]
    public class Structure
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tunnels.
        /// </summary>
        /// <value>
        /// The tunnels.
        /// </value>
        [XmlArrayItem("Tunnel", IsNullable = false)]
        public List<Tunnel> Tunnels { get; set; }
    }
}
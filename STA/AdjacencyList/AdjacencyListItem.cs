using System;

namespace STA.AdjacencyList
{
    /// <summary>
    /// The item of the modified adjacency list of topology graph.
    /// </summary>
    [Serializable]
    public class AdjacencyListItem
    {
        /// <summary>
        /// Gets or sets the tunnel name.
        /// </summary>
        /// <value>
        /// The tunnel name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tunnel's direction.
        /// </summary>
        /// <value>
        /// The tunnel's direction.
        /// </value>
        public TunnelDirection Direction { get; set; }
    }
}

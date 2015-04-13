using System.Collections.Generic;

namespace STA.MineTopology
{
    /// <summary>
    /// Represents part of the topology with its characteristic.
    /// </summary>
    internal class TopologyPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopologyPart"/> class.
        /// </summary>
        public TopologyPart()
        {
            Tunnels = new List<Tunnel>();
        }

        /// <summary>
        /// Gets or sets the tunnels.
        /// </summary>
        /// <value>
        /// The tunnels.
        /// </value>
        public List<Tunnel> Tunnels { get; set; }

        /// <summary>
        /// Gets or sets the interlinks count.
        /// </summary>
        /// <value>
        /// The interlinks count.
        /// </value>
        public int InterlinksCount { get; set; }
    }
}

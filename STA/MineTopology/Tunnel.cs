using System;
using System.Xml.Serialization;

namespace STA.MineTopology
{
    /// <summary>
    /// Tunnel of the mine topology (edge of the topology graph).
    /// </summary>
    [Serializable]
    public class Tunnel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the source node (where tunnel starts).
        /// </summary>
        /// <value>
        /// The source node.
        /// </value>
        public string SourceNode { get; set; }

        /// <summary>
        /// Gets or sets the target node (where tunnel ends).
        /// </summary>
        /// <value>
        /// The target node.
        /// </value>
        public string TargetNode { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [XmlElement(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the length of the tunnel.
        /// </summary>
        /// <value>
        /// The length of the tunnel.
        /// </value>
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the cross-section area of the tunnel.
        /// </summary>
        /// <value>
        /// The cross-section area of the tunnel.
        /// </value>
        public double CrossSectionArea { get; set; }

        /// <summary>
        /// Gets or sets the air density in the tunnel.
        /// </summary>
        /// <value>
        /// The air density in the tunnel.
        /// </value>
        public double AirDensity { get; set; }

        /// <summary>
        /// Gets or sets the air resistance in the tunnel.
        /// </summary>
        /// <value>
        /// The air resistance in the tunnel.
        /// </value>
        public double AirResistance { get; set; }

        /// <summary>
        /// Gets or sets the air pressure (if it exists) on the source node.
        /// </summary>
        /// <value>
        /// The air pressure (if it exists) on the source node.
        /// </value>
        public double SourceAirPressure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether source air pressure value is specified.
        /// </summary>
        /// <value>
        /// <c>true</c> if source air pressure value is specified; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnoreAttribute]
        public bool SourceAirPressureSpecified { get; set; }

        /// <summary>
        /// Gets or sets the air pressure (if it exists) on the target node.
        /// </summary>
        /// <value>
        /// The air pressure (if it exists) on the target node.
        /// </value>
        public double TargetAirPressure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether target air pressure value is specified.
        /// </summary>
        /// <value>
        /// <c>true</c> if target air pressure value is specified; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnoreAttribute]
        public bool TargetAirPressureSpecified { get; set; }
    }
}
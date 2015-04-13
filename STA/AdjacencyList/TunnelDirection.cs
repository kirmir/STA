namespace STA.AdjacencyList
{
    using System;

    /// <summary>
    /// The tunnel's direction.
    /// </summary>
    [Serializable]
    public enum TunnelDirection
    {
        /// <summary>
        /// Direction out of the node.
        /// </summary>
        Out,

        /// <summary>
        /// Direction into the node.
        /// </summary>
        In
    }
}
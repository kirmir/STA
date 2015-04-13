using System;
using System.Collections.Generic;
using System.Linq;
using STA.AdjacencyList;
using STA.MineTopology;

namespace STA
{
    /// <summary>
    /// Do topological analysis functions on mine topology.
    /// </summary>
    internal class TopologyProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopologyProcessor"/> class.
        /// </summary>
        /// <param name="mine">The mine for processing on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mine"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Mine topology is empty or has invalid data.</exception>
        public TopologyProcessor(Mine mine)
        {
            if (mine == null)
                throw new ArgumentNullException("mine", "Mine topology is invalid");

            Mine = mine;
        }

        /// <summary>
        /// The topology description for processing on.
        /// </summary>
        public Mine Mine { get; private set; }

        /// <summary>
        /// Gets the initial parts of topology. Simply partition to parts with equal size.
        /// </summary>
        /// <param name="tunnels">The tunnels to partition.</param>
        /// <param name="partsNumber">Number of parts.</param>
        /// <returns>Parts of topology.</returns>
        public static List<TopologyPart> GetInitialParts(List<Tunnel> tunnels, int partsNumber)
        {
            // Calculate number of tunnels in each part.
            var n = (int)Math.Round((double)tunnels.Count / partsNumber);
            var lastN = tunnels.Count - n * (partsNumber - 1);

            // Generating parts.
            var parts = new List<TopologyPart>();

            for (var i = 0; i < partsNumber - 1; i++)
            {
                var part = new TopologyPart();
                part.Tunnels.AddRange(tunnels.GetRange(i * n, n));
                parts.Add(part);
            }

            // Add last part.
            var last = new TopologyPart();
            last.Tunnels.AddRange(tunnels.GetRange((partsNumber - 1) * n, lastN));
            parts.Add(last);

            return parts;
        }

        /// <summary>
        /// Optimizes the topology parts by swapping their tunnels.
        /// </summary>
        /// <param name="part1">First part.</param>
        /// <param name="part2">Second part.</param>
        /// <param name="nodesDegrees">The node's local degrees in whole graph.</param>
        /// <returns><c>true</c>, if some tunnels were swapped; otherwise - <c>false</c>.</returns>
        public static bool OptimizeParts(TopologyPart part1, TopologyPart part2, IDictionary<string, int> nodesDegrees)
        {
            // For saving information about the best swap tunnels option.
            var maxDelta = 0;
            var maxI = 0;
            var maxJ = 0;
            var maxCount1 = 0;
            var maxCount2 = 0;

            // Run over all possible pairs of tunnels to swap.
            for (var i = 0; i < part1.Tunnels.Count; i++)
                for (var j = 0; j < part2.Tunnels.Count; j++)
                {
                    // Try to swap and look the changing of interlinks count.
                    Helpers.Swap(part1.Tunnels, part2.Tunnels, i, j);

                    var count1 = Mine.GetInterlinksCount(part1.Tunnels, nodesDegrees);
                    var count2 = Mine.GetInterlinksCount(part2.Tunnels, nodesDegrees);
                    var delta = part1.InterlinksCount - count1 + part2.InterlinksCount - count2;

                    // Save maximum.
                    if (delta > maxDelta)
                    {
                        maxDelta = delta;
                        maxI = i;
                        maxJ = j;
                        maxCount1 = count1;
                        maxCount2 = count2;
                    }

                    // Swap back.
                    Helpers.Swap(part1.Tunnels, part2.Tunnels, i, j);
                }

            // No possible swaps that will improve parts.
            if (maxDelta == 0)
                return false;

            // Found the best swap option.
            Helpers.Swap(part1.Tunnels, part2.Tunnels, maxI, maxJ);
            part1.InterlinksCount = maxCount1;
            part2.InterlinksCount = maxCount2;

            return true;
        }

        /// <summary>
        /// Generates the adjacency list for the first topology structure in the mine. Must be
        /// used on the whole graph (before partitioning).
        /// </summary>
        public void GenerateAdjacencyList()
        {
            var nodes = Mine.GetNodes(Mine.Topology.Structures.First().Tunnels);

            // Generate items for each node's list.
            Mine.Topology.AdjacencyList = (from n in nodes
                                           select new AdjacencyListNode
                                                      {
                                                          Name = n,
                                                          Tunnels = (from t in Mine.Topology.Structures.First().Tunnels
                                                                     where t.SourceNode == n || t.TargetNode == n
                                                                     select new AdjacencyListItem
                                                                                {
                                                                                    Name = t.Name,
                                                                                    Direction = t.SourceNode == n
                                                                                        ? TunnelDirection.Out
                                                                                        : TunnelDirection.In
                                                                                }).ToList()
                                                      }).ToList();
        }

        /// <summary>
        /// Approximates mine topology.
        /// </summary>
        /// <param name="dx">The delta X value for approximation.</param>
        public void Approximate(double dx)
        {
            var approximated = new List<Tunnel>();

            foreach (var tunnel in Mine.Topology.Structures.First().Tunnels)
            {
                // Skip tunnel if its length less than dx.
                if (tunnel.Length <= dx)
                {
                    approximated.Add(tunnel);
                    continue;
                }

                // Calculating lengths.
                var n = (int)Math.Ceiling(tunnel.Length / dx);
                var lastLength = tunnel.Length % dx;

                if (lastLength <= 0)
                    lastLength = dx;

                for (var i = 1; i <= n; i++)
                {
                    // Create new part.
                    var part = new Tunnel
                               {
                                   AirDensity = tunnel.AirDensity,
                                   AirResistance = tunnel.AirResistance,
                                   CrossSectionArea = tunnel.CrossSectionArea,
                                   Description = string.Format("Part of [{0}]", tunnel.Description),
                                   Length = i == n ? lastLength : dx,
                                   Name = string.Format("{0}.{1}", tunnel.Name, i)
                               };

                    // Setting new source node's name.
                    if (i == 1) part.SourceNode = tunnel.SourceNode;
                    else if (n == 2) part.SourceNode = string.Format("U{0}", tunnel.Name);
                    else part.SourceNode = string.Format("U{0}.{1}", tunnel.Name, i - 1);

                    // Setting new target node's name.
                    if (i == n) part.TargetNode = tunnel.TargetNode;
                    else if (n == 2) part.TargetNode = string.Format("U{0}", tunnel.Name);
                    else part.TargetNode = string.Format("U{0}.{1}", tunnel.Name, i);

                    // Set air pressures at the source and target nodes.
                    if (i == 1)
                    {
                        part.SourceAirPressure = tunnel.SourceAirPressure;
                        part.SourceAirPressureSpecified = tunnel.SourceAirPressureSpecified;
                    }

                    if (i == n)
                    {
                        part.TargetAirPressure = tunnel.TargetAirPressure;
                        part.TargetAirPressureSpecified = tunnel.TargetAirPressureSpecified;
                    }

                    // Add new part.
                    approximated.Add(part);
                }
            }

            // Replace original structure with approximated one.
            Mine.Topology.Structures.First().Tunnels = approximated;
        }

        /// <summary>
        /// Partitions mine topology.
        /// </summary>
        /// <param name="partsNumber">The number of parts for partitioning.</param>
        public void Partition(int partsNumber)
        {
            // No need to partition if parts number is 1.
            if (partsNumber == 1) return;

            var tunnels = Mine.Topology.Structures.First().Tunnels;

            if (partsNumber > tunnels.Count)
                throw new ArgumentException("Specified number of parts exceeds number of tunnels.", "partsNumber");

            // Initial partitioning.
            var parts = GetInitialParts(tunnels, partsNumber);

            // Check if all parts contains only one tunnel. In this case we don't need to make optimization.
            var containsMany = parts.Any(p => p.Tunnels.Count > 1);

            if (containsMany)
            {
                // Get all nodes and calculate local degree of each one.
                var nodesDegrees = Mine.GetNodes(tunnels).ToDictionary(n => n, n => Mine.GetLocalDegree(tunnels, n));

                // For each part calculate count of interlinks.
                foreach (var part in parts)
                    part.InterlinksCount = Mine.GetInterlinksCount(part.Tunnels, nodesDegrees);

                // Optimize all parts (in pairs).
                bool swapped;
                do
                {
                    swapped = false;
                    for (var i = 0; i < partsNumber - 1; i++)
                        for (var j = i + 1; j < partsNumber; j++)
                            swapped = swapped || OptimizeParts(parts[i], parts[j], nodesDegrees);
                }
                while (swapped);
            }

            // Update mine with new topology structures.
            var baseName = Mine.Topology.Structures.First().Name;
            Mine.Topology.Structures.Clear();
            
            for (var i = 0; i < partsNumber; i++)
            {
                var structure = new Structure
                                    {
                                        Name = string.Format("{0}.{1}", baseName, i + 1),
                                        Tunnels = parts[i].Tunnels
                                    };

                Mine.Topology.Structures.Add(structure);
            }
        }
    }
}

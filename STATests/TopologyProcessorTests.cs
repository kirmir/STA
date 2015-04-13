namespace STATests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using STA;
    using STA.AdjacencyList;
    using STA.MineTopology;

    /// <summary>
    /// Tests for <see cref="TopologyProcessor"/>
    /// </summary>
    [TestFixture]
    public class TopologyProcessorTests
    {
        /// <summary>
        /// Test for creation processor without mine object.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullMineCreationTest()
        {
            // Execute.
            new TopologyProcessor(null);
        }

        /// <summary>
        /// Test for successful creation of the <see cref="TopologyProcessor"/>.
        /// </summary>
        [Test]
        public void SuccessfullCreationTest()
        {
            // Setup.
            var mine = new Mine();

            // Execute.
            var processor = new TopologyProcessor(mine);

            // Verify.
            Assert.AreSame(mine, processor.Mine);
        }

        /// <summary>
        /// Test for initial partitioning of graph's tunnels.
        /// </summary>
        [Test]
        public void InitialPartitioningTest()
        {
            // Setup.
            var initTunnels = new List<Tunnel>
                {
                    new Tunnel { Name = "Q1", SourceNode = "U1", TargetNode = "U2" },
                    new Tunnel { Name = "Q2", SourceNode = "U2", TargetNode = "U3" },
                    new Tunnel { Name = "Q3", SourceNode = "U2", TargetNode = "U4" },
                    new Tunnel { Name = "Q4", SourceNode = "U3", TargetNode = "U4" },
                    new Tunnel { Name = "Q5", SourceNode = "U1", TargetNode = "U4" },
                };

            // Execute.
            var parts = TopologyProcessor.GetInitialParts(initTunnels, 3);

            // Verify.
            Assert.AreEqual(3, parts.Count);

            var tunnels = (from p in parts from t in p.Tunnels select t).ToList();
            Assert.AreEqual(5, tunnels.Count);

            var diff = initTunnels.Except(tunnels);
            Assert.AreEqual(0, diff.Count());
        }

        /// <summary>
        /// Test of approximation function.
        /// </summary>
        [Test]
        public void ApproximationTest()
        {
            // Setup.
            var initTunnels = new List<Tunnel>
                {
                    new Tunnel { Name = "Q1", SourceNode = "U1", TargetNode = "U2", Length = 500 },
                    new Tunnel { Name = "Q2", SourceNode = "U2", TargetNode = "U3", Length = 100 },
                    new Tunnel { Name = "Q3", SourceNode = "U3", TargetNode = "U4", Length = 900 }
                };

            var mine = new Mine { Topology = new Topology { Structures = new List<Structure>() } };
            var structure = new Structure { Tunnels = initTunnels };
            mine.Topology.Structures.Add(structure);

            // Execute.
            var processor = new TopologyProcessor(mine);
            processor.Approximate(300);

            // Verify.
            var tunnels = mine.Topology.Structures.First().Tunnels;
            Assert.AreEqual(6, tunnels.Count);

            // Verifying existence of parts with expected names.
            var q11 = tunnels.FirstOrDefault(t => t.Name == "Q1.1");
            var q12 = tunnels.FirstOrDefault(t => t.Name == "Q1.2");
            var q2 = tunnels.FirstOrDefault(t => t.Name == "Q2");
            var q31 = tunnels.FirstOrDefault(t => t.Name == "Q3.1");
            var q32 = tunnels.FirstOrDefault(t => t.Name == "Q3.2");
            var q33 = tunnels.FirstOrDefault(t => t.Name == "Q3.3");
            Assert.IsNotNull(q11);
            Assert.IsNotNull(q12);
            Assert.IsNotNull(q2);
            Assert.IsNotNull(q31);
            Assert.IsNotNull(q32);
            Assert.IsNotNull(q33);

            // Verifying lengths.
            Assert.AreEqual(300, q11.Length);
            Assert.AreEqual(200, q12.Length);
            Assert.AreEqual(100, q2.Length);
            Assert.AreEqual(300, q31.Length);
            Assert.AreEqual(300, q32.Length);
            Assert.AreEqual(300, q33.Length);

            // Verifying connections.
            Assert.AreEqual("U1", q11.SourceNode);
            Assert.AreEqual("UQ1", q11.TargetNode);
            Assert.AreEqual("UQ1", q12.SourceNode);
            Assert.AreEqual("U2", q12.TargetNode);
            Assert.AreEqual("U2", q2.SourceNode);
            Assert.AreEqual("U3", q2.TargetNode);
            Assert.AreEqual("U3", q31.SourceNode);
            Assert.AreEqual("UQ3.1", q31.TargetNode);
            Assert.AreEqual("UQ3.1", q32.SourceNode);
            Assert.AreEqual("UQ3.2", q32.TargetNode);
            Assert.AreEqual("UQ3.2", q33.SourceNode);
            Assert.AreEqual("U4", q33.TargetNode);
        }

        /// <summary>
        /// Test of generating adjacency list.
        /// </summary>
        [Test]
        public void GeneratingAdjacencyListTest()
        {
            // Setup.
            var initTunnels = new List<Tunnel>
                {
                    new Tunnel { Name = "Q1", SourceNode = "U1", TargetNode = "U2", Length = 500 },
                    new Tunnel { Name = "Q2", SourceNode = "U2", TargetNode = "U3", Length = 100 }
                };

            var mine = new Mine { Topology = new Topology { Structures = new List<Structure>() } };
            var structure = new Structure { Tunnels = initTunnels };
            mine.Topology.Structures.Add(structure);

            // Execute.
            var processor = new TopologyProcessor(mine);
            processor.GenerateAdjacencyList();

            // Verify.
            var items = mine.Topology.AdjacencyList;
            Assert.AreEqual(3, items.Count);

            var u1 = items.FirstOrDefault(n => n.Name == "U1");
            var u2 = items.FirstOrDefault(n => n.Name == "U2");
            var u3 = items.FirstOrDefault(n => n.Name == "U3");
            Assert.IsNotNull(u1);
            Assert.IsNotNull(u2);
            Assert.IsNotNull(u3);

            Assert.AreEqual(1, u1.Tunnels.Count);
            Assert.AreEqual(2, u2.Tunnels.Count);
            Assert.AreEqual(1, u3.Tunnels.Count);

            var q1InU1 = u1.Tunnels.FirstOrDefault(t => t.Name == "Q1");
            Assert.IsNotNull(q1InU1);
            Assert.AreEqual(TunnelDirection.Out, q1InU1.Direction);

            var q1InU2 = u2.Tunnels.FirstOrDefault(t => t.Name == "Q1");
            var q2InU2 = u2.Tunnels.FirstOrDefault(t => t.Name == "Q2");
            Assert.IsNotNull(q1InU2);
            Assert.IsNotNull(q2InU2);
            Assert.AreEqual(TunnelDirection.In, q1InU2.Direction);
            Assert.AreEqual(TunnelDirection.Out, q2InU2.Direction);

            var q2InU3 = u3.Tunnels.FirstOrDefault(t => t.Name == "Q2");
            Assert.IsNotNull(q2InU3);
            Assert.AreEqual(TunnelDirection.In, q2InU3.Direction);
        }

        /// <summary>
        /// Test of partition function.
        /// </summary>
        [Test]
        public void PartitionTest()
        {
            // Setup.
            var initTunnels = new List<Tunnel>
                {
                    new Tunnel { Name = "Q2", SourceNode = "U1", TargetNode = "U3" },
                    new Tunnel { Name = "Q5", SourceNode = "U4", TargetNode = "U5" },
                    new Tunnel { Name = "Q1", SourceNode = "U1", TargetNode = "U2" },
                    new Tunnel { Name = "Q4", SourceNode = "U3", TargetNode = "U4" },
                    new Tunnel { Name = "Q3", SourceNode = "U2", TargetNode = "U3" }
                };

            var mine = new Mine { Topology = new Topology { Structures = new List<Structure>() } };
            var structure = new Structure { Name = "G1", Tunnels = initTunnels };
            mine.Topology.Structures.Add(structure);

            // Execute.
            var processor = new TopologyProcessor(mine);
            processor.Partition(2);

            // Verify.
            Assert.AreEqual(2, mine.Topology.Structures.Count);

            var partWith3 = mine.Topology.Structures.FirstOrDefault(s => s.Tunnels.Count == 3);
            var partWith2 = mine.Topology.Structures.FirstOrDefault(s => s.Tunnels.Count == 2);
            Assert.IsNotNull(partWith3);
            Assert.IsNotNull(partWith2);

            Assert.IsTrue((partWith3.Name == "G1.1" && partWith2.Name == "G1.2")
                || (partWith3.Name == "G1.2" && partWith2.Name == "G1.1"));

            var q1 = partWith3.Tunnels.FirstOrDefault(t => t.Name == "Q1");
            var q2 = partWith3.Tunnels.FirstOrDefault(t => t.Name == "Q2");
            var q3 = partWith3.Tunnels.FirstOrDefault(t => t.Name == "Q3");
            var q4 = partWith2.Tunnels.FirstOrDefault(t => t.Name == "Q4");
            var q5 = partWith2.Tunnels.FirstOrDefault(t => t.Name == "Q5");
            Assert.IsNotNull(q1);
            Assert.IsNotNull(q2);
            Assert.IsNotNull(q3);
            Assert.IsNotNull(q4);
            Assert.IsNotNull(q5);
        }

        /// <summary>
        /// Test of partition to 1 part.
        /// </summary>
        [Test]
        public void PartitionTo1PartTest()
        {
            // Setup.
            var initTunnels = new List<Tunnel>
                {
                    new Tunnel { Name = "Q1", SourceNode = "U1", TargetNode = "U2" },
                    new Tunnel { Name = "Q2", SourceNode = "U2", TargetNode = "U3" }
                };

            var mine = new Mine { Topology = new Topology { Structures = new List<Structure>() } };
            var structure = new Structure { Tunnels = initTunnels };
            mine.Topology.Structures.Add(structure);

            // Execute.
            var processor = new TopologyProcessor(mine);
            processor.Partition(1);

            // Verify.
            Assert.AreEqual(1, mine.Topology.Structures.Count);
        }

        /// <summary>
        /// Test of partition to 1 part.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void PartitionToMorePartsThanTunnelsTest()
        {
            // Setup.
            var initTunnels = new List<Tunnel>
                {
                    new Tunnel { Name = "Q1", SourceNode = "U1", TargetNode = "U2" },
                    new Tunnel { Name = "Q2", SourceNode = "U2", TargetNode = "U3" }
                };

            var mine = new Mine { Topology = new Topology { Structures = new List<Structure>() } };
            var structure = new Structure { Tunnels = initTunnels };
            mine.Topology.Structures.Add(structure);

            // Execute.
            var processor = new TopologyProcessor(mine);
            processor.Partition(3);
        }
    }
}

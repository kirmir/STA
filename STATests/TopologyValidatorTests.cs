namespace STATests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using STA.MineTopology;
    using STA.Validation;

    /// <summary>
    /// Tests for <see cref="TopologyValidator"/>
    /// </summary>
    [TestFixture]
    public class TopologyValidatorTests
    {
        /// <summary>
        /// Test of validation of correct topology.
        /// </summary>
        [Test]
        public void CorrectTopologyValidationTest()
        {
            // Setup.
            var initTunnels = new List<Tunnel>
                {
                    new Tunnel { Name = "Q1", SourceNode = "U1", TargetNode = "U2" },
                    new Tunnel { Name = "Q2", SourceNode = "U1", TargetNode = "U3" },
                    new Tunnel { Name = "Q3", SourceNode = "U2", TargetNode = "U3" },
                    new Tunnel { Name = "Q4", SourceNode = "U3", TargetNode = "U4" },
                    new Tunnel { Name = "Q5", SourceNode = "U4", TargetNode = "U5" }
                };

            var mine = new Mine { Topology = new Topology { Structures = new List<Structure>() } };
            var structure = new Structure { Tunnels = initTunnels };
            mine.Topology.Structures.Add(structure);

            // Execute.
            var validator = new TopologyValidator(mine);
            var errors = validator.Validate();

            // Verify.
            Assert.IsEmpty(errors.Log);
        }

        /// <summary>
        /// Test of validation of incorrect topology.
        /// </summary>
        [Test]
        public void IncorrectTopologyValidationTest()
        {
            // Setup.
            var initTunnels = new List<Tunnel>
                {
                    new Tunnel { Name = "Q1", SourceNode = "U1", TargetNode = "U2" },
                    new Tunnel { Name = "Q2", SourceNode = "U1", TargetNode = "U3" },
                    new Tunnel { Name = "Q3", SourceNode = "U2", TargetNode = "U3" },
                    new Tunnel { Name = "Q4", SourceNode = "U5", TargetNode = "U4" },
                    new Tunnel { Name = "Q5", SourceNode = "U4", TargetNode = "U4" }
                };

            var mine = new Mine { Topology = new Topology { Structures = new List<Structure>() } };
            var structure = new Structure { Tunnels = initTunnels };
            mine.Topology.Structures.Add(structure);

            // Execute.
            var validator = new TopologyValidator(mine);
            var errors = validator.Validate();

            // Verify.
            Assert.AreEqual(3, errors.Log.Count);
        }
    }
}

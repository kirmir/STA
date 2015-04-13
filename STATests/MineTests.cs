namespace STATests
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using STA.MineTopology;

    /// <summary>
    /// Tests for <see cref="Mine"/>.
    /// </summary>
    [TestFixture]
    public class MineTests
    {
        /// <summary>
        /// Mine tunnels for tests.
        /// </summary>
        private List<Tunnel> _tunnels;

        /// <summary>
        /// Local degrees of nodes.
        /// </summary>
        private IDictionary<string, int> _localDegrees;

        /// <summary>
        /// Setups tests.
        /// </summary>
        [TestFixtureSetUp]
        public void Init()
        {
            _tunnels = new List<Tunnel>
                {
                    new Tunnel { Name = "Q1", SourceNode = "U1", TargetNode = "U2" },
                    new Tunnel { Name = "Q2", SourceNode = "U2", TargetNode = "U3" },
                    new Tunnel { Name = "Q3", SourceNode = "U2", TargetNode = "U4" },
                    new Tunnel { Name = "Q4", SourceNode = "U3", TargetNode = "U4" }
                };

            _localDegrees = new Dictionary<string, int> { { "U1", 1 }, { "U2", 3 }, { "U3", 2 }, { "U4", 2 } };
        }

        /// <summary>
        /// Test for GetNodes method.
        /// </summary>
        [Test]
        public void GettingNodesTest()
        {
            // Execute.
            var nodes = Mine.GetNodes(this._tunnels).ToList();

            // Verify.
            Assert.AreEqual(4, nodes.Count);
            Assert.Contains("U1", nodes);
            Assert.Contains("U2", nodes);
            Assert.Contains("U3", nodes);
            Assert.Contains("U4", nodes);
        }

        /// <summary>
        /// Test for GetLocalDegree method.
        /// </summary>
        [Test]
        public void GettingLocalDegreeTest()
        {
            // Execute.
            var degreeU2 = Mine.GetLocalDegree(_tunnels, "U2");
            var degreeUx = Mine.GetLocalDegree(_tunnels, "UX");

            // Verify.
            Assert.AreEqual(_localDegrees["U2"], degreeU2);
            Assert.AreEqual(0, degreeUx);
        }

        /// <summary>
        /// Test for GetInterlinksCount method.
        /// </summary>
        [Test]
        public void GettingInterlinksCountTest()
        {
            // Setup.
            var part = new List<Tunnel>();
            part.AddRange(_tunnels.GetRange(0, 2));

            // Execute.
            var count = Mine.GetInterlinksCount(part, _localDegrees);

            // Verify.
            Assert.AreEqual(2, count);
        }
    }
}
namespace STATests
{
    using System.Linq;

    using NUnit.Framework;

    using STA;
    using STA.Validation;

    /// <summary>
    /// Tests for <see cref="MineTopologyReader"/>.
    /// </summary>
    [TestFixture]
    public class MineTopologyReaderTests
    {
        /// <summary>
        /// Test of reading of correct topology.
        /// </summary>
        [Test]
        public void CorrectReadingTest()
        {
            // Execute.
            var reader = new MineTopologyReader(@"..\..\..\STATests\CorrectInputData.xml");
            var mine = reader.Read();

            // Verify.
            Assert.IsNotNull(mine);
            Assert.AreEqual(2, mine.Topology.Structures.First().Tunnels.Count);
        }

        /// <summary>
        /// Test of reading of incorrect topology.
        /// </summary>
        [Test]
        [ExpectedException(typeof(TopologyValidationException))]
        public void IncorrectReadingTest()
        {
            // Execute.
            var reader = new MineTopologyReader(@"..\..\..\STATests\IncorrectInputData.xml");
            reader.Read();
        }
    }
}

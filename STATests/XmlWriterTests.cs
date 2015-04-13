namespace STATests
{
    using System.Collections.Generic;
    using System.IO;

    using NUnit.Framework;

    using STA;
    using STA.MineTopology;

    /// <summary>
    /// Tests for <see cref="XmlWriter"/>
    /// </summary>
    [TestFixture]
    public class XmlWriterTests
    {
        /// <summary>
        /// File name of test file for writing results.
        /// </summary>
        public const string TestOutFileName = @"Out.xml";

        /// <summary>
        /// Test for writing mine object to file.
        /// </summary>
        [Test]
        public void WriteMineToFile()
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
            XmlWriter.Write(mine, TestOutFileName);

            // Verify.
            Assert.IsTrue(File.Exists(TestOutFileName));
        }

        /// <summary>
        /// Deletes created files after running tests.
        /// </summary>
        [TearDown]
        public void CleanUp()
        {
            if (File.Exists(TestOutFileName))
                File.Delete(TestOutFileName);
        }
    }
}

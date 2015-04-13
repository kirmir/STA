namespace STATests
{
    using NUnit.Framework;
    using STA;

    /// <summary>
    /// Tests for <see cref="StaSettings"/>.
    /// </summary>
    [TestFixture]
    public class StaSettingsTests
    {
        /// <summary>
        /// Test for reading valid settings.
        /// </summary>
        [Test]
        public void ValidSettingsTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/V:Errors.xml", "/A:100", "/P:3", "Output.xml" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.AreEqual("Input.xml", settings.InputTopologyFileName);
            Assert.AreEqual("Errors.xml", settings.OutputErrorsLogFileName);
            Assert.AreEqual("Output.xml", settings.OutputTopologyFileName);
            Assert.IsTrue(settings.ValidationEnabled);
            Assert.IsTrue(settings.ApproximationEnabled);
            Assert.IsTrue(settings.PartitioningEnabled);
            Assert.IsFalse(settings.ShowHelp);
            Assert.AreEqual(100, settings.DeltaX);
            Assert.AreEqual(3, settings.PartsNumber);
        }

        /// <summary>
        /// Test for reading help parameter.
        /// </summary>
        [Test]
        public void HelpTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/?", "/V:Errors.xml" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsTrue(settings.ShowHelp);
            Assert.AreEqual("Input.xml", settings.InputTopologyFileName);
            Assert.IsFalse(settings.ValidationEnabled);
            Assert.AreNotEqual("Errors.xml", settings.OutputErrorsLogFileName);
        }

        /// <summary>
        /// Test for reading null arguments.
        /// </summary>
        [Test]
        public void NoArgsTest()
        {
            // Execute.
            var settings = StaSettings.GetStaSettings(null);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading only 1 argument but not help option.
        /// </summary>
        [Test]
        public void OneArgNotHelpArgsTest()
        {
            // Setup.
            var args = new[] { "123" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments with excess file name.
        /// </summary>
        [Test]
        public void ExcessFileNameTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "FunnyCats.jpg", "/V:Errors.xml", "/A:100", "Output.xml" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments with invalid option.
        /// </summary>
        [Test]
        public void InvalidOptionTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments with unknown option.
        /// </summary>
        [Test]
        public void UnknownOptionTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/X" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments without input file name.
        /// </summary>
        [Test]
        public void NoInputFileNameTest()
        {
            // Setup.
            var args = new[] { "/A:100", "/P:3" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments without output file name.
        /// </summary>
        [Test]
        public void NoOutputFileNameTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/V:Errors.xml", "/A:100" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments without approximation number specified.
        /// </summary>
        [Test]
        public void NoApproximationNumberTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/V:Errors.xml", "/A", "Output.xml" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments with invalid approximation number specified.
        /// </summary>
        [Test]
        public void InvalidApproximationNumberTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/V:Errors.xml", "/A:-10", "Output.xml" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments without parts number specified.
        /// </summary>
        [Test]
        public void NoPartsNumberTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/V:Errors.xml", "/P", "Output.xml" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments with invalid parts number specified.
        /// </summary>
        [Test]
        public void InvalidPartsNumberTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/V:Errors.xml", "/P:-10", "Output.xml" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }

        /// <summary>
        /// Test for reading arguments without name of the file for validation errors log.
        /// </summary>
        [Test]
        public void NoErrorsLogFileNameTest()
        {
            // Setup.
            var args = new[] { "Input.xml", "/V", "/P:3", "Output.xml" };

            // Execute.
            var settings = StaSettings.GetStaSettings(args);

            // Validate.
            Assert.IsNull(settings);
        }
    }
}

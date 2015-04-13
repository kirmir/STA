namespace STA
{
    using System.Collections.Generic;

    /// <summary>
    /// Modes settings of the STA.
    /// </summary>
    internal class StaSettings
    {
        #region Command-line parameter's symbols

        /// <summary>
        /// Symbols for calling help for command-line.
        /// </summary>
        public const string HelpParameter = "/?";

        /// <summary>
        /// Symbols for enabling validation mode for command-line.
        /// </summary>
        public const string ValidationParameter = "/V";

        /// <summary>
        /// Symbols for enabling approximation mode for command-line.
        /// </summary>
        public const string ApproximationParameter = "/A";

        /// <summary>
        /// Symbols for enabling partitioning mode for command-line.
        /// </summary>
        public const string PartitioningParameter = "/P";

        #endregion

        /// <summary>
        /// Prevents a default instance of the <see cref="StaSettings"/> class from being created.
        /// </summary>
        private StaSettings()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether help is showing on the start.
        /// </summary>
        public bool ShowHelp { get; set; }

        #region Functions

        /// <summary>
        /// Gets or sets a value indicating whether validation function is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if validation function is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool ValidationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether approximation function is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if approximation function is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool ApproximationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether partitioning function is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if partitioning function is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool PartitioningEnabled { get; set; }

        #endregion

        #region Input and output files names

        /// <summary>
        /// Gets or sets the name of the input topology file.
        /// </summary>
        /// <value>
        /// The name of the input topology file.
        /// </value>
        public string InputTopologyFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the output topology file.
        /// </summary>
        /// <value>
        /// The name of the output topology file.
        /// </value>
        public string OutputTopologyFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the output errors log file.
        /// </summary>
        /// <value>
        /// The name of the output errors log file.
        /// </value>
        public string OutputErrorsLogFileName { get; set; }

        #endregion

        #region Parameters for STA functions

        /// <summary>
        /// Gets or sets the delta X value for approximation function.
        /// </summary>
        /// <value>
        /// The delta X value.
        /// </value>
        public double DeltaX { get; set; }

        /// <summary>
        /// Gets or sets the parts number for partitioning function.
        /// </summary>
        /// <value>
        /// The parts number.
        /// </value>
        public int PartsNumber { get; set; }

        #endregion

        /// <summary>
        /// Gets the STA settings from their string representations.
        /// </summary>
        /// <param name="args">The string array that contains STA settings.</param>
        /// <returns>STA settings from their string representation.</returns>
        public static StaSettings GetStaSettings(IList<string> args)
        {
            var settings = new StaSettings();

            // Check if no arguments were specified.
            if (args == null || args.Count == 0)
                return null;

            // Check if only help arguments was specified.
            if (args.Count == 1 && args[0] != HelpParameter)
                return null;

            // Analyze each argument.
            foreach (var arg in args)
            {
                // Check if argument is not flag. Then we consider it is file name.
                if (arg[0] != '/')
                {
                    if (settings.InputTopologyFileName == null)
                        settings.InputTopologyFileName = arg;
                    else if (settings.OutputTopologyFileName == null)
                        settings.OutputTopologyFileName = arg;
                    else
                        return null;
                    continue;
                }

                // Each flag must have 2 or more characters length.
                if (arg.Length == 1)
                    return null;

                // Analyze flags.
                switch (arg.Substring(0, 2).ToUpper())
                {
                    case HelpParameter:
                        settings.ShowHelp = true;
                        return settings;
                    case ValidationParameter:
                        settings.ValidationEnabled = true;

                        // Extract file name for output errors log.
                        if (arg.Length > 3)
                            settings.OutputErrorsLogFileName = arg.Substring(3);
                        else
                            return null;

                        break;
                    case ApproximationParameter:
                        settings.ApproximationEnabled = true;

                        // Extract delta X.
                        if (arg.Length > 3)
                        {
                            // Try parse value.
                            double dx;
                            double.TryParse(arg.Substring(3), out dx);

                            if (dx <= 0)
                                return null;

                            settings.DeltaX = dx;
                        }
                        else
                            return null;

                        break;
                    case PartitioningParameter:
                        settings.PartitioningEnabled = true;

                        // Extract number of parts.
                        if (arg.Length > 3)
                        {
                            // Try parse value.
                            int n;
                            int.TryParse(arg.Substring(3), out n);

                            if (n <= 0)
                                return null;

                            settings.PartsNumber = n;
                        }
                        else
                            return null;

                        break;
                    default:
                        return null;
                }
            }

            // Check if input file is specified.
            if (settings.InputTopologyFileName == null)
                return null;

            // Check if output file is specified with /A and /P parameters.
            if ((settings.ApproximationEnabled || settings.PartitioningEnabled) &&
                settings.OutputTopologyFileName == null)
                return null;

            return settings;
        }
    }
}

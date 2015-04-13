using System;
using System.IO;
using System.Linq;
using System.Reflection;
using STA.MineTopology;
using STA.Validation;

namespace STA
{
    /// <summary>
    /// Main class that controls overall execution of STA functions.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Text file with help information.
        /// </summary>
        private const string HelpText = @"STA.Help.txt";

        /// <summary>
        /// Main entry points of application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <exception cref="TopologyValidationException">Topology has errors.</exception>
        private static void Main(string[] args)
        {
            if (!args.Any())
            {
                ShowHelp();
                return;
            }

            var settings = StaSettings.GetStaSettings(args);

            // Analyze arguments.
            if (settings == null)
            {
                Console.WriteLine("Invalid arguments were specified.");
                ShowHelp();
                return;
            }

            // Only need to show help.
            if (settings.ShowHelp)
            {
                ShowHelp();
                return;
            }

            try
            {
                // Read topology.
                var reader = new MineTopologyReader(settings.InputTopologyFileName);
                ConsoleWriteWithTimestamps("Reading topology...");
                var mine = reader.Read();
                ConsoleWriteWithTimestamps("Topology reading is completed.");

                // Validate topology.
                if (settings.ValidationEnabled)
                {
                    var validator = new TopologyValidator(mine);
                    ConsoleWriteWithTimestamps("Validating topology...");
                    var log = validator.Validate();
                    ConsoleWriteWithTimestamps("Topology validation is completed.");
                    if (log.Log.Any())
                        throw new TopologyValidationException("There are errors in topology graph.", log);
                }

                // Executing functions.
                if (settings.ApproximationEnabled || settings.PartitioningEnabled)
                {
                    var processor = new TopologyProcessor(mine);

                    // Approximation.
                    if (settings.ApproximationEnabled)
                    {
                        ConsoleWriteWithTimestamps("Topology approximation...");
                        processor.Approximate(settings.DeltaX);
                        ConsoleWriteWithTimestamps("Topology approximation is completed.");
                    }

                    // Generate adjacency list.
                    ConsoleWriteWithTimestamps("Generating adjacency list for the whole topology...");
                    processor.GenerateAdjacencyList();
                    ConsoleWriteWithTimestamps("Adjacency list generating is completed.");

                    // Partitioning.
                    if (settings.PartitioningEnabled)
                    {
                        ConsoleWriteWithTimestamps("Topology partitioning...");
                        processor.Partition(settings.PartsNumber);
                        ConsoleWriteWithTimestamps("Topology partitioning is completed.");
                    }

                    // Set new type.
                    mine.Type = Mine.SecondaryMineType;

                    // Save results to the output file.
                    ConsoleWriteWithTimestamps("Writing topology to the output file...");
                    XmlWriter.Write(mine, settings.OutputTopologyFileName);
                    ConsoleWriteWithTimestamps("Writing topology to the output file is completed.");
                }
            }
            catch (TopologyValidationException ex)
            {
                // Catch exception of validation.
                Console.WriteLine(ex.Message);

                // Write errors to console.
                foreach (var record in ex.ErrorsLog.Log)
                    Console.WriteLine(record.Severity + ": " + record.Message);

                // Save to log file if validation is enabled.
                if (settings.ValidationEnabled)
                    XmlWriter.Write(ex.ErrorsLog, settings.OutputErrorsLogFileName);
            }
            catch (Exception ex)
            {
                // Catch any exception and show message in console.
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Shows the help text in console.
        /// </summary>
        private static void ShowHelp()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(HelpText))
            {
                if (stream == null)
                {
                    Console.WriteLine("Can't access to help resource.");
                    return;
                }

                using (var reader = new StreamReader(stream))
                {
                    var help = reader.ReadToEnd();
                    Console.WriteLine(help);
                }
            }
        }

        /// <summary>
        /// Writes message with current timestamps to the console.
        /// </summary>
        /// <param name="message">The message to write.</param>
        private static void ConsoleWriteWithTimestamps(string message)
        {
            var text = string.Format("[{0}] {1}", DateTime.Now.TimeOfDay, message);
            Console.WriteLine(text);
        }
    }
}

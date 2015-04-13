using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using STA.MineTopology;
using STA.Validation;

namespace STA
{
    /// <summary>
    /// Reads and verify input file with mine topology data.
    /// </summary>
    internal class MineTopologyReader
    {
        /// <summary>
        /// Schema for input file validation.
        /// </summary>
        private const string InitTopologySchema = @"STA.Schemas.InitTopology.xsd";

        /// <summary>
        /// The input file name with mine topology.
        /// </summary>
        private readonly string _fileName;

        /// <summary>
        /// Log of validation errors found during file parsing.
        /// </summary>
        private ErrorsLog _errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="MineTopologyReader"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public MineTopologyReader(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Reads the mine topology.
        /// </summary>
        /// <returns>The <see cref="Mine"/> object.</returns>
        public Mine Read()
        {
            VerifyFormat();

            // Deserialize mine from XML.
            var serializer = new XmlSerializer(typeof(Mine));
            using (var reader = new StreamReader(_fileName))
            {
                var mine = (Mine)serializer.Deserialize(reader);
                return mine;
            }
        }

        /// <summary>
        /// Retrieves the validation schema from the resources.
        /// </summary>
        /// <returns>Validation schema.</returns>
        private static XmlSchema RetrieveValidationSchema()
        {
            using (var reader = Assembly.GetExecutingAssembly().GetManifestResourceStream(InitTopologySchema))
            {
                return reader != null ? XmlSchema.Read(reader, null) : null;
            }
        }

        /// <summary>
        /// Verifies the format of the input file.
        /// </summary>
        /// <exception cref="TopologyValidationException">There are errors in data format.</exception>
        private void VerifyFormat()
        {
            // Prepare settings.
            var settings = new XmlReaderSettings
                           {
                               ValidationType = ValidationType.Schema,
                               ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings |
                                                 XmlSchemaValidationFlags.ProcessIdentityConstraints
                           };
            settings.ValidationEventHandler += ValidationEventHandler;

            // Set validation schema file.
            settings.Schemas.Add(RetrieveValidationSchema());

            // Parse file.
            using (var reader = XmlReader.Create(_fileName, settings))
            {
                _errors = new ErrorsLog();
                while (reader.Read())
                {
                }
            }

            // Check if were any errors.
            if (_errors.Log.Any())
            {
                throw new TopologyValidationException("There are errors in data format.", _errors);
            }
        }

        /// <summary>
        /// Handler for the validation error event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Xml.Schema.ValidationEventArgs"/> instance
        /// containing the event data.</param>
        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            var record = new ValidationLogRecord { Severity = e.Severity, Message = e.Message };
            _errors.Log.Add(record);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace STA.Validation
{
    /// <summary>
    /// Represents validation errors log.
    /// </summary>
    [Serializable]
    [XmlRoot("Log")]
    public class ErrorsLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorsLog"/> class.
        /// </summary>
        public ErrorsLog()
        {
            Log = new List<ValidationLogRecord>();
        }

        /// <summary>
        /// Gets or sets the errors log.
        /// </summary>
        /// <value>
        /// The errors log.
        /// </value>
        [XmlArray("Errors")]
        [XmlArrayItem("Record")]
        public List<ValidationLogRecord> Log { get; set; }
    }
}

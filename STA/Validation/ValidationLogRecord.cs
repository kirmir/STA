using System;
using System.Xml.Schema;

namespace STA.Validation
{
    /// <summary>
    /// Record of validation log.
    /// </summary>
    [Serializable]
    public class ValidationLogRecord
    {
        /// <summary>
        /// Gets or sets the severity of the violation.
        /// </summary>
        /// <value>
        /// The severity of the violation.
        /// </value>
        public XmlSeverityType Severity { get; set; }

        /// <summary>
        /// Gets or sets the record message.
        /// </summary>
        /// <value>
        /// The record message.
        /// </value>
        public string Message { get; set; }
    }
}

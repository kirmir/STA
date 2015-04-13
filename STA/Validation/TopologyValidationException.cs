using System;

namespace STA.Validation
{
    /// <summary>
    /// Exception during validation of the mine topology.
    /// </summary>
    internal class TopologyValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopologyValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="log">The errors log.</param>
        public TopologyValidationException(string message, ErrorsLog log) : base(message)
        {
            ErrorsLog = log;
        }

        /// <summary>
        /// Gets the log with validation errors.
        /// </summary>
        public ErrorsLog ErrorsLog { get; private set; }
    }
}

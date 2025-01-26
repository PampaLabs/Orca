namespace Orca
{
    /// <summary>
    /// Represents a subject.
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// Gets or sets the unique identifier for the subject.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the subject.
        /// </summary>
        public string Sub { get; set; }

        /// <summary>
        /// Gets or sets the name of the subject.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the subject.
        /// </summary>
        public string Email { get; set; }
    }
}

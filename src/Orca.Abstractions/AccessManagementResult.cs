using System.Globalization;

namespace Orca
{
    /// <summary>
    ///     Represents the result of an access control operation
    /// </summary>
    public class AccessManagementResult
    {
        private static readonly AccessManagementResult _success = new AccessManagementResult { Succeeded = true };
        private readonly List<AccessManagementError> _errors = new List<AccessManagementError>();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="AccessManagementError"/> instances containing errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="AccessManagementError"/> instances.</value>
        public IEnumerable<AccessManagementError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="AccessManagementResult"/> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="AccessManagementResult"/> indicating a successful operation.</returns>
        public static AccessManagementResult Success => _success;

        /// <summary>
        /// Creates an <see cref="AccessManagementResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="AccessManagementError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="AccessManagementResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static AccessManagementResult Failed(params AccessManagementError[] errors)
        {
            var result = new AccessManagementResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        internal static AccessManagementResult Failed(List<AccessManagementError> errors)
        {
            var result = new AccessManagementResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="AccessManagementResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="AccessManagementResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
    }
}

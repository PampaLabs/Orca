using System.Globalization;

namespace Orca
{
    /// <summary>
    ///     Represents the result of an access control operation
    /// </summary>
    public class AccessControlResult
    {
        private static readonly AccessControlResult _success = new AccessControlResult { Succeeded = true };
        private readonly List<AccessControlError> _errors = new List<AccessControlError>();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="AccessControlError"/> instances containing errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="AccessControlError"/> instances.</value>
        public IEnumerable<AccessControlError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="IdentityResult"/> indicating a successful identity operation.
        /// </summary>
        /// <returns>An <see cref="IdentityResult"/> indicating a successful operation.</returns>
        public static AccessControlResult Success => _success;

        /// <summary>
        /// Creates an <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="IdentityError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="IdentityResult"/> indicating a failed identity operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static AccessControlResult Failed(params AccessControlError[] errors)
        {
            var result = new AccessControlResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        internal static AccessControlResult Failed(List<AccessControlError>? errors)
        {
            var result = new AccessControlResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="AccessControlResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="AccessControlResult"/> object.</returns>
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

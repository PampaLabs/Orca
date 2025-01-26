using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;

namespace Orca.Mvc
{
    /// A custom <see cref="TagHelper"/> that checks user roles and permissions to determine whether or not to render HTML content.
    public class OrcaTagHelper : TagHelper
    {
        /// <summary>
        /// The name of the attribute used to specify roles in the tag.
        /// </summary>
        public const string ROLES_NAME_ATTRIBUTE = "roles";

        /// <summary>
        /// The name of the attribute used to specify permissions in the tag.
        /// </summary>
        public const string PERMISSIONS_NAME_ATTRIBUTE = "permissions";

        private static readonly char[] Separator = new[] { ',' };

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAuthorizationGrantor authorizationGrantor;
        private readonly IPermissionEvaluator permissionEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrcaTagHelper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="authorizationGrantor">The authorization grantor to evaluate user roles and permissions.</param>
        /// <param name="permissionEvaluator">The permission evaluator used to evaluate permissions.</param>
        public OrcaTagHelper(
            IHttpContextAccessor httpContextAccessor,
            IAuthorizationGrantor authorizationGrantor,
            IPermissionEvaluator permissionEvaluator)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.authorizationGrantor = authorizationGrantor ?? throw new ArgumentNullException(nameof(authorizationGrantor));
            this.permissionEvaluator = permissionEvaluator ?? throw new ArgumentNullException(nameof(permissionEvaluator));
        }

        /// <summary>
        /// Gets or sets the roles attribute, a comma-separated list of roles to check for the user.
        /// </summary>
        [HtmlAttributeName(ROLES_NAME_ATTRIBUTE)]
        public string Roles { get; set; }

        /// <summary>
        /// Gets or sets the permissions attribute, a comma-separated list of permissions to check for the user.
        /// </summary>
        [HtmlAttributeName(PERMISSIONS_NAME_ATTRIBUTE)]
        public string Permissions { get; set; }

        /// <inheritdoc />
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var authorized = false;

            if (String.IsNullOrWhiteSpace(Roles) 
                && String.IsNullOrWhiteSpace(Permissions))
            {
                return;
            }

            if (!String.IsNullOrWhiteSpace(Roles))
            {
                var roles = new StringTokenizer(Roles, Separator);

                foreach (var item in roles)
                {
                    var role = item.Trim();

                    if (role.HasValue && role.Length > 0)
                    {
                        authorized = httpContextAccessor.HttpContext.User.IsInRole(role.Value);

                        if (authorized)
                        {
                            break;
                        }
                    }
                }
            }

            if (!String.IsNullOrWhiteSpace(Permissions))
            {
                var permissions = new StringTokenizer(Permissions, Separator);

                foreach (var item in permissions)
                {
                    var permission = item.Trim();

                    if (permission.HasValue && permission.Length > 0)
                    {
                        authorized = await permissionEvaluator.HasPermissionAsync(
                            httpContextAccessor.HttpContext.User,
                            permission.Value);

                        if (authorized)
                        {
                            break;
                        }
                    }
                }
            }

            if (!authorized)
            {
                output.SuppressOutput();
            }
        }
    }
}

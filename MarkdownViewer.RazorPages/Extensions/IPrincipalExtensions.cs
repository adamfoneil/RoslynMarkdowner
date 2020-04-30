using System.Linq;
using System.Security.Claims;

namespace MarkdownViewer.App.Extensions
{
    public static class IPrincipalExtensions
    {
        public static string Email(this ClaimsPrincipal principal)
        {
            var dictionary = principal.Claims.ToDictionary(item => item.Type);
            return dictionary["email"].Value;
        }
    }
}

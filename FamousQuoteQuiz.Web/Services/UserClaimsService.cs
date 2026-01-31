using FamousQuoteQuiz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FamousQuoteQuiz.Web.Services
{
    public class UserClaimsService : IUserClaimsService
    {
        private const string AdminRole = "Admin";
        private const string UserRole = "User";

        public ClaimsPrincipal CreatePrincipal(int userId, string userName, bool isAdmin)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, GetUserRole(isAdmin))
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        public string GetUserRole(bool isAdmin)
        {
            return isAdmin ? AdminRole : UserRole;
        }
    }
}

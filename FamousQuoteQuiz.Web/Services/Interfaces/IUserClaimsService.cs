using System.Security.Claims;

namespace FamousQuoteQuiz.Web.Services.Interfaces
{
    /// <summary>
    /// Service for handling user claims and principal creation.
    /// Follows Single Responsibility Principle for authentication claims management.
    /// </summary>
    public interface IUserClaimsService
    {
        /// <summary>
        /// Creates claims principal for an authenticated user.
        /// </summary>
        /// <param name="userId">The user's ID</param>
        /// <param name="userName">The user's display name</param>
        /// <param name="isAdmin">Whether the user is an administrator</param>
        /// <returns>Claims principal for the authenticated user</returns>
        ClaimsPrincipal CreatePrincipal(int userId, string userName, bool isAdmin);

        /// <summary>
        /// Determines the appropriate role for a user.
        /// </summary>
        /// <param name="isAdmin">Whether the user is an administrator</param>
        /// <returns>Role name (Admin or User)</returns>
        string GetUserRole(bool isAdmin);
    }
}

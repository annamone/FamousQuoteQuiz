using System.Security.Claims;

namespace FamousQuoteQuiz.Web.Services.Interfaces
{
    /// <summary>
    /// Service for managing user session and identity information.
    /// Follows Single Responsibility Principle for session management.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the current authenticated user's ID.
        /// </summary>
        /// <returns>User ID if authenticated, null otherwise</returns>
        int? GetUserId();

        /// <summary>
        /// Gets the current authenticated user's name.
        /// </summary>
        /// <returns>User name if authenticated, null otherwise</returns>
        string? GetUserName();

        /// <summary>
        /// Checks if the current user is authenticated.
        /// </summary>
        /// <returns>True if authenticated, false otherwise</returns>
        bool IsAuthenticated();

        /// <summary>
        /// Checks if the current user is an administrator.
        /// </summary>
        /// <returns>True if user is admin, false otherwise</returns>
        bool IsAdmin();
    }
}

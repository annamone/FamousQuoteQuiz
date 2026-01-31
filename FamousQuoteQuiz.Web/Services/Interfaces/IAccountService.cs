using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Web.Models;

namespace FamousQuoteQuiz.Web.Services.Interfaces
{
    /// <summary>
    /// Service for user account operations.
    /// Handles business logic for user authentication and registration.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Authenticates a user with email and password.
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <returns>Authenticated user or null if authentication fails</returns>
        Task<User?> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="model">Registration information</param>
        /// <returns>The newly created user</returns>
        Task<User> RegisterAsync(RegisterViewModel model);

        /// <summary>
        /// Checks if an email address is already registered.
        /// </summary>
        /// <param name="email">Email address to check</param>
        /// <returns>True if email exists, false otherwise</returns>
        Task<bool> EmailExistsAsync(string email);
    }
}

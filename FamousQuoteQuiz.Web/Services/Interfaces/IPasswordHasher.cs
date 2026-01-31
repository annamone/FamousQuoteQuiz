namespace FamousQuoteQuiz.Web.Services.Interfaces
{
    /// <summary>
    /// Service for password hashing and verification.
    /// Follows Single Responsibility Principle by isolating password-related operations.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes a plain text password using BCrypt.
        /// </summary>
        /// <param name="password">The plain text password</param>
        /// <returns>The hashed password</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies a plain text password against a hashed password.
        /// </summary>
        /// <param name="password">The plain text password to verify</param>
        /// <param name="passwordHash">The hashed password to verify against</param>
        /// <returns>True if the password matches, false otherwise</returns>
        bool VerifyPassword(string password, string passwordHash);
    }
}

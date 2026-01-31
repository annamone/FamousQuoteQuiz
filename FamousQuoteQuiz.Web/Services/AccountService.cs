using FamousQuoteQuiz.Domain.Entities;
using FamousQuoteQuiz.Domain.Interfaces.Repositories;
using FamousQuoteQuiz.Web.Models;
using FamousQuoteQuiz.Web.Services.Interfaces;

namespace FamousQuoteQuiz.Web.Services
{
    public class AccountService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IAccountService
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var user = await _userRepository.GetByEmailAsync(email.Trim());

            if (user == null || !user.IsActive)
            {
                return null;
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return null;
            }

            if (!_passwordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public async Task<User> RegisterAsync(RegisterViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var user = new User
            {
                UserName = model.UserName.Trim(),
                Email = model.Email.Trim(),
                IsActive = true,
                IsAdmin = false,
                PasswordHash = _passwordHasher.HashPassword(model.Password)
            };

            return await _userRepository.AddAsync(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var existing = await _userRepository.GetByEmailAsync(email.Trim());
            return existing != null;
        }
    }
}

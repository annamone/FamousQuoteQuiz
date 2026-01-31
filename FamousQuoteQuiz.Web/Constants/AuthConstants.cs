namespace FamousQuoteQuiz.Web.Constants
{
    public static class AuthConstants
    {
        public const string AdminRole = "Admin";
        public const string UserRole = "User";
        
        public const string LoginPath = "/Account/Login";
        public const string LogoutPath = "/Account/Logout";
        public const string RegisterPath = "/Account/Register";
        
        public const int SessionExpirationHours = 8;
    }
}

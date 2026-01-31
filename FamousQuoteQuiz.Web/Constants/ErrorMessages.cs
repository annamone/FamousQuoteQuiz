namespace FamousQuoteQuiz.Web.Constants
{
    public static class ErrorMessages
    {
        // Authentication errors
        public const string InvalidCredentials = "Invalid email or password.";
        public const string EmailAlreadyExists = "An account with this email already exists.";
        public const string AccountNotActive = "This account has been deactivated.";
        
        // Authorization errors
        public const string UnauthorizedAccess = "You are not authorized to access this resource.";
        public const string LoginRequired = "Please log in to continue.";
        
        // Validation errors
        public const string InvalidInput = "The provided information is invalid.";
        public const string RequiredField = "This field is required.";
    }
}

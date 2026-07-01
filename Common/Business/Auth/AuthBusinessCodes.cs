namespace Como.CRM.Api.Common.Business.Auth
{
    public static class AuthBusinessCodes
    {
        public const string InvalidCredentials = "INVALID_CREDENTIALS";

        public const string CurrentPasswordIsWrong = "CURRENT_PASSWORD_IS_WRONG";

        public const string NewPasswordMustBeDifferent = "NEW_PASSWORD_MUST_BE_DIFFERENT";

        public const string Unauthorized = "UNAUTHORIZED";

        public const string UserNotFound = "USER_NOT_FOUND";

        public const string UserSuspended = "USER_SUSPENDED";

        public const string InvalidUserNameOrPassword = "INVALID_USERNAME_OR_PASSWORD";

        public const string PasswordConfirmationDoesNotMatch = "PASSWORD_CONFIRMATION_DOES_NOT_MATCH";
    }
}

namespace Exadel.ReportHub.Handlers;

    public static class Constants
    {
        public static class Validation
        {
            public static class User
            {
                public const string EmailRequiredMessage = "Email is required.";
                public const string InvalidEmailMessage = "Invalid email format.";
                public const string EmailTakenMessage = "Email is taken.";
                public const string FullNameRequiredMessage = "Full name is required.";
                public const string PasswordRequiredMessage = "Password is required.";
                public const int PasswordMinimumLength = 8;
                public const string PasswordMinLengthMessage = "Password must be at least 8 characters long.";
                public const string PasswordUppercaseMessage = "Password must have at least one uppercase letter.";
                public const string PasswordLowercaseMessage = "Password must have at least one lowercase letter.";
                public const string PasswordDigitMessage = "Password must have at least one digit.";
                public const string PasswordSpecialCharacterMessage = "Password must contain at least one special character.";
            }
        }
    }

namespace Exadel.ReportHub.Handlers;

public static class Constants
{
    public static class Validation
    {
        public static class RuleSet
        {
            public const string Names = nameof(Names);
            public const string Passwords = nameof(Passwords);
            public const string Countries = nameof(Countries);
        }

        public static class Common
        {
            public const string EmailIsTaken = "Email is already taken.";
            public const string EmailIsInvalid = "Email is invalid.";
            public const string NameIsTaken = "Name is already taken";
            public const string CustomerDoesNotExist = "Customer does not exist.";
            public const string ClientDoesNotExist = "Client does not exist.";
            public const string ItemDoesNotExist = "Item does not exist.";
            public const string UserDoesNotExist = "User does not exist.";
            public const string CurrencyDoesNotExist = "Currency does not exist.";
            public const string CountryDoesNotExist = "Country does not exist.";
        }

        public static class Name
        {
            public const int MaxLength = 100;
            public const string NameMustStartWithCapital = "Name must begin with a capital letter.";
        }

        public static class Password
        {
            public const int MinimumLength = 8;
            public const string RequireUppercase = "Password must have at least one uppercase letter.";
            public const string RequireLowercase = "Password must have at least one lowercase letter.";
            public const string RequireDigit = "Password must have at least one digit.";
            public const string RequireSpecialCharacter = "Password must contain at least one special character.";
        }

        public static class Country
        {
            public const int MaxLength = 56;
            public const string CountryMustStartWithCapital = "Country must begin with a capital letter.";
        }

        public static class Invoice
        {
            public const int InvoiceNumberMaxLength = 15;
            public const string InvalidInvoiceFormat = "Invoice number must start with 'INV' followed by digits.";
            public const string IssueDateInFuture = "Issue date cannot be in the future.";
            public const string DueDateBeforeIssueDate = "Due date must be greater than issue date.";
            public const string TimeComponentNotAllowed = "Date cannot have a time component.";
            public const int BankAccountNumberMinLength = 8;
            public const int BankAccountNumberMaxLength = 28;
            public const string InvalidBankAccountFormat = "Bank account number must start with two uppercase letters followed by digits.";
            public const string DuplicateInvoice = "Invoice number already exists.";
            public const string DuplicateItem = "Items must not be duplicated.";
        }

        public static class Item
        {
            public const int DescriptionMaxLength = 250;
            public const string DescriptionShouldStartWithCapital = "Description must begin with a capital letter.";
            public const string PriceMustBePositive = "Price must be positive";
            public const string ClientIdImmutable = "Client Id cannot be changed.";
        }

        public static class Import
        {
            public const string InvalidFileExtention = "The file must be in CSV format (.csv extension).";
            public const string EmptyFileUpload = "Uploaded file must not be empty.";
        }

        public static class Plan
        {
            public const string InvalidStartDate = "Start date must be less than end date";
            public const string EndDateInPast = "End date must be in the future";
            public const string AlreadyExistsForItemAndClient = "Plan already exists for this item and client";
        }
    }

    public static class File
    {
        public static class Extension
        {
            public const string Pdf = ".pdf";
        }

        public static class Name
        {
            public const string Invoice = "Invoice_";
        }
    }
}

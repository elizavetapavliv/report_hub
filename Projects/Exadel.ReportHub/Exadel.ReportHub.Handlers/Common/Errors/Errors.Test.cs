using ErrorOr;

namespace Exadel.ReportHub.Handlers.Common.Errors;

public static partial class Errors
{
    public static class Test
    {
        public static Error DoesNotExist(string msg = null) => Error.NotFound(description: msg ?? "Test does not exist.");

        public static Error AlreadyExist(string msg = null) => Error.Conflict(description: msg ?? "Test already exist.");

        public static Error ValidationError(string msg = null) => Error.Validation(description: msg ?? "Test not valid.");
    }
}

using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.User.UpdateNotificationFrequency;
public class UpdateUserNotificationFrequencyRequestValidator : AbstractValidator<UpdateUserNotificationFrequencyRequest>
{
    public UpdateUserNotificationFrequencyRequestValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.UpdateUserNotificationFrequencyDto)
            .SetValidator(new UpdateUserNotificationFrequencyDtoValidator());
    }
}

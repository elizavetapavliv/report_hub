using Exadel.ReportHub.SDK.DTOs.User;
using Exadel.ReportHub.SDK.Enums;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Validators;

public class UpdateUserNotificationFrequencyDtoValidator : AbstractValidator<UpdateUserNotificationFrequencyDTO>
{
    public UpdateUserNotificationFrequencyDtoValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.NotificationTime)
           .InclusiveBetween(0, Constants.Validation.NotificationFrequency.MaxHour)
           .WithMessage(Constants.Validation.NotificationFrequency.TimeHourRange);

        When(x => x.NotificationFrequency == NotificationFrequency.Daily, () =>
        {
            RuleFor(x => x.NotificationDayOfWeek)
                .Null()
                .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
            RuleFor(x => x.NotificationDayOfMonth)
                .Equal(0)
                .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
        });

        When(x => x.NotificationFrequency == NotificationFrequency.Weekly, () =>
        {
            RuleFor(x => x.NotificationDayOfWeek)
                .NotNull()
                .WithMessage(Constants.Validation.NotificationFrequency.ShouldBeSet);
            RuleFor(x => x.NotificationDayOfMonth)
                .Equal(0)
                .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
        });

        When(x => x.NotificationFrequency == NotificationFrequency.Monthly, () =>
        {
            RuleFor(x => x.NotificationDayOfWeek)
                .Null()
                .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
            RuleFor(x => x.NotificationDayOfMonth)
                .InclusiveBetween(1, Constants.Validation.NotificationFrequency.MaxMonthDay)
                .WithMessage(Constants.Validation.NotificationFrequency.MonthDayRange);
        });
    }
}

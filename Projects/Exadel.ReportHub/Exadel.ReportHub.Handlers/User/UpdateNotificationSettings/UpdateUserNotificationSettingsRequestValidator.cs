using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.Enums;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.User.UpdateNotificationFrequency;
public class UpdateUserNotificationSettingsRequestValidator : AbstractValidator<UpdateUserNotificationSettingsRequest>
{
    public UpdateUserNotificationSettingsRequestValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.UpdateUserNotificationFrequencyDto)
            .ChildRules(child =>
            {
                child.ClassLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.Hour)
                   .InclusiveBetween(0, Constants.Validation.NotificationFrequency.MaxHour)
                   .WithMessage(Constants.Validation.NotificationFrequency.TimeHourRange);

                child.When(x => x.Frequency == NotificationFrequency.Daily, () =>
                {
                    child.RuleFor(x => x.DayOfWeek)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
                    child.RuleFor(x => x.DayOfMonth)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
                });

                child.When(x => x.Frequency == NotificationFrequency.Weekly, () =>
                {
                    child.RuleFor(x => x.DayOfWeek)
                        .NotNull()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldBeSet);
                    child.RuleFor(x => x.DayOfMonth)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
                });

                child.When(x => x.Frequency == NotificationFrequency.Monthly, () =>
                {
                    child.RuleFor(x => x.DayOfWeek)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
                    child.RuleFor(x => x.DayOfMonth)
                        .InclusiveBetween(1, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month))
                        .WithMessage(Constants.Validation.NotificationFrequency.MonthDayRange);
                });
            });
    }
}

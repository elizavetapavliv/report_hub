using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.Enums;
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
            .ChildRules(child =>
            {
                child.ClassLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.NotificationHour)
                   .InclusiveBetween(0, Constants.Validation.NotificationFrequency.MaxHour)
                   .WithMessage(Constants.Validation.NotificationFrequency.TimeHourRange);

                child.When(x => x.NotificationFrequency == NotificationFrequency.Daily, () =>
                {
                    child.RuleFor(x => x.NotificationDayOfWeek)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
                    child.RuleFor(x => x.NotificationDayOfMonth)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
                });

                child.When(x => x.NotificationFrequency == NotificationFrequency.Weekly, () =>
                {
                    child.RuleFor(x => x.NotificationDayOfWeek)
                        .NotNull()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldBeSet);
                    child.RuleFor(x => x.NotificationDayOfMonth)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
                });

                child.When(x => x.NotificationFrequency == NotificationFrequency.Monthly, () =>
                {
                    child.RuleFor(x => x.NotificationDayOfWeek)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationFrequency.ShouldNotBeSet);
                    child.RuleFor(x => x.NotificationDayOfMonth)
                        .InclusiveBetween(1, DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month))
                        .WithMessage(Constants.Validation.NotificationFrequency.MonthDayRange);
                });
            });
    }
}

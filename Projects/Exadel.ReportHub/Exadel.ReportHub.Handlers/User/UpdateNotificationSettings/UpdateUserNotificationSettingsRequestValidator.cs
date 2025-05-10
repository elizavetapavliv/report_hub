using Exadel.ReportHub.SDK.Enums;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.User.UpdateNotificationSettings;

public class UpdateUserNotificationSettingsRequestValidator : AbstractValidator<UpdateUserNotificationSettingsRequest>
{
    private const int MaxMonthDay = 31;

    public UpdateUserNotificationSettingsRequestValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.NotificationSettingsDto)
            .ChildRules(child =>
            {
                child.ClassLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.Hour)
                    .InclusiveBetween(0, Constants.Validation.NotificationSettings.MaxHour)
                    .WithMessage(Constants.Validation.NotificationSettings.TimeHourRange);

                child.RuleFor(x => x.ReportPeriod)
                    .IsInEnum();

                child.When(x => x.Frequency == NotificationFrequency.Daily, () =>
                {
                    child.RuleFor(x => x.DayOfWeek)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationSettings.ShouldNotBeSet);

                    child.RuleFor(x => x.DayOfMonth)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationSettings.ShouldNotBeSet);

                    child.RuleFor(x => x.ReportPeriod)
                        .Must(period => period is not(ReportPeriod.LastMonth or ReportPeriod.LastWeek))
                        .WithMessage(Constants.Validation.NotificationSettings.InvalidPeriod);
                });

                child.When(x => x.Frequency == NotificationFrequency.Weekly, () =>
                {
                    child.RuleFor(x => x.DayOfWeek)
                        .NotNull()
                        .WithMessage(Constants.Validation.NotificationSettings.ShouldBeSet)
                        .IsInEnum()
                        .WithMessage(Constants.Validation.NotificationSettings.WeekDayRange);

                    child.RuleFor(x => x.DayOfMonth)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationSettings.ShouldNotBeSet);

                    child.RuleFor(x => x.ReportPeriod)
                        .Must(period => period is not ReportPeriod.LastMonth)
                        .WithMessage(Constants.Validation.NotificationSettings.InvalidPeriod);
                });

                child.When(x => x.Frequency == NotificationFrequency.Monthly, () =>
                {
                    child.RuleFor(x => x.DayOfWeek)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationSettings.ShouldNotBeSet);

                    child.RuleFor(x => x.DayOfMonth)
                        .NotNull()
                        .WithMessage(Constants.Validation.NotificationSettings.ShouldBeSet)
                        .InclusiveBetween(1, MaxMonthDay)
                        .WithMessage(Constants.Validation.NotificationSettings.MonthDayRange);

                    child.RuleFor(x => x.ReportPeriod)
                        .Must(period => period is not(ReportPeriod.LastWeek or ReportPeriod.Week))
                        .WithMessage(Constants.Validation.NotificationSettings.InvalidPeriod);
                });

                child.When(x => x.ReportPeriod is ReportPeriod.CustomPeriod, () =>
                {
                    child.RuleFor(x => x.DaysCount)
                        .NotNull()
                        .WithMessage(Constants.Validation.NotificationSettings.ShouldBeSet)
                        .GreaterThan(0)
                        .WithMessage(Constants.Validation.NotificationSettings.ZeroDaysCount);
                })
                .Otherwise(() =>
                {
                    child.RuleFor(x => x.DaysCount)
                        .Null()
                        .WithMessage(Constants.Validation.NotificationSettings.ShouldNotBeSet);
                });
            });
    }
}

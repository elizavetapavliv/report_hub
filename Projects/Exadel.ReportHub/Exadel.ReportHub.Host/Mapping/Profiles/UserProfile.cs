using AutoMapper;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.SDK.DTOs.User;

namespace Exadel.ReportHub.Host.Mapping.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.NotificationFrequency,
            opt => opt.MapFrom(scr => scr.NotificationSettings.Frequency));
        CreateMap<User, UserProfileDTO>().ForMember(dest => dest.NotificationSettings, opt =>
            opt.MapFrom(src => new NotificationSettingsDTO
            {
                Frequency = (SDK.Enums.NotificationFrequency)src.NotificationSettings.Frequency,
                Hour = src.NotificationSettings.Hour,
                DayOfMonth = src.NotificationSettings.DayOfMonth,
                DayOfWeek = src.NotificationSettings.DayOfWeek,
                ExportFormat = (SDK.Enums.ExportFormat)src.NotificationSettings.ExportFormat,
                ReportStartDate = src.NotificationSettings.ReportStartDate,
                ReportEndDate = src.NotificationSettings.ReportEndDate
            }));
        CreateMap<CreateUserDTO, User>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.PasswordSalt, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore())
            .ForMember(x => x.NotificationSettings, opt => opt.Ignore());
        CreateMap<UpdateUserNotificationSettingsDTO, NotificationSettings>()
            .ForMember(x => x.ReportStartDate, opt => opt.Ignore())
            .ForMember(x => x.ReportEndDate, opt => opt.Ignore());
    }
}

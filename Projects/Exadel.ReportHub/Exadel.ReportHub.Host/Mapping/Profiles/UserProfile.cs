using AutoMapper;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.SDK.DTOs.User;

namespace Exadel.ReportHub.Host.Mapping.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<CreateUserDTO, User>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.PasswordSalt, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore())
            .ForMember(x => x.ReportFormat, opt => opt.Ignore())
            .ForMember(x => x.NotificationDayOfMonth, opt => opt.Ignore())
            .ForMember(x => x.NotificationDayOfWeek, opt => opt.Ignore())
            .ForMember(x => x.NotificationTime, opt => opt.Ignore())
            .ForMember(x => x.NotificationFrequency, opt => opt.Ignore());
        CreateMap<UpdateUserNotificationFrequencyDTO, User>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Email, opt => opt.Ignore())
            .ForMember(x => x.FullName, opt => opt.Ignore())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.PasswordSalt, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore())
            .ForMember(x => x.ReportFormat, opt => opt.Ignore());
    }
}

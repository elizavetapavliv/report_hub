﻿using AutoMapper;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.SDK.DTOs.User;

namespace Exadel.ReportHub.Host.Mapping.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.NotificationFrequency,
            opt => opt.MapFrom(scr => scr.NotificationSettings.NotificationFrequency));
        CreateMap<CreateUserDTO, User>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.PasswordHash, opt => opt.Ignore())
            .ForMember(x => x.PasswordSalt, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore())
            .ForMember(x => x.NotificationSettings, opt => opt.Ignore());
        CreateMap<UpdateUserNotificationSettingsDTO, NotificationSettings>()
            .ForMember(x => x.ReportFormat, opt => opt.Ignore());
    }
}

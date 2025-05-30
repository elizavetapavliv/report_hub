﻿using AutoMapper;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.SDK.DTOs.Client;

namespace Exadel.ReportHub.Host.Mapping.Profiles;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDTO>();
        CreateMap<CreateClientDTO, Client>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.IsDeleted, opt => opt.Ignore())
            .ForMember(x => x.Country, opt => opt.Ignore())
            .ForMember(x => x.CurrencyId, opt => opt.Ignore())
            .ForMember(x => x.CurrencyCode, opt => opt.Ignore());
    }
}

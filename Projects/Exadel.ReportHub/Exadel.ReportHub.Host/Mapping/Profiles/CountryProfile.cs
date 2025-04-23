using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.SDK.DTOs.Country;

namespace Exadel.ReportHub.Host.Mapping.Profiles;

[ExcludeFromCodeCoverage]
public class CountryProfile : Profile
{
    public CountryProfile()
    {
        CreateMap<Country, CountryDTO>();
    }
}

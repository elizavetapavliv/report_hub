using AutoMapper;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.SDK.DTOs.AuditReport;

namespace Exadel.ReportHub.Host.Mapping.Profiles;

public class AuditReportProfile : Profile
{
    public AuditReportProfile()
    {
        CreateMap<AuditReport, AuditReportDTO>();
    }
}

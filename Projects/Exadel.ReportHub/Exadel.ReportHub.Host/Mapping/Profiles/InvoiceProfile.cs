﻿using AutoMapper;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.SDK.DTOs.Invoice;

namespace Exadel.ReportHub.Host.Mapping.Profiles;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<CreateInvoiceDTO, Invoice>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CurrencyId, opt => opt.Ignore())
            .ForMember(x => x.CurrencyCode, opt => opt.Ignore())
            .ForMember(x => x.Amount, opt => opt.Ignore())
            .ForMember(x => x.CurrencyCode, opt => opt.Ignore());
    }
}

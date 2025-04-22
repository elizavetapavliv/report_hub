using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Country.GetAll;
using Exadel.ReportHub.Host.Services.Abstract;
using Exadel.ReportHub.SDK.DTOs.Country;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/countries")]
public class CountriesService(ISender sender) : BaseService
{
    /// <summary>
    /// Retrieves the list of all countries.
    /// </summary>
    /// <returns>A list of countries.</returns>
    /// <response code="200">Returns the list of all available countries.</response>
    /// <response code="401">If the user is unauthorized.</response>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IList<CountryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IList<CountryDTO>>> GetAllCountries()
    {
        var result = await sender.Send(new GetAllCountriesRequest());

        return FromResult(result);
    }
}

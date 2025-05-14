using Exadel.ReportHub.SDK.DTOs.Client;

namespace Exadel.ReportHub.Handlers.Managers.Client;

public interface IClientManager
{
    Task<ClientDTO> GenerateClientAsync(CreateClientDTO createClientDto, CancellationToken cancellationToken);

    Task<IList<ClientDTO>> GenerateClientsAsync(IEnumerable<CreateClientDTO> createClientDtos, CancellationToken cancellationToken);
}

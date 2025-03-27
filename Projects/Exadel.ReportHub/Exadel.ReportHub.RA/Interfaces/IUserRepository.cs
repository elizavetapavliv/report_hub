using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.RA.Abstract;

namespace Exadel.ReportHub.RA.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllActiveAsync(CancellationToken cancellationToken);

    Task AddUserAsync(User user, CancellationToken cancellationToken);

    Task UpdateActivityAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> IsActiveByIdAsync(Guid id, CancellationToken cancellationToken);
}

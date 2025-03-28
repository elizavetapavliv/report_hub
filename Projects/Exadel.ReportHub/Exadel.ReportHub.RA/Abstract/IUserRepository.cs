using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.RA.Abstract;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);

    Task<IEnumerable<User>> GetAllActiveAsync(CancellationToken cancellationToken);

    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

    Task AddUserAsync(User user, CancellationToken cancellationToken);

    Task UpdateActivityAsync(Guid id, bool isActive, CancellationToken cancellationToken);

    Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken);
}

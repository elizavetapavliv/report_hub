﻿using System;
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

    Task UpdateActivityAsync(Guid id, bool isActive, CancellationToken cancellationToken);

    Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken);
}

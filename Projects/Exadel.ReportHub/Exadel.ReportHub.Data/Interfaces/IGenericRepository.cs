using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.Data.Interfaces
{
    public interface IGenericRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(string id);

        Task AddAsync(T entity);

        Task UpdateAsync(string id, T entity);

        Task DeleteAsync(string id);
    }
}

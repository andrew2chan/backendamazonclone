using BackendAmazonClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> Get();
        Task<Users> Get(long id);
        Task<Users> Create(Users user);
        Task Update(Users user, long id, bool passwordNeedsUpdating);
        Task Delete(long id);
        Task ClearAll();
        IQueryable SearchSpecificColumn(string columnName);
        IQueryable Authenticate(string email);
    }
}

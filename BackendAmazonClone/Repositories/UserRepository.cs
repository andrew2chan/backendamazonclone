using BackendAmazonClone.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendAmazonClone.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Users> Create(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task Delete(long id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            _context.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Users>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Users> Get(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task Update(Users user, long searchId, bool passwordNeedsUpdating)
        {
            //_context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            var userToBeFound = _context.Users.FirstOrDefault(u => u.Id == searchId);

            userToBeFound.Name = user.Name;
            userToBeFound.Address = user.Address;

            if (passwordNeedsUpdating)
            {
                userToBeFound.Salt = user.Salt;
                userToBeFound.HashedPassword = user.HashedPassword;
            }

            _context.Users.Update(userToBeFound);

            await _context.SaveChangesAsync();
        }

        public async Task ClearAll()
        {
            foreach (Users user in _context.Users)
            {
                _context.Users.Remove(user);
            }
            await _context.SaveChangesAsync();
        }

        public IQueryable SearchSpecificColumn(string columnName)
        {
            string lowerCaseColumnName = columnName.ToLower();

            switch (lowerCaseColumnName)
            {
                case "name":
                    return _context.Users.Select(s => new { id = s.Id, field = s.Name });
                case "email":
                    return _context.Users.Select(s => new{ id = s.Id, field = s.Email });
                case "password":
                    return _context.Users.Select(s => new { id = s.Id, field = s.Password });
            }

            return null;
        }

        public IQueryable Authenticate(string email)
        {
            return _context.Users.Where(s => s.Email == email).Select(s => new { id = s.Id, email = s.Email, salt = s.Salt, hashPassword = s.HashedPassword });
        }
    }
}

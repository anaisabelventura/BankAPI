using BankAPI.Data.DBInterfaces;
using BankAPI.Core.Models;
using BankAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data;
using System.Data.Common;

namespace BankAPI.Data
{
    public class UserDB : BaseDB<User>, IUserDB
    {
        private readonly IDbTransaction _dbTransaction;
        public UserDB(DbContextOptions<postgresContext> options) : base(options)
        {
        }
        
        public async override Task<User> Create(User user)
        {
            var query = "INSERT INTO users (username, password, full_name, email)"
             + " VALUES(@username,  @password,  @full_name, @email) RETURNING id";
            var parameters = new DynamicParameters();
            parameters.Add("username", user.UserName);
            parameters.Add("password", user.Password);
            parameters.Add("full_name", user.FullName);
            parameters.Add("email", user.Email);

            await _db.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
        public async Task<User> GetByUsername(string userName)
        {
            var query = "SELECT * FROM users WHERE username=@username";
            var parameters = new DynamicParameters();
           
            return await _db.User.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<User> GetById(int userId)
        {
            return await _db.User.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}

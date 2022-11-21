using BankAPI.Core.Models;
using Microsoft.EntityFrameworkCore;
using BankAPI.Data.DBInterfaces;
using BankAPI.Infrastructure.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace BankAPI.Data
{
    public class BaseDB<T> : IBaseDB<T> 
    {
        protected postgresContext _db;

        public BaseDB(DbContextOptions<postgresContext> options)
        {
            _db = new postgresContext(options);
            
        }
        public async virtual Task<T> Create(T entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
       
        public async Task<T> Update(T entityToUpdate)
        {
            _db.Update(entityToUpdate);
            await _db.SaveChangesAsync();
            return entityToUpdate;
        }
    }
}
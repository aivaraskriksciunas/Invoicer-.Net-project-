using Invoicer.Core.Data.Models;
using System;

namespace Invoicer.Core.Data.Repositories;

public interface IRepository <T> where T : IEntity
{
    public Task<T?> FindByIdAsync( int id );

    public Task<bool> ExistsAsync( int id );

    public Task<IEnumerable<T>> FindAllAsync();

    public Task CreateAsync( T entity );

    public Task<bool> UpdateAsync( T entity );

    public Task<bool> DeleteAsync( int id );

    
}

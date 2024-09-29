using System;

namespace Invoicer.Data.Repositories;

public interface IRepository <T> where T : class
{
    public Task<T> FindByIdAsync( int id );
    public Task<IEnumerable<T>> FindAllAsync();

    
}

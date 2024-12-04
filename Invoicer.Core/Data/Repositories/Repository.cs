using Invoicer.Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Invoicer.Core.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly InvoicerDbContext _db;

    public IQueryable<T> Query { 
        get
        {
            return _db.Set<T>().AsQueryable<T>();
        }
    }

    public Repository( InvoicerDbContext dbContext )
    {
        _db = dbContext;    
    }

    public async Task CreateAsync( T entity )
    {
        ProcessProperties( entity );
        await _db.AddAsync( entity );
    }

    public async Task<bool> DeleteAsync( int id )
    {
        var entity = await _db.Set<T>().FindAsync( id );
        if ( entity != null )
        {
            _db.Remove( entity );
            return true;
        }

        return false;
    }

    public async Task<bool> ExistsAsync( int id )
    {
        var entity = await FindByIdAsync( id );
        return entity != null;
    }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task<T?> FindByIdAsync( int id )
    {
        return await _db.FindAsync<T>( id );
    }

    public async Task<bool> UpdateAsync( T entity )
    {
        ProcessProperties( entity );
        var currentEntity = await _db.Set<T>().FindAsync( entity.Id );
        if ( currentEntity == null )
        {
            return false;
        }

        _db.Entry( currentEntity ).CurrentValues.SetValues( entity );
        _db.Entry( currentEntity ).State = EntityState.Modified;

        return true;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }

    private void ProcessProperties( T entity )
    {
        Type entityType = entity.GetType();
        foreach ( var prop in entityType.GetProperties() )
        {
            if ( !prop.CanWrite || !prop.CanRead ) continue;

            var value = prop.GetValue( entity );
            if ( value == null ) continue;

            if ( prop.PropertyType == typeof( DateTime ) )
            {
                var datetime = (DateTime)value;
                prop.SetValue(
                    entity,
                    datetime.ToUniversalTime() );
            }
        }
    }

}

using Invoicer.Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Invoicer.Core.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class, IEntity
{
    public InvoicerDbContext Db { get; private set; }

    public IQueryable<T> Query { 
        get
        {
            return Db.Set<T>().AsQueryable<T>();
        }
    }

    public Repository( InvoicerDbContext dbContext )
    {
        Db = dbContext;    
    }

    public async Task CreateAsync( T entity )
    {
        ProcessProperties( entity );
        await Db.AddAsync( entity );
    }

    public async Task<bool> DeleteAsync( int id )
    {
        var entity = await Db.Set<T>().FindAsync( id );
        if ( entity != null )
        {
            Db.Remove( entity );
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
        return await Db.Set<T>().ToListAsync();
    }

    public async Task<T?> FindByIdAsync( int id )
    {
        return await Db.FindAsync<T>( id );
    }

    public async Task<bool> UpdateAsync( T entity )
    {
        ProcessProperties( entity );
        var currentEntity = await Db.Set<T>().FindAsync( entity.Id );
        if ( currentEntity == null )
        {
            return false;
        }

        Db.Entry( currentEntity ).CurrentValues.SetValues( entity );
        Db.Entry( currentEntity ).State = EntityState.Modified;

        return true;
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

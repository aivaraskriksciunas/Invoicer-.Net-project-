using System;
using System.Linq.Expressions;
using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Caching.Memory;

namespace Invoicer.Core.Data;

public class InvoicerDbContext : IdentityDbContext<User>
{
    public InvoicerDbContext( DbContextOptions<InvoicerDbContext> options ) : base( options )
    {}

    public DbSet<BillableRecord> BillableRecords { get; set; }

    public DbSet<Client> Clients { get; set; }

    public DbSet<BillableUnit> BillableUnits { get; set; }


    protected override void OnModelCreating( ModelBuilder builder )
    {
        base.OnModelCreating( builder );

        InitialDataSeeder.SeedData( builder );
    }

    public override Task<int> SaveChangesAsync( bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default )
    {
        foreach ( var entry in ChangeTracker.Entries<IEntity>() )
        {
            if ( entry.State == EntityState.Added )
            {
                entry.Entity.Id = Ulid.NewUlid().ToString().ToLower();
            }
        }

        return base.SaveChangesAsync( acceptAllChangesOnSuccess, cancellationToken );
    }

    public override Task<int> SaveChangesAsync( CancellationToken cancellationToken = default )
    {
        return SaveChangesAsync( true, cancellationToken );
    }
}

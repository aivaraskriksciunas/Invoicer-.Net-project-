using System;
using Invoicer.Core.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
}

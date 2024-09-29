using System;
using Invoicer.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Data;

public class InvoicerDbContext : IdentityDbContext<User>
{
    public InvoicerDbContext( DbContextOptions<InvoicerDbContext> options ) : base( options )
    {}

    public DbSet<BillableRecord> BillableRecords { get; set; }

    public DbSet<Client> Clients { get; set; }

    public DbSet<BillableUnit> BillableUnits { get; set; }
}

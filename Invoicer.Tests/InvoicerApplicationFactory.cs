using Invoicer.Core.Data;
using Invoicer.Core.Data.Models;
using Invoicer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Respawn;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace Invoicer.Tests;

public class InvoicerApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage( "postgres:alpine" )
        .Build();

    private DbConnection _dbConnection = null!;

    public InvoicerDbContext Db { get; private set; } = null!;

    public UserManager<User> UserManager { get; private set; } = null!;

    private Respawner _respawner = null!;

    private IServiceScope _serviceScope = null!;
    public IServiceProvider ServiceProvider { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        _serviceScope = Services.CreateScope();
        ServiceProvider = _serviceScope.ServiceProvider;
        Db = ServiceProvider.GetRequiredService<InvoicerDbContext>();
        _dbConnection = Db.Database.GetDbConnection();
        await _dbConnection.OpenAsync();

        UserManager = ServiceProvider.GetRequiredService<UserManager<User>>();

        _respawner = await Respawner.CreateAsync( _dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
        } );
    }

    public async Task DisposeAsync()
    {
        _serviceScope.Dispose();
        await base.DisposeAsync();
        await _postgres.DisposeAsync().AsTask();
    }

    public async Task ResetDatabase()
    {
        await _respawner.ResetAsync( _dbConnection );
    }

    protected override void ConfigureWebHost( IWebHostBuilder builder )
    {
        builder.ConfigureServices( services =>
        {
            services.RemoveAll<DbContextOptions<InvoicerDbContext> >();
            services.RemoveAll<DbConnection>();

            services.AddDbContext<InvoicerDbContext>(
                options => options.UseNpgsql( _postgres.GetConnectionString() )
            );

            // Ensure tables are created
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<InvoicerDbContext>().Database.EnsureCreated();
            }

            // Override database seeder
            services.RemoveAll<IDatabaseSeeder>();
            services.AddTransient<IDatabaseSeeder, TestDatabaseSeeder>();
        } );

        builder.UseEnvironment( "Development" );
    }
}

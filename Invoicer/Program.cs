using Invoicer.Data;
using Microsoft.EntityFrameworkCore;
using Invoicer.Services;

var builder = WebApplication.CreateBuilder(args);

if ( builder.Environment.IsProduction() ) {
    builder.Services.AddDbContext<InvoicerDbContext>( 
        options => options.UseSqlServer( 
            builder.Configuration.GetConnectionString( "InvoicerDb" ),
            options => options.EnableRetryOnFailure() 
        )
    );
}
else {
    builder.Services.AddDbContext<InvoicerDbContext>( 
        options => options.UseSqlite( 
            builder.Configuration.GetConnectionString( "InvoicerDbSqlite" )
        )
    );
}


builder.Services.AddInvoicerIdentity();

builder.Services.AddRepositories();

var app = builder.Build();

await app.AddAdminUserAsync();

app.UseStaticFiles();

app.MapRazorPages();

// Map controllers with areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

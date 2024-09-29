using Invoicer.Data.Models;
using Invoicer.Data.Repositories;

namespace Invoicer.Data;

public static class RegisterRepositoriesService
{
    public static void AddRepositories( this IServiceCollection services )
    {
        services.AddScoped<IRepository<BillableUnit>, Repository<BillableUnit>>();
    }
}

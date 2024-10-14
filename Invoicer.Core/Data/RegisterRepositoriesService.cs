using Invoicer.Core.Data.Models;
using Invoicer.Core.Data.Repositories;

namespace Invoicer.Core.Data;

public static class RegisterRepositoriesService
{
    public static void AddRepositories( this IServiceCollection services )
    {
        services.AddScoped<IRepository<BillableUnit>, Repository<BillableUnit>>();
    }
}

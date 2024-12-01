using Invoicer.Api.Features.BillableRecordFeature;
using Invoicer.Api.Features.ClientsFeature;

namespace Invoicer.Api;

public static class RegisterApiServices
{
    public static void AddApiServices( this IServiceCollection services )
    {
        services.AddTransient<ClientService>();
        services.AddTransient<BillableRecordService>();
    }
}

﻿using Invoicer.Api.Features.AccountsFeature.Services;
using Invoicer.Api.Features.BillableRecordFeature;
using Invoicer.Api.Features.BillableUnitFeature;
using Invoicer.Api.Features.ClientsFeature;

namespace Invoicer.Api;

public static class RegisterApiServices
{
    public static void AddApiServices( this IServiceCollection services )
    {
        services.AddTransient<ClientService>();
        services.AddTransient<BillableRecordService>();
        services.AddTransient<BillableUnitService>();
        services.AddTransient<ConfirmationEmailSender>();
        services.AddTransient<NewAccountInitializer>();
    }
}

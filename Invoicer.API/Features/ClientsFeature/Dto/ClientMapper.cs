using Invoicer.Core.Data.Models;
using Riok.Mapperly.Abstractions;
using System.Runtime.CompilerServices;

namespace Invoicer.Api.Features.ClientsFeature.Dto;

[Mapper]
public static partial class ClientMapper
{
    public static partial IQueryable<ClientDto> ToDto( this IQueryable<Client> source );

    public static partial Client ToModel( this CreateClient source );
}

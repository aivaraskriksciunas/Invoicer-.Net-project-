using Invoicer.Api.Features.AccountsFeature.Dto;
using Invoicer.Core.Data.Models;
using Riok.Mapperly.Abstractions;

namespace Invoicer.Api.Features.AccountsFeature;

[Mapper]
public static partial class AccountsMapper
{
    public static partial UserDto ToUserDto( this User user );
}

using Invoicer.Api.Features.AccountsFeature.Dto;
using Invoicer.Core.Data.Models;
using Riok.Mapperly.Abstractions;

namespace Invoicer.Api.Features.AccountsFeature;

[Mapper]
public static partial class AccountsMapper
{
    public static AccountDto ToAccountDto( this User user )
    {
        var dto = new AccountDto();
        dto.Email = user.Email ?? "";
        dto.Name = user.GetFullName();
        dto.EmailConfirmed = user.EmailConfirmed;
        return dto;
    }
}

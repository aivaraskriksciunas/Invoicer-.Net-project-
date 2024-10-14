using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Api.Features;

[Route( "Api/[controller]" )]
[ApiController]
[Authorize]
public abstract class BaseApiController : ControllerBase
{
}

using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Api;

[Route( "Api/[controller]" )]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
}

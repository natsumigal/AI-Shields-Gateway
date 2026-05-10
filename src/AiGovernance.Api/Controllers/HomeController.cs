using Microsoft.AspNetCore.Mvc;

namespace AiGovernance.Api.Controllers;

[ApiController]
public sealed class HomeController : ControllerBase
{
    [HttpGet("/")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Index()
    {
        return Redirect("/swagger");
    }
}

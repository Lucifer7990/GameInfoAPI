using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GameDetailsController : ControllerBase
{
    private static readonly string[] Games =
    [
        "Roblox", "Minecraft","Grand Theft Auto V","Mario"
    ];

    [HttpGet]
    public IEnumerable<string> Get()
    {
        return Games;
    }
}

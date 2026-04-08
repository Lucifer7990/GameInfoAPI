using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GameDetailsController(AppDbContext dbContext) : ControllerBase
{
    private static readonly string[] Games =
    [
        "Roblox", "Minecraft","Grand Theft Auto V","Mario"
    ];

    [HttpGet]
    public async Task<IEnumerable<GameDetail>> Get()
    {
        var result = await dbContext.GameDetails.ToListAsync();
        return result;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedGames()
    {
        var games = new List<GameDetail>
        {
            new GameDetail
            {
                Title = "The Witcher 3",
                Description = "Open world RPG",
                CoverImageUrl = "https://example.com/witcher3.jpg"
            },
            new GameDetail
            {
                Title = "Cyberpunk 2077",
                Description = "Futuristic open world game",
                CoverImageUrl = "https://example.com/cyberpunk.jpg"
            },
            new GameDetail
            {
                Title = "Red Dead Redemption 2",
                Description = "Wild west story-driven game",
                CoverImageUrl = "https://example.com/rdr2.jpg"
            }
        };

        await dbContext.GameDetails.AddRangeAsync(games);
        await dbContext.SaveChangesAsync();

        return Ok(new { message = "Dummy data inserted", count = games.Count });
    }
}

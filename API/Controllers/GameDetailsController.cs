using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GameDetailsController(AppDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<GameDetail>> Get()
    {
        var result = await dbContext.GameDetails.ToListAsync();
        return result;
    }

    [HttpGet("seed")]
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


    [HttpPost]
    public async Task<ActionResult<GameDetail>> Create(GameDetailDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var gameDetail = new GameDetail
        {
            Title = dto.Title,
            Description = dto.Description,
            CoverImageUrl = dto.CoverImageUrl
        };
        dbContext.GameDetails.Add(gameDetail);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = gameDetail.Id }, gameDetail);

    }


    [HttpGet("id:int")]
    public async Task<ActionResult<GameDetail>> GetById(int id)
    {
        var gameDetail = await dbContext.GameDetails.FindAsync(id);
        if (gameDetail == null)
            return NotFound();

        return Ok(gameDetail);
    }

}

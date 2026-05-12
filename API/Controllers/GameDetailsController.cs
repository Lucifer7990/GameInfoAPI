using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameDetailsController(AppDbContext dbContext) : ControllerBase
{

    [HttpGet("userinfo")]
    [Authorize(Roles = "User")]
    public async Task<object> GetUserId()
    {
        var result = new {  nameid = User.FindFirstValue(ClaimTypes.NameIdentifier), 
                            name = User.FindFirstValue(ClaimTypes.Name),
                            email =  User.FindFirstValue(ClaimTypes.Email),
                            Role =  User.FindFirstValue(ClaimTypes.Role),
                            temp =  User.FindFirstValue(ClaimTypes.GivenName)
                         };
        return result;
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IEnumerable<GameDetail>> Get()
    {
        var result = await dbContext.GameDetails.ToListAsync();
        return result;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameDetail>> GetById(int id)
    {
        var gameDetail = await dbContext.GameDetails.FindAsync(id);
        if (gameDetail == null)
            return NotFound();

        return Ok(gameDetail);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GameDetail>> Create(GameDetailDTO dto)
    {
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


    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GameDetail>> Update(int id, GameDetailDTO dto)
    {
        var gameDetail = await dbContext.GameDetails.FindAsync(id);
        if (gameDetail == null)
            return NotFound();
        else
        {

            gameDetail.Title = dto.Title;
            gameDetail.Description = dto.Description;
            gameDetail.CoverImageUrl = dto.CoverImageUrl;


            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> DeleteGame(int id)
    {
        var gameDetail = await dbContext.GameDetails.FindAsync(id);
        if (gameDetail == null)
            return NotFound();

        dbContext.GameDetails.Remove(gameDetail);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}

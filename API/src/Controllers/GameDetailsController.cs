using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameDetailsController(IGameService gameService) : ControllerBase
{

    [HttpGet]
    public async Task<IEnumerable<GameDetail>> Get()
    {
        return await gameService.getGameDetails();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameDetail>> GetById(int id)
    {
        var game = await gameService.getGameDetailsById(id);
        if (game is null) return NotFound();

        return Ok(game);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GameDetail>> Create(GameDetailDTO dto)
    {
        if (await gameService.CreateGame(dto))
        {
            return Created();
        }
        return Problem("Data not created");
    }


    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<GameDetail>> Update(int id, GameDetailDTO dto)
    {
        if (await gameService.UpdateGame(id,dto))
        {
            return Ok();
        }
        return Problem("Data not updated");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        if (await gameService.DeleteGame(id))
        {
            return Ok();
        }
        return Problem("No data deleted");
    }
}

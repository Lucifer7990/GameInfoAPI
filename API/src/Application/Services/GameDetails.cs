using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class GameService(AppDbContext dbContext, IMapper mapper) : IGameService
{
    public async Task<bool> CreateGame(GameDetailDTO gameDto)
    {
        await dbContext.GameDetails.AddAsync(mapper.Map<GameDetail>(gameDto));

        var rowsAffected = await dbContext.SaveChangesAsync();

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteGame(int id)
    {
        var gameDetail = await dbContext.GameDetails.FindAsync(id);
        if (gameDetail == null)
            return false;

        dbContext.GameDetails.Remove(gameDetail);
        var run = await dbContext.SaveChangesAsync();
        return run>0;
    }

    public async Task<IEnumerable<GameDetail>> getGameDetails()
    {
        return await dbContext.GameDetails.ToListAsync();
    }

    public async Task<GameDetail?> getGameDetailsById(int id)
    {
        return await dbContext.GameDetails.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<bool> UpdateGame(int id, GameDetailDTO gameDetail)
    {
        var game = await dbContext.GameDetails.FindAsync(id);
        if (gameDetail == null)
            return false;
        else
        {
            mapper.Map(gameDetail, game);

            var run = await dbContext.SaveChangesAsync();

            return run > 0;
        }
    }
}
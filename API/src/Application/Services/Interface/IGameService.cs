namespace Application.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameDetail>> getGameDetails();
        Task<GameDetail?> getGameDetailsById(int id);
        Task<bool> CreateGame(GameDetailDTO gameDto);

        Task<bool> UpdateGame(int id, GameDetailDTO gameDetail);

        Task<bool> DeleteGame(int id);

    }
}
using TicTacToe.Models;

namespace TicTacToe.Repositories.Interfaces;

public interface IGameRepository
{
	Task AddAsync(Game game);
	Task<List<Game>> GetUserGamesAsync(string username);
}
using Microsoft.EntityFrameworkCore;
using TicTacToe.Database;
using TicTacToe.Models;
using TicTacToe.Repositories.Interfaces;

namespace TicTacToe.Repositories;

public class GameRepository(ApplicationDbContext context) : IGameRepository
{
	public async Task AddAsync(Game game)
	{
		try
		{
			await context.Games.AddAsync(game);
			await context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Adding game failed: {ex.Message}");
		}
	}

	public async Task<List<Game>> GetUserGamesAsync(string username)
	{
		return await context.Games
			.Where(g => g.Player1 == username || g.Player2 == username)
			.OrderByDescending(g => g.GameDate)
			.ToListAsync();
	}
}
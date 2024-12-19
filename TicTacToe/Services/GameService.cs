using TicTacToe.Exceptions;
using TicTacToe.Models;
using TicTacToe.Repositories;
using TicTacToe.Repositories.Interfaces;

namespace TicTacToe.Services;

public class GameService(IGameRepository gameRepository)
{
	public async Task<Game> CreateGameAsync(string player1, string player2)
	{
		var game = new Game(player1, player2);
		await gameRepository.AddAsync(game);
		return game;
	}

	public async Task<List<Game>> GetUserHistoryAsync(string username)
	{
		return await gameRepository.GetUserGamesAsync(username);
	}

	public static void MakeMove(Game game, int row, int col, char symbol)
	{
		GameLogicService.MakeMove(game, row, col, symbol);
		game.AddMove(row, col, symbol);

		try
		{
			GameLogicService.CheckWin(game, symbol);
		}
		catch (WinException)
		{
			game.Winner = symbol == 'X' ? game.Player1 : game.Player2;
			throw;
		}

		GameLogicService.ValidateBoardState(game);
	}

	public static (int row, int col) ConvertPositionToCoordinates(int position)
	{
		if (position is < 0 or > 8)
		{
			throw new ArgumentException("Position must be between 0 and 8");
		}

		return (position / 3, position % 3);
	}
}
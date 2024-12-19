using TicTacToe.Exceptions;
using TicTacToe.Models;
using TicTacToe.Services;
using TicTacToe.UI;

namespace TicTacToe.Commands.Implementations;

public class PlayVsPlayerCommand(ConsoleUi ui, GameService gameService, UserService userService) : ICommand
{
	public string Name => "Play vs Player";

	public void Execute()
	{
		PlayGameAsync().GetAwaiter().GetResult();
	}

	private async Task PlayGameAsync()
	{
		var currentUser = ui.CurrentUser;
		if (currentUser == null)
		{
			return;
		}

		var secondPlayer = await GetSecondPlayer();
		if (secondPlayer == null)
		{
			return;
		}

		var game = await gameService.CreateGameAsync(currentUser.Username, secondPlayer.Username);
		var currentPlayer = 'X';

		while (true)
		{
			Console.Clear();
			ConsoleUi.DisplayBoard(game);
			Console.WriteLine(
				$"Current player: {(currentPlayer == 'X' ? game.Player1 : game.Player2)} ({currentPlayer})");

			Console.WriteLine("Enter position (0-8): ");
			if (!int.TryParse(Console.ReadLine(), out var position) || position < 0 || position > 8)
			{
				ConsoleUi.ShowError("Invalid input. Please enter a number between 0 and 8.");
				continue;
			}

			try
			{
				var (row, col) = GameService.ConvertPositionToCoordinates(position);
				GameService.MakeMove(game, row, col, currentPlayer);
				currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
			}
			catch (WinException we)
			{
				var winner = we.WinningSymbol == 'X' ? game.Player1 : game.Player2;
				game.Winner = winner;
				await userService.UpdateRatingAsync(game.Player1, winner == game.Player1);
				await userService.UpdateRatingAsync(game.Player2, winner == game.Player2);

				ConsoleUi.DisplayBoard(game);
				Console.WriteLine($"{game.Winner} wins!");
				Thread.Sleep(2000);
				break;
			}
			catch (GameDrawException)
			{
				ConsoleUi.DisplayBoard(game);
				Console.WriteLine("It's a draw!");
				Thread.Sleep(2000);
				break;
			}
			catch (Exception ex)
			{
				ConsoleUi.ShowError($"Invalid move: {ex.Message}");
			}
		}
	}

	private async Task<User?> GetSecondPlayer()
	{
		var currentUser = ui.CurrentUser;

		while (true)
		{
			Console.WriteLine("Enter second player's name (or 'cancel' to return): ");
			var username = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(username) ||
			    username.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
			{
				return null;
			}

			if (username == currentUser?.Username)
			{
				ConsoleUi.ShowError("You cannot play against yourself!");
				continue;
			}

			try
			{
				var user = await userService.GetUserByNameAsync(username);
				if (user == null)
				{
					ConsoleUi.ShowError("User doesn't exist!");
					continue;
				}

				var password = ConsoleUi.GetSecureInput("Enter second player's password: ", true);

				try
				{
					return await userService.LoginAsync(username, password);
				}
				catch (InvalidOperationException)
				{
					ConsoleUi.ShowError("Invalid password!");
				}
			}
			catch (Exception ex)
			{
				ConsoleUi.ShowError($"An error occurred: {ex.Message}");
			}
		}
	}
}
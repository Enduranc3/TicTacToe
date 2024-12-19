using System.Diagnostics;
using TicTacToe.Models;
using TicTacToe.Services;
using TicTacToe.UI;

namespace TicTacToe.Commands.Implementations;

public class WatchReplayCommand(ConsoleUi ui, GameService gameService) : ICommand
{
    public string Name => "Watch Game Replay";

    public void Execute()
    {
        Console.Clear();

        var currentUser = ui.CurrentUser;

        if (currentUser == null)
        {
            return;
        }

        var history = gameService.GetUserHistoryAsync(currentUser.Username).Result;

        if (history.Count == 0)
        {
            Console.WriteLine("No games to replay!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Select a game to replay:");
        for (var i = 0; i < history.Count; i++)
        {
            var game = history[i];
            Console.WriteLine(
                $"{i + 1}. {game.GameDate.ToLocalTime()}: {game.Player1} vs {game.Player2} - Winner: {(game.Winner.Length == 0 ? "Draw" : game.Winner)}");
        }

        if (!int.TryParse(Console.ReadLine(), out var choice) || choice < 1 || choice > history.Count)
        {
            ConsoleUi.ShowError("Invalid selection");
            return;
        }

        var selectedGame = history[choice - 1];
        ReplayGame(selectedGame);
    }

    private static void ReplayGame(Game game)
    {
        var replayBoard = new char[3, 3];
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                replayBoard[i, j] = ' ';
            }
        }

        foreach (var move in game.Moves)
        {
            Console.Clear();
            Console.WriteLine($"Move {move.MoveNumber} by {(move.Symbol == 'X' ? game.Player1 : game.Player2)}");
            replayBoard[move.Row, move.Col] = move.Symbol;
            DisplayReplayBoard(replayBoard);
            Thread.Sleep(2000);
        }

        Console.WriteLine($"\nGame ended. Winner: {(game.Winner.Length == 0 ? "Draw" : game.Winner)}");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void DisplayReplayBoard(char[,] board)
    {
        for (var i = 0; i < 3; i++)
        {
            Console.Write(" ");
            for (var j = 0; j < 3; j++)
            {
                var cellValue = board[i, j];
                var displayValue = cellValue == ' ' ? (i * 3 + j).ToString() : cellValue.ToString();

                Console.ForegroundColor = cellValue switch
                {
                    'X' => ConsoleColor.Blue,
                    'O' => ConsoleColor.Green,
                    _ => ConsoleColor.Gray
                };

                Console.Write(displayValue);
                Console.ResetColor();

                if (j < 2)
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine();
            if (i < 2)
            {
                Console.WriteLine("---+---+---");
            }
        }
    }
}

using TicTacToe.Services;
using TicTacToe.UI;

namespace TicTacToe.Commands.Implementations;

public class ViewHistoryCommand(ConsoleUi ui, GameService gameService) : ICommand
{
    public string Name => "View History";

    public void Execute()
    {
        var currentUser = ui.CurrentUser;
        Console.Clear();
        
        if (currentUser != null)
        {
            var history = gameService.GetUserHistoryAsync(currentUser.Username).Result;

            if (history.Count == 0)
            {
                Console.WriteLine("No games played yet!");
            }
            else
            {
                foreach (var game in history)
                {
                    Console.WriteLine(
                        $"{game.GameDate.ToLocalTime()}: {game.Player1} vs {game.Player2} - Winner: {(game.Winner.Length == 0 ? "Draw" : game.Winner)}");
                }
            }
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}

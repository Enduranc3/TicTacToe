using TicTacToe.Services;
using TicTacToe.UI;

namespace TicTacToe.Commands.Implementations;

public class ShowPlayersCommand(UserService userService) : ICommand
{
    public string Name => "Show Players";

    public void Execute()
    {
        Console.Clear();
        var players = userService.GetTopPlayersAsync().Result;

        Console.WriteLine("Player Rankings:");
        Console.WriteLine("---------------");
        foreach (var player in players)
        {
            Console.WriteLine($"{player.Username} - Rating: {player.Rating}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}

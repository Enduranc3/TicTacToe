using TicTacToe.UI;

namespace TicTacToe.Commands.Implementations;

public class ViewRatingCommand(ConsoleUi ui) : ICommand
{
    public string Name => "View Rating";

    public void Execute()
    {
        var currentUser = ui.CurrentUser;
        Console.Clear();

        if (currentUser == null)
        {
            throw new InvalidOperationException("Current user is null");
        }

        Console.WriteLine($"Your rating: {currentUser.Rating}");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}

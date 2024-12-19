using TicTacToe.UI;

namespace TicTacToe.Commands.Implementations;

public class LogoutCommand(ConsoleUi ui) : ICommand
{
    public string Name => "Logout";

    public void Execute()
    {
        ui.Logout();
    }
}

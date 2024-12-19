using TicTacToe.Commands.Implementations;
using TicTacToe.Enums;
using TicTacToe.UI;
using TicTacToe.Services;

namespace TicTacToe.Commands;

public abstract class CommandProcessor
{
    public static Dictionary<LoginChoice, ICommand> CreateLoginCommands(ConsoleUi ui, UserService userService)
    {
        return new Dictionary<LoginChoice, ICommand>
        {
            { LoginChoice.Login, new LoginCommand(ui, userService) },
            { LoginChoice.Register, new RegisterCommand(userService) },
            { LoginChoice.ShowPlayers, new ShowPlayersCommand(userService) },
            { LoginChoice.Exit, new ExitCommand() }
        };
    }

    public static Dictionary<MainChoice, ICommand> CreateMainCommands(ConsoleUi ui, UserService userService, GameService gameService)
    {
        return new Dictionary<MainChoice, ICommand>
        {
            { MainChoice.PlayVsPlayer, new PlayVsPlayerCommand(ui, gameService, userService) },
            { MainChoice.PlayVsComputer, new PlayVsComputerCommand(ui) },
            { MainChoice.ViewRating, new ViewRatingCommand(ui) },
            { MainChoice.ViewHistory, new ViewHistoryCommand(ui, gameService) },
            { MainChoice.WatchReplay, new WatchReplayCommand(ui, gameService) },
            { MainChoice.Logout, new LogoutCommand(ui) }
        };
    }
}
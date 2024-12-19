using TicTacToe.Services;
using TicTacToe.UI;

namespace TicTacToe.Commands.Implementations;

public class LoginCommand(ConsoleUi ui, UserService userService) : ICommand
{
	public string Name => "Login";

	public void Execute()
	{
		HandleLoginAsync().GetAwaiter().GetResult();
	}

	private async Task HandleLoginAsync()
	{
		var username = ConsoleUi.GetSecureInput("Username: ");
		var password = ConsoleUi.GetSecureInput("Password: ", true);

		try
		{
			var user = await userService.LoginAsync(username, password);
			ui.SetCurrentUser(user);
		}
		catch (Exception ex)
		{
			ConsoleUi.ShowError(ex.Message);
		}
	}
}
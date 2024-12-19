using TicTacToe.Services;
using TicTacToe.UI;

namespace TicTacToe.Commands.Implementations;

public class RegisterCommand(UserService userService) : ICommand
{
	public string Name => "Register";

	public void Execute()
	{
		HandleRegisterAsync().GetAwaiter().GetResult();
	}

	private async Task HandleRegisterAsync()
	{
		var username = ConsoleUi.GetSecureInput("Username: ");
		var password = ConsoleUi.GetSecureInput("Password: ", true);

		try
		{
			await userService.RegisterAsync(username, password);
			Console.WriteLine("User registered successfully!");
			Thread.Sleep(2000);
		}
		catch (Exception ex)
		{
			ConsoleUi.ShowError(ex.Message);
		}
	}
}
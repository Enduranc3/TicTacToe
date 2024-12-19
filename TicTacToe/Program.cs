using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Database;
using TicTacToe.Repositories;
using TicTacToe.Repositories.Interfaces;
using TicTacToe.Services;
using TicTacToe.UI;

namespace TicTacToe;

internal static class Program
{
	public static void Main()
	{
		Env.Load();

		var serviceProvider = new ServiceCollection()
			.AddDbContext<ApplicationDbContext>(options =>
				options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_TICTACTOE")))
			.AddScoped<IUserRepository, UserRepository>()
			.AddScoped<IGameRepository, GameRepository>()
			.AddScoped<UserService>()
			.AddScoped<GameService>()
			.AddScoped<ConsoleUi>()
			.BuildServiceProvider();

		using (var scope = serviceProvider.CreateScope())
		{
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			context.Database.EnsureDeleted();
			try
			{
				if (!context.Database.EnsureCreated())
				{
					context.Database.Migrate();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Database initialization error: {ex.Message}");
			}
		}

		var userService = serviceProvider.GetService<UserService>();
		userService?.MockUsers();

		var ui = serviceProvider.GetService<ConsoleUi>();
		ui?.Start();
	}
}
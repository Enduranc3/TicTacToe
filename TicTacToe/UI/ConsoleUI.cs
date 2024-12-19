using System.Text;
using TicTacToe.Commands;
using TicTacToe.Enums;
using TicTacToe.Exceptions;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.UI;

public class ConsoleUi
{
    private readonly Dictionary<LoginChoice, ICommand> _loginCommands;
    private readonly Dictionary<MainChoice, ICommand> _mainCommands;
    public User? CurrentUser { get; private set; }

    public ConsoleUi(UserService userService, GameService gameService)
    {
        _loginCommands = CommandProcessor.CreateLoginCommands(this, userService);
        _mainCommands = CommandProcessor.CreateMainCommands(this, userService, gameService);
    }

    public void SetCurrentUser(User? user)
    {
        CurrentUser = user;
    }

    public static string GetSecureInput(string prompt, bool isPassword = false)
    {
        Console.Write(prompt);
        var input = new StringBuilder();
        ConsoleKeyInfo key;

        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Length--;
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                input.Append(key.KeyChar);
                Console.Write(isPassword ? "*" : key.KeyChar);
            }
        }

        Console.WriteLine();
        return input.ToString();
    }

    public static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        Thread.Sleep(2000);
    }

    public void Start()
    {
        while (true)
        {
            if (CurrentUser == null)
            {
                ShowMenu(_loginCommands);
            }
            else
            {
                ShowMenu(_mainCommands);
            }
        }
    }

    private void ShowMenu<T>(Dictionary<T, ICommand> commands) where T : struct, Enum
    {
        while (true)
        {
            Console.Clear();
            if (CurrentUser != null)
            {
                Console.WriteLine($"Welcome, {CurrentUser.Username}! Rating: {CurrentUser.Rating}");
            }

            foreach (var (key, command) in commands)
            {
                Console.WriteLine($"{(int)(object)key}. {command.Name}");
            }

            if (!Enum.TryParse<T>(Console.ReadLine(), out var choice) || !commands.ContainsKey(choice))
            {
                continue;
            }

            commands[choice].Execute();
            if (ShouldReturn(choice))
            {
                return;
            }
        }
    }

    private static bool ShouldReturn<T>(T choice) where T : struct, Enum
    {
        return choice switch
        {
            LoginChoice loginChoice => loginChoice is LoginChoice.Login or LoginChoice.Register or LoginChoice.Exit,
            MainChoice mainChoice => mainChoice is MainChoice.PlayVsPlayer or MainChoice.PlayVsComputer
                or MainChoice.Logout,
            _ => false
        };
    }

    public static void DisplayBoard(Game game)
    {
        for (var i = 0; i < 3; i++)
        {
            Console.Write(" ");
            for (var j = 0; j < 3; j++)
            {
                var cellValue = game.Board[i, j];
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

    public void Logout() => CurrentUser = null;
}
using TicTacToe.Exceptions;
using TicTacToe.Models;
using TicTacToe.Services;
using TicTacToe.UI;

namespace TicTacToe.Commands.Implementations;

public class PlayVsComputerCommand(ConsoleUi ui) : ICommand
{
    public string Name => "Play vs Computer";

    public void Execute()
    {
        var currentUser = ui.CurrentUser;
        if (currentUser == null)
        {
            return;
        }
        var game = new Game(currentUser.Username, "Computer");
        var currentPlayer = 'X';

        while (true)
        {
            Console.Clear();
            ConsoleUi.DisplayBoard(game);

            try
            {
                if (currentPlayer == 'X')
                {
                    Console.WriteLine($"Your turn ({currentPlayer})");
                    Console.WriteLine("Enter position (0-8): ");
                    if (!int.TryParse(Console.ReadLine(), out var position) || position < 0 || position > 8)
                    {
                        continue;
                    }

                    var (row, col) = GameService.ConvertPositionToCoordinates(position);
                    GameService.MakeMove(game, row, col, currentPlayer);
                }
                else
                {
                    Thread.Sleep(1000);
                    var (row, col) = GameLogicService.GetComputerMove(game);
                    GameService.MakeMove(game, row, col, currentPlayer);
                }

                currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
            }
            catch (WinException)
            {
                ConsoleUi.DisplayBoard(game);
                Console.WriteLine($"{(currentPlayer == 'X' ? "You win!" : "Computer wins!")}");
                Thread.Sleep(2000);
                break;
            }
            catch (GameDrawException)
            {
                ConsoleUi.DisplayBoard(game);
                Console.WriteLine("It's a draw!");
                Thread.Sleep(2000);
                break;
            }
            catch (Exception ex)
            {
                ConsoleUi.ShowError($"Invalid move: {ex.Message}");
                Thread.Sleep(2000);
            }
        }
    }
}

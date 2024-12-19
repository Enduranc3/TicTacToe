namespace TicTacToe.Exceptions;

public class InvalidMoveException(string message = "Invalid move") : Exception(message);

public class WinException(char symbol) : Exception($"Player {symbol} has won!")
{
	public char WinningSymbol { get; } = symbol;
}

public class GameDrawException(string message = "The game ended in a draw") : Exception(message);

public class UserNotFoundException(string username)
	: Exception($"User '{username}' not found");

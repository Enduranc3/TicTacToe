using TicTacToe.Exceptions;
using TicTacToe.Models;

namespace TicTacToe.Services;

public static class GameLogicService
{
	public static void MakeMove(Game game, int row, int col, char symbol)
	{
		if (row < 0 || row > 2 || col < 0 || col > 2)
		{
			throw new InvalidMoveException("Position is outside the board");
		}

		if (game.Board[row, col] != ' ')
		{
			throw new InvalidMoveException("This cell is already occupied");
		}
	}

	public static void ValidateBoardState(Game game)
	{
		var isBoardFull = true;
		for (var i = 0; i < 3; i++)
		{
			for (var j = 0; j < 3; j++)
			{
				if (game.Board[i, j] != ' ')
				{
					continue;
				}
				isBoardFull = false;
				break;
			}

			if (!isBoardFull)
			{
				break;
			}
		}

		if (!isBoardFull)
		{
			return;
		}
		if (!CheckWinningBoard(game.Board, 'X') && !CheckWinningBoard(game.Board, 'O'))
		{
			throw new GameDrawException();
		}
	}

	public static void CheckWin(Game game, char symbol)
	{
		for (var i = 0; i < 3; i++)
		{
			if ((game.Board[i, 0] == symbol && game.Board[i, 1] == symbol && game.Board[i, 2] == symbol) ||
			    (game.Board[0, i] == symbol && game.Board[1, i] == symbol && game.Board[2, i] == symbol))
			{
				throw new WinException(symbol);
			}
		}

		if ((game.Board[0, 0] == symbol && game.Board[1, 1] == symbol && game.Board[2, 2] == symbol) ||
		    (game.Board[0, 2] == symbol && game.Board[1, 1] == symbol && game.Board[2, 0] == symbol))
		{
			throw new WinException(symbol);
		}
	}

	public static (int row, int col) GetComputerMove(Game game)
	{
		// Try to win
		var winningMove = FindWinningMove(game, 'O');
		if (winningMove.HasValue)
		{
			return winningMove.Value;
		}

		// Block player's winning move
		var blockingMove = FindWinningMove(game, 'X');
		if (blockingMove.HasValue)
		{
			return blockingMove.Value;
		}

		// Try to take center
		if (game.Board[1, 1] == ' ')
		{
			return (1, 1);
		}

		// Try to take corners
		var corners = new[] { (0, 0), (0, 2), (2, 0), (2, 2) };
		var availableCorners = corners.Where(c => game.Board[c.Item1, c.Item2] == ' ').ToList();
		if (availableCorners.Count != 0)
		{
			return availableCorners[new Random().Next(availableCorners.Count)];
		}

		// Take any available side
		var sides = new[] { (0, 1), (1, 0), (1, 2), (2, 1) };
		var availableSides = sides.Where(s => game.Board[s.Item1, s.Item2] == ' ').ToList();
		if (availableSides.Count != 0)
		{
			return availableSides[new Random().Next(availableSides.Count)];
		}

		throw new InvalidOperationException("No valid moves available");
	}

	private static bool CheckWinningBoard(char[,] board, char symbol)
	{
		// Check rows and columns
		for (var i = 0; i < 3; i++)
		{
			if ((board[i, 0] == symbol && board[i, 1] == symbol && board[i, 2] == symbol) ||
			    (board[0, i] == symbol && board[1, i] == symbol && board[2, i] == symbol))
			{
				return true;
			}
		}

		// Check diagonals
		return (board[0, 0] == symbol && board[1, 1] == symbol && board[2, 2] == symbol) ||
		       (board[0, 2] == symbol && board[1, 1] == symbol && board[2, 0] == symbol);
	}

	private static (int row, int col)? FindWinningMove(Game game, char symbol)
	{
		var boardCopy = new char[3, 3];
		Array.Copy(game.Board, boardCopy, game.Board.Length);

		for (var i = 0; i < 3; i++)
		{
			for (var j = 0; j < 3; j++)
			{
				if (boardCopy[i, j] != ' ')
				{
					continue;
				}

				boardCopy[i, j] = symbol;

				if (CheckWinningBoard(boardCopy, symbol))
				{
					return (i, j);
				}

				boardCopy[i, j] = ' ';
			}
		}

		return null;
	}
}
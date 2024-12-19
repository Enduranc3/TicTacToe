using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Models;

public class Game
{
	private int _moveCounter;

	public Game(string player1, string player2)
	{
		Player1 = player1;
		Player2 = player2;
		GameDate = DateTime.UtcNow;
		InitializeBoard();
	}

	public int Id { get; init; }
	public string Player1 { get; }
	public string Player2 { get; }
	public string BoardState { get; set; } = string.Empty;
	public string Winner { get; set; } = string.Empty;
	public DateTime GameDate { get; private set; }
	public List<GameMove> Moves { get; } = [];

	[NotMapped]
	public char[,] Board
	{
		get => DeserializeBoard();
		set => BoardState = SerializeBoard(value);
	}

	private void InitializeBoard()
	{
		var board = new char[3, 3];
		for (var i = 0; i < 3; i++)
		{
			for (var j = 0; j < 3; j++)
			{
				board[i, j] = ' ';
			}
		}

		Board = board;
	}

	private static string SerializeBoard(char[,] board)
	{
		var chars = new char[9];
		for (var i = 0; i < 3; i++)
		{
			for (var j = 0; j < 3; j++)
			{
				chars[i * 3 + j] = board[i, j];
			}
		}

		return new string(chars);
	}

	private char[,] DeserializeBoard()
	{
		var board = new char[3, 3];
		for (var i = 0; i < 3; i++)
		{
			for (var j = 0; j < 3; j++)
			{
				board[i, j] = BoardState[i * 3 + j];
			}
		}

		return board;
	}

	public void AddMove(int row, int col, char symbol)
	{
		var move = new GameMove(row, col, symbol, ++_moveCounter);
		Moves.Add(move);

		var board = DeserializeBoard();
		board[row, col] = symbol;
		BoardState = SerializeBoard(board);
	}
}
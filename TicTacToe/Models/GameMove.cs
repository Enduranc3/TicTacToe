namespace TicTacToe.Models;

public class GameMove
{
	protected GameMove()
	{
	}

	public GameMove(int row, int col, char symbol, int moveNumber)
	{
		Row = row;
		Col = col;
		Symbol = symbol;
		MoveNumber = moveNumber;
	}

	public int Id { get; init; }
	public int GameId { get; init; }
	public Game Game { get; init; } = null!;
	public int Row { get; init; }
	public int Col { get; init; }
	public char Symbol { get; init; }
	public int MoveNumber { get; init; }
}
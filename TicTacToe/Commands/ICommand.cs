namespace TicTacToe.Commands;

public interface ICommand
{
	string Name { get; }
	void Execute();
}
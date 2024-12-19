namespace TicTacToe.Models;

public class User(string username, string password, string salt)
{
	public int Id { get; init; }
	public string Username { get; } = username;
	public string Password { get; } = password;
	public string Salt { get; } = salt;
	public int Rating { get; set; } = 1000;
}
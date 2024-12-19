using System.Security.Cryptography;
using System.Text;
using TicTacToe.Exceptions;
using TicTacToe.Models;
using TicTacToe.Repositories;
using TicTacToe.Repositories.Interfaces;

namespace TicTacToe.Services;

public class UserService(IUserRepository userRepository)
{
	public async Task MockUsers()
	{
		await RegisterAsync("Alice", "123");
		await RegisterAsync("Bob", "123");
		await RegisterAsync("Charlie", "123");
		await RegisterAsync("David", "123");
		await RegisterAsync("Eve", "123");
		await RegisterAsync("Frank", "123");
		await RegisterAsync("Grace", "123");
		await RegisterAsync("Heidi", "123");
		await RegisterAsync("Taras", "123");
		await RegisterAsync("Judy", "123");
	}

	private static void ValidateCredentials(string username, string password)
	{
		if (string.IsNullOrWhiteSpace(username))
		{
			throw new EmptyUsernameException();
		}

		if (string.IsNullOrWhiteSpace(password))
		{
			throw new EmptyPasswordException();
		}
	}

	private static string GenerateSalt()
	{
		var saltBytes = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(saltBytes);
		return Convert.ToBase64String(saltBytes);
	}

	private static string HashPassword(string password, string salt)
	{
		var saltedPassword = password + salt;
		var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
		var builder = new StringBuilder();
		foreach (var b in bytes)
		{
			builder.Append(b.ToString("x2"));
		}

		return builder.ToString();
	}

	public async Task RegisterAsync(string username, string password)
	{
		try
		{
			ValidateCredentials(username, password);

			if (await userRepository.GetByUsernameAsync(username) != null)
			{
				throw new DuplicateUsernameException(username);
			}

			var salt = GenerateSalt();
			var hashedPassword = HashPassword(password, salt);
			var user = new User(username, hashedPassword, salt);
			await userRepository.AddAsync(user);
		}
		catch (Exception ex) when (ex is not DuplicateUsernameException)
		{
			throw new UserAuthenticationException(ex.Message);
		}
	}

	public async Task<User> LoginAsync(string username, string password)
	{
		ValidateCredentials(username, password);
		var user = await userRepository.GetByUsernameAsync(username) ??
		           throw new UserNotFoundException(username);
		var hashedPassword = HashPassword(password, user.Salt);
		if (user.Password != hashedPassword)
		{
			throw new PasswordMismatchException();
		}

		return user;
	}

	public async Task UpdateRatingAsync(string username, bool won)
	{
		try
		{
			var user = await userRepository.GetByUsernameAsync(username) ??
			           throw new UserNotFoundException(username);
			var ratingChange = CalculateRatingChange(user.Rating, won);
			user.Rating = Math.Max(1, user.Rating + ratingChange);
			await userRepository.UpdateAsync(user);
		}
		catch (Exception)
		{
			throw new UserRatingUpdateException(username);
		}
	}

	public async Task<User?> GetUserByNameAsync(string username)
	{
		return await userRepository.GetByUsernameAsync(username);
	}

	public async Task<List<User>> GetTopPlayersAsync()
	{
		return await userRepository.GetAllOrderedByRatingAsync();
	}

	private static int CalculateRatingChange(int currentRating, bool won)
	{
		var baseChange = won ? 10 : -10;
		var ratingFactor = Math.Max(1, (2000 - currentRating) / 100);
		return baseChange * ratingFactor;
	}
}
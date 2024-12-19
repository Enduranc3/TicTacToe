using Microsoft.EntityFrameworkCore;
using TicTacToe.Database;
using TicTacToe.Models;
using TicTacToe.Repositories.Interfaces;

namespace TicTacToe.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
	public async Task<User?> GetByUsernameAsync(string username)
	{
		return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
	}

	public async Task AddAsync(User user)
	{
		if (await GetByUsernameAsync(user.Username) != null)
		{
			throw new InvalidOperationException("User already exists");
		}

		await context.Users.AddAsync(user);
		await context.SaveChangesAsync();
	}

	public async Task UpdateAsync(User user)
	{
		var existingUser = await GetByUsernameAsync(user.Username);
		if (existingUser != null)
		{
			existingUser.Rating = user.Rating;
			await context.SaveChangesAsync();
		}
		else
		{
			throw new InvalidOperationException("User doesn't exist");
		}
	}

	public async Task<List<User>> GetAllOrderedByRatingAsync()
	{
		return await context.Users
			.OrderByDescending(u => u.Rating)
			.ToListAsync();
	}
}
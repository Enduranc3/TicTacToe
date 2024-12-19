using TicTacToe.Models;

namespace TicTacToe.Repositories.Interfaces;

public interface IUserRepository
{
	Task<User?> GetByUsernameAsync(string username);
	Task AddAsync(User user);
	Task UpdateAsync(User user);
	Task<List<User>> GetAllOrderedByRatingAsync();
}
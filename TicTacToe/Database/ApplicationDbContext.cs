using Microsoft.EntityFrameworkCore;
using TicTacToe.Models;

namespace TicTacToe.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Game> Games { get; set; } = null!;
	public DbSet<GameMove> GameMoves { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => e.Username).IsUnique();
			entity.Property(e => e.Password).IsRequired();
			entity.Property(e => e.Salt).IsRequired();
			entity.Property(e => e.Rating).HasDefaultValue(1000);
		});

		modelBuilder.Entity<Game>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.BoardState).IsRequired();
			entity.Property(e => e.Player1).IsRequired();
			entity.Property(e => e.Player2).IsRequired();
			entity.Property(e => e.GameDate)
				.IsRequired()
				.HasColumnType("timestamp with time zone");

			entity.HasMany(e => e.Moves)
				.WithOne(e => e.Game)
				.HasForeignKey(e => e.GameId)
				.OnDelete(DeleteBehavior.Cascade);

			entity.Navigation(e => e.Moves).AutoInclude();
		});

		modelBuilder.Entity<GameMove>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Row).IsRequired();
			entity.Property(e => e.Col).IsRequired();
			entity.Property(e => e.Symbol).IsRequired();
			entity.Property(e => e.MoveNumber).IsRequired();

			entity.HasOne(e => e.Game)
				.WithMany(e => e.Moves)
				.HasForeignKey(e => e.GameId)
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}
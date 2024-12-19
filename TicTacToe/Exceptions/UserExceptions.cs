namespace TicTacToe.Exceptions;

public class EmptyUsernameException() : Exception("Username cannot be empty");

public class EmptyPasswordException() : Exception("Password cannot be empty");

public class PasswordMismatchException() : Exception("Incorrect password");

public class DuplicateUsernameException(string username) : Exception($"Username '{username}' is already taken")
{
}

public class UserRatingUpdateException(string username) : Exception($"Failed to update rating for user '{username}'")
{
}

public class UserAuthenticationException(string message = "Authentication failed") : Exception(message)
{
}
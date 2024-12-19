# TicTacToe Game

A console-based Tic-tac-toe game implemented in C# using .NET 9.0.

## Features

- Command-based gameplay interface
- Database integration using Entity Framework Core
- Environment variable configuration with DotNetEnv
- Exception handling for game and user operations

## Prerequisites

- .NET 9.0 SDK
- PostgreSQL database

## Getting Started

1. Clone the repository
2. Configure the database connection in `.env` file
3. Run the migrations:

```sh
dotnet ef database update
```

4. Build and run the application:

```sh
dotnet build
dotnet run
```

## Configuration

Create a `.env` file in the root directory of the project and add the following environment variable:

```env
DATABASE_CONNECTION_STRING=your_connection_string_here
```

## Technologies used

- C#
- .NET 9.0
- Entity Framework Core
- DotNetEnv 3.1.1
- PostgreSQL

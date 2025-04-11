-- C# .NET TURN BASED TicTacToe GAME SERVER --
In order to run this code you will need to:
1. Put your own Redis connection string and password in both servers' appsettings.Development.json files.
2. Run two separate terminals: one for the lobby server and one for the game server. cd to the project directory of each server and use "dotnet run".
3. Have two running instances of your TicTacToe game (or any game where a player's "move" could be an index of a tile on the board).

namespace TicTacToeGameServer.Interfaces {
    public interface IGamesActiveRedisService {
        void incrementActiveGames();

        void decrementActiveGames();
    }
}
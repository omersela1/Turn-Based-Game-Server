using TicTacToeGameServer.Interfaces;

namespace TicTacToeGameServer.Services.Redis {
    public class GamesActiveRedisService : IGamesActiveRedisService {
        private readonly IRedisBaseService _redisBaseService;

        public GamesActiveRedisService(IRedisBaseService redisBaseService) {
            _redisBaseService = redisBaseService;
        }

        public void incrementActiveGames() {
            string? activeGames = _redisBaseService.GetString("activeGames");
            if (activeGames == null) {
                _redisBaseService.SetString("activeGames", "1");
            } else {
                _redisBaseService.SetString("activeGames", (int.Parse(activeGames) + 1).ToString());
            }
        }

        public void decrementActiveGames() {
            string? activeGames = _redisBaseService.GetString("activeGames");
            if (activeGames != null) {
                _redisBaseService.SetString("activeGames", (int.Parse(activeGames) - 1).ToString());
            }
        }
}

}
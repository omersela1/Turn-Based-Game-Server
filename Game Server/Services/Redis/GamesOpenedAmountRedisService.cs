using TicTacToeGameServer.Interfaces;

namespace TicTacToeGameServer.Services.Redis
{
    public class GamesOpenedAmountRedisService : IGamesOpenedAmountRedisService
    {
        private readonly IRedisBaseService _redisBaseService;

        public GamesOpenedAmountRedisService(IRedisBaseService redisBaseService)
        {
            _redisBaseService = redisBaseService;
        }

        public void IncrementGamesOpenedAmount()
        {
            int? gamesOpenedAmount = int.Parse(GetGamesOpenedAmount());
            if (gamesOpenedAmount == null)
            {
                _redisBaseService.SetString("GamesOpenedAmount", "1");
            }
            else
            {
                _redisBaseService.SetString("GamesOpenedAmount", (gamesOpenedAmount + 1).ToString());
            }
        }

        public string GetGamesOpenedAmount()
        {
            return _redisBaseService.GetString("GamesOpenedAmount");
        }
    }
}
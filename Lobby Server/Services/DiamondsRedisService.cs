using TicTacToeLobbyServer.Interfaces;

namespace TicTacToeLobbyServer.Services
{
    public class DiamondsRedisService : IDiamondsRedisService
    {
        private readonly IRedisBaseService _redisBaseService;

        public DiamondsRedisService(IRedisBaseService redisBaseService)
        {
            _redisBaseService = redisBaseService;
        }

        public string GetDiamonds(string email)
        {
            return _redisBaseService.GetString($"{email}#Diamonds");
        }

         public string GetCurrentBonusAmount(string email) {
             return _redisBaseService.GetString($"{email}#CurrentBonusAmount");
         }

        public void SetDiamonds(string email, int amount)
        {
            _redisBaseService.SetString($"{email}#Diamonds", amount.ToString());
        }

         public void SetCurrentBonusAmount(string email, int amount) {
             _redisBaseService.SetString($"{email}#CurrentBonusAmount", amount.ToString());
         }


    }
}
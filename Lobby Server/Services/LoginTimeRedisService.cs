using TicTacToeLobbyServer.Interfaces;

namespace TicTacToeLobbyServer.Services
{
    public class LoginTimeRedisService : ILoginTimeRedisService
    {
     
            private readonly IRedisBaseService _redisBaseService;

            public LoginTimeRedisService(IRedisBaseService redisBaseService)
            {
                _redisBaseService = redisBaseService;
            }
            public string GetLoginTime(string email) {
                return _redisBaseService.GetString($"{email}#LoginTime");
            }

            public void SetLoginTime(string email, string loginTime) {
                _redisBaseService.SetString($"{email}#LoginTime", loginTime);
            }
    }
    
}
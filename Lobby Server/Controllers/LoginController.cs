using TicTacToeLobbyServer.Interfaces;
using TicTacToeLobbyServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToeLobbyServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : Controller
    {
        private string _gameServerUrl = string.Empty;
        private readonly IPlayersRedisService _playersRedisService;
        private readonly IRatingRedisService _ratingRedisService;

        private readonly IDiamondsRedisService _diamondsRedisService;

        private readonly ILoginTimeRedisService _loginTimeRedisService;

        public LoginController(IConfiguration configuration, IPlayersRedisService playersRedisService, IRatingRedisService ratingRedisService, 
        IDiamondsRedisService diamondsRedisService, ILoginTimeRedisService loginTimeRedisService)
        {
            _gameServerUrl = configuration["GameServer:ConnectionString"].ToString();
            _playersRedisService = playersRedisService;
            _ratingRedisService = ratingRedisService;
            _diamondsRedisService = diamondsRedisService;
            _loginTimeRedisService = loginTimeRedisService;
        }
        
        [HttpGet("Login/{email}&{password}")]
        public Dictionary<string, object> Login(string email, string password)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            Dictionary<string, string> playerData = _playersRedisService.GetPlayer(email);
            if (playerData.Count > 0 && playerData.ContainsKey("Password"))
            {
                string playerDataPassword = playerData["Password"];
                if (playerDataPassword == password)
                {
                    result.Add("IsLoggedIn", true);
                    result.Add("UserId", playerData["UserId"]);
                    result.Add("Rating", _ratingRedisService.GetPlayerRating(playerData["UserId"]));
                    result.Add("GameServerUrl", _gameServerUrl);
                    string lastLoginTime = _loginTimeRedisService.GetLoginTime(email);
                    if (lastLoginTime == null)
                    {
                        _loginTimeRedisService.SetLoginTime(email, DateTime.UtcNow.ToString());
                        _diamondsRedisService.SetDiamonds(email, 100);
                        result.Add("Diamonds", _diamondsRedisService.GetDiamonds(email));
                    }
                    else {
                        DateTime lastLogin = DateTime.Parse(_loginTimeRedisService.GetLoginTime(email));
                        TimeSpan timeDifference = DateTime.UtcNow - lastLogin;
                        result.Add("LoginTimeDifference", timeDifference.TotalHours);
                        if (timeDifference.TotalHours > 24 && timeDifference.TotalHours < 48)
                        {
                            int currentBonusAmount = int.Parse(_diamondsRedisService.GetCurrentBonusAmount(email));
                            _diamondsRedisService.SetDiamonds(email, (int.Parse(_diamondsRedisService.GetDiamonds(email)) + currentBonusAmount * 10));
                            _loginTimeRedisService.SetLoginTime(email, DateTime.UtcNow.ToString());
                            _diamondsRedisService.SetCurrentBonusAmount(email, ((currentBonusAmount) % 6 + 1));
                            result.Add("Bonus", currentBonusAmount * 10);
                        }
                        else if (timeDifference.TotalHours > 48) {
                            _loginTimeRedisService.SetLoginTime(email, DateTime.UtcNow.ToString());
                            _diamondsRedisService.SetCurrentBonusAmount(email, 1);
                            result.Add("Bonus", "More than 24 hours passed since last login, streak reset.");
                        }
                         result.Add("Diamonds", _diamondsRedisService.GetDiamonds(email));
                    }
                }
                else
                {
                    result.Add("IsLoggedIn", false);
                    result.Add("ErrorMessage", "Wrong Password");
                }
            }
            else
            {
                result.Add("IsLoggedIn", false);
                result.Add("ErrorMessage", "Player Doesnt Exist");
            }

            result.Add("Response", "Login");
            return result;
        }
    }
}

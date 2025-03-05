using Microsoft.AspNetCore.Mvc;

namespace TicTacToeLobbyServer.Interfaces
{
    public interface IPlayersRedisService
    {
        public Dictionary<string, string> GetPlayer(string email);

        public void SetPlayer(string email, Dictionary<string, string> playerData);
    }
}

using Microsoft.AspNetCore.Mvc;

namespace TicTacToeLobbyServer.Interfaces
{
    public interface ILoginTimeRedisService
    {
        public string GetLoginTime(string email);

        public void SetLoginTime(string email, string loginTime);
    }
}

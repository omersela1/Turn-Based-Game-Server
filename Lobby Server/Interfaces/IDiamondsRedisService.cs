namespace TicTacToeLobbyServer.Interfaces
{
    public interface IDiamondsRedisService
    {
           public string GetDiamonds(string email);

           public string GetCurrentBonusAmount(string email);

            public void SetDiamonds(string email, int amount);

            public void SetCurrentBonusAmount(string email, int amount);
    }
}
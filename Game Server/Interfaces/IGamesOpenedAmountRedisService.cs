namespace TicTacToeGameServer.Interfaces
{
    public interface IGamesOpenedAmountRedisService
    {
         public void IncrementGamesOpenedAmount();

        public string GetGamesOpenedAmount();
    }
}
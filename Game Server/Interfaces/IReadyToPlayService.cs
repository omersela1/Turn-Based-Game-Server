using TicTacToeGameServer.Models;

namespace TicTacToeGameServer.Interfaces
{
    public interface IReadyToPlayService
    {
        Dictionary<string, object> Get(User user, Dictionary<string, object> details);
    }
}

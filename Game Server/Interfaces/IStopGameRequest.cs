using TicTacToeGameServer.Models;

namespace TicTacToeGameServer.Interfaces
{
    public interface IStopGameRequest
    {
        Dictionary<string, object> Get(User user, Dictionary<string, object> details);
    }
}

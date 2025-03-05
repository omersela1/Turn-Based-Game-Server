using TicTacToeGameServer.Models;

namespace TicTacToeGameServer.Interfaces
{
    public interface ISendMoveRequest
    {
        Dictionary<string, object> Get(User user, Dictionary<string, object> details);
    }
}

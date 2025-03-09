using TicTacToeGameServer.Models;

namespace TicTacToeGameServer.Interfaces
{
    public interface IServiceHandler
    {
        public string ServiceName { get; }
        public List<Dictionary<string, object>> Handle(User user, Dictionary<string, object> details);
    }
}

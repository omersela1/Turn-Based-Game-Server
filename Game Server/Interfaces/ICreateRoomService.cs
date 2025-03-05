using TicTacToeGameServer.Managers;

namespace TicTacToeGameServer.Interfaces
{
    public interface ICreateRoomService
    {
        Dictionary<string, object> Create(MatchData curMatchData);
    }
}

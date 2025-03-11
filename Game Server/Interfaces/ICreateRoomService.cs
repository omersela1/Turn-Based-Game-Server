using TicTacToeGameServer.Managers;

namespace TicTacToeGameServer.Interfaces
{
    public interface ICreateRoomService
    {
        Dictionary<string, object> Create(MatchData curMatchData, 
        string roomName, string owner, int maxUsers, string password);
    }
}

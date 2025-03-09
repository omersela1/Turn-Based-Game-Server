using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Models;

namespace TicTacToeGameServer.Managers
{
    public class RoomsManager
    {
        private Dictionary<string, GameRoom> _activeRooms;
        public Dictionary<string, GameRoom> ActiveRooms { get => _activeRooms; }

        private readonly IGamesOpenedAmountRedisService _gamesOpenedAmountRedisService;

        public RoomsManager(IGamesOpenedAmountRedisService gamesOpenedAmountRedisService) 
        { 
            _activeRooms = new Dictionary<string, GameRoom>();
            _gamesOpenedAmountRedisService = gamesOpenedAmountRedisService;
        }

        public void AddRoom(string matchId,GameRoom gameRoom)
        {
            if (_activeRooms == null)
                _activeRooms = new Dictionary<string, GameRoom>();

            if(_activeRooms.ContainsKey(matchId))
                _activeRooms[matchId] = gameRoom;
            else _activeRooms.Add(matchId,gameRoom);
            
            _gamesOpenedAmountRedisService.IncrementGamesOpenedAmount();
        }

        public void RemoveRoom(string matchId) 
        {
             if(_activeRooms != null && _activeRooms.ContainsKey(matchId))
                _activeRooms.Remove(matchId);
        }

        public GameRoom GetRoom(string matchId)
        {
            if (_activeRooms != null && _activeRooms.ContainsKey(matchId))
                return _activeRooms[matchId];
            return null;
        }

        public bool IsRoomExist(string matchId)
        {
            if(GetRoom(matchId) != null)
                return true;    
            return false;
        }

        public int GetRoomsCount()
        {
            return _activeRooms.Count;
        }
    }
}

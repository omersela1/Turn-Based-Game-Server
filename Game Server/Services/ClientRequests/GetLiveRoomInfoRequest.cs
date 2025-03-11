using TicTacToeGameServer.Models;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeGameServer.Services.ClientRequests
{
    public class GetLiveRoomInfoRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;

        private string _currentRoomId = string.Empty;
        public string ServiceName => "GetLiveRoomInfo";

        public GetLiveRoomInfoRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }

        public List<Dictionary<string, object>> Handle(User user, Dictionary<string, object> details)
        {
            _currentRoomId = details["RoomId"].ToString();
            Console.WriteLine("GetLiveRoomInfoRequest: Handle");
            Console.WriteLine("RoomId: " + _currentRoomId);
            GameRoom room = _roomManager.GetRoom(_currentRoomId);
            Dictionary<string, object> responseDictionary = new Dictionary<string, object>
            {
                { "RoomData", room.ConvertToDictionary() },
                { "RoomProperties", room.Password },
                { "Users", room.Users }
            };
            return new List<Dictionary<string, object>> { responseDictionary };
        }
    }
}
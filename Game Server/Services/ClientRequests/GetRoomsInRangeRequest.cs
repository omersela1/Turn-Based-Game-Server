using TicTacToeGameServer.Models;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeGameServer.Services.ClientRequests
{
    public class GetRoomsInRangeRequest : IServiceHandler
    {
        private readonly RoomsManager _roomManager;
        public string ServiceName => "GetRoomsInRange";

        public GetRoomsInRangeRequest(RoomsManager roomManager)
        {
            _roomManager = roomManager;
        }

        public List<Dictionary<string, object>> Handle(User user, Dictionary<string, object> details)
        {
            return _roomManager.ActiveRooms.Values.Select(room => room.ConvertToDictionary()).ToList();
        }
    }
}
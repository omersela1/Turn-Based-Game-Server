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

        public object Handle(User user, Dictionary<string, object> details)
        {
            Dictionary<string, object> response = new Dictionary<string, object> {
                { "Response", ServiceName },
                { "Rooms", _roomManager.ActiveRooms.Values.Select(room => room.ConvertToDictionary()).ToList() }
            };
            return response;
        }
    }
}
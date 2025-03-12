using TicTacToeGameServer.Models;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeGameServer.Services.ClientRequests {
    public class SubscribeRoomRequest : IServiceHandler {
        private readonly RoomsManager _roomManager;

        public string ServiceName => "SubscribeRoom";

        public SubscribeRoomRequest(RoomsManager roomsManager) {
            _roomManager = roomsManager;
        }

        public List<Dictionary<string, object>> Handle(User user, Dictionary<string, object> details) {
            // prepare the data
            string senderId = user.UserId;
            string roomId = details["RoomId"].ToString();
            Console.WriteLine("SubscribeRoomRequest: Handle");
            Console.WriteLine("Sender: " + senderId);
            Console.WriteLine("RoomId: " + roomId);
            
            _roomManager.GetRoom(roomId).SubscribeRoom(senderId);

            // return result

            return new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "isSuccess", _roomManager.GetRoom(roomId).IsUserSubscribed(senderId) },
                    { "Sender", senderId },
                    { "RoomId", roomId }
                }
            };
        }
    }
}
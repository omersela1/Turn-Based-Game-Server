using TicTacToeGameServer.Models;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeGameServer.Services.ClientRequests {
    public class JoinRoomRequest : IServiceHandler {
        private readonly RoomsManager _roomManager;

        public string ServiceName => "JoinRoom";

        public JoinRoomRequest(RoomsManager roomsManager) {
            _roomManager = roomsManager;
        }

        public List<Dictionary<string, object>> Handle(User user, Dictionary<string, object> details) {
            // prepare the data
            string senderId = user.UserId;
            string roomId = details["RoomId"].ToString();
            Console.WriteLine("JoinRoomRequest: Handle");
            Console.WriteLine("Sender: " + senderId);
            Console.WriteLine("RoomId: " + roomId);
            
            // join the room
            _roomManager.GetRoom(roomId).JoinRoom(senderId);
            _roomManager.UserToRoom(senderId, roomId);
            // return result

            return new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "isSuccess", _roomManager.GetRoom(roomId).IsUserInRoom(senderId) },
                    { "Sender", senderId },
                    { "RoomId", roomId }
                }
            };
        }
    }
}
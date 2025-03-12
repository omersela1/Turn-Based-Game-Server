using TicTacToeGameServer.Models;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeGameServer.Services.ClientRequests {
    public class LeaveRoomRequest : IServiceHandler {
        private readonly RoomsManager _roomManager;

        public string ServiceName => "LeaveRoom";

        public LeaveRoomRequest(RoomsManager roomsManager) {
            _roomManager = roomsManager;
        }

        public List<Dictionary<string, object>> Handle(User user, Dictionary<string, object> details) {
            // prepare the data
            string senderId = user.UserId;
            string roomId = details["RoomId"].ToString();
            Console.WriteLine("LeaveRoomRequest: Handle");
            Console.WriteLine("Sender: " + senderId);
            Console.WriteLine("RoomId: " + roomId);
            
            // keep the data before leaving (room might be removed from room manager)
            string owner = _roomManager.GetRoom(roomId).Owner;
            string name = _roomManager.GetRoom(roomId).Name;
            int maxUsers = _roomManager.GetRoom(roomId).MaxUsersCount;
            bool isSuccess = false;

            _roomManager.RemoveUserFromRoom(senderId); // removes the room from the user's current room assignment
            _roomManager.GetRoom(roomId).LeaveRoom(senderId);
            
            if (_roomManager.IsRoomExist(roomId)) {
                isSuccess = !_roomManager.GetRoom(roomId).IsUserInRoom(senderId);
            }
            else {
                isSuccess = true;
            }
            
            // return result

            return new List<Dictionary<string, object>> {
                new Dictionary<string, object> {
                    { "IsSuccess", isSuccess },
                    { "RoomId", roomId },
                    { "Owner", owner },
                    { "Name", name },
                    { "MaxUsers", maxUsers },
                    { "RoomExist", _roomManager.IsRoomExist(roomId) }
                }
            };
        }
    }
}
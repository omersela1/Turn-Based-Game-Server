using TicTacToeGameServer.Models;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeGameServer.Services.ClientRequests {
    public class StartGameRequest : IServiceHandler {
        private readonly RoomsManager _roomManager;

        public string ServiceName => "StartGame";

        public StartGameRequest(RoomsManager roomsManager) {
            _roomManager = roomsManager;
        }

        public List<Dictionary<string, object>> Handle(User user, Dictionary<string, object> details) {
            // prepare the data
            string senderId = user.UserId;
            string roomId = _roomManager.GetCurrentUserRoom(senderId);
            if (roomId != null && roomId != string.Empty) {
                Console.WriteLine("StartGameRequest: Handle");
                Console.WriteLine("Sender: " + senderId);
                Console.WriteLine("RoomId: " + roomId);
                // start the game
                _roomManager.GetRoom(roomId).StartGame(senderId);
                // return result
                return new List<Dictionary<string, object>> {
                    new Dictionary<string, object> {
                        { "isSuccess", _roomManager.GetRoom(roomId).IsRoomActive },
                        { "Sender", senderId },
                        { "RoomId", roomId }
                    }
                };

            }
            else {
                return new List<Dictionary<string, object>> {
                    new Dictionary<string, object> {
                        { "isSuccess", false },
                        { "Sender", senderId },
                        { "ErrorMessage", senderId + " is not in a room"}
                    }
                };
            }
        }
    }
}


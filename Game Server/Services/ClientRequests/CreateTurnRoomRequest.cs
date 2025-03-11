using TicTacToeGameServer.Models;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace TicTacToeGameServer.Services.ClientRequests {
    public class CreateTurnRoomRequest : IServiceHandler {
        private readonly ICreateRoomService _createRoomService;

        private readonly IRatingRedisService _ratingRedisService;

        private readonly IGamesOpenedAmountRedisService _gamesOpenedAmountRedisService;

        private readonly IMatchIdRedisService _matchIdRedisService;

        private readonly IdToUserIdManager _idToUserIdManager;

        public string ServiceName => "CreateTurnRoom";

        public CreateTurnRoomRequest(ICreateRoomService createRoomService, 
        IRatingRedisService ratingRedisService, 
        IGamesOpenedAmountRedisService gamesOpenedAmountRedisService, 
        IMatchIdRedisService matchIdRedisService, 
        IdToUserIdManager idToUserIdManager) {
            _createRoomService = createRoomService;
            _ratingRedisService = ratingRedisService;
            _gamesOpenedAmountRedisService = gamesOpenedAmountRedisService;
            _matchIdRedisService = matchIdRedisService;
            _idToUserIdManager = idToUserIdManager;
        }

        public List<Dictionary<string, object>> Handle(User user, Dictionary<string, object> details) {
            // build the room creator's search data
            string creatorId = user.UserId;
            Console.WriteLine("CreatorId: " + creatorId);
            int creatorRating = int.Parse(_ratingRedisService.GetPlayerRating(creatorId));
            Console.WriteLine("CreatorRating: " + creatorRating);
            SearchData roomCreatorSearchData = new SearchData(creatorId, creatorRating);

            // create the match data
            _gamesOpenedAmountRedisService.IncrementGamesOpenedAmount();
            int matchId = int.Parse(_matchIdRedisService.GetMatchId()) + 1;
            Console.WriteLine("MatchId: " + matchId);
            List<SearchData> searchDataList = new List<SearchData> { roomCreatorSearchData };
            MatchData matchData = new MatchData(matchId, searchDataList);

            string roomName = details["Name"].ToString();
            int maxUsers = int.Parse(details["MaxUsers"].ToString());
            string password = null;
            
           if (details.TryGetValue("TableProperties", out var tablePropertiesObj))
            {
                if (tablePropertiesObj is Dictionary<string, object> tablePropertiesDict)
                {
                     // Already a Dictionary
                    if (tablePropertiesDict.TryGetValue("Password", out var passwordObj))
                    {
                         password = passwordObj?.ToString();
                    }
                }
                else if (tablePropertiesObj is JObject tablePropertiesJObject)
                {
                     // Convert from JObject to Dictionary
                     var tablePropertiesDictionary = tablePropertiesJObject.ToObject<Dictionary<string, object>>();
                    if (tablePropertiesDictionary.TryGetValue("Password", out var passwordObj))
                    {
                         password = passwordObj?.ToString();
                    }
                }
            }
            if (password != null) {
          
            Console.WriteLine("RoomName: " + roomName);
            Console.WriteLine("Password: " + password);
            Console.WriteLine("MaxUsers: " + maxUsers);

            // create the room
            var createRoomResponse = _createRoomService.Create(
                     matchData, roomName, creatorId, maxUsers, password);
            if ((createRoomResponse.TryGetValue("MatchId", out var receivedRoomId)) && receivedRoomId != null)
            {
                string roomId = receivedRoomId.ToString();
                Dictionary<string, object> roomData = new Dictionary<string, object> {
                    { "RoomId", roomId },
                    { "isSuccess", true }
                };
                return new List<Dictionary<string, object>> { roomData };
            }
            else
            {
                Dictionary<string, object> roomData = new Dictionary<string, object> {
                    { "ErrorMessage", "Failed to create room" },
                    { "RoomId", null },
                    { "isSuccess", false }
                };
                return new List<Dictionary<string, object>> { roomData };
            }
        }
            else
            {
                Dictionary<string, object> roomData = new Dictionary<string, object> {
                    { "ErrorMessage", "Password is null" },
                    { "RoomId", null },
                    { "isSuccess", false }
                };
                return new List<Dictionary<string, object>> { roomData };
              
            }
        }
    }
}
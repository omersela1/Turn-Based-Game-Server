using TicTacToeGameServer.Models;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using System.Collections.Generic;
using System.Linq;

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
            Console.WriteLine("RoomName: " + roomName);
            Console.WriteLine("MaxUsers: " + maxUsers);

            // create the room
            var createRoomResponse = _createRoomService.Create(
                     matchData, roomName, creatorId, maxUsers);
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
                    { "RoomId", null },
                    { "isSuccess", false }
                };
                return new List<Dictionary<string, object>> { roomData };
            }
        }
    }
}
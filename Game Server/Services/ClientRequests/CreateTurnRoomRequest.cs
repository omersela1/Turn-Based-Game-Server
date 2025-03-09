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

        private readonly IdToUserIdManager _idToUserIdManager;

        public string ServiceName => "CreateTurnRoom";

        public CreateTurnRoomRequest(ICreateRoomService createRoomService, 
        IRatingRedisService ratingRedisService, 
        IGamesOpenedAmountRedisService gamesOpenedAmountRedisService, IdToUserIdManager idToUserIdManager) {
            _createRoomService = createRoomService;
            _ratingRedisService = ratingRedisService;
            _gamesOpenedAmountRedisService = gamesOpenedAmountRedisService;
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
            string matchIdString = _gamesOpenedAmountRedisService.GetGamesOpenedAmount();
            Console.WriteLine("MatchIdString: " + matchIdString);
            int matchId = int.Parse(matchIdString);
            List<SearchData> searchDataList = new List<SearchData> { roomCreatorSearchData };
            MatchData matchData = new MatchData(matchId, searchDataList);

            // create the room
            string roomId = _createRoomService.Create(matchData)["MatchId"].ToString();
            Dictionary<string, object> roomData = new Dictionary<string, object> {
                { "RoomId", roomId },
                { "isSuccess", true }
            };
            return new List<Dictionary<string, object>> { roomData };
        }
    }
}
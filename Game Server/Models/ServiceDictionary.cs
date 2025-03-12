using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using TicTacToeGameServer.Services;
using TicTacToeGameServer.Services.Redis;
using TicTacToeGameServer.Services.ClientRequests;
using System.Collections.Generic;


namespace TicTacToeGameServer.Models
{
    public class ServiceDictionary : Dictionary<string, IServiceHandler>
    {
        public ServiceDictionary(RoomsManager roomManager, 
        ICreateRoomService createRoomService, 
        IRatingRedisService ratingRedisService, 
        IGamesOpenedAmountRedisService gamesOpenedAmountRedisService,
        IMatchIdRedisService matchIdRedisService,
        IdToUserIdManager idToUserIdManager)
        {
            Add("GetRoomsInRange", new GetRoomsInRangeRequest(roomManager));
            Add("GetLiveRoomInfo", new GetLiveRoomInfoRequest(roomManager));
            Add("CreateTurnRoom", new CreateTurnRoomRequest(createRoomService, ratingRedisService, gamesOpenedAmountRedisService, matchIdRedisService, idToUserIdManager));
            Add("JoinRoom", new JoinRoomRequest(roomManager));
            Add("SubscribeRoom", new SubscribeRoomRequest(roomManager));
            Add("StartGame", new StartGameRequest(roomManager));
            Add("SendMove", new SendMoveRequest(roomManager));
            Add("StopGame", new StopGameRequest(roomManager));
            Add("LeaveRoom", new LeaveRoomRequest(roomManager));
        }   
    }

}
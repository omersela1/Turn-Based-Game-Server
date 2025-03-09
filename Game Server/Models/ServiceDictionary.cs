using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using TicTacToeGameServer.Services;
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
        IdToUserIdManager idToUserIdManager)
        {
            Add("GetRoomsInRange", new GetRoomsInRangeRequest(roomManager));
            Add("CreateTurnRoom", new CreateTurnRoomRequest(createRoomService, ratingRedisService, gamesOpenedAmountRedisService, idToUserIdManager));
        }   
    }

}
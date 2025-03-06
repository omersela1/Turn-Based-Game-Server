using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using TicTacToeGameServer.Services.ClientRequests;
using System.Collections.Generic;


namespace TicTacToeGameServer.Models
{
    public class ServiceDictionary : Dictionary<string, IServiceHandler>
    {
        public ServiceDictionary(RoomsManager roomManager)
        {
            Add("GetRoomsInRange", new GetRoomsInRangeRequest(roomManager));
        }   
    }

}
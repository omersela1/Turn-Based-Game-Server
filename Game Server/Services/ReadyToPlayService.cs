﻿using System;
using System.Collections.Generic;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;
using TicTacToeGameServer.Models;

namespace TicTacToeGameServer.Services
{
    public class ReadyToPlayService : IReadyToPlayService
    {
        private readonly MatchingManager _matchingManager;
        private readonly ICreateRoomService _createRoomService;

        public ReadyToPlayService(MatchingManager matchingManager, ICreateRoomService createRoomService) 
        {
            _matchingManager = matchingManager;
            _createRoomService = createRoomService;
        }

        public Dictionary<string, object> Get(User user, Dictionary<string, object> details)
        {
            Dictionary<string,object> response = new Dictionary<string,object>();
            if(details.ContainsKey("MatchId"))
            {
                bool isReady = ReadyToPlayLogic(user, details["MatchId"].ToString());
                response.Add("IsSuccess", isReady);
            }
            else response.Add("IsSuccess", false);
            return response;
        }

        private bool ReadyToPlayLogic(User user,string matchId)
        {
            MatchData matchData = _matchingManager.GetMatchData(matchId);
            if(matchData != null)
            {
                matchData.ChangePlayerReady(user.UserId,true);
                if(matchData.IsAllReady())
                {
                    _matchingManager.RemoveFromMatchingData(matchId);
                    _createRoomService.Create(matchData);
                    Console.WriteLine("Create Room");
                }
                return true;
            }
            return false;
        }
    }
}

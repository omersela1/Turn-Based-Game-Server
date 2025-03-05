﻿using Newtonsoft.Json;
using System;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;

namespace TicTacToeGameServer.Models
{
    public class GameRoom
    {
        private string _matchId;
        private SessionManager _sessionManager;
        private RoomsManager _roomManager;
        private IRandomizerService _randomizerService;
        private IDateTimeService _dateTimeService;

        private bool _isRoomActive = false;
        private bool _isDestroyThread = false;
        private int _moveCounter = 0;
        private int _turnIndex = 0;
        private int _turnTime = 30;
        private int _timeOutTime = 360;
        private RoomTime roomTime;

        private List<string> _playersOrder;
        private Dictionary<string,User> _users;

        public GameRoom(string matchId, SessionManager sessionManager, RoomsManager roomManager,
           IRandomizerService randomizerService,IDateTimeService dateTimeService, 
           MatchData matchData)
        {
            _matchId = matchId;
            _sessionManager = sessionManager;
            _roomManager = roomManager;
            _randomizerService = randomizerService;
            _dateTimeService = dateTimeService;
            _isRoomActive = false;
            _moveCounter = 0;
            _turnIndex = 0;
            roomTime = new RoomTime(_turnTime, _timeOutTime);

            _playersOrder = new List<string>();
            _users = new Dictionary<string, User>();

            foreach(string userId in matchData.PlayersData.Keys)
            {
                User user = sessionManager.GetUser(userId);
                if (user != null)
                {
                    user.CurUserState = User.UserState.Playing;
                    user.MatchId = _matchId;
                    _sessionManager.UpdateUser(user);

                    _playersOrder.Add(userId);
                    _users.Add(userId, user);
                }
            }
        }

        #region Requests
        public Dictionary<string,object> ReceivedMove(User curUser,string boardIndex)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            if (_playersOrder[_turnIndex] == curUser.UserId)
            {
                PassTurn();
                response = new Dictionary<string, object>()
                {
                    {"Service","BroadcastMove" },
                    {"SenderId",curUser.UserId},
                    {"Index", boardIndex},
                    {"CP",_playersOrder[_turnIndex] },
                    {"MC", _moveCounter }
                };

                string toSend = JsonConvert.SerializeObject(response);
                BroadcastToRoom(toSend);
            }
            else response.Add("ErrorCode", GlobalEnums.ErrorCodes.NotPlayerTurn);

            return response;
        }

        public Dictionary<string, object> StopGame(User user, string winner)
        {
            CloseRoom();
            Dictionary<string, object> response = new Dictionary<string, object>()
            {
                {"Service","StopGame"},
                {"Winner",winner}
            };

            string toSend = JsonConvert.SerializeObject(response);
            BroadcastToRoom(toSend);

            return response;
        }
        #endregion

        #region Logic
        public void StartGame()
        {
            _turnIndex = _randomizerService.GetRandomNumber(0, 1);

            Dictionary<string, object> sendData = new Dictionary<string, object>()
            {
                { "Service","StartGame"},
                { "MI",_matchId},
                { "TT",_dateTimeService.GetUtcTime()},
                { "MTT",_turnTime},
                { "CP",_playersOrder[_turnIndex]},
                { "Players",_playersOrder},
                { "MC",_moveCounter}
            };

            string toSend = JsonConvert.SerializeObject(sendData);
            BroadcastToRoom(toSend);

            _isRoomActive = true;
            _isDestroyThread = true;
            roomTime.ResetTimer();
        }

        private void PassTurn()
        {
            _moveCounter++;
            _turnIndex = _turnIndex == 0 ? 1 : 0;
        }

        private void CloseRoom()
        {
            Console.WriteLine("Closed Room " + DateTime.UtcNow.ToShortTimeString());
           _isRoomActive = false;
            _roomManager.RemoveRoom(_matchId);
        }

        #endregion

        private void BroadcastToRoom(string toSend)
        {
            foreach (string userId in _users.Keys)
                _users[userId].SendMessage(toSend);
        }

    }
}

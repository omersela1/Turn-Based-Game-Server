using Newtonsoft.Json;
using System;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;

namespace TicTacToeGameServer.Models
{
    public class GameRoom
    {
        private string _matchId;

        private string _roomName;
        public string Name { get => _roomName; set => _roomName = value; }

        private string _owner;

        public string Owner { get => _owner; set => _owner = value; }

        private int _maxUsers;

        public int MaxUsersCount { get => _maxUsers; set => _maxUsers = value; }

        private string _password;

        public string Password { get => _password; set => _password = value; }
        private SessionManager _sessionManager;
        private RoomsManager _roomManager;
        private IRandomizerService _randomizerService;
        private IDateTimeService _dateTimeService;

        private bool _isRoomActive = false;

        public bool IsRoomActive { get => _isRoomActive; }
        private bool _isDestroyThread = false;
        private int _moveCounter = 0;
        private int _turnIndex = 0;
        private int _turnTime = 30;

        public int TurnTime { get => _turnTime; }
        private int _timeOutTime = 360;
        private RoomTime roomTime;

        private List<string> _playersOrder;
        private Dictionary<string,User> _users;

        public Dictionary<string, User> Users { get => _users; }

        public int JoinedUsersCount { get => _users.Count; }

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
               JoinRoom(userId);
            }
        }

        public void JoinRoom(string userId)
        {
            User user = _sessionManager.GetUser(userId);
            if (user != null && !_users.ContainsKey(userId))
            {
                user.CurUserState = User.UserState.Playing;
                user.MatchId = _matchId;
                _sessionManager.UpdateUser(user);

                _playersOrder.Add(userId);
                _users.Add(userId, user);
            }
            if (user != null && _users.ContainsKey(userId))
                Console.WriteLine("User already in room");
        }

        public void SubscribeRoom(string userId)
        {
            // User user = _sessionManager.GetUser(userId);
            // if (user != null && !_users.ContainsKey(userId))
            // {
            //     user.CurUserState = User.UserState.Idle;
            //     user.MatchId = _matchId;
            //     _sessionManager.UpdateUser(user);

            //     _playersOrder.Add(userId);
            //     _users.Add(userId, user);
            // }
        }

        public void LeaveRoom(string userId)
        {
            User user = _sessionManager.GetUser(userId);
            if (user != null)
            {
                user.CurUserState = User.UserState.Idle;
                user.MatchId = "";
                _sessionManager.UpdateUser(user);

                _playersOrder.Remove(userId);
                _users.Remove(userId);
            }
        }

        public Dictionary<string, object> ConvertToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "RoomId", _matchId },
                { "IsRoomActive", _isRoomActive },
                { "Name", Name },
                { "Owner", Owner },
                { "Password", Password },
                { "MaxUsersCount", MaxUsersCount },
                { "JoinedUsersCount", JoinedUsersCount },
                { "MoveCounter", _moveCounter },
                { "TurnIndex", _turnIndex },
                { "TurnTime", _turnTime },
                { "TimeOutTime", _timeOutTime },
                { "PlayersOrder", _playersOrder },
                { "Users", _users.Keys.Cast<object>().ToList() }
            };
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
                { "Service","StartGame" },
                { "MI",_matchId },
                { "TT",_dateTimeService.GetUtcTime() },
                { "MTT",_turnTime },
                { "CP",_playersOrder[_turnIndex] },
                { "Players",_playersOrder },
                { "MC",_moveCounter }
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

        public bool IsUserInRoom(string userId)
        {
            return _users.ContainsKey(userId);
        }

    }
}

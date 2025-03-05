using Newtonsoft.Json;
using System;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Managers;

namespace TicTacToeGameServer.Models {
    public class LocalGameServerData {

        private readonly SessionManager _sessionManager;

        private readonly RoomsManager _roomManager;
        private int _connectionsOpened;

        private int _playersActive;

        private int _gamesActive;

        private int _usersMatching;

        private DateTime _serverStartTime;

        private DateTime _lastPlayerConnectionTime;

        public LocalGameServerData(SessionManager sessionManager, RoomsManager roomManager) {
            _sessionManager = sessionManager;
            _roomManager = roomManager;
            _connectionsOpened = 0;
            _playersActive = 0;
            _gamesActive = 0;
            _usersMatching = 0;
            _serverStartTime = DateTime.Now;
            _lastPlayerConnectionTime = DateTime.Now;
        }

        public Dictionary<string, string> GetData() {
            _connectionsOpened = _sessionManager.GetAllConnectionsCount();
            _gamesActive = _roomManager.GetRoomsCount();
            _playersActive = _sessionManager.GetActiveUsersCount();
            _usersMatching = _sessionManager.GetMatchingUsersCount();
            _lastPlayerConnectionTime = _sessionManager.GetLatestMatchingDate();

            return new Dictionary<string, string> {
                { "connectionsOpened", _connectionsOpened.ToString() },
                { "gamesActive", _gamesActive.ToString() },
                { "playersActive", _playersActive.ToString() },
                { "lastPlayerConnectionTime", _lastPlayerConnectionTime.ToString() },
                { "serverRunningTime", (DateTime.Now - _serverStartTime).ToString() },
                 { "usersMatching", _usersMatching.ToString() },
            };
        }
    }
}
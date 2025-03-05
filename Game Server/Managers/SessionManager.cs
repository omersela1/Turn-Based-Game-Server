using TicTacToeGameServer.Models;

namespace TicTacToeGameServer.Managers
{
    public class SessionManager
    {
        private Dictionary<string, User> _userSessions;

        public SessionManager()
        {
            _userSessions = new Dictionary<string, User>();
        }

        public void AddUser(User user)
        {
            if (user == null)
                return;

            if (_userSessions.ContainsKey(user.UserId))
            {
                User storedUser = _userSessions[user.UserId];
                if (storedUser.GetConnectionState() == WebSocketSharp.WebSocketState.Open)
                    storedUser.CloseConnection(User.CloseCode.DuplicateConnection,"Closed Old Connection");

                _userSessions.Remove(user.Session.ID);
                _userSessions[user.UserId] = user;
            }
            else _userSessions.Add(user.UserId, user);

            if (_userSessions.ContainsKey(user.Session.ID))
                _userSessions[user.Session.ID] = user;
            else _userSessions.Add(user.Session.ID, user);
        }

        public User GetUser(string Id)
        {
            if(_userSessions != null && _userSessions.ContainsKey(Id))
                return _userSessions[Id];
            return null;
        }

        public void UpdateUser(User user)
        {
            if (user == null)
                return;

            if (_userSessions.ContainsKey(user.UserId))
                _userSessions[user.UserId] = user;

            if (_userSessions.ContainsKey(user.Session.ID))
                _userSessions[user.Session.ID] = user;
        }

        public int GetAllConnectionsCount()
        {
            return _userSessions.Count;
        }

        public int GetActiveUsersCount()
        {
            int count = 0;
            foreach (var user in _userSessions.Values)
            {
                if (user.IsLive())
                    count++;
            }
            return count;
        }

        public int GetMatchingUsersCount()
        {
            int count = 0;
            foreach (var user in _userSessions.Values)
            {
                if (user.CurUserState == User.UserState.Matching)
                    count++;
            }
            return count;
        }

        public DateTime GetLatestMatchingDate()
        {
            DateTime latestMatchingDate = DateTime.MinValue; // Set initial value to the smallest possible DateTime

            foreach (var user in _userSessions.Values)
            {
             // Check if the user is active (you can use your `IsLive` method to determine this)
             if (user.IsLive())
                {
                 // Update the latestMatchingDate if this user's MatchingDate is more recent
                     if (user.MatchingDate > latestMatchingDate)
                    {
                        latestMatchingDate = user.MatchingDate;
                    }
                }
            }
            
            return latestMatchingDate;
         }

    }

}


  

using TicTacToeGameServer.Models;

namespace TicTacToeGameServer.Managers {
    public class LocalDataManager {
        private readonly LocalGameServerData _localGameServerData;

        public LocalDataManager(LocalGameServerData localGameServerData) {
            _localGameServerData = localGameServerData;
        }

        public Dictionary<string, string> GetLocalData() {
            return _localGameServerData.GetData();
        }
    }
}
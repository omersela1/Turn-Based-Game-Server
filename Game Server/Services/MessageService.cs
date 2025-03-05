using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Models;

namespace TicTacToeGameServer.Services
{
    public class MessageService : IMessageService
    {
        private readonly IReadyToPlayService _readyToPlayService;
        private readonly ISendMoveRequest _sendMoveRequest;
        private readonly IStopGameRequest _stopGameRequest;

        public MessageService(IReadyToPlayService readyToPlayService, 
            ISendMoveRequest sendMoveRequest, IStopGameRequest stopGameRequest) 
        {
            _readyToPlayService = readyToPlayService;
            _sendMoveRequest = sendMoveRequest;
            _stopGameRequest =  stopGameRequest;
        }

        public async Task<object> HandleMessageAsync(User curUser, string data)
        {
            Dictionary<string,object> response = new Dictionary<string,object>();
            try
            {
                if(curUser != null)
                {
                    Dictionary<string,object> msgData = JsonConvert.DeserializeObject<Dictionary<string,object>>(data);
                    if(msgData.ContainsKey("Service"))
                    {
                        string service = msgData["Service"].ToString();
                        switch(service)
                        {
                            case "ReadyToPlay":_readyToPlayService.Get(curUser, msgData);break;
                            case "SendMove":_sendMoveRequest.Get(curUser, msgData); break;
                            case "StopGame":_stopGameRequest.Get(curUser, msgData); break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            if(response.Count > 0)
            {
                string retData = JsonConvert.SerializeObject(response);
                curUser.SendMessage(retData);
            }

            await Task.Delay(500);

            return null;
        }
    }
}

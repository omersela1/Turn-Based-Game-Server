using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Models;
using TicTacToeGameServer.Services.ClientRequests;

namespace TicTacToeGameServer.Services
{
    public class MessageService : IMessageService
    {
        private readonly IReadyToPlayService _readyToPlayService;
        private readonly ISendMoveRequest _sendMoveRequest;
        private readonly IStopGameRequest _stopGameRequest;

        private readonly IDictionary<string, IServiceHandler> _services;


        public MessageService(IReadyToPlayService readyToPlayService, 
            ISendMoveRequest sendMoveRequest, IStopGameRequest stopGameRequest, IDictionary<string, IServiceHandler> services) 
        {
            _readyToPlayService = readyToPlayService;
            _sendMoveRequest = sendMoveRequest;
            _stopGameRequest =  stopGameRequest;
            _services = services;
            
        }

        public async Task<object> HandleMessageAsync(User curUser, string data)
        {
            Console.WriteLine("MessageService: HandleMessageAsync");
            try
            {
                if(curUser != null)
                {
                    Dictionary<string,object> msgData = JsonConvert.DeserializeObject<Dictionary<string,object>>(data);
                    if(msgData.ContainsKey("Service"))
                    {
                        string service = msgData["Service"].ToString();
                        if(_services.ContainsKey(service))
                        {
                           Dictionary<string,object> response = new Dictionary<string,object>
                                {
                                    {"Response", service },
                                    {"Rooms", (Dictionary<string,object>)_services[service].Handle(curUser, msgData)}
                                };
                                if (response.Count > 0)
                                {
                                    string retData = JsonConvert.SerializeObject(response);
                                    return retData;
                                }
                        }   
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }


            await Task.Delay(500);

            return null;
        }
    }
}

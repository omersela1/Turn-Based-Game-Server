using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using TicTacToeGameServer.Interfaces;
using TicTacToeGameServer.Models;
using TicTacToeGameServer.Services.ClientRequests;
using TicTacToeGameServer.Services.SerializationResolution;

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
            Console.WriteLine("Data: " + data);
            try
            {
                if(curUser != null)
                {
                    if (string.IsNullOrEmpty(data))
                    {
                        Console.WriteLine("Data is null");
                        return null;
                    }
                    Dictionary<string,object> msgData = JsonConvert.DeserializeObject<Dictionary<string,object>>(data);
                    if(msgData.ContainsKey("Service"))
                    {
                        string service = msgData["Service"].ToString();
                        Console.WriteLine("Required service: " + service);
                        if(_services.ContainsKey(service))
                        {
                           var serviceResponse = _services[service].Handle(curUser, msgData);
                           if (serviceResponse != null) {
                            Dictionary<string,object> response = new Dictionary<string,object>();
                            response.Add("Response", service);
                                switch (service) {
                                    case "GetRoomsInRange":
                                      response.Add("Rooms", serviceResponse);
                                      break;
                                    case "GetLiveRoomInfo":
                                        Dictionary<string, object> roomData = (Dictionary<string, object>)serviceResponse[0]["RoomData"];
                                        response.Add("RoomData", roomData);
                                        response.Add("RoomProperties", serviceResponse[0]["RoomProperties"]);
                                        response.Add("Users", roomData["Users"]);
                                        break;
                                    case "CreateTurnRoom":
                                        response.Add("RoomId", serviceResponse[0]["RoomId"]);
                                        response.Add("IsSuccess", serviceResponse[0]["isSuccess"]);
                                        break;

                                    case "JoinRoom":
                                        response.Add("RoomId", serviceResponse[0]["RoomId"]);
                                        response.Add("IsSuccess", serviceResponse[0]["isSuccess"]);
                                        response.Add("UserId", serviceResponse[0]["Sender"]);
                                        break;
                                    case "SubscribeRoom":
                                        response.Add("RoomId", serviceResponse[0]["RoomId"]);
                                        response.Add("IsSuccess", serviceResponse[0]["isSuccess"]);
                                        break;
                                    case "StartGame":
                                        break;
                                        default:
                                        break;
                                }
                                if (response.Count > 0)
                                {
                                    Console.WriteLine("Response received from service: " + service);
                                    // JsonSerializerSettings settings = new JsonSerializerSettings();
                                    // settings.Converters.Add(new CustomJsonConverter());
                                    string retData = JsonConvert.SerializeObject(response); // add settings as second parameter if needed
                                    return retData;
                                }
                                }
                           else
                           {
                               Console.WriteLine(service + " response is null");
                               return null;
                           }
                           }
                        }   
                    }
                Console.WriteLine("User is null");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }


            await Task.Delay(500);

            return null;
        }
    }
}

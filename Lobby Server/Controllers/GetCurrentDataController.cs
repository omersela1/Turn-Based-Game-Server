using TicTacToeLobbyServer.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToeLobbyServer.Controllers {

    [Route("api/")]
    [ApiController]
    public class GetCurrentDataController : ControllerBase {

        private HttpClient _httpClient;
        private string _gameServerApiConnectionString;

       public GetCurrentDataController(IConfiguration configuration) {
            _httpClient = new HttpClient();
            _gameServerApiConnectionString = configuration["GameServerHttpApi:ConnectionString"].ToString();
       }

       [HttpGet("GetCurrentData")]
       public async Task<ActionResult<Dictionary<string, string>>> GetCurrentData()
    {
        try
        {
            // Make a request to the Game Server API
            var response = await _httpClient.GetStringAsync(_gameServerApiConnectionString + "/GetData");

            // Deserialize the response into a Dictionary
            var gameData = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

            return Ok(gameData); // Return JSON response
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message); // Return 500 Internal Server Error
        }
    }
    }
}
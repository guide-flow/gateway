using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Gateway.Controllers.IdentityController
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly HttpClient _client;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Identity");
        }

        // POST: api/identity/authentication/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] object registrationCreds)
        {
            var json = JsonSerializer.Serialize(registrationCreds, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authentication/register", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return Content(responseContent, "application/json");
        }

        // POST: api/identity/authentication/authenticate
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] object authenticationRequest)
        {
            var json = JsonSerializer.Serialize(authenticationRequest, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authentication/authenticate", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return Content(responseContent, "application/json");
        }
    }
}

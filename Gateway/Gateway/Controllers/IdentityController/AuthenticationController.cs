using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.IdentityController
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly HttpClient _client;

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Identity");
        }

        // POST: api/identity/authentication/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] object registrationCreds)
        {
            var response = await _client.PostAsJsonAsync("/api/authentication/register", registrationCreds);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // POST: api/identity/authentication/authenticate
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] object authenticationRequest)
        {
            var response = await _client.PostAsJsonAsync("/api/authentication/authenticate", authenticationRequest);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}

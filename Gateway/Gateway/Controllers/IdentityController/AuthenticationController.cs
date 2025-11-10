using Microsoft.AspNetCore.Mvc;
using System.Text;

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
        public async Task<IActionResult> Register()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authentication/register", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return Ok(await response.Content.ReadFromJsonAsync<object>());

            return StatusCode((int)response.StatusCode, responseContent);
        }

        // POST: api/identity/authentication/authenticate
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/authentication/authenticate", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return Ok(await response.Content.ReadFromJsonAsync<object>());

            return StatusCode((int)response.StatusCode, responseContent);
        }
    }
}

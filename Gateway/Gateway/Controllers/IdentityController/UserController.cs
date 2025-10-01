using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.IdentityController
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly HttpClient _client;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Identity");
        }

        [HttpGet()]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _client.GetAsync("/api/users");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // PUT: api/identity/users/block/{id}
        [HttpPut("block/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var response = await _client.PutAsync($"/api/users/block/{id}", null);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}

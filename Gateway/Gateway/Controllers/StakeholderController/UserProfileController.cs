using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.StakeholderController
{
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly HttpClient _client;

        public UserProfileController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Stakeholders");
        }

        // GET: api/stakeholders/admin
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminProfile()
        {
            var response = await _client.GetAsync("/userprofile/admin");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // POST: api/stakeholders/create-user-profile
        [HttpPost("create-user-profile")]
        public async Task<IActionResult> CreateUserProfile([FromBody] object userProfileDto)
        {
            var response = await _client.PostAsJsonAsync("/userprofile/create-user-profile", userProfileDto);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/stakeholders/user-profile
        [HttpGet("user-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var response = await _client.GetAsync("/userprofile/user-profile");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // PUT: api/stakeholders/user-profile
        [HttpPut("user-profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] object userProfileDto)
        {
            var response = await _client.PutAsJsonAsync("/userprofile/user-profile", userProfileDto);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}

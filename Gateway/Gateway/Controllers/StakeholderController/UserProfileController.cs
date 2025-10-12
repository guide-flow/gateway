using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.StakeholderController
{
    [ApiController]
    [Route("api/user-profiles")]
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
            var request = new HttpRequestMessage(HttpMethod.Post, "/userprofile/create-user-profile")
            {
                Content = JsonContent.Create(userProfileDto)
            };

            // prosledi claim headere dalje
            foreach (var header in HttpContext.Request.Headers)
            {
                if (header.Key.StartsWith("X-User-"))
                    request.Headers.Add(header.Key, header.Value.ToString());
            }

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/stakeholders/user-profile
        [HttpGet("user-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/userprofile/user-profile");

            // ubaci claim headere koje je middleware stavio
            foreach (var header in HttpContext.Request.Headers)
            {
                if (header.Key.StartsWith("X-User-"))
                    request.Headers.Add(header.Key, header.Value.ToString());
            }

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }

        // PUT: api/stakeholders/user-profile
        [HttpPut("user-profile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] object userProfileDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "/userprofile/user-profile")
            {
                Content = JsonContent.Create(userProfileDto)
            };

            // ubaci claim headere koje je middleware popunio
            foreach (var header in HttpContext.Request.Headers)
            {
                if (header.Key.StartsWith("X-User-"))
                    request.Headers.Add(header.Key, header.Value.ToString());
            }

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, "application/json");
        }
    }
}

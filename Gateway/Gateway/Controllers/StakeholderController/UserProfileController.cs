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

        [HttpGet("user-profile/{userId}")]
        public async Task<IActionResult> GetUserProfileById(string userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/userprofile/user-profile/{userId}");
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

        // GET: api/user-profiles/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUserProfiles()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/userprofile/all");
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

        // GET: api/user-profiles/images/{**path} - Proxy for static images
        [HttpGet("images/{**path}")]
        public async Task<IActionResult> GetImage(string path)
        {
            var response = await _client.GetAsync($"/images/{path}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
            var bytes = await response.Content.ReadAsByteArrayAsync();

            return File(bytes, contentType);
        }
    }
}

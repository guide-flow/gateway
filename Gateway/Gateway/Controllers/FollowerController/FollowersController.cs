using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.FollowerController
{
    [ApiController]
    [Route("api/followers")]
    public class FollowersController : ControllerBase
    {
        private readonly HttpClient _client;

        public FollowersController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Followers");
        }

        // POST: api/followers/follow
        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] object userDto)
        {
            var response = await _client.PostAsJsonAsync("/follow/follow", userDto);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // DELETE: api/followers/unfollow/{targetId}
        [HttpDelete("unfollow/{targetId}")]
        public async Task<IActionResult> Unfollow(string targetId)
        {
            var response = await _client.DeleteAsync($"/follow/unfollow/{targetId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return NoContent();
            return Content(content, "application/json");
        }

        // GET: api/followers/following
        [HttpGet("following/{id}")]
        public async Task<IActionResult> GetFollowing(string id)
        {
            var response = await _client.GetAsync($"/follow/following/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/followers/followers
        [HttpGet("followers/{id}")]
        public async Task<IActionResult> GetFollowers(string id)
        {
            var response = await _client.GetAsync($"/follow/followers/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/followers/recommendations
        [HttpGet("recommendations/{id}")]
        public async Task<IActionResult> GetRecommendations(string id)
        {
            var response = await _client.GetAsync($"/follow/recommendations/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}

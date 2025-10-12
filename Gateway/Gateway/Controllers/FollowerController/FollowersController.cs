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
        [HttpGet("following")]
        public async Task<IActionResult> GetFollowing()
        {
            var response = await _client.GetAsync("/follow/following");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/followers/followers
        [HttpGet("followers")]
        public async Task<IActionResult> GetFollowers()
        {
            var response = await _client.GetAsync("/follow/followers");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/followers/recommendations
        [HttpGet("recommendations")]
        public async Task<IActionResult> GetRecommendations()
        {
            var response = await _client.GetAsync("/follow/recommendations");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}

using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.TourController
{
    [ApiController]
    [Route("api/checkpoints")]
    public class CheckpointController : ControllerBase
    {
        private readonly HttpClient _client;

        public CheckpointController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Tours");
        }

        private void AddUserHeaders(HttpRequestMessage req)
        {
            foreach (var header in HttpContext.Request.Headers)
            {
                if (header.Key.StartsWith("X-User-", StringComparison.OrdinalIgnoreCase))
                    req.Headers.TryAddWithoutValidation(header.Key, header.Value.ToString());
            }
        }

        // POST: api/checkpoints
        [HttpPost]
        public async Task<IActionResult> CreateCheckpoint([FromBody] object checkpointDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/checkpoints")
            {
                Content = JsonContent.Create(checkpointDto)
            };
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/checkpoints/tour-checkpoints/{tourId}
        [HttpGet("tour-checkpoints/{tourId}")]
        public async Task<IActionResult> GetTourCheckpoints(int tourId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/checkpoints/tour-checkpoints/{tourId}");
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/checkpoints/{checkpointId}
        [HttpGet("{checkpointId}")]
        public async Task<IActionResult> GetCheckpoint(int checkpointId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/checkpoints/checkpoint/{checkpointId}");
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // DELETE: api/checkpoints/{checkpointId}
        [HttpDelete("{checkpointId}")]
        public async Task<IActionResult> DeleteCheckpoint(int checkpointId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/checkpoints/{checkpointId}");
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            // može i Ok(content), ali radi konzistentnosti vraćam JSON content
            return Content(content, "application/json");
        }

        // PUT: api/checkpoints
        [HttpPut]
        public async Task<IActionResult> UpdateCheckpoint([FromBody] object checkpointDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "api/checkpoints")
            {
                Content = JsonContent.Create(checkpointDto)
            };
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}

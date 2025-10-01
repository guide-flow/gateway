using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.TourController
{
    [ApiController]
    [Route("[controller]")]
    public class CheckpointController : ControllerBase
    {
        private readonly HttpClient _client;

        public CheckpointController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Tours");
        }

        // POST: api/checkpoints
        [HttpPost]
        public async Task<IActionResult> CreateCheckpoint([FromBody] object checkpointDto)
        {
            var response = await _client.PostAsJsonAsync("/checkpoint", checkpointDto);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/checkpoints/tour-checkpoints/{tourId}
        [HttpGet("tour-checkpoints/{tourId}")]
        public async Task<IActionResult> GetTourCheckpoints(int tourId)
        {
            var response = await _client.GetAsync($"/checkpoint/tour-checkpoints/{tourId}");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/checkpoints/{checkpointId}
        [HttpGet("{checkpointId}")]
        public async Task<IActionResult> GetCheckpoint(int checkpointId)
        {
            var response = await _client.GetAsync($"/checkpoint/checkpoint/{checkpointId}");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
        // DELETE: api/checkpoints/{checkpointId}
        [HttpDelete("{checkpointId}")]
        public async Task<IActionResult> DeleteCheckpoint(int checkpointId)
        {
            var response = await _client.DeleteAsync($"/checkpoint?checkpointId={checkpointId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return Ok(content);
            return Content(content, "application/json");
        }

        // PUT: api/checkpoints
        [HttpPut]
        public async Task<IActionResult> UpdateCheckpoint([FromBody] object checkpointDto)
        {
            var response = await _client.PutAsJsonAsync("/checkpoint", checkpointDto);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}

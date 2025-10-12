using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.TourController
{
    [ApiController]
    [Route("api/tours")]
    public class TourController : ControllerBase
    {
        private readonly HttpClient _client;

        public TourController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("Tours");
        }

        // POST: api/tours
        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] object tourDto)
        {
            var response = await _client.PostAsJsonAsync("api/tours", tourDto);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/tours/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTourById(int id)
        {
            var response = await _client.GetAsync($"api/tours/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/tours/author
        [HttpGet("author")]
        public async Task<IActionResult> GetToursByAuthor()
        {
            var response = await _client.GetAsync("api/tours/author");
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // PUT: api/tours/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTour(int id, [FromBody] object tourDto)
        {
            var response = await _client.PutAsJsonAsync($"api/tours/{id}", tourDto);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}

using System.Net.Http;
using System.Net.Http.Json;
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

        private void AddUserHeaders(HttpRequestMessage req)
        {
            foreach (var header in HttpContext.Request.Headers)
            {
                if (header.Key.StartsWith("X-User-", StringComparison.OrdinalIgnoreCase))
                    req.Headers.TryAddWithoutValidation(header.Key, header.Value.ToString());
            }
        }

        // POST: api/tours
        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] object tourDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/tours")
            {
                Content = JsonContent.Create(tourDto)
            };
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/tours/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTourById(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/tours/{id}");
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // GET: api/tours/author
        [HttpGet("author")]
        public async Task<IActionResult> GetToursByAuthor()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/tours/author");
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // PUT: api/tours/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTour(int id, [FromBody] object tourDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/tours/{id}")
            {
                Content = JsonContent.Create(tourDto)
            };
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/tours/{id}");
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // PUT: api/tours/tour-metrics/{id}
        [HttpPut("tour-metrics/{id}")]
        public async Task<IActionResult> UpdateTourMetrics(int id, [FromBody] object tourMetrics)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/tours/tour-metrics/{id}")
            {
                Content = JsonContent.Create(tourMetrics)
            };
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        // PUT: api/tours/tour-status/{id}
        [HttpPut("tour-status/{id}")]
        public async Task<IActionResult> UpdateTourStatus(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/tours/tour-status/{id}")
            {
                Content = JsonContent.Create(new { })
            };
            AddUserHeaders(request);

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}

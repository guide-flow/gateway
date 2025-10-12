using System.Net.Http.Headers;

namespace Gateway.Middlewares
{
    public class IdentityValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityValidationMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            var isStartingPath = path?.StartsWith("/");

            if (isStartingPath.HasValue && !isStartingPath.Value)
            {
                await _next(context);
                return;
            }

            if (path != null &&
                (path.Contains("/api/authentication/register") ||
                path.Contains("/api/authentication/authenticate")))
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing Authorization header");
                return;
            }

            // napravi poziv ka Identity servisu da proveri token
            var client = _httpClientFactory.CreateClient("Identity");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", ""));

            var response = await client.GetAsync("/api/authentication/validate-token"); // napravi endpoint u Identity servisu
            if (!response.IsSuccessStatusCode)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token");
                return;
            }

            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            // ubacujemo claimove kao headere
            if (content != null)
            {
                if (content.TryGetValue("sub", out var userId))
                    context.Request.Headers["X-User-Id"] = userId;

                if (content.TryGetValue("email", out var email))
                    context.Request.Headers["X-User-Email"] = email;

                if (content.TryGetValue("role", out var role))
                    context.Request.Headers["X-User-Role"] = role;
            }

            // ako token valja -> pusti dalje
            await _next(context);
        }
    }
}

using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTranscoding.Tours;
using System;

namespace Gateway.ProtoControllers
{
    public class TourProtoController : ToursService.ToursServiceBase
    {
        private readonly ILogger<TourProtoController> _logger;
        private readonly ToursService.ToursServiceClient _client;

        public TourProtoController(
            ILogger<TourProtoController> logger,
            ToursService.ToursServiceClient client)
        {
            _logger = logger;
            _client = client;
        }

        public override async Task<Tour> CreateTour(CreateUpdateTour request, ServerCallContext context)
        {
            var httpCtx = context.GetHttpContext();
            var meta = new Metadata();

            if (httpCtx.Request.Headers.TryGetValue("X-User-Id", out var id)) meta.Add("x-user-id", id.ToString());
            if (httpCtx.Request.Headers.TryGetValue("X-User-Email", out var email)) meta.Add("x-user-email", email.ToString());
            if (httpCtx.Request.Headers.TryGetValue("X-User-Role", out var role)) meta.Add("x-user-role", role.ToString());

            _logger.LogInformation($"Request:{request}");
            try
            {
                var resp = await _client.CreateTourAsync(request, headers: meta);
                return resp;
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "CreateTour downstream call failed: {Status} {Detail}", ex.Status, ex.Status.Detail);
                throw;
            }
        }
    }
}

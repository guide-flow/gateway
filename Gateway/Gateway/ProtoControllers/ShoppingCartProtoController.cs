using Gateway.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;

namespace Gateway.ProtoControllers;

public class ShoppingCartProtoController : ShoppingCartService.ShoppingCartServiceBase
{
    private readonly ILogger<ShoppingCartProtoController> logger;

    public ShoppingCartProtoController(ILogger<ShoppingCartProtoController> logger)
    {
        this.logger = logger;
    }

    public override async Task<ShoppingCartItem> AddToCart(ItemCreation request, ServerCallContext context)
    {
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress("https://localhost:7029", new GrpcChannelOptions { HttpHandler = httpHandler });

        var httpCtx = context.GetHttpContext();
        var meta = new Metadata();

        if (httpCtx.Request.Headers.TryGetValue("X-User-Id", out var id)) meta.Add("x-user-id", id.ToString());
        if (httpCtx.Request.Headers.TryGetValue("X-User-Email", out var email)) meta.Add("x-user-email", email.ToString());
        if (httpCtx.Request.Headers.TryGetValue("X-User-Role", out var role)) meta.Add("x-user-role", role.ToString());

        var client = new ShoppingCartService.ShoppingCartServiceClient(channel);
        var response = await client.AddToCartAsync(request, headers: meta);
        return response;
    }

    public override async Task<Empty> RemoveFromCart(ShoppingCartItemId request, ServerCallContext context)
    {
        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress("https://localhost:7029", new GrpcChannelOptions { HttpHandler = httpHandler });

        var httpCtx = context.GetHttpContext();
        var meta = new Metadata();

        if (httpCtx.Request.Headers.TryGetValue("X-User-Id", out var id)) meta.Add("x-user-id", id.ToString());
        if (httpCtx.Request.Headers.TryGetValue("X-User-Email", out var email)) meta.Add("x-user-email", email.ToString());
        if (httpCtx.Request.Headers.TryGetValue("X-User-Role", out var role)) meta.Add("x-user-role", role.ToString());

        var client = new ShoppingCartService.ShoppingCartServiceClient(channel);
        var response = await client.RemoveFromCartAsync(request, headers: meta);
        return response;
    }
}

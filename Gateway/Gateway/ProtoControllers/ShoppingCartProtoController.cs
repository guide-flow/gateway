using Gateway.Protos;
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


        var client = new ShoppingCartService.ShoppingCartServiceClient(channel);
        var response = await client.AddToCartAsync(request);
        return response;
    }
}

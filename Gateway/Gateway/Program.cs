using DotNetEnv;
using Gateway.Handlers;
using Gateway.Middlewares;
using Gateway.ProtoControllers;
using Gateway.Startup;
using GrpcServiceTranscoding.Tours;

// Enable HTTP/2 without TLS for gRPC clients (required for non-HTTPS gRPC)
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
if (builder.Environment.IsDevelopment())
{
    Env.Load();
}
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthForwardingHandler>();
builder.Services.AddHttpClient("Stakeholders", c =>
{
    c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("STAKEHOLDER_URL")!);
}).AddHttpMessageHandler<AuthForwardingHandler>();
builder.Services.AddHttpClient("Identity", c =>
{
    c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("IDENTITY_URL")!);
})
.AddHttpMessageHandler<AuthForwardingHandler>()
.ConfigureHttpClient(client =>
{
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddHttpClient("Followers", c =>
{
    c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("FOLLOWER_URL")!);
}).AddHttpMessageHandler<AuthForwardingHandler>();
builder.Services.AddHttpClient("Tours", c =>
{
    c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("TOUR_HTTP_URL")!);
}).AddHttpMessageHandler<AuthForwardingHandler>();

builder.Services.ConfigureCors();

builder.Services.AddGrpc().AddJsonTranscoding();

builder.Services
  .AddGrpcClient<ToursService.ToursServiceClient>(o =>
  {
      o.Address = new Uri(Environment.GetEnvironmentVariable("TOUR_HTTPS_URL") ?? "https://tour:7029");
  })
  .ConfigurePrimaryHttpMessageHandler(() =>
  {
      var h = new HttpClientHandler();
      h.ServerCertificateCustomValidationCallback =
          HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
      return h;
  });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("_allowDevClients");

app.UseMiddleware<IdentityValidationMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.MapGrpcService<TourProtoController>();

app.MapGrpcService<ShoppingCartProtoController>();

app.Run();

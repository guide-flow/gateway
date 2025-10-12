using DotNetEnv;
using Gateway.Handlers;
using Gateway.Middlewares;
using Gateway.ProtoControllers;
using Gateway.Startup;
using GrpcServiceTranscoding.Tours;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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
}).AddHttpMessageHandler<AuthForwardingHandler>();
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

app.UseAuthorization();
app.UseMiddleware<IdentityValidationMiddleware>();
app.MapControllers();

app.MapGrpcService<TourProtoController>();

app.Run();

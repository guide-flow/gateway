using DotNetEnv;
using Gateway.Handlers;
using Gateway.Middlewares;
using Gateway.Startup;

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
builder.Services.AddHttpClient("Stakeholders", c=> { 
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
    c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("TOUR_URL")!);
}).AddHttpMessageHandler<AuthForwardingHandler>();

builder.Services.ConfigureCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<IdentityValidationMiddleware>();
app.MapControllers();

app.Run();

using Gateway.Handlers;
using Gateway.Middlewares;
using Gateway.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthForwardingHandler>();
builder.Services.AddHttpClient("Stakeholders", c=> { 
    c.BaseAddress = new Uri("http://localhost:5227/");
}).AddHttpMessageHandler<AuthForwardingHandler>();
builder.Services.AddHttpClient("Identity", c =>
{
    c.BaseAddress = new Uri("http://localhost:5226/");
}).AddHttpMessageHandler<AuthForwardingHandler>();
builder.Services.AddHttpClient("Followers", c =>
{
    c.BaseAddress = new Uri("http://localhost:5228/");
}).AddHttpMessageHandler<AuthForwardingHandler>();
builder.Services.AddHttpClient("Tours", c =>
{
    c.BaseAddress = new Uri("http://localhost:5229/");
}).AddHttpMessageHandler<AuthForwardingHandler>();

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

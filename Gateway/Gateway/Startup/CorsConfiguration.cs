namespace Gateway.Startup
{
    public static class CorsConfiguration
    {
        private const string _corsPolicyName = "_allowDevClients";
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_corsPolicyName, builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            return services;
        }
    }
}

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
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}

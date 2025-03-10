using Microsoft.Extensions.Options;

namespace Auto1040.Api.Extensions
{
    public static class CorsExtension
    {
        public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public static void AddAllowAnyCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins, builder =>
                {
                     builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                });
            });
        }


    }

}

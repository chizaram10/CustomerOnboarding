using Common.Core.Interfaces.Services;
using Common.Core.Settings;
using Common.Infrastructure.Data;
using Common.Infrastructure.Implementations.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace CustomerOnboarding.API
{
    public static class ServiceExtension
    {
        public static void ConfigureDI(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IHttpRequestService, HttpRequestService>();
        }

        public static void ConfigureOtherServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<AppSettings>().Bind(config.GetSection("AppSettings"));
            services.AddOptions<TestAPIClient>().Bind(config.GetSection("TestAPIClient"));

            string connStr = config["AppSettings:DbConnectionString"];
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connStr), ServiceLifetime.Transient);
        }

        public static void ConfigureHttpClientFactory(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient(config["TestAPIClient:Name"], c =>
            {
                c.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    }
                });
        }
    }
}

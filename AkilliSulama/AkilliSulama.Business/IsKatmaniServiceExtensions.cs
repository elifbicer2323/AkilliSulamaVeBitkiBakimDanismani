using AkilliSulama.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AkilliSulama.Business;

public static class IsKatmaniServiceExtensions
{
    // Web katmanı sadece bunu çağırır; veri katmanını iş katmanı ayarlar
    public static IServiceCollection AddIsKatmani(this IServiceCollection services, string connectionString)
    {
        services.AddDataKatmani(connectionString);
        return services;
    }
}
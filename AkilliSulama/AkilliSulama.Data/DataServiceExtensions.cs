using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AkilliSulama.Data;

public static class DataServiceExtensions
{
    // Veri katmanının ihtiyaç duyduğu servisleri kaydeder
    public static IServiceCollection AddDataKatmani(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AkilliSulamaDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ParselRepository>();
        return services;
        
    }
}
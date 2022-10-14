using FoodOrderingServices.Repositories.RestaurantDataRepository;
using FoodOrderingServices.Services.RestaurantDataService;

namespace FoodOrderingServices;

public class Startup
{
    private IConfiguration ConfigRoot { get; }

    public Startup(IConfiguration configuration)
    {
        ConfigRoot = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddRazorPages();
        services.AddLogging(config => config.ClearProviders());
        
        services.AddSingleton<IRestaurantDataRepository, RestaurantDataRepository>();
            
        services.AddSingleton<IRestaurantDataService, RestaurantDataService>();
    }

    public static void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
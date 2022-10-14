using System.Collections.Concurrent;

namespace FoodOrderingServices.Repositories.RestaurantDataRepository;

public class RestaurantDataRepository : IRestaurantDataRepository
{
    private ConcurrentBag<RestaurantData> RestaurantData { get; set; }

    public RestaurantDataRepository()
    {
        RestaurantData = new ConcurrentBag<RestaurantData>();
    }

    public Task<ConcurrentBag<RestaurantData>>  GetRestaurantData()
    {
        return Task.FromResult(RestaurantData);
    }

    public Task Insert(RestaurantData restaurantData)
    {
        RestaurantData.Add(restaurantData);
        return Task.CompletedTask;
    }
}
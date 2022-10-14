using System.Collections.Concurrent;
using FoodOrderingServices.Repositories.RestaurantDataRepository;

namespace FoodOrderingServices.Services.RestaurantDataService;

public class RestaurantDataService : IRestaurantDataService
{
    private readonly IRestaurantDataRepository _restaurantDataRepository;

    public RestaurantDataService(IRestaurantDataRepository restaurantDataRepository)
    {
        _restaurantDataRepository = restaurantDataRepository;
    }
    public Task<ConcurrentBag<RestaurantData>> GetRestaurantData()
    {
        return _restaurantDataRepository.GetRestaurantData();
    }

    public Task Insert(RestaurantData restaurantData)
    {
        return _restaurantDataRepository.Insert(restaurantData);
    }
}
using System.Collections.Concurrent;

namespace FoodOrderingServices.Services.RestaurantDataService;

public interface IRestaurantDataService
{
    Task<ConcurrentBag<RestaurantData>> GetRestaurantData();
    Task Insert(RestaurantData restaurantData);
}
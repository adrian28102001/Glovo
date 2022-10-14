using System.Collections.Concurrent;

namespace FoodOrderingServices.Repositories.RestaurantDataRepository;

public interface IRestaurantDataRepository
{
    Task<ConcurrentBag<RestaurantData>> GetRestaurantData();
    Task Insert(RestaurantData restaurantData);
}
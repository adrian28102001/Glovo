using Client.Models;
using Client.RestaurantDataRepository;

namespace Client.Service.RestaurantDataService;

public class RestaurantDataService  : IRestaurantDataService
{
    private readonly IRestaurantDataRepository _restaurantDataRepository;

    public RestaurantDataService(IRestaurantDataRepository restaurantDataRepository)
    {
        _restaurantDataRepository = restaurantDataRepository;
    }

    public Task<IList<RestaurantData>> GetRestaurantData()
    {
        return _restaurantDataRepository.GetRestaurantData();
    }

    public Task Insert(RestaurantData restaurantData)
    {
        return _restaurantDataRepository.Insert(restaurantData);
    }
}
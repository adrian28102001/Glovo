using Client.Models;

namespace Client.RestaurantDataRepository;

public class RestaurantDataRepository : IRestaurantDataRepository 
{
    private IList<RestaurantData> RestaurantData { get; set; }

    public RestaurantDataRepository()
    {
        RestaurantData = new List<RestaurantData>();
    }

    public Task<IList<RestaurantData>> GetRestaurantData()
    {
        return Task.FromResult(RestaurantData);
    } 
    public Task Insert(RestaurantData restaurantData)
    {
        RestaurantData.Add(restaurantData);
        return Task.CompletedTask;
    } 
}

public interface IRestaurantDataRepository
{
    public Task<IList<RestaurantData>> GetRestaurantData();
    public Task Insert(RestaurantData restaurantData);
}
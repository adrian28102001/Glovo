using System.Net;
using System.Text;
using Client.Helpers;
using Client.Models;
using Client.Service.RestaurantDataService;
using Newtonsoft.Json;

namespace Client.Service.OrderService;

public class OrderService : IOrderService
{
    private readonly IRestaurantDataService _restaurantDataService;

    public OrderService(IRestaurantDataService restaurantDataService)
    {
        _restaurantDataService = restaurantDataService;
    }

    public async Task<ClientOrder> CreateOrder(int clientId)
    {
        var orders = new List<Order>();
        var restaurantDataList = await _restaurantDataService.GetRestaurantData();

        var chooseFoodFromNRestaurants =
            RandomGenerator.ListNumberGenerator(restaurantDataList.Count); //order for max 3 restaurants at a time
        foreach (var restaurantId in chooseFoodFromNRestaurants)
        {
            var restaurant = await _restaurantDataService.GetRestaurantDataById(restaurantId);
            var menu = restaurant.Menu.ToList();
            var randomIdsFoodList = RandomGenerator.ListNumberGenerator(menu.Count).ToList();
            var randomFoodList = GetFoodFromRandomIdsList(menu, randomIdsFoodList);
            orders.Add(new Order
            {
                OrderId = await IdGenerator.GenerateOrderId(),
                RestaurantId = restaurant.RestaurantId,
                ClientId = clientId,
                Foods = randomIdsFoodList,
                Priority = RandomGenerator.NumberGenerator(3),
                MaxWait = MaxWaitingTime.CalculateMaxWaitingTine(randomFoodList),
                CreateOnTime = DateTime.Now
            });
        }

        return new ClientOrder
        {
            ClientId = clientId,
            Orders = orders,
        };
    }

    private static List<Food> GetFoodFromRandomIdsList(IReadOnlyList<Food> foods, IEnumerable<int> randomFoodList)
    {
        return randomFoodList.Select(id => foods[id]).ToList();
    }

    public async Task SendOrder(ClientOrder clientOrder)
    {
        try
        {
            var serializeObject = JsonConvert.SerializeObject(clientOrder);
            var data = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            const string url = Settings.FoodOrderingServiceUrl;
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                ConsoleHelper.Print($"The order from client: {clientOrder.ClientId} was sent to food ordering service");
            }
        }
        catch (Exception e)
        {
            ConsoleHelper.Print($"Failed to send order {clientOrder.ClientId}", ConsoleColor.Red);
        }
    }
}
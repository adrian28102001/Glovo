using System.Net;
using System.Text;
using Client.Models;
using Client.Service.RestaurantDataService;
using DiningHall.Helpers;
using Newtonsoft.Json;

namespace Client.Service.OrderService;

public class OrderService : IOrderService
{
    private readonly IRestaurantDataService _restaurantDataService;

    public OrderService(IRestaurantDataService restaurantDataService)
    {
        _restaurantDataService = restaurantDataService;
    }

    public async Task<Order> CreateOrder()
    {
        var foodIds = new List<int>();
        var restaurantData = await _restaurantDataService.GetRestaurantData();
        foreach (var data in restaurantData)
        {
            ConsoleHelper.Print(
                $"We have restaurant with name: {data.RestaurantName} which has a menu of size {data.FoodList.Count}",
                ConsoleColor.Blue);
            
            foodIds.Add(data.FoodList[0].Id);
            foodIds.Add(data.FoodList[1].Id);
            foodIds.Add(data.FoodList[2].Id);
            foodIds.Add(data.FoodList[3].Id);
        }

        return new Order
        {
            Id = await IdGenerator.GenerateId(),
            FoodList = foodIds,
            OrderStatus = OrderStatus.OrderSent
        };
    }

    public async Task SendOrder(Order order)
    {
        try
        {
            var serializeObject = JsonConvert.SerializeObject(order);
            var data = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            const string url = Settings.FoodOrderingServiceUrl;
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                ConsoleHelper.Print($"The order with id {order.Id} was sent to food ordering service");
            }
        }
        catch (Exception e)
        {
            ConsoleHelper.Print($"Failed to send order {order.Id}", ConsoleColor.Red);
        }
    }
}
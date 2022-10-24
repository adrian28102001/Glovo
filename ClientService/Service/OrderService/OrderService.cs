using System.Net;
using System.Text;
using Client.Helpers;
using Client.Models;
using Client.Service.RestaurantDataService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace Client.Service.OrderService;

public class OrderService : IOrderService
{
    private readonly IRestaurantDataService _restaurantDataService;

    public OrderService(IRestaurantDataService restaurantDataService)
    {
        _restaurantDataService = restaurantDataService;
    }

    public async Task<ClientOrder> CreateOrder()
    {
        var order = new List<Order>();
        var restaurantDataList = await _restaurantDataService.GetRestaurantData();

        var chooseFoodFromNRestaurants =
            RandomGenerator.ListNumberGenerator(restaurantDataList.Count); //order for max 3 restaurants at a time
        foreach (var restaurantId in chooseFoodFromNRestaurants)
        {
            var restaurant = await _restaurantDataService.GetRestaurantDataById(restaurantId);
            var menu = restaurant.Menu;
            var randomFoodList = RandomGenerator.ListNumberGenerator(menu.Count());
            order.Add(new Order
            {
                RestaurantId = restaurant.RestaurantId,
                Foods = randomFoodList,
                Priority = RandomGenerator.NumberGenerator(3),
                MaxWait = 0,
                CreateOnTime = DateTime.Now
            });
        }
        
        return new ClientOrder
        {
            ClientId = await IdGenerator.GenerateId(),
            OrderId = await IdGenerator.GenerateId(),
            Orders = order
        };
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
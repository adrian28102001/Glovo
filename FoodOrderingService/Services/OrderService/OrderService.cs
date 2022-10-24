using System.Collections.Concurrent;
using System.Net;
using System.Text;
using FoodOrderingService.Helpers;
using FoodOrderingService.Models;
using FoodOrderingService.Services.RestaurantDataService;
using Newtonsoft.Json;

namespace FoodOrderingService.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IRestaurantDataService _restaurantDataService;

    public OrderService(IRestaurantDataService restaurantDataService)
    {
        _restaurantDataService = restaurantDataService;
    }

    public async Task SeparateOrders(ClientOrder clientOrder)
    {
        foreach (var order in clientOrder.Orders)
        {
            await SendOrderToRestaurant(order);
        }
    }

    private async Task SendOrderToRestaurant(Order order)
    {
        var url = _restaurantDataService.GetRestaurantAddressById(order.RestaurantId);
        try
        {
            var serializeObject = JsonConvert.SerializeObject(order);
            var data = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                ConsoleHelper.Print($"The order was sent to Restaurant with id {order.RestaurantId}");
            }
        }
        catch (Exception e)
        {
            ConsoleHelper.Print($"Failed to send order", ConsoleColor.Red);
        }
        
    }

    public string GetRestaurant(int id)
    {
        return _restaurantDataService.GetRestaurantAddressById(id);
    }
}
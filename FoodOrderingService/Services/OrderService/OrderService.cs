using System.Net;
using System.Text;
using FoodOrderingService.Helpers;
using FoodOrderingService.Models;
using FoodOrderingService.Services.ClientOrderService;
using FoodOrderingService.Services.RestaurantDataService;
using FoodOrderingService.Settings;
using Newtonsoft.Json;

namespace FoodOrderingService.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IRestaurantDataService _restaurantDataService;
    private readonly IClientOrderService _clientOrderService;

    public OrderService(IRestaurantDataService restaurantDataService, IClientOrderService clientOrderService)
    {
        _restaurantDataService = restaurantDataService;
        _clientOrderService = clientOrderService;
    }

    public async Task SeparateOrders(ClientOrder clientOrder)
    {
        foreach (var order in clientOrder.Orders)
        {
            await SendOrderToRestaurant(order);
        }
    }

    public async Task SendResponseToClient(Response sendingResponse)
    {
        var url = Setting.ClientUrl;
        try
        {
            var serializeObject = JsonConvert.SerializeObject(sendingResponse);
            var data = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                ConsoleHelper.Print($"The response for order with {sendingResponse.OrderId} was sent");
            }
        }
        catch (Exception e)
        {
            ConsoleHelper.Print($"Failed to send order response", ConsoleColor.Red);
        }
    }

    public async Task CheckIfOrderReady(ClientOrder clientOrder)
    {
        var order = await _clientOrderService.GetOrderByClientId(clientOrder.OrderId);
        var readyOrder = order.Orders.First(order1 => order1.OrderId.Equals(clientOrder.OrderId));
        order.Orders.Remove(readyOrder);

        //this means the order is done
        if (order.Orders.Count == 0)
        {
            await SendOrderToClient(clientOrder);
        }
    }

    private static async Task SendOrderToClient(ClientOrder clientOrder)
    {
        var url = Setting.ClientOrderUrl;
        try
        {
            var serializeObject = JsonConvert.SerializeObject(clientOrder);
            var data = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                ConsoleHelper.Print($"The order with id {clientOrder.OrderId} was sent to client{clientOrder.ClientId}");
            }
        }
        catch (Exception e)
        {
            ConsoleHelper.Print($"Failed to send order", ConsoleColor.Red);
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
}
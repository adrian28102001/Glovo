using System.Collections.Concurrent;
using FoodOrderingService.Helpers;
using FoodOrderingService.Helpers.Mapping;
using FoodOrderingService.Models;
using FoodOrderingService.Services.ClientOrderService;
using FoodOrderingService.Services.OrderService;
using FoodOrderingService.Services.RestaurantDataService;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingService.Controllers;

[ApiController]
[Route("/foodorderingservice")]
public class ApiController : Controller
{
    private readonly IRestaurantDataService _restaurantDataService;
    private readonly IOrderService _orderService;
    private readonly IClientOrderService _clientOrderService;
    private static readonly Semaphore Semaphore = new(1, 1);
    public ApiController(IRestaurantDataService restaurantDataService, IOrderService orderService, IClientOrderService clientOrderService)
    {
        _restaurantDataService = restaurantDataService;
        _orderService = orderService;
        _clientOrderService = clientOrderService;
    }

    //here there is a list will all restaurants, helps to make an order in Client ch
    [HttpGet("/menu")]
    public Task<ConcurrentBag<Restaurant>> GetRestaurants()
    {
        return _restaurantDataService.GetRestaurantData();
    }

    [HttpPost("/register")]
    public Task RegisterRestaurant([FromBody] Restaurant restaurant)
    {
        var restaurantModel = MappingRestaurant.MapRestaurant(restaurant);
        ConsoleHelper.Print($"A new restaurant with id {restaurantModel.RestaurantId} was registered");
        Semaphore.WaitOne();
        _restaurantDataService.Insert(restaurantModel);
        Semaphore.Release();
        return Task.CompletedTask;
    }

    [HttpPost("/order")]
    public Task GetOrderFromClient([FromBody] ClientOrder order)
    {
        Semaphore.WaitOne();
        _clientOrderService.Insert(order);
        Semaphore.Release();
        _orderService.SeparateOrders(order);
        return Task.CompletedTask;
    }
    [HttpPost("/orderready")]
    public Task GetOrderFromRestaurant([FromBody] ClientOrder order)
    {
        Semaphore.WaitOne();
        ConsoleHelper.Print($"I received from the restaurant an order with id {order.OrderId}", ConsoleColor.Cyan);
        _orderService.CheckIfOrderReady(order);
        Semaphore.Release();
        return Task.CompletedTask;
    }
    [HttpPost("/response")]
    public Task GetResponseFromRestaurant([FromBody] Response response)
    {
        ConsoleHelper.Print($"I received from the restaurant an order with id {response.OrderId}", ConsoleColor.Cyan);
        Semaphore.WaitOne();
        _orderService.SendResponseToClient(response);
        Semaphore.Release();
        return Task.CompletedTask;
    }
}
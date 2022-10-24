using System.Collections.Concurrent;
using FoodOrderingService.Helpers;
using FoodOrderingService.Helpers.Mapping;
using FoodOrderingService.Models;
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

    public ApiController(IRestaurantDataService restaurantDataService, IOrderService orderService)
    {
        _restaurantDataService = restaurantDataService;
        _orderService = orderService;
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
        _restaurantDataService.Insert(restaurantModel);
        return Task.CompletedTask;
    }

    [HttpPost("/order")]
    public Task GetOrderFromClient([FromBody] ClientOrder order)
    {
        ConsoleHelper.Print($"I received from the client an order with id {order.OrderId}", ConsoleColor.Cyan);
        _orderService.SeparateOrders(order);
        return Task.CompletedTask;
    }
}
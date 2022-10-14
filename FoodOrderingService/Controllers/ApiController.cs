using System.Collections.Concurrent;
using DiningHall.Helpers;
using FoodOrderingServices.Models;
using FoodOrderingServices.Services.RestaurantDataService;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingServices.Controllers;

[ApiController]
[Route("/foodorderingservice")]
public class ApiController : Controller
{
    private readonly IRestaurantDataService _restaurantDataService;
    public ApiController(IRestaurantDataService restaurantDataService)
    {
        _restaurantDataService = restaurantDataService;
    }
    
    //here there is a list will all restaurants, helps to make an order in Client ch
    [HttpGet("/menu")] 
    public Task<ConcurrentBag<RestaurantData>> GetRestaurants()
    {
        return _restaurantDataService.GetRestaurantData();
    }
    
    //post the info about restaurant from DH
    [HttpPost("/register")] // this registers the restaurant 
    public Task RegisterRestaurant([FromBody] RestaurantData restaurantData)
    {
        ConsoleHelper.Print($"A new restaurant with id {restaurantData.RestaurantId} was registered");
        _restaurantDataService.Insert(restaurantData);
        return Task.CompletedTask;
    }
    
    [HttpPost("/order")]
    public Task GetOrderFromClient([FromBody] Order order)
    {
        ConsoleHelper.Print($"I received from the client an order with id {order.Id}", ConsoleColor.Cyan);
        return Task.CompletedTask;
    }
}
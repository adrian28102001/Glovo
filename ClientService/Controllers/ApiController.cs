using Client.Helpers;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Client.Controllers;

[ApiController]
[Route("/clientside")]
public class ApiController : Controller
{
    [HttpGet]
    public Task<IList<RestaurantData>?> GetRestaurantData()
    {
        var client = new RestClient(Settings.GetMenuUrl);
        var response = client.Execute<IList<RestaurantData>>(new RestRequest());
        return Task.FromResult(response.Data);
    }

    [HttpPost("/orderdetails")]
    public Task GetOrderDetails([FromBody] Response response)
    {
        ConsoleHelper.Print($"For order with id {response.OrderId} waiting time is: {response.WaitingTime}",
            ConsoleColor.Green);


        return Task.CompletedTask;
    }
    
    [HttpPost("/receiveorder")]
    public Task GetOrderFromFoodOrderingService([FromBody] ClientOrder clientOrder)
    {
        ConsoleHelper.Print($"The order for client: {clientOrder.ClientId} is ready" );
        return Task.CompletedTask;
    }
}
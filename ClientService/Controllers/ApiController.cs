using Client.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Client.Controllers;

[ApiController]
[Route("/clientside")]
public class ApiController: Controller
{
    [HttpGet]
    public Task<IList<RestaurantData>?> GetRestaurantData()
    {
        var client = new RestClient(Settings.GetMenuUrl);
        var response = client.Execute<IList<RestaurantData>>(new RestRequest());
        return Task.FromResult(response.Data);
    }
}
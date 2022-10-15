using System.Net;
using System.Text;
using DiningHall.Helpers;
using DiningHall.Models;
using DiningHall.Models.SettingsFolder;
using Newtonsoft.Json;

namespace DiningHall.Services.RegisterRestaurantService;

public class RegisterRestaurantService : IRegisterRestaurantService
{
    
    private static async Task<RestaurantData> GetRestaurantDetails()
    {
        using var streamReader = new StreamReader(Settings.RestaurantData);
        var json = await streamReader.ReadToEndAsync();
        return JsonConvert.DeserializeObject<RestaurantData>(json)!;
    }
    public async Task RegisterRestaurant()
    {
        try
        {
            var restaurantData =await GetRestaurantDetails();
            var serializeObject = JsonConvert.SerializeObject(restaurantData);
            var data = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            const string url = Settings.FoodOrderingServiceRegisterUrl;
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                await ConsoleHelper.Print($"I was registered to Food Ordering Service ");
               
            }
        }
        catch (Exception e)
        {
            await ConsoleHelper.Print($"Something went wrong", ConsoleColor.Red);
        }
    }
}
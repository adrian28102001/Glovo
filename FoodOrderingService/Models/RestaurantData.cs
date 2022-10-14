using System.Collections.Concurrent;
using FoodOrderingServices.Models;


//provides restaurants menu
public class RestaurantData : BaseEntity
{
    public RestaurantData()
    {
        FoodList = new List<Food>();
    }

    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; }
    public IList<Food> FoodList { get; set; }
}
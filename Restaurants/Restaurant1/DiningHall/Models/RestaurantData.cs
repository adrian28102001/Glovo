namespace DiningHall.Models;

public class RestaurantData
{
    public RestaurantData()
    {
        FoodList = new List<Food>();
    }

    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; }
    public IList<Food> FoodList { get; set; }
}
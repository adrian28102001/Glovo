namespace Client.Models;

public class RestaurantData
{
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; }
    public IList<Food> FoodList { get; set; }
}
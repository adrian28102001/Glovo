namespace FoodOrderingServices.Models;

public class FoodOrderingService : BaseEntity
{
    public FoodOrderingService()
    {
        RestaurantId = 0;
        Name = "name";
        Address = "address";
        MenuItems = 0;
        Rating = 0;
    }

    private int RestaurantId { get; set; }
    private string Name { get; set; }
    private string Address { get; set; }
    private int MenuItems { get; set; }
    // private Menu Menu { get; set; }
    private int Rating { get; set; }
}
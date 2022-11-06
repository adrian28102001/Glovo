namespace FoodOrderingService.Models;

public class ClientOrder
{
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public List<Order> Orders{ get; set; }
}
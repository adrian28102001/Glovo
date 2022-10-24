namespace FoodOrderingService.Models;

public class ClientOrder
{
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public IEnumerable<Order> Orders{ get; set; }
}
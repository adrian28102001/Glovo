namespace FoodOrderingService.Models;

public class OrderResponse
{
    public int OrderId { get; set; }
    public IEnumerable<ClientOrderResponse> Orders { get; set; }
}
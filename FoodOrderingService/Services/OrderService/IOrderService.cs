using FoodOrderingService.Models;

namespace FoodOrderingService.Services.OrderService;

public interface IOrderService
{
    Task SeparateOrders(ClientOrder clientOrder);
    Task SendResponseToClient(Response response);
    Task CheckIfOrderReady(ClientOrder order);
}
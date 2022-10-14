using Client.Models;

namespace Client.Service.OrderService;

public interface IOrderService
{
    Task<Order> CreateOrder();
    Task SendOrder(Order order);
}
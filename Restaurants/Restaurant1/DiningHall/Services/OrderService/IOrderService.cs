using System.Collections.Concurrent;
using DiningHall.Models;

namespace DiningHall.Services.OrderService;

public interface IOrderService
{
    Task SendOrderToKitchen(Order order);
    Task<ConcurrentBag<Order>> GetAll();
    Task<Order?> GetOrderByTableId(int id);
    Task AskForResponseInKitchen(ClientOrder order);
    Order MapOrders(ClientOrder clientOrder);
    Task SendOrderToFoodOrderingService(Order order);
}
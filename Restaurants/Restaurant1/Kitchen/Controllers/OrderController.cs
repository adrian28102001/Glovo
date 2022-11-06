using System.Collections.Concurrent;
using Kitchen.Helpers;
using Kitchen.Models;
using Kitchen.Services.OrderHistoryService;
using Kitchen.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen.Controllers;

[ApiController]
[Route("/kitchen")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IOrderHistoryService _orderHistoryService;
    private readonly SemaphoreSlim _semaphore;

    public OrderController(IOrderService orderService, IOrderHistoryService orderHistoryService)
    {
        _orderService = orderService;
        _orderHistoryService = orderHistoryService;
        _semaphore = new SemaphoreSlim(1);
    }

    [HttpGet]
    public ConcurrentBag<OrderHistory> GetOrderHistory()
    {
        return _orderHistoryService.GetAll();
    }

    [HttpPost]
    public async Task GetOrderFromRestaurant([FromBody] Order? order)
    {
        if (order == null) return;
        try
        {
            await ConsoleHelper.Print($"An order with {order.Id} came in the kitchen", ConsoleColor.DarkYellow);
            await _orderService.InsertOrder(order);
        }
        catch (Exception e)
        {
            //ignore
        }
    }

    [HttpPost("/response")]
    public async Task GetResponseFromKitchen([FromBody] ClientOrder? order)
    {
        if (order == null) return;
        try
        {
            await ConsoleHelper.Print($"Preparing waiting time for order {order.OrderId} from client {order.ClientId}",
                ConsoleColor.DarkYellow);
            await _orderService.PrepareOrderResponse(order);
        }
        catch (Exception e)
        {
            //ignore
        }
    }
}
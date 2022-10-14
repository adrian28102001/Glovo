using Client.Service.OrderService;
using DiningHall.Helpers;

namespace Client.ClientService;

public class ClientService : IClientService
{
    private readonly IOrderService _orderService;

    public ClientService(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task GenerateOrder(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ConsoleHelper.Print("I will think about getting...");
            await SleepGenerator.Delay(10);
            var order = await _orderService.CreateOrder();
            await _orderService.SendOrder(order);
            ConsoleHelper.Print($"I just have sent order with id {order.Id}...");
        }
    }
}
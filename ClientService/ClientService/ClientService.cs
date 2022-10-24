using Client.Helpers;
using Client.Service.OrderService;

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
            var clientOrder = await _orderService.CreateOrder();
            await _orderService.SendOrder(clientOrder);
            ConsoleHelper.Print($"I just have sent order from client {clientOrder.ClientId}...");
        }
    }
}
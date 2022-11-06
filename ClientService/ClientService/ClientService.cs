using Client.Helpers;
using Client.Service.OrderService;

namespace Client.ClientService;

public class ClientService : IClientService
{
    private readonly IOrderService _orderService;
    private static int _currentClients = 3;
    private static readonly Semaphore Semaphore = new(1, 1);


    public ClientService(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task ExecuteCode(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_currentClients >= 0)
            {
                _currentClients -= 3;

                var clientId = await IdGenerator.GenerateClientId();
                var client1 = Task.Run(() => GenerateOrder(clientId), cancellationToken);
                // var client2 = Task.Run(GenerateOrder, cancellationToken);
                // var client3 = Task.Run(GenerateOrder, cancellationToken);
                // var taskList = new List<Task>
                // {
                //     client1
                // };

                // await Task.WhenAll(taskList);
            }
        }
    }

    private async Task GenerateOrder(int clientId)
    {
        var clientOrder = await _orderService.CreateOrder(clientId);
        Semaphore.WaitOne();
        await _orderService.SendOrder(clientOrder);
        Semaphore.Release();
        ConsoleHelper.Print(
            $"I just have sent order with id {clientOrder.OrderId} from client {clientOrder.ClientId}...");
    }
}
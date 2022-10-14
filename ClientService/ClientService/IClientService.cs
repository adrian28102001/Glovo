namespace Client.ClientService;

public interface IClientService
{
    Task GenerateOrder(CancellationToken cancellationToken);
}
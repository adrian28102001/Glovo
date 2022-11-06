namespace Client.Helpers;

public static class IdGenerator
{
    private static readonly Semaphore NormalOrderSemaphore = new(1, 1);
    private static int ClientId { get; set; }
    private static int OrderId { get; set; }

    public static Task<int> GenerateClientId()
    {
        NormalOrderSemaphore.WaitOne();
        ClientId += 1;
        NormalOrderSemaphore.Release();
        return Task.FromResult(ClientId);
    }
    public static Task<int> GenerateOrderId()
    {
        NormalOrderSemaphore.WaitOne();
        OrderId += 1;
        NormalOrderSemaphore.Release();
        return Task.FromResult(OrderId);
    }
}
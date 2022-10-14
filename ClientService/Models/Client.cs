namespace Client.Models;

public class Client : BaseEntity
{
    public string Name { get; set; }
    public Order Order { get; set; }
}
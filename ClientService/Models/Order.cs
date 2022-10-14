using System.Collections;
using System.Text.Json.Serialization;

namespace Client.Models;

public class Order : BaseEntity
{
    public IEnumerable FoodList { get; set; }
    [JsonIgnore] public OrderStatus OrderStatus { get; set; }
}
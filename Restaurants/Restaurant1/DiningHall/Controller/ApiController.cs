using System.Net;
using System.Text;
using DiningHall.Helpers;
using DiningHall.Models;
using DiningHall.Models.Status;
using DiningHall.Repositories.TableRepository;
using DiningHall.Services.OrderService;
using DiningHall.SettingsFolder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiningHall.Controller;

[ApiController]
[Route("/dininghall")]
public class ApiController : ControllerBase
{
    private readonly ITableRepository _tableRepository;
    private readonly IOrderService _orderService;
    private readonly Semaphore _semaphore;

    public ApiController(ITableRepository tableRepository, IOrderService orderService)
    {
        _tableRepository = tableRepository;
        _orderService = orderService;
        _semaphore = new Semaphore(1, 1);
    }

    [HttpPost("/order")]
    public async Task GetOrderFromFoodOrderingService([FromBody] ClientOrder clientOrder)
    {
        var order = _orderService.MapOrders(clientOrder);
        await ConsoleHelper.Print($"I received from the foodOrderingService an order", ConsoleColor.Cyan);
        // _orderService.AskForResponseInKitchen(clientOrder);
        await _orderService.SendOrderToKitchen(order);
    }

    [HttpPost("/response")]
    public async Task GetResponseFromOrder([FromBody] Response response)
    {
        await ConsoleHelper.Print(
            $"I received from the kitchen waiting time {response.WaitingTime} for orderWith id {response.OrderId}",
            ConsoleColor.Cyan);
        await SendResponseToFoodOrderingService(response);
    }

    private static async Task SendResponseToFoodOrderingService(Response sendingResponse)
    {
        try
        {
            var serializeObject = JsonConvert.SerializeObject(sendingResponse);
            var data = new StringContent(serializeObject, Encoding.UTF8, "application/json");

            const string url = Settings.FoodOrderingServiceResponseUrl;
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                await ConsoleHelper.Print($"Waiting time was sent to foodOrderingService");
            }
        }
        catch (Exception e)
        {
            await ConsoleHelper.Print($"Failed to send order waiting time", ConsoleColor.Red);
        }
    }

    [HttpPost]
    public async Task GetOrderFromKitchen([FromBody] Order order)
    {
        if (order.TableId == null || order.WaiterId == null)
        {
            await _orderService.SendOrderToFoodOrderingService(order);
        }
        
        order.OrderStatus = OrderStatus.OrderCooked;
        var table = await _tableRepository.GetById(order.TableId);
        if (table != null)
        {
            table.TableStatus = TableStatus.IsAvailable;
            await ConsoleHelper.Print(
                $"I received from the kitchen an order with id {order.Id} for table {order.TableId}",
                ConsoleColor.Cyan);
            _semaphore.WaitOne();
            await RatingHelper.GetRating(order);
            _semaphore.Release();
        }
    }
}
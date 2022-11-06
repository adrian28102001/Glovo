using System.Collections.Concurrent;
using Kitchen.Models;

namespace Kitchen.Helpers;

public static class EstimationTime
{
    public static int ComputeEstimationTime(ClientOrder order, IList<Food> foodList, ConcurrentBag<Cook> cooks,
        int cookingApparatusCount,
        List<Order> waitingOrders)
    {
        var timeForFoodWithoutCookingApparatus = 0;
        var timeForFoodWithCookingApparatus = 0;
        var totalFoodWaitingCurrentOrder = order.Foods.Count();
        var totalFoodsWaiting = waitingOrders.Sum(order1 => order1.FoodList.Count);
        var totalCooksProficiency = cooks.Sum(cook => cook.Proficiency);
        foreach (var food in foodList)
        {
            if (food.CookingApparatus != null)
            {
                timeForFoodWithoutCookingApparatus += food.PreparationTime;
            }
            else
            {
                timeForFoodWithCookingApparatus += food.PreparationTime;
            }
        }

        return (timeForFoodWithoutCookingApparatus / totalCooksProficiency +
                timeForFoodWithCookingApparatus / cookingApparatusCount) * (totalFoodsWaiting + totalFoodWaitingCurrentOrder) /
            totalFoodWaitingCurrentOrder;
    }
}
using Client.Models;

namespace Client.Helpers;

public static class MaxWaitingTime
{
    public static int CalculateMaxWaitingTine(IEnumerable<Food> randomFoodList)
    {
        var maxFood = randomFoodList.MaxBy(food => food.PreparationTime);
        if (maxFood == null) return 0;
        
        var time = maxFood.PreparationTime * 1.8;
        return (int) Math.Ceiling(time);
    }
}
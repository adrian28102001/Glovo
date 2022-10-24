namespace Client.Helpers;

public static class RandomGenerator
{
    public static int NumberGenerator(int max)
    {
        var random = new Random();
        return random.Next(1, max);
    }

    public static int NumberGenerator(int min, int max)
    {
        var random = new Random();
        return random.Next(min, max);
    }

    public static IEnumerable<int> ListNumberGenerator(int listSize)
    {
        var listNumberGenerator = new List<int>();
        while (listNumberGenerator.Count != listSize / 2)
        {
            var randomNumber = NumberGenerator(listSize);
            if (!listNumberGenerator.Contains(randomNumber))
            {
                listNumberGenerator.Add(randomNumber);
            }
        }

        return listNumberGenerator;
    }
}
namespace PhotoSiTest.Common.Extensions;

public static class RandomExtensions
{
    public static decimal NextDecimal(this Random random, decimal min, decimal max, int decimals)
    {
        var value = random.NextDouble() * (double)(max - min) + (double)min;
        return Math.Round((decimal)value, decimals);
    }
}

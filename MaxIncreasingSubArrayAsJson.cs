using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

public static class Solutions   // <<< sınıf ekledik
{
    public static string MaxIncreasingSubArrayAsJson(List<int> numbers)
    {
        if (numbers == null || numbers.Count == 0)
            return "[]";

        int bestStart = 0, bestEnd = 0, bestSum = numbers[0];
        int curStart = 0, curSum = numbers[0];

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] > numbers[i - 1])
            {
                curSum += numbers[i];
            }
            else
            {
                if (curSum > bestSum ||
                    (curSum == bestSum && (i - 1 - curStart) > (bestEnd - bestStart)))
                {
                    bestSum = curSum;
                    bestStart = curStart;
                    bestEnd = i - 1;
                }
                curStart = i;
                curSum = numbers[i];
            }
        }

        if (curSum > bestSum ||
            (curSum == bestSum && (numbers.Count - 1 - curStart) > (bestEnd - bestStart)))
        {
            bestSum = curSum;
            bestStart = curStart;
            bestEnd = numbers.Count - 1;
        }

        var result = numbers.GetRange(bestStart, bestEnd - bestStart + 1);
        return JsonSerializer.Serialize(result);
    }
}

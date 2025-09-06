using System.Linq;
using System.Text.Json;
using System.Collections.Generic;


public static string MaxIncreasingSubArrayAsJson(List<int> numbers)
{
    // Returns the increasing subarray with highest sum as JSON.
    // Edge case: if two subarrays have the same sum, the FIRST one is chosen.

    // Handle empty input
    if (numbers == null || numbers.Count == 0)
        return JsonSerializer.Serialize(new List<int>());

    // Set up variables to track the best subarray and current candidate
    int bestStart = 0, bestEnd = 0, bestSum = int.MinValue;
    int curStart = 0, curSum = numbers[0];

    for (int i = 1; i < numbers.Count; i++)
    {
        if (numbers[i] > numbers[i - 1])
        {
            // Still increasing, add to current sum
            curSum += numbers[i];
        }
        else
        {
            // Growth broke → compare current segment against the best so far
            // Using '>' instead of '>=' ensures we keep the first subarray on ties.
            if (curSum > bestSum)
            {
                bestSum = curSum;
                bestStart = curStart;
                bestEnd = i - 1;
            }

            // Start a new segment
            curStart = i;
            curSum = numbers[i];
        }
    }
    // Handle the last segment (may end at the array's end without a break)
    if (curSum > bestSum)
    {
        bestSum = curSum;
        bestStart = curStart;
        bestEnd = numbers.Count - 1;
    }
    
    // Return the best subarray as JSON
    var result = new List<int>();
    for (int i = bestStart; i <= bestEnd; i++)
        result.Add(numbers[i]);

    return JsonSerializer.Serialize(result);
}
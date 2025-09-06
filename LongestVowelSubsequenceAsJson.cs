using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

public static string LongestVowelSubsequenceAsJson(List<string> words)
{

    // edge case: if two subsequences have the same length, the first one is kept
    // Handle empty input
    if (words == null || words.Count == 0)
    {
        return JsonSerializer.Serialize(new List<object>());
    }

    // store vowels in HashSet for fast
    var vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };
    var result = new List<object>(words.Count);

    foreach (var word in words)
    {
        // skipping null or empty words
        if (string.IsNullOrEmpty(word))
        {
            result.Add(new { word = "", sequence = "", length = 0 });
            continue;
        }

        int bestStart = -1;     // best subsequence start index (-1 means still no vowel is found)
        int bestLen = 0;        // length of the longest vowel subsequence

        int curStart = -1;      // current subsequence start index (-1 means still no vowel is found)
        int curLen = 0;         // current subsequence length


        //Checking each character in the word
        for (int i = 0; i < word.Length; i++)
        {
            char c = word[i];
            bool isVowel = vowels.Contains(c);

            if (isVowel)
            {
                if (curLen == 0)
                {
                    curStart = i; // start new subsequence
                }

                curLen++;

                if (curLen > bestLen)
                {
                    bestLen = curLen;
                    bestStart = curStart;
                }
            }
            else
            {
                curLen = 0; // reset
            }
        }

        // extract longest vowel subsequence if found or null
        string seq = bestLen > 0 ? word.Substring(bestStart, bestLen) : "";

        result.Add(new { word, sequence = seq, length = bestLen });
    }

    // serialize result list to JSON
    return JsonSerializer.Serialize(result);

}


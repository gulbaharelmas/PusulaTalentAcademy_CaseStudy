using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public static string FilterEmployees(IEnumerable<(string Name, int Age, string Department, decimal Salary, DateTime HireDate)> employees)
{
    // If null , return empty JSON
    if (employees == null)
    {
        return JsonSerializer.Serialize(new
        {
            Names = Array.Empty<string>(),
            TotalSalary = 0m,
            AverageSalary = 0m,
            MinSalary = 0m,
            MaxSalary = 0m,
            Count = 0
        });
    }

    // Filters:
    // - Age between 25 and 40 
    // - Department is IT or Finance
    // - Salary between 5000 and 9000
    // filter: HireDate >= 2017-01-01
    // note: although statement says "after 2017", example 5 includes 2017-12-31, so year 2017 is treated as included

    var hireDateThreshold = new DateTime(2017, 1, 1);

    var filtered = employees
        .Where(e =>
            e.Age >= 25 && e.Age <= 40 &&
            (string.Equals(e.Department, "IT", StringComparison.OrdinalIgnoreCase) ||
             string.Equals(e.Department, "Finance", StringComparison.OrdinalIgnoreCase)) &&
            e.Salary >= 5000m && e.Salary <= 9000m &&
            e.HireDate >= hireDateThreshold)
        .ToList();

    // Names: order by length descending, then alphabetically ascending 
    var names = filtered
        .Select(e => e.Name)
        .OrderByDescending(n => n.Length)
        .ThenBy(n => n, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    var count = filtered.Count;
    var total = filtered.Sum(e => e.Salary);
    var min = count > 0 ? filtered.Min(e => e.Salary) : 0m;
    var max = count > 0 ? filtered.Max(e => e.Salary) : 0m;

    //calculate average salary with 2-decimal precision
    decimal avg = 0m;
    if (count > 0)
    {
        decimal rawAverage = total / count;
        avg = Math.Round(rawAverage, 2, MidpointRounding.AwayFromZero); // ex: 7166.67(example1) or stay 7000(example2)
    }

    return JsonSerializer.Serialize(new
    {
        Names = names,
        TotalSalary = total,
        AverageSalary = avg,
        MinSalary = min,
        MaxSalary = max,
        Count = count
    });
}

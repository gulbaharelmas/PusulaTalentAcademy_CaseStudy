using System;
using System.Linq;
using System.Xml.Linq;
using System.Text.Json;

public static string FilterPeopleFromXml(string xmlData)
{
    // Filters people from XML according to given criteria and returns JSON
    // Criteria: Age > 30, Department = "IT", Salary > 5000, HireDate < 2019-01-01
    
    // STEP 1: Check for null/empty input
    if (string.IsNullOrWhiteSpace(xmlData))
    {
        return JsonSerializer.Serialize(new
        {
            Names = Array.Empty<string>(),
            TotalSalary = 0,
            AverageSalary = 0,
            MaxSalary = 0,
            Count = 0
        });
    }

    try
    {
        // STEP 2: Parse XML
        XDocument xmlDocument = XDocument.Parse(xmlData);
        
        // STEP 3: Define filter criteria
        DateTime hireDateThreshold = new DateTime(2019, 1, 1);
        const int minAge = 30;
        const string targetDepartment = "IT";
        const int minSalary = 5000;

        // STEP 4: Read Person nodes and safely parse values
        var allPeople = xmlDocument.Descendants("Person")
            .Select(person => new
            {
                // safe string read
                Name = person.Element("Name")?.Value?.Trim() ?? "",
                
                // safe int parse (default 0)
                Age = int.TryParse(person.Element("Age")?.Value, out int age) ? age : 0,
                
                // safe string read and trim
                Department = person.Element("Department")?.Value?.Trim() ?? "",
                
                // safe int parse (default 0)
                Salary = int.TryParse(person.Element("Salary")?.Value, out int salary) ? salary : 0,
                
                // safe DateTime parse (default MaxValue)
                HireDate = DateTime.TryParse(person.Element("HireDate")?.Value, out DateTime hireDate) 
                            ? hireDate 
                            : DateTime.MaxValue
            })
            .ToList();

        // STEP 5: Apply filter conditions
        var filteredPeople = allPeople
            .Where(person => 
                person.Age > minAge &&                                                    // age condition
                string.Equals(person.Department, targetDepartment, StringComparison.OrdinalIgnoreCase) && // department condition (case-insensitive)
                person.Salary > minSalary &&                                            // salary condition
                person.HireDate < hireDateThreshold                                     // hire date condition
            )
            .ToList();

        // STEP 6: Sort names alphabetically
        // note: this is case-insensitive but doesn't handle Turkish letters (ç, ğ, ş, ü, ö, İ)
        string[] sortedNames = filteredPeople
            .Select(person => person.Name)
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase) // case-insensitive order
            .ToArray();

        // STEP 7: Calculate salary stats
        int totalSalary = filteredPeople.Sum(person => person.Salary);
        int peopleCount = filteredPeople.Count;
        int averageSalary = peopleCount > 0 ? totalSalary / peopleCount : 0;
        int maxSalary = peopleCount > 0 ? filteredPeople.Max(person => person.Salary) : 0;

        // STEP 8: Build result object and serialize to JSON
        var result = new
        {
            Names = sortedNames,
            TotalSalary = totalSalary,
            AverageSalary = averageSalary,
            MaxSalary = maxSalary,
            Count = peopleCount
        };

        return JsonSerializer.Serialize(result);
    }
    catch (System.Xml.XmlException)
    {
        // if XML parse fails, return empty result
        return JsonSerializer.Serialize(new
        {
            Names = Array.Empty<string>(),
            TotalSalary = 0,
            AverageSalary = 0,
            MaxSalary = 0,
            Count = 0
        });
    }
}

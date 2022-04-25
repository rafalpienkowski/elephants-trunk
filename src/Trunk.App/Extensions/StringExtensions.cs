using System.Globalization;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Spectre.Console;

namespace Trunk.App.Extensions;

public static class StringExtensions
{
    public static async Task WriteToCsvFile<T>(this string fileName, IEnumerable<T> entities)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        await using var sw = new StreamWriter(fileName);
        await using var csv = new CsvWriter(sw, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(entities);
    }


    public static IList<T> ReadFromCsvFile<T>(this string fileName)
    {
        using var sr = new StreamReader(fileName);
        using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

        var result = csv.GetRecords<T>();

        return result.ToList();
    }

    public static async Task WriteToJsonFile<T>(this string fileName, T entities)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        var contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
        var serializer = new JsonSerializer();
        serializer.ContractResolver = contractResolver;
        await using var sw = new StreamWriter(fileName);
        using var writer = new JsonTextWriter(sw);
        
        serializer.Serialize(writer,entities);
    }

    public static ValidationResult ValidateFileArgument(this string? fileName, string argumentName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return ValidationResult.Error($"{argumentName} can't be null or empty");
        }

        if (!File.Exists(fileName))
        {
            return ValidationResult.Error($"File: '{fileName}' does not exist");
        }
        
        return ValidationResult.Success();
    }
}
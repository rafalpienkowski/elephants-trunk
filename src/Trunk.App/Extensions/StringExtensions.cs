using System.Globalization;
using CsvHelper;

namespace Trunk.App.Extensions;

public static class StringExtensions
{
    public static async Task WriteToCsvFile<T>(this string? fileName, IEnumerable<T> entities)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException(nameof(fileName), "Filename has to be specified");
        }
        
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        await using var sw = new StreamWriter(fileName);
        await using var csv = new CsvWriter(sw, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(entities);
    }

    public static IEnumerable<T> ReadFromCsvFile<T>(this string? fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new ArgumentOutOfRangeException(nameof(fileName), $"File: '{fileName}' does not exist");
        }

        using var sr = new StreamReader(fileName);
        using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

        var result = csv.GetRecords<T>();

        return result;
    }
}
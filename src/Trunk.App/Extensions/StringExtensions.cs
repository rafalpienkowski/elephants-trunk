using System.Globalization;
using CsvHelper;

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
}
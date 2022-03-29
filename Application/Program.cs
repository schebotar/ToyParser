using CsvHelper;
using ParsingLibrary;
using System.Globalization;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

try
{
    var links = await Parser.GetShopPagesAsync(@"https://www.toy.ru/catalog/boy_transport/")
        .GetProductLinks()
        .ToListAsync();
    var products = await links
        .GetProducts()
        .ToListAsync();

    using var writer = new StreamWriter("result.csv");
    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    {
        csv.Context.RegisterClassMap<ParsingLibrary.CSVMap>();
        csv.WriteRecords(products);
    }
    Console.WriteLine($"Parsed {products.Count} entries");
}

catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

finally
{
    Console.WriteLine("Done");
}

Console.ReadKey();
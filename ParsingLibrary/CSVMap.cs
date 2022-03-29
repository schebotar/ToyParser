using CsvHelper.Configuration;

namespace ParsingLibrary
{
    public class CSVMap : ClassMap<Product>
    {
        public CSVMap()
        {
            AutoMap(System.Globalization.CultureInfo.InvariantCulture);
            Map(p => p.Breadcrumbs).TypeConverter<ListToCellConverter<List<string>>>();
            Map(p => p.ImagesUrl).TypeConverter<ListToCellConverter<List<string>>>();
        }
    }
}
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ParsingLibrary
{
    public class ListToCellConverter<List> : DefaultTypeConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            List<string>? list = value as List<string>;

            if (list == null)
            {
                return string.Empty;
            }

            else
            {
                return string.Join(",", list);
            }
        }
    }
}
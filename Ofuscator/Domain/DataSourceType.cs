namespace Obfuscator.Domain
{
    public enum DataSourceType
    {
        CSV,
        NIFGenerator
    }

    public class DataSourceBase
    {
        public static string GetDataSourcePrefix(DataSourceType dataSourceType)
        {
            var charBefore = ":";
            var charAfter = ":";
            return charBefore + dataSourceType.ToString() + charAfter;
        }

        public static bool IsNifGenerator(string dataSourceName)
        {
            return dataSourceName?.StartsWith(GetDataSourcePrefix(DataSourceType.NIFGenerator)) == true;
        }
    }
}

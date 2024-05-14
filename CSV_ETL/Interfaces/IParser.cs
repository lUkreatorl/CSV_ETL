using CSV_ETL.Models;

namespace CSV_ETL.Interfaces
{
    public interface IParser
    {
        List<DataModel> Parse();
    }
}

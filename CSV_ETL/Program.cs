using CSV_ETL.Services;

Console.WriteLine("Enter path to CSV file. Print \"Exit\" for leave program.");
string path = "";
while (!(path = Console.ReadLine()).Equals("exit", StringComparison.CurrentCultureIgnoreCase))
{
    if (!string.IsNullOrEmpty(path))
    {
		try
		{
            CSVParser parser = new CSVParser(path);
            var parsedData = parser.Parse();
            
            DBService db = new DBService();
            db.BulkInsert(parsedData);

            Console.WriteLine("Data imported successfully. \NPlease check folder with CSV file for for duplicate records and broken data.");
        }
		catch (Exception ex)
		{
            Console.WriteLine(ex.Message);
        }
    }
}
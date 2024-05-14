namespace CSV_ETL.Services
{
    using CSV_ETL.Models;
    using CsvHelper.Configuration;
    using CsvHelper;
    using System.Globalization;
    using System.IO;
    using System.Reflection.PortableExecutable;

    public class CSVParser : Interfaces.IParser
    {
        public readonly string _filePath;
        public readonly string _savePath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">Path to file for parce</param>
        /// <param name="savePath">Save folder for duplicated records (additional)</param>
        public CSVParser(string filePath)
        {
            _filePath = filePath;
            _savePath = Path.GetDirectoryName(filePath);
        }

        /// <summary>
        /// Parce CSV file. Save duplicates records in the same dirrectory as input file.
        /// </summary>
        /// <returns></returns>
        public List<DataModel> Parse()
        {
            List<DataModel> records = new List<DataModel>();
            List<DataModel> duplicateRecords = new List<DataModel>();
            List<string> errorMessages = new List<string>();

            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim
            }))
            {
                csv.Context.RegisterClassMap<Mapper>();

                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<DataModel>();
                        record.StoreAndFwdFlag = ConvertToYesNo(record.StoreAndFwdFlag);
                        records.Add(record);
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add(ex.Message);
                    }
                }
            }

            WriteErrorMessagesToFile(errorMessages);


            var groupedDuplicates = records
                    .GroupBy(r => new { r.PickupDateTime, r.DropoffDateTime, r.PassengerCount })
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g);

            duplicateRecords.AddRange(groupedDuplicates);

            WriteDuplicatesToCsv(duplicateRecords);

            return records.Except(duplicateRecords).ToList();
        }

        private string ConvertToYesNo(string value)
        {
            if (value == "N")
            {
                return "No";
            }
            else if (value == "Y")
            {
                return "Yes";
            }
            else
            {
                return value;
            }
        }

        private void WriteDuplicatesToCsv(List<DataModel> duplicateRecords)
        {
            using (var writer = new StreamWriter(Path.Combine(_savePath, "duplicates.csv")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(duplicateRecords);
            }
        }

        private void WriteErrorMessagesToFile(List<string> errorMessages)
        {
            string errorFilePath = Path.Combine(_savePath, "error_records.txt");
            File.WriteAllLines(errorFilePath, errorMessages);
        }
    }
}

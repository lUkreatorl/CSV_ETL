namespace CSV_ETL.Services
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Microsoft.Data.SqlClient;

    public class DBService
    {
        private string _conString;

        public DBService()
        {
            _conString = Environment.GetEnvironmentVariable("MyConString", EnvironmentVariableTarget.User) ?? "";
        }

        public void BulkInsert(List<DataModel> records)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PickupDateTime", typeof(DateTime));
            dataTable.Columns.Add("DropoffDateTime", typeof(DateTime));
            dataTable.Columns.Add("PassengerCount", typeof(int));
            dataTable.Columns.Add("TripDistance", typeof(double));
            dataTable.Columns.Add("StoreAndFwdFlag", typeof(string));
            dataTable.Columns.Add("PULocationID", typeof(int));
            dataTable.Columns.Add("DOLocationID", typeof(int));
            dataTable.Columns.Add("FareAmount", typeof(decimal));
            dataTable.Columns.Add("TipAmount", typeof(decimal));

            foreach (var record in records)
            {
                dataTable.Rows.Add(
                    record.PickupDateTime.ToUniversalTime(),
                    record.DropoffDateTime.ToUniversalTime(),
                    record.PassengerCount,
                    record.TripDistance,
                    record.StoreAndFwdFlag,
                    record.PULocationID,
                    record.DOLocationID,
                    record.FareAmount,
                    record.TipAmount
                );
            }
            
            using (SqlConnection connection = new SqlConnection(_conString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "FormatedData"; // Set the destination table name
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}

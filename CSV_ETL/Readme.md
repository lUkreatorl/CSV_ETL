
# CSV Parser 

 ETL application in CLI program for parse CSV file.

## Environment Variables

To run this project, you will need follow next steps.

1. Add connection string to the database. For it please add environment variables with name **"MyConString"** on your Windows. You can use next command in terminal

`setx MyConString "your connection string"`

2. Run the script on your MSSQL Server it will create database **"CSVParser"** with table **"FormatedData"**

3. Build project and enter full path to CSV file (you can just drop file to CLI).

#### **NOTE: Dont forget check duplicated and broken records in folder with main CSV file**

Count of rows after insertion was 29730.

##### **P.S.** 
I added broken records because I don't know what to do with records where missed data about count of passenger.
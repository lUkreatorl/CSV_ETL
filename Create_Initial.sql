GO
CREATE DATABASE CSVParser
GO
use CSVParser CREATE TABLE FormatedData(
    tpep_pickup_datetime DATETIMEOFFSET,
    tpep_dropoff_datetime DATETIMEOFFSET,
    passenger_count INT,
    trip_distance DECIMAL(10, 2),
    store_and_fwd_flag CHAR(3),
    PULocationID INT,
    DOLocationID INT,
    fare_amount DECIMAL(10, 2),
    tip_amount DECIMAL(10, 2)
);

DROP TABLE IF EXISTS resources;

CREATE TABLE IF NOT EXISTS resources (
    url varchar(64) PRIMARY KEY
    , data_schema varchar(8192)
);

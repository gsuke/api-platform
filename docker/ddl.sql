DROP TABLE IF EXISTS resources;

CREATE TABLE IF NOT EXISTS resources (
    url varchar(64) PRIMARY KEY
    , data_schema varchar(8192)
);

INSERT
    INTO
    resources
VALUES
(
    'markets'
    , 'test-dataschema1'
);

INSERT
    INTO
    resources
VALUES
(
    'wish-list'
    , 'test-dataschema2'
);
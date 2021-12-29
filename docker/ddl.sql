DROP TABLE IF EXISTS resource;
CREATE TABLE IF NOT EXISTS resource (
    id uuid PRIMARY KEY,
    url varchar(32)
);
INSERT
    INTO
    resource (
        id
        , url
    )
VALUES
(
    '8771a522-ca5c-fe20-a745-56d5e5bfee01'
    , 'hello'
)
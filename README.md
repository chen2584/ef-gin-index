# ef-gin-index

for study propose.

### Note
```
SELECT COUNT(*)::INT
      FROM "Bars" AS x
      WHERE (x."Guid" LIKE ANY ('{%111%, %ข้าวอยู่ในนา%}'))
      
select pg_relation_size('users_search_idx1')

CREATE EXTENSION IF NOT EXISTS pg_trgm;
CREATE INDEX users_search_idx2 ON "Foos" USING gin ("Guid" gin_trgm_ops);
```

### Set Default
```
select * from pg_opclass where opcname = 'gin_trgm_ops';
update pg_opclass set opcdefault = true where opcname='gin_trgm_ops'
```
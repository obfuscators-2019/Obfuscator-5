# Obfuscator
Obfuscates code on SQL by using selected columns from CSV files

SELECT tc.CONSTRAINT_CATALOG, tc.CONSTRAINT_SCHEMA, tc.CONSTRAINT_TYPE, tc.CONSTRAINT_NAME,
tc.TABLE_CATALOG,tc.TABLE_SCHEMA,tc.TABLE_NAME,
ccu.COLUMN_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
	JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON tc.CONSTRAINT_NAME = ccu.Constraint_name
WHERE tc.CONSTRAINT_TYPE = 'Primary Key'
ORDER BY tc.CONSTRAINT_SCHEMA, tc.CONSTRAINT_NAME

SELECT schema_name(ta.schema_id) SchemaName 
	,ta.name TableName
	,ind.name
	,indcol.key_ordinal Ord
	,col.name ColumnName
from sys.tables ta
	inner join sys.indexes ind on ind.object_id = ta.object_id
	inner join sys.index_columns indcol on indcol.object_id = ta.object_id and indcol.index_id = ind.index_id
	inner join sys.columns col on col.object_id = ta.object_id and col.column_id = indcol.column_id
where ind.is_primary_key = 1 OR ind.is_unique = 1
order by schema_name(ta.schema_id),ta.name,indcol.key_ordinal

SELECT [schema] = s.name, [table] = t.name
FROM sys.schemas AS s
	INNER JOIN sys.tables AS t ON s.[schema_id] = t.[schema_id]
WHERE EXISTS 
(
  SELECT 1 FROM sys.identity_columns
    WHERE [object_id] = t.[object_id]
);

SELECT OBJECT_SCHEMA_NAME(id) + '.' + OBJECT_NAME(id) as TableName, Name as IdentityColumn
FROM syscolumns
WHERE COLUMNPROPERTY( id ,name, 'IsIdentity') = 1
ORDER BY 1, 2

SELECT *
FROM Information_Schema.Columns
WHERE TABLE_SCHEMA + '.' + TABLE_NAME = 'Sales.TarjetasPrueba'
ORDER BY ORDINAL_POSITION

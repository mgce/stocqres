database=Stocqres
password=Your_password123

echo 'Please wait while SQL Server 2017 warms up'
/opt/mssql-tools/bin/sqlcmd -S localhost,20 -U sa -P $password -d master -i InitDb.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,20 -U sa -P $password -d $database -i IdentityScript.sql
echo 'Finished initializing the database'
echo 'Please wait while SQL Server 2017 warms up'
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Your_password123 -d master -i IdentityScript.sql
echo 'Finished initializing the database'
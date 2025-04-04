# Run Away Api

## Setup Database and Migrations

Create database to store the data on postgresql
```
CREATE Database yourdatabase
```

Add connection string to the environment variable using

```
$env:DB_CONNECTION_STRING = "Host=localhost;Port=5432;Database=yourdatabase;Username=youruser;Password=yourpassword"
```

Run migrations through script already provided ef.ps1 or ef.sh depend on your OS using command
```
./ef.ps1 migrations add InitialCreate
./ef.ps1 database update
```
# BBMS

.NET Core Web Api application which stores, updates and retrieves different types of beer, their associated breweries and bars that serve the beers.
This project is with N-Layer Architecture.

Domain
Entities Layer created for database tables.

Repsoitory
Repsoitory layer is used to create Data folder, created to perform database CRUD operations.

Layers:
Services
Services Layer created to process or control the incoming information according to the required conditions.

WebAPI
Web API Layer that opens the business layer to the internet.

Tests
Tests Layer to create and test the project functinality.

# change the connection string in Appsettings.json file
 "ConnectionStrings": {
    "DefaultConnection": "Data Source=.\\SQLEXPRESS; Initial Catalog=DatabaseName; user id = UserName; password=******; Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;"
  },

Entity Framework Core Migrations
CLI Based Execution
All Entity Framework migration commands must be excuted from inside Visual Studio's Package Manager Console or the CLI. CLI integration easy for CI systems to interact with the migrations to deploy them to an environment or generate data change scripts at build time.

PM> Add-Migration m1
# Applying Migrations to Database

PM>Update-Database
# Will connect to the database and apply any migrations that have not already been applied.

Creating and Destroying Migrations
dotnet ef migrations add MeaningfullName
# Will look at the current state of our model and generate migration for any changes from the previous migration.

dotnet ef migrations remove MeaningfullName
# Will attempt to remove the migration from the project.

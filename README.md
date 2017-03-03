# RentalTracker
MVC project to provide a House Rental Account tracker.

Uses NET 4.5 / MVC 5 / Entity Framework 6 / SQL Server 2013.

Developed in Visual Studio 2015, using Code First Migrations.

The project is configured in demo mode, which creates a new instance of the database, with some seeded data, each time the application is re-started.

To remove this, edit Application_Start() in Global.asax.cs and remove the call to DataHelper.NewDb().

To then create a new instance of the database, set DesignTimeHelpers as the Startup Project. Then from the Package Manager Console, select a default projet of RentalTracker.DAL and exectute 'update-database'. This will create a MSSQLLocalDB database with the RentalTracker schema.

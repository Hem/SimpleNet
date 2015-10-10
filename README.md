# SimpleNet
A simplified implimentation of .net repository. <br />
A simplified implementation of .net wcf services. <br />


# Simplified Database Access

I have written this package with hopes of simplifing database access using traditional ADO.NET application processes. I would higly recommend you download the project and review the BestPractices.cs in SimpleNet.Data.UnitTest


* Create a connection string in App.config or Web.config
* Create an instance of new SimpleDataAccess("Connection String")
* Execute a Read to read data into a DataTable from the database.


Best Practices
* I would recommend using the IRepository<T> pattern to handle your database calls.
* Extend AbstractSimpleSqlRepository be sure to provide an instance of SimpleDataAccess to the Database property.
* The AbstractSimpleSqlRepository contains methods to read, execute, executescalar and getDbParameters



Enjoy
- Hem


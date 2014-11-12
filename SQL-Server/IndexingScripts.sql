A Creating a simple nonclustered index
IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'IX_ProductVendor_VendorID')
    DROP INDEX IX_ProductVendor_VendorID ON Purchasing.ProductVendor;
GO
CREATE INDEX IX_ProductVendor_VendorID 
    ON Purchasing.ProductVendor (VendorID);
    
    
    
B. Creating a simple nonclustered composite index
The following example creates a nonclustered composite index on the SalesQuota and SalesYTD columns of the Sales.SalesPerson table in the AdventureWorks2012 database.
IF EXISTS (SELECT name FROM sys.indexes
            WHERE name = N'IX_SalesPerson_SalesQuota_SalesYTD')
    DROP INDEX IX_SalesPerson_SalesQuota_SalesYTD ON Sales.SalesPerson ;
GO
CREATE NONCLUSTERED INDEX IX_SalesPerson_SalesQuota_SalesYTD
    ON Sales.SalesPerson (SalesQuota, SalesYTD);
GO


C. Creating a unique nonclustered index
The following example creates a unique nonclustered index on the Name column of the Production.UnitMeasure table in the AdventureWorks2012 database. The index will enforce uniqueness on the data inserted into the Name column.
IF EXISTS (SELECT name from sys.indexes
             WHERE name = N'AK_UnitMeasure_Name')
    DROP INDEX AK_UnitMeasure_Name ON Production.UnitMeasure;
GO
CREATE UNIQUE INDEX AK_UnitMeasure_Name 
    ON Production.UnitMeasure(Name);

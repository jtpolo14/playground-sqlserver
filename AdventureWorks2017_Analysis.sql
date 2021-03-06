-- drill into the target db
USE [AdventureWorks2017]
GO

-- using while and cursor
DECLARE @year As int, @month As int, @sales As int;
PRINT '-------- Vendor Products Report --------'; 
DECLARE myCursor CURSOR FOR
SELECT TOP 5 year(OrderDate) As Salesyear, month(OrderDate) as 'SalesMonth', sum(TotalDue) As 'Sales' from Sales.SalesOrderHeader
group by year(OrderDate), month(OrderDate)
order by 'Sales' DESC;

OPEN myCursor
FETCH NEXT from myCursor
INTO @year, @month, @sales
	while @@FETCH_STATUS = 0
	begin
	PRINT CAST(@year AS varchar(5)) + ' ' +  CAST(@month AS varchar(5)) + ' Sales: ' +  CAST(@sales AS varchar(50))
	FETCH NEXT from myCursor
	INTO @year, @month, @sales
	end
CLOSE myCursor
DEALLOCATE myCursor


-- sub procedure to get the sales by year and month
CREATE Procedure [dbo].[getSalesByYearAndMonth]
	@year as int, @month as int
AS
BEGIN
	-- get most profitable sales by year and month
	SELECT TOP 5 year(OrderDate) As Salesyear, month(OrderDate) as 'SalesMonth', sum(TotalDue) As 'Sales' from Sales.SalesOrderHeader
	WHERE year(OrderDate) = @year and month(OrderDate) = @month
	group by year(OrderDate), month(OrderDate)
	order by 'Sales' DESC
END;
GO

-- call a user defined stored proc
EXEC [dbo].[getSalesByYearAndMonth] 2012, 2
GO

-- get most profitable sales by year and month
SELECT TOP 5 year(OrderDate) As Salesyear, month(OrderDate) as 'SalesMonth', sum(TotalDue) As 'Sales' from Sales.SalesOrderHeader
group by year(OrderDate), month(OrderDate)
order by 'Sales' DESC

-- get most profitable sales by year
SELECT year(OrderDate) as 'year', sum(TotalDue) AS AnnualSales from sales.SalesOrderHeader
group by year(OrderDate)
order by AnnualSales DESC

-- confirm all orders with null salespersionid are online orders
-- should return 0 rows
SELECT * from Sales.SalesOrderHeader
where SalesPersonID is null
and OnlineOrderFlag != 1

-- get most valuable customers by purchases breakdown
Select Top 5 CustomerID As PersonID, sum(TotalDue) as TotalPurchases from Sales.SalesOrderHeader
Group by CustomerID
order by TotalPurchases DESC

-- get most valuable salespersons by sales breakdown
-- note: exclude online sales
-- use inner join
SELECT TOP 5 isnull(s.SalesPersonID, 99999) AS PersonID, p.FirstName, p.LastName, sum(s.TotalDue) As TotalSales FROM  Sales.SalesOrderHeader s
inner join Person.Person p on p.BusinessEntityID = s.SalesPersonID
where SalesPersonID != 99999
group by s.SalesPersonID, p.FirstName, p.LastName
order by TotalSales DESC

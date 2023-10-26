CREATE DATABASE JaensGadgetGarage;

-- Create the 'Alerts' table
CREATE TABLE Alerts (
    AlertDateTime DATETIME,
    Message VARCHAR(250)
);
go

-- Create the 'Audit' table
CREATE TABLE [Audit] (
    ID INT PRIMARY KEY IDENTITY(1,1),
    auditDate DATE,
    message VARCHAR(250)
);
go

-- Create the 'Contacts' table with few tuples of info
CREATE TABLE Contacts (
    Phone VARCHAR(15) PRIMARY KEY,
    Name VARCHAR(255),
    Company VARCHAR(255),
    Email VARCHAR(255),
    Address VARCHAR(255),
    Image VARCHAR(255)
);
go

INSERT INTO Contacts (Phone, Name, Company, Email, Address, Image) VALUES ('111-111-1111', 'John Doe', 'ABC Company', 'john.doe@example.com', '123 Main St', 'https://i.pinimg.com/564x/9e/83/75/9e837528f01cf3f42119c5aeeed1b336.jpg');
INSERT INTO Contacts (Phone, Name, Company, Email, Address, Image) VALUES ('222-222-2222', 'Jane Smith', 'XYZ Corporation', 'jane.smith@example.com', '456 Oak St', 'https://i.pinimg.com/564x/9e/83/75/9e837528f01cf3f42119c5aeeed1b336.jpg');
INSERT INTO Contacts (Phone, Name, Company, Email, Address, Image) VALUES ('333-333-3333', 'Bob Johnson', '123 Industries', 'bob.johnson@example.com', '789 Elm St', 'https://i.pinimg.com/564x/9e/83/75/9e837528f01cf3f42119c5aeeed1b336.jpg');
INSERT INTO Contacts (Phone, Name, Company, Email, Address, Image) VALUES ('444-444-4444', 'Sarah Brown', 'Tech Solutions', 'sarah.brown@example.com', '101 Pine St', 'https://i.pinimg.com/564x/9e/83/75/9e837528f01cf3f42119c5aeeed1b336.jpg');
INSERT INTO Contacts (Phone, Name, Company, Email, Address, Image) VALUES ('555-555-5555', 'Mike Wilson', 'Acme Corporation', 'mike.wilson@example.com', '222 Cedar St', 'https://i.pinimg.com/564x/9e/83/75/9e837528f01cf3f42119c5aeeed1b336.jpg');
go

-- Create the 'Inventory' table with few tuples of info
CREATE TABLE Inventory (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Product VARCHAR(255),
    Category VARCHAR(50),
    Brand VARCHAR(50),
    Stock INT,
    Price DECIMAL(18, 2)
);
go

INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('Smartphone X', 'Mobile Phones', 'BrandX', 100, 499.99);
INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('Tablet Y', 'Tablets', 'BrandY', 50, 299.99);
INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('Laptop Z', 'Laptops', 'BrandZ', 30, 899.99);
INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('Headphones A', 'Audio Accessories', 'BrandA', 200, 49.99);
INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('Camera B', 'Cameras', 'BrandB', 15, 799.99);
INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('Smartwatch C', 'Wearable Tech', 'BrandC', 75, 199.99);
INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('Gaming Console D', 'Gaming', 'BrandD', 40, 299.99);
INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('External Hard Drive E', 'Storage', 'BrandE', 25, 79.99);
INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('Monitor F', 'Monitors', 'BrandF', 10, 249.99);
INSERT INTO Inventory (Product, Category, Brand, Stock, Price) VALUES ('Keyboard G', 'Computer Accessories', 'BrandG', 50, 29.99);
go

-- Create the Orders' table
CREATE TABLE Orders (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Product VARCHAR(255),
    Quantity INT,
    Price DECIMAL(18, 2),
    Total AS (Quantity * Price)
);
go

-- Create the 'Sales' table
CREATE TABLE Sales (
    OrdersDate DATE,
    TotalSales DECIMAL(18, 2)
);
go

INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-10-27', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-10-28', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-10-29', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-10-30', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-10-31', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-11-01', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-11-02', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-11-03', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-11-04', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-11-05', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-11-06', 0.00);
INSERT INTO Sales (OrdersDate, TotalSales) VALUES ('2023-11-07', 0.00);
go


USE [master]
GO
CREATE DATABASE HR_Management
GO

-- DROP DATABASE HR_Management

USE HR_Management
GO

-- Create tables

CREATE TABLE [dbo].[social_insurance](
	[social_insurance_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[issue_date] [datetime] NOT	NULL,
	[issue_place] [nvarchar](50) NOT NULL,
	[registered_medical_facility] [nvarchar](100) NOT NULL,
	[notes] [nvarchar](50) NULL,
) 
GO

CREATE TABLE [dbo].[qualification](
	[qualification_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[qualification_name] [nvarchar](50) NOT NULL,
	[notes] [nvarchar](100) NULL
);
GO

CREATE TABLE [dbo].[expertise](
	[expertise_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[expertise_name] [nvarchar](50) NOT NULL,
	[notes] [nvarchar](100) NULL
);
GO

CREATE TABLE [dbo].[unit](
	[unit_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[unit_name] [nvarchar](100) NOT NULL,
	[notes] [nvarchar](100) NULL
);
GO

CREATE TABLE [dbo].[project](
	[project_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[project_name] NVARCHAR(100) NOT NULL,
	[start_date] DATETIME NOT NULL,
	[end_date] DATETIME NOT NULL,
	[status] NVARCHAR(50) NOT NULL,
	[notes] NVARCHAR(200) NULL
);
GO

CREATE TABLE [dbo].[salary](
	[salary_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[expertise_id] INT,
	[qualification_id] INT,
	[unit_id] INT,
	[basic_salary] [int] NULL,
	[new_basic_salary] [int] NULL,
	[entry_date] [datetime] NULL,
	[modify_date] [datetime] NULL,
	[notes] [nvarchar](100) NULL,

	CONSTRAINT FK_salary_expertise FOREIGN KEY ([expertise_id]) REFERENCES [dbo].[expertise]([expertise_id]),
	CONSTRAINT FK_salary_qualification FOREIGN KEY ([qualification_id]) REFERENCES [dbo].[qualification]([qualification_id]),
	CONSTRAINT FK_salary_unit FOREIGN KEY ([unit_id]) REFERENCES [dbo].[unit]([unit_id])
);
GO



CREATE TABLE [dbo].[personal_income_tax](
	[tax_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[tax_authority] [nvarchar](100) NOT NULL,
	[salary_id] INT,
	[amount] [int] NOT NULL,
	[registration_date] [datetime] NOT NULL,
	[notes] [nvarchar](50) NULL,

	FOREIGN KEY ([salary_id]) REFERENCES [dbo].[salary]([salary_id])
)
GO



CREATE TABLE [dbo].[employee](
	[employee_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[full_name] [nvarchar](50) NOT NULL,
	[date_of_birth] [datetime] NOT NULL,
	[gender] [nvarchar](6) NOT NULL,
	[id_card_number] [char](12) NOT NULL,
	[place_of_birth] [nchar](100) NOT NULL,
	[address] [nvarchar](100) NOT NULL,
	[phone_number] [char](12) NOT NULL,
	[qualification_id] INT,
	[social_insurance_id] INT,
	[salary_id] INT,
	[unit_id] INT,
	[project_id] INT,
	[tax_id] INT,
	[expertise_id] INT,
	[email] [nvarchar](100) NOT NULL,
	[password] [nvarchar](30) NOT NULL,
	[permission] [int] NOT NULL,
	[image] [nvarchar](MAX) NULL,
	[notes] [nvarchar](100) NULL,
	[ethnicity] [nvarchar](30) NULL,
	[religion] [nvarchar](30) NULL,
	[nationality] [nvarchar](30) NULL,
	FOREIGN KEY ([qualification_id]) REFERENCES [dbo].[qualification]([qualification_id]),
	FOREIGN KEY ([social_insurance_id]) REFERENCES [dbo].[social_insurance]([social_insurance_id]),
	FOREIGN KEY ([salary_id]) REFERENCES [dbo].[salary]([salary_id]),
	FOREIGN KEY ([unit_id]) REFERENCES [dbo].[unit]([unit_id]),
	FOREIGN KEY ([project_id]) REFERENCES [dbo].[project]([project_id]),
	FOREIGN KEY ([tax_id]) REFERENCES [dbo].[personal_income_tax]([tax_id]),
	FOREIGN KEY ([expertise_id]) REFERENCES [dbo].[expertise]([expertise_id]),
)
GO

CREATE TABLE [dbo].[month](
	[month_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[month_name] Nvarchar(200) NOT NULL,
	[notes] [nvarchar](200) NULL
) 
GO


CREATE TABLE [dbo].[salary_statistic](
	[salary_statistic_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[employee_id] INT NOT NULL,
	[month_id] INT NOT NULL,
	[basic_salary] INT NULL,
	[tax_to_pay] INT NULL,
	[bonus] INT NULL,
	[fine] INT NULL,
	[total_salary] INT NULL,
	[notes] [nvarchar](200) NULL,
	[create_date] datetime NULL,
	FOREIGN KEY ([employee_id]) REFERENCES [dbo].[employee]([employee_id]),
	FOREIGN KEY ([month_id]) REFERENCES [dbo].[month]([month_id])
) 
GO

-- Insert information to tables


INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'January', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'February', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'March', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'April', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'May', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'June', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'July', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'August', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'September', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'October', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'November', N'No')
INSERT [dbo].[month] ( [month_name],[notes]) VALUES ( N'December', N'No')


INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES ('2018-02-16', N'TP Quy Nhon', N'BV Đa Khoa Bình Định', N'No')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES ('2018-03-24', N'TP Quy Nhon', N'BV 13 QK5', N'No')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES ('2018-04-10', N'TP Quy Nhon', N'BV Phong Quy Hòa', N'No')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES ('2018-05-27', N'TP Quy Nhon', N'BV Đa Khoa Hòa Bình', N'No')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES ('2018-06-18', N'TP Quy Nhon', N'BV Đa Khoa Tỉnh Mở Rộng', N'No')


INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Human Resources Manager', N'Responsible for HR management')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'CEO', N'Chief Executive Officer')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Accountancy', N'Handles company accounts')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Sales Representative', N'Responsible for sales')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Marketing Specialist', N'Handles marketing tasks')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'IT Specialist', N'Handles IT related issues')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Receptionist', N'Front desk operations')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Procurement Specialist', N'Handles procurement tasks')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Supply Chain Analyst', N'Analyzes supply chain')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Production Manager', N'Oversees production')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Customer Service Manager', N'Handles customer service')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES (N'Product Development Specialist', N'Develops new products')


INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Human Resources Department', N'Manages human resources')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Executive Management Department', N'Handles executive management')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Finance - Accounting Department', N'Oversees finance and accounting')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Sales Department', N'Manages sales operations')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Marketing Department', N'Handles marketing operations')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'IT Department', N'Manages IT operations')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Office', N'General office operations')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Materials Department', N'Manages material supplies')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Planning - Distribution Department', N'Plans and distributes resources')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Production Management Department', N'Manages production process')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Customer Service Department', N'Handles customer inquiries')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES (N'Research and Development Department', N'Oversees R&D activities')


INSERT INTO [dbo].[project] (project_name, start_date, end_date, status, notes)
VALUES 
(N'Product Development Project A', '2024-01-01', '2024-12-31', N'In Progress', N'Developing a new product for the domestic market'),
(N'Production Process Optimization', '2024-02-15', '2024-08-15', N'In Progress', N'Reducing costs and increasing production efficiency'),
(N'Southern Market Expansion', '2024-03-01', '2024-09-30', N'In Progress', N'Implementing a new business campaign in the southern region'),
(N'ERP System Improvement Project', '2024-01-15', '2024-06-30', N'Completed', N'Improving and upgrading the current ERP system'),
(N'European Market Research', '2024-04-01', '2024-10-31', N'In Progress', N'Researching the potential for expanding into the European market'),
(N'Customer Relationship Management (CRM) Implementation', '2024-05-01', '2024-11-30', N'In Progress', N'Deploying a new CRM system to enhance customer relations'),
(N'Office Renovation', '2024-06-01', '2024-07-31', N'Planned', N'Renovating the main office to improve working conditions'),
(N'Supply Chain Efficiency Project', '2024-07-01', '2024-12-31', N'In Progress', N'Optimizing the supply chain to reduce delays and costs'),
(N'New Marketing Strategy', '2024-08-01', '2024-12-31', N'In Progress', N'Developing and implementing a new marketing strategy for the upcoming year'),
(N'IT Infrastructure Upgrade', '2024-09-01', '2025-02-28', N'In Progress', N'Upgrading the company’s IT infrastructure to improve performance and security');


INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'C', N'College')
INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'U', N'University')
INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'MD', N'Master Degree')
INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'PhD', N'Doctor of Philoshophy')
INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'P', N'Professor')

INSERT [dbo].[salary] ([expertise_id], [basic_salary], [qualification_id], [entry_date], [new_basic_salary], [modify_date], [notes], [unit_id]) 
VALUES 
(1, 45000000, 1, '2021-03-05', 0, '2021-04-05', N'No', 1),
(2, 16000000, 2, '2021-03-18', 15000000, '2021-04-18', N'Promote', 2),
(3, 8000000, 1, '2021-05-17', 12000000, '2021-06-17', N'Promote', 3),
(4, 6000000, 1, '2021-08-15', 0, '2021-09-15', N'No', 4),
(5, 12000000, 1, '2021-10-20', 15000000, '2021-11-20', N'Promote', 5),
(6, 20000000, 3, '2021-12-01', 0, '2022-01-01', N'No', 6),
(7, 9000000, 2, '2022-01-10', 12000000, '2022-02-10', N'Promote', 7),
(8, 11000000, 1, '2022-02-15', 13000000, '2022-03-15', N'Promote', 8),
(9, 9500000, 2, '2022-03-20', 11500000, '2022-04-20', N'Promote', 9),
(10, 14000000, 2, '2022-04-25', 16000000, '2022-05-25', N'Promote', 10),
(11, 18000000, 3, '2022-05-30', 0, '2022-06-30', N'No', 11),
(12, 17000000, 3, '2022-06-05', 0, '2022-07-05', N'No', 12);


INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES ( N'Cục Thuế Bình Định', 1, 800000, '2020-07-19', N'No')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế Cần Thơ', 2, 750000, '2020-08-23', N'No')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế Đà Nẵng', 3, 350000, '2020-09-26', N'No')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế Phú Yên', 4, 550000, '2020-10-30', N'No')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế Hồ Chí Minh', 5, 900000, '2020-11-17', N'No')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế Hà Nội', 6, 950000, '2020-11-29', N'No')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế Quảng Ngãi', 7, 700000, '2020-12-09', N'No')


INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [project_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permission]) 
VALUES 
(N'Trần Huy Hoàng', '2003-02-14', N'Male', N'215487745', N'Bình Định', N'TP QN', N'0913201503 ', 3, 1, 1, 1, 3, 1, 1, N'huyhoangero@gmail.com', N'Không ', N'Kinh', N'Không', N'Việt Nam', '123456',1),
(N'Lê Việt Thy', '2003-11-22', N'Female', N'242523652', N'Phú Yên', N'TP Tuy Hòa', N'0822543757 ', 3, 2, 2, 2, 3, 2, 2, N'levietthy03@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam', '123456',1),
(N'Phan Phương Sinh', '2003-05-02', N'Male', N'212823367', N'Bình Định', N'TP Quy Nhơn', N'0522991730 ', 3, 3, 3, 3, 2, 3, 3, N'sinh123123444@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam','123456',2),
(N'Nguyễn Ngô Chiến', '2003-07-01', N'Male', N'212823367', N'Bình Định', N'TP Quy Nhơn', N'0978177739 ', 3, 4, 4, 7, 1, 4, 4, N'chienlag1@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam', '123456',3),
(N'Phan Quốc Đại', '2003-03-20', N'Male', N'216849367', N'Bình Định', N'TP Quy Nhơn', N'0976697093 ', 3, 4, 8, 7, 5, 4, 5, N'daiphan245@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam', '123456',2),
(N'Phan Trần Lê Nguyễn', '2003-12-31', N'Male', N'286848367', N'Cần Thơ', N'TP Cần Thơ', N'1900100688 ', 5, 4, 6, 7, 5, 4, 4, N'ptln1@gmail.com', N'Không', N'Tày', N'Không', N'Việt Nam', '123456',1);


-- Insert more rows to the table employee
DECLARE @i INT = 7;

WHILE @i <= 100
BEGIN
    DECLARE @full_name NVARCHAR(50) = N'Employee ' + CAST(@i AS NVARCHAR(3));
    DECLARE @date_of_birth DATE = DATEADD(YEAR, -24, GETDATE());
    DECLARE @gender NVARCHAR(6) = CASE WHEN @i % 2 = 0 THEN N'Male' ELSE N'Female' END;
    DECLARE @id_card_number CHAR(12) = CAST(210000000 + @i AS CHAR(12));
    DECLARE @place_of_birth NCHAR(30);
    DECLARE @address NVARCHAR(100);
    DECLARE @phone_number CHAR(12) = '026' + RIGHT('000000' + CAST(@i AS VARCHAR(6)), 6);
    DECLARE @qualification_id INT = (1 + @i % 5);
    DECLARE @social_insurance_id INT = (1 + @i % 5);
    DECLARE @salary_id INT = (1 + @i % 12);
    DECLARE @unit_id INT = (1 + @i % 12);
    DECLARE @project_id INT = (1 + @i % 10);
    DECLARE @tax_id INT = (1 + @i % 7);
    DECLARE @expertise_id INT = (1 + @i % 12);
    DECLARE @email NVARCHAR(100) = N'employee' + CAST(@i AS NVARCHAR(3)) + N'@company.com';
    DECLARE @notes NVARCHAR(100) = N'No';
    DECLARE @ethnicity NVARCHAR(30) = N'Kinh';
    DECLARE @religion NVARCHAR(30) = N'Không';
    DECLARE @nationality NVARCHAR(30) = N'Việt Nam';
    DECLARE @password NVARCHAR(30) = N'123';
    DECLARE @permission INT;

	-- Assign permission 
	IF @i % 11 = 0 SET @permission = 2;
	ELSE SET @permission = 3;

    -- Assign place_of_birth and address
    IF @i % 3 = 0 AND @i % 4 = 0 AND @i % 7 = 0
    BEGIN
        SET @place_of_birth = N'Quảng Ngãi';
        SET @address = N'TP Quảng Ngãi';
    END
    ELSE IF @i % 3 = 0
    BEGIN
        SET @place_of_birth = N'Gia Lai';
        SET @address = N'TP Pleiku';
    END
    ELSE IF @i % 7 = 0
    BEGIN
        SET @place_of_birth = N'Phú Yên';
        SET @address = N'TP Tuy Hòa';
    END
    ELSE
    BEGIN
        SET @place_of_birth = N'Bình Định';
        SET @address = N'TP Quy Nhơn';
    END

    INSERT [dbo].[employee] 
    ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], 
    [social_insurance_id], [salary_id], [unit_id], [project_id], [tax_id], [expertise_id], [email], [notes], 
    [ethnicity], [religion], [nationality], [password], [permission])
    VALUES 
    (@full_name, @date_of_birth, @gender, @id_card_number, @place_of_birth, @address, @phone_number, @qualification_id, 
    @social_insurance_id, @salary_id, @unit_id, @project_id, @tax_id, @expertise_id, @email, @notes, 
    @ethnicity, @religion, @nationality, @password, @permission);

    SET @i = @i + 1;

END

INSERT [dbo].[salary_statistic] ([employee_id], [month_id], [basic_salary], [tax_to_pay], [bonus], [fine], [total_salary], [notes], [create_date]) 
VALUES 
(1, 3, 45000000, 800000, 500000, 0, 44700000, 'No', '2024-06-30'),
(2, 5, 16000000, 750000, 500000, 0, 15750000, 'No', '2024-06-30'),
(3, 7, 8000000, 350000, 520000, 200000, 7970000, 'Late 20%', '2024-07-01'),
(4, 2, 6000000, 550000, 500000, 0, 5950000, 'No', '2024-06-30'),
(5, 7, 11000000, 550000, 600000, 100000, 10950000, 'Late 10%', '2024-06-30')
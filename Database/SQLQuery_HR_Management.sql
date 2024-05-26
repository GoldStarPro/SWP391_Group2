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
	[issue_date] [datetime] NULL,
	[issue_place] [nvarchar](50) NULL,
	[registered_medical_facility] [nvarchar](100) NULL,
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
	[unit_name] [nvarchar](100) NULL,
	[notes] [nvarchar](100) NULL
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
	[tax_authority] [nvarchar](100) NULL,
	[salary_id] INT,
	[amount] [int] NULL,
	[registration_date] [datetime] NULL,
	[notes] [nvarchar](50) NULL,

	FOREIGN KEY ([salary_id]) REFERENCES [dbo].[salary]([salary_id])
)
GO



CREATE TABLE [dbo].[employee](
	[employee_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[full_name] [nvarchar](50) NULL,
	[date_of_birth] [datetime] NULL,
	[gender] [nvarchar](6) NULL,
	[id_card_number] [char](12) NULL,
	[place_of_birth] [nchar](10) NULL,
	[address] [nvarchar](100) NULL,
	[phone_number] [char](12) NULL,
	[qualification_id] INT,
	[social_insurance_id] INT,
	[salary_id] INT,
	[unit_id] INT,
	[tax_id] INT,
	[expertise_id] INT,
	[email] [nvarchar](100) NULL,
	[password] [nvarchar](30) NULL,
	[permisson] [int] NOT NULL,
	[image] [nvarchar](MAX) NULL,
	[notes] [nvarchar](100) NULL,
	[ethnicity] [nvarchar](30) NULL,
	[religion] [nvarchar](30) NULL,
	[nationality] [nvarchar](30) NULL,
	FOREIGN KEY ([qualification_id]) REFERENCES [dbo].[qualification]([qualification_id]),
	FOREIGN KEY ([social_insurance_id]) REFERENCES [dbo].[social_insurance]([social_insurance_id]),
	FOREIGN KEY ([salary_id]) REFERENCES [dbo].[salary]([salary_id]),
	FOREIGN KEY ([unit_id]) REFERENCES [dbo].[unit]([unit_id]),
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
	[allowance] INT NULL,
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

INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A60E00000000 AS DateTime), N'TP QN', N'BV Đa Khoa Bình Định', N'Không có')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A60100000000 AS DateTime), N'TP QN', N'BV 13 QK5', N'Không có')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A69700000000 AS DateTime), N'TP QN', N'BV Phong Quy Hòa', N'Không có')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A60A00000000 AS DateTime), N'TP QN', N'BV Đa Khoa Hòa Bình', N'Không có')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A58000000000 AS DateTime), N'TP QN', N'BV Đa Khoa Tỉnh Mở Rộng', N'Không có')

INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES ( N'Sales', N'No')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES ( N'CEO', N'No')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES ( N'Accountancy', N'No')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES ( N'Manager', N'No')

INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Office', N'No')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Human Resources Department', N'No')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Finance - Accounting Department', N'No')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Materials Department', N'No')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Planning - Distribution Department', N'No')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Production Management Department', N'No')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Bussiness Department', N'Không')

INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'C', N'College')
INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'Pg', N'Postgraduate')
INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'U', N'University')

INSERT [dbo].[salary] ([expertise_id], [basic_salary], [qualification_id], [entry_date], [new_basic_salary], [modify_date], [notes], [unit_id]) VALUES ( 1, 45000000, 1, CAST(0x0000A60E00000000 AS DateTime), 0, CAST(0x0000A60E00000000 AS DateTime), N'No',1)
INSERT [dbo].[salary] ([expertise_id], [basic_salary], [qualification_id], [entry_date], [new_basic_salary], [modify_date], [notes], [unit_id]) VALUES (2, 10000000, 2, CAST(0x0000A60100000000 AS DateTime), 15000000, CAST(0x0000A60200000000 AS DateTime), N'Promote',2)
INSERT [dbo].[salary] ([expertise_id], [basic_salary], [qualification_id], [entry_date], [new_basic_salary], [modify_date], [notes], [unit_id]) VALUES ( 3, 6000000, 1, CAST(0x0000A60100000000 AS DateTime), 12000000, CAST(0x0000A60100000000 AS DateTime), N'Promote',3)
INSERT [dbo].[salary] ([expertise_id], [basic_salary], [qualification_id], [entry_date], [new_basic_salary], [modify_date], [notes], [unit_id]) VALUES ( 4, 8000000, 1, CAST(0x0000A58000000000 AS DateTime), 0, CAST(0x0000A58000000000 AS DateTime), N'No',4)

INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES ( N'Cục Thuế Bình Định', 1, 8000000, CAST(0x0000A60E00000000 AS DateTime), N'Không Có')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế Bình Định', 2, 750000, CAST(0x0000A6DA00000000 AS DateTime), N'Không Có')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế Bình Định', 3, 350000, CAST(0x0000A6DA00000000 AS DateTime), N'Không Có')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế Phú Yên', 4, 550000, CAST(0x0000A6DA00000000 AS DateTime), N'Không Có')

INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permisson]) 
VALUES (N'Trần Huy Hoàng', CAST(0x0000884600000000 AS DateTime), N'Male', N'215487745   ', N'Bình Định', N'TP QN', N'0913201503 ', 3, 1, 1, 2, 1, 1, N'huyhoangero@gmail.com', N'Không ', N'Kinh', N'Không', N'Việt Nam', '123456',1)
INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permisson]) 
VALUES (N'Lê Việt Thy', CAST(0x0000A60100000000 AS DateTime), N'Female', N'242523652   ', N'Phú Yên', N'TP Tuy Hòa', N'0822543757 ', 3, 2, 2, 3, 2,2, N'levietthy03@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam', '123456',1)
INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permisson]) 
VALUES (N'Phan Phương Sinh', CAST(0x0000806800000000 AS DateTime), N'Male', N'212823367   ', N'Bình Định', N'TP Quy Nhơn', N'0522991730 ', 3, 3, 3, 1, 3, 3, N'sinh123123444@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam','123456',2)
INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permisson]) 
VALUES (N'Nguyễn Ngô Chiến', CAST(0x0000806800000000 AS DateTime), N'Male', N'212823367   ', N'Bình Định', N'TP Quy Nhơn', N'0978177739 ', 3, 4, 4, 7, 4, 4, N'chienlag1@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam', '123456',3)
INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permisson]) 
VALUES (N'Phan Quốc Đại', CAST(0x0000806800000000 AS DateTime), N'Male', N'216849367   ', N'Bình Định', N'TP Quy Nhơn', N'0976697093 ', 3, 4, 4, 7, 4, 4, N'DaiQPQE170228@fpt.edu.vn', N'Không', N'Kinh', N'Không', N'Việt Nam', '123456',2)



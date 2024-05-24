USE [master]
GO
CREATE DATABASE HR_Management
GO

-- DROP DATABASE HR_Management

USE HR_Management
GO

CREATE TABLE [dbo].[social_insurance](
	[social_insurance_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[issue_date] [datetime] NULL,
	[issue_place] [nvarchar](50) NULL,
	[registered_medical_facility] [nvarchar](50) NULL,
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
	[unit_name] [nvarchar](50) NULL,
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
	[full_name] [nvarchar](30) NULL,
	[date_of_birth] [datetime] NULL,
	[gender] [nvarchar](3) NULL,
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


CREATE TABLE [dbo].[salary_statistics](
	[salary_statistics_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[employee_id] INT NOT NULL,
	[month_id] INT NOT NULL,
	[basic_salary] INT Null,
	[tax_to_pay] INT Null,
	[bonus] INT Null,
	[allowance] INT Null,
	[total_salary] INT Null,
	[notes] [nvarchar](200) NULL,
	[create_date] datetime NULL,
	FOREIGN KEY ([employee_id]) REFERENCES [dbo].[employee]([employee_id]),
	FOREIGN KEY ([month_id]) REFERENCES [dbo].[month]([month_id])
) 
GO



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

INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A60E00000000 AS DateTime), N'TP HCM', N'BV Tân Phú', N'Không có')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A60100000000 AS DateTime), N'TP HCM', N'BV Tân Phú', N'Không có')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A69700000000 AS DateTime), N'TP HCM', N'BV Chợ Rẫy', N'Không có')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A60A00000000 AS DateTime), N'TP HCM', N'BV Nhi Đồng', N'Không có')
INSERT INTO [dbo].[social_insurance] ([issue_date], [issue_place], [registered_medical_facility], [notes]) VALUES (CAST(0x0000A58000000000 AS DateTime), N'TP HCM', N'BV Thống Nhất', N'BV Thống Nhất')

INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES ( N'Bán Hàng', N'Không')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES ( N'CEO', N'Không')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES ( N'Kế Toán', N'Không')
INSERT [dbo].[expertise] ( [expertise_name], [notes]) VALUES ( N'Quản Lý', N'Không')

INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Văn Phòng', N'Không')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Phòng Tổ chức lao động', N'Không')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Phòng Kế toán - Tài chính', N'Không')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Phòng Vật tư', N'Không')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Phòng Kế hoạch - Tiêu thụ', N'Không')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Phòng Điều hành sản xuất', N'Không')
INSERT [dbo].[unit] ( [unit_name], [notes]) VALUES ( N'Phòng Kinh Doanh', N'Không')

INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'CĐ', N'Cao Đẳng')
INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'CH', N'Cao Học')
INSERT [dbo].[qualification] ([qualification_name], [notes]) VALUES (N'ĐH', N'Đại Học')

INSERT [dbo].[salary] ([expertise_id], [basic_salary], [qualification_id], [entry_date], [new_basic_salary], [modify_date], [notes], [unit_id]) VALUES ( 1, 45000000, 1, CAST(0x0000A60E00000000 AS DateTime), 0, CAST(0x0000A60E00000000 AS DateTime), N'Không',1)
INSERT [dbo].[salary] ([expertise_id], [basic_salary], [qualification_id], [entry_date], [new_basic_salary], [modify_date], [notes], [unit_id]) VALUES (2, 10000000, 2, CAST(0x0000A60100000000 AS DateTime), 15000000, CAST(0x0000A60200000000 AS DateTime), N'Lên Chức',2)
INSERT [dbo].[salary] ([expertise_id], [basic_salary], [qualification_id], [entry_date], [new_basic_salary], [modify_date], [notes], [unit_id]) VALUES ( 3, 6000000, 1, CAST(0x0000A60100000000 AS DateTime), 12000000, CAST(0x0000A60100000000 AS DateTime), N'Xấu trai',3)
INSERT [dbo].[salary] ([expertise_id], [basic_salary], [qualification_id], [entry_date], [new_basic_salary], [modify_date], [notes], [unit_id]) VALUES ( 4, 8000000, 1, CAST(0x0000A58000000000 AS DateTime), 0, CAST(0x0000A58000000000 AS DateTime), N'Không',4)

INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES ( N'Cục Thuế Hà Nội', 1, 8000000, CAST(0x0000A60E00000000 AS DateTime), N'Không Có')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế TPHCM', 2, 750000, CAST(0x0000A6DA00000000 AS DateTime), N'Không Có')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế TPHCM', 3, 350000, CAST(0x0000A6DA00000000 AS DateTime), N'Không Có')
INSERT [dbo].[personal_income_tax] ([tax_authority], [salary_id], [amount], [registration_date], [notes]) VALUES (N'Cục Thuế TPHCM', 4, 550000, CAST(0x0000A6DA00000000 AS DateTime), N'Không Có')

INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permisson]) 
VALUES (N'Nguyễn Dũng', CAST(0x0000884600000000 AS DateTime), N'Nam', N'215487745   ', N'Phú Thọ', N'TP HCM', N'01256985471 ', 3, 1, 1, 2, 1, 1, N'dung@gmail.com', N'Đẹp Trai ', N'Kinh', N'Không', N'Việt Nam', '123456',1)
INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permisson]) 
VALUES (N'Vũ Như Tuấn Hùng', CAST(0x0000A60100000000 AS DateTime), N'Nam', N'242523652   ', N'Hưng Yên', N'TP HCM', N'01665655214 ', 3, 2, 2, 3, 2,2, N'vuhung@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam', '123456',1)
INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permisson]) 
VALUES (N'Nguyễn Thị Mạnh Hùng', CAST(0x0000806800000000 AS DateTime), N'Nữ', N'212823367   ', N'Phú Thọ', N'Hà Nội', N'01667899877 ', 3, 3, 3, 1, 3, 3, N'thihung@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam','123456',3)
INSERT [dbo].[employee] ([full_name], [date_of_birth], [gender], [id_card_number], [place_of_birth], [address], [phone_number], [qualification_id], [social_insurance_id], [salary_id], [unit_id], [tax_id], [expertise_id], [email], [notes], [ethnicity], [religion], [nationality], [password],[permisson]) 
VALUES (N'Nguyễn Thị Đạt', CAST(0x0000806800000000 AS DateTime), N'Nữ', N'212823367   ', N'Hưng Yên', N'Hà Nội', N'01667899877 ', 3, 4, 4, 7, 4, 4, N'dat@gmail.com', N'Không', N'Kinh', N'Không', N'Việt Nam', '123456',2)

--INSERT [dbo].[tblUser] ([email], [password], [permisson]) VALUES (N'dung@gmail.com', N'admin', 1)
--INSERT [dbo].[tblUser] ([email], [password], [permisson]) VALUES (N'vuhung@gmail.com', N'admin', 2)
--INSERT [dbo].[tblUser] ([email], [password], [permisson]) VALUES (N'thihung@gmail.com', N'admin', 3)
--INSERT [dbo].[tblUser] ([email], [password], [permisson]) VALUES (N'dat@gmail.com', N'admin', 1)


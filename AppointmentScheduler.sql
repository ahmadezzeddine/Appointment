USE [AppointmentScheduler]
GO
/****** Object:  Table [dbo].[tblAdministrator]    Script Date: 1/31/2018 10:49:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAdministrator](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](250) NOT NULL,
	[LastName] [nvarchar](250) NOT NULL,
	[Password] [nvarchar](250) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[ContactNumber] [nvarchar](50) NULL,
	[IsAdmin] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[AdministratorId] [bigint] NULL,
 CONSTRAINT [PK_Administrator] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblAppointment]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAppointment](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GlobalAppointmentId] [bigint] NOT NULL,
	[BusinessServiceId] [bigint] NULL,
	[Title] [nvarchar](max) NULL,
	[PatternType] [int] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[IsRecuring] [bit] NOT NULL,
	[IsAllDayEvent] [bit] NOT NULL,
	[TextColor] [int] NULL,
	[BackColor] [int] NULL,
	[RecureEvery] [int] NULL,
	[EndAfter] [int] NULL,
	[EndAfterDate] [datetime] NULL,
	[StatusType] [int] NULL,
	[CancelReason] [ntext] NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [date] NOT NULL,
	[BusinessOfferId] [bigint] NULL,
	[ServiceLocationId] [bigint] NULL,
	[BusinessEmployeeId] [bigint] NULL,
	[BusinessCustomerId] [bigint] NULL,
	[ScheduleId] [bigint] NULL,
 CONSTRAINT [PK_tblAppointment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblAppointmentDocument]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAppointmentDocument](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[DocumentType] [int] NULL,
	[IsEmployeeUpload] [bit] NOT NULL,
	[DocumentLink] [nvarchar](max) NULL,
	[DocumentCategoryId] [bigint] NULL,
	[AppointmentId] [bigint] NULL,
 CONSTRAINT [PK_AppointmentDocument] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblAppointmentFeedback]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAppointmentFeedback](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IsEmployee] [bit] NOT NULL,
	[BusinessEmployeeId] [bigint] NULL,
	[BusinessCustomerId] [bigint] NULL,
	[Rating] [int] NULL,
	[Feedback] [ntext] NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[AppointmentId] [bigint] NULL,
 CONSTRAINT [PK_tblAppointmentFeedback] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblAppointmentInvitee]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAppointmentInvitee](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BusinessEmployeeId] [bigint] NULL,
	[AppointmentId] [bigint] NULL,
 CONSTRAINT [PK_tblAppointmentInvitee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblAppointmentPayment]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAppointmentPayment](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IsPaid] [bit] NOT NULL,
	[PaidDate] [datetime] NOT NULL,
	[Amount] [money] NULL,
	[BillingType] [int] NOT NULL,
	[PurchaseOrderNo] [nvarchar](250) NULL,
	[ChequeNumber] [nvarchar](250) NULL,
	[CardType] [int] NULL,
	[CCFirstName] [nvarchar](250) NULL,
	[CCLastName] [nvarchar](250) NULL,
	[CCardNumber] [nvarchar](250) NULL,
	[CCSecurityCode] [nvarchar](250) NULL,
	[CCExpirationDate] [datetime] NULL,
	[Created] [datetime] NOT NULL,
	[AppointmentId] [bigint] NULL,
 CONSTRAINT [PK_AppointmentPayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusiness]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusiness](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[ShortName] [nvarchar](50) NULL,
	[Logo] [nvarchar](max) NULL,
	[PhoneNumbers] [nvarchar](250) NULL,
	[FaxNumbers] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Website] [nvarchar](250) NULL,
	[Add1] [nvarchar](250) NULL,
	[Add2] [nvarchar](250) NULL,
	[City] [nvarchar](250) NULL,
	[State] [nvarchar](250) NULL,
	[Zip] [nvarchar](50) NULL,
	[CountryId] [int] NULL,
	[IsInternational] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[MembershipId] [int] NULL,
	[BusinessCategoryId] [int] NULL,
	[TimezoneId] [int] NULL,
 CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusinessCategory]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [nvarchar](250) NULL,
	[PictureLink] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[OrderNumber] [int] NULL,
	[AdministratorId] [bigint] NULL,
	[ParentId] [int] NULL,
 CONSTRAINT [PK_BusinessCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusinessCustomer]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessCustomer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](250) NOT NULL,
	[LastName] [nvarchar](250) NOT NULL,
	[ProfilePicture] [image] NULL,
	[StdCode] [int] NULL,
	[PhoneNumber] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Add1] [nvarchar](250) NULL,
	[Add2] [nvarchar](250) NULL,
	[City] [nvarchar](250) NULL,
	[State] [nvarchar](250) NULL,
	[Zip] [nvarchar](10) NULL,
	[Password] [nvarchar](250) NOT NULL,
	[Created] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[TimezoneId] [int] NULL,
	[ServiceLocationId] [bigint] NULL,
 CONSTRAINT [PK_tblBusinessCustomer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusinessEmployee]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessEmployee](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](250) NOT NULL,
	[LastName] [nvarchar](250) NOT NULL,
	[Email] [nvarchar](250) NOT NULL,
	[STD] [int] NULL,
	[PhoneNumber] [nvarchar](250) NULL,
	[Password] [nvarchar](250) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[ServiceLocationId] [bigint] NULL,
 CONSTRAINT [PK_BusinessEmployee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusinessHolidays]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessHolidays](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OnDate] [date] NOT NULL,
	[Type] [int] NOT NULL,
	[ServiceLocationId] [bigint] NULL,
 CONSTRAINT [PK_tblBusinessHolidays] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusinessHours]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessHours](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[WeekDayId] [int] NOT NULL,
	[IsStartDay] [bit] NOT NULL,
	[IsHoliday] [bit] NOT NULL,
	[From] [datetime] NOT NULL,
	[To] [datetime] NOT NULL,
	[IsSplit1] [bit] NULL,
	[FromSplit1] [datetime] NULL,
	[ToSplit1] [datetime] NULL,
	[IsSplit2] [bit] NULL,
	[FromSplit2] [datetime] NULL,
	[ToSplit2] [datetime] NULL,
	[ServiceLocationId] [bigint] NULL,
 CONSTRAINT [PK_BusinessHours] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusinessOffer]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessOffer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [ntext] NULL,
	[Code] [nvarchar](100) NULL,
	[ValidFrom] [date] NOT NULL,
	[ValidTo] [date] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[BusinessEmployeeId] [bigint] NULL,
 CONSTRAINT [PK_tblBusinessOffer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusinessOfferServiceLocation]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessOfferServiceLocation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[BusinessOfferId] [bigint] NULL,
	[ServiceLocationId] [bigint] NULL,
 CONSTRAINT [PK_tblBusinessOfferServiceLocation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusinessService]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBusinessService](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [ntext] NULL,
	[Cost] [money] NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[EmployeeId] [bigint] NULL,
 CONSTRAINT [PK_tblBusinessService] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCountry]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCountry](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[ISO] [nvarchar](10) NULL,
	[ISO3] [nvarchar](10) NULL,
	[CurrencyName] [nvarchar](50) NULL,
	[CurrencyCode] [nvarchar](50) NULL,
	[PhoneCode] [int] NOT NULL,
	[AdministratorId] [bigint] NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblDocumentCategory]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblDocumentCategory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[OrderNo] [int] NOT NULL,
	[PictureLink] [nvarchar](max) NULL,
	[Type] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[IsParent] [bit] NOT NULL,
	[ParentId] [bigint] NULL,
 CONSTRAINT [PK_DocumentCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblMembership]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMembership](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Benifits] [ntext] NULL,
	[Price] [money] NULL,
	[IsUnlimited] [bit] NOT NULL,
	[TotalEmployee] [int] NOT NULL,
	[TotalCustomer] [int] NOT NULL,
	[TotalAppointment] [int] NOT NULL,
	[TotalLocation] [int] NOT NULL,
	[TotalOffers] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[AdministratorId] [bigint] NULL,
 CONSTRAINT [PK_Membership] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblSchedule]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSchedule](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [ntext] NOT NULL,
	[Interval] [int] NOT NULL,
	[Duration] [int] NOT NULL,
	[Capacity] [int] NOT NULL,
	[From] [date] NULL,
	[To] [date] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblSchedule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblServiceLocation]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblServiceLocation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Add1] [nvarchar](250) NULL,
	[Add2] [nvarchar](250) NULL,
	[City] [nvarchar](250) NULL,
	[State] [nvarchar](250) NULL,
	[Zip] [nvarchar](10) NULL,
	[CountryId] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[TimezoneId] [int] NULL,
	[BusinessId] [bigint] NULL,
 CONSTRAINT [PK_tblBusinessLocation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblTimezone]    Script Date: 1/31/2018 10:49:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTimezone](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[UtcOffset] [int] NOT NULL,
	[IsDST] [bit] NOT NULL,
	[CountryId] [int] NULL,
	[AdministratorId] [bigint] NULL,
 CONSTRAINT [PK_Timezone] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[tblAdministrator] ON 

INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (40, N'Test1', N'User', N'N74vW75gn1CfOZJ/+l2bvA==', N'test1@gmail.com', NULL, 1, 1, CAST(0x0000A8230046A7E6 AS DateTime), 0)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (44, N'Dev', N'Test', N'N74vW75gn1CfOZJ/+l2bvA==', N'test@gmail.com', NULL, 0, 0, CAST(0x0000A824010F31DC AS DateTime), 0)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (47, N'UserTest', N'Test', N'N74vW75gn1CfOZJ/+l2bvA==', N'test9@gmail.com', N'9909980330', 1, 1, CAST(0x0000A828012AD438 AS DateTime), 39)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (48, N'TestUser1', N'Test', N'N74vW75gn1CfOZJ/+l2bvA==', N'test10@gmail.com', N'9909980330', 1, 1, CAST(0x0000A828012C8EE0 AS DateTime), 39)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (49, N'TEst10', N'User', N'N74vW75gn1CfOZJ/+l2bvA==', N'test10@gmail.com', NULL, 1, 1, CAST(0x0000A828012F0500 AS DateTime), 39)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (50, N'TEst10', N'User', N'N74vW75gn1CfOZJ/+l2bvA==', N'test11@gmail.com', NULL, 1, 1, CAST(0x0000A828012F286D AS DateTime), 39)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (51, N'User12', N'Test', N'N74vW75gn1CfOZJ/+l2bvA==', N'user12@gmail.com', NULL, 1, 1, CAST(0x0000A8290062E3A0 AS DateTime), 39)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (52, N'Devendra', N'Gohel', N'N74vW75gn1CfOZJ/+l2bvA==', N'dev.gohel@gmail.com', NULL, 0, 1, CAST(0x0000A839001F3130 AS DateTime), 0)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (53, N'Deve', N'Gohel', N'N74vW75gn1CfOZJ/+l2bvA==', N'dev.test@gmail.com', NULL, 0, 1, CAST(0x0000A83900216FDA AS DateTime), 0)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (54, N'Sagar', N'Tester', N'N74vW75gn1CfOZJ/+l2bvA==', N'admin.test@gmail.com', NULL, 0, 1, CAST(0x0000A85C012CA997 AS DateTime), NULL)
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (55, N'Tester', N'Devednra', N'N74vW75gn1CfOZJ/+l2bvA==', N'dev.tester@gmail.com', NULL, 0, 0, CAST(0x0000A86500B9EA36 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[tblAdministrator] OFF
SET IDENTITY_INSERT [dbo].[tblAppointment] ON 

INSERT [dbo].[tblAppointment] ([Id], [GlobalAppointmentId], [BusinessServiceId], [Title], [PatternType], [StartTime], [EndTime], [IsRecuring], [IsAllDayEvent], [TextColor], [BackColor], [RecureEvery], [EndAfter], [EndAfterDate], [StatusType], [CancelReason], [IsActive], [Created], [BusinessOfferId], [ServiceLocationId], [BusinessEmployeeId], [BusinessCustomerId], [ScheduleId]) VALUES (9, 0, 3, N'Video Call 30 Minute 2', 1, CAST(0x0000A81001545CCE AS DateTime), CAST(0x0000A81001545CCE AS DateTime), 1, 0, 123445, 123456, 1, 1, CAST(0x0000A81001545CCE AS DateTime), 0, N'', 1, CAST(0x6B3D0B00 AS Date), 1, 1, 1, 3, NULL)
INSERT [dbo].[tblAppointment] ([Id], [GlobalAppointmentId], [BusinessServiceId], [Title], [PatternType], [StartTime], [EndTime], [IsRecuring], [IsAllDayEvent], [TextColor], [BackColor], [RecureEvery], [EndAfter], [EndAfterDate], [StatusType], [CancelReason], [IsActive], [Created], [BusinessOfferId], [ServiceLocationId], [BusinessEmployeeId], [BusinessCustomerId], [ScheduleId]) VALUES (10, 0, 3, N'Video Call 30 Minute', 0, CAST(0x0000A81001545CCE AS DateTime), CAST(0x0000A81001545CCE AS DateTime), 1, 0, 123445, 123456, 1, 1, CAST(0x0000A81001545CCE AS DateTime), 0, N'', 1, CAST(0x6B3D0B00 AS Date), 1, 1, 1, 3, NULL)
SET IDENTITY_INSERT [dbo].[tblAppointment] OFF
SET IDENTITY_INSERT [dbo].[tblAppointmentDocument] ON 

INSERT [dbo].[tblAppointmentDocument] ([Id], [Title], [DocumentType], [IsEmployeeUpload], [DocumentLink], [DocumentCategoryId], [AppointmentId]) VALUES (1, N'Law document for case', 1, 1, N'://www.documetndodasf.com/asdf.img', 1, 9)
SET IDENTITY_INSERT [dbo].[tblAppointmentDocument] OFF
SET IDENTITY_INSERT [dbo].[tblAppointmentFeedback] ON 

INSERT [dbo].[tblAppointmentFeedback] ([Id], [IsEmployee], [BusinessEmployeeId], [BusinessCustomerId], [Rating], [Feedback], [IsActive], [Created], [AppointmentId]) VALUES (2, 1, 1, 3, 1, N'sample string 3', 1, CAST(0x0000A811003CBCC0 AS DateTime), 9)
INSERT [dbo].[tblAppointmentFeedback] ([Id], [IsEmployee], [BusinessEmployeeId], [BusinessCustomerId], [Rating], [Feedback], [IsActive], [Created], [AppointmentId]) VALUES (3, 1, 1, 3, 1, N'sample string 3', 1, CAST(0x0000A811003CBCC0 AS DateTime), 9)
INSERT [dbo].[tblAppointmentFeedback] ([Id], [IsEmployee], [BusinessEmployeeId], [BusinessCustomerId], [Rating], [Feedback], [IsActive], [Created], [AppointmentId]) VALUES (4, 1, 1, NULL, 1, N'awesome appointment', 1, CAST(0x0000A811003CBCC0 AS DateTime), 9)
SET IDENTITY_INSERT [dbo].[tblAppointmentFeedback] OFF
SET IDENTITY_INSERT [dbo].[tblAppointmentInvitee] ON 

INSERT [dbo].[tblAppointmentInvitee] ([Id], [BusinessEmployeeId], [AppointmentId]) VALUES (1, 1, 9)
SET IDENTITY_INSERT [dbo].[tblAppointmentInvitee] OFF
SET IDENTITY_INSERT [dbo].[tblAppointmentPayment] ON 

INSERT [dbo].[tblAppointmentPayment] ([Id], [IsPaid], [PaidDate], [Amount], [BillingType], [PurchaseOrderNo], [ChequeNumber], [CardType], [CCFirstName], [CCLastName], [CCardNumber], [CCSecurityCode], [CCExpirationDate], [Created], [AppointmentId]) VALUES (2, 1, CAST(0x0000A811003239B7 AS DateTime), 1.0000, 4, N'sample string 5', N'sample string 6', 1, N'', N'', N'', N'', CAST(0x0000A811008CDCDB AS DateTime), CAST(0x0000A811003239BB AS DateTime), 9)
SET IDENTITY_INSERT [dbo].[tblAppointmentPayment] OFF
SET IDENTITY_INSERT [dbo].[tblBusiness] ON 

INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (1, N'ABC COMPANY', N'ABC', N'', N'(079)-0022121', N'(079_-0022121', N'lectroondev@gmail.com', N'www.abc.com', N'03-aaf adf asf asdf', N'asdf saf sadf sdf0', N'test', N'test state', N'232323', 2, 1, 1, CAST(0x0000A80F0079E181 AS DateTime), 1, 1, 1)
INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (12, N'Spactron', N'spact', NULL, N'9909980330', NULL, N'lectroondev@gmail.com', N'www.spactron.com', N'This is address 1', N'This is address 2', N'Lebanon', N'Lebanon', N'456478', 3, 1, 0, CAST(0x0000A83800E11B42 AS DateTime), 1, 5, 6)
INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (13, N'Spactron2', N'spact2', NULL, N'9909980330', NULL, N'lectroondev@gmail.com', N'www.spactron.com', N'This is address 1', N'This is address 2', N'Lebanon', N'Lebanon', N'456478', 3, 1, 0, CAST(0x0000A83800E31AF3 AS DateTime), 1, 5, 6)
INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (14, N'Spactron2', N'spact2', NULL, N'9909980330', NULL, N'lectroondev@gmail.com', N'www.spactron.com', N'This is address 1', N'This is address 2', N'Lebanon', N'Lebanon', N'456478', 3, 1, 0, CAST(0x0000A83800E31BCF AS DateTime), 1, 5, 6)
INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (15, N'Spactron4', N'spact', NULL, N'9909980330', N'End Solution', N'lectroondev@gmail.com', N'End Solution', N'2122. sfs. Dodo', N'2122. sfs. Dodo', N'sdf', N'VT', N'23423', 1, 0, 0, CAST(0x0000A83800E98CE2 AS DateTime), 1, 1, 1)
INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (16, N'Spactron6', N'asdfasdf', NULL, N'9909980330', N'End Solution', N'lectroondev@gmail.com', N'End Solution', N'2122. sfs. Dodo', N'2122. sfs. Dodo', N'sdf', N'VT', N'23423', 2, 0, 0, CAST(0x0000A83800F2AE5A AS DateTime), 1, 1, 1)
INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (17, N'Lectroon', N'solution', NULL, N'9909980330', N'End Solution', N'lectroondev@gmail.com', N'End Solution', N'2122. sfs. Dodo', N'2122. sfs. Dodo', N'sdf', N'VT', N'23423', 2, 0, 0, CAST(0x0000A838011C8D32 AS DateTime), 1, 4, 4)
INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (10007, N'Lectroon Solution', N'lectroon', NULL, N'9909980330', N'9909980330', N'lectroondev@gmail.com', N'www.devgohel.com', N'asdfdsf', N'asdfdsdaf asfdf', N'ahmedabad', N'gujarat', N'382350', 2, 0, 0, CAST(0x0000A84800F0E642 AS DateTime), 3, 5, 1)
SET IDENTITY_INSERT [dbo].[tblBusiness] OFF
SET IDENTITY_INSERT [dbo].[tblBusinessCategory] ON 

INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (1, N'Business Category 1', N'This is sample business category', N'A', N'', 1, CAST(0x0000A80F008977EE AS DateTime), 1, 1, 2)
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (2, N'Education', N'This is sample business category.', N'A1', N'data:image/jpeg;base64,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', 1, CAST(0x0000A80F008977EE AS DateTime), 1, 52, NULL)
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (3, N'Consultant', N'NA', N'A1', N'data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wgARCAD6APoDASIAAhEBAxEB/8QAHAAAAQQDAQAAAAAAAAAAAAAAAAIDBgcBBAUI/8QAGwEBAAIDAQEAAAAAAAAAAAAAAAEFAgQGAwf/2gAMAwEAAhADEAAAAffgBnOMgAAAAAAAAAAAAAAACRQAAAAAAkAzlIKEgoSChIKEgoSChODKkJS6MiHhsFpxrmwvR2xwSChKgAEgAAYEgrCQUIBY2Dg2Iczq6p0U8zoi8pwlxCBHLe22WL72ltslqaVEu5QuWQBIYMYEmU4bQ5jSfQ9hnLJxtGvEaWo12HlxOvuR3JJ1IMPZ3LOMz2Ixo54TevJtCt3U25vS3a9FhdSobS0djey09r+uVIVM4SpIhl7lTixyers+njG5OhvAp5lPnsPIbzJzDSpiLVTdfn/qKP0q3S0V0duczqlbU9/B+pbdqGz0WpBHdi8rO11onra/tfPaWfPeu3WeN1fDZ3XsLnzG1tpQy4yiNVbYtRdBU38nERpLSTUpAOR2POenoLJ97nrTzAvcR9I47e6PDdsNbqO8hXph0Yt10+c7e9xl549VfHdyx7Gk3s6m13uzG+3yPQei3NR/hen2FtOTKmXWzU0nKb3deb0b14p3HLSyrXuff1O5rarXR07Egj71PYdZXR3cfPS6fRdpbLMJnEA39V/OmvoqvaVqGM7qtLHn6dXeVaXO2sC3bloult/THTj0i4Dq9nKHMM8tusnOhFg6W1r0jV14VF9G42FaNgPW9dCZpXk71c88GTxzU2OtLa9lep71bZSUWWjFtSxYNs+eirv+ic8fLM570+pN7zz6Bru9aq2rqJ375T8vS4avuOnbXS9Cb1ETnlL+5dqLSemsXG3GYy0q3sehLmt4ulxY/wDQeOflUak1fsx3o8vi2WhMYbxupv63V5Miv6msfOWnbUfyd12cs8zd+bvQ/m+6utpKd9MUdctLvyOO1xGtfO36yiG71dJPOZzO7EyK2ad9HcR1LfTac5S+fYfYjLWp63YhaaPmvS6EJ+wfOrFqr0hy+WuKtnk15On67nGmUioriBWLX8e9MN/pa7u5rd9NZ1lZ6nqLz1FHeopO1zUuXNdlSntjzztJ2tTYvGX1dcnx76HR1z1pY3vjOFC+Wu3dd/mec6em+rwnzvU3pbzv9q+c3V5p9ep5215kG59g+WbEMrrHR1Fpx+COWuivU23Lut2ptIr0+edf4lTM+B2/Mae1ec25W78qTWNelfWKea9IK5ToKHvtTvN3HHkjb3h6vqS55eoy+1i16fuCorXThde2Pwvo3G8V3e7UxUz8rL6qirkt6PnnBtjvXrW7vm9PorhaPvSvrGr7c4rqfMVsWErLX1eT3HqS38tXjLtm3r2nsLorQdHplTuHRbmFCEuJNWOydvPzjtQ3xWd9VdCZQCyKzepHk2d3OhqItLOm3z1rQdjIklnrdF59PPWzSdgNVO2g13HObDe1+Zk2O3y+pLL6XgVlRlRkxhaRtt7ExqcvuHphx+m8eebA+Q11PJiEJW0lOcv4y0PYzMs7eEsavQyjn7GxgBSxKxRkMgoAxkE4XgQLwYwrIjKsAhecTS1YMYVmTbO0iWo4/lKMOCG8rBtYowZyYyZAAAAAAAAAAAAAAAAAMZwYAAAwvGQAAAAD/8QALhAAAAYCAQQBAwMEAwAAAAAAAAECAwQFBhESBxATIRQVIjEWIDAjJDJAJUFQ/9oACAEBAAEFAuxf+AX+rv8A9rY32IGDMgb4J/7wR9i/h2NjY2NjY2Njfc1DQ13UEc3gUczCEEXYv4j/AHbGxsbCpCEBc5KTKV5FJP1sGfrYMx6ISLBqGI9g1KBjYIwn+Aj9bHIchyGxsOPeNDl6jUmxWs43kWlMA3IpMJZHvtyG+3IWCOcrGEaml9pNWjK5Jeh+f3mZAvY37XLSRksjLl75dpaDfgx6Va1IhNpDkpERlOXQ1SdnrkFLHIheZOxRCDnEKabcpD6MhfJEqJki6axLIJFqRn8NUSSUtjlxBex/3vvv246TZP2PqXJc4RokhS1u7BGvlyHIaSPyjYy5o34c9ttUWMZlEmTmq9q3zmXKcxO1etKXLadMpt6uYW4xHXHMpZhpZ7hrJ5xWNoUxWQvp0PykkHM9o9/sMTW1KcZri0TREajHL2a9DfMc+I9EPJy7XJtnXWLh+KJ1jQ9XOzZNu95iIdP7uPZUGS3cdmG6/seYFIBPiZa/BJB822pTb6pLalWEeHoko4dzCgYyW1XAYeyedAdUvaf+59nHqGL3rGppzC8mLK8f6qHYqXxnoBTbghAfkmFyOQQ5ovMYWvkTtOn5VeyiG35h5gToJ4WbPzIdBmdk7V40Z1WSfgIUCG+ygrtlrP8AbyWzmO8eKcizCHjrdtcP5FLU6e+mqml4bmbKf08sy2StAnNDzDy8R5dg3RseTQ8o8gJ0JcDZ7Eb+kbTnB9KuYIwXZQV+XnPGlU1kk5HcMnX2FoqA9O6wPzY/NXl82wtwUN9cYeZ5FY2qyd5A1/a3WTHK/HqRF+k7GgpySv0pwEscwSx5ASwh0NL0Glhxwvj1q/LASE9jMGLP3Cnwy5vF8ZMw9h1ww6sE4Y3zBHsNLMyi0ZfT6ajJWUOdSpbs/GjavLuF03m/Hd/pu8gRjYSvauXtDpKDFBKeqqPpeibGcpqrGo0hzhExtzy0BfhJ77GDFiXKKxRfPVlFV9JkSl7N5z2RqknIx6bVwpeKUuPg49fGi5hERHubx34mJPxkxcoYPyM4F5q/IrWzkzpq23GT56CnCbLlssbwKZkcdMfxWWX9PGH6TBv+U6c0ObzY73Vlsvmvq/s8Gf8APiFnkjNVKjLQ6gGFBz8OH66i+kVFErIpl/isVFbOyhzHqC3yObfFfYrLyGZc1/6HxH5qbXCsqf3EvLMoFzP6fS5dtk1iVRYT84rY0i2t5N25iWOpyi2qul1VXo6oSKxaMTukY3096pUX0u8dyBFdRVlAxDl2DfwrrqO0U7HSLacSuZ9E9Ga+QMUeU7WbBhRiU94mZ+ckyWQ5C7cKrY7t9jUyjuqBvGW6qVgOQ5fXTa+3tZicBqHyziixeR8O+zVbSsml25za+iYsrh62r3aayz7D2Mdq+s8c22K23lUcno/cSLaHJQbb8rFHcg6Z5hjC7PB4731vpF0zzdHxcvcRIyaVmMqxrWi5CmZ80iPjCkprYRVzAMLElHlatTFge1JZkqKhzizauYdSq0qGOm70QTbFheLw1Osy6uslZlaxMDe/Wf1fGsYs6aTCjdXusEI2MuyQvrPSbLlfV+krPKWOikkk3mZwjgZdmmYfQcUxnqZKpEfqeWiKSS016JhJpDDYjT/pryDS6kuxhQc9DMYHxJc0/VDkX6btEZRjmOCHfzax7FMDn589kHQuRCg9CENSsdXiq8J6n5dObxDqfJr6HLBkOGM4RlnWijdmFGivVHSbpk8xcdPri/x+hrMWyNWLWF7dP3lj/mCIEEEGv8unlHHdrb+nYfrnYnyGcNtPqFOnsYWHPuLKK36nXWRCUriMMwGvx2i6nY3T2GKwrRWCdHejeZzbizwCKVFnMO5i5bP64QUvi66LSIx9VZx1+MY31liMU2c9TTymK3JkMxSYBhOtcNgiBNHptoIa0fRm4FurjUtM6PFqRyuWkhxBhw9FKlJaKPJ+SM3qvpdpKRtXXAjPEUxpMyJTwDy/o10SppSMoYuEq615lau4n1T6m51Dy6hidVLivhWdlItph/51lU9aPyunc+JBdTxImeYbj7BMHoke6bDpVpGVhE4h+lJTIw/DpTGVKaS83Ex5mI6n2C7OB8zUFRvMTUcmS6kxUuQpjJElda11MwDDsQLAscwrqSrGJ911miMRm5TjdlIUuU4mOCjgo/vxaPo/wclaJxOTw0NXLMXynjHT9pqLMxyE/BfTwXgswpeIq/x5+y9giBEEBPZRbCkgy9XuYzI8m5u5NwzIY++iyKfjB32Y2eRMlB4go3r4ux8LaUw/bENLpfB9fCMYzcO4pmikmlqXBdkT8UwxFehglDICcTTvwdjAq5ytxVZbSn8cdBJAiCSCS7GFhZFxuInNx+EaSkRBGqHZq3cLmNMOV5trTEBQjCsdeSwcH7qjEoDUCZgsVYkdO3eMTpoZzT+4eFJDWy/CyLilNeykGfIlFtJeggtgiCSBAuxgy9GXuZS/JdkVLbsB+Bwcw2M21C8JGd3B42rdcpw8fxbQt0f8ZJg8m+n89c2mcb5tpL0ZDiDSNDj9uvs9JC5TTQOcp1zQT+SGgRAu5hXsj9hwtImQvIvFzVXT0J8aZNS5JlV1EiGaS+6SwbkdyicdFDTHUsmjiSU/bocdjgGyHj5KkyXEGpSnFNRVbZrjMiYIJaJIItdi7a7GDBkFNczVTJM49QiO6adAm+I4mgGj3rQ1xGgrRBKfXEcRxDfoa2HoxvLRDSgJQRAy2OI0Ndtft0NAyBENDQ0NDj2MwZGCb2Eo4kZAi0NjiRjgCT24giGgRAiH4/druX7tDiNeu69gy5BCdd9fsLsX8Gu+v4dDiOI0NDQ0Ndtf7R/t1/tn/L//xAAtEQABAwQBAwMDAwUAAAAAAAABAAIDBAUREiETMUEQFCIgI1EGUGEVMDIzgf/aAAgBAwEBPwH9nys+uv18Lb0zhZyhqDgrVZBW7c4KqZMN4UNWNMqGXcZ+hxwiHOTGa9/UYVfKWShTXFkbFb60TOOVcJ9H5aU+se4d1TSEvChjaPK14+ipqHsk1CMzWN2epb4N9GqZ7vb9QL+sTZwpa18jslOnc7uoap0XZPqXSO5XVUcxGCqWuJeAs+s9S1irq77wIVwvEkvHhCfn4jlSXaqMWhUdJJ8SfKiipuqGt7q47NOJBj8IFbnOAhyMjwqO3Tv5PlGjFPI1Rn4+stMHnJVypz7jVibbdXEzHhU4i+Rhbkj8qpjB+weHEZUJJdo7woaboyGaX88KugBDn5zhW+1PqAHBUdsj9w5jjwFQ0LGPfE8KsD4Gs07K4vxO12U2uaAAE1wIz6V8z4hkI1eZsv8AKgDf8m85/KfJMwue/g+F7mWURuI+ecZ/hU9rlnL3POqlodaUkuzgqWmYY3Nx3CtM00c4j7BUBbHLL1SpbnT/AOwHxhS3V8kYjTTJLjKtUDXjLk3Hb0uMG0XCkkdHMMKpo5pIdg7HlR0lM1jYZOXOUVJF0mtHcFbSMe5oGQpJabZ9M/hT3aKGUanIAwqy8GV4dH4Uk0jjusHwogNuVTU8XtQf4Vsy12EB6OaNMK5wiOq/6qugdPI2Rpw3CE9MdJ3nlqnvGchn5yprtUvf1GnCk6j3buPKt9ijkp+o9SU33dI1F+n3mPqOKtVtE7y0o2GPKgpxGzRMha3t61cxYOFcMyu2KYKosw3sjTOB1chbiRlqpbUZeAnWOQdla4nsh0cqeztY8yJ8eWahUFtMBJytvpLNlW0zDhQMAjwEbd1JMptI1jNVb4Sxx/uOjDu6AwmjCxzlAAftX//EADIRAAEEAQIFAgIJBQAAAAAAAAEAAgMEEQUSBhATITEiQRQyFSAjMFFhcYGRQEJQwdH/2gAIAQIBAT8B/wAXj6/dY5HkInnuhjPqQIPYowv+ZqoR7pMOVjT5eqQPCsV+n2Q5tGVuDU5245WeRBC0qDqwHt3VbRJ5pDuGFq+mfDsG3ytCrGSP7QJumxtOWhXICI3PUr3O+ZD8uecKlTjkgLyo4HyP2Rqpwu4x73qCNjbvScPdfQFdwzhQ6cyFuGroDHpU1Fkg9YTKjWD0roqeAPaWq9pMbIyQsY51qhn+VaZpW2uY/crSeHooDn3XQwDuTNApsmMw8qbVYG7gO+P9KxdvGu5727R+K0R8BzseXFYRZnwnbW/Mr+uVIB5UN83GvDRgKRuHkc4bhiGGLRLwFXrTHwjr29jfhhkk4V+WwCxtqTaDnOFQfI0/F53NBx+ymrvw17f7t3/VY1H4yFtSEdyO/wCS0iwdzY+ntC1XiOCk7pOGXLUuIJzUbNGNpJWq6nM9kNiI/stIbFbkm3j2yuH2h1QtUulv3uJTm4ODy0inHO/aU7TgKmxpxhXTKAIpcNwQchMr1pjHHXy/ByUaEURmaXYjI8fgVb1itWijhibvI75VbUzJc6YbgObn88qvdlE0ZeflcQuJ6sUlN07cFwwtYElmGtHCFBw9dz0nDsDlU9CbXsOsN8H2QigremP3WvWnxSho905xc7J5aRaENgEqOKKxXMb/AAVU1OnDYMW0vBOMlSajfzJardmMOFPqM3XklHh47othkgieXbXYx/CgjvFsdyJu7Hb9QqnDVizXLpPS4uytO4abBE+KR27co6sMYaxo8IuaPKnf27K/Yn+kD+q1za8NJ88nvwg6QvBC0C26bTzu8rTdXhpV3QvZuk3KSC/H1K0bPTJ3/lVOGdkw6vy7cFVuG6ccXRcNwz7qJkcLQyPwta4qngt9FngKC9urtlJwrPF8TJumwZXEWsvrRMfH7ocWWFctunl6vhSzOkOXFFYytNrtkPdaVJHBGIwpXaayfqSAbkLrSOyk1SNvk4Wp6+yqBu75UXFVd3da5ZimtdVvdXNelkiELOwUUrWv3P7rVNZ+LY1pHhbfqsnczwtItO9RKuSl024oa2I4Q33Ul+SWYOetVtiVjcIePu2Sub4TnF3co9xy3FA/fHlj+p//xAA9EAABAwIDBQYEBAQFBQAAAAABAAIDBBESITEFEyJBURAUIDJSYUJxgZEjMDOhFUBDsQY0NVNiYGNwgsH/2gAIAQEABj8C/wDCWvgyWZAQ52/lsystEPn4uFcbg1cBv/Jl/II4WFx68lnZudsl5C7kow612rqszfxSGXidopyMmlquVugbu/LPJNbcFx7OXa9jfMVeY4LjMNQyxW6ouc4MaEY8RCz59vUprZM5HC9lbeYD7q8b2uXDY4gnO3e8ZI2xsi02iiOeWqZM2/AU2QfH+R7rhCF3ErE0WzvcoYRxLNuXaLHMo559jPQDmjbhdyKhB1EYRkmeGNb7o/w7DHFfJzhqopqjKe9sua71hG8aLFG7CHexVmVErW9LoDEXC3Nf8U0aXRaHahMivfCs7LJX8LC3XRcTj7o8ItrcrXtzWStoevRfD91opWyFgGHQlPtm8DhQYKV5qgMIFsrrfVsxlkGjOQVsWH/4mND2Y6cljxitYqWFskb5HN0BvbwxSYrYZWoOGjs05rJWyPj81in2biuBmuLXxaq0QGN6bKX3aXcQPRBw0Iuua3tTPHFH1cURs+j34afNJoUyswbtxJaW+4UAgldFTWzc3m7ov9Qm+6sNpy25IOqamSoePVy7dVrqnvjlkh3vmDeaOHV2pPhe3m79l3ORrYw3gEnOypsL3Fs7sL781YZfkCT0pkZ0kcAmj0iwCOOXHU24Ym5ozVjrsvwxcmo2OHL6Kn3RDrXvbqp8ZsAAW36rRcuzVZ5XWv5OlyoJL+SVpWLr4ib2X6zFJECHucOSbOwYjE/FhRbS0L4HuGb3nRGSV5klfq49sjNnzsMErsRa/wCEp79oVO8x/AzQdlyNEaju8m5GrrKZ0lS2lipwHueRe4RZHHPXTO8rvhH5PsVhbcokaA3UDvVGPE8L4vurDNFa6dmmfZ5rnSwF8SK2fUvlwsrp9za2bVPFMcUNC5zpD6g1CSMRtoNNzbItW1IaYFsNZTkxs6FYquaGia3TFmU5oOINNr9fBbroj06rhvlqpK4MHdYxcuUM01W5zJGBwa0Ih2Fjng2J1UvMZ2VGesLVr4XAIulLo2j90WtJLHDELo9mGPE8nLgFymVU8Jgie7A0u1TDtHaM802G+6j5KHbuy97u6ObDPDJ0TpYP8rVtFRD9eS/w+B63Tk/VbVY03dWURkZ7kg3V/bRUFQ9gZDUYo43dVMJ6mad0by21/dfiNczFmMQstQsyAvmu8RERQHR3NCKX4ZcEg+RW8ohu5qduMNH9T2VbTkZNxj36qhgc9rYYiI7W15Kgl5ODmqW3pVA69+DCmxAYyfMeiDm8/FTn2KlG97vBCLvl6KSp2ZW997t+s3oOq2X3Kmp2OqYyXTBoxYgrVNbJMNcHIFUVVSUz5BU0rCX2yFhZT7PqHxvrtovbIWjRrQpIf6+yX7xp/wC0Vsyn5R0eL7rYG0PhfTNEvuNCj3ANdQzHFE8aMC2dQwyYmbJtif6nc1I+g2XFvJOJ0j26lNmqjnbC3K1l3UyboYMVx7K8kAqHdZFTR0W4xMvfdhUNRJ5Xvt9yu9MFo6vjy9Q1VHVPF4pAwPPS/NVU0NgysFyweW/VVDf9mc2+6o6jnGQ77rD6sk+lY78A+W/woufmSeL3XF/TcW+BztbJmGB1+d0HygMw5WCraKk/zTJQ8tvbG1SOMEsMcjcD8PNqbUbWc/d7PqXNZbni5J9FQbMZC0/1iOIrYZpqmaBnHFJgda9lUUVUd5tSkZjpZXeaRvpUbKlpZDL+DOP+LslIymOOCniZGw9clS0zjc0V8Lut+SFLQzT21LWvs0J9LU23sZGM/NbOqaJlrvG8J5hbIfFGA1+K9hkDkt7TuwzMFr/NbRbUzPncHZYje1wp2Wtge5v7qhoYXtjksx1yEKcneVNKwHF6iBmnjzFsNvsjQ1En6LcTJHH4VVyQvDo5XZFMpHNj3TWhumfYGa3V8eqwA3ub+BwHROGfMLLRSTUu8a6nGKR7TawVGzvBqI5pAwxuzxtK/wATbMo24nNqBJEz3WPa1bTUDPTjuV/DWPMjqerL4nepqbJTb0SxfiNcB5U2Jj2uqKnMvOV0zYznHE0Y3n2TNklsDZi3O7LgfMqL+HyxPpKlhuGaB1tFK+1hPED9kJRm5tO1y3rbHdwsff5L8NrpBa5ICqov9yEO/dVMOG28lx/cqmgop4+9DC23QWVV3jHWGUjBc6Kenhk3VNUPc8s6X5LLTSyADbLM6laKKXkHi6DhmDmtPA1E/C/NFb5zd5C9pZKz1NUs2y6R7q14OEuHkVQ+Cd0clR5zzKdIXO3LfNM93mW9oanfSMFyx+X2VXG9jXPjms7EMwCFRTMFqComIjPpLh5VS7SlB3c8RjNkJ392me/IOxWK2ZtGkAjojOGvF9CclR1VPG6VubHYcz7KZlWLPFObD6JsFQWmMExvDjqpaRr4WXaWhrG3K38LcRLCyxRqJ7CVwtki4uJLhmT4XTyNErnPsPZSOjYGuYLpzeqaw/qU/A7xOLTd8Iv8wj7Im113/a+7c+Tj49GI7VoJIYpYm8GHSX2UFTTsG9EYOfVxVTR1su+4N4xx11zW3aIcIlLZ2/VVWzahu7rNnT47fLRwWypHv3cbp9y53S4VL/CJABb8Rz3WseoVFsx8m8rOEl3y5qKGujm7xC3CXNbcPXdqaOSKn+MvyLluWzSMhd8LTkvL9ezRaDwZKt2dI471r94z3BU/DcuauLLJSSvyEw08JJzt0UnDgDeXqCdHYgOGIe6A6uC2aOLdukGIDnwoxxNnfAy/APK0qOmF96IsP/s0qaolhkijghLLkanon4XCz4t2fopK6CxxBriPULZhRU0IfvcbZCeQQhZJE8NbYF7c0Z6mR007ssXILPT+yDIwXvdonzEAhguQFzHZkr2PZvoojb35r9BZwSXuoK2z4o4b4r/GjG69nCyxG7/n4iHDIq49IUcx/UHD2QRh4EsYGfpeAq6WrfHJI+73G2QsFPvm46KokL7D4CU5tBDjld0FgF3lshbVE/qe/NY5JTK85YndPB9FXHCDJAG2VnaHhw9VVNjHC2QgBWs7ot7Wi/MNT4zBG2PDiBGqfYeW9lRyjhu3NN+fZl+Rr807u0Q3YyzCbvgBh5AK6d3N1mSeZpFwjFLNhgdk5gFsXZotFa2S9kdcloeykkB/ArnCCYf2KfY8ViQpBhcZibkWW9qWB0rtAeSLTa98vkpt358Ky4nHkqWF/C7MkFX9/wArRPv1XlJC0Voo3PPsFjdA7L2WYK0XlK3hik3fqtkiOSjLosbntBuiWOLPmuCRhChfUOZuo3B5A52RyzQcGNxczzWSB65LMYrq7YIw5cQzGn5ZXtzUkOHVuqs34ck90di5zrH2WdzdTtDeEG6sGE2W9n+jVMD5S3JYfot3KPxaU4Poj90PHmVrdMwjK6z1/LcfZHqnREXZUf3Vk8i13FBz+Jx/ZEKRvNHh5qUu80+a+njcxothF7o4s8r5dmehXP8AM1QfqQj7oW1WSFle3Zcq1/Gc+FwsuqH85f8An8/+rv/EACoQAQACAgICAgICAQUBAQAAAAEAESExQVFhcRCBkaEgwbEwQFDR8OHx/9oACAEBAAE/IfjT/gNP+AGv9Yf5LUJcNf7ZxNTGZHxrFmdqEcxEdS7mCbm3+hdSkpGhCfr/AB+/UGyVIauZc1moUY+5ZUWGLQg/4jMBAq8JfqIqCQZd/wA3mW+JbLuOJc+ktPrMeo9WbxMyZa8zmeBP2OZ9PhDvr4ulU3xdcwTd/wARUwqfDlD+KZi4lENY/BkfCsJuNWameCVVBg7EeSoaL6lJ676uNUlqKeo1cA4l4mEby6+C7SU9RaKCp5uIsMveiIg7b4nDzBgMw382+Jc7jcKqsii6heF5l5j0hMCazWZhtCklxysdfcJaowAqBcAArqbiM3A8y98Qykciuo9sBzCMXvS6jchg6jyHEHEvgdKlGt3eIDWmZ6iUQHAGUI5Q2e4bZIO7v3KTqFxsjIsPKU+OYwzThdRbOjF9R+ZOA1UrPNDCjU8P8yvqIaTTmGQ2r7lUbKZjMBPDtSvPmsxibsV9QP323/TmBw5Upq/ZXFFIr7HsVPIiKJ0bPjgqtodoC5XPObizVKmNQHumbfOsdvbq4OEaby/l1Df5J8RHfWDwwerDTdmUHIhGnZuUhhqC77C1DqtVJr/tEIhJmLKUsgO6YHg6WUG5dkHUUDXBC64gYtCBdkrpmTwvz2hdwr3HE2H1cp8NBzhvmK5goDUwpkHWJbARj5VRRLTYDiH+xOpTag0rMEoV8IKHFEu+CVBboDLByCCemoRet8bKir+Hd7HiXmsvEYdTpfE4b2tiEToe+otS35qXdk0y805eZyGRuJrXexZYLxLeIkbxADFL7gcaai9Vz/skAseRveIlUt+Zg3fy2ZZ0RaIr69eosmG/mV3gIPGIbklnWX6gNMj+rhDwVRCxa+f53LMPJdlmPWDKV2Al8Wq74lXh+Y4VHBfMrwSg3KvMyb+G88kslzGVhwdquoNAH1GcwcDpj4lDU0+AL0RpeJaLPuD1UlsblKJjNbJ96J5CUuWvI/EThL2iNtKVd3UOhw4pjhB4A1d69wV1vUrsOp1xrAiEYiraStw7HFrMHwfWVzWWzNXURileSZgORrLMyuQ9k6/T+poRYlfFvMizMGOd+ksEbZW7hu2b3FNYaTNuv7ij9FxpWwUsvE4tJtYekItXjPcszoI5G57s5s6PupatX6WebqMdHnHTR9xFcF2FH1Kk61Wstx+LvmjzKUZ/ZDGcAuL3JelweZdXJrmrqVJnOhZc5msisVHHiizsnI6j9Q0Gj6hrCVPv52CKtajpH6rMCSbQfqPzo5tKzL45KGzliLvKQZRioVcuHVTDckzXpmfGpuDN/K5aEG2cJFGo1yiin7BBqHiSkOPMuLJvVYqALcUZB3KbBLB7lqYreNHhgK3A2WniC5StvGB/MF3bE1q7TKqqt7GCe5XxPMh2iHLYenmFVZLMAwLNXFLiK9xQye8tPa34mZzmnwYMdA5yRo7FKPH7mN1VhV/6xzoxKJWMRvnKxwtNTdhNoHKCuuZp8P7gpHCuVaT6WEG4X+WZv/rrPwzIX4srj6zHjLp8mZbGZsFnMuOBjUC7oglKNgK4Y/cpiQcweYwluogPdQKKI/vmJ2G6BcXviXRof5T19uwpB5nFtvwq5BoDYvQGH18ku6XJ0XiPY5RvKVlZ+k4+PP4sWta0QwY3xVK3JTqTuUE1usyYi0wKscw1B4LBbUqHi+COjw7g/DQzWgzmYsW7lzqDdLalBYfTTH+Ei6FJcvDoUH9Eayoowd1F+QZc4Z/DO4YZ0JBcNaYwpcU34f7IWqCrgTBKkPadpYlVVQZZV0tO/wDICXNn6eaxVIU2rpfiW2ujowQ76/dPNy9byzF1hi+4dFMFGNN8b7lznFLJ02PcYHMbbwkMcqGYqY1ev75yVb9MJkPPIc1HLow0enEpBvph7zAkygrffqMwNcqwtxBMRCaa20yithwR0cxQB3/x5Ud9bk0oj/vgJUDILHXC4fVnAO46nXHaU/uIJVQPJZcQqWRbdpiI0wYe8rUMPhLQqrthKgLddxS2dHjxGw2gPYCXRH1QheMx6w+WswrzmIFqT7ZUD1N/wXNu/shgmDdX53CAfhNubz7hw2FTLoJ18Uygz7RlUTt0CUkVflrX3qJCvYtNZlCxSrXxCQ4u0hvuw9FmUzb4jkHAlMckALjV7KhVjMZCCE75Y4RhOWCOWUkbfgExk4ORAncIBYRpv8CYK6B5g3XH58QSptDRDYf+Ix0njr/sieFJmFQylxZfQqw9y4Tm0MmadxeoMCx3H8xoz5Z0P3A7BD9o7AEUq7j7gzJxFe67v1FvnIWOQI6RN3MTgGFrAktH+psYrxHToVyCyi3B6uWVG03vzMzC3XiZFyLd8/C6xR9xea/My6Fa3FsJ4H/+CLRwTEzoVS68x8gWi/v4fvMdw8iEbGuSf3l4YotckrbI72YxCBaX3AE1tNHFfiJXRqr/APXEPyzvgfqMQMUhXP6Qreet5LPZYoYIDOhIVhyX3Mk51D9wXoFmA6PEwAm2cQ69sDJHCMG1UqnCvPMdGHMtbNTjV3MQ71PDx0xHPLejCy1VXlRBTRFVFuJyswJngutiV1EoqoaIVUU9BHrgKuJFvA32TPpjPiJBFVwHcYLGeBZgQgSwsFTBcGdKOPETwPCHkMvUAvki9esqMkqlcJayA2Ad9g3mcgZHAsDvzQAhAgpoHLBTG7VV7jYg7QCQpCCxhY98xaFqsNDVeoDwfFjgzMpUPeCQrpQNHmUgVdnPMFATiBBAzKTsi8fNdyqJsHe06VSigj1DllIrBKXepKEq9icwPsmRyfcvjUFsy/RuV9IsDN5qXtxRu3uFzZehLW85B9Zc9qNcZzMxLsAhot3kULwrHiZAxCzEtJdBuZ5U5zScgWYNq1UaVSbrEyb96lKO81RK6bBlU4ImqSKhnTFeTxKVXbKhTVhvioahtXHqP2n9CO+0nMW4UbNlh+ZY2pbcVA3hSA0F1m+Y3rkX8VBQpYxe5OUmcyuviWgZRgXnEozxzwQ5+fL4CWutTO8yAwASppAtX3COdIIB6h6qDnYjE5/BglsDI2+5bil0OHiYwNt2i5RjnnjO/wAWN2S2MYY9dxsl3CpnS4x+fhLYFoviU6wAx1DZD4Ar4S5aHBxDT1FGTeWkwm2peoWuu0WhnOYJuTILHjQy7Atnggpber1Kx5JSdcYcHUuw00ZUK0Yiyd084iLzdRoXLZPM5EhhcU4My+wPVkyTWXHExBtXmai/hGoLYfCoIIsxC0JUQoW/Eq7xPEoEL2uUkArCxS1lTZcRVjZeIIXiWyg9E7gxnGFNxtqbZ4gMXCriJW4Y0o66n6XCcILoaIV8w8JlAuFIb+eY2eZh3M09k+0+0+0+0xh8SZvO5ZmZbMiUHM1G44/BXme3x/aeyYZUG/4V8G45giT6YepXx6xLxARUdxiGmL2HwPzKriekrxK8Q1qaRW8TLcqv5VcpEqBKRMfNSpn4qMZOSVmn8QwwFR3AlV/t9P4mX+70/ib/AND/2gAMAwEAAgADAAAAEAKAAAAAAAAABAAAAAIAAAAABKCANKACAAAOvGPNcMCFam/mAAHFQhbQFlpVhblErAFZz4weiqkA+JYkWXOZ9lzooBNGSTElzOOWIYiygZovLf6OvNEELJUd9dn17o5aBbFLtIoMutEtkoN2LfL6ebx7inJ6gcTRG4Hecclen7R33dz1vqHek7gqe+fwXkFFMOCu6KjPROeEgpILKIEbRqVXtNYOFCDHIEJHHMDjuMVcILCGAAAAAAAAAAAAAIAAAP/EACQRAQACAgIBBAMBAQAAAAAAAAEAESExEEFRIGFxkTBAgbHB/9oACAEDAQE/EP2E5E/HZGC3DrgHXqQZYFaI7ixFOmZMgBIWAdRyLMa5RtzkmYQOaVwXGIu3MM5NRSlhbdxmHEI1lqGGM3ZleYMVw4iOITlcG+EuAZ9lahOrH+3iGTUPLChWohaloi2owHTEawaHOD7jHRTfYvDiTpLqIobZ+Jf6y/bLcl3A7+YCkEiuwwOaoNFzLQ8miiFI8Ls4zPDUBs5q6jlAJT38fUY0FT/JebHQ93iNlJYvq4vVEMiQ3DEVQj7MOHtX8i5QNTNcwPwvtNzba3D3dkAHe5QOhVKxWtQTX7iKOpU/pAuwRvcGmilHsRmjkMwvDOo9EWpHxqeIvdy0Z13AOwgJTriyfCMWw1K7q4HcYRNavmNrCkRHO8x4vbEoBw/3xxPgwxxdbllrcVFY8plgKBibuGe7DA5GGe7qPiYMMYeagWNJQ/8AJtKYQ18zdhOWUYtq5axyOo6pPjqNBgnbOIfDZVeidukUSmZ6Vx9xUjGYMMeiWLZmoeIlcq3LTBcBe0HVPpPBjgrUrFipaIjk7ZYpxj8KSqpDNEBWQ780HBuJ+c3Fr9f/xAAnEQEAAwACAQMEAgMBAAAAAAABABEhMUFREGFxIIGRobHRMEBQwf/aAAgBAgEBPxD/AIVP1Oc+oniInJLPpLYEUY+oLhtSBU6ROiq4iiiyPQShGpjhZ6lzg+quNTMijJQoMtqJReKEFQYNPcKSeBkFBoRVhCersJzJhKkXvU7ieIRuGP3UqhSUTyWo5Q9okrYlIkuIJ2xKo8+vxSO57v3g6vlE2GdPiEGLL/exngrfyqT85LtCWDaMs+ajN4Vd3RnXX4iAtii1UDa3qIAs2iHmKhAaXuUQ3r6qGpYHqA5e4Yu9mA1e9wrT7HmcFv3g4vUrytF+HPiI2FqvvQ/US1kVegafnplQGvVq1CsDOfePUeA/tmMkgfEdbbY+4rGG1+qeH+5UvSz+Zwspajvznp3JgUV5V8feHZ0B4tW+KJyzmto2UtsHuotvmzrzGdFpi68vz1DOCRVWR/TaK8W8QvNElLVk56IOb1yyrwMvXvMchp/lAmktdfMyhISegxNDkCWitfjZXLZ7Gf8AhH1N4A5BrZy8wOqSr+0uRiT1XC4txNP6GL/lPaPSNbv2lIQNE5hGXeHJWvdG0XQittxDhAcqoQcAkEuCoTy8w62fXjlX2hpQqsfzKMwSeF+IAQEoDqMgBCDSxLfxO/BVw6C+cpUhUerS3kKIyC22eaPTMpRd2KGk+131AOKDI7kny8xOD7EqLU+8wEM4iIUFZCRWG4eCQX0YFfQzcDUxMm3E42AIqGCVCxcR9PWiUfUITldRiCwZaI+IlHuv84L/ANp//8QAKhABAAICAgICAQMEAwEAAAAAAQARITFBUWFxgZGhELHBIDDR4UDw8VD/2gAIAQEAAT8Q/TT/APA0/wDFL6PzBs/tYg/u3RmUepZ2fpZ2f0AURV8V7jsvj9Vr+pB4r+6BcajgsS3i4mo/RV8CViuJR0+GWwUqEJanTzB50F8PEs4X4jnf8zZ/YQqUGeQnkIdhSxbyVK/+pTs+5Ts+5Ts+5Ts+5dLCj2xLEL8MBJE6CUvwKDuy+JjygVM6qIOcC45xN4ntqNKTytCliGaXEXbR6gNzeIYcm4ZaZhYlV/WpgX8zxfaJcH3LiqIqLWpfo+4oL/dBGqz1MVGlbzqKzw+ZXaVvLDDQ6dpwLxOAKFe4pdKV1pLP+0fSrsyMoFaX3LDeKzLHU6cSzKDba/pD9Kgqfu6+Yii5nWLfcRNF6iW8JN/6cj3CKPGpYHnZLXGXqYjiAeW/eohC2O7a0SvglzWy5gAhVWSrYMaVe3GGUWjAYNL/ABE4SsHBWH5mZFAOhTEJYanEqXqVTVYglCm9wzVdTJDF9RzCzToCpAC0WUouOGo2OJ+4cQgQ2noiCii1t9sFN1iVeFSla8/rc1CBygzIHwBiECbB29wOJaakBtjRsK3G6yGKYxuCELfEUWmSNzCXxcEy4m9eMTSKdEaOeWsy2bshBpdEWFY1BetRRZKJL9fcJbbL6WpIVCeUHB7lGDYsGR4glWrfW4OiQ1m6th52w0qhFlLK7S+H8WxjxCwWQUebMy20w700nXxHZOwoNlkWICmuPEW6t47IhhVqNQPpPIfpyemZmt7YqQIQG0hk9j50OhLJkLhj3CliKZR2ZmeZrawXi0bvJaIwgjjHcUulOrGGD6WVx5x8yixk3NFOTFQuWqbrTmDr1FLAjGskeQpjlysQp4WfhCP6gLqN1ehiMq3JYynWZbeobZX3XzBpg5SNRFl4tp4XNZgKuEwBFrCXfiEtPVgc0EpyXbsDX7wiXl8hVmPtdIWfENVN18RtxYNQdfobfczs/cfEKlNKExIhrKokMjaBadt+oYAeNHBD8sYlmtjREFoELheQR87jOFVy6+aZBPgNc/mOy1t5LDCqTKC6TNujUcAVJqk17xFb0WAAvVUMBanEU4Ax7+JeVlDqwfbAAc4DkRxepVRMIvzha5g3iN53EJpfuCNNdxGQu+5bD1nmblafgLEBy8SzsNIUusO47NXsoswVlRVdQpyP1e+Bqci5YGGC0MOYROaiWwaqJjUBxC/HHuDvHHlYP8zL0zT8kRmy/VIXlYxkQGA21m5csYBKTZmsx7jB4i3ZjAuW7KLYUHUG0go46aiCwJglxVfc3wBS2W7YfCwCwF83Kwo2jZFM041z84NvNojWZfPEBWECLX7igM68z6OqZUUTPmIQRfMEsIsco2J5xMow60lMsWm45X63hI/bNQ1bgXolWhbtuXC5ell3gi2PERsCGyx/JFwl8QBiAa2AuLeNwe6HXvkQIFERlRhXczcFIbVlqDm4mNXReKhdNSrNARgU1XmP+jW0Glej4Ya+UKqAU9ixioW5seY60XutERBZktS9RrvzmDBrw6ao9IaGytw9Bz3K90X3AAFh6dToOJ2J6hoUJVETxEe4PJeow0M4uE234GgpPoRwAhYOE7mXfH3L8Z1NfqKvSfuR7WUGK/sGXHMpMAg0NM+pfBwMXRk91E5AAHKwO/2llorxN8NyGdQlyzuK7UuDPMVkovsNsOPqMrBhKzXR+JeiwCvIQmXYRQXCORYk0ZptGOoOZUCaaZaVim4yLVRYiguDNwYBylcN5PiBWh3qDVhrECNoKgIu2ILZ31ATRl4hWrBVhYcF/wC1MVWs1yZc/EeQBHlj/BCPqg+kzDRNC+JWXDg+5zNTBYWi+riW9uRhTncZ3AoaLxUct9yYTCqcALLijq0LLLtAtDayFiGNgKl/MVlQSdRoBycxxqU1INdi8aSWbVuoCK3hbqYcqZlCp0C/KEeSdSMpSqLKzGZW23v8GsgWUyJMLQurH5jI040pRW853E4DHmY84+dwCBA8hoim6BtuGOboyY3iMsDYCg6dkGx0SAqjxcIapMBAty4YFiV5IK+YGcptrJY1GHoWNl4MBClvaoIHU3zLO8V37IyzpjaWoC1jDaian4ua3mTLUy7xfyQWBorjqIrBTW93qIqcrLGKAze4W6slmq1rF/UDO2vwiJlyOJSfTpETUOUR8SxEEGioLtDhxcpoohSS1XWfxOiMggQc6jJjZXxNL7QhrIbXlQeE/Mwn3zoqUmjEBb2fbcB2Xi/ERCBrdQ0MUBzPECkQ1qsDddwwfmjAkFws1GBIdh+XzKZIFIdnK0DVdw1vLtQaHCT9kvz7RUeyUUrNuADjuGRBncY4lUKLN2B6JFyjbvwWpS1AKLo5no/UdkHO4KSYrqO3P3LYxib2FV+8pVaygN3cbfiMWp2rOs0apuVpHWtBSsplfModBi0uEYulPmVrRbukM04Ymzh7WDkMHyhnUlZjvetXipcBJZ01t7SI6zcdp9yvxFHAPnNaxvSXW8xaspeeSWdB8wLW1zcrUuwJoAszL02FABFlf6QgxZsYXdLr8Qpai3KCqDOGFM4A6Z/EJnW0KBUfNW/EfiJ2xbnhfpCMOYO/4A245ZiexTpQQ6TQ7D98MLKqX5yZOUTbG1w3mPNPZZT9yBmtwdoqk+f1NvYmLREIbyBguZ616H0dSrfTKs3h3ncXpFzRcmSzJ4hjfVX8itfO4ABqarlKHDqovZ2MTdhyr8w3ap6seTQMJ+wwDF08rIrzF5S1uY72BekuWuPYmODrGJUwIaNC04pqNFWSirNsDz8y7IojkSFXNmXqBbAN6F6t5x8wQYiiliDa89QR2YuqBQ8WfiKfDCAmzRZLUnScAL8xPWWwWTGbzKCvU0Ki5wIw8ppZo0mdVUczZUZhEttqPURlgnJU32rcV4ltskKfCC7N2Y4iIisZvw/mN4IS6aiZrdi1W5fxNvYhU5OJfC4g261Gbk0VkcxZSq2bvFfxLnQBk6abGpXuhSyRq1rF74YmzFAypTgCq+IUO4pZ45Bl9OpCmt3sSHiNXBC2TfeARhJYiL8eox96Kou2P8zGeIYcBSg0bhExBLWYSjKmuLlYkNaZjyFQBgPTLTd94h1iEZXz9/UXUSl6DNmvMpJly8kfqB1gWApA8MdPQaMrOjUbxRjYhzYCMAnz/YyXqooCaBQB4vkgiQKevMzgLZLp/hEhULhpzEJK68ZhXxDyPJyAIkYAid8yjv8AEdL7Jv8AcQSULVBLB4ObLiklbV9sOkkMpRFXKVRFxYKKXC1VLeDiKibjmSqDmGDza2WnLFNagZjFczEILcMDBJVwGN0vGSANSDZQ5x+YljkzcA49GbgTz0oGqiIlxAQNndCXlGxfcs9bKyTR1hzMDzrIgbpviHGRUaHa6qZhwxIwUHmuZTnStYhE4qiJkkOgeA8yw5q1tOAvEZ/9m1C25fdqvcSKnwAMpHZkLSWP3ASOQCgh1BSI6ZGMJSMnDBPqUVPZmz2Qk28x1ErmmoXmEdBlPowaa0PMZBaJ5/6zUl3btCNNxzEtAqiAIMpHHmbbEHIq9pCa9cULyXo09T70PJ/jEFkC7jcrOh8xTN7ZJUq87cXBbBW7aguRP2il3oguy5Cz6lmsLevyGSvmBL9Iy6UB1Gx8SoiP8jKn1CT6ZcaG8+YEmG5TkwvIUezxMULGfwI1GKK7l2i6cmY9iOcZS6cdrWFXr7gNiq1YWHkR8ReRMvZof8w6hAOReBR8Q22WcsbGd1Ba2dz2hj4XmK0CtsdgI2AMVU6ish18yikD8Ca9Th8IgAq/ivmKmLhwlfZcfjpF10hwUHXEfobcYrqveBXmU6+FenkRTUMPcYlOnyLFYVLbGUrYBZDbGwWKkZosGtyq6qNs12xmfUAIyZr+UpnngN3o34x+YT1QlodKS5jItIthI4pWnBr6hIMBR7hWl3xArCYwahYDdkcs24maLeI6q7VfxiLAmMwnqLLzEGE+q3DvVlqHuLy0xA12RqUVRcHjxMrMCWlVxuPoaNBBtK1FKGmLoLZLwVNq7sZb3X5ZmCJFL2+NywWdcYgDjUOvfXCCHt/LL9TAUlHojio9eFiCPEXTD/dkt7h8sfuqLCDRBGgN8hvzLGt6jW1pmTlFb4YYgPW3I9VEtUBwjyp6uAD0PoqwfKx4ONkrNFeLj7yDqih03KJ46AWW+yPlDR4Vr9pVbrIrGvtFodSG95Zc74Xd7ldYOQFF8zNWW+JiZcGuox2xBYzslen6iG2Mbl3W/cCOyMrDqA+KZJDW0Zz2sLXlSJB2L3V7ie+A5hwWlykwSlfm3VcpGNooBjo1LUDY4Vfi+oYoExs09y3plDmA4JpazKdYSkL9dxTsDaD9QezTRSmMX15EfgNL8xrMoBAAD6g5QzFZbfUP87LChdnL1Epsl2UixAoQQiwOAPUA3MDNtkd+gHNASr1MYA0hgXqErKV8wsDD5hpsR7mmiEsN3WuYBjdU8z2Zt7EuopfUNlnbxqJ4dShLgNmBCp8xVgaCg0O49PItgDq5XMDLFc10R1SsXI6iMPgMYYYGJVcIQ/aAndwyxQP5H1EzvctE3TiYpVooWYqLMksxfWXolgVhPgQhZNXYHH4gZdqGEQolhbLioqs5g2ch/MvPexWH/UpPd4Ccy8unBrX/AF+JQ9vUDNdwwBQGHZKVC6Yhz3FLLBDBoxmANJZ+n7xMC7zzCAIuUuiNRbwRZnZGRC5moxWeR/EzXWLbUsJRm1fnJhI1ZSzC2sfbEdiYZHxM9oiNDOoazGBjwPcZc/jK2f5j6LscgFvWIizga6tv+IwnA5O4qLaT4mMB/Mxt0vuIAlR1DYhXUVJlNTPibM62hDVvRmWBnhzymMtjzmKCr4MQKzxFrz6iWqcaqpXtsev0G9jnzKigKMRWdWzRuIZbQqjH3DfMdJjXUoo3QqznXX8xAtg8g/DDqXUc6nOtRUJLJe4Q5wI6F5lPGASn2SkpQ2YPUMXcNMZHHojwNJmUFC85gjlFnzX/AFlIHCzK2i78yww10wKgGl7lQRCC9QQ2y9wUJTGyaCo4vqRy8oqvIuLEMyYq8QAlRV+BUIDRndxFRkhTRZeWNVMmsVK+fuKDX5guNyw8xg0W9xEv6sdwOIS4G4fZqoeiJoMU9QNrA1pl5UuVZTCgXfET5BdoIAlylZL4mFN6dEatGBStS5WRcTIy8RdDjcsBtSmcREU4lOwHF1qIR2CZAbuWquRbHEvYXeccSonhjOhoYHuM5oD+IWD1+IPJuImlbgUNUHn9ayruEi/WV0m7Z1ySu7/CIvjKf+JW+/cStbMuOPqAQxBrVI1iXCXT4juzameQF4l8yldsp7Zim0rJGqIqULj4lR48Sg6SvbPmVVzWIYeEoDWWKh33Dr35mr+irxLcJUqf9QKyLBbsT5lNUL3+gLQVUCR13A0ZS2tt9wgKwFVAnBLmqlQtaYgWBzRMCGy84zLNhpiKU2OqhmpZgSShp9Qq3aNRk1BWeITgJdtNagdD+pOxPATgEsWz8zwEEVGYDWmU9MS8EqcSkuJT0wKZILqVsh4YkGNcyvn7lfP3K+fuV8/cBEFs8xAxfuBMC/mWmTMBof8AH/e/pqeN5gUf8r97+nf6/sf/2Q==', 1, CAST(0x0000A829009571CB AS DateTime), 2, 40, NULL)
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (4, N'Lawyer', N'asdf adsf sdf sdf', N'AA1', N'data:image/png;base64,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', 1, CAST(0x0000A82900A2C9DB AS DateTime), 1, 40, 3)
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (5, N'Doctor', N'This is the doctor category', N'AA2', N'data:image/png;base64,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', 1, CAST(0x0000A82C004C4E7B AS DateTime), 2, 40, 3)
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (6, N'Master Category', N'Master category sample test', N'A3', N'data:image/gif;base64,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', 1, CAST(0x0000A82C007D4020 AS DateTime), 3, 40, NULL)
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (7, N'Sub Category of Master', N'NAa', N'SC', N'data:image/gif;base64,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', 1, CAST(0x0000A82C007D74C3 AS DateTime), 1, 39, 6)
SET IDENTITY_INSERT [dbo].[tblBusinessCategory] OFF
SET IDENTITY_INSERT [dbo].[tblBusinessCustomer] ON 

INSERT [dbo].[tblBusinessCustomer] ([Id], [FirstName], [LastName], [ProfilePicture], [StdCode], [PhoneNumber], [Email], [Add1], [Add2], [City], [State], [Zip], [Password], [Created], [IsActive], [TimezoneId], [ServiceLocationId]) VALUES (3, N'Krishna', N'Gohel', 0x4040, 1, N'(079)-798654123)', N'krishna@gmail.com', N'sdf sda fsd fds f', N'asfd sda fs df dsf', N'Ahmedabad', N'Gujarat', N'382350', N'Test@123#', CAST(0x0000A81000FAF81A AS DateTime), 1, 2, 1)
INSERT [dbo].[tblBusinessCustomer] ([Id], [FirstName], [LastName], [ProfilePicture], [StdCode], [PhoneNumber], [Email], [Add1], [Add2], [City], [State], [Zip], [Password], [Created], [IsActive], [TimezoneId], [ServiceLocationId]) VALUES (4, N'Krishna', N'Gohel', 0x4040, 1, N'(079)-798654123)', N'krishna@gmail.com', N'sdf sda fsd fds f', N'asfd sda fs df dsf', N'Ahmedabad', N'Gujarat', N'382350', N'Test@123#', CAST(0x0000A81000FAF81A AS DateTime), 1, 2, 1)
SET IDENTITY_INSERT [dbo].[tblBusinessCustomer] OFF
SET IDENTITY_INSERT [dbo].[tblBusinessEmployee] ON 

INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (1, N'Sagar', N'Gohel', N'sagar.gohel@gmail.com', 79, N'12345678', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A80F00D4A200 AS DateTime), 1, 1)
INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (8, N'Ezz', N'Monty', N'test@gmail.com', NULL, N'9909980330', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A83800E11CC1 AS DateTime), 1, 10)
INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (9, N'Ezz', N'Monty', N'test@gmail.com', NULL, N'9909980330', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A83800E3223C AS DateTime), 1, 11)
INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (10, N'Ezz', N'Monty', N'test@gmail.com', NULL, N'9909980330', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A83800E32241 AS DateTime), 1, 12)
INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (11, N'Dev', N'Gohel', N'emptest@gmail.com', NULL, N'9909980330', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A83800E98D57 AS DateTime), 1, 13)
INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (12, N'Dev', N'Gohel', N'emptest1@gmail.com', NULL, N'9909980330', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A83800F2B09B AS DateTime), 1, 14)
INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (13, N'Dev', N'Gohel', N'test2@gmail.com', NULL, N'9909980330', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A838011C8E3C AS DateTime), 1, 15)
INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (10007, N'Devendra', N'Gohel', N'devendra.gohel@gmail.com', NULL, N'9909980330', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A84800F0E6F6 AS DateTime), 1, 10007)
SET IDENTITY_INSERT [dbo].[tblBusinessEmployee] OFF
SET IDENTITY_INSERT [dbo].[tblBusinessHolidays] ON 

INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (1, CAST(0x6C3D0B00 AS Date), 1, 1)
INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (2, CAST(0x6C3D0B00 AS Date), 0, 1)
INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (8, CAST(0xB33D0B00 AS Date), 0, 10007)
INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (9, CAST(0x963D0B00 AS Date), 2, 10007)
INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (11, CAST(0xBF3D0B00 AS Date), 1, 10007)
SET IDENTITY_INSERT [dbo].[tblBusinessHolidays] OFF
SET IDENTITY_INSERT [dbo].[tblBusinessHours] ON 

INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (4, 0, 1, 0, CAST(0x0000A84B0083D600 AS DateTime), CAST(0x0000A84B01499700 AS DateTime), 0, CAST(0x0000A84B00000000 AS DateTime), CAST(0x0000A84B00000000 AS DateTime), 0, CAST(0x0000A84B00000000 AS DateTime), CAST(0x0000A84B00000000 AS DateTime), 10007)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (5, 1, 0, 0, CAST(0x0000A84B0083D600 AS DateTime), CAST(0x0000A84B0151D460 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10007)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (6, 2, 0, 0, CAST(0x0000A8480083D600 AS DateTime), CAST(0x0000A8480128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10007)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (7, 3, 0, 0, CAST(0x0000A84B0083D600 AS DateTime), CAST(0x0000A84B00C5C100 AS DateTime), 1, CAST(0x0000A84B00D63BC0 AS DateTime), CAST(0x0000A84B01499700 AS DateTime), 0, CAST(0x0000A84B00000000 AS DateTime), CAST(0x0000A84B00000000 AS DateTime), 10007)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (8, 4, 0, 1, CAST(0x0000A84B00000000 AS DateTime), CAST(0x0000A84B00000000 AS DateTime), 0, CAST(0x0000A84B00000000 AS DateTime), CAST(0x0000A84B00000000 AS DateTime), 0, CAST(0x0000A84B00000000 AS DateTime), CAST(0x0000A84B00000000 AS DateTime), 10007)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (9, 5, 0, 0, CAST(0x0000A84B00000000 AS DateTime), CAST(0x0000A84B006B1DE0 AS DateTime), 0, CAST(0x0000A84B00000000 AS DateTime), CAST(0x0000A84B00000000 AS DateTime), 0, CAST(0x0000A84B00000000 AS DateTime), CAST(0x0000A84B00000000 AS DateTime), 10007)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (10, 6, 0, 1, CAST(0x0000A86600000000 AS DateTime), CAST(0x0000A86600000000 AS DateTime), 0, CAST(0x0000A86600000000 AS DateTime), CAST(0x0000A86600000000 AS DateTime), 0, CAST(0x0000A86600000000 AS DateTime), CAST(0x0000A86600000000 AS DateTime), 10007)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (11, 0, 1, 0, CAST(0x0000A8550083D600 AS DateTime), CAST(0x0000A8550128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10008)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (12, 1, 0, 0, CAST(0x0000A8550083D600 AS DateTime), CAST(0x0000A8550128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10008)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (13, 2, 0, 0, CAST(0x0000A8550083D600 AS DateTime), CAST(0x0000A8550128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10008)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (14, 3, 0, 0, CAST(0x0000A8550083D600 AS DateTime), CAST(0x0000A8550128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10008)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (15, 4, 0, 0, CAST(0x0000A8550083D600 AS DateTime), CAST(0x0000A8550128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10008)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (16, 5, 0, 0, CAST(0x0000A8550083D600 AS DateTime), CAST(0x0000A8550128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10008)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (17, 6, 0, 0, CAST(0x0000A8550083D600 AS DateTime), CAST(0x0000A8550128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10008)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (18, 0, 1, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10009)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (19, 1, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10009)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (20, 2, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10009)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (21, 3, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10009)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (22, 4, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10009)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (23, 5, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10009)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (24, 6, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10009)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (25, 0, 1, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10010)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (26, 1, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10010)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (27, 2, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10010)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (28, 3, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10010)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (29, 4, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10010)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (30, 5, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10010)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (31, 6, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10010)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (32, 0, 1, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10011)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (33, 1, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10011)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (34, 2, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10011)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (35, 3, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10011)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (36, 4, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10011)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (37, 5, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10011)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (38, 6, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10011)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (39, 0, 1, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10012)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (40, 1, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10012)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (41, 2, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10012)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (42, 3, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10012)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (43, 4, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10012)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (44, 5, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10012)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (45, 6, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10012)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (46, 0, 1, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10013)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (47, 1, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10013)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (48, 2, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10013)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (49, 3, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10013)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (50, 4, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10013)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (51, 5, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10013)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (52, 6, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10013)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (53, 0, 1, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10014)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (54, 1, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10014)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (55, 2, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10014)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (56, 3, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10014)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (57, 4, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10014)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (58, 5, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10014)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (59, 6, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10014)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (60, 0, 1, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10015)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (61, 1, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10015)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (62, 2, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10015)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (63, 3, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10015)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (64, 4, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10015)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (65, 5, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10015)
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (66, 6, 0, 0, CAST(0x0000A8570083D600 AS DateTime), CAST(0x0000A8570128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 10015)
SET IDENTITY_INSERT [dbo].[tblBusinessHours] OFF
SET IDENTITY_INSERT [dbo].[tblBusinessOffer] ON 

INSERT [dbo].[tblBusinessOffer] ([Id], [Name], [Description], [Code], [ValidFrom], [ValidTo], [IsActive], [Created], [BusinessEmployeeId]) VALUES (1, N'Member 30% offer new test', N'member 30% offer data test', N'455445A', CAST(0x6B3D0B00 AS Date), CAST(0x753D0B00 AS Date), 1, CAST(0x0000A81000E55A9D AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[tblBusinessOffer] OFF
SET IDENTITY_INSERT [dbo].[tblBusinessService] ON 

INSERT [dbo].[tblBusinessService] ([Id], [Name], [Description], [Cost], [IsActive], [Created], [EmployeeId]) VALUES (3, N'Member Video calling 20 minutes', N'this is video calling servervice provided by me', 1.0000, 1, CAST(0x0000A81000F078CE AS DateTime), 1)
INSERT [dbo].[tblBusinessService] ([Id], [Name], [Description], [Cost], [IsActive], [Created], [EmployeeId]) VALUES (4, N'Member Video calling 20 minutes', N'this is video calling servervice provided by me', 1.0000, 1, CAST(0x0000A81000F078CE AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[tblBusinessService] OFF
SET IDENTITY_INSERT [dbo].[tblCountry] ON 

INSERT [dbo].[tblCountry] ([Id], [Name], [ISO], [ISO3], [CurrencyName], [CurrencyCode], [PhoneCode], [AdministratorId]) VALUES (1, N'AFGHANISTAN', N'AF', N'AFG', N'afghan', N'afghani', 93, NULL)
INSERT [dbo].[tblCountry] ([Id], [Name], [ISO], [ISO3], [CurrencyName], [CurrencyCode], [PhoneCode], [AdministratorId]) VALUES (2, N'INDIA', N'IN', N'IND', NULL, N'rupees', 91, NULL)
INSERT [dbo].[tblCountry] ([Id], [Name], [ISO], [ISO3], [CurrencyName], [CurrencyCode], [PhoneCode], [AdministratorId]) VALUES (3, N'Lebanon', N'LB', N'LBN', N'Lebanese pound', N'ل.ل.‎', 961, NULL)
INSERT [dbo].[tblCountry] ([Id], [Name], [ISO], [ISO3], [CurrencyName], [CurrencyCode], [PhoneCode], [AdministratorId]) VALUES (4, N'Kenya', N'KE', N'KEN', NULL, N'dollar', 254, NULL)
INSERT [dbo].[tblCountry] ([Id], [Name], [ISO], [ISO3], [CurrencyName], [CurrencyCode], [PhoneCode], [AdministratorId]) VALUES (8, N'United State of America', N'US', N'USA', N'Dollar', N'$', 1, 52)
SET IDENTITY_INSERT [dbo].[tblCountry] OFF
SET IDENTITY_INSERT [dbo].[tblDocumentCategory] ON 

INSERT [dbo].[tblDocumentCategory] ([Id], [Name], [OrderNo], [PictureLink], [Type], [IsActive], [Created], [IsParent], [ParentId]) VALUES (1, N'Xray data', 1, N'://www.documentdirectoru.com/content/image.jpg', N'A', 1, CAST(0x0000A811003DDF74 AS DateTime), 0, NULL)
INSERT [dbo].[tblDocumentCategory] ([Id], [Name], [OrderNo], [PictureLink], [Type], [IsActive], [Created], [IsParent], [ParentId]) VALUES (2, N'Xray data', 1, N'://www.documentdirectoru.com/content/image.jpg', N'A', 1, CAST(0x0000A811003DDF74 AS DateTime), 0, NULL)
SET IDENTITY_INSERT [dbo].[tblDocumentCategory] OFF
SET IDENTITY_INSERT [dbo].[tblMembership] ON 

INSERT [dbo].[tblMembership] ([Id], [Title], [Description], [Benifits], [Price], [IsUnlimited], [TotalEmployee], [TotalCustomer], [TotalAppointment], [TotalLocation], [TotalOffers], [IsActive], [Created], [AdministratorId]) VALUES (1, N'Silver', N'In silver xyt adas sdf sd fsd f sd df dsf s f sfd sd df sdfsfsd fsd fsd df dsf sd df sddfsd.fsd dfsd dfs df dsdf s f sdf sd df.sdf ds f sdf sd fs df sd fsdf sdf s f', N'html content here', 100.0000, 0, 16, 100, 0, 1, 1, 1, CAST(0x0000A864003C13D6 AS DateTime), 52)
INSERT [dbo].[tblMembership] ([Id], [Title], [Description], [Benifits], [Price], [IsUnlimited], [TotalEmployee], [TotalCustomer], [TotalAppointment], [TotalLocation], [TotalOffers], [IsActive], [Created], [AdministratorId]) VALUES (2, N'Gold', N'In silver xyt adas sdf sd fsd f sd df dsf s f sfd sd df sdfsfsd fsd fsd df dsf sd df sddfsd.fsd dfsd dfs df dsdf s f sdf sd df.sdf ds f sdf sd fs df sd fsdf sdf s f', N'html content here', 200.0000, 0, 32, 200, 800, 10, 10, 1, CAST(0x0000A80F00670258 AS DateTime), NULL)
INSERT [dbo].[tblMembership] ([Id], [Title], [Description], [Benifits], [Price], [IsUnlimited], [TotalEmployee], [TotalCustomer], [TotalAppointment], [TotalLocation], [TotalOffers], [IsActive], [Created], [AdministratorId]) VALUES (3, N'Platinum pkg', N'In silver xyt adas sdf sd fsd f sd df dsf s f sfd sd df sdfsfsd fsd fsd df dsf sd df sddfsd.fsd dfsd dfs df dsdf s f sdf sd df.sdf ds f sdf sd fs df sd fsdf sdf s f', N'html content here', 300.0000, 0, 32, 300, 1200, 20, 20, 1, CAST(0x0000A80F00691B07 AS DateTime), NULL)
INSERT [dbo].[tblMembership] ([Id], [Title], [Description], [Benifits], [Price], [IsUnlimited], [TotalEmployee], [TotalCustomer], [TotalAppointment], [TotalLocation], [TotalOffers], [IsActive], [Created], [AdministratorId]) VALUES (4, N'Silver pro', N'asfd sd fs f s fsd f sdf sd f sdf ', N'there are lot of benfits usign sfdsf', 550.0000, 1, 13, 7, 2, 11, 11, 1, CAST(0x0000A8230106FA40 AS DateTime), NULL)
INSERT [dbo].[tblMembership] ([Id], [Title], [Description], [Benifits], [Price], [IsUnlimited], [TotalEmployee], [TotalCustomer], [TotalAppointment], [TotalLocation], [TotalOffers], [IsActive], [Created], [AdministratorId]) VALUES (5, N'Gold Pro +', N' asdf sd f sdf sd fsd f', N'asadfsd asf sadf sdf sd f', 787.0000, 1, 12, 12, 12, 12, 12, 0, CAST(0x0000A82301140411 AS DateTime), NULL)
INSERT [dbo].[tblMembership] ([Id], [Title], [Description], [Benifits], [Price], [IsUnlimited], [TotalEmployee], [TotalCustomer], [TotalAppointment], [TotalLocation], [TotalOffers], [IsActive], [Created], [AdministratorId]) VALUES (6, N'Silver Pro +', N'ssadfsdf', N'asdfdsf', 350.0000, 0, 11, 11, 11, 11, 11, 1, CAST(0x0000A8290066B390 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[tblMembership] OFF
SET IDENTITY_INSERT [dbo].[tblServiceLocation] ON 

INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (1, N'Nikol', N'Business location description here 2', N'Address 2 is here', N'Address 3 is here', N'Ahmedabad', N'Gujarat', N'382350', 2, 1, CAST(0x0000A80F00CA897A AS DateTime), 1, 1)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10, N'Main Address', N'', N'This is address 1', N'This is address 2', N'Lebanon', N'Lebanon', N'456478', 3, 0, CAST(0x0000A83800E11BFF AS DateTime), 6, 12)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (11, N'Main Address', N'', N'This is address 1', N'This is address 2', N'Lebanon', N'Lebanon', N'456478', 3, 0, CAST(0x0000A83800E32233 AS DateTime), 6, 13)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (12, N'Main Address', N'', N'This is address 1', N'This is address 2', N'Lebanon', N'Lebanon', N'456478', 3, 0, CAST(0x0000A83800E32240 AS DateTime), 6, 14)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (13, N'Main Address', N'', N'2122. sfs. Dodo', N'2122. sfs. Dodo', N'sdf', N'VT', N'23423', 1, 0, CAST(0x0000A83800E98D2D AS DateTime), 1, 15)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (14, N'Main Address', N'', N'2122. sfs. Dodo', N'2122. sfs. Dodo', N'sdf', N'VT', N'23423', 2, 0, CAST(0x0000A83800F2AF80 AS DateTime), 1, 16)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (15, N'Main Address', N'', N'2122. sfs. Dodo', N'2122. sfs. Dodo', N'sdf', N'VT', N'23423', 2, 0, CAST(0x0000A838011C8E07 AS DateTime), 4, 17)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10007, N'Main Address', N'', N'asdfdsf', N'asdfdsdaf asfdf', N'ahmedabad', N'gujarat', N'382350', 2, 0, CAST(0x0000A84800F0E6D2 AS DateTime), 1, 10007)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10008, N'HDFC Drvon', N'asdf dsf sda f', N'asfd ', N'asdf sda fsd fsdf', N'ahmedabad', N'gujarat', N'382350', 2, 0, CAST(0x0000A86A00ADF34A AS DateTime), 4, 10007)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10009, N'Nirman asdf', N'sadf sa fs f sdf sdf', N'asdf asd fdsf', N'asdf sd fsdf ', N'ahmedabad', N'gujarat', N'382350', 2, 0, CAST(0x0000A857006002DA AS DateTime), 4, 10007)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10010, N'West zone', N'asdjf asdfj', N'hjkhsjdhf dshfjsdf', N'kjhadsfsd fshdfj', N'RAJKOT', N'hjad', N'360002', 2, 0, CAST(0x0000A8570065C517 AS DateTime), 4, 10007)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10011, N'West zone 2', N'asdjf asdfj', N'hjkhsjdhf dshfjsdf', N'kjhadsfsd fshdfj', N'RAJKOT', N'hjad', N'360002', 2, 0, CAST(0x0000A85700661703 AS DateTime), 4, 10007)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10012, N'Washisf ', N'asdfsd sadf', N' asdf asdf sd', N'asd fsd fsdf', N'ahmedabad', N'gujarat', N'382350', 2, 0, CAST(0x0000A85700CF9220 AS DateTime), 4, 10007)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10013, N'Devparak asdf', N'asdf sdadf', N'a sdf sdaf dsaf', N' asdf sdaf sadf', N'ahmedabad', N'gujarat', N'382350', 2, 0, CAST(0x0000A85700D03E54 AS DateTime), 4, 10007)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10014, N'test', N'adsfasdf', N'asdfdsaf sadf', N'asdf sdf ', N'asdf ', N'asd', N'234234', 2, 0, CAST(0x0000A85700D0CFBF AS DateTime), 4, 10007)
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (10015, N'Test 2', N'asdfds f ', N'asdf sadf', N'asdf dsa f', N'asdf sdf', N'sad fsdf', N'3223424', 2, 1, CAST(0x0000A85700D16476 AS DateTime), 4, 10007)
SET IDENTITY_INSERT [dbo].[tblServiceLocation] OFF
SET IDENTITY_INSERT [dbo].[tblTimezone] ON 

INSERT [dbo].[tblTimezone] ([Id], [Title], [UtcOffset], [IsDST], [CountryId], [AdministratorId]) VALUES (1, N'America/Sao Paulo GMT -02:00', -7200, 1, 8, 52)
INSERT [dbo].[tblTimezone] ([Id], [Title], [UtcOffset], [IsDST], [CountryId], [AdministratorId]) VALUES (4, N'Asia/Colombo GMT +05:30', 19800, 0, 2, 52)
INSERT [dbo].[tblTimezone] ([Id], [Title], [UtcOffset], [IsDST], [CountryId], [AdministratorId]) VALUES (6, N'Asia/Beirut GMT +02:00', 7200, 0, 3, 52)
SET IDENTITY_INSERT [dbo].[tblTimezone] OFF
ALTER TABLE [dbo].[tblAppointment]  WITH CHECK ADD  CONSTRAINT [FK_BusinessCustomer_Appointment] FOREIGN KEY([BusinessCustomerId])
REFERENCES [dbo].[tblBusinessCustomer] ([Id])
GO
ALTER TABLE [dbo].[tblAppointment] CHECK CONSTRAINT [FK_BusinessCustomer_Appointment]
GO
ALTER TABLE [dbo].[tblAppointment]  WITH CHECK ADD  CONSTRAINT [FK_BusinessOffer_Appointment] FOREIGN KEY([BusinessOfferId])
REFERENCES [dbo].[tblBusinessOffer] ([Id])
GO
ALTER TABLE [dbo].[tblAppointment] CHECK CONSTRAINT [FK_BusinessOffer_Appointment]
GO
ALTER TABLE [dbo].[tblAppointment]  WITH CHECK ADD  CONSTRAINT [FK_BusinessService_Appointment] FOREIGN KEY([BusinessServiceId])
REFERENCES [dbo].[tblBusinessService] ([Id])
GO
ALTER TABLE [dbo].[tblAppointment] CHECK CONSTRAINT [FK_BusinessService_Appointment]
GO
ALTER TABLE [dbo].[tblAppointment]  WITH CHECK ADD  CONSTRAINT [FK_ServiceLocation_Appointment] FOREIGN KEY([ServiceLocationId])
REFERENCES [dbo].[tblServiceLocation] ([Id])
GO
ALTER TABLE [dbo].[tblAppointment] CHECK CONSTRAINT [FK_ServiceLocation_Appointment]
GO
ALTER TABLE [dbo].[tblAppointment]  WITH CHECK ADD  CONSTRAINT [FK_tblAppointment_tblSchedule] FOREIGN KEY([ScheduleId])
REFERENCES [dbo].[tblSchedule] ([Id])
GO
ALTER TABLE [dbo].[tblAppointment] CHECK CONSTRAINT [FK_tblAppointment_tblSchedule]
GO
ALTER TABLE [dbo].[tblAppointmentDocument]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_AppointmentDocument] FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[tblAppointment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblAppointmentDocument] CHECK CONSTRAINT [FK_Appointment_AppointmentDocument]
GO
ALTER TABLE [dbo].[tblAppointmentDocument]  WITH CHECK ADD  CONSTRAINT [FK_DocumentCategory_AppointmentDocument] FOREIGN KEY([DocumentCategoryId])
REFERENCES [dbo].[tblDocumentCategory] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[tblAppointmentDocument] CHECK CONSTRAINT [FK_DocumentCategory_AppointmentDocument]
GO
ALTER TABLE [dbo].[tblAppointmentFeedback]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_AppointmentFeedback] FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[tblAppointment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblAppointmentFeedback] CHECK CONSTRAINT [FK_Appointment_AppointmentFeedback]
GO
ALTER TABLE [dbo].[tblAppointmentFeedback]  WITH CHECK ADD  CONSTRAINT [FK_BusinessCustomer_AppointmentFeedback] FOREIGN KEY([BusinessCustomerId])
REFERENCES [dbo].[tblBusinessCustomer] ([Id])
GO
ALTER TABLE [dbo].[tblAppointmentFeedback] CHECK CONSTRAINT [FK_BusinessCustomer_AppointmentFeedback]
GO
ALTER TABLE [dbo].[tblAppointmentFeedback]  WITH CHECK ADD  CONSTRAINT [FK_BusinessEmployee_AppointmentFeedback] FOREIGN KEY([BusinessEmployeeId])
REFERENCES [dbo].[tblBusinessEmployee] ([Id])
GO
ALTER TABLE [dbo].[tblAppointmentFeedback] CHECK CONSTRAINT [FK_BusinessEmployee_AppointmentFeedback]
GO
ALTER TABLE [dbo].[tblAppointmentInvitee]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_AppointmentInvitee] FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[tblAppointment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblAppointmentInvitee] CHECK CONSTRAINT [FK_Appointment_AppointmentInvitee]
GO
ALTER TABLE [dbo].[tblAppointmentInvitee]  WITH CHECK ADD  CONSTRAINT [FK_BusinessEmployee_AppointmentInvitee] FOREIGN KEY([BusinessEmployeeId])
REFERENCES [dbo].[tblBusinessEmployee] ([Id])
GO
ALTER TABLE [dbo].[tblAppointmentInvitee] CHECK CONSTRAINT [FK_BusinessEmployee_AppointmentInvitee]
GO
ALTER TABLE [dbo].[tblAppointmentPayment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_AppointmentPayment] FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[tblAppointment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblAppointmentPayment] CHECK CONSTRAINT [FK_Appointment_AppointmentPayment]
GO
ALTER TABLE [dbo].[tblBusiness]  WITH CHECK ADD  CONSTRAINT [FK_BusinessCategory_Business] FOREIGN KEY([BusinessCategoryId])
REFERENCES [dbo].[tblBusinessCategory] ([Id])
GO
ALTER TABLE [dbo].[tblBusiness] CHECK CONSTRAINT [FK_BusinessCategory_Business]
GO
ALTER TABLE [dbo].[tblBusiness]  WITH CHECK ADD  CONSTRAINT [FK_Membership_Business] FOREIGN KEY([MembershipId])
REFERENCES [dbo].[tblMembership] ([Id])
GO
ALTER TABLE [dbo].[tblBusiness] CHECK CONSTRAINT [FK_Membership_Business]
GO
ALTER TABLE [dbo].[tblBusiness]  WITH CHECK ADD  CONSTRAINT [FK_Timezone_Business] FOREIGN KEY([TimezoneId])
REFERENCES [dbo].[tblTimezone] ([Id])
GO
ALTER TABLE [dbo].[tblBusiness] CHECK CONSTRAINT [FK_Timezone_Business]
GO
ALTER TABLE [dbo].[tblBusinessCategory]  WITH CHECK ADD  CONSTRAINT [FK_BusinessCategory_Administrator] FOREIGN KEY([ParentId])
REFERENCES [dbo].[tblBusinessCategory] ([Id])
GO
ALTER TABLE [dbo].[tblBusinessCategory] CHECK CONSTRAINT [FK_BusinessCategory_Administrator]
GO
ALTER TABLE [dbo].[tblBusinessHolidays]  WITH CHECK ADD  CONSTRAINT [FK_BusinessHolidays_ServiceLocation] FOREIGN KEY([ServiceLocationId])
REFERENCES [dbo].[tblServiceLocation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblBusinessHolidays] CHECK CONSTRAINT [FK_BusinessHolidays_ServiceLocation]
GO
ALTER TABLE [dbo].[tblBusinessHours]  WITH CHECK ADD  CONSTRAINT [FK_ServiceLocation_BusinessHours] FOREIGN KEY([ServiceLocationId])
REFERENCES [dbo].[tblServiceLocation] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblBusinessHours] CHECK CONSTRAINT [FK_ServiceLocation_BusinessHours]
GO
ALTER TABLE [dbo].[tblBusinessOffer]  WITH CHECK ADD  CONSTRAINT [FK_BusinessEmployee_BusinessOffer] FOREIGN KEY([BusinessEmployeeId])
REFERENCES [dbo].[tblBusinessEmployee] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[tblBusinessOffer] CHECK CONSTRAINT [FK_BusinessEmployee_BusinessOffer]
GO
ALTER TABLE [dbo].[tblBusinessOfferServiceLocation]  WITH CHECK ADD  CONSTRAINT [FK_BusinessOffer_BusinessOfferServiceLocation] FOREIGN KEY([BusinessOfferId])
REFERENCES [dbo].[tblBusinessOffer] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[tblBusinessOfferServiceLocation] CHECK CONSTRAINT [FK_BusinessOffer_BusinessOfferServiceLocation]
GO
ALTER TABLE [dbo].[tblBusinessOfferServiceLocation]  WITH CHECK ADD  CONSTRAINT [FK_ServiceLocation_BusinessOfferServiceLocation] FOREIGN KEY([ServiceLocationId])
REFERENCES [dbo].[tblServiceLocation] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[tblBusinessOfferServiceLocation] CHECK CONSTRAINT [FK_ServiceLocation_BusinessOfferServiceLocation]
GO
ALTER TABLE [dbo].[tblBusinessService]  WITH CHECK ADD  CONSTRAINT [FK_BusinessEmployee_BusinessService] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[tblBusinessEmployee] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[tblBusinessService] CHECK CONSTRAINT [FK_BusinessEmployee_BusinessService]
GO
ALTER TABLE [dbo].[tblCountry]  WITH CHECK ADD  CONSTRAINT [FK_Administrator_Country] FOREIGN KEY([AdministratorId])
REFERENCES [dbo].[tblAdministrator] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[tblCountry] CHECK CONSTRAINT [FK_Administrator_Country]
GO
ALTER TABLE [dbo].[tblDocumentCategory]  WITH CHECK ADD  CONSTRAINT [FK_DocumentCategory_DocumentCategory] FOREIGN KEY([ParentId])
REFERENCES [dbo].[tblDocumentCategory] ([Id])
GO
ALTER TABLE [dbo].[tblDocumentCategory] CHECK CONSTRAINT [FK_DocumentCategory_DocumentCategory]
GO
ALTER TABLE [dbo].[tblMembership]  WITH CHECK ADD  CONSTRAINT [FK_Administrator_Membership] FOREIGN KEY([AdministratorId])
REFERENCES [dbo].[tblAdministrator] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[tblMembership] CHECK CONSTRAINT [FK_Administrator_Membership]
GO
ALTER TABLE [dbo].[tblServiceLocation]  WITH CHECK ADD  CONSTRAINT [FK_Business_ServiceLocation] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[tblBusiness] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblServiceLocation] CHECK CONSTRAINT [FK_Business_ServiceLocation]
GO
ALTER TABLE [dbo].[tblServiceLocation]  WITH CHECK ADD  CONSTRAINT [FK_Country_ServiceLocation] FOREIGN KEY([CountryId])
REFERENCES [dbo].[tblCountry] ([Id])
GO
ALTER TABLE [dbo].[tblServiceLocation] CHECK CONSTRAINT [FK_Country_ServiceLocation]
GO
ALTER TABLE [dbo].[tblServiceLocation]  WITH CHECK ADD  CONSTRAINT [FK_Timezone_ServiceLocation] FOREIGN KEY([TimezoneId])
REFERENCES [dbo].[tblTimezone] ([Id])
GO
ALTER TABLE [dbo].[tblServiceLocation] CHECK CONSTRAINT [FK_Timezone_ServiceLocation]
GO
ALTER TABLE [dbo].[tblTimezone]  WITH CHECK ADD  CONSTRAINT [FK_Administrator_Timezone] FOREIGN KEY([AdministratorId])
REFERENCES [dbo].[tblAdministrator] ([Id])
GO
ALTER TABLE [dbo].[tblTimezone] CHECK CONSTRAINT [FK_Administrator_Timezone]
GO

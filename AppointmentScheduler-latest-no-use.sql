USE [AppointmentScheduler]
GO
/****** Object:  Table [dbo].[tblAdministrator]    Script Date: 2/21/2018 7:31:50 AM ******/
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
/****** Object:  Table [dbo].[tblAppointment]    Script Date: 2/21/2018 7:31:51 AM ******/
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
 CONSTRAINT [PK_tblAppointment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblAppointmentDocument]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblAppointmentFeedback]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblAppointmentInvitee]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblAppointmentPayment]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblBusiness]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessCategory]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessCustomer]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessEmployee]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessHolidays]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessHours]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessOffer]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessOfferServiceLocation]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessService]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblCountry]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblDocumentCategory]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblMembership]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblServiceLocation]    Script Date: 2/21/2018 7:31:51 AM ******/
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
/****** Object:  Table [dbo].[tblTimezone]    Script Date: 2/21/2018 7:31:51 AM ******/
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

GO
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (1, N'Devendra', N'Gohel', N'N74vW75gn1CfOZJ/+l2bvA==', N'dev.gohel@gmail.com', NULL, 1, 1, CAST(0x0000A865006C0C8C AS DateTime), 1)
GO
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (2, N'Dev', N'Test', N'N74vW75gn1BtSojz5AB4oQ==', N'dev.test@gmail.com', NULL, 1, 1, CAST(0x0000A86500B75163 AS DateTime), 1)
GO
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (3, N'Test', N'Dev', N'N74vW75gn1CfOZJ/+l2bvA==', N'dev.admintester@gmail.com', NULL, 1, 1, CAST(0x0000A86500B9190E AS DateTime), 1)
GO
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (4, N'Test', N'Dev', N'N74vW75gn1CfOZJ/+l2bvA==', N'tester.dev@gmail.com', NULL, 0, 0, CAST(0x0000A86500C229C1 AS DateTime), NULL)
GO
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (5, N'bhavin', N'darji', N'11MCi/0dVzQzi56A2+12iQ==', N'bhavin.darji@gmail.com', NULL, 0, 0, CAST(0x0000A86B0117329E AS DateTime), NULL)
GO
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (6, N'Bhavin', N'Darji', N'WG93BvuasEptSojz5AB4oQ==', N'bhavin@gmail.com', NULL, 0, 0, CAST(0x0000A86B0117C634 AS DateTime), NULL)
GO
INSERT [dbo].[tblAdministrator] ([Id], [FirstName], [LastName], [Password], [Email], [ContactNumber], [IsAdmin], [IsActive], [Created], [AdministratorId]) VALUES (7, N'Bhavin', N'Darji', N'ls1dLP403KHvmGfpPPE3BQ==', N'bhavin@gmail.com', NULL, 0, 0, CAST(0x0000A86B0117EF27 AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[tblAdministrator] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBusiness] ON 

GO
INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (1, N'Digital Zero', N'DZ', NULL, N'23232322', N'23323232', N'devendra.gohel@gmail.com', N'www.digitalzero.com', N'Aesome, Siman, Sendalpal', N'Asdfo, Noepil, ', N'Ahmedabad', N'Gujarat', N'356545', 1, 1, 0, CAST(0x0000A866012F72F3 AS DateTime), 1, 3, 2)
GO
INSERT [dbo].[tblBusiness] ([Id], [Name], [ShortName], [Logo], [PhoneNumbers], [FaxNumbers], [Email], [Website], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsInternational], [IsActive], [Created], [MembershipId], [BusinessCategoryId], [TimezoneId]) VALUES (2, N'Spactron', N'ST', NULL, N'+961 1748847', N'+961 1748847', N'info@spactron.com', N'www.spactron.com', N'First Floor, Gefinor Center, Bloc E, ', N'Hamra Street, Beirut - Lebanon', N'Beirut', N'Beirut ', N'454545', 4, 0, 0, CAST(0x0000A86A009675B4 AS DateTime), 2, 3, 3)
GO
SET IDENTITY_INSERT [dbo].[tblBusiness] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBusinessCategory] ON 

GO
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (1, N'Consultant', NULL, N'B1', N'data:image/png;base64,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', 1, CAST(0x0000A86500EB761C AS DateTime), 2, 1, NULL)
GO
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (2, N'Education', NULL, N'A1', N'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAADICAYAAACtWK6eAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAALEwAACxMBAJqcGAAABtZJREFUeJzt3WmMXWUdx/HvHQtt2jQICrQIyFIjSEr7AkJUGsISZQsGXgnCC8UoAm+ANAEBE5e+cEkamxC2EuICCWggbigCYfGFaECQXQ1lKYS1RhPSsrRTX5xOqMPMf+7znHvOc+6930/yvLv3PP8zM7/M/M659w5IkiRJkiRJkiRJkiRJkiRJkiRJkiRJkiRJQ64HzCs9hNRFy4E/A48Bny48i9QZC4HvA+8B23esSeAa4MMF55KKOwnYwPvBmL5eAb5YbDqpkCXALcwejOnr98CBRSaVWtQDvgH8h/7DMbU2A5cCu7Q+tdSCw6lKeGowpq/HscRrhCwEfsD/l/C6yxKvkXAS8ByDC4YlXiNhKXAr6T/wvwF+nvG8PwAHtXJmUg0TwPmkl/CXgDN2Os4JwL8Sj7EZuAxLvDoqp4RvA9YBi2c43gLge8A7icd8HPhMA+cnZckt4Y8AR/Zx/E8Bf0o89iRwLZZ4FZZTwt8CLgY+lLBPD/gq8O/EvV4FzqxxflKW3BL+a2D/GvvuhSVeHTaoEl6XJV6dM+gSXtcC4LtY4lVYbgn/G3OX8B6wCrgReHbHHtuAF4CbgRN3PCZyKPBA4myWeA1EkyV8CfDbPo73AHP3hx5wLrApcdZXgbPmOLb0AU2X8AOpfkv0e9w3gZV9HHdP4GcZc9+JJV59mAAuoNkSvgh4OvH424GXqQLQj+OBfyYefzPwTSzxmsXhwIOk/VDllPA1iXvsvG5I2Ce3xD8BfDZhH424Jkv4dIuA/ybus/PaSvXnX4rcEn8dsHviXhoxJ9POnfAppyXuNdP6Wsa+lnglWQr8gvQfzrp3wr+Vsef0dXWN/fcEfpqxpyV+TLRRwiNrE/edad06gDks8fqAFbRTwiN1CvrUunFAs8wHvoMlfuwtBH5IOyV8LmcnzjDTWj3gmQ4B7k+cYRK4Hkv80DsZeJ60b36dEj6XvamuRNUJyGENzNUDvkJ6iX8N+FID86hhpUp4P27KmGtq3dPwbLkl/o/AwQ3PpgGYKuGp9xoG/XL0yH7kfXDc21QfcN2G40gv8VuAy7HEd1YXSni/PkdaOZ6k/fsRuSX+SeDolmdVoE4JP6LAvFNWUf3mmmvON4FTC80IlvihdgrdKuGpFlPdW5jp3YEvUl0W/kix6d7XA75MFdaUr7UlvpDcEv4rmi/hufaheqff0VQzzvVGqRI+CvyE9K+7Jb4ldUr46QXmHVXHAv8g7XtgiW/YCuAvpH1TSpXwcTAf+DaW+OJyS/jDlC3h4+IQ4D7SS/x6LPG15Zbwi+hGCR8nuSX+7BLDDrt9gF8yWiV8HOSW+LuAZQXmHToTwIWkl/CNWMK7JLfEXwHsWmDeoZBbwn+MJbyL6pT4VQXm7axFwI+whI+qTzLEJb70zahTgKuAj2c89xmq3yAaDodS/Qmd4nXgEqoP7B4ruSXcNZ5rbEp8bgl3ubYAVzLCJT6nhLtc09dTjFiJzy3hLtdsa5LqUyb3YMjl3Al3ufpdrwPnMIQs4a42193AJ2hAE5d5J4AvALs1cGxpNluA24F3Sw8iSZIkSblKvxYLqv/kur70EOqkKxnch3VnmVdy8x0WAB8rPYQ6aVHpAVJfXSmNFQMiBQyIFDAgUsCASIEuXOZdChxVegh10mPAhtJDSJIkSZIqn6f6t8k5a9/Eve7I3Oe2xH2W1TinYxL3Wpe5z98T95lX45zOTdyrVV14LVakR3uf0j6RuVfOpfLcc0q96ph7Tm09B7pxJXVW3geRAgZEChgQKWBApIABkQIGRAoYEClgQKSAAZECBkQKGBApYECkgAGRAgZEChgQKWBApIABkQIGRAoYEClgQKSAAZECBkQKGBApYECkgAGRAgZEChgQKWBApIABkQIGRAoYEClgQKSAAZECBkQKGBApYECkgAGRAgZEChgQKWBApIABkQIGRAoYEClgQKSAAZECBkQKGBApYECkgAGRAgZEChgQKWBApIABkQIGRAoYEClgQKSAAZECBkQKGBApYECkgAGRAgZEChgQKWBApIABkQIGRAoYEClgQKTAvNIDzOEN4HeZz3078fF/BbZm7PNQ4uM3k39OmxIf/2TmXi8mPn575j4AGzOfJ0mSJEkjp1d6AGAlcF7pIdRJtwD3lhygC1exDgC+XnoIddITFA6I90GkgAGRAgZEChgQKWBApEAXrmK9ANxQegh10lOlB5AkSZKkNjX9WqzlwNqG99B4W0ODL0dp+irWbsDxDe+h8ba+yYN7H0QKGBApYECkgAGRAgZECjR9Fet5YHXDe2i8PVp6AEmSJEmSJEmSJEmSJEmSJEmSJEkaCv8DWc7JHy0ebU0AAAAASUVORK5CYII=', 1, CAST(0x0000A86500EBD1E3 AS DateTime), 1, 1, NULL)
GO
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (3, N'Doctor', NULL, N'AA1', N'data:image/png;base64,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', 1, CAST(0x0000A86500EC79E7 AS DateTime), 1, 1, 1)
GO
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (4, N'Lawyer', NULL, N'AA2', N'data:image/png;base64,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', 1, CAST(0x0000A86500EC9DEB AS DateTime), 2, 1, 1)
GO
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (5, N'University', NULL, N'A11', N'data:image/png;base64,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', 1, CAST(0x0000A86501043177 AS DateTime), 1, 1, 2)
GO
INSERT [dbo].[tblBusinessCategory] ([Id], [Name], [Description], [Type], [PictureLink], [IsActive], [Created], [OrderNumber], [AdministratorId], [ParentId]) VALUES (6, N'School', NULL, N'A12', N'data:image/png;base64,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', 1, CAST(0x0000A865010493EB AS DateTime), 2, 1, 2)
GO
SET IDENTITY_INSERT [dbo].[tblBusinessCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBusinessEmployee] ON 

GO
INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (1, N'Digital', N'Zero', N'devendra.gohel@gmail.com', NULL, N'232323232', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A866012F7566 AS DateTime), 1, 1)
GO
INSERT [dbo].[tblBusinessEmployee] ([Id], [FirstName], [LastName], [Email], [STD], [PhoneNumber], [Password], [IsActive], [Created], [IsAdmin], [ServiceLocationId]) VALUES (2, N'Spactron', N'Test', N'infotest@spactron.com', NULL, N'+961 1748847', N'N74vW75gn1CfOZJ/+l2bvA==', 1, CAST(0x0000A86A009677FD AS DateTime), 1, 2)
GO
SET IDENTITY_INSERT [dbo].[tblBusinessEmployee] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBusinessHolidays] ON 

GO
INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (1, CAST(0xB83D0B00 AS Date), 1, 1)
GO
INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (2, CAST(0xE43D0B00 AS Date), 2, 1)
GO
INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (3, CAST(0xB63D0B00 AS Date), 0, 2)
GO
INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (4, CAST(0xBC3D0B00 AS Date), 1, 2)
GO
INSERT [dbo].[tblBusinessHolidays] ([Id], [OnDate], [Type], [ServiceLocationId]) VALUES (5, CAST(0xCB3D0B00 AS Date), 2, 2)
GO
SET IDENTITY_INSERT [dbo].[tblBusinessHolidays] OFF
GO
SET IDENTITY_INSERT [dbo].[tblBusinessHours] ON 

GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (1, 0, 1, 0, CAST(0x0000A87200000000 AS DateTime), CAST(0x0000A8720083D600 AS DateTime), 0, CAST(0x0000A87200000000 AS DateTime), CAST(0x0000A87200000000 AS DateTime), 0, CAST(0x0000A87200000000 AS DateTime), CAST(0x0000A87200000000 AS DateTime), 1)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (2, 1, 0, 0, CAST(0x0000A8660083D600 AS DateTime), CAST(0x0000A8660128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 1)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (3, 2, 0, 0, CAST(0x0000A8660083D600 AS DateTime), CAST(0x0000A86600D63BC0 AS DateTime), 1, CAST(0x0000A86600E6B680 AS DateTime), CAST(0x0000A86601499700 AS DateTime), 0, CAST(0x0000A86600000000 AS DateTime), CAST(0x0000A86600000000 AS DateTime), 1)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (4, 3, 0, 0, CAST(0x0000A8660083D600 AS DateTime), CAST(0x0000A8660128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 1)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (5, 4, 0, 0, CAST(0x0000A8660083D600 AS DateTime), CAST(0x0000A8660128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 1)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (6, 5, 0, 0, CAST(0x0000A8660083D600 AS DateTime), CAST(0x0000A8660128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 1)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (7, 6, 0, 1, CAST(0x0000A87A00000000 AS DateTime), CAST(0x0000A87A00000000 AS DateTime), 0, CAST(0x0000A87A00000000 AS DateTime), CAST(0x0000A87A00000000 AS DateTime), 0, CAST(0x0000A87A00000000 AS DateTime), CAST(0x0000A87A00000000 AS DateTime), 1)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (8, 0, 1, 1, CAST(0x0000A86A00000000 AS DateTime), CAST(0x0000A86A00000000 AS DateTime), 0, CAST(0x0000A86A00000000 AS DateTime), CAST(0x0000A86A00000000 AS DateTime), 0, CAST(0x0000A86A00000000 AS DateTime), CAST(0x0000A86A00000000 AS DateTime), 2)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (9, 1, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 2)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (10, 2, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 2)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (11, 3, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 2)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (12, 4, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 2)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (13, 5, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 2)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (14, 6, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 2)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (15, 0, 1, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 3)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (16, 1, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 3)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (17, 2, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 3)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (18, 3, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 3)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (19, 4, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 3)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (20, 5, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 3)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (21, 6, 0, 0, CAST(0x0000A86A0083D600 AS DateTime), CAST(0x0000A86A0128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 3)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (22, 0, 1, 1, CAST(0x0000A87A00000000 AS DateTime), CAST(0x0000A87A00000000 AS DateTime), 0, CAST(0x0000A87A00000000 AS DateTime), CAST(0x0000A87A00000000 AS DateTime), 0, CAST(0x0000A87A00000000 AS DateTime), CAST(0x0000A87A00000000 AS DateTime), 4)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (23, 1, 0, 0, CAST(0x0000A87A0083D600 AS DateTime), CAST(0x0000A87A01499700 AS DateTime), 0, CAST(0x0000A87A00000000 AS DateTime), CAST(0x0000A87A00000000 AS DateTime), 0, CAST(0x0000A87A00000000 AS DateTime), CAST(0x0000A87A00000000 AS DateTime), 4)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (24, 2, 0, 0, CAST(0x0000A8720083D600 AS DateTime), CAST(0x0000A8720128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 4)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (25, 3, 0, 0, CAST(0x0000A8720083D600 AS DateTime), CAST(0x0000A8720128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 4)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (26, 4, 0, 0, CAST(0x0000A8720083D600 AS DateTime), CAST(0x0000A8720128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 4)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (27, 5, 0, 0, CAST(0x0000A8720083D600 AS DateTime), CAST(0x0000A8720128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 4)
GO
INSERT [dbo].[tblBusinessHours] ([Id], [WeekDayId], [IsStartDay], [IsHoliday], [From], [To], [IsSplit1], [FromSplit1], [ToSplit1], [IsSplit2], [FromSplit2], [ToSplit2], [ServiceLocationId]) VALUES (28, 6, 0, 0, CAST(0x0000A8720083D600 AS DateTime), CAST(0x0000A8720128A180 AS DateTime), 0, NULL, NULL, 0, NULL, NULL, 4)
GO
SET IDENTITY_INSERT [dbo].[tblBusinessHours] OFF
GO
SET IDENTITY_INSERT [dbo].[tblCountry] ON 

GO
INSERT [dbo].[tblCountry] ([Id], [Name], [ISO], [ISO3], [CurrencyName], [CurrencyCode], [PhoneCode], [AdministratorId]) VALUES (1, N'India', N'IN', N'IND', N'Rupees', N'₹', 91, 1)
GO
INSERT [dbo].[tblCountry] ([Id], [Name], [ISO], [ISO3], [CurrencyName], [CurrencyCode], [PhoneCode], [AdministratorId]) VALUES (4, N'Lebanon', N'LB', N'LBN', N'Lebanese pound', N'ل.ل.‎', 961, 1)
GO
INSERT [dbo].[tblCountry] ([Id], [Name], [ISO], [ISO3], [CurrencyName], [CurrencyCode], [PhoneCode], [AdministratorId]) VALUES (5, N'United State of America', N'US', N'USA', N'Dollar', N'$', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[tblCountry] OFF
GO
SET IDENTITY_INSERT [dbo].[tblMembership] ON 

GO
INSERT [dbo].[tblMembership] ([Id], [Title], [Description], [Benifits], [Price], [IsUnlimited], [TotalEmployee], [TotalCustomer], [TotalAppointment], [TotalLocation], [TotalOffers], [IsActive], [Created], [AdministratorId]) VALUES (1, N'Silver', NULL, NULL, 1000.0000, 0, 50, 1000, 0, 1, 1, 1, CAST(0x0000A86500EA8D64 AS DateTime), 1)
GO
INSERT [dbo].[tblMembership] ([Id], [Title], [Description], [Benifits], [Price], [IsUnlimited], [TotalEmployee], [TotalCustomer], [TotalAppointment], [TotalLocation], [TotalOffers], [IsActive], [Created], [AdministratorId]) VALUES (2, N'Gold', NULL, NULL, 2000.0000, 0, 100, 500, 0, 5, 5, 1, CAST(0x0000A86500EAC1C5 AS DateTime), 1)
GO
INSERT [dbo].[tblMembership] ([Id], [Title], [Description], [Benifits], [Price], [IsUnlimited], [TotalEmployee], [TotalCustomer], [TotalAppointment], [TotalLocation], [TotalOffers], [IsActive], [Created], [AdministratorId]) VALUES (3, N'Platinum', NULL, NULL, 5000.0000, 1, 0, 0, 0, 0, 0, 0, CAST(0x0000A86500EAF736 AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[tblMembership] OFF
GO
SET IDENTITY_INSERT [dbo].[tblServiceLocation] ON 

GO
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (1, N'Main Address', N'', N'Aesome, Siman, Sendalpal', N'Asdfo, Noepil, ', N'Ahmedabad', N'Gujarat', N'356545', 1, 0, CAST(0x0000A866012F73A0 AS DateTime), 2, 1)
GO
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (2, N'Main Address', N'', N'First Floor, Gefinor Center, Bloc E, ', N'Hamra Street, Beirut - Lebanon', N'Beirut', N'Beirut ', N'454545', 4, 0, CAST(0x0000A86A0096763C AS DateTime), 3, 2)
GO
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (3, N'Spactroon business', N'xyz information here', N'office address 1', N'office address 2', N'Beirut', N'Beirut', N'23423', 4, 0, CAST(0x0000A86A00AFA6AA AS DateTime), 3, 2)
GO
INSERT [dbo].[tblServiceLocation] ([Id], [Name], [Description], [Add1], [Add2], [City], [State], [Zip], [CountryId], [IsActive], [Created], [TimezoneId], [BusinessId]) VALUES (4, N'Nanavati Chowk', NULL, N'218, JASAL COMMERCIAL COMPLEX', N'NANAVATI CHOWK', N'RAJKOT', N'GUJARAT', N'360005', 1, 0, CAST(0x0000A872003A810F AS DateTime), 2, 1)
GO
SET IDENTITY_INSERT [dbo].[tblServiceLocation] OFF
GO
SET IDENTITY_INSERT [dbo].[tblTimezone] ON 

GO
INSERT [dbo].[tblTimezone] ([Id], [Title], [UtcOffset], [IsDST], [CountryId], [AdministratorId]) VALUES (1, N'America/Sao Paulo (GMT -02:00)', -7200, 1, 5, 1)
GO
INSERT [dbo].[tblTimezone] ([Id], [Title], [UtcOffset], [IsDST], [CountryId], [AdministratorId]) VALUES (2, N'Asia/Colombo (GMT +05:30)', 19800, 0, 1, 1)
GO
INSERT [dbo].[tblTimezone] ([Id], [Title], [UtcOffset], [IsDST], [CountryId], [AdministratorId]) VALUES (3, N'Asia/Beirut (GMT +02:00)', 7200, 0, 4, 1)
GO
SET IDENTITY_INSERT [dbo].[tblTimezone] OFF
GO
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

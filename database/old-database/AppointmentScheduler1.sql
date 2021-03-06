USE [AppointmentScheduler]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 5/21/2018 5:48:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 5/21/2018 5:48:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 5/21/2018 5:48:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 5/21/2018 5:48:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 5/21/2018 5:48:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 5/21/2018 5:48:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblAdministrator]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblAppointment]    Script Date: 5/21/2018 5:48:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAppointment](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GlobalAppointmentId] [nvarchar](100) NOT NULL,
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
/****** Object:  Table [dbo].[tblAppointmentDocument]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblAppointmentFeedback]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblAppointmentInvitee]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblAppointmentPayment]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblBusiness]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessCategory]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessCustomer]    Script Date: 5/21/2018 5:48:32 AM ******/
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
	[ServiceLocationId] [bigint] NULL,
 CONSTRAINT [PK_tblBusinessCustomer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblBusinessEmployee]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessHolidays]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessHours]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessOffer]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessOfferServiceLocation]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblBusinessService]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblCountry]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblDocumentCategory]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblMembership]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblServiceLocation]    Script Date: 5/21/2018 5:48:32 AM ******/
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
/****** Object:  Table [dbo].[tblTimezone]    Script Date: 5/21/2018 5:48:32 AM ******/
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
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
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

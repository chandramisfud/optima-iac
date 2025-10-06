/****** Object:  Table [dbo].[tbmst_account]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubChannelId] [int] NOT NULL,
	[LongDesc] [varchar](50) NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[RefId]  AS (('ACC'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[SAPCode] [varchar](15) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_activity]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_activity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SubCategoryId] [int] NOT NULL,
	[LongDesc] [varchar](50) NOT NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[RefId]  AS (('ACT'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tblmst_activity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_activity_period]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_activity_period](
	[Id] [int] NOT NULL,
	[startdate] [date] NOT NULL,
	[enddate] [date] NOT NULL,
 CONSTRAINT [PK_tbmst_activity_period] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[startdate] ASC,
	[enddate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_brand]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_brand](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrincipalId] [int] NOT NULL,
	[LongDesc] [varchar](255) NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[RefID]  AS (('BR'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
	[BrandGroupId] [int] NULL,
 CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_brand_group]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_brand_group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrincipalId] [int] NOT NULL,
	[LongDesc] [varchar](255) NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[RefID]  AS (('BRG'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
	[SAPCode] [varchar](20) NULL,
 CONSTRAINT [PK__tbmst_br__3214EC073E870B35] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_cancelreason]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_cancelreason](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LongDesc] [varchar](100) NOT NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_CancelReason] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_category]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LongDesc] [varchar](50) NOT NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[RefId]  AS (('CTG'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tblmst_category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_channel]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_channel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LongDesc] [varchar](50) NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[RefId]  AS (('CHN'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_Channel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_distributor]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_distributor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LongDesc] [varchar](50) NOT NULL,
	[ShortDesc] [varchar](50) NULL,
	[CompanyName] [varchar](255) NULL,
	[Address] [varchar](255) NULL,
	[NPWP] [varchar](255) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[RefId]  AS (('DIS'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[Phone] [varchar](50) NULL,
	[Fax] [varchar](50) NULL,
	[NoRekening] [varchar](50) NULL,
	[BankName] [varchar](50) NULL,
	[BankCabang] [varchar](50) NULL,
	[ClaimManager] [varchar](50) NULL,
	[SAPCode] [varchar](30) NULL,
	[SAPCodex] [varchar](30) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_Distributor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_doc_status]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_doc_status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RefId] [varchar](50) NOT NULL,
	[DocType] [varchar](50) NOT NULL,
	[LongDesc] [varchar](50) NOT NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_DocStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_investment_type]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_investment_type](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RefId] [varchar](10) NULL,
	[LongDesc] [varchar](200) NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbmst_investment_type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_investment_type_deleted]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_investment_type_deleted](
	[Id] [int] NULL,
	[RefId] [varchar](10) NULL,
	[LongDesc] [varchar](200) NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_main_activity]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_main_activity](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[longDesc] [varchar](255) NULL,
	[createdOn] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[createdByEmail] [varchar](50) NULL,
	[modifiedOn] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
	[modifiedByEmail] [varchar](50) NULL,
	[deletedOn] [datetime] NULL,
	[deletedBy] [varchar](50) NULL,
	[deleteByEmail] [varchar](50) NULL,
 CONSTRAINT [tbmst_main_activity_pk] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_main_activity_dtl]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_main_activity_dtl](
	[subactivityid] [int] NOT NULL,
	[mainactivityid] [int] NOT NULL,
	[createdOn] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[createdByEmail] [varchar](50) NULL,
 CONSTRAINT [tbmst_main_activity_dtl_pk] PRIMARY KEY CLUSTERED 
(
	[subactivityid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_master_status]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_master_status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusCode] [nvarchar](5) NULL,
	[StatusDesc] [nvarchar](50) NOT NULL,
	[StatusType] [nvarchar](3) NULL,
	[StatusSeq] [int] NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_MasterStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_MasterStatus] UNIQUE NONCLUSTERED 
(
	[StatusCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_mechanism]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_mechanism](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NULL,
	[Entity] [varchar](100) NULL,
	[SubCategoryId] [int] NULL,
	[SubCategory] [varchar](100) NULL,
	[ActivityId] [int] NULL,
	[Activity] [varchar](100) NULL,
	[SubActivityId] [int] NULL,
	[SubActivity] [varchar](100) NULL,
	[ProductId] [int] NULL,
	[Product] [varchar](100) NULL,
	[Requirement] [varchar](255) NULL,
	[Discount] [varchar](255) NULL,
	[Mechanism] [varchar](255) NULL,
	[ChannelId] [int] NULL,
	[Channel] [varchar](255) NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbmst_mechanism] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_principal]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_principal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LongDesc] [varchar](50) NULL,
	[ShortDesc] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[RefId]  AS (('PCP'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[DescForInvoice] [varchar](50) NULL,
	[EntityUp] [varchar](50) NULL,
	[EntityAddress] [varchar](255) NULL,
	[CompanyName] [varchar](255) NULL,
	[EntityNPWP] [varchar](50) NULL,
	[ShortDesc2] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tblmst_principal] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_product]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrincipalId] [int] NOT NULL,
	[BrandId] [int] NOT NULL,
	[LongDesc] [varchar](255) NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[RefID]  AS (('SKU'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[SeqNo] [int] NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_product_period]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_product_period](
	[Id] [int] NOT NULL,
	[startdate] [date] NOT NULL,
	[enddate] [date] NOT NULL,
 CONSTRAINT [PK_tbmst_product_period] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[startdate] ASC,
	[enddate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_profit_center]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_profit_center](
	[ProfitCenter] [varchar](10) NOT NULL,
	[ProfitCenterDesc] [varchar](250) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_ProfitCenter] PRIMARY KEY CLUSTERED 
(
	[ProfitCenter] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_region]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_region](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LongDesc] [varchar](50) NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[RefID]  AS (('RG'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[SAPCode] [int] NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_sellingpoint]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_sellingpoint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RefId] [varchar](36) NULL,
	[AreaCode] [varchar](3) NULL,
	[RegionId] [int] NOT NULL,
	[LongDesc] [varchar](50) NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[ProfitCenter] [varchar](10) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_Sellingpoint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_subaccount]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_subaccount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[LongDesc] [varchar](50) NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[RefId]  AS (('SACC'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[SAPCode] [varchar](20) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_SubAccount] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_subactivity]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_subactivity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SubCategoryId] [int] NOT NULL,
	[ActivityId] [int] NOT NULL,
	[SubActivityTypeId] [int] NOT NULL,
	[LongDesc] [varchar](50) NOT NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[RefId]  AS (('SACT'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tblmst_subactivity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_subactivity_period]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_subactivity_period](
	[Id] [int] NOT NULL,
	[startdate] [date] NOT NULL,
	[enddate] [date] NOT NULL,
 CONSTRAINT [PK_tbmst_subactivity_period] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[startdate] ASC,
	[enddate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_subactivity_type]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_subactivity_type](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RefId]  AS (('SAT'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[LongDesc] [varchar](50) NOT NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbmst_subactivity_type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_subcategory]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_subcategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[LongDesc] [varchar](50) NOT NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[RefId]  AS (('SCTG'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tblmst_sub_category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_subcategory_period]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_subcategory_period](
	[Id] [int] NOT NULL,
	[startdate] [date] NOT NULL,
	[enddate] [date] NOT NULL,
 CONSTRAINT [PK_tbmst_subcategory_period] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[startdate] ASC,
	[enddate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_subchannel]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_subchannel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChannelId] [int] NOT NULL,
	[LongDesc] [varchar](50) NULL,
	[ShortDesc] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[RefId]  AS (('SCHN'+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_SubChannel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbmst_wht_type]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbmst_wht_type](
	[id] [int] NOT NULL,
	[WHTType] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tbmst_wht_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

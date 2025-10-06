/****** Object:  Table [dbo].[tbset_app]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_app](
	[license] [varchar](100) NULL,
	[ppn] [numeric](20, 2) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_brand_group]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_brand_group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BrandId] [int] NOT NULL,
	[LongDesc] [varchar](255) NULL,
	[ShortDesc] [varchar](10) NULL,
	[SAPCode] [varchar](20) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_BrandGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_config_dropdown]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_config_dropdown](
	[id] [int] NOT NULL,
	[category] [varchar](30) NOT NULL,
	[name] [varchar](30) NOT NULL,
	[nourut] [int] NULL,
	[description] [varchar](100) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [tbset_config_dropdown_PK] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_config_email]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_config_email](
	[id] [varchar](50) NOT NULL,
	[email] [varchar](255) NULL,
 CONSTRAINT [PK_tbset_config_email] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_config_promo_calculator]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_config_promo_calculator](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[mainActivityId] [int] NULL,
	[baseline] [tinyint] NULL,
	[totalSales] [tinyint] NULL,
	[uplift] [tinyint] NULL,
	[salesContribution] [tinyint] NULL,
	[storesCoverage] [tinyint] NULL,
	[redemptionRate] [tinyint] NULL,
	[cr] [tinyint] NULL,
	[cost] [tinyint] NULL,
	[createdOn] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[createdByEmail] [varchar](50) NULL,
	[modifiedOn] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
	[modifiedByEmail] [varchar](50) NULL,
	[deletedOn] [datetime] NULL,
	[deletedBy] [varchar](50) NULL,
	[deleteByEmail] [varchar](50) NULL,
	[baselineRecon] [tinyint] NULL,
	[totalSalesRecon] [tinyint] NULL,
	[upliftRecon] [tinyint] NULL,
	[salesContributionRecon] [tinyint] NULL,
	[storesCoverageRecon] [tinyint] NULL,
	[redemptionRateRecon] [tinyint] NULL,
	[crRecon] [tinyint] NULL,
	[costRecon] [tinyint] NULL,
	[channelId] [int] NOT NULL,
 CONSTRAINT [tbset_config_promo_calculator_pk] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_config_promo_items]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_config_promo_items](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[categoryId] [int] NULL,
	[budgetYear] [bit] NULL,
	[promoPlanning] [bit] NULL,
	[budgetSource] [bit] NULL,
	[entity] [bit] NULL,
	[distributor] [bit] NULL,
	[subCategory] [bit] NULL,
	[activity] [bit] NULL,
	[subActivity] [bit] NULL,
	[subActivityType] [bit] NULL,
	[startPromo] [bit] NULL,
	[endPromo] [bit] NULL,
	[activityName] [bit] NULL,
	[initiatorNotes] [bit] NULL,
	[incrSales] [bit] NULL,
	[investment] [bit] NULL,
	[channel] [bit] NULL,
	[subChannel] [bit] NULL,
	[account] [bit] NULL,
	[subAccount] [bit] NULL,
	[region] [bit] NULL,
	[groupBrand] [bit] NULL,
	[brand] [bit] NULL,
	[SKU] [bit] NULL,
	[mechanism] [bit] NULL,
	[Attachment] [bit] NULL,
	[ROI] [bit] NULL,
	[CR] [bit] NULL,
	[createOn] [datetime] NULL,
	[createBy] [varchar](50) NULL,
	[createdEmail] [varchar](50) NULL,
	[modifiedOn] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
	[modifiedEmail] [varchar](50) NULL,
	[isDelete] [bit] NULL,
	[deleteOn] [datetime] NULL,
	[deleteBy] [varchar](50) NULL,
	[deleteEmail] [varchar](50) NULL,
 CONSTRAINT [tbset_config_promo_items_pk] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_config_promo_items_disabled]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_config_promo_items_disabled](
	[categoryId] [int] NULL,
	[budgetYear] [bit] NULL,
	[promoPlanning] [bit] NULL,
	[budgetSource] [bit] NULL,
	[entity] [bit] NULL,
	[distributor] [bit] NULL,
	[subCategory] [bit] NULL,
	[activity] [bit] NULL,
	[subActivity] [bit] NULL,
	[subActivityType] [bit] NULL,
	[startPromo] [bit] NULL,
	[endPromo] [bit] NULL,
	[activityName] [bit] NULL,
	[initiatorNotes] [bit] NULL,
	[incrSales] [bit] NULL,
	[investment] [bit] NULL,
	[channel] [bit] NULL,
	[subChannel] [bit] NULL,
	[account] [bit] NULL,
	[subAccount] [bit] NULL,
	[region] [bit] NULL,
	[groupBrand] [bit] NULL,
	[brand] [bit] NULL,
	[SKU] [bit] NULL,
	[mechanism] [bit] NULL,
	[Attachment] [bit] NULL,
	[ROI] [bit] NULL,
	[CR] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_config_reminder]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_config_reminder](
	[id] [int] NOT NULL,
	[remindertype] [int] NOT NULL,
	[category] [int] NOT NULL,
	[Description] [varchar](50) NULL,
	[daysfrom] [int] NOT NULL,
	[daysto] [int] NOT NULL,
	[frequency] [int] NOT NULL,
	[userinput] [varchar](30) NULL,
	[dateinput] [datetime] NULL,
	[useredit] [varchar](30) NULL,
	[dateedit] [datetime] NULL,
	[isdeleted] [int] NULL,
	[deletedby] [varchar](50) NULL,
	[deletedon] [datetime] NULL,
	[datereminder] [date] NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeletedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [tbset_config_reminder_PK] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_distributor_wht]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_distributor_wht](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[distributor] [varchar](50) NOT NULL,
	[subActivity] [varchar](50) NOT NULL,
	[subAccount] [varchar](50) NOT NULL,
	[WHTType] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NOT NULL,
	[ModifiedBy] [varchar](20) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_tbset_distributor_wht] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_general_parameter]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_general_parameter](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParameterName] [varchar](50) NOT NULL,
	[Sequence] [int] NOT NULL,
	[ParameterValue] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[ParameterDesc] [varchar](255) NULL,
	[ParameterNumericValue] [int] NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_GeneralParameter] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[ParameterName] ASC,
	[Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_general_parameter_paralel]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_general_parameter_paralel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParameterName] [varchar](50) NOT NULL,
	[Sequence] [int] NOT NULL,
	[ParameterValue] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[ParameterDesc] [varchar](255) NULL,
	[ParameterNumericValue] [int] NULL,
 CONSTRAINT [PK_GeneralParameter_paralel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[ParameterName] ASC,
	[Sequence] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_major_changes]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_major_changes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Activity] [bit] NULL,
	[SubActivity] [bit] NULL,
	[StartPromo] [bit] NULL,
	[EndPromo] [bit] NULL,
	[ActivityDesc] [bit] NULL,
	[InitiatorNotes] [bit] NULL,
	[IncrSales] [bit] NULL,
	[Investment] [bit] NULL,
	[ROI] [bit] NULL,
	[CR] [bit] NULL,
	[Channel] [bit] NULL,
	[SubChannel] [bit] NULL,
	[Account] [bit] NULL,
	[SubAccount] [bit] NULL,
	[Region] [bit] NULL,
	[Brand] [bit] NULL,
	[SKU] [bit] NULL,
	[Mechanism] [bit] NULL,
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
	[CategoryId] [int] NULL,
	[Year] [bit] NULL,
	[Entity] [bit] NULL,
	[Distributor] [bit] NULL,
	[SubCategory] [bit] NULL,
	[BudgetSources] [bit] NULL,
	[PromoPlan] [bit] NULL,
	[Attachment] [bit] NULL,
	[GroupBrand] [bit] NULL,
 CONSTRAINT [tbset_major_changes_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_brand_distributor]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_brand_distributor](
	[brandId] [int] NOT NULL,
	[distributorId] [int] NOT NULL,
	[createdOn] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[createdEmail] [varchar](100) NULL,
	[modfiedOn] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
	[modifiedEmail] [varchar](100) NULL,
	[isDeleted] [bit] NULL,
	[deletedOn] [datetime] NULL,
	[deletedBy] [varchar](50) NULL,
	[deletedEmail] [varchar](100) NULL,
 CONSTRAINT [PK_tbset_map_brand_distributorl] PRIMARY KEY CLUSTERED 
(
	[brandId] ASC,
	[distributorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_brand_subacc_dist_ts]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_brand_subacc_dist_ts](
	[BrandId] [int] NOT NULL,
	[SubAccountId] [int] NOT NULL,
	[DistributorId] [int] NOT NULL,
	[GroupBrandDesc] [varchar](255) NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](1) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](1) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](1) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_map_brand_subacc_dist_ts] PRIMARY KEY CLUSTERED 
(
	[BrandId] ASC,
	[SubAccountId] ASC,
	[DistributorId] ASC,
	[GroupBrandDesc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_channel_brand_distributor]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_channel_brand_distributor](
	[channelId] [int] NOT NULL,
	[brandId] [int] NOT NULL,
	[distributorId] [int] NOT NULL,
	[createdOn] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[createdEmail] [varchar](100) NULL,
	[modfiedOn] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
	[modifiedEmail] [varchar](100) NULL,
	[isDeleted] [bit] NULL,
	[deletedOn] [datetime] NULL,
	[deletedBy] [varchar](50) NULL,
	[deletedEmail] [varchar](100) NULL,
 CONSTRAINT [PK_tbset_map_channel_brand_distributor] PRIMARY KEY CLUSTERED 
(
	[channelId] ASC,
	[brandId] ASC,
	[distributorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_channel_head]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_channel_head](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChannelId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_map_channel_head] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_channel_subactivity_skp_draft]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_channel_subactivity_skp_draft](
	[ChannelId] [int] NOT NULL,
	[SubActivityId] [int] NOT NULL,
	[SKP_Draft] [int] NOT NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_map_channel_subactivity_skp_draft] PRIMARY KEY CLUSTERED 
(
	[ChannelId] ASC,
	[SubActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_distributor_account]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_distributor_account](
	[DistributorId] [int] NOT NULL,
	[ChannelId] [int] NOT NULL,
	[SubChannelId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
	[SubAccountId] [int] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](100) NULL,
	[ModifiedEmail] [varchar](100) NULL,
	[DeleteEmail] [varchar](100) NULL,
 CONSTRAINT [PK_MAPDIST_ACC] PRIMARY KEY CLUSTERED 
(
	[DistributorId] ASC,
	[SubAccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_investment_type]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_investment_type](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubactivityId] [int] NULL,
	[InvestmentTypeId] [int] NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](36) NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
 CONSTRAINT [PK_tbset_map_investment_type] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_investment_type_deleted]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_investment_type_deleted](
	[Id] [int] NULL,
	[SubactivityId] [int] NULL,
	[InvestmentTypeId] [int] NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](36) NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_product_sap]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_product_sap](
	[ProductId] [int] NOT NULL,
	[SAPCode] [varchar](20) NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [tbset_map_product_sap_PK] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[SAPCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_profile_category]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_profile_category](
	[ProfileID] [varchar](50) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](100) NULL,
 CONSTRAINT [PK_tbset_map_profile_category] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC,
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_promorecon_period_subactivity]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_promorecon_period_subactivity](
	[SubActivityId] [int] NOT NULL,
	[AllowEdit] [tinyint] NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](36) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_map_promorecon_period_subactivity] PRIMARY KEY CLUSTERED 
(
	[SubActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_subaccount_region]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_subaccount_region](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[subAccountId] [int] NOT NULL,
	[regionId] [int] NOT NULL,
	[createOn] [datetime] NULL,
	[createBy] [varchar](36) NULL,
	[createdEmail] [varchar](50) NULL,
	[modifiedOn] [datetime] NULL,
	[modifiedBy] [varchar](10) NULL,
	[modifiedEmail] [varchar](50) NULL,
	[isDeleted] [bit] NULL,
	[deletedOn] [datetime] NULL,
	[deletedBy] [varchar](36) NULL,
	[deletedEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_map_subaccount_region] PRIMARY KEY CLUSTERED 
(
	[subAccountId] ASC,
	[regionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_subaccount_sap]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_subaccount_sap](
	[subaccountid] [int] NULL,
	[sapcode] [varchar](20) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
	[accountid] [int] NULL,
	[channelid] [int] NULL,
	[subchannelid] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_subactivity_salesamount]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_subactivity_salesamount](
	[SubActivityID] [int] NULL,
	[SalesAmount] [varchar](5) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_subcategory_channel]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_subcategory_channel](
	[subCategoryId] [int] NOT NULL,
	[channelId] [int] NOT NULL,
	[createdOn] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[createdEmail] [varchar](100) NULL,
	[modfiedOn] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
	[modifiedEmail] [varchar](100) NULL,
	[isDeleted] [bit] NULL,
	[deletedOn] [datetime] NULL,
	[deletedBy] [varchar](50) NULL,
	[deletedEmail] [varchar](100) NULL,
 CONSTRAINT [PK_tbset_map_subcategory_channel] PRIMARY KEY CLUSTERED 
(
	[subCategoryId] ASC,
	[channelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_user_channel]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_user_channel](
	[userid] [varchar](50) NOT NULL,
	[channelid] [int] NOT NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_map_user_channel] PRIMARY KEY CLUSTERED 
(
	[userid] ASC,
	[channelid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_user_profitcenter]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_user_profitcenter](
	[UserId] [varchar](50) NULL,
	[ProfitCenter] [varchar](10) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_map_user_subaccount_skp_draft]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_map_user_subaccount_skp_draft](
	[id] [int] NOT NULL,
	[longdesc] [varchar](50) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_mapping_material]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_mapping_material](
	[MaterialNumber] [varchar](20) NULL,
	[Description] [varchar](255) NULL,
	[WHT_Type] [varchar](255) NULL,
	[WHT_Code] [varchar](255) NULL,
	[Purpose] [varchar](255) NULL,
	[Entity] [varchar](255) NULL,
	[EntityId] [int] NULL,
	[PPNPct] [decimal](9, 4) NULL,
	[PPHPct] [decimal](10, 4) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_mapping_pricecondition]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_mapping_pricecondition](
	[CategoryId] [int] NOT NULL,
	[SubCategoryId] [int] NOT NULL,
	[ActivityId] [int] NOT NULL,
	[PriceConditionForPayment] [varchar](10) NULL,
	[PriceConditionForAccrual] [varchar](10) NULL,
	[PriceConditionForAccrualReversal] [varchar](10) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](50) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [tbset_mapping_pricecondition_PK] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC,
	[SubCategoryId] ASC,
	[ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[tbset_matrix_approval_process]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_approval_process](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsFinished] [bit] NOT NULL,
	[UserProfile] [varchar](50) NOT NULL,
	[UserEmail] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbset_matrix_approval_process] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_matrix_approval_process_detail]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_approval_process_detail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessId] [int] NOT NULL,
	[MatrixPromoApprovalId] [int] NOT NULL,
	[PromoId] [int] NULL,
 CONSTRAINT [PK_tbset_matrix_approval_process_detail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_matrix_budget_approval]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_budget_approval](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Periode] [varchar](4) NULL,
	[RefId]  AS ((('MTR'+right([Periode],(2)))+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[PrincipalId] [int] NOT NULL,
	[DistributorId] [int] NOT NULL,
	[SubActivityType] [int] NULL,
	[ChannelId] [int] NULL,
	[Initiator] [varchar](50) NULL,
	[MinInvestment] [numeric](19, 0) NOT NULL,
	[MaxInvestment] [numeric](19, 0) NOT NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_MatrixBudgetAproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[PrincipalId] ASC,
	[DistributorId] ASC,
	[MinInvestment] ASC,
	[MaxInvestment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_matrix_budget_approver]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_budget_approver](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NOT NULL,
	[SeqApproval] [int] NOT NULL,
	[Approver] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_BudgetApprover] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[ParentId] ASC,
	[SeqApproval] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_matrix_dn_approval]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_dn_approval](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Periode] [varchar](4) NULL,
	[RefId]  AS ((('MTRDN'+right([Periode],(2)))+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[PrincipalId] [int] NOT NULL,
	[DistributorId] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_MatrixDNApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[PrincipalId] ASC,
	[DistributorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_matrix_dn_flow]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_dn_flow](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NOT NULL,
	[SeqApproval] [int] NOT NULL,
	[ProcessCode] [varchar](5) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_DNApprover] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[ParentId] ASC,
	[SeqApproval] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_matrix_dnmanual_assignment]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_dnmanual_assignment](
	[ChannelId] [int] NOT NULL,
	[UserId] [varchar](50) NOT NULL,
	[CreateOn] [datetime] NOT NULL,
	[CreateBy] [varchar](50) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_matrix_dnmanual_assignment_new]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_dnmanual_assignment_new](
	[ChannelId] [varchar](255) NULL,
	[SubChannelId] [varchar](255) NULL,
	[AccountId] [varchar](255) NULL,
	[SubAccountId] [varchar](255) NULL,
	[Pic1] [varchar](50) NULL,
	[Pic2] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](50) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_matrix_promo_approval]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_promo_approval](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Periode] [varchar](4) NULL,
	[RefId]  AS ((('MTR'+right([Periode],(2)))+'-')+right('00000'+CONVERT([varchar](5),[Id]),(5))) PERSISTED,
	[PrincipalId] [int] NOT NULL,
	[DistributorId] [int] NOT NULL,
	[SubActivityType] [int] NOT NULL,
	[ChannelId] [int] NOT NULL,
	[Initiator] [varchar](50) NOT NULL,
	[MinInvestment] [numeric](19, 0) NOT NULL,
	[MaxInvestment] [numeric](19, 0) NOT NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[SubChannelId] [int] NULL,
	[CreatedEmail] [varchar](100) NULL,
	[ModifiedEmail] [varchar](100) NULL,
	[DeleteEmail] [varchar](50) NULL,
	[CategoryId] [int] NULL,
 CONSTRAINT [PK_MatrixPromoAproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[PrincipalId] ASC,
	[SubActivityType] ASC,
	[ChannelId] ASC,
	[Initiator] ASC,
	[MinInvestment] ASC,
	[MaxInvestment] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_matrix_promo_approver]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_matrix_promo_approver](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NOT NULL,
	[SeqApproval] [int] NOT NULL,
	[Approver] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_PromoApprover] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[ParentId] ASC,
	[SeqApproval] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_menu]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_menu](
	[id] [varchar](10) NOT NULL,
	[parent] [varchar](10) NULL,
	[name] [varchar](50) NOT NULL,
	[icon] [varchar](50) NOT NULL,
	[url] [varchar](50) NOT NULL,
	[number] [int] NOT NULL,
	[flag] [varchar](10) NULL,
	[crud] [tinyint] NULL,
	[approve] [tinyint] NULL,
 CONSTRAINT [PK__tbset_me__3213E83F54ED4879] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_menu_v4]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_menu_v4](
	[id] [varchar](10) NOT NULL,
	[parent] [varchar](10) NULL,
	[name] [varchar](50) NOT NULL,
	[icon] [varchar](50) NOT NULL,
	[url] [varchar](50) NOT NULL,
	[slug] [varchar](100) NULL,
	[number] [int] NOT NULL,
	[flag] [varchar](10) NULL,
	[crud] [tinyint] NULL,
	[approve] [tinyint] NULL,
 CONSTRAINT [PK_tbset_menu_jummy] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_principal_distributor]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_principal_distributor](
	[PrincipalId] [int] NOT NULL,
	[DistributorId] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](36) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_pcp_distributor] PRIMARY KEY CLUSTERED 
(
	[PrincipalId] ASC,
	[DistributorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_promo_budget_approval]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_promo_budget_approval](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[LongDesc] [varchar](100) NULL,
	[Approver1] [varchar](50) NULL,
	[Approver2] [varchar](50) NULL,
	[Approver3] [varchar](50) NULL,
	[Approver4] [varchar](50) NULL,
	[Approver5] [varchar](50) NULL,
	[MinAmount] [float] NOT NULL,
	[MaxAmount] [float] NOT NULL,
	[CC1] [varchar](50) NULL,
	[CC2] [varchar](50) NULL,
	[CC3] [varchar](50) NULL,
	[CC4] [varchar](50) NULL,
	[CC5] [varchar](50) NULL,
	[category] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_promo_budget_approval] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_promo_mechanism_input_method]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_promo_mechanism_input_method](
	[subActivityId] [int] NOT NULL,
	[inputMethod] [bit] NULL,
 CONSTRAINT [PK_tbset_promo_mechanism_input_method] PRIMARY KEY CLUSTERED 
(
	[subActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_register]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_register](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Requester] [varchar](50) NULL,
	[Department] [varchar](50) NULL,
	[Requester_Status] [varchar](50) NULL,
	[Job_Title] [varchar](50) NULL,
	[Contact_Info] [varchar](50) NULL,
	[Requester_Description] [varchar](255) NULL,
	[approve] [int] NULL,
	[daterequest] [datetime] NULL,
	[approveon] [datetime] NULL,
	[approveby] [varchar](50) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK__tbset_re__3213E83F20284F49] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_roi_cr]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_roi_cr](
	[id] [int] NOT NULL,
	[SubActivityID] [varchar](max) NULL,
	[SubActivity] [varchar](max) NULL,
	[MinimumROI] [numeric](19, 2) NULL,
	[MaksimumCostRatio] [numeric](19, 2) NULL,
	[MaksimumROI] [numeric](19, 2) NULL,
	[MinimumCostRatio] [numeric](19, 2) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](1) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](1) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](1) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [tbset_roi_cr_pk] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_roi_cr_xls]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_roi_cr_xls](
	[Sub Activity ID] [varchar](max) NULL,
	[Sub Activity] [varchar](max) NULL,
	[Minimum ROI] [varchar](max) NULL,
	[Maksimum Cost Ratio] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_tools_promo_approval_reminder]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_tools_promo_approval_reminder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Dt1] [int] NOT NULL,
	[Dt2] [int] NOT NULL,
	[EOD] [bit] NOT NULL,
	[autorun] [bit] NOT NULL,
	[email] [nvarchar](max) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_tools_promo_approval_reminder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_tools_promo_approval_reminder_email]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_tools_promo_approval_reminder_email](
	[Id] [int] NOT NULL,
	[email] [varchar](50) NOT NULL,
	[username] [varchar](50) NULL,
	[usergroupname] [varchar](100) NOT NULL,
	[statusname] [varchar](10) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_tools_promo_approval_reminder_email] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_tools_promo_approval_reminder_email_send]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_tools_promo_approval_reminder_email_send](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[username] [varchar](50) NULL,
	[usergroupname] [varchar](100) NOT NULL,
	[statusname] [varchar](10) NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_tools_promo_approval_reminder_email_send] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_tscode_mapping]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_tscode_mapping](
	[ACTIVITY] [varchar](255) NULL,
	[TSCodeMap] [varchar](255) NULL,
	[id] [int] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_user]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_user](
	[id] [varchar](50) NOT NULL,
	[username] [varchar](50) NULL,
	[email] [varchar](50) NULL,
	[password] [varchar](100) NULL,
	[usergroupid] [varchar](15) NULL,
	[userlevel] [int] NULL,
	[department] [varchar](50) NULL,
	[jobtitle] [nvarchar](max) NULL,
	[contactinfo] [varchar](50) NULL,
	[distributorid] [varchar](36) NULL,
	[registered] [int] NULL,
	[code] [varchar](10) NULL,
	[password_change] [datetime] NULL,
	[token] [text] NULL,
	[token_date] [datetime] NULL,
	[userinput] [varchar](30) NULL,
	[dateinput] [datetime] NULL,
	[useredit] [varchar](30) NULL,
	[dateedit] [datetime] NULL,
	[isdeleted] [int] NULL,
	[deletedby] [varchar](50) NULL,
	[deletedon] [datetime] NULL,
	[lastLogin] [datetime] NULL,
	[isLogin] [int] NULL,
	[usernew] [int] NULL,
	[loginFailedCount] [int] NULL,
	[loginFailedLastTime] [datetime] NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK__tbset_us__3213E83F267ED509] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_user_channel_sstt]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_user_channel_sstt](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [varchar](36) NOT NULL,
	[channelId] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](36) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_user_channel_sstt] PRIMARY KEY CLUSTERED 
(
	[userId] ASC,
	[channelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_user_distributor]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_user_distributor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](36) NOT NULL,
	[DistributorId] [varchar](36) NOT NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbset_user_distributor] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[DistributorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_user_login]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_user_login](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[email] [varchar](50) NOT NULL,
	[password] [varchar](100) NOT NULL,
	[usergroupid] [varchar](15) NULL,
	[userlevel] [int] NULL,
	[department] [varchar](50) NULL,
	[jobtitle] [varchar](255) NULL,
	[contactinfo] [varchar](50) NULL,
	[distributorid] [varchar](36) NULL,
	[registered] [int] NOT NULL,
	[code] [varchar](10) NULL,
	[password_change] [datetime] NULL,
	[token] [text] NULL,
	[token_date] [datetime] NULL,
	[userinput] [varchar](30) NULL,
	[dateinput] [datetime] NULL,
	[useredit] [varchar](30) NULL,
	[dateedit] [datetime] NULL,
	[isdeleted] [int] NULL,
	[deletedby] [varchar](50) NULL,
	[deletedon] [datetime] NULL,
	[lastLogin] [datetime] NULL,
	[isLogin] [int] NULL,
	[usernew] [int] NULL,
	[loginFailedCount] [int] NULL,
	[loginFailedLastTime] [datetime] NULL,
	[pictureprofilefile] [varchar](50) NULL,
 CONSTRAINT [PK__tbset_user_login] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_user_login_profile]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_user_login_profile](
	[id] [int] NOT NULL,
	[profileid] [varchar](50) NOT NULL,
	[userinput] [varchar](30) NULL,
	[dateinput] [datetime] NULL,
	[useredit] [varchar](30) NULL,
	[dateedit] [datetime] NULL,
	[isdeleted] [int] NULL,
	[deletedby] [varchar](50) NULL,
	[deletedon] [datetime] NULL,
 CONSTRAINT [PK__tbset_user_login_profile] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[profileid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_user_principal]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_user_principal](
	[UserId] [varchar](36) NOT NULL,
	[PrincipalId] [varchar](36) NOT NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tblmst_userprincipal] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[PrincipalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_user_subaccount]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_user_subaccount](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](36) NOT NULL,
	[SubAccountId] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[CreateOn] [datetime] NULL,
	[CreateBy] [varchar](36) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](10) NULL,
	[IsDeleted] [bit] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [varchar](36) NULL,
	[IsDelete] [bit] NULL,
	[DeleteOn] [datetime] NULL,
	[DeleteBy] [varchar](36) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
	[accountid] [int] NULL,
	[channelid] [int] NULL,
	[subchannelid] [int] NULL,
 CONSTRAINT [PK_tbset_user_subaccount] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[SubAccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_usergroup]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_usergroup](
	[usergroupid] [varchar](15) NOT NULL,
	[usergroupname] [varchar](50) NOT NULL,
	[userinput] [varchar](30) NULL,
	[dateinput] [datetime2](0) NULL,
	[useredit] [varchar](30) NULL,
	[dateedit] [datetime2](0) NULL,
	[groupmenupermission] [int] NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK__tbset_us__0FF0818DC5270782] PRIMARY KEY CLUSTERED 
(
	[usergroupid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_userlevel]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_userlevel](
	[userlevel] [int] NOT NULL,
	[levelname] [varchar](30) NOT NULL,
	[usergroupid] [varchar](15) NULL,
	[userinput] [varchar](30) NULL,
	[dateinput] [datetime2](0) NULL,
	[useredit] [varchar](30) NULL,
	[dateedit] [datetime2](0) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK__tbset_us__92D55DAF5925AAC9] PRIMARY KEY CLUSTERED 
(
	[userlevel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_userlevel_access]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_userlevel_access](
	[userlevel] [int] NOT NULL,
	[usergroupid] [varchar](15) NOT NULL,
	[menuid] [varchar](10) NOT NULL,
	[create_rec] [smallint] NULL,
	[read_rec] [smallint] NULL,
	[update_rec] [smallint] NULL,
	[delete_rec] [smallint] NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK__tbset_us__4160AA7AC54DFE1E] PRIMARY KEY CLUSTERED 
(
	[userlevel] ASC,
	[usergroupid] ASC,
	[menuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_userrights]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_userrights](
	[usergroupid] [varchar](15) NOT NULL,
	[menuid] [varchar](10) NOT NULL,
	[create_rec] [int] NULL,
	[read_rec] [int] NULL,
	[update_rec] [int] NULL,
	[delete_rec] [int] NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK__tbset_us__DC457658C1AF93A6] PRIMARY KEY CLUSTERED 
(
	[usergroupid] ASC,
	[menuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[_temp_ApprovalPlanType]    Script Date: 10/6/2025 10:41:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_temp_ApprovalPlanType](
	[PromoPlanRefId] [varchar](50) NOT NULL,
	[TSCode] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[_tempPromo]    Script Date: 10/6/2025 10:41:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[_tempPromo](
	[PromoId] [int] NULL,
	[Periode] [int] NOT NULL,
	[EntityId] [int] NOT NULL,
	[DistributorId] [int] NOT NULL,
	[BudgetMasterId] [int] NULL,
	[CategoryId] [int] NULL,
	[SubCategoryId] [int] NULL,
	[ActivityId] [int] NULL,
	[SubActivityId] [int] NULL,
	[SubActivityTypeId] [varchar](255) NULL,
	[ActivityDesc] [varchar](255) NULL,
	[StartPromo] [date] NULL,
	[EndPromo] [date] NULL,
	[ChannelId] [int] NULL,
	[SubChannelId] [int] NULL,
	[AccountId] [int] NULL,
	[SubAccountId] [int] NULL,
	[GroupBrandId] [int] NULL,
	[Baseline] [float] NULL,
	[UpLift] [float] NULL,
	[TotalSales] [float] NULL,
	[SalesContribution] [float] NULL,
	[StoresCoverage] [float] NULL,
	[RedemptionRate] [float] NULL,
	[CR] [float] NULL,
	[Cost] [float] NULL,
	[StatusApproval] [varchar](10) NULL,
	[Notes] [varchar](255) NULL,
	[CreateOn] [date] NULL,
	[CreateBy] [varchar](50) NULL,
	[initiator_notes] [varchar](255) NULL,
	[CreatedEmail] [varchar](100) NULL,
	[ModifReason] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbset_mapping_pricecondition_temp]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbset_mapping_pricecondition_temp](
	[Category] [varchar](255) NULL,
	[Sub Category] [varchar](255) NULL,
	[Activity] [varchar](255) NULL,
	[PriceCondition (For payment)] [varchar](255) NULL,
	[PriceCondition (For accrual)] [varchar](255) NULL,
	[PriceCondition (For accrual reversal)] [varchar](255) NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[tbtemp_blitz]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbtemp_blitz](
	[Distributor_Id] [nvarchar](50) NULL,
	[Distributor_Name] [nvarchar](50) NULL,
	[Region_Code] [nvarchar](255) NULL,
	[Region_Desc] [nvarchar](255) NULL,
	[Year] [smallint] NULL,
	[Month_Name] [nvarchar](10) NULL,
	[AccountCode] [nvarchar](255) NULL,
	[AccountDesc] [nvarchar](255) NULL,
	[SubAccountCode] [nvarchar](255) NULL,
	[SubAccountDesc] [nvarchar](255) NULL,
	[Product_Code] [nvarchar](50) NULL,
	[Product_Name] [nvarchar](75) NULL,
	[Original SKU] [nvarchar](50) NULL,
	[Original SKU Desc] [nvarchar](75) NULL,
	[QTY IN TON] [decimal](38, 6) NULL,
	[QTY IN CAR] [decimal](38, 6) NULL,
	[SS DBP] [decimal](38, 12) NULL,
	[SS RBP] [decimal](38, 12) NULL,
	[SOURCE] [varchar](2) NOT NULL,
	[Created_At] [datetime2](3) NOT NULL,
	[Update_At] [datetime2](3) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbtemp_blitz_optima]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbtemp_blitz_optima](
	[Distributor_Id] [nvarchar](50) NULL,
	[Distributor_Name] [nvarchar](50) NULL,
	[Region_Code] [nvarchar](255) NULL,
	[Region_Desc] [nvarchar](255) NULL,
	[Year] [smallint] NULL,
	[Month_Name] [nvarchar](10) NULL,
	[AccountCode] [nvarchar](255) NULL,
	[AccountDesc] [nvarchar](255) NULL,
	[SubAccountCode] [nvarchar](255) NULL,
	[SubAccountDesc] [nvarchar](255) NULL,
	[Product_Code] [nvarchar](50) NULL,
	[Product_Name] [nvarchar](75) NULL,
	[Original SKU] [nvarchar](50) NULL,
	[Original SKU Desc] [nvarchar](75) NULL,
	[QTY IN TON] [decimal](38, 6) NULL,
	[QTY IN CAR] [decimal](38, 6) NULL,
	[SS DBP] [decimal](38, 12) NULL,
	[SS RBP] [decimal](38, 12) NULL,
	[SOURCE] [varchar](2) NOT NULL,
	[Created_At] [datetime2](3) NOT NULL,
	[Update_At] [datetime2](3) NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbtemp_blitz_optima_map_check]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbtemp_blitz_optima_map_check](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[type] [varchar](50) NULL,
	[code] [nvarchar](255) NULL,
	[desc] [nvarchar](255) NULL,
	[created_on] [date] NULL,
	[notif_status] [varchar](50) NULL,
	[notified_on] [date] NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL,
 CONSTRAINT [PK_tbtemp_blitz_optima_map_check] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbtemp_DebetNoteFPType]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbtemp_DebetNoteFPType](
	[Type] [varchar](50) NULL,
	[RefId] [varchar](255) NULL,
	[FPNumber] [varchar](50) NULL,
	[FPDate] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbtemp_kpi_scoring]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbtemp_kpi_scoring](
	[DistributorId] [int] NOT NULL,
	[DistributorShortDesc] [varchar](50) NULL,
	[EntityId] [int] NULL,
	[PrincipalShortDesc] [varchar](50) NULL,
	[PromoPlanCreateBy] [varchar](36) NOT NULL,
	[PromoPlanCreateOn] [datetime] NOT NULL,
	[RegionDesc] [nvarchar](max) NULL,
	[SubActivityTypeId] [int] NOT NULL,
	[SubActivityType] [varchar](50) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Category] [varchar](50) NOT NULL,
	[SubCategoryId] [int] NOT NULL,
	[SubCategory] [varchar](50) NOT NULL,
	[ActivityId] [int] NOT NULL,
	[Activity] [varchar](50) NOT NULL,
	[SubActivityId] [int] NOT NULL,
	[SubActivity] [varchar](50) NOT NULL,
	[StartPlanning] [date] NOT NULL,
	[EndPlanning] [date] NOT NULL,
	[ChannelDesc] [nvarchar](max) NULL,
	[SubChannelDesc] [nvarchar](max) NULL,
	[AccountDesc] [nvarchar](max) NULL,
	[SubAccountDesc] [nvarchar](max) NULL,
	[TSCode] [varchar](50) NULL,
	[BrandDesc] [nvarchar](max) NULL,
	[SKUDesc] [nvarchar](max) NULL,
	[SourceOfPromo] [varchar](1) NOT NULL,
	[PromotionType] [varchar](1) NOT NULL,
	[ActivityDesc] [varchar](255) NULL,
	[Mechanisme1] [varchar](255) NULL,
	[Mechanisme2] [varchar](255) NULL,
	[Mechanisme3] [varchar](255) NULL,
	[Mechanisme4] [varchar](255) NULL,
	[Investment] [numeric](19, 2) NOT NULL,
	[BaselineSales] [numeric](19, 2) NOT NULL,
	[UpliftSales] [numeric](19, 2) NOT NULL,
	[TotalSales] [numeric](20, 2) NULL,
	[UpliftSalesPersen] [numeric](38, 19) NULL,
	[ROIPersen] [numeric](38, 18) NULL,
	[PromoId] [int] NULL,
	[PromoRefId] [varchar](50) NULL,
	[Creator] [varchar](50) NULL,
	[CreationDate] [datetime] NULL,
	[PeriodStart] [date] NULL,
	[PeriodEnd] [date] NULL,
	[PromoAmount] [numeric](19, 2) NOT NULL,
	[today] [datetime] NOT NULL,
	[Quater2Start] [varchar](1) NOT NULL,
	[today60] [datetime] NULL,
	[DaysOfCreation] [int] NULL,
	[PeriodStartMatching] [int] NULL,
	[PeriodEndMatcing] [int] NULL,
	[Amount] [numeric](20, 2) NULL,
	[PeriodOfStart] [int] NULL,
	[PeriodOfEnd] [int] NULL,
	[PromoPlanSubmissionDays] [int] NULL,
	[PromoPlanSubmittedBfrQuarterStart90] [varchar](1) NOT NULL,
	[AvailabilityInOptima] [varchar](1) NOT NULL,
	[OptimaCreationBrfActivityStart60] [varchar](1) NOT NULL,
	[OptimaPeriodMatch] [varchar](1) NOT NULL,
	[OptimaAmountMatch] [varchar](1) NOT NULL,
	[OptimaDescMatch] [varchar](1) NOT NULL,
	[OptimaMechanismMatch] [varchar](1) NOT NULL,
	[OptimaDescAndMechanismMatch] [varchar](1) NOT NULL,
	[SKPAvailability] [varchar](1) NOT NULL,
	[SKPAvailBfrActivityStart60] [varchar](1) NOT NULL,
	[SKPPeriodMatch] [varchar](1) NOT NULL,
	[SKPAmountMatch] [varchar](1) NOT NULL,
	[SKPMechanismMatch] [varchar](1) NOT NULL,
	[SKPSigned7] [varchar](1) NOT NULL,
	[ActivityAuditCompliance] [varchar](1) NOT NULL,
	[ExtendedPeriodMatch] [varchar](1) NOT NULL,
	[ExtendedAmountMatch] [varchar](1) NOT NULL,
	[ExtendedMechanismMatch] [varchar](1) NOT NULL,
	[ReconNKA] [varchar](1) NOT NULL,
	[AdjustBudgetRecon] [varchar](1) NOT NULL,
	[FeedbackOnPostROI] [varchar](1) NOT NULL,
	[AgingDNBySales] [varchar](1) NOT NULL,
	[JobTitle1] [varchar](255) NULL,
	[JobTitle2] [varchar](255) NULL,
	[PromoPlanID] [int] NOT NULL,
	[PromoPlanRefID] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbtemp_promo_submission_exception]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbtemp_promo_submission_exception](
	[Idx] [varchar](200) NOT NULL,
	[PromoId] [int] NOT NULL,
	[Reason] [varchar](255) NULL,
	[ActionOn] [datetime] NULL,
	[ActionBy] [varchar](36) NULL,
	[ActionEmail] [varchar](100) NULL,
 CONSTRAINT [PK_tbtemp_promo_submission_exception] PRIMARY KEY CLUSTERED 
(
	[Idx] ASC,
	[PromoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbtemp_tbtrx_blitz]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbtemp_tbtrx_blitz](
	[Distributor_Id] [int] NULL,
	[Distributor_Name] [varchar](100) NULL,
	[Region_Code] [int] NULL,
	[Region_Desc] [varchar](100) NULL,
	[Year] [int] NULL,
	[Month_Name] [varchar](15) NULL,
	[AccountCode] [varchar](15) NULL,
	[AccountDesc] [varchar](100) NULL,
	[SubAccountCode] [varchar](20) NULL,
	[SubAccountDesc] [varchar](100) NULL,
	[Product_Code] [varchar](20) NULL,
	[Product_Name] [varchar](100) NULL,
	[Qty_In_Car] [numeric](18, 6) NULL,
	[SS_DBP] [numeric](18, 6) NULL,
	[SS_RBP] [numeric](18, 6) NULL,
	[SOURCE] [varchar](15) NULL,
	[Month_Int] [int] NOT NULL,
	[Created_At] [datetime] NULL,
	[Updated_At] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbtemp_validate_array]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbtemp_validate_array](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[keyid] [int] NULL,
	[action] [datetime] NULL,
	[actionby] [varchar](50) NULL,
	[status] [varchar](50) NULL,
 CONSTRAINT [PK_Temp_Validate_Array] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[temp_ApprovalPlanType]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[temp_ApprovalPlanType](
	[PromoPlanRefId] [varchar](50) NOT NULL,
	[TSCode] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[temp_investment]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[temp_investment](
	[Entity] [varchar](50) NULL,
	[EntityShortDesc] [varchar](50) NULL,
	[Distributor] [varchar](50) NULL,
	[BudgetAllocationName] [varchar](255) NOT NULL,
	[ActivityType] [varchar](50) NOT NULL,
	[IsLastLayer] [int] NOT NULL,
	[Channel] [varchar](8000) NOT NULL,
	[BudgetDeployed] [numeric](19, 2) NOT NULL,
	[PromoCreated] [numeric](38, 2) NOT NULL,
	[DNClaimed] [numeric](38, 0) NOT NULL,
	[DNPaid] [numeric](38, 0) NOT NULL,
	[ReturnBalanceFromPromo] [numeric](38, 0) NOT NULL,
	[RemainingBudget] [numeric](20, 0) NULL,
	[GapBudgetDeployedvsPromoCreated] [numeric](38, 2) NULL,
	[GapPromoCreatedvsDNClaimed] [numeric](38, 0) NULL,
	[GapDNClaimedvsDNPaid] [numeric](38, 0) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[temp_tbset_user]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[temp_tbset_user](
	[id] [varchar](max) NULL,
	[password] [varchar](max) NULL,
	[password_change] [datetime] NOT NULL,
	[CreatedEmail] [varchar](50) NULL,
	[ModifiedEmail] [varchar](50) NULL,
	[DeleteEmail] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempAccount4]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempAccount4](
	[ParentId] [int] NULL,
	[Id] [varchar](50) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempActivityType]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempActivityType](
	[Category] [varchar](50) NULL,
	[SubCategory] [varchar](50) NULL,
	[Activity] [varchar](50) NULL,
	[SubActivity] [varchar](50) NULL,
	[SubActivityType] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempBrand4]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempBrand4](
	[ParentId] [int] NULL,
	[Id] [varchar](50) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempBudgetAssignmentDetailUpdateType]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempBudgetAssignmentDetailUpdateType](
	[Id] [int] NOT NULL,
	[OwnId] [varchar](50) NOT NULL,
	[Desc] [varchar](255) NOT NULL,
	[BudgetAmount] [numeric](19, 0) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tempcekbudget]    Script Date: 10/6/2025 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tempcekbudget](
	[Budget] [varchar](255) NULL,
	[Remain] [numeric](19, 0) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempChannel4]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempChannel4](
	[ParentId] [int] NULL,
	[Id] [varchar](50) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempDCBudgetUploadType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempDCBudgetUploadType](
	[BudgetYear] [varchar](50) NULL,
	[Category] [varchar](255) NULL,
	[Distributor] [varchar](255) NULL,
	[Brand] [varchar](255) NULL,
	[SubActivity] [varchar](255) NULL,
	[BudgetAmount] [numeric](18, 0) NULL,
	[BudgetApproval] [varchar](255) NULL,
	[UserAccess1] [varchar](255) NULL,
	[UserAccess2] [varchar](255) NULL,
	[UserAccess3] [varchar](255) NULL,
	[UserAccess4] [varchar](255) NULL,
	[UserAccess5] [varchar](255) NULL,
	[UserAccess6] [varchar](255) NULL,
	[UserAccess7] [varchar](255) NULL,
	[UserAccess8] [varchar](255) NULL,
	[UserAccess9] [varchar](255) NULL,
	[UserAccess10] [varchar](255) NULL,
	[UserAccess11] [varchar](255) NULL,
	[UserAccess12] [varchar](255) NULL,
	[UserAccess13] [varchar](255) NULL,
	[UserAccess14] [varchar](255) NULL,
	[UserAccess15] [varchar](255) NULL,
	[UserAccess16] [varchar](255) NULL,
	[UserAccess17] [varchar](255) NULL,
	[UserAccess18] [varchar](255) NULL,
	[UserAccess19] [varchar](255) NULL,
	[UserAccess20] [varchar](255) NULL,
	[UserAccess21] [varchar](255) NULL,
	[UserAccess22] [varchar](255) NULL,
	[UserAccess23] [varchar](255) NULL,
	[UserAccess24] [varchar](255) NULL,
	[UserAccess25] [varchar](255) NULL,
	[UserAccess26] [varchar](255) NULL,
	[UserAccess27] [varchar](255) NULL,
	[UserAccess28] [varchar](255) NULL,
	[UserAccess29] [varchar](255) NULL,
	[UserAccess30] [varchar](255) NULL,
	[UserAccess31] [varchar](255) NULL,
	[UserAccess32] [varchar](255) NULL,
	[UserAccess33] [varchar](255) NULL,
	[UserAccess34] [varchar](255) NULL,
	[UserAccess35] [varchar](255) NULL,
	[ProfileId] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempDebetnoteFailed]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempDebetnoteFailed](
	[Periode] [varchar](50) NULL,
	[Entity] [varchar](255) NULL,
	[Distributor] [varchar](255) NULL,
	[Account] [varchar](255) NULL,
	[Activity] [varchar](255) NULL,
	[PromoNumber] [varchar](255) NULL,
	[InternalDocNumber] [varchar](255) NULL,
	[TotalClaim] [varchar](255) NULL,
	[DueDate] [varchar](50) NULL,
	[FeeDesc] [varchar](255) NULL,
	[FeeAmount] [varchar](255) NULL,
	[DNType] [varchar](255) NULL,
	[statusPPN] [varchar](30) NULL,
	[PPNPct] [varchar](255) NULL,
	[PPNAmt] [varchar](255) NULL,
	[statusPPH] [varchar](30) NULL,
	[PPHPct] [varchar](255) NULL,
	[PPHAmt] [varchar](255) NULL,
	[FPNumber] [varchar](50) NULL,
	[FPDate] [varchar](50) NULL,
	[TaxLevel] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempDebetnoteType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempDebetnoteType](
	[Periode] [varchar](50) NULL,
	[Entity] [varchar](255) NULL,
	[Distributor] [varchar](255) NULL,
	[Account] [varchar](255) NULL,
	[Activity] [varchar](255) NULL,
	[PromoNumber] [varchar](255) NULL,
	[InternalDocNumber] [varchar](255) NULL,
	[TotalClaim] [varchar](255) NULL,
	[DueDate] [varchar](50) NULL,
	[FeeDesc] [varchar](255) NULL,
	[FeeAmount] [varchar](255) NULL,
	[DNType] [varchar](255) NULL,
	[statusPPN] [varchar](30) NULL,
	[PPNPct] [varchar](255) NULL,
	[PPNAmt] [varchar](255) NULL,
	[statusPPH] [varchar](30) NULL,
	[PPHPct] [varchar](255) NULL,
	[PPHAmt] [varchar](255) NULL,
	[FPNumber] [varchar](50) NULL,
	[FPDate] [varchar](50) NULL,
	[TaxLevel] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempDebetnoteType_tes]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempDebetnoteType_tes](
	[Periode] [varchar](50) NULL,
	[Entity] [varchar](255) NULL,
	[Distributor] [varchar](255) NULL,
	[Account] [varchar](255) NULL,
	[Activity] [varchar](255) NULL,
	[PromoNumber] [varchar](255) NULL,
	[InternalDocNumber] [varchar](255) NULL,
	[TotalClaim] [varchar](255) NULL,
	[DueDate] [varchar](50) NULL,
	[FeeDesc] [varchar](255) NULL,
	[FeeAmount] [varchar](255) NULL,
	[DNType] [varchar](255) NULL,
	[statusPPN] [varchar](30) NULL,
	[PPNPct] [varchar](255) NULL,
	[PPNAmt] [varchar](255) NULL,
	[statusPPH] [varchar](30) NULL,
	[PPHPct] [varchar](255) NULL,
	[PPHAmt] [varchar](255) NULL,
	[FPNumber] [varchar](50) NULL,
	[FPDate] [varchar](50) NULL,
	[TaxLevel] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempDebetNoteTypeProd]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempDebetNoteTypeProd](
	[Periode] [varchar](50) NULL,
	[Entity] [varchar](255) NULL,
	[Distributor] [varchar](255) NULL,
	[Account] [varchar](255) NULL,
	[Activity] [varchar](255) NULL,
	[PromoNumber] [varchar](255) NULL,
	[InternalDocNumber] [varchar](255) NULL,
	[TotalClaim] [varchar](255) NULL,
	[DueDate] [varchar](50) NULL,
	[FeeDesc] [varchar](255) NULL,
	[FeeAmount] [varchar](255) NULL,
	[DNType] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempFailedRec]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempFailedRec](
	[Activity] [varchar](255) NULL,
	[TotalClaim] [varchar](255) NULL,
	[PromoNumber] [varchar](255) NULL,
	[InternalDocNumber] [varchar](255) NULL,
	[Entity] [varchar](255) NULL,
	[Distributor] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempGetbaseline]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempGetbaseline](
	[p_promoid] [int] NULL,
	[p_period] [int] NULL,
	[p_date] [date] NULL,
	[p_type] [int] NULL,
	[p_distributor] [int] NULL,
	[p_subcategory] [int] NULL,
	[p_startpromo] [date] NULL,
	[p_endpromo] [date] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportAccountType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportAccountType](
	[Allocation] [varchar](255) NULL,
	[Derivative] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL,
	[21] [varchar](50) NULL,
	[22] [varchar](50) NULL,
	[23] [varchar](50) NULL,
	[24] [varchar](50) NULL,
	[25] [varchar](50) NULL,
	[26] [varchar](50) NULL,
	[27] [varchar](50) NULL,
	[28] [varchar](50) NULL,
	[29] [varchar](50) NULL,
	[30] [varchar](50) NULL,
	[31] [varchar](50) NULL,
	[32] [varchar](50) NULL,
	[33] [varchar](50) NULL,
	[34] [varchar](50) NULL,
	[35] [varchar](50) NULL,
	[36] [varchar](50) NULL,
	[37] [varchar](50) NULL,
	[38] [varchar](50) NULL,
	[39] [varchar](50) NULL,
	[40] [varchar](50) NULL,
	[41] [varchar](50) NULL,
	[42] [varchar](50) NULL,
	[43] [varchar](50) NULL,
	[44] [varchar](50) NULL,
	[45] [varchar](50) NULL,
	[46] [varchar](50) NULL,
	[47] [varchar](50) NULL,
	[48] [varchar](50) NULL,
	[49] [varchar](50) NULL,
	[50] [varchar](50) NULL,
	[51] [varchar](50) NULL,
	[52] [varchar](50) NULL,
	[53] [varchar](50) NULL,
	[54] [varchar](50) NULL,
	[55] [varchar](50) NULL,
	[56] [varchar](50) NULL,
	[57] [varchar](50) NULL,
	[58] [varchar](50) NULL,
	[59] [varchar](50) NULL,
	[60] [varchar](50) NULL,
	[61] [varchar](50) NULL,
	[62] [varchar](50) NULL,
	[63] [varchar](50) NULL,
	[64] [varchar](50) NULL,
	[65] [varchar](50) NULL,
	[66] [varchar](50) NULL,
	[67] [varchar](50) NULL,
	[68] [varchar](50) NULL,
	[69] [varchar](50) NULL,
	[70] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportAllocationType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportAllocationType](
	[Periode] [varchar](4) NULL,
	[BudgetDesc] [varchar](255) NULL,
	[Owner] [varchar](50) NULL,
	[Distributor] [varchar](255) NULL,
	[Principal] [varchar](255) NULL,
	[Category] [varchar](255) NULL,
	[SubCategory] [varchar](255) NULL,
	[Activity] [varchar](255) NULL,
	[SubActivity] [varchar](255) NULL,
	[BudgetAmount] [numeric](19, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportAllocationUserType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportAllocationUserType](
	[Allocation] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL,
	[21] [varchar](50) NULL,
	[22] [varchar](50) NULL,
	[23] [varchar](50) NULL,
	[24] [varchar](50) NULL,
	[25] [varchar](50) NULL,
	[26] [varchar](50) NULL,
	[27] [varchar](50) NULL,
	[28] [varchar](50) NULL,
	[29] [varchar](50) NULL,
	[30] [varchar](50) NULL,
	[31] [varchar](50) NULL,
	[32] [varchar](50) NULL,
	[33] [varchar](50) NULL,
	[34] [varchar](50) NULL,
	[35] [varchar](50) NULL,
	[36] [varchar](50) NULL,
	[37] [varchar](50) NULL,
	[38] [varchar](50) NULL,
	[39] [varchar](50) NULL,
	[40] [varchar](50) NULL,
	[41] [varchar](50) NULL,
	[42] [varchar](50) NULL,
	[43] [varchar](50) NULL,
	[44] [varchar](50) NULL,
	[45] [varchar](50) NULL,
	[46] [varchar](50) NULL,
	[47] [varchar](50) NULL,
	[48] [varchar](50) NULL,
	[49] [varchar](50) NULL,
	[50] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportBrandType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportBrandType](
	[Allocation] [varchar](255) NULL,
	[Derivative] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL,
	[21] [varchar](50) NULL,
	[22] [varchar](50) NULL,
	[23] [varchar](50) NULL,
	[24] [varchar](50) NULL,
	[25] [varchar](50) NULL,
	[26] [varchar](50) NULL,
	[27] [varchar](50) NULL,
	[28] [varchar](50) NULL,
	[29] [varchar](50) NULL,
	[30] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportDerivativeType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportDerivativeType](
	[BudgetParent] [varchar](255) NULL,
	[TotalAssignmentAmount] [numeric](19, 2) NULL,
	[AssignTo] [varchar](255) NULL,
	[AssignDesc] [varchar](255) NULL,
	[AssignAmount] [numeric](19, 2) NULL,
	[Category] [varchar](255) NULL,
	[SubCategory] [varchar](255) NULL,
	[Activity] [varchar](255) NULL,
	[SubActivity] [varchar](255) NULL,
	[approve] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportDerivativeUserType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportDerivativeUserType](
	[Derivative] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL,
	[21] [varchar](50) NULL,
	[22] [varchar](50) NULL,
	[23] [varchar](50) NULL,
	[24] [varchar](50) NULL,
	[25] [varchar](50) NULL,
	[26] [varchar](50) NULL,
	[27] [varchar](50) NULL,
	[28] [varchar](50) NULL,
	[29] [varchar](50) NULL,
	[30] [varchar](50) NULL,
	[31] [varchar](50) NULL,
	[32] [varchar](50) NULL,
	[33] [varchar](50) NULL,
	[34] [varchar](50) NULL,
	[35] [varchar](50) NULL,
	[36] [varchar](50) NULL,
	[37] [varchar](50) NULL,
	[38] [varchar](50) NULL,
	[39] [varchar](50) NULL,
	[40] [varchar](50) NULL,
	[41] [varchar](50) NULL,
	[42] [varchar](50) NULL,
	[43] [varchar](50) NULL,
	[44] [varchar](50) NULL,
	[45] [varchar](50) NULL,
	[46] [varchar](50) NULL,
	[47] [varchar](50) NULL,
	[48] [varchar](50) NULL,
	[49] [varchar](50) NULL,
	[50] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportPromoPlanAccountType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportPromoPlanAccountType](
	[Activity] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL,
	[21] [varchar](50) NULL,
	[22] [varchar](50) NULL,
	[23] [varchar](50) NULL,
	[24] [varchar](50) NULL,
	[25] [varchar](50) NULL,
	[26] [varchar](50) NULL,
	[27] [varchar](50) NULL,
	[28] [varchar](50) NULL,
	[29] [varchar](50) NULL,
	[30] [varchar](50) NULL,
	[31] [varchar](50) NULL,
	[32] [varchar](50) NULL,
	[33] [varchar](50) NULL,
	[34] [varchar](50) NULL,
	[35] [varchar](50) NULL,
	[36] [varchar](50) NULL,
	[37] [varchar](50) NULL,
	[38] [varchar](50) NULL,
	[39] [varchar](50) NULL,
	[40] [varchar](50) NULL,
	[41] [varchar](50) NULL,
	[42] [varchar](50) NULL,
	[43] [varchar](50) NULL,
	[44] [varchar](50) NULL,
	[45] [varchar](50) NULL,
	[46] [varchar](50) NULL,
	[47] [varchar](50) NULL,
	[48] [varchar](50) NULL,
	[49] [varchar](50) NULL,
	[50] [varchar](50) NULL,
	[51] [varchar](50) NULL,
	[52] [varchar](50) NULL,
	[53] [varchar](50) NULL,
	[54] [varchar](50) NULL,
	[55] [varchar](50) NULL,
	[56] [varchar](50) NULL,
	[57] [varchar](50) NULL,
	[58] [varchar](50) NULL,
	[59] [varchar](50) NULL,
	[60] [varchar](50) NULL,
	[61] [varchar](50) NULL,
	[62] [varchar](50) NULL,
	[63] [varchar](50) NULL,
	[64] [varchar](50) NULL,
	[65] [varchar](50) NULL,
	[66] [varchar](50) NULL,
	[67] [varchar](50) NULL,
	[68] [varchar](50) NULL,
	[69] [varchar](50) NULL,
	[70] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportPromoPlanBrandType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportPromoPlanBrandType](
	[ImportId] [varchar](255) NOT NULL,
	[Id] [int] NOT NULL,
	[Activity] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL,
	[21] [varchar](50) NULL,
	[22] [varchar](50) NULL,
	[23] [varchar](50) NULL,
	[24] [varchar](50) NULL,
	[25] [varchar](50) NULL,
	[26] [varchar](50) NULL,
	[27] [varchar](50) NULL,
	[28] [varchar](50) NULL,
	[29] [varchar](50) NULL,
	[30] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportPromoPlanRegionType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportPromoPlanRegionType](
	[ImportId] [varchar](255) NOT NULL,
	[Id] [int] NOT NULL,
	[Activity] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportPromoPlanSkuType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportPromoPlanSkuType](
	[ImportId] [varchar](255) NOT NULL,
	[Id] [int] NOT NULL,
	[Activity] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL,
	[21] [varchar](50) NULL,
	[22] [varchar](50) NULL,
	[23] [varchar](50) NULL,
	[24] [varchar](50) NULL,
	[25] [varchar](50) NULL,
	[26] [varchar](50) NULL,
	[27] [varchar](50) NULL,
	[28] [varchar](50) NULL,
	[29] [varchar](50) NULL,
	[30] [varchar](50) NULL,
	[31] [varchar](50) NULL,
	[32] [varchar](50) NULL,
	[33] [varchar](50) NULL,
	[34] [varchar](50) NULL,
	[35] [varchar](50) NULL,
	[36] [varchar](50) NULL,
	[37] [varchar](50) NULL,
	[38] [varchar](50) NULL,
	[39] [varchar](50) NULL,
	[40] [varchar](50) NULL,
	[41] [varchar](50) NULL,
	[42] [varchar](50) NULL,
	[43] [varchar](50) NULL,
	[44] [varchar](50) NULL,
	[45] [varchar](50) NULL,
	[46] [varchar](50) NULL,
	[47] [varchar](50) NULL,
	[48] [varchar](50) NULL,
	[49] [varchar](50) NULL,
	[50] [varchar](50) NULL,
	[51] [varchar](50) NULL,
	[52] [varchar](50) NULL,
	[53] [varchar](50) NULL,
	[54] [varchar](50) NULL,
	[55] [varchar](50) NULL,
	[56] [varchar](50) NULL,
	[57] [varchar](50) NULL,
	[58] [varchar](50) NULL,
	[59] [varchar](50) NULL,
	[60] [varchar](50) NULL,
	[61] [varchar](50) NULL,
	[62] [varchar](50) NULL,
	[63] [varchar](50) NULL,
	[64] [varchar](50) NULL,
	[65] [varchar](50) NULL,
	[66] [varchar](50) NULL,
	[67] [varchar](50) NULL,
	[68] [varchar](50) NULL,
	[69] [varchar](50) NULL,
	[70] [varchar](50) NULL,
	[71] [varchar](50) NULL,
	[72] [varchar](50) NULL,
	[73] [varchar](50) NULL,
	[74] [varchar](50) NULL,
	[75] [varchar](50) NULL,
	[76] [varchar](50) NULL,
	[77] [varchar](50) NULL,
	[78] [varchar](50) NULL,
	[79] [varchar](50) NULL,
	[80] [varchar](50) NULL,
	[81] [varchar](50) NULL,
	[82] [varchar](50) NULL,
	[83] [varchar](50) NULL,
	[84] [varchar](50) NULL,
	[85] [varchar](50) NULL,
	[86] [varchar](50) NULL,
	[87] [varchar](50) NULL,
	[88] [varchar](50) NULL,
	[89] [varchar](50) NULL,
	[90] [varchar](50) NULL,
	[91] [varchar](50) NULL,
	[92] [varchar](50) NULL,
	[93] [varchar](50) NULL,
	[94] [varchar](50) NULL,
	[95] [varchar](50) NULL,
	[96] [varchar](50) NULL,
	[97] [varchar](50) NULL,
	[98] [varchar](50) NULL,
	[99] [varchar](50) NULL,
	[100] [varchar](50) NULL,
	[101] [varchar](50) NULL,
	[102] [varchar](50) NULL,
	[103] [varchar](50) NULL,
	[104] [varchar](50) NULL,
	[105] [varchar](50) NULL,
	[106] [varchar](50) NULL,
	[107] [varchar](50) NULL,
	[108] [varchar](50) NULL,
	[109] [varchar](50) NULL,
	[110] [varchar](50) NULL,
	[111] [varchar](50) NULL,
	[112] [varchar](50) NULL,
	[113] [varchar](50) NULL,
	[114] [varchar](50) NULL,
	[115] [varchar](50) NULL,
	[116] [varchar](50) NULL,
	[117] [varchar](50) NULL,
	[118] [varchar](50) NULL,
	[119] [varchar](50) NULL,
	[120] [varchar](50) NULL,
	[121] [varchar](50) NULL,
	[122] [varchar](50) NULL,
	[123] [varchar](50) NULL,
	[124] [varchar](50) NULL,
	[125] [varchar](50) NULL,
	[126] [varchar](50) NULL,
	[127] [varchar](50) NULL,
	[128] [varchar](50) NULL,
	[129] [varchar](50) NULL,
	[130] [varchar](50) NULL,
	[131] [varchar](50) NULL,
	[132] [varchar](50) NULL,
	[133] [varchar](50) NULL,
	[134] [varchar](50) NULL,
	[135] [varchar](50) NULL,
	[136] [varchar](50) NULL,
	[137] [varchar](50) NULL,
	[138] [varchar](50) NULL,
	[139] [varchar](50) NULL,
	[140] [varchar](50) NULL,
	[141] [varchar](50) NULL,
	[142] [varchar](50) NULL,
	[143] [varchar](50) NULL,
	[144] [varchar](50) NULL,
	[145] [varchar](50) NULL,
	[146] [varchar](50) NULL,
	[147] [varchar](50) NULL,
	[148] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportRegionType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportRegionType](
	[Allocation] [varchar](255) NULL,
	[Derivative] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempImportUserType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempImportUserType](
	[Allocation] [varchar](255) NULL,
	[Derivative] [varchar](255) NULL,
	[1] [varchar](50) NULL,
	[2] [varchar](50) NULL,
	[3] [varchar](50) NULL,
	[4] [varchar](50) NULL,
	[5] [varchar](50) NULL,
	[6] [varchar](50) NULL,
	[7] [varchar](50) NULL,
	[8] [varchar](50) NULL,
	[9] [varchar](50) NULL,
	[10] [varchar](50) NULL,
	[11] [varchar](50) NULL,
	[12] [varchar](50) NULL,
	[13] [varchar](50) NULL,
	[14] [varchar](50) NULL,
	[15] [varchar](50) NULL,
	[16] [varchar](50) NULL,
	[17] [varchar](50) NULL,
	[18] [varchar](50) NULL,
	[19] [varchar](50) NULL,
	[20] [varchar](50) NULL,
	[21] [varchar](50) NULL,
	[22] [varchar](50) NULL,
	[23] [varchar](50) NULL,
	[24] [varchar](50) NULL,
	[25] [varchar](50) NULL,
	[26] [varchar](50) NULL,
	[27] [varchar](50) NULL,
	[28] [varchar](50) NULL,
	[29] [varchar](50) NULL,
	[30] [varchar](50) NULL,
	[31] [varchar](50) NULL,
	[32] [varchar](50) NULL,
	[33] [varchar](50) NULL,
	[34] [varchar](50) NULL,
	[35] [varchar](50) NULL,
	[36] [varchar](50) NULL,
	[37] [varchar](50) NULL,
	[38] [varchar](50) NULL,
	[39] [varchar](50) NULL,
	[40] [varchar](50) NULL,
	[41] [varchar](50) NULL,
	[42] [varchar](50) NULL,
	[43] [varchar](50) NULL,
	[44] [varchar](50) NULL,
	[45] [varchar](50) NULL,
	[46] [varchar](50) NULL,
	[47] [varchar](50) NULL,
	[48] [varchar](50) NULL,
	[49] [varchar](50) NULL,
	[50] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tempmap_promorecon_period_subactivity]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tempmap_promorecon_period_subactivity](
	[Sub Activity ID] [varchar](max) NULL,
	[Category] [varchar](max) NULL,
	[Sub Category] [varchar](max) NULL,
	[Activity] [varchar](max) NULL,
	[Type] [varchar](max) NULL,
	[Long Desc] [varchar](max) NULL,
	[Action] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempMapDistributorAccount]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempMapDistributorAccount](
	[Channel] [varchar](255) NULL,
	[SubChannel] [varchar](255) NULL,
	[Account] [varchar](255) NULL,
	[SubAccount] [varchar](255) NULL,
	[Dist1] [varchar](255) NULL,
	[Dist2] [varchar](255) NULL,
	[Dist3] [varchar](255) NULL,
	[Dist4] [varchar](255) NULL,
	[Dist5] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempMatrixDnManualType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempMatrixDnManualType](
	[Channel] [varchar](255) NULL,
	[SubChannel] [varchar](255) NULL,
	[Account] [varchar](255) NULL,
	[SubAccount] [varchar](255) NULL,
	[Pic1] [varchar](50) NULL,
	[Pic2] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempMechanism4]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempMechanism4](
	[Id] [int] NULL,
	[Mechanism] [varchar](max) NULL,
	[Notes] [varchar](255) NULL,
	[ProductId] [int] NULL,
	[Product] [varchar](255) NULL,
	[BrandId] [int] NULL,
	[Brand] [varchar](255) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tempMechanismType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tempMechanismType](
	[Id] [int] NULL,
	[Mechanism] [varchar](255) NULL,
	[Notes] [varchar](255) NULL,
	[ProductId] [int] NULL,
	[Product] [varchar](255) NULL,
	[BrandId] [int] NULL,
	[Brand] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempMstMechanismType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempMstMechanismType](
	[Entity] [varchar](max) NULL,
	[SubCategory] [varchar](max) NULL,
	[Activity] [varchar](max) NULL,
	[SubActivity] [varchar](max) NULL,
	[SKU] [varchar](max) NULL,
	[Requirement] [varchar](max) NULL,
	[Discount] [varchar](max) NULL,
	[Mechanism] [varchar](max) NULL,
	[Channel] [varchar](max) NULL,
	[StartPromo] [varchar](max) NULL,
	[EndPromo] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPromoApproval]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPromoApproval](
	[promoid] [int] NULL,
	[statuscode] [varchar](3) NULL,
	[notes] [varchar](255) NULL,
	[approvaldate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPromoApproverType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPromoApproverType](
	[Periode] [varchar](4) NULL,
	[PrincipalId] [varchar](255) NOT NULL,
	[DistributorId] [varchar](255) NOT NULL,
	[Category] [varchar](255) NOT NULL,
	[SubActivityType] [varchar](255) NOT NULL,
	[ChannelId] [varchar](255) NOT NULL,
	[SubChannelId] [varchar](255) NOT NULL,
	[Initiator] [varchar](255) NOT NULL,
	[MinInvestment] [numeric](19, 0) NOT NULL,
	[MaxInvestment] [numeric](19, 0) NOT NULL,
	[Approver1] [varchar](255) NULL,
	[Approver2] [varchar](255) NULL,
	[Approver3] [varchar](255) NULL,
	[Approver4] [varchar](255) NULL,
	[Approver5] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPromoPlanningImport]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPromoPlanningImport](
	[ImportId] [varchar](255) NULL,
	[id] [int] NOT NULL,
	[periode] [varchar](4) NULL,
	[entity] [varchar](255) NULL,
	[distributor] [varchar](255) NULL,
	[category] [varchar](255) NULL,
	[subactivitytype] [varchar](255) NULL,
	[subcategory] [varchar](255) NULL,
	[activity] [varchar](255) NULL,
	[subactivity] [varchar](255) NULL,
	[activitydesc] [varchar](255) NULL,
	[startpromo] [varchar](255) NULL,
	[endpromo] [varchar](255) NULL,
	[mechanisme] [varchar](255) NULL,
	[baselinesales] [numeric](19, 0) NULL,
	[incrsales] [numeric](19, 0) NULL,
	[investment] [numeric](19, 0) NULL,
	[channel] [varchar](255) NULL,
	[subchannel] [varchar](255) NULL,
	[account] [varchar](255) NULL,
	[subaccount] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tempPromoPlanningType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tempPromoPlanningType](
	[PromoPlanId] [int] NULL,
	[Periode] [varchar](4) NULL,
	[DistributorId] [int] NULL,
	[EntityId] [int] NULL,
	[PrincipalShortDesc] [varchar](50) NULL,
	[CategoryShortDesc] [varchar](50) NULL,
	[CategoryId] [int] NULL,
	[SubCategoryId] [int] NULL,
	[ActivityId] [int] NULL,
	[SubActivityId] [int] NULL,
	[ActivityDesc] [varchar](255) NULL,
	[StartPromo] [date] NULL,
	[EndPromo] [date] NULL,
	[Mechanisme1] [varchar](255) NULL,
	[Mechanisme2] [varchar](255) NULL,
	[Mechanisme3] [varchar](255) NULL,
	[Mechanisme4] [varchar](255) NULL,
	[Investment] [numeric](19, 2) NULL,
	[NormalSales] [numeric](19, 2) NULL,
	[IncrSales] [numeric](19, 2) NULL,
	[Roi] [numeric](19, 2) NULL,
	[CostRatio] [numeric](19, 2) NULL,
	[Notes] [varchar](255) NULL,
	[CreateOn] [date] NULL,
	[CreateBy] [varchar](50) NULL,
	[initiator_notes] [varchar](255) NULL,
	[createdEmail] [varchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tempPromoPlanningV4Type]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tempPromoPlanningV4Type](
	[PromoPlanId] [int] NULL,
	[Periode] [varchar](4) NULL,
	[DistributorId] [int] NULL,
	[EntityId] [int] NULL,
	[PrincipalShortDesc] [varchar](50) NULL,
	[CategoryShortDesc] [varchar](50) NULL,
	[CategoryId] [int] NULL,
	[SubCategoryId] [int] NULL,
	[ActivityId] [int] NULL,
	[SubActivityId] [int] NULL,
	[ActivityDesc] [varchar](255) NULL,
	[StartPromo] [date] NULL,
	[EndPromo] [date] NULL,
	[Mechanisme1] [varchar](255) NULL,
	[Mechanisme2] [varchar](255) NULL,
	[Mechanisme3] [varchar](255) NULL,
	[Mechanisme4] [varchar](255) NULL,
	[Investment] [numeric](19, 2) NULL,
	[NormalSales] [numeric](19, 2) NULL,
	[IncrSales] [numeric](19, 2) NULL,
	[Roi] [numeric](19, 2) NULL,
	[CostRatio] [numeric](19, 2) NULL,
	[Notes] [varchar](255) NULL,
	[CreateOn] [date] NULL,
	[CreateBy] [varchar](50) NULL,
	[initiator_notes] [varchar](255) NULL,
	[createdEmail] [varchar](100) NULL,
	[ModifReason] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPromoReconType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPromoReconType](
	[PromoId] [int] NULL,
	[PromoPlanId] [int] NULL,
	[AllocationId] [int] NULL,
	[AllocationRefId] [varchar](50) NULL,
	[PrincipalShortDesc] [varchar](50) NULL,
	[CategoryShortDesc] [varchar](50) NULL,
	[BudgetMasterId] [int] NULL,
	[CategoryId] [int] NULL,
	[SubCategoryId] [int] NULL,
	[ActivityId] [int] NULL,
	[SubActivityId] [int] NULL,
	[ActivityDesc] [varchar](255) NULL,
	[StartPromo] [date] NULL,
	[EndPromo] [date] NULL,
	[Mechanisme1] [varchar](255) NULL,
	[Mechanisme2] [varchar](255) NULL,
	[Mechanisme3] [varchar](255) NULL,
	[Mechanisme4] [varchar](255) NULL,
	[Investment] [numeric](19, 2) NULL,
	[NormalSales] [numeric](19, 2) NULL,
	[IncrSales] [numeric](19, 2) NULL,
	[Roi] [numeric](19, 2) NULL,
	[CostRatio] [numeric](19, 2) NULL,
	[StatusApproval] [varchar](10) NULL,
	[Notes] [varchar](255) NULL,
	[TsCoding] [varchar](50) NULL,
	[CreateOn] [date] NULL,
	[CreateBy] [varchar](50) NULL,
	[initiator_notes] [varchar](255) NULL,
	[actual_sales] [numeric](19, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPromoSKPType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPromoSKPType](
	[PromoId] [int] NOT NULL,
	[SKPDraftAvail] [bit] NULL,
	[SKPDraftAvailOn] [datetime] NULL,
	[SKPDraftAvailBy] [varchar](50) NULL,
	[SKPDraftAvailBfrAct60] [bit] NULL,
	[SKPDraftAvailBfrAct60On] [datetime] NULL,
	[SKPDraftAvailBfrAct60By] [varchar](50) NULL,
	[PeriodMatch] [bit] NULL,
	[PeriodMatchOn] [datetime] NULL,
	[PeriodMatchBy] [varchar](50) NULL,
	[InvestmentMatch] [bit] NULL,
	[InvestmentMatchOn] [datetime] NULL,
	[InvestmentMatchBy] [varchar](50) NULL,
	[MechanismMatch] [bit] NULL,
	[MechanismMatchOn] [datetime] NULL,
	[MechanismMatchBy] [varchar](50) NULL,
	[SKPSign7] [bit] NULL,
	[SKPSign7On] [datetime] NULL,
	[SKPSign7By] [varchar](50) NULL,
	[EntityDraft] [bit] NULL,
	[EntityDraftOn] [datetime] NULL,
	[EntityDraftBy] [varchar](50) NULL,
	[BrandDraft] [bit] NULL,
	[BrandDraftOn] [datetime] NULL,
	[BrandDraftBy] [varchar](50) NULL,
	[PeriodDraft] [bit] NULL,
	[PeriodDraftOn] [datetime] NULL,
	[PeriodDraftBy] [varchar](50) NULL,
	[ActivityDescDraft] [bit] NULL,
	[ActivityDescDraftOn] [datetime] NULL,
	[ActivityDescDraftBy] [varchar](50) NULL,
	[MechanismDraft] [bit] NULL,
	[MechanismDraftOn] [datetime] NULL,
	[MechanismDraftBy] [varchar](50) NULL,
	[InvestmentDraft] [bit] NULL,
	[InvestmentDraftOn] [datetime] NULL,
	[InvestmentDraftBy] [varchar](50) NULL,
	[Entity] [bit] NULL,
	[EntityOn] [datetime] NULL,
	[EntityBy] [varchar](50) NULL,
	[Brand] [bit] NULL,
	[BrandOn] [datetime] NULL,
	[BrandBy] [varchar](50) NULL,
	[ActivityDesc] [bit] NULL,
	[ActivityDescOn] [datetime] NULL,
	[ActivityDescBy] [varchar](50) NULL,
	[DistributorDraft] [bit] NULL,
	[DistributorDraftOn] [datetime] NULL,
	[DistributorDraftBy] [varchar](50) NULL,
	[Distributor] [bit] NULL,
	[DistributorOn] [datetime] NULL,
	[DistributorBy] [varchar](50) NULL,
	[ChannelDraft] [bit] NULL,
	[ChannelDraftOn] [datetime] NULL,
	[ChannelDraftBy] [varchar](50) NULL,
	[Channel] [bit] NULL,
	[ChannelOn] [datetime] NULL,
	[ChannelBy] [varchar](50) NULL,
	[StoreNameDraft] [bit] NULL,
	[StoreNameDraftOn] [datetime] NULL,
	[StoreNameDraftBy] [varchar](50) NULL,
	[StoreName] [bit] NULL,
	[StoreNameOn] [datetime] NULL,
	[StoreNameBy] [varchar](50) NULL,
	[skpstatus] [int] NULL,
	[skp_notes] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPromoType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPromoType](
	[PromoId] [int] NULL,
	[PromoPlanId] [int] NULL,
	[AllocationId] [int] NULL,
	[AllocationRefId] [varchar](50) NULL,
	[PrincipalShortDesc] [varchar](50) NULL,
	[CategoryShortDesc] [varchar](50) NULL,
	[BudgetMasterId] [int] NULL,
	[CategoryId] [int] NULL,
	[SubCategoryId] [int] NULL,
	[ActivityId] [int] NULL,
	[SubActivityId] [int] NULL,
	[ActivityDesc] [varchar](255) NULL,
	[StartPromo] [date] NULL,
	[EndPromo] [date] NULL,
	[Mechanisme1] [varchar](255) NULL,
	[Mechanisme2] [varchar](255) NULL,
	[Mechanisme3] [varchar](255) NULL,
	[Mechanisme4] [varchar](255) NULL,
	[Investment] [numeric](19, 2) NULL,
	[NormalSales] [numeric](19, 2) NULL,
	[IncrSales] [numeric](19, 2) NULL,
	[Roi] [numeric](19, 2) NULL,
	[CostRatio] [numeric](19, 2) NULL,
	[StatusApproval] [varchar](10) NULL,
	[Notes] [varchar](255) NULL,
	[TsCoding] [varchar](50) NULL,
	[CreateOn] [date] NULL,
	[CreateBy] [varchar](50) NULL,
	[initiator_notes] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempPromoV4ReconType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempPromoV4ReconType](
	[PromoId] [int] NULL,
	[PromoPlanId] [int] NULL,
	[AllocationId] [int] NULL,
	[AllocationRefId] [varchar](50) NULL,
	[PrincipalShortDesc] [varchar](50) NULL,
	[CategoryShortDesc] [varchar](50) NULL,
	[BudgetMasterId] [int] NULL,
	[CategoryId] [int] NULL,
	[SubCategoryId] [int] NULL,
	[ActivityId] [int] NULL,
	[SubActivityId] [int] NULL,
	[ActivityDesc] [varchar](255) NULL,
	[StartPromo] [date] NULL,
	[EndPromo] [date] NULL,
	[Mechanisme1] [varchar](255) NULL,
	[Mechanisme2] [varchar](255) NULL,
	[Mechanisme3] [varchar](255) NULL,
	[Mechanisme4] [varchar](255) NULL,
	[Investment] [numeric](19, 2) NULL,
	[NormalSales] [numeric](19, 2) NULL,
	[IncrSales] [numeric](19, 2) NULL,
	[Roi] [numeric](19, 2) NULL,
	[CostRatio] [numeric](19, 2) NULL,
	[StatusApproval] [varchar](10) NULL,
	[Notes] [varchar](255) NULL,
	[TsCoding] [varchar](50) NULL,
	[CreateOn] [date] NULL,
	[CreateBy] [varchar](50) NULL,
	[initiator_notes] [varchar](255) NULL,
	[actual_sales] [numeric](19, 2) NULL,
	[CreatedEmail] [varchar](100) NULL,
	[ModifReason] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tempPromoV4Type]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tempPromoV4Type](
	[PromoId] [int] NULL,
	[PromoPlanId] [int] NULL,
	[AllocationId] [int] NULL,
	[AllocationRefId] [varchar](50) NULL,
	[PrincipalShortDesc] [varchar](50) NULL,
	[CategoryShortDesc] [varchar](50) NULL,
	[BudgetMasterId] [int] NULL,
	[CategoryId] [int] NULL,
	[SubCategoryId] [int] NULL,
	[ActivityId] [int] NULL,
	[SubActivityId] [int] NULL,
	[ActivityDesc] [varchar](255) NULL,
	[StartPromo] [date] NULL,
	[EndPromo] [date] NULL,
	[Mechanisme1] [varchar](255) NULL,
	[Mechanisme2] [varchar](255) NULL,
	[Mechanisme3] [varchar](255) NULL,
	[Mechanisme4] [varchar](255) NULL,
	[Investment] [numeric](19, 2) NULL,
	[NormalSales] [numeric](19, 2) NULL,
	[IncrSales] [numeric](19, 2) NULL,
	[Roi] [numeric](19, 2) NULL,
	[CostRatio] [numeric](19, 2) NULL,
	[StatusApproval] [varchar](10) NULL,
	[Notes] [varchar](255) NULL,
	[TsCoding] [varchar](50) NULL,
	[CreateOn] [date] NULL,
	[CreateBy] [varchar](50) NULL,
	[initiator_notes] [varchar](255) NULL,
	[CreatedEmail] [varchar](100) NULL,
	[ModifReason] [varchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempRegion4]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempRegion4](
	[ParentId] [int] NULL,
	[Id] [varchar](50) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempSku4]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempSku4](
	[ParentId] [int] NULL,
	[Id] [varchar](50) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tempSSVolumeType]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tempSSVolumeType](
	[period] [varchar](4) NOT NULL,
	[channel] [varchar](255) NOT NULL,
	[subchannel] [varchar](255) NOT NULL,
	[account] [varchar](255) NOT NULL,
	[subaccount] [varchar](255) NOT NULL,
	[region] [varchar](255) NOT NULL,
	[groupbrand] [varchar](255) NOT NULL,
	[m1] [float] NULL,
	[m2] [float] NULL,
	[m3] [float] NULL,
	[m4] [float] NULL,
	[m5] [float] NULL,
	[m6] [float] NULL,
	[m7] [float] NULL,
	[m8] [float] NULL,
	[m9] [float] NULL,
	[m10] [float] NULL,
	[m11] [float] NULL,
	[m12] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempSubAccount4]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempSubAccount4](
	[ParentId] [int] NULL,
	[Id] [varchar](50) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempSubChannel4]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempSubChannel4](
	[ParentId] [int] NULL,
	[Id] [varchar](50) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempTemplateDebetNote]    Script Date: 10/6/2025 10:41:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempTemplateDebetNote](
	[RefId] [varchar](255) NULL,
	[PromoRefId] [varchar](255) NULL,
	[Activity] [varchar](255) NULL,
	[TotalClaim] [varchar](255) NULL,
	[LastStatus] [varchar](255) NULL,
	[LastUpdate] [varchar](255) NULL
) ON [PRIMARY]
GO

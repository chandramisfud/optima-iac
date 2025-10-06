ALTER TABLE [dbo].[tbhis_accrual_report_dtl] ADD  DEFAULT ((0)) FOR [grpBrandId]
GO
ALTER TABLE [dbo].[tbhis_accrual_report_dtl] ADD  DEFAULT ('') FOR [grpBrandDesc]
GO
ALTER TABLE [dbo].[tbhis_budget] ADD  CONSTRAINT [DF_tbhis_budget_sspsvalue]  DEFAULT ((0)) FOR [sspsvalue]
GO
ALTER TABLE [dbo].[tbhis_budget] ADD  CONSTRAINT [DF_tbhis_budget_approvalstatus]  DEFAULT ((0)) FOR [approvalstatus]
GO
ALTER TABLE [dbo].[tbhis_budget] ADD  CONSTRAINT [DF_tbhis_budget_deploymentstatus]  DEFAULT ((0)) FOR [deploymentstatus]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  DEFAULT ((0)) FOR [baseline]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  DEFAULT ((0)) FOR [totalSales]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  DEFAULT ((0)) FOR [uplift]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  DEFAULT ((0)) FOR [salesContribution]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  DEFAULT ((0)) FOR [storesCoverage]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  DEFAULT ((0)) FOR [redemptionRate]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  DEFAULT ((0)) FOR [cr]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  DEFAULT ((0)) FOR [cost]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  CONSTRAINT [DF_tbhis_config_promo_calculator_baselineRecon]  DEFAULT ((0)) FOR [baselineRecon]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  CONSTRAINT [DF_tbhis_config_promo_calculator_totalSalesRecon]  DEFAULT ((0)) FOR [totalSalesRecon]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  CONSTRAINT [DF_tbhis_config_promo_calculator_upliftRecon]  DEFAULT ((0)) FOR [upliftRecon]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  CONSTRAINT [DF_tbhis_config_promo_calculator_salesContributionRecon]  DEFAULT ((0)) FOR [salesContributionRecon]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  CONSTRAINT [DF_tbhis_config_promo_calculator_storesCoverageRecon]  DEFAULT ((0)) FOR [storesCoverageRecon]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  CONSTRAINT [DF_tbhis_config_promo_calculator_redemptionRateRecon]  DEFAULT ((0)) FOR [redemptionRateRecon]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  CONSTRAINT [DF_tbhis_config_promo_calculator_crRecon]  DEFAULT ((0)) FOR [crRecon]
GO
ALTER TABLE [dbo].[tbhis_config_promo_calculator] ADD  CONSTRAINT [DF_tbhis_config_promo_calculator_costRecon]  DEFAULT ((0)) FOR [costRecon]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [budgetYear]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [promoPlanning]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [budgetSource]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [subCategory]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [activity]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [subActivity]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [subActivityType]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [startPromo]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [endPromo]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [activityName]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [initiatorNotes]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [incrSales]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [investment]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [channel]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [subChannel]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [account]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [subAccount]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [region]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [brand]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [SKU]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [mechanism]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [Attachment]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [ROI]
GO
ALTER TABLE [dbo].[tbhis_config_promo_items] ADD  DEFAULT ((0)) FOR [CR]
GO
ALTER TABLE [dbo].[tbhis_config_reminder] ADD  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbhis_debetnote] ADD  DEFAULT ((0)) FOR [DNAmount]
GO
ALTER TABLE [dbo].[tbhis_debetnote] ADD  DEFAULT ('') FOR [FeeDesc]
GO
ALTER TABLE [dbo].[tbhis_debetnote] ADD  DEFAULT ((0)) FOR [FeePct]
GO
ALTER TABLE [dbo].[tbhis_debetnote] ADD  DEFAULT ((0)) FOR [FeeAmount]
GO
ALTER TABLE [dbo].[tbhis_debetnote] ADD  DEFAULT ((0)) FOR [PPHPct]
GO
ALTER TABLE [dbo].[tbhis_debetnote] ADD  DEFAULT ((0)) FOR [PPHAmt]
GO
ALTER TABLE [dbo].[tbhis_debetnote] ADD  DEFAULT ('') FOR [statusPPH]
GO
ALTER TABLE [dbo].[tbhis_debetnote] ADD  DEFAULT ((0)) FOR [VATExpired]
GO
ALTER TABLE [dbo].[tbhis_debetnote] ADD  DEFAULT ('') FOR [WHTType]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [Activity]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [SubActivity]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [StartPromo]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [EndPromo]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [ActivityDesc]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [InitiatorNotes]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [IncrSales]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [Investment]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [ROI]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [CR]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [Channel]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [SubChannel]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [Account]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [SubAccount]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [Region]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [Brand]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [SKU]
GO
ALTER TABLE [dbo].[tbhis_major_changes] ADD  DEFAULT ((0)) FOR [Mechanism]
GO
ALTER TABLE [dbo].[tbhis_map_promorecon_period_subactivity] ADD  DEFAULT ((0)) FOR [AllowEdit]
GO
ALTER TABLE [dbo].[tbhis_map_promorecon_period_subactivity] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tbhis_mechanism] ADD  DEFAULT ((0)) FOR [ChannelId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_PromoPlanId]  DEFAULT ((0)) FOR [PromoPlanId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_AllocationId]  DEFAULT ((0)) FOR [AllocationId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_AllocationRefId]  DEFAULT ('') FOR [AllocationRefId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_CategoryShortDesc]  DEFAULT ('') FOR [CategoryShortDesc]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_PrincipalShortDesc]  DEFAULT ('') FOR [PrincipalShortDesc]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_BudgetMasterId]  DEFAULT ((0)) FOR [BudgetMasterId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_CategoryId]  DEFAULT ((0)) FOR [CategoryId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_SubCategoryId]  DEFAULT ((0)) FOR [SubCategoryId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_ActivityId]  DEFAULT ((0)) FOR [ActivityId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_SubActivityId]  DEFAULT ((0)) FOR [SubActivityId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_Investment]  DEFAULT ((0)) FOR [Investment]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_NormalSales]  DEFAULT ((0)) FOR [NormalSales]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_IncrSales]  DEFAULT ((0)) FOR [IncrSales]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_Roi]  DEFAULT ((0)) FOR [Roi]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_CostRatio]  DEFAULT ((0)) FOR [CostRatio]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF__tbhis_pro__IsAct__3B2BBE9D]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF__tbhis_pro__IsLoc__3C1FE2D6]  DEFAULT ((0)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF__tbhis_pro__IsDel__3E082B48]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF__tbhis_pro__IsCan__3EFC4F81]  DEFAULT ((0)) FOR [IsCancel]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF_tbhis_promo_IsCancelLocked]  DEFAULT ((0)) FOR [IsCancelLocked]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF__tbhis_pro__IsClo__3D14070F]  DEFAULT ((0)) FOR [IsClose]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  CONSTRAINT [DF__tbhis_pro__actua__42E1EEFE]  DEFAULT ((0)) FOR [actual_sales]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  DEFAULT ((0)) FOR [EntityId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  DEFAULT ((0)) FOR [DistributorId]
GO
ALTER TABLE [dbo].[tbhis_promo] ADD  DEFAULT ((0)) FOR [GroupBrandId]
GO
ALTER TABLE [dbo].[tbhis_promo_account] ADD  CONSTRAINT [DF_tbhis_promo_account_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbhis_promo_account] ADD  CONSTRAINT [DF_tbhis_promo_account_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbhis_promo_account] ADD  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_account] ADD  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_account] ADD  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_brand] ADD  CONSTRAINT [DF_tbhis_promo_brand_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbhis_promo_brand] ADD  CONSTRAINT [DF_tbhis_promo_brand_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbhis_promo_brand] ADD  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_brand] ADD  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_brand] ADD  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_calculator] ADD  CONSTRAINT [DF_tbhis_promo_calculator_Baseline]  DEFAULT ((0)) FOR [Baseline]
GO
ALTER TABLE [dbo].[tbhis_promo_calculator] ADD  CONSTRAINT [DF_tbhis_promo_calculator_Uplift]  DEFAULT ((0)) FOR [Uplift]
GO
ALTER TABLE [dbo].[tbhis_promo_calculator] ADD  CONSTRAINT [DF_tbhis_promo_calculator_TotalSales]  DEFAULT ((0)) FOR [TotalSales]
GO
ALTER TABLE [dbo].[tbhis_promo_calculator] ADD  CONSTRAINT [DF_tbhis_promo_calculator_SalesContribution]  DEFAULT ((0)) FOR [SalesContribution]
GO
ALTER TABLE [dbo].[tbhis_promo_calculator] ADD  CONSTRAINT [DF_tbhis_promo_calculator_StoresCoverage]  DEFAULT ((0)) FOR [StoresCoverage]
GO
ALTER TABLE [dbo].[tbhis_promo_calculator] ADD  CONSTRAINT [DF_tbhis_promo_calculator_RedemptionRate]  DEFAULT ((0)) FOR [RedemptionRate]
GO
ALTER TABLE [dbo].[tbhis_promo_calculator] ADD  CONSTRAINT [DF_tbhis_promo_calculator_CR]  DEFAULT ((0)) FOR [CR]
GO
ALTER TABLE [dbo].[tbhis_promo_calculator] ADD  CONSTRAINT [DF_tbhis_promo_calculator_Cost]  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[tbhis_promo_channel] ADD  CONSTRAINT [DF_tbhis_promo_channel_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbhis_promo_channel] ADD  CONSTRAINT [DF_tbhis_promo_channel_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbhis_promo_planning] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbhis_promo_planning] ADD  DEFAULT ((1)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[tbhis_promo_planning] ADD  DEFAULT ((0)) FOR [late_submission_day]
GO
ALTER TABLE [dbo].[tbhis_promo_product] ADD  CONSTRAINT [DF_tbhis_promo_product_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbhis_promo_product] ADD  CONSTRAINT [DF_tbhis_promo_product_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbhis_promo_product] ADD  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_product] ADD  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_product] ADD  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_region] ADD  CONSTRAINT [DF_tbhis_promo_region_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbhis_promo_region] ADD  CONSTRAINT [DF_tbhis_promo_region_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbhis_promo_subaccount] ADD  CONSTRAINT [DF_tbhis_promo_subaccount_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbhis_promo_subaccount] ADD  CONSTRAINT [DF_tbhis_promo_subaccount_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbhis_promo_subaccount] ADD  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_subaccount] ADD  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_subaccount] ADD  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbhis_promo_subchannel] ADD  CONSTRAINT [DF_tbhis_promo_subchannel_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbhis_promo_subchannel] ADD  CONSTRAINT [DF_tbhis_promo_subchannel_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbhis_roi_cr] ADD  DEFAULT ((0)) FOR [MaksimumROI]
GO
ALTER TABLE [dbo].[tbhis_roi_cr] ADD  DEFAULT ((0)) FOR [MinimumCostRatio]
GO
ALTER TABLE [dbo].[tbhis_tools_promo_approval_reminder] ADD  DEFAULT ((0)) FOR [EOD]
GO
ALTER TABLE [dbo].[tbhis_tools_promo_approval_reminder] ADD  DEFAULT ((0)) FOR [autorun]
GO
ALTER TABLE [dbo].[tbhis_user] ADD  DEFAULT ((0)) FOR [isdeleted]
GO
ALTER TABLE [dbo].[tbhis_user] ADD  DEFAULT ((0)) FOR [isLogin]
GO
ALTER TABLE [dbo].[tbhis_user] ADD  DEFAULT ((1)) FOR [usernew]
GO
ALTER TABLE [dbo].[tbhis_user] ADD  DEFAULT ((0)) FOR [loginFailedCount]
GO
ALTER TABLE [dbo].[tbhlp_promo_baseline] ADD  DEFAULT ((0)) FOR [actual_sales]
GO
ALTER TABLE [dbo].[tbhlp_promo_product] ADD  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbhlp_promo_product] ADD  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbhlp_promo_product] ADD  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tblog_baseline] ADD  CONSTRAINT [DF_tblog_baseline_dtime]  DEFAULT (getdate()) FOR [dtime]
GO
ALTER TABLE [dbo].[tblog_user] ADD  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tblog_user] ADD  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tblog_user] ADD  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbmst_account] ADD  CONSTRAINT [DF_Account_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_account] ADD  CONSTRAINT [DF_Account_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbmst_account] ADD  CONSTRAINT [DF__tbmst_acc__Creat__2DB9B954]  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbmst_account] ADD  CONSTRAINT [DF__tbmst_acc__Modif__2EADDD8D]  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbmst_account] ADD  CONSTRAINT [DF__tbmst_acc__Delet__2FA201C6]  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbmst_activity] ADD  CONSTRAINT [DF_tblmst_activity_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_activity] ADD  CONSTRAINT [DF__tbmst_act__Creat__364EFF55]  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbmst_activity] ADD  CONSTRAINT [DF__tbmst_act__Modif__3743238E]  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbmst_activity] ADD  CONSTRAINT [DF__tbmst_act__Delet__383747C7]  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbmst_brand] ADD  CONSTRAINT [DF_Brand_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_brand] ADD  CONSTRAINT [DF__tbmst_bra__Creat__3C07D8AB]  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbmst_brand] ADD  CONSTRAINT [DF__tbmst_bra__Modif__3CFBFCE4]  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbmst_brand] ADD  CONSTRAINT [DF__tbmst_bra__Delet__3DF0211D]  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbmst_brand_group] ADD  CONSTRAINT [DF__tbmst_bra__IsAct__37782DB8]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_cancelreason] ADD  CONSTRAINT [DF__tbmst_can__Creat__25247353]  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbmst_cancelreason] ADD  CONSTRAINT [DF__tbmst_can__Modif__2618978C]  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbmst_cancelreason] ADD  CONSTRAINT [DF__tbmst_can__Delet__270CBBC5]  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbmst_category] ADD  CONSTRAINT [DF_tblmst_category_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_channel] ADD  CONSTRAINT [DF_Channel_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_channel] ADD  CONSTRAINT [DF_Date]  DEFAULT (getdate()) FOR [CreateOn]
GO
ALTER TABLE [dbo].[tbmst_channel] ADD  CONSTRAINT [DF_Channel_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbmst_distributor] ADD  CONSTRAINT [DF_tblmst_distributor_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_distributor] ADD  CONSTRAINT [DF_tbmst_distributor_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tbmst_doc_status] ADD  CONSTRAINT [DF_tblmst_doc_status_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_mechanism] ADD  CONSTRAINT [DF__tbmst_mec__Chann__2200E977]  DEFAULT ((0)) FOR [ChannelId]
GO
ALTER TABLE [dbo].[tbmst_principal] ADD  CONSTRAINT [DF_tblmst_principal_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_principal] ADD  CONSTRAINT [DF_tbmst_principal_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tbmst_product] ADD  CONSTRAINT [DF_Product_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_region] ADD  CONSTRAINT [DF_Region_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_sellingpoint] ADD  CONSTRAINT [DF_Sellingpoint_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_subaccount] ADD  CONSTRAINT [DF_SubAccount_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_subaccount] ADD  CONSTRAINT [DF_SubAccount_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbmst_subactivity] ADD  CONSTRAINT [DF_tblmst_subactivity_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_subactivity_type] ADD  CONSTRAINT [DF_tblmst_subactivity_type_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_subcategory] ADD  CONSTRAINT [DF_tblmst_sub_category_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_subchannel] ADD  CONSTRAINT [DF_SubChannel_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbmst_subchannel] ADD  CONSTRAINT [DF_SubChannel_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbset_config_promo_calculator] ADD  DEFAULT ((0)) FOR [baseline]
GO
ALTER TABLE [dbo].[tbset_config_promo_calculator] ADD  DEFAULT ((0)) FOR [totalSales]
GO
ALTER TABLE [dbo].[tbset_config_promo_calculator] ADD  DEFAULT ((0)) FOR [uplift]
GO
ALTER TABLE [dbo].[tbset_config_promo_calculator] ADD  DEFAULT ((0)) FOR [salesContribution]
GO
ALTER TABLE [dbo].[tbset_config_promo_calculator] ADD  DEFAULT ((0)) FOR [storesCoverage]
GO
ALTER TABLE [dbo].[tbset_config_promo_calculator] ADD  DEFAULT ((0)) FOR [redemptionRate]
GO
ALTER TABLE [dbo].[tbset_config_promo_calculator] ADD  DEFAULT ((0)) FOR [cr]
GO
ALTER TABLE [dbo].[tbset_config_promo_calculator] ADD  DEFAULT ((0)) FOR [cost]
GO
ALTER TABLE [dbo].[tbset_config_promo_calculator] ADD  DEFAULT ((99999)) FOR [channelId]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__budge__5786A145]  DEFAULT ((0)) FOR [budgetYear]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__promo__587AC57E]  DEFAULT ((0)) FOR [promoPlanning]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__budge__596EE9B7]  DEFAULT ((0)) FOR [budgetSource]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__subCa__5A630DF0]  DEFAULT ((0)) FOR [subCategory]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__activ__5B573229]  DEFAULT ((0)) FOR [activity]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__subAc__5C4B5662]  DEFAULT ((0)) FOR [subActivity]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__subAc__5D3F7A9B]  DEFAULT ((0)) FOR [subActivityType]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__start__5E339ED4]  DEFAULT ((0)) FOR [startPromo]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__endPr__5F27C30D]  DEFAULT ((0)) FOR [endPromo]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__activ__601BE746]  DEFAULT ((0)) FOR [activityName]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__initi__61100B7F]  DEFAULT ((0)) FOR [initiatorNotes]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__incrS__62042FB8]  DEFAULT ((0)) FOR [incrSales]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__inves__62F853F1]  DEFAULT ((0)) FOR [investment]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__chann__63EC782A]  DEFAULT ((0)) FOR [channel]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__subCh__64E09C63]  DEFAULT ((0)) FOR [subChannel]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__accou__65D4C09C]  DEFAULT ((0)) FOR [account]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__subAc__66C8E4D5]  DEFAULT ((0)) FOR [subAccount]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__regio__67BD090E]  DEFAULT ((0)) FOR [region]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__brand__68B12D47]  DEFAULT ((0)) FOR [brand]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_confi__SKU__69A55180]  DEFAULT ((0)) FOR [SKU]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__mecha__6A9975B9]  DEFAULT ((0)) FOR [mechanism]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__Attac__6B8D99F2]  DEFAULT ((0)) FOR [Attachment]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_confi__ROI__6C81BE2B]  DEFAULT ((0)) FOR [ROI]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_config__CR__6D75E264]  DEFAULT ((0)) FOR [CR]
GO
ALTER TABLE [dbo].[tbset_config_promo_items] ADD  CONSTRAINT [DF__tbset_con__isDel__6E6A069D]  DEFAULT ((0)) FOR [isDelete]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__budge__70524F0F]  DEFAULT ((0)) FOR [budgetYear]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__promo__71467348]  DEFAULT ((0)) FOR [promoPlanning]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__budge__723A9781]  DEFAULT ((0)) FOR [budgetSource]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__subCa__732EBBBA]  DEFAULT ((0)) FOR [subCategory]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__activ__7422DFF3]  DEFAULT ((0)) FOR [activity]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__subAc__7517042C]  DEFAULT ((0)) FOR [subActivity]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__subAc__760B2865]  DEFAULT ((0)) FOR [subActivityType]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__start__76FF4C9E]  DEFAULT ((0)) FOR [startPromo]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__endPr__77F370D7]  DEFAULT ((0)) FOR [endPromo]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__activ__78E79510]  DEFAULT ((0)) FOR [activityName]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__initi__79DBB949]  DEFAULT ((0)) FOR [initiatorNotes]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__incrS__7ACFDD82]  DEFAULT ((0)) FOR [incrSales]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__inves__7BC401BB]  DEFAULT ((0)) FOR [investment]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__chann__7CB825F4]  DEFAULT ((0)) FOR [channel]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__subCh__7DAC4A2D]  DEFAULT ((0)) FOR [subChannel]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__accou__7EA06E66]  DEFAULT ((0)) FOR [account]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__subAc__7F94929F]  DEFAULT ((0)) FOR [subAccount]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__regio__0088B6D8]  DEFAULT ((0)) FOR [region]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__brand__017CDB11]  DEFAULT ((0)) FOR [brand]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_confi__SKU__0270FF4A]  DEFAULT ((0)) FOR [SKU]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__mecha__03652383]  DEFAULT ((0)) FOR [mechanism]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_con__Attac__045947BC]  DEFAULT ((0)) FOR [Attachment]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_confi__ROI__054D6BF5]  DEFAULT ((0)) FOR [ROI]
GO
ALTER TABLE [dbo].[tbset_config_promo_items_disabled] ADD  CONSTRAINT [DF__tbset_config__CR__0641902E]  DEFAULT ((0)) FOR [CR]
GO
ALTER TABLE [dbo].[tbset_config_reminder] ADD  CONSTRAINT [DF__tbset_con__Delet__3EE44556]  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Activ__3834E5C2]  DEFAULT ((0)) FOR [Activity]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__SubAc__392909FB]  DEFAULT ((0)) FOR [SubActivity]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Start__3A1D2E34]  DEFAULT ((0)) FOR [StartPromo]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__EndPr__3B11526D]  DEFAULT ((0)) FOR [EndPromo]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Activ__3C0576A6]  DEFAULT ((0)) FOR [ActivityDesc]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Initi__3CF99ADF]  DEFAULT ((0)) FOR [InitiatorNotes]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__IncrS__3DEDBF18]  DEFAULT ((0)) FOR [IncrSales]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Inves__3EE1E351]  DEFAULT ((0)) FOR [Investment]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_major__ROI__3FD6078A]  DEFAULT ((0)) FOR [ROI]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_major___CR__40CA2BC3]  DEFAULT ((0)) FOR [CR]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Chann__41BE4FFC]  DEFAULT ((0)) FOR [Channel]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__SubCh__42B27435]  DEFAULT ((0)) FOR [SubChannel]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Accou__43A6986E]  DEFAULT ((0)) FOR [Account]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__SubAc__449ABCA7]  DEFAULT ((0)) FOR [SubAccount]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Regio__458EE0E0]  DEFAULT ((0)) FOR [Region]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Brand__46830519]  DEFAULT ((0)) FOR [Brand]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_major__SKU__47772952]  DEFAULT ((0)) FOR [SKU]
GO
ALTER TABLE [dbo].[tbset_major_changes] ADD  CONSTRAINT [DF__tbset_maj__Mecha__486B4D8B]  DEFAULT ((0)) FOR [Mechanism]
GO
ALTER TABLE [dbo].[tbset_map_promorecon_period_subactivity] ADD  CONSTRAINT [DF__tbset_map__Allow__3FD3A585]  DEFAULT ((0)) FOR [AllowEdit]
GO
ALTER TABLE [dbo].[tbset_map_promorecon_period_subactivity] ADD  CONSTRAINT [DF__tbset_map__IsDel__40C7C9BE]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tbset_matrix_promo_approval] ADD  CONSTRAINT [DF_tbset_matrix_promo_approval_CreateOn]  DEFAULT (getdate()) FOR [CreateOn]
GO
ALTER TABLE [dbo].[tbset_menu] ADD  CONSTRAINT [DF__tbset_men__paren__3EDC53F0]  DEFAULT (NULL) FOR [parent]
GO
ALTER TABLE [dbo].[tbset_menu] ADD  CONSTRAINT [DF__tbset_menu__flag__3FD07829]  DEFAULT (NULL) FOR [flag]
GO
ALTER TABLE [dbo].[tbset_menu] ADD  CONSTRAINT [DF_tbset_menu_crud]  DEFAULT ((0)) FOR [crud]
GO
ALTER TABLE [dbo].[tbset_menu] ADD  CONSTRAINT [DF_tbset_menu_approve]  DEFAULT ((0)) FOR [approve]
GO
ALTER TABLE [dbo].[tbset_menu_v4] ADD  CONSTRAINT [DF__tbset_men__paren__42E9E064]  DEFAULT (NULL) FOR [parent]
GO
ALTER TABLE [dbo].[tbset_menu_v4] ADD  CONSTRAINT [DF__tbset_menu__flag__43DE049D]  DEFAULT (NULL) FOR [flag]
GO
ALTER TABLE [dbo].[tbset_menu_v4] ADD  CONSTRAINT [DF__tbset_menu__crud__44D228D6]  DEFAULT ((0)) FOR [crud]
GO
ALTER TABLE [dbo].[tbset_menu_v4] ADD  CONSTRAINT [DF__tbset_men__appro__45C64D0F]  DEFAULT ((0)) FOR [approve]
GO
ALTER TABLE [dbo].[tbset_promo_budget_approval] ADD  CONSTRAINT [DF_tbset_promo_budget_approval_MinAmount]  DEFAULT ((0)) FOR [MinAmount]
GO
ALTER TABLE [dbo].[tbset_promo_budget_approval] ADD  CONSTRAINT [DF_tbset_promo_budget_approval_MaxAmount]  DEFAULT ((0)) FOR [MaxAmount]
GO
ALTER TABLE [dbo].[tbset_register] ADD  CONSTRAINT [DF_tbset_register_approve]  DEFAULT ((9)) FOR [approve]
GO
ALTER TABLE [dbo].[tbset_register] ADD  CONSTRAINT [DF_tbset_register_daterequest]  DEFAULT (getdate()) FOR [daterequest]
GO
ALTER TABLE [dbo].[tbset_tools_promo_approval_reminder] ADD  CONSTRAINT [DF__tbset_tools__EOD__79245ED6]  DEFAULT ((0)) FOR [EOD]
GO
ALTER TABLE [dbo].[tbset_tools_promo_approval_reminder] ADD  CONSTRAINT [DF__tbset_too__autor__7A18830F]  DEFAULT ((0)) FOR [autorun]
GO
ALTER TABLE [dbo].[tbset_user] ADD  CONSTRAINT [DF_tbset_user_isdeleted]  DEFAULT ((0)) FOR [isdeleted]
GO
ALTER TABLE [dbo].[tbset_user] ADD  CONSTRAINT [DF__tbset_use__isLog__2F650636]  DEFAULT ((0)) FOR [isLogin]
GO
ALTER TABLE [dbo].[tbset_user] ADD  CONSTRAINT [DF__tbset_use__usern__2744C181]  DEFAULT ((1)) FOR [usernew]
GO
ALTER TABLE [dbo].[tbset_user] ADD  CONSTRAINT [DF__tbset_use__login__2838E5BA]  DEFAULT ((0)) FOR [loginFailedCount]
GO
ALTER TABLE [dbo].[tbset_user_distributor] ADD  CONSTRAINT [DF_tbset_distributor_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbset_user_login] ADD  CONSTRAINT [DF__tbset_use__isdel__454A25B4]  DEFAULT ((0)) FOR [isdeleted]
GO
ALTER TABLE [dbo].[tbset_user_login] ADD  CONSTRAINT [DF__tbset_use__lastL__463E49ED]  DEFAULT (getdate()) FOR [lastLogin]
GO
ALTER TABLE [dbo].[tbset_user_login] ADD  CONSTRAINT [DF__tbset_use__usern__4826925F]  DEFAULT ((0)) FOR [usernew]
GO
ALTER TABLE [dbo].[tbset_user_principal] ADD  CONSTRAINT [DF_tblmst_userprincipal_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbset_usergroup] ADD  CONSTRAINT [DF__tbset_use__useri__477199F1]  DEFAULT (NULL) FOR [userinput]
GO
ALTER TABLE [dbo].[tbset_usergroup] ADD  CONSTRAINT [DF__tbset_use__datei__4865BE2A]  DEFAULT (NULL) FOR [dateinput]
GO
ALTER TABLE [dbo].[tbset_usergroup] ADD  CONSTRAINT [DF__tbset_use__usere__4959E263]  DEFAULT (NULL) FOR [useredit]
GO
ALTER TABLE [dbo].[tbset_usergroup] ADD  CONSTRAINT [DF__tbset_use__datee__4A4E069C]  DEFAULT (NULL) FOR [dateedit]
GO
ALTER TABLE [dbo].[tbset_usergroup] ADD  CONSTRAINT [DF__tbset_use__userc__216BEC9A]  DEFAULT ((2)) FOR [groupmenupermission]
GO
ALTER TABLE [dbo].[tbset_userlevel] ADD  CONSTRAINT [DF__tbset_use__userg__4B422AD5]  DEFAULT (NULL) FOR [usergroupid]
GO
ALTER TABLE [dbo].[tbset_userlevel] ADD  CONSTRAINT [DF__tbset_use__useri__4C364F0E]  DEFAULT (NULL) FOR [userinput]
GO
ALTER TABLE [dbo].[tbset_userlevel] ADD  CONSTRAINT [DF__tbset_use__datei__4D2A7347]  DEFAULT (NULL) FOR [dateinput]
GO
ALTER TABLE [dbo].[tbset_userlevel] ADD  CONSTRAINT [DF__tbset_use__usere__4E1E9780]  DEFAULT (NULL) FOR [useredit]
GO
ALTER TABLE [dbo].[tbset_userlevel] ADD  CONSTRAINT [DF__tbset_use__datee__4F12BBB9]  DEFAULT (NULL) FOR [dateedit]
GO
ALTER TABLE [dbo].[tbset_userlevel_access] ADD  CONSTRAINT [DF__tbset_use__creat__5006DFF2]  DEFAULT ('0') FOR [create_rec]
GO
ALTER TABLE [dbo].[tbset_userlevel_access] ADD  CONSTRAINT [DF__tbset_use__read___50FB042B]  DEFAULT ('0') FOR [read_rec]
GO
ALTER TABLE [dbo].[tbset_userlevel_access] ADD  CONSTRAINT [DF__tbset_use__updat__51EF2864]  DEFAULT ('0') FOR [update_rec]
GO
ALTER TABLE [dbo].[tbset_userlevel_access] ADD  CONSTRAINT [DF__tbset_use__delet__52E34C9D]  DEFAULT ('0') FOR [delete_rec]
GO
ALTER TABLE [dbo].[tbset_userrights] ADD  CONSTRAINT [DF__tbset_use__creat__6754599E]  DEFAULT (NULL) FOR [create_rec]
GO
ALTER TABLE [dbo].[tbset_userrights] ADD  CONSTRAINT [DF__tbset_use__read___68487DD7]  DEFAULT (NULL) FOR [read_rec]
GO
ALTER TABLE [dbo].[tbset_userrights] ADD  CONSTRAINT [DF__tbset_use__updat__693CA210]  DEFAULT (NULL) FOR [update_rec]
GO
ALTER TABLE [dbo].[tbset_userrights] ADD  CONSTRAINT [DF__tbset_use__delet__6A30C649]  DEFAULT (NULL) FOR [delete_rec]
GO
ALTER TABLE [dbo].[tbtemp_blitz_optima] ADD  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbtemp_blitz_optima] ADD  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbtemp_blitz_optima] ADD  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbtemp_blitz_optima_map_check] ADD  DEFAULT (NULL) FOR [CreatedEmail]
GO
ALTER TABLE [dbo].[tbtemp_blitz_optima_map_check] ADD  DEFAULT (NULL) FOR [ModifiedEmail]
GO
ALTER TABLE [dbo].[tbtemp_blitz_optima_map_check] ADD  DEFAULT (NULL) FOR [DeleteEmail]
GO
ALTER TABLE [dbo].[tbtrx_allocation] ADD  CONSTRAINT [DF_BudgetAllocation_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_account] ADD  CONSTRAINT [DF_BudgetAllocationAccount_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_brand] ADD  CONSTRAINT [DF_BudgetAllocationbrand_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_channel] ADD  CONSTRAINT [DF_BudgetAllocationChannel_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_detail] ADD  CONSTRAINT [DF_BudgetAllocationDetail_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_detail] ADD  CONSTRAINT [DF_BudgetAllocationDetail_IsRecon]  DEFAULT ((1)) FOR [IsRecon]
GO
ALTER TABLE [dbo].[tbtrx_allocation_product] ADD  CONSTRAINT [DF_BudgetAllocationproduct_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_region] ADD  CONSTRAINT [DF_BudgetAllocationRegion_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_subaccount] ADD  CONSTRAINT [DF_BudgetAllocationSubAccount_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_subchannel] ADD  CONSTRAINT [DF_BudgetAllocationSubChannel_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_useraccess] ADD  CONSTRAINT [DF_BudgetAllocationUserAccess_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_userassign] ADD  CONSTRAINT [DF_BudgetAllocationUserAssign_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_allocation_userpromo] ADD  CONSTRAINT [DF_BudgetAllocationUserPromo_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_blitz_offtake_one_baseline] ADD  CONSTRAINT [DF_tbtrx_blitz_offtake_one_baseline_oneBaselineValue]  DEFAULT ((0)) FOR [oneBaselineValue]
GO
ALTER TABLE [dbo].[tbtrx_budgetassignment] ADD  CONSTRAINT [DF_BudgetAssignment_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_debetnote] ADD  CONSTRAINT [DF_DN_IsActive]  DEFAULT ((1)) FOR [IsCancel]
GO
ALTER TABLE [dbo].[tbtrx_debetnote] ADD  DEFAULT ((0)) FOR [DNAmount]
GO
ALTER TABLE [dbo].[tbtrx_debetnote] ADD  DEFAULT ('') FOR [FeeDesc]
GO
ALTER TABLE [dbo].[tbtrx_debetnote] ADD  DEFAULT ((0)) FOR [FeeAmount]
GO
ALTER TABLE [dbo].[tbtrx_debetnote] ADD  DEFAULT ((0)) FOR [PPHPct]
GO
ALTER TABLE [dbo].[tbtrx_debetnote] ADD  DEFAULT ((0)) FOR [PPHAmt]
GO
ALTER TABLE [dbo].[tbtrx_debetnote] ADD  DEFAULT ('') FOR [statusPPH]
GO
ALTER TABLE [dbo].[tbtrx_debetnote] ADD  DEFAULT ((0)) FOR [VATExpired]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_his] ADD  CONSTRAINT [DF__tbtrx_deb__DNAmo__51851410]  DEFAULT ((0)) FOR [DNAmount]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_his] ADD  CONSTRAINT [DF__tbtrx_deb__FeeDe__52793849]  DEFAULT ('') FOR [FeeDesc]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_his] ADD  CONSTRAINT [DF__tbtrx_deb__FeePc__536D5C82]  DEFAULT ((0)) FOR [FeePct]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_his] ADD  CONSTRAINT [DF__tbtrx_deb__FeeAm__546180BB]  DEFAULT ((0)) FOR [FeeAmount]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_his] ADD  DEFAULT ((0)) FOR [PPHPct]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_his] ADD  DEFAULT ('') FOR [statusPPH]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_his] ADD  DEFAULT ((0)) FOR [PPHAmt]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_overbudget_status] ADD  DEFAULT ((0)) FOR [investment]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_overbudget_status] ADD  DEFAULT ((0)) FOR [investmentForDN]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_overbudget_status] ADD  DEFAULT ('') FOR [StatusApprovalCodeRecon]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_overbudget_status] ADD  DEFAULT ((0)) FOR [totalClaim]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_overbudget_status] ADD  DEFAULT ((0)) FOR [isOverBudget]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_overbudget_status] ADD  DEFAULT ((0)) FOR [overBudgetStatus]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_workflow_change] ADD  DEFAULT ((0)) FOR [VATExpiredOld]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_workflow_change] ADD  DEFAULT ((0)) FOR [VATExpired]
GO
ALTER TABLE [dbo].[tbtrx_debetnote_workflow_change] ADD  DEFAULT ('') FOR [statusField]
GO
ALTER TABLE [dbo].[tbtrx_invoice_header] ADD  DEFAULT ('') FOR [dnPeriod]
GO
ALTER TABLE [dbo].[tbtrx_invoice_header] ADD  DEFAULT ((0)) FOR [categoryId]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_PromoPlanId]  DEFAULT ((0)) FOR [PromoPlanId]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_AllocationId]  DEFAULT ((0)) FOR [AllocationId]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_AllocationRefId]  DEFAULT ('') FOR [AllocationRefId]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_CategoryShortDesc]  DEFAULT ('') FOR [CategoryShortDesc]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_PrincipalShortDesc]  DEFAULT ('') FOR [PrincipalShortDesc]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_BudgetMasterId]  DEFAULT ((0)) FOR [BudgetMasterId]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_CategoryId]  DEFAULT ((0)) FOR [CategoryId]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_SubCategoryId]  DEFAULT ((0)) FOR [SubCategoryId]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_ActivityId]  DEFAULT ((0)) FOR [ActivityId]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_NormalSales]  DEFAULT ((0)) FOR [NormalSales]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_IncrSales]  DEFAULT ((0)) FOR [IncrSales]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_Roi]  DEFAULT ((0)) FOR [Roi]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_tbtrx_promo_CostRatio]  DEFAULT ((0)) FOR [CostRatio]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_Promo_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF_Promo_IsLocked]  DEFAULT ((1)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF__tbtrx_pro__IsClo__1CA7377D]  DEFAULT ((0)) FOR [IsClose]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF__tbtrx_pro__actua__1D9B5BB6]  DEFAULT ((0)) FOR [actual_sales]
GO
ALTER TABLE [dbo].[tbtrx_promo] ADD  CONSTRAINT [DF__tbtrx_pro__late___1E8F7FEF]  DEFAULT ((0)) FOR [late_submission_day]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status] ADD  CONSTRAINT [DF_tbtrx_promo_budget_status_Approved]  DEFAULT ((0)) FOR [Approved]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status] ADD  CONSTRAINT [DF_tbtrx_promo_budget_status_Deployed]  DEFAULT ((0)) FOR [Deployed]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status] ADD  CONSTRAINT [DF_tbtrx_promo_budget_status_Budget_Amount]  DEFAULT ((0)) FOR [BudgetAmount]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_send_email_failed] ADD  DEFAULT ((0)) FOR [promoId]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_send_email_failed] ADD  DEFAULT ('') FOR [promoRefId]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_send_email_failed] ADD  DEFAULT ((0)) FOR [cost]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_summary_email] ADD  CONSTRAINT [DF__tbtrx_pro__batch__280708EC]  DEFAULT ('') FOR [batchId]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_summary_email] ADD  CONSTRAINT [DF__tbtrx_prom__step__28FB2D25]  DEFAULT ((0)) FOR [step]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_summary_email] ADD  CONSTRAINT [DF__tbtrx_pro__proce__29EF515E]  DEFAULT ('') FOR [processSummary]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_summary_email] ADD  CONSTRAINT [DF__tbtrx_pro__below__2AE37597]  DEFAULT ((0)) FOR [below5BioQty]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_summary_email] ADD  CONSTRAINT [DF__tbtrx_pro__below__2BD799D0]  DEFAULT ((0)) FOR [below5Bio]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_summary_email] ADD  CONSTRAINT [DF__tbtrx_pro__above__2CCBBE09]  DEFAULT ((0)) FOR [above5BioQty]
GO
ALTER TABLE [dbo].[tbtrx_promo_budget_status_summary_email] ADD  CONSTRAINT [DF__tbtrx_pro__above__2DBFE242]  DEFAULT ((0)) FOR [above5Bio]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator] ADD  CONSTRAINT [DF_tbtrx_promo_calculator_Baseline]  DEFAULT ((0)) FOR [Baseline]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator] ADD  CONSTRAINT [DF_tbtrx_promo_calculator_Uplift]  DEFAULT ((0)) FOR [Uplift]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator] ADD  CONSTRAINT [DF_tbtrx_promo_calculator_TotalSales]  DEFAULT ((0)) FOR [TotalSales]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator] ADD  CONSTRAINT [DF_tbtrx_promo_calculator_SalesContribution]  DEFAULT ((0)) FOR [SalesContribution]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator] ADD  CONSTRAINT [DF_tbtrx_promo_calculator_StoresCoverage]  DEFAULT ((0)) FOR [StoresCoverage]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator] ADD  CONSTRAINT [DF_tbtrx_promo_calculator_RedemptionRate]  DEFAULT ((0)) FOR [RedemptionRate]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator] ADD  CONSTRAINT [DF_tbtrx_promo_calculator_CR]  DEFAULT ((0)) FOR [CR]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator] ADD  CONSTRAINT [DF_tbtrx_promo_calculator_Cost]  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator_recon] ADD  DEFAULT ((0)) FOR [Baseline]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator_recon] ADD  DEFAULT ((0)) FOR [Uplift]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator_recon] ADD  DEFAULT ((0)) FOR [TotalSales]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator_recon] ADD  DEFAULT ((0)) FOR [SalesContribution]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator_recon] ADD  DEFAULT ((0)) FOR [StoresCoverage]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator_recon] ADD  DEFAULT ((0)) FOR [RedemptionRate]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator_recon] ADD  DEFAULT ((0)) FOR [CR]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator_recon] ADD  DEFAULT ((0)) FOR [ROI]
GO
ALTER TABLE [dbo].[tbtrx_promo_calculator_recon] ADD  DEFAULT ((0)) FOR [Cost]
GO
ALTER TABLE [dbo].[tbtrx_promo_mechanism_smartcode] ADD  CONSTRAINT [DF_tbtrx_promo_mechanism_smartcode_Baseline]  DEFAULT ((0)) FOR [baseline]
GO
ALTER TABLE [dbo].[tbtrx_promo_mechanism_smartcode] ADD  CONSTRAINT [DF_tbtrx_promo_mechanism_smartcode_Uplift]  DEFAULT ((100)) FOR [uplift]
GO
ALTER TABLE [dbo].[tbtrx_promo_mechanism_smartcode] ADD  CONSTRAINT [DF_tbtrx_promo_mechanism_smartcode_TotalSales]  DEFAULT ((0)) FOR [totalSales]
GO
ALTER TABLE [dbo].[tbtrx_promo_mechanism_smartcode] ADD  CONSTRAINT [DF_tbtrx_promo_mechanism_smartcode_SalesContribution]  DEFAULT ((100)) FOR [salesContribution]
GO
ALTER TABLE [dbo].[tbtrx_promo_mechanism_smartcode] ADD  CONSTRAINT [DF_tbtrx_promo_mechanism_smartcode_StoresCoverage]  DEFAULT ((100)) FOR [storesCoverage]
GO
ALTER TABLE [dbo].[tbtrx_promo_mechanism_smartcode] ADD  CONSTRAINT [DF_tbtrx_promo_mechanism_smartcode_RedemptionRate]  DEFAULT ((100)) FOR [redemptionRate]
GO
ALTER TABLE [dbo].[tbtrx_promo_mechanism_smartcode] ADD  CONSTRAINT [DF_tbtrx_promo_mechanism_smartcode_CR]  DEFAULT ((100)) FOR [cr]
GO
ALTER TABLE [dbo].[tbtrx_promo_mechanism_smartcode] ADD  CONSTRAINT [DF_tbtrx_promo_mechanism_smartcode_Cost]  DEFAULT ((0)) FOR [cost]
GO
ALTER TABLE [dbo].[tbtrx_promo_plan] ADD  CONSTRAINT [DF_PromoPlan_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_promo_planning] ADD  CONSTRAINT [DF_tbtrx_promo_planning_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbtrx_promo_planning] ADD  CONSTRAINT [DF_tbtrx_promo_planning_IsLocked]  DEFAULT ((1)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[tbtrx_promo_planning] ADD  DEFAULT ((0)) FOR [late_submission_day]
GO
ALTER TABLE [dbo].[tbtrx_promo_skp_validate] ADD  DEFAULT ((0)) FOR [skpstatus]
GO
ALTER TABLE [dbo].[tbtrx_promo_skp_validate_deleted] ADD  DEFAULT ((0)) FOR [skpstatus]
GO
ALTER TABLE [dbo].[tbtrx_promo_submission_static] ADD  DEFAULT ((0)) FOR [OnTime]
GO
ALTER TABLE [dbo].[tbtrx_ss_conversion_rate] ADD  CONSTRAINT [DF__tbtrx_ss___subCh__471D5F8D]  DEFAULT ((0)) FOR [subChannelId]
GO
ALTER TABLE [dbo].[tbtrx_ss_tt] ADD  CONSTRAINT [DF_tbtrx_ss_tt_tt]  DEFAULT ((0)) FOR [tt]
GO
ALTER TABLE [dbo].[tbtrx_ss_tt] ADD  CONSTRAINT [DF_tbtrx_ss_tt_ssvalue]  DEFAULT ((0)) FOR [ssvalue]
GO
ALTER TABLE [dbo].[tbtrx_ss_tt] ADD  CONSTRAINT [DF_tbtrx_ss_tt_approvalstatus]  DEFAULT ((0)) FOR [approvalstatus]
GO
ALTER TABLE [dbo].[tbtrx_ss_tt] ADD  CONSTRAINT [DF_tbtrx_ss_tt_deploymentstatus]  DEFAULT ((0)) FOR [deploymentstatus]
GO
ALTER TABLE [dbo].[tbxml_SAP_upload] ADD  DEFAULT ((0)) FOR [isDeleted]
GO
ALTER TABLE [dbo].[tbmst_brand]  WITH CHECK ADD  CONSTRAINT [fk_pcp] FOREIGN KEY([PrincipalId])
REFERENCES [dbo].[tbmst_principal] ([Id])
GO
ALTER TABLE [dbo].[tbmst_brand] CHECK CONSTRAINT [fk_pcp]
GO
ALTER TABLE [dbo].[tbmst_product]  WITH CHECK ADD  CONSTRAINT [fk_brandprd] FOREIGN KEY([BrandId])
REFERENCES [dbo].[tbmst_brand] ([Id])
GO
ALTER TABLE [dbo].[tbmst_product] CHECK CONSTRAINT [fk_brandprd]
GO
ALTER TABLE [dbo].[tbmst_product]  WITH CHECK ADD  CONSTRAINT [fk_pcpprd] FOREIGN KEY([PrincipalId])
REFERENCES [dbo].[tbmst_principal] ([Id])
GO
ALTER TABLE [dbo].[tbmst_product] CHECK CONSTRAINT [fk_pcpprd]
GO
ALTER TABLE [dbo].[tbmst_subaccount]  WITH CHECK ADD  CONSTRAINT [fk_account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[tbmst_account] ([Id])
GO
ALTER TABLE [dbo].[tbmst_subaccount] CHECK CONSTRAINT [fk_account]
GO
ALTER TABLE [dbo].[tbmst_subcategory]  WITH CHECK ADD  CONSTRAINT [fk_categorysub] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[tbmst_category] ([Id])
GO
ALTER TABLE [dbo].[tbmst_subcategory] CHECK CONSTRAINT [fk_categorysub]
GO
ALTER TABLE [dbo].[tbmst_subchannel]  WITH CHECK ADD  CONSTRAINT [fk_channel] FOREIGN KEY([ChannelId])
REFERENCES [dbo].[tbmst_channel] ([Id])
GO
ALTER TABLE [dbo].[tbmst_subchannel] CHECK CONSTRAINT [fk_channel]
GO
ALTER TABLE [dbo].[tbset_matrix_approval_process_detail]  WITH CHECK ADD  CONSTRAINT [FK_tbset_matrix_approval_process_detail_tbset_matrix_approval_process] FOREIGN KEY([ProcessId])
REFERENCES [dbo].[tbset_matrix_approval_process] ([Id])
GO
ALTER TABLE [dbo].[tbset_matrix_approval_process_detail] CHECK CONSTRAINT [FK_tbset_matrix_approval_process_detail_tbset_matrix_approval_process]
GO
ALTER TABLE [dbo].[tbset_userrights]  WITH CHECK ADD  CONSTRAINT [tbset_userrights_FK] FOREIGN KEY([usergroupid])
REFERENCES [dbo].[tbset_usergroup] ([usergroupid])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbset_userrights] CHECK CONSTRAINT [tbset_userrights_FK]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0-freeText;1-selectFromMaster' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbset_promo_mechanism_input_method', @level2type=N'COLUMN',@level2name=N'inputMethod'
GO
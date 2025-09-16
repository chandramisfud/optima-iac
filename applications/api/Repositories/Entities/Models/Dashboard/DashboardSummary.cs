namespace Repositories.Entities.Models.Dashboard
{
    // kpiscoring/all
    public class KPIScoringDashboardSummary
    {
        public int DistributorId { get; set; }
        public string? DistributorShortDesc { get; set; }
        public int EntityId { get; set; }
        public string? PrincipalShortDesc { get; set; }
        public string? PromoPlanCreateBy { get; set; }
        public string? PromoPlanCreateOn { get; set; }
        public string? RegionDesc { get; set; }
        public int SubActivityTypeId { get; set; }
        public string? SubActivityType { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public int ActivityId { get; set; }
        public string? Activity { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public DateTime StartPlanning { get; set; }
        public DateTime EndPlanning { get; set; }
        public string? ChannelDesc { get; set; }
        public string? SubChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? TSCode { get; set; }
        public string? BrandDesc { get; set; }
        public string? SKUDesc { get; set; }
        public string? SourceOfPromo { get; set; }
        public string? PromotionType { get; set; }
        public string? ActivityDesc { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public double Investment { get; set; }
        public double BaselineSales { get; set; }
        public double UpliftSales { get; set; }
        public double TotalSales { get; set; }
        public double UpliftSalesPersen { get; set; }
        public double ROIPersen { get; set; }
        public int PromodId { get; set; }
        public string? PromoRefId { get; set; }
        public string? Creator { get; set; }
        public DateTime PromoCreateOn { get; set; }
        public double PromoAmount { get; set; }
        public DateTime today { get; set; }
        public string? Quater2Start { get; set; }
        public DateTime today60 { get; set; }
        public double DaysOfCreation { get; set; }
        public double PeriodStartMatching { get; set; }
        public double PeriodEndMatcing { get; set; }
        public double Amount { get; set; }
        public double PeriodOfStart { get; set; }
        public double PeriodOfEnd { get; set; }
        public double PromoPlanSubmissionDays { get; set; }
        public string? PromoPlanSubmittedBfrQuarterStart90 { get; set; }
        public string? AvailabilityInOptima { get; set; }
        public string? OptimaCreationBrfActivityStart60 { get; set; }
        public string? OptimaPeriodMatch { get; set; }
        public string? OptimaAmountMatch { get; set; }
        public string? OptimaDescMatch { get; set; }
        public string? OptimaMechanismMatch { get; set; }
        public string? OptimaDescAndMechanismMatch { get; set; }
        public string? SKPAvailability { get; set; }
        public string? SKPAvailBfrActivityStart60 { get; set; }
        public string? SKPPeriodMatch { get; set; }
        public string? SKPAmountMatch { get; set; }
        public string? SKPMechanismMatch { get; set; }
        public string? SKPSigned7 { get; set; }
        public string? ActivityAuditCompliance { get; set; }
        public string? ExtendedPeriodMatch { get; set; }
        public string? ExtendedAmountMatch { get; set; }
        public string? ExtendedMechanismMatch { get; set; }
        public string? ReconNKA { get; set; }
        public string? AdjustBudgetRecon { get; set; }
        public string? FeedbackOnPostROI { get; set; }
        public string? AgingDNBySales { get; set; }
    }

    // kpiscoring/approver
    public class KPIScoringApproverDashboardSummary
    {
        public List<KPIApproverResult1>? KPIApproverResults1 { get; set; }
        public List<KPIApproverResult2>? KPIApproverResults2 { get; set; }
    }
    // kpiscoring/approver
    public class KPIApproverResult1
    {
        public double approval_request { get; set; }
        public double tot_days { get; set; }
        public double avg_approval_response_time { get; set; }
        public double promo_approval_sla_max_3days { get; set; }
        public double promo_approval_sla_max_3days_subscore { get; set; }
        public double promo_approval_sla_max_3days_bobot { get; set; }
        public double promo_approval_pct { get; set; }
    }
    // kpiscoring/approver
    public class KPIApproverResult2
    {
        public string? userapprover_top { get; set; }
        public double score_top { get; set; }
        public double num_of_optima_top { get; set; }
        public double avg_days_top { get; set; }
        public string? userapprover_bottom { get; set; }
        public double score_bottom { get; set; }
        public double num_of_optima_bottom { get; set; }
        public double avg_days_bottom { get; set; }
    }

    // kpiscoring/dashboard
    public class KPIScoringDashboard_DashboardSummary
    {
        public double OptimaAmountMatch_Score { get; set; }
        public double OptimaAmountMatch_bobot { get; set; }
        public double OptimaCreationBrfActivityStart60_Score { get; set; }
        public double OptimaCreationBrfActivityStart60_bobot { get; set; }
        public double ExtendedMechanismMatch_Score { get; set; }
        public double ExtendedMechanismMatch_bobot { get; set; }
        public double ExtendedAmountMatch_Score { get; set; }
        public double ExtendedAmountMatch_bobot { get; set; }
        public double SKPSigned7_Score { get; set; }
        public double SKPSigned7_Bobot { get; set; }
        public double SubscorePromoPlan_bobot { get; set; }
        public double SubscoreAccuracy_OptimaInput_vs_PromoPlan_bobot { get; set; }
        public double SubscoreAccuracy_SKP_vs_Optima_bobot { get; set; }
        public double SubscoreReconMonitoring_bobot { get; set; }
        public double ScoreboardPromoPlan { get; set; }
        public double ScoreboardAccuracy_OptimaInput_vs_PromoPlan { get; set; }
        public double ScoreboardAccuracy_SKP_vs_Optima { get; set; }
        public double ScoreboardReconMonitoring { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? JobTitle { get; set; }
        public double PromoPlanEvaluated { get; set; }
        public double PromoPlanSubmittedBfrQuarterStart90_Score { get; set; }
        public double PromoPlanSubmittedBfrQuarterStart90_bobot { get; set; }
        public double OptimaPeriodMatch_Score { get; set; }
        public double OptimaPeriodMatch_bobot { get; set; }
        public double OptimaDescAndMechanismMatch_Score { get; set; }
        public double OptimaDescAndMechanismMatch_bobot { get; set; }
        public double PromoPlanSubmittedBfrQuarterStart90_Score_pct { get; set; }
        public double ScoreboardAccuracy_OptimaInput_vs_PromoPlan_pct { get; set; }
        public double ScoreboardAccuracy_SKP_vs_Optima_pct { get; set; }
        public double ScoreboardReconMonitoring_pct { get; set; }
        public double Overall_Score_pct { get; set; }
        public double SKPPeriodMatch_Score { get; set; }
        public double SKPPeriodMatch_bobot { get; set; }
        public double SKPDescAndMechanismMatch_Score { get; set; }
        public double SKPDescAndMechanismMatch_bobot { get; set; }
        public double SKPAmountMatch_Score { get; set; }
        public double SKPAmountMatch_bobot { get; set; }
        public double SKPPeriodMatch_pct { get; set; }
        public double SKPDescAndMechanismMatch_pct { get; set; }
        public double SKPAmountMatch_pct { get; set; }
        public double SKPDraftH60_Score { get; set; }
        public double SKPDraftH60_bobot { get; set; }
        public double SKPDraftH60_pct { get; set; }
        public double OptimaPeriodMatch_pct { get; set; }
        public double OptimaDescAndMechanismMatch_pct { get; set; }
        public double OptimaAmountMatch_pct { get; set; }
        public double OptimaCreationBrfActivityStart60_pct { get; set; }
        public double ReconNKA_Score { get; set; }
        public double ReconNKA_bobot { get; set; }
        public double AdjustBudgetRecon_Score { get; set; }
        public double AdjustBudgetRecon_bobot { get; set; }
        public double FeedbackOnPostROI_Score { get; set; }
        public double FeedbackOnPostROI_bobot { get; set; }
        public double Recon_pct { get; set; }
        public double AgingDNBySales_Score { get; set; }
        public double AgingDNBySales_bobot { get; set; }
    }

    // kpiscoreboard/detail/all
    public class KPIScoringDetailDashboardSummary
    {
        public string? Period { get; set; }
        public int PromoPlanID { get; set; }
        public string? PromoPlanRefID { get; set; }
        public string? TSCode { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public int DistributorId { get; set; }
        public string? Distributor { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategory { get; set; }
        public int ActivityId { get; set; }
        public string? Activity { get; set; }
        public int SubActivityId { get; set; }
        public string? SubActivity { get; set; }
        public string? ActivityDesc { get; set; }
        public string? RegionDesc { get; set; }
        public string? ChannelDesc { get; set; }
        public string? SubChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? BrandDesc { get; set; }
        public string? SKUDesc { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public DateTime StartPlanning { get; set; }
        public DateTime EndPlanning { get; set; }
        public double NormalSales { get; set; }
        public double SalesInc { get; set; }
        public double Investment { get; set; }
        public double Roi { get; set; }
        public double CostRatio { get; set; }
        public string? Creator { get; set; }
        public DateTime CreationDate { get; set; }
        public string? LastStatus { get; set; }
        public string? PromoRefId { get; set; }
        public string? CancelNotes { get; set; }
        public string? PIC { get; set; }
        public string? StatusPromo { get; set; }
        public int PeriodStart { get; set; }
        public int PeriodEnd { get; set; }
        public double InvestmentCreation { get; set; }
        public DateTime PromoCreation { get; set; }
        public int PromoPlanCreation { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? PromoActivityDesc { get; set; }
        public string? PromoPlanSubmittedBfrQuarterStart90 { get; set; }
        public int PeriodOfStart { get; set; }
        public int PeriodOfEnd { get; set; }
        public string? check { get; set; }
        public string? OptimaPeriodMatch { get; set; }
        public string? OptimaMechanismMatch { get; set; }
        public string? OptimaDescMatch { get; set; }
        public string? OptimaDescAndMechanismMatch { get; set; }
        public string? OptimaAmountMatch { get; set; }
        public string? OptimaCreationBrfActivityStart60 { get; set; }

    }

    // kpiscoring/league
    public class KPIScoringLeagueDashboardSummary
    {
        public List<OverallScore>? OverallScores { get; set; }
        public List<ChannelDescription>? ChannelDescriptions { get; set; }
    }
    // kpiscoring/league
    public class OverallScore
    {
        public string? ChannelDesc { get; set; }
        public double PromoPlanSubmittedBfrQuarterStart90_Score_pct { get; set; }
        public double ScoreboardAccuracy_OptimaInput_vs_PromoPlan_pct { get; set; }
        public double ScoreboardAccuracy_SKP_vs_Optima_pct { get; set; }
        public double ScoreboardReconMonitoring_pct { get; set; }
        public double Overall_Score_pct { get; set; }
    }
    // kpiscoring/league
    public class ChannelDescription
    {
        public string? channeldesc { get; set; }
    }

    // kpiscoring/standing
    public class KPIScoringStandingDashboardSummary
    {
        public List<Result1>? Result1s { get; set; }
        public List<Result2>? Result2s { get; set; }
        public List<Result3>? Result3s { get; set; }
        public List<Result4>? Result4s { get; set; }
        public List<Result5>? Result5s { get; set; }

    }
    // kpiscoring/standing
    public class Result1
    {
        public int INumTop { get; set; }
        public string? ChannelDescTop { get; set; }
        public string? Top_Creator { get; set; }
        public double TopTotalScore { get; set; }
        public int INumBot { get; set; }
        public string? ChannelDescBot { get; set; }
        public string? Bottom_Creator { get; set; }
        public double BotTotalScore { get; set; }

    }
    // kpiscoring/standing
    public class Result2
    {
        public string? top_creator { get; set; }
        public double total_score { get; set; }
    }
    // kpiscoring/standing
    public class Result3
    {
        public string? top_creator { get; set; }
        public double total_score { get; set; }
    }
    // kpiscoring/standing
    public class Result4
    {
        public string? top_creator { get; set; }
        public double total_score { get; set; }
    }
    // kpiscoring/standing
    public class Result5
    {
        public string? top_creator { get; set; }
        public double total_score { get; set; }
    }

    // kpiscoring/summaries
    public class KPIScoringSummaryDashboardSummary
    {
        public string? PromoPlanCreateBy { get; set; }
        public string? Creator { get; set; }
        public string? PromoPlanEvaluated { get; set; }
        public string? PromoPlanSubmittedBfrQuarterStart90_Y { get; set; }
        public string? PromoPlanSubmittedBfrQuarterStart90_N { get; set; }
        public string? PromoPlanSubmittedBfrQuarterStart90_Total { get; set; }
        public string? PromoPlanSubmittedBfrQuarterStart90_Score { get; set; }
        public string? OptimaPeriodMatch_Y { get; set; }
        public string? OptimaPeriodMatch_N { get; set; }
        public string? OptimaPeriodMatch_Total { get; set; }
        public string? OptimaPeriodMatch_Score { get; set; }
        public string? OptimaDescAndMechanismMatch_Y { get; set; }
        public string? OptimaDescAndMechanismMatch_N { get; set; }
        public string? OptimaDescAndMechanismMatch_Total { get; set; }
        public string? OptimaDescAndMechanismMatch_Score { get; set; }
        public string? OptimaAmountMatch_Y { get; set; }
        public string? OptimaAmountMatch_N { get; set; }
        public string? OptimaAmountMatch_Total { get; set; }
        public string? OptimaAmountMatch_Score { get; set; }
        public string? OptimaCreationBrfActivityStart60_Y { get; set; }
        public string? OptimaCreationBrfActivityStart60_N { get; set; }
        public string? OptimaCreationBrfActivityStart60_Total { get; set; }
        public string? OptimaCreationBrfActivityStart60_Score { get; set; }
        public string? SKPPeriodMatch_Y { get; set; }
        public string? SKPPeriodMatch_N { get; set; }
        public string? SKPPeriodMatch_Total { get; set; }
        public string? SKPPeriodMatch_Score { get; set; }
        public string? SKPAmountMatch_Y { get; set; }
        public string? SKPAmountMatch_N { get; set; }
        public string? SKPAmountMatch_Total { get; set; }
        public string? SKPAmountMatch_Score { get; set; }
        public string? SKPMechanismMatch_Y { get; set; }
        public string? SKPMechanismMatch_N { get; set; }
        public string? SKPMechanismMatch_Total { get; set; }
        public string? SKPMechanismMatch_Score { get; set; }
        public string? SKPSigned7_Y { get; set; }
        public string? SKPSigned7_N { get; set; }
        public string? SKPSigned7_Total { get; set; }
        public string? SKPSigned7_Score { get; set; }
        public string? ActivityAuditCompliance_Y { get; set; }
        public string? ActivityAuditCompliance_N { get; set; }
        public string? ActivityAuditCompliance_Total { get; set; }
        public string? ActivityAuditCompliance_Score { get; set; }
        public string? ReconNKA_Y { get; set; }
        public string? ReconNKA_N { get; set; }
        public string? ReconNKA_Total { get; set; }
        public string? ReconNKA_Score { get; set; }
        public string? AdjustBudgetRecon_Y { get; set; }
        public string? AdjustBudgetRecon_N { get; set; }
        public string? AdjustBudgetRecon_Total { get; set; }
        public string? AdjustBudgetRecon_Score { get; set; }
        public string? FeedbackOnPostROI_Y { get; set; }
        public string? FeedbackOnPostROI_N { get; set; }
        public string? FeedbackOnPostROI_Total { get; set; }
        public string? FeedbackOnPostROI_Score { get; set; }
        public string? AgingDNBySales_Y { get; set; }
        public string? AgingDNBySales_N { get; set; }
        public string? AgingDNBySales_Total { get; set; }
        public string? AgingDNBySales_Score { get; set; }
        public string? Total_Score { get; set; }
        public string? JobTitle1 { get; set; }
        public string? JobTitle2 { get; set; }
    }

}

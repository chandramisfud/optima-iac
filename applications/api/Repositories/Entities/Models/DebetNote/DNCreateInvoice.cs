using System.ComponentModel.DataAnnotations;

namespace Repositories.Entities.Models.DN
{
    public class DNCreateInvoice
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? InvoiceDate { get; set; }
        public string? InvoiceDesc { get; set; }
        public decimal DPP { get; set; }
        public decimal PPN { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string? Terbilang { get; set; }
        public string? ClaimManager { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorDesc { get; set; }
        public string? DistributorAddress { get; set; }
        public string? DistributorPhone { get; set; }
        public string? DistributorFax { get; set; }
        public string? DistributorNPWP { get; set; }
        public int EntityId { get; set; }
        public string? EntityDesc { get; set; }
        public string? EntityAddress { get; set; }
        public string? EntityNPWP { get; set; }
        public string? EntityUp { get; set; }
        public IList<InvoiceDetail>? DetailDN { get; set; }
        public IList<DNDetailforInvoice>? StandartDetailDN { get; set; }
        public string? TaxLevel { get; set; }
        public string? TaxLevelDesc { get; set; }
        public string? SalesValidationStatus { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public double ppnpct { get; set; }
        public string? dnPeriod { get; set; }
        public int categoryId { get; set; }
        public string? categoryDesc { get; set; }

    }
    public class InvoiceDetail
    {
        public int No { get; set; }
        public string? PromoNumber { get; set; }
        public string? DNNumber { get; set; }
        public string? ActivityDesc { get; set; }
        public decimal TotalClaim { get; set; }


    }
    public class DNDetailforInvoice
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public string? ActivityDesc { get; set; }
        public decimal DPP { get; set; }
        public decimal PPNAmt { get; set; }
        public decimal TotalClaim { get; set; }
        public decimal TotalPaid { get; set; }
        public string? LastStatus { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsFreeze { get; set; }
        public bool IsOverBudget { get; set; }
        public bool IsDNManual { get; set; }
        public bool IsDNPromo { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? approver_userid { get; set; }
        public string? approver_username { get; set; }
        public string? approver_email { get; set; }
        public string? InternalOrderNumber { get; set; }
        public string? sp_ho { get; set; }
        public string? sp_principal { get; set; }
        public string? EntityId { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? MemDocNo { get; set; }
        public string? IntDocNo { get; set; }
        public string? TaxLevel { get; set; }
        public int docCount { get; set; }
        public string? Notes { get; set; }
        public string? MaterialNumber { get; set; }
        public double pphPct { get; set; }
        public double pphAmt { get; set; }
        public string? StatusSalesCode { get; set; }
        public string? StatusSales { get; set; }
        public DateTime StatusSalesOn { get; set; }
        public string? StatusSalesBy { get; set; }
        public DateTime StatusSalesDistOn { get; set; }
        public string? StatusSalesDistBy { get; set; }
        public string? StatusSalesNotes { get; set; }
        public string? remarkSales { get; set; }
        public string? SalesValidationStatus { get; set; }
        public DateTime SalesValidationStatusOn { get; set; }
        public int SubAccountId { get; set; }
        public string? SubAccount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? validate_by_finance_by { get; set; }
        public DateTime validate_by_finance_on { get; set; }
        public string? validate_by_sales_by { get; set; }
        public DateTime validate_by_sales_on { get; set; }
        public string? confirm_paid_by { get; set; }
        public DateTime confirm_paid_on { get; set; }
        public int AccountId { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public bool VATExpired { get; set; }
        public bool DNVATExpired { get; set; }
        public bool Original_Invoice_from_retailers { get; set; }
        public bool Tax_Invoice { get; set; }
        public bool Promotion_Agreement_Letter { get; set; }
        public bool Trading_Term { get; set; }
        public bool Sales_Data { get; set; }
        public bool Copy_of_Mailer { get; set; }
        public bool Copy_of_Photo_Doc { get; set; }
        public bool List_of_Transfer { get; set; }
        public string? validate_by_finance_username { get; set; }
        public string? FinanceValidationStatus { get; set; }
        public string? FinanceValidationStatusOn { get; set; }

        // added: March 22 by AND #868
        public double Remaining { get; set; }
        public string? dnCategory { get; set; }

    }
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string? Desc { get; set; }
        public decimal DPPAmount { get; set; }
        public decimal PPNpct { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string? RefId { get; set; }
        public int DistributorId { get; set; }
        public int EntityId { get; set; }
        public string? UserId { get; set; }
        public IList<DNId>? DnId { get; set; }
        public string? TaxLevel { get; set; }

    }
    public class SelectTaxLevel
    {
        public int Id { get; set; }
        public string? MaterialNumber { get; set; }
        public string? Description { get; set; }
        public string? WHT_Type { get; set; }
        public string? WHT_Code { get; set; }
        public string? Purpose { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
        public string? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public string? CreateByName { get; set; }
        public string? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedByName { get; set; }
        public int IsDelete { get; set; }
        public string? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteByName { get; set; }
        public string? CreatedEmail { get; set; }
        public string? DeleteEmail { get; set; }

        // added by andRie, May 30 2023, #878
        public decimal PPHPct { get; set; }
        public decimal PPNPct { get; set; }
    }
    public class DNCreateInvoiceEntityList
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class DNGetbyIdforCreateInvoice
    {
        public bool IsDNPromo { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? Periode { get; set; }
        public int EntityId { get; set; }
        public string? AccountDesc { get; set; }
        public string? EntityShortDesc { get; set; }
        public string? EntityLongDesc { get; set; }
        public string? EntityUp { get; set; }
        public string? EntityAddress { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorShortDesc { get; set; }
        public string? DistributorLongDesc { get; set; }
        public string? ActivityDesc { get; set; }
        public int AccountId { get; set; }
        public int PromoId { get; set; }
        public string? PromoRefId { get; set; }
        public string? StartPromo { get; set; }
        public string? EndPromo { get; set; }
        public string? IntDocNo { get; set; }
        public string? MemDocNo { get; set; }
        public decimal DPP { get; set; }
        public double DNAmount { get; set; }
        public string? FeeDesc { get; set; }
        public double FeePct { get; set; }
        public double FeeAmount { get; set; }
        public decimal PPNPct { get; set; }
        public DateTime DeductionDate { get; set; }
        public string? UserId { get; set; }
        public bool IsFreeze { get; set; }
        public string? approver_userid { get; set; }
        public string? approver_username { get; set; }
        public string? approver_email { get; set; }
        public IList<DNSellpoint>? sellpoint { get; set; }
        public IList<DNAttachment>? dnattachment { get; set; }
        public IList<DNDocCompleteness>? DNDocCompletenessHeader { get; set; }
        public string? InternalOrderNumber { get; set; }
        public string? TaxLevel { get; set; }
        public string? Notes { get; set; }
        public string? LastStatus { get; set; }
        public double pphPct { get; set; }
        public double pphAmt { get; set; }
        public string? statusPPH { get; set; }
        public string? StatusSalesNotes { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public int ChannelId { get; set; }
        public double PPNAmt { get; set; }
        public string? statusPPN { get; set; }
        //#835
        public string? DNCreator { get; set; }
    }
    public class DNRejectCreateInvoiceGlobalResponse
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
    }
    public class DNIdReadytoInvoiceArray
    {
        public int DNId { get; set; }
    }
    public class DNChangeStatusReadytoInvoice
    {
        public string? UserId { get; set; }
        public string? status { get; set; }
        public IList<DNIdReadytoInvoiceArray>? DNId { get; set; }
    }
    public class InvoicePrintDto
    {
        public int Id { get; set; }
        public string? RefId { get; set; }
        public string? InvoiceDate { get; set; }
        public string? InvoiceDesc { get; set; }
        public decimal DPP { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string? Terbilang { get; set; }
        public string? ClaimManager { get; set; }
        public int DistributorId { get; set; }
        public string? DistributorDesc { get; set; }
        public string? DistributorAddress { get; set; }
        public string? DistributorPhone { get; set; }
        public string? DistributorFax { get; set; }
        public string? DistributorNPWP { get; set; }
        public int EntityId { get; set; }
        public string? EntityDesc { get; set; }
        public string? EntityAddress { get; set; }
        public string? EntityNPWP { get; set; }
        public string? EntityUp { get; set; }
        public List<InvoiceDetailDNDto>? DetailDN { get; set; }
        public List<DNDetailforInvoice>? StandartDetailDN { get; set; }
        public string? TaxLevel { get; set; }
        public string? SalesValidationStatus { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public double ppnpct { get; set; }

        //#138, April 5 2024
        public double pphpct { get; set; }
        public string PPNLabel { get; set; } = String.Empty;
        public string PPHLabel { get; set; } = String.Empty;
        public decimal PPN { get; set; }

        public decimal PPH { get; set; }

    }
    public class InvoiceDetailDNDto
    {
        public int No { get; set; }
        public string? PromoNumber { get; set; }
        public string? DNNumber { get; set; }
        public string? ActivityDesc { get; set; }
        public decimal TotalClaim { get; set; }

    }
    public class DistributorforCreateInvoice
    {
        public int Id { get; set; }
        public string? ShortDesc { get; set; }
        public string? LongDesc { get; set; }
        public string? CompanyName { get; set; }

    }
    public class UserProfileDataByIdforDNCreateInvoice
    {
        //[Key]
        public string? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? pictureprofilefile { get; set; }
        public string? password { get; set; }
        public string? usergroupid { get; set; }
        public int userlevel { get; set; }
        public int isLogin { get; set; }
        public DateTime lastLogin { get; set; }
        public int cnt { get; set; }
        public string? groupmenupermission { get; set; }
        public string? department { get; set; }
        public string? jobtitle { get; set; }
        public string? contactinfo { get; set; }
        public IList<ListDistributorDNCreateInvoice>? distributorlist { get; set; }
        public int registered { get; set; }
        public string? code { get; set; }
        public DateTime password_change { get; set; }
        public string? token { get; set; }
        public DateTime token_date { get; set; }
        public string? userinput { get; set; }
        public DateTime dateinput { get; set; }
        public string? useredit { get; set; }
        public DateTime dateedit { get; set; }
        public int isdeleted { get; set; }
        public bool usernew { get; set; }
        public int loginFailedCount { get; set; }
        public DateTime loginFailedLastTime { get; set; }
        public double loginFreezeTime { get; set; }
        public int errCode { get; set; }
        public string? errMessage { get; set; }

    }
    public partial class ListDistributorDNCreateInvoice
    {
        [Key]
        public string? UserId { get; set; }
        public string? DistributorId { get; set; }
        public string? DistributorShortDesc { get; set; }
        public string? DistributorLongDesc { get; set; }
        public string? CompanyName { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public int IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }
}
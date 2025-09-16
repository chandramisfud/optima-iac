namespace Repositories.Entities.Models
{
    public class XMLGenerate
    {
        public string? EntityId { get; set; }
        public string? EntityDesc { get; set; }
        public string? OrderCBU { get; set; }
        public string? OrderLogicalMessage { get; set; }
        public string? SoldToCode { get; set; }
        public string? TypeOfSoldToCode { get; set; }
        public string? ShipToCode { get; set; }
        public string? TypeofShipToCode { get; set; }
        public string? OtherPartnerCode { get; set; }
        public string? PartnerFunctionOfOtherPartner { get; set; }
        public string? TypeOfOtherPartnerCode { get; set; }
        public string? OtherPartnerCode2 { get; set; }
        public string? PartnerFunctionOfOtherPartner2 { get; set; }
        public string? TypeOfOtherPartnerCode2 { get; set; }
        public string? OtherPartnerCode3 { get; set; }
        public string? PartnerFunctionOfOtherPartner3 { get; set; }
        public string? TypeOfOtherPartnerCode3 { get; set; }
        public string? PaymentTerm { get; set; }
        public string? PONumber { get; set; }
        public string? RequestedDeliveryDate { get; set; }
        public string? RequestedDeliveryTime { get; set; }
        public string? PODate { get; set; }
        public string? OrderType { get; set; }
        public string? SalesOrg { get; set; }
        public string? Channel { get; set; }
        public string? Division { get; set; }
        public string? DeliveryBlock { get; set; }
        public string? BillingBlock { get; set; }
        public string? ShippingCondition { get; set; }
        public string? ShippingType { get; set; }
        public string? Assignment { get; set; }
        public string? OrderReason { get; set; }
        public string? PurchaseOrderType { get; set; }
        public string? OrderModificationReason { get; set; }
        public string? ShipToPartysPurchaseOrderNumber { get; set; }
        public string? ShipToPartysPurchaseOrderDate { get; set; }
        public string? TypeDocumentInReferenceHeader { get; set; }
        public string? DocumentInReference { get; set; }
        public string? SoldToYourReference { get; set; }
        public string? ShipToYourReference { get; set; }
        public string? ShipToName1 { get; set; }
        public string? ShipToName2 { get; set; }
        public string? ShipToStreet { get; set; }
        public string? ShipToPostalCode { get; set; }
        public string? ShipToCity { get; set; }
        public string? ShipToCountry { get; set; }
        public string? TextType { get; set; }
        public string? DetailOfText { get; set; }
        public string? TextType2 { get; set; }
        public string? DetailOfText2 { get; set; }
        public string? ItemNumber { get; set; }
        public string? material { get; set; }
        public string? TypeOfMaterialCode { get; set; }
        public string? ItemCategory { get; set; }
        public double Quantity { get; set; }
        public string? UnitOfQuantity { get; set; }
        public string? PlannedGoodIssueDate { get; set; }
        public string? PriceCondition { get; set; }
        public string? PriceList { get; set; }
        public string? Sign { get; set; }
        public double Value { get; set; }
        public string? ConditionPricingUnit { get; set; }
        public string? UnitOfPrice { get; set; }
        public string? ConditionCurrency { get; set; }
        public string? Plant { get; set; }
        public string? StorageLocation { get; set; }
        public string? ItemUsage { get; set; }
        public string? BatchNumber { get; set; }
        public string? InternalOrder { get; set; }
        public string? TypeDocumentInReferenceLine { get; set; }
        public string? ItemInReference { get; set; }
        public string? ItemTextType { get; set; }
        public string? ItemTextLanguage { get; set; }
        public string? ItemDetailOfText { get; set; }
        public string? ItemTextType2 { get; set; }
        public string? ItemTextLanguage2 { get; set; }
        public string? ItemDetailOfText2 { get; set; }
        public string? OriginalId { get; set; }

    }
    public class EntityParam
    {
        public int entity { get; set; }
        public int[]? id { get; set; }

    }
    public class XMLGenerateErrorMessage
    {
        public bool error { get; set; }
        public string? message { get; set; }
        public int Id { get; set; }
        public string? RefId { get; set; }
        public int statuscode { get; set; }
        public string? userid_approver { get; set; }
        public string? username_approver { get; set; }
        public string? email_approver { get; set; }
        public string? userid_initiator { get; set; }
        public string? username_initiator { get; set; }
        public string? email_initiator { get; set; }
        public bool IsFullyApproved { get; set; }
        public bool IsFullyApprovedRecon { get; set; }
        public bool major_changes { get; set; }

    }
    public class XmlFlaggingBody
    {
        public string? userid { get; set; }
        public string? useremail { get; set; }
        public List<PoNumber>? PoNumber { get; set; }

    }
    public class PoNumber
    {
        public string? PONumber { get; set; }
        public string? OriginalId { get; set; }
        public int entityId { get; set; }
    }
    public class XmlGenerateAccrual
    {
        public int PrincipalId { get; set; }
        public string? PrincipalDesc { get; set; }
        public string? OrderCBU { get; set; }
        public string? OrderLogicalMessage { get; set; }
        public string? SoldToCode { get; set; }
        public string? TypeOfSoldToCode { get; set; }
        public string? ShipToCode { get; set; }
        public string? TypeofShipToCode { get; set; }
        public string? OtherPartnerCode { get; set; }
        public string? PartnerFunctionOfOtherPartner { get; set; }
        public string? TypeOfOtherPartnerCode { get; set; }
        public string? OtherPartnerCode2 { get; set; }
        public string? PartnerFunctionOfOtherPartner2 { get; set; }
        public string? TypeOfOtherPartnerCode2 { get; set; }
        public string? OtherPartnerCode3 { get; set; }
        public string? PartnerFunctionOfOtherPartner3 { get; set; }
        public string? TypeOfOtherPartnerCode3 { get; set; }
        public string? PaymentTerm { get; set; }
        public string? PONumber { get; set; }
        public string? RequestedDeliveryDate { get; set; }
        public string? RequestedDeliveryTime { get; set; }
        public string? PODate { get; set; }
        public string? OrderType { get; set; }
        public string? SalesOrg { get; set; }
        public string? Channel { get; set; }
        public string? Division { get; set; }
        public string? DeliveryBlock { get; set; }
        public string? BillingBlock { get; set; }
        public string? ShippingCondition { get; set; }
        public string? ShippingType { get; set; }
        public string? Assignment { get; set; }
        public string? OrderReason { get; set; }
        public string? PurchaseOrderType { get; set; }
        public string? OrderModificationReason { get; set; }
        public string? ShipToPartysPurchaseOrderNumber { get; set; }
        public string? ShipToPartysPurchaseOrderDate { get; set; }
        public string? TypeDocumentInReferenceHeader { get; set; }
        public string? DocumentInReference { get; set; }
        public string? SoldToYourReference { get; set; }
        public string? ShipToYourReference { get; set; }
        public string? ShipToName1 { get; set; }
        public string? ShipToName2 { get; set; }
        public string? ShipToStreet { get; set; }
        public string? ShipToPostalCode { get; set; }
        public string? ShipToCity { get; set; }
        public string? ShipToCountry { get; set; }
        public string? TextType { get; set; }
        public string? DetailOfText { get; set; }
        public string? TextType2 { get; set; }
        public string? DetailOfText2 { get; set; }
        public string? ItemNumber { get; set; }
        public string? material { get; set; }
        public string? TypeOfMaterialCode { get; set; }
        public string? ItemCategory { get; set; }
        public string? Quantity { get; set; }
        public string? UnitOfQuantity { get; set; }
        public string? PlannedGoodIssueDate { get; set; }
        public string? PriceCondition { get; set; }
        public string? PriceList { get; set; }
        public string? Sign { get; set; }
        public string? Value { get; set; }
        public string? ConditionPricingUnit { get; set; }
        public string? UnitOfPrice { get; set; }
        public string? ConditionCurrency { get; set; }
        public string? Plant { get; set; }
        public string? StorageLocation { get; set; }
        public string? ItemUsage { get; set; }
        public string? BatchNumber { get; set; }
        public string? InternalOrder { get; set; }
        public string? TypeDocumentInReferenceLine { get; set; }
        public string? ItemInReference { get; set; }
        public string? ItemTextType { get; set; }
        public string? ItemTextLanguage { get; set; }
        public string? ItemDetailOfText { get; set; }
        public string? ItemTextType2 { get; set; }
        public string? ItemTextLanguage2 { get; set; }
        public string? ItemDetailOfText2 { get; set; }
    }

    public class XmlGenerateAccrualBody
    {
        public int entity { get; set; }
        public string? userid { get; set; }
    }
    public class XMLGenerateNonBatchDitributorsPayment
    {
        public string? InvoiceNo { get; set; }
        public string? BatchName { get; set; }
        public string? Distributor { get; set; }
        public string? BudgetBucket { get; set; }
        public string? InvoiceDNNumber { get; set; }
        public double DNAmount { get; set; }
        public double FeeAmount { get; set; }
        public double FeePct { get; set; }
        public double PPNAmt { get; set; }
        public double PPNPct { get; set; }
        public double PPHAmt { get; set; }
        public double PPHPct { get; set; }
        public double TotalToPay { get; set; }
        public string? DNNumber { get; set; }
        public string? DNDesc { get; set; }
        public string? PromoNumber { get; set; }
    }
    public class XMLGeneratePaymentBatch
    {
        public IList<XMLGenerate>? xmlgenerate { get; set; }
        public IList<DistributorPayment>? distributorpayment { get; set; }
        public IList<PaymentBatch>? paymentbacth { get; set; }

    }

    public class DistributorPayment
    {
        public string? InvoiceNo { get; set; }
        public string? BatchName { get; set; }
        public string? Distributor { get; set; }
        public string? BudgetBucket { get; set; }
        public string? InvoiceDNNumber { get; set; }
        public double DNAmount { get; set; }
        public double FeeAmount { get; set; }
        public double FeePct { get; set; }
        public double PPNAmt { get; set; }
        public double PPNPct { get; set; }
        public double PPHAmt { get; set; }
        public double PPHPct { get; set; }
        public double TotalToPay { get; set; }
        public string? DNNumber { get; set; }
        public string? DNDesc { get; set; }
        public string? PromoNumber { get; set; }
        public string? TaxLevel { get; set; }
        public string? FPNumber { get; set; }
        public DateTime FPDate { get; set; }
        public string? OriginalId { get; set; }
        public string? DNDescription { get; set; }

    }

    public class PaymentBatch
    {
        public string? DNNumber { get; set; }
        public string? DNDesc { get; set; }
        public string? promoNumber { get; set; }
        public double DNAmount { get; set; }
        public double FeeAmount { get; set; }
        public double PPNAmt { get; set; }
        public double PPHAmt { get; set; }
        public double TotalToPay { get; set; }
        public string? BatchName { get; set; }
    }
    public class XMLGeneratePaymentBatchBodyReq
    {
        public int entity { get; set; }
        public int batching { get; set; }
    }
    public class XMLGeneratePaymentBatchBody
    {
        public int entity { get; set; }
        public int batching { get; set; }
        public int[]? id { get; set; }
    }
    public class GenerateXMLAccrualbyIdBodyReq
    {
        public int id { get; set; }
    }
    public class XMLFlaggingList
    {
        public int Id { get; set; }
        public string? PONumber { get; set; }
        public string? OriginalId { get; set; }
        public string? GenerateBy { get; set; }
        public string? GeneratedEmail { get; set; }
        public DateTime? GenerateOn { get; set; }
        public int isDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletedEmail { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int EntityId { get; set; }
        public string? Entity { get; set; }
    }
    public class XMLFlaggingListBodyReq
    {
        public int entityid { get; set; }
        public string? userid { get; set; }
        public string? generateon { get; set; }
    }
    public class XMLFlaggingUpdateBody
    {
        public string? userid { get; set; }
        public string? useremail { get; set; }
        public IList<XMLGenerateNMNbody>? UpdateId { get; set; }
    }
    public class XMLUploadListDto
    {
        public string? UploadType { get; set; }
        public string? FileName { get; set; }
        public string? UploadBy { get; set; }
        public string? UploadEmail { get; set; }
        public DateTime UploadOn { get; set; }
    }
    public class XMLUploadListBody
    {
        public string? uploadtype { get; set; }
    }
    public class XMLUploadBody
    {
        public string? userid { get; set; }
        public string? useremail { get; set; }
        public string? filename { get; set; }
        public string? uploadtype { get; set; }
    }
    public class XMLFlaggingHistoryBody
    {
        public string? ponumber { get; set; }
    }
    public class XMLGenerateNMN
    {
        public int nomor { get; set; }
        public string? glaw { get; set; }
        public string? dateretrieve1 { get; set; }
        public string? dateretrieve2 { get; set; }
        public string? entitycode { get; set; }
        public string? idr { get; set; }
        public string? blank1 { get; set; }
        public string? nama { get; set; }
        public string? tc { get; set; }
        public string? dateretrieve10 { get; set; }
        public object? noldua { get; set; }
        public string? cbufin { get; set; }
        public string? topline { get; set; }
        public string? blank2 { get; set; }
        public string? fix1 { get; set; }
        public string? blank3 { get; set; }
        public string? fix2 { get; set; }
        public string? blank4 { get; set; }
        public string? blank5 { get; set; }
        public double ytdvalue { get; set; }
        public string? fix3 { get; set; }
        public string? blank6 { get; set; }
        public string? blank7 { get; set; }
        public string? accruts { get; set; }
        public string? promoid { get; set; }
        public string? blank8 { get; set; }
        public string? my { get; set; }
        public string? blank10 { get; set; }
        public string? blank11 { get; set; }
        public string? sh { get; set; }
        public string? blank12 { get; set; }
        public string? blank13 { get; set; }
        public string? blank14 { get; set; }
        public string? blank15 { get; set; }
        public string? blank16 { get; set; }
        public string? blank17 { get; set; }
        public string? blank18 { get; set; }
    }
    public class XMLGenerateNMNbody
    {
        public int id { get; set; }
    }
    public class XMLGenerateBatchNameList
    {
        public string? BatchName { get; set; }
        public string? Distributor { get; set; }
        public string? BudgetBucket { get; set; }
        public string? InvoiceDNNumber { get; set; }
        public double DNAmount { get; set; }
        public double FeeAmount { get; set; }
        public double FeePct { get; set; }
        public double PPNAmt { get; set; }
        public double PPNPct { get; set; }
        public double PPHAmt { get; set; }
        public double PPHPct { get; set; }
        public double TotalToPay { get; set; }
        public string? fpnumber { get; set; }
        public string? fpdate { get; set; }
        public string? DNNumber { get; set; }
        public string? DNDesc { get; set; }
        public string? PromoNumber { get; set; }
        public string? taxlevel { get; set; }
    }
    public class XMLGenerateBatchNameBodyReq
    {
        public int entitylist { get; set; }
        public string? userid { get; set; }
        public int[]? id { get; set; }
    }

    public class DistributorEntityXMLGenerate
    {
        public int DistributorId { get; set; }
        public string? LongDesc { get; set; }
    }
    public class EntityforXMLGenerate
    {
        public int id { get; set; }
        public string? shortDesc { get; set; }
        public string? LongDesc { get; set; }
    }
    public class UserProfileXMLGenerate
    {
        public string? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? usergroupid { get; set; }
        public int userlevel { get; set; }
        public string? department { get; set; }
        public string? jobtitle { get; set; }
        public string? contactinfo { get; set; }
        public string? distributorid { get; set; }
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
        public string? deletedby { get; set; }
        public DateTime deletedon { get; set; }
        public string? statusname { get; set; }
        public string? statussearch { get; set; }
        public string? usergroupname { get; set; }
        public string? levelname { get; set; }
    }
    public class PromoAccrualReportHeader
    {
        public int Id { get; set; }
        public string? Periode { get; set; }
        public int EntityId { get; set; }
        public int DistributorId { get; set; }
        public int budgetparent { get; set; }
        public int ChannelId { get; set; }
        public string? UserId { get; set; }
        public DateTime ClosingDt { get; set; }
        public DateTime CreateOn { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
    }
    public class XmlGenerateAccrualById
    {
        public int PrincipalId { get; set; }
        public string? PrincipalDesc { get; set; }
        public string? OrderCBU { get; set; }
        public string? OrderLogicalMessage { get; set; }
        public string? SoldToCode { get; set; }
        public string? TypeOfSoldToCode { get; set; }
        public string? ShipToCode { get; set; }
        public string? TypeofShipToCode { get; set; }
        public string? OtherPartnerCode { get; set; }
        public string? PartnerFunctionOfOtherPartner { get; set; }
        public string? TypeOfOtherPartnerCode { get; set; }
        public string? OtherPartnerCode2 { get; set; }
        public string? PartnerFunctionOfOtherPartner2 { get; set; }
        public string? TypeOfOtherPartnerCode2 { get; set; }
        public string? OtherPartnerCode3 { get; set; }
        public string? PartnerFunctionOfOtherPartner3 { get; set; }
        public string? TypeOfOtherPartnerCode3 { get; set; }
        public string? PaymentTerm { get; set; }
        public string? PONumber { get; set; }
        public string? RequestedDeliveryDate { get; set; }
        public string? RequestedDeliveryTime { get; set; }
        public string? PODate { get; set; }
        public string? OrderType { get; set; }
        public string? SalesOrg { get; set; }
        public string? Channel { get; set; }
        public string? Division { get; set; }
        public string? DeliveryBlock { get; set; }
        public string? BillingBlock { get; set; }
        public string? ShippingCondition { get; set; }
        public string? ShippingType { get; set; }
        public string? Assignment { get; set; }
        public string? OrderReason { get; set; }
        public string? PurchaseOrderType { get; set; }
        public string? OrderModificationReason { get; set; }
        public string? ShipToPartysPurchaseOrderNumber { get; set; }
        public string? ShipToPartysPurchaseOrderDate { get; set; }
        public string? TypeDocumentInReferenceHeader { get; set; }
        public string? DocumentInReference { get; set; }
        public string? SoldToYourReference { get; set; }
        public string? ShipToYourReference { get; set; }
        public string? ShipToName1 { get; set; }
        public string? ShipToName2 { get; set; }
        public string? ShipToStreet { get; set; }
        public string? ShipToPostalCode { get; set; }
        public string? ShipToCity { get; set; }
        public string? ShipToCountry { get; set; }
        public string? TextType { get; set; }
        public string? DetailOfText { get; set; }
        public string? TextType2 { get; set; }
        public string? DetailOfText2 { get; set; }
        public string? ItemNumber { get; set; }
        public string? material { get; set; }
        public string? TypeOfMaterialCode { get; set; }
        public string? ItemCategory { get; set; }
        public string? Quantity { get; set; }
        public string? UnitOfQuantity { get; set; }
        public string? PlannedGoodIssueDate { get; set; }
        public string? PriceCondition { get; set; }
        public string? PriceList { get; set; }
        public string? Sign { get; set; }
        public string? Value { get; set; }
        public string? ConditionPricingUnit { get; set; }
        public string? UnitOfPrice { get; set; }
        public string? ConditionCurrency { get; set; }
        public string? Plant { get; set; }
        public string? StorageLocation { get; set; }
        public string? ItemUsage { get; set; }
        public string? BatchNumber { get; set; }
        public string? InternalOrder { get; set; }
        public string? TypeDocumentInReferenceLine { get; set; }
        public string? ItemInReference { get; set; }
        public string? ItemTextType { get; set; }
        public string? ItemTextLanguage { get; set; }
        public string? ItemDetailOfText { get; set; }
        public string? ItemTextType2 { get; set; }
        public string? ItemTextLanguage2 { get; set; }
        public string? ItemDetailOfText2 { get; set; }
    }
}

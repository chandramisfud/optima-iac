using Repositories.Contracts;
using V7.Controllers;

namespace V7.Controllers.DebitNote
{
    /// <summary>
    /// DebitNote Controller
    /// </summary>
    public partial class DebitNoteController : BaseController
    {
        private readonly IConfiguration __config;
        private readonly string __TokenSecret;
        private readonly IDNCreationRepo __repoDNCreation;
        private readonly IDNCreationHORepo __repoDNCreationHO;
        private readonly IDNSendtoHORepo __repoSendtoHO;
        private readonly IDNSuratJalanAndTandaTerimatoHORepo __repoSJandTTtoHO;
        private readonly IDNReceivedandApprovedbyHORepo __repoReceivedAndApprovedHO;
        private readonly IDNSendtoDanoneRepo __repoSendtoDanone;
        private readonly IDNSuratJalanAndTandaTerimatoDanoneRepo __repoSJandTTtoDanone;
        private readonly IDNReceivedbyDanoneRepo __repoDNReceivedbyDanone;
        private readonly IDNValidationbyFinanceRepo __repoValidationbyFinance;
        private readonly IDNValidationbySalesRepo __repoValidationbySales;
        private readonly IDNInvoiceNotificationByDanoneRepo __repoInvoiceNotif;
        private readonly IDNCreateInvoiceRepo __repoCreateInvoice;
        private readonly IDNConfirmDNPaidRepo __repoConfirmPaid;
        private readonly IDNMultiPrintRepo __repoMultiPrintDN;
        private readonly IDNUploadRepo __repoDNUpload;
        private readonly IDNUploadAttachmentRepo __repoDNUploadAttachment;
        private readonly IDNListingPromoDistributorRepo __repoDNListingPromoDistributor;
        private readonly IDNWorkflowRepo __repoDNWorkflow;
        private readonly IDNReassignmentRepo __repoDNReassignment;
        private readonly IDNManualAssignmentRepo __repoDNManualAssignment;
        private readonly IDNListingOverBudgetRepo __repoDNListingOverBudget;
        private readonly IDNReassignmentbyFinanceRepo __repoDNReassignmentFinance;
        private readonly IDNSendBackRepo __repoDNSendBack;
        private readonly IDNOverBudgetRepo __repoDNOverBudget;
        private readonly IDNVATExpiredChecklistRepo __repoDNVATExpiredChecklist;
        private readonly IDNPromoDisplayRepo __repoDNPromoDisplay;
        private readonly IDNMultiPrintPromoRepo __repoDNMultiPrintPromo;
        private readonly IDNUploadFakturRepo __repoDNUploadFaktur;

        public DebitNoteController(IConfiguration config,
        IDNCreationRepo repoDNCreation,
        IDNCreationHORepo repoDNCreationHO,
        IDNSendtoHORepo repoSendtoHO,
        IDNSuratJalanAndTandaTerimatoHORepo repoSJandTTtoHO,
        IDNReceivedandApprovedbyHORepo repoReceivedAndApproved,
        IDNSendtoDanoneRepo repoSendtoDamone,
        IDNSuratJalanAndTandaTerimatoDanoneRepo repoSJandTTtoDanone,
        IDNReceivedbyDanoneRepo repoDNReceivedbyDanone,
        IDNValidationbyFinanceRepo repoValidationbyFinance,
        IDNValidationbySalesRepo repoValidationbySales,
        IDNInvoiceNotificationByDanoneRepo repoInvoiceNotif,
        IDNCreateInvoiceRepo repoCreateInvoice,
        IDNConfirmDNPaidRepo repoConfirmPaid,
        IDNMultiPrintRepo repoMultiPrintDN,
        IDNUploadRepo repoDNUpload,
        IDNUploadAttachmentRepo repoDNUploadAttachment,
        IDNListingPromoDistributorRepo repoDNListingPromoDistributor,
        IDNWorkflowRepo repoDNWorkflow,
        IDNReassignmentRepo repoDNReassignment,
        IDNManualAssignmentRepo repoDNManualAssignment,
        IDNListingOverBudgetRepo repoDNListingOverBudget,
        IDNReassignmentbyFinanceRepo repoDNReassignmentFinance,
        IDNSendBackRepo repoDNSendBack,
        IDNOverBudgetRepo repoDNOverBudget,
        IDNVATExpiredChecklistRepo repoDNVATExpiredChecklist,
        IDNPromoDisplayRepo repoDNPromoDisplay,
        IDNMultiPrintPromoRepo repoDNMultiPrintPromo,
        IDNUploadFakturRepo repoDNUploadFaktur
        )
        {
            __config = config;
            __repoDNCreation = repoDNCreation;
            __repoDNCreationHO = repoDNCreationHO;
            __repoSendtoHO = repoSendtoHO;
            __repoSJandTTtoHO = repoSJandTTtoHO;
            __repoReceivedAndApprovedHO = repoReceivedAndApproved;
            __repoSendtoDanone = repoSendtoDamone;
            __repoSJandTTtoDanone = repoSJandTTtoDanone;
            __repoDNReceivedbyDanone = repoDNReceivedbyDanone;
            __repoValidationbyFinance = repoValidationbyFinance;
            __repoValidationbySales = repoValidationbySales;
            __repoInvoiceNotif = repoInvoiceNotif;
            __repoCreateInvoice = repoCreateInvoice;
            __repoConfirmPaid = repoConfirmPaid;
            __repoMultiPrintDN = repoMultiPrintDN;
            __repoDNUpload = repoDNUpload;
            __repoDNUploadAttachment = repoDNUploadAttachment;
            __repoDNListingPromoDistributor = repoDNListingPromoDistributor;
            __repoDNWorkflow = repoDNWorkflow;
            __repoDNReassignment = repoDNReassignment;
            __repoDNManualAssignment = repoDNManualAssignment;
            __repoDNListingOverBudget = repoDNListingOverBudget;
            __repoDNReassignmentFinance = repoDNReassignmentFinance;
            __repoDNSendBack = repoDNSendBack;
            __repoDNOverBudget = repoDNOverBudget;
            __repoDNVATExpiredChecklist = repoDNVATExpiredChecklist;
            __repoDNPromoDisplay = repoDNPromoDisplay;
            __repoDNMultiPrintPromo = repoDNMultiPrintPromo;
            __repoDNUploadFaktur = repoDNUploadFaktur;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
        }
    }
}

// Remove Mark
using System.Data;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;

namespace Repositories.Contracts
{
    public interface IDNCreateInvoiceRepo
    {
        //debetnote/invoicelist

        //#139 
        //
        Task<object> GetInvoiceList(string createdate, int entity, int distributor, string profileid, string sortColumn, string sortDirection = "ASC", int length = 10, int start = 0, string? txtSearch = null);

        //debetnote/getInvoiceById
        Task<DNCreateInvoice> GetInvoiceById(int id);
        //debetnote/invoicetaxlevel
        Task<IList<DNDetailforInvoice>> GetDNByStatusforInvoiceTaxLevel(string status, string userid, int entityid, int distributorid, string TaxLevel, string dnPeriod, int categoryId);
        //debetnote/invoice/store
        Task<InvoiceDto> CreateInvoice(
            int DistributorId,
            int EntityId,
            decimal DPPAmount,
            decimal PPNpct,
            decimal InvoiceAmount,
            string Desc,
            string UserId,
            IList<DNIdReadytoInvoiceArray> DNId,
            string TaxLevel,
            string dnPeriod,
            int categoryId
        );
        //debetnote/invoice/update
        Task<InvoiceDto> UpdateInvoice(
            int InvoiceId,
            int DistributorId,
            int EntityId,
            decimal DPPAmount,
            decimal PPNpct,
            decimal InvoiceAmount,
            string Desc,
            string UserId,
            IList<DNIdReadytoInvoiceArray> DNId,
            string TaxLevel,
            string dnPeriod,
            int categoryId
            );
        //debetnote/reject
        Task<DNRejectCreateInvoiceGlobalResponse> DNRejectCreateInvoice(int dnid, string reason, string userid);
        //promoattachment/delete
        Task DeletePromoAttachmentDNCreateInvoice(int PromoId, string DocLink);
        //debetnote/getbyId/
        Task<DNGetbyIdforCreateInvoice> GetDNGetbyIdforCreateInvoice(int id);
        //Select entity
        Task<IList<DNCreateInvoiceEntityList>> GetEntityList();
        //debetnote/ready_to_invoice
        Task<IList<DNDetailforInvoice>> GetDNStatusReadytoInvoice(string status, string userid, int entityid, int distributorid);
        //debetnote/printinvoice
        Task<InvoicePrintDto> GetPrintInvoicebyId(int id);
        //select taxlevel
        Task<IList<SelectTaxLevel>> GetTaxLevelList();
        // debetnote/ready_to_invoice [POST]
        Task<DNCreateInvoice> DNChangeStatusReadytoInvoice(DNChangeStatusReadytoInvoice param);
        // debetnote/filter/{status}/{entity}/{userid}/{TaxLevel}
        Task<IList<DNFilter>> DNFilterforCreatedInvoice(
            string userid, 
            string status, 
            int entity, 
            string TaxLevel, 
            DataTable dn, 
            int invoiceId, 
            string dnPeriod,
            int categoryId
        );
        Task<IList<DistributorforCreateInvoice>> GetDistributorforCreateInvoice(int entityId);
        Task<UserProfileDataByIdforDNCreateInvoice> GetById(string id);
        //Select entity
        Task<DNDistributorEntity> GetAttributeByUser(string userid);
        Task<IList<object>> GetCategoryDropdownList();
    }
}
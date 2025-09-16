using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Models.Promo
{
    public class PromoCloseListDto
    {
        public string? PromoId { get; set; }
        public string? PromoNumber { get; set; }
        public string? Entity { get; set; }
        public string? Distributor { get; set; }
        public string? Initiator { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public string? ChannelDesc { get; set; }
        public string? SubChannelDesc { get; set; }
        public string? AccountDesc { get; set; }
        public string? SubAccountDesc { get; set; }
        public string? BrandDesc { get; set; }
        public string? SubCategory { get; set; }
        public string? ActivityDesc { get; set; }
        public string? Mechanisme1 { get; set; }
        public string? Mechanisme2 { get; set; }
        public string? Mechanisme3 { get; set; }
        public string? Mechanisme4 { get; set; }
        public string? PromoStatus { get; set; }
        public string? ReconStatus { get; set; }
        public double Investment { get; set; }
        public double DNPaid { get; set; }
        public double DNClaim { get; set; }
        public double aging { get; set; }
        public DateTime LastDNCreationDate { get; set; }
        public double RemainingInvestment_DN { get; set; }
        public bool ClosureStatus { get; set; }
        public string? CloseBy { get; set; }
        public DateTime CloseOn { get; set; }
        public string? statusdate { get; set; }
        // Added, andrie Oct 9 2023 E2#38
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
        // Added, andrie Nov 11 2024 
        public bool? isOldPromo
        {
            get
            {
                return StartPromo.Year < 2025;
            }
            set { }
        }
        public bool reconciled { get; set;}
    }

    public class PromoCloseListDtoPagination
    {
        public IList<PromoCloseListDto>? Data { get; set; }
        public RecordTotal? RecordsTotal { get; set; }
    }

    public class PromoImportResponse
    {
        public string? id { get; set; }
        public string? doc { get; set; }
        public string? status { get; set; }
    }

    public class PromoToCloseDto
    {

        public string? userId { get; set; }
        public IList<DetailPromoToClose>? lsPromoId { get; set; }
    }

    public class DetailPromoToClose
    {
        public int Id { get; set; }

    }

    public class ResponseMultipleDocDto
    {
        public string? doc { get; set; }
        public string? status { get; set; }
    }

}

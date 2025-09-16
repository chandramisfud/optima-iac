using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Entities.Configuration;
using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.PromoApproval;

namespace Repositories.Entities.Models.PromoSendback
{
    public class InitiatorView
    {
        public DateTime CreateOn { get; set; }
        public int PromoId { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? PromoNumber { get; set; }
        public string? ActivityDesc { get; set; }
        public DateTime StartPromo { get; set; }
        public DateTime EndPromo { get; set; }
        public decimal Target { get; set; }
        public decimal Investment { get; set; }
        public string? LastStatus { get; set; }
        public DateTime ApproveOn { get; set; }
        public string? ApprovalNotes { get; set; }
        // Added May 24 2023 by AND #875
        public bool allowedit { get; set; }

        // Added, andrie Oct 9 2023 E2#38
        public int categoryId { get; set; }
        public string? categoryShortDesc { get; set; }
        // Added, andrie Oct 16 2024 E2#38
        public bool? isOldPromo
        {
            get
            {
                return StartPromo.Year < 2025;
            }
            set { }
        }

        // Added, andrie Oct 23 2024 
        public int approvalCycle { get; set; }
    }

    public class Activity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? LongDesc { get; set; }
        public string? ShortDesc { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateOn { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeleteOn { get; set; }
        public string? DeleteBy { get; set; }
        public string? RefId { get; set; }
    }

    public class PromoSendbackV3Dto
    {
        public PromoRevise? PromoHeader { get; set; }
        public List<PromoRegionRes>? Regions { get; set; }
        public List<PromoChannelRes>? Channels { get; set; }
        public List<PromoSubChannelRes>? SubChannels { get; set; }
        public List<PromoAccountRes>? Accounts { get; set; }
        public List<PromoSubAccountRes>? SubAccounts { get; set; }
        public List<PromoActivityRes>? Activity { get; set; }
        public List<PromoSubActivityRes>? SubActivity { get; set; }
        public IList<PromoBrandRes>? Brands { get; set; }
        public IList<PromoProductRes>? Skus { get; set; }
        public IList<PromoAttachment>? Attachments { get; set; }
        public IList<ApprovalRes>? ListApprovalStatus { get; set; }
        public IList<PromoApproval.MechanismSelect>? Mechanism { get; set; }

        //Add:  AND Oct 11 2023 E2#38
        public List<Object>? Investment { get; set; }
        public List<Object>? GroupBrand { get; set; }
        public IList<PromoItem>? PromoConfigItem { get; set; }
    }
}

using Repositories.Entities.Models.PromoApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities.Models.Promo
{
    public class PromoSelectSKP
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
        public IList<SKPValidation>? SKPValidations { get; set; }
        public IList<MechanismSelect>? Mechanisms { get; set; }

        public List<Object>? GroupBrand { get; set; }

    }
}

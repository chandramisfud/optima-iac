using System;
using System.Collections.Generic;

namespace Entities
{
    public class PromoApproval
    {
        public int PromoId { get; set; }
        public string? ApprovalStatusCode { get; set; }
        public string? Notes { get; set; }
        public DateTime ApprovalDate { get; set; }
    }

    public class PromoApprovalReminderSetting
    {
        public int id { get; set; }
        public int dt1 { get; set; }
        public int dt2 { get; set; }
        public bool eod { get; set; }
        public bool autorun { get; set; } 
        public List<PromoApprovalReminderConfigEmail>? configEmail { get; set; }

    }
    public class PromoApprovalReminderConfigEmail
    {

        public string? email { get; set; }
        public string? userName { get; set; }
        public string? userGroupName { get; set; }
        public string? statusName { get; set; }
   
    }

    public class PromoApprovalReminderRegularSend
    {
        public List<string?>? email { get; set; }
        public List<PromoApprovalReminder>? data { get; set; }
        public PromoApprovalInvestmentGap? gap { get; set; }
        public object? lsPromo { get; set; }
    }

    public class PromoApprovalReminderResp
    {
        public PromoApprovalInvestmentGap? gap { get; set; }
        public List<PromoApprovalReminder>? data { get; set; }
        public object? lsPromo { get; set; }
    }

    public class PromoApprovalInvestmentGap
    {
        public int promoCount { get; set; }
        public double promoInvestment { get; set; }
    }
    public class PromoApprovalReminder
    {
        public string? channelhead { get; set; }
		public string? channel { get; set; }
		public string? subacc { get; set; }
		public string? kamfcmcem { get; set; } 
		public string? status2 { get; set; }

        public string? sb_group { get; set; }

        public string? W11 { get; set; } 
		public decimal Inv11 { get; set; } 	
		public string? W12 { get; set; }
		public decimal Inv12 { get; set; } 
		public string? W1tot { get; set; } 
		public decimal Inv1tot { get; set; }

        public string? W21 { get; set; }
        public decimal Inv21 { get; set; }
        public string? W22 { get; set; }
        public decimal Inv22 { get; set; }
        public string? W2tot { get; set; }
        public decimal Inv2tot { get; set; }

        public string? W31 { get; set; }
        public decimal Inv31 { get; set; }
        public string? W32 { get; set; }
        public decimal Inv32 { get; set; }
        public string? W3tot { get; set; }
        public decimal Inv3tot { get; set; }

        public string? W41 { get; set; }
        public decimal Inv41 { get; set; }
        public string? W42 { get; set; }
        public decimal Inv42 { get; set; }
        public string? W4tot { get; set; }
        public decimal Inv4tot { get; set; }

        public string? W51 { get; set; }
        public decimal Inv51 { get; set; }
        public string? W52 { get; set; }
        public decimal Inv52 { get; set; }
        public string? W5tot { get; set; }
        public decimal Inv5tot { get; set; }

        public string? W61 { get; set; }
        public decimal Inv61 { get; set; }
        public string? W62 { get; set; }
        public decimal Inv62 { get; set; }
        public string? W6tot { get; set; }
        public decimal Inv6tot { get; set; }

        public string? W71 { get; set; }
        public decimal Inv71 { get; set; }
        public string? W72 { get; set; }
        public decimal Inv72 { get; set; }
        public string? W7tot { get; set; }
        public decimal Inv7tot { get; set; }

        public string? W81 { get; set; }
        public decimal Inv81 { get; set; }
        public string? W82 { get; set; }
        public decimal Inv82 { get; set; }
        public string? W8tot { get; set; }
        public decimal Inv8tot { get; set; }

        public string? W91 { get; set; }
        public decimal Inv91 { get; set; }
        public string? W92 { get; set; }
        public decimal Inv92 { get; set; }
        public string? W9tot { get; set; }
        public decimal Inv9tot { get; set; }

        public string? W101 { get; set; }
        public decimal Inv101 { get; set; }
        public string? W102 { get; set; }
        public decimal Inv102 { get; set; }
        public string? W10tot { get; set; }
        public decimal Inv10tot { get; set; }

        public string? W111 { get; set; }
        public decimal Inv111 { get; set; }
        public string? W112 { get; set; }
        public decimal Inv112 { get; set; }
        public string? W11tot { get; set; }
        public decimal Inv11tot { get; set; }

        public string? W121 { get; set; }
        public decimal Inv121 { get; set; }
        public string? W122 { get; set; }
        public decimal Inv122 { get; set; }
        public string? W12tot { get; set; }
        public decimal Inv12tot { get; set; }
        public string? Wtot { get; set; }

        public decimal InvTot { get; set; }
        public string? Urut { get; set; }
        public string? Urut1 { get; set; }
		
    }
}
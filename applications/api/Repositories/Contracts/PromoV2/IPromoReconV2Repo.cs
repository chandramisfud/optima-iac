using Repositories.Entities;
using Repositories.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IPromoReconV2Repository
    {
        Task<object> GetPromoReconById(int id);
        Task<object> GetPromoReconDCById(int id);
        Task<object> SetPromoReconDCUpdate(DataTable promo, DataTable region, DataTable sku, DataTable attachment, DataTable mechanism);
        Task<object> SetPromoReconUpdate(DataTable promo, DataTable region, DataTable sku, DataTable attachment, DataTable mechanism, decimal baselineCalcRecon, decimal upliftCalcRecon, decimal totalSalesCalcRecon, decimal salesContributionCalcRecon, decimal storesCoverageCalcRecon, decimal redemptionRateCalcRecon, decimal crCalcRecon, decimal roiCalcRecon, decimal costCalcRecon);
    }
}

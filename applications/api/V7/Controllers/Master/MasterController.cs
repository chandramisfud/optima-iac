using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using V7.Controllers;

namespace V7.Controllers.Master
{
    public partial class MasterController : BaseController
    {
        private readonly IConfiguration __config;
        private readonly IActivityRepository __repoActivity;
        private readonly IAccountRepository __repoAccount;
        private readonly IBrandRepository __repoBrand;
        private readonly ICategoryRepository __repoCategory;
        private readonly IChannelRepository __repoChannel;
        private readonly IEntityRepository __repoEntity;
        private readonly IDistributorRepository __repoDistributor;
        private readonly ICancelReasonRepository __repoCancelReason;
        private readonly IRegionRepository __repoRegion;
        private readonly IProfitCenterRepository __repoProfitCenter;
        private readonly ISubChannelRepository __repoSubChannel;
        private readonly ISubActivityTypeRepository __repoSubActivityType;
        private readonly IInvestmentTypeRepository __repoInvestmentType;
        private readonly ISellingPointRepository __repoSellingPoint;
        private readonly string __TokenSecret;
        private readonly ISubCategoryRepository __repoSubCategory;
        private readonly IProductRepository __repoProduct;
        private readonly ISubAccountRepository __repoSubAccount;
        private readonly ISubActivityRepository __repoSubActivity;
        private readonly IMechanismRepository __repoMechanism;
        private readonly IMatrixPromoRepository __repoMatrixPromo;
        private readonly IGroupBrandRepo __repoGroupBrand;


        public MasterController(IConfiguration config
        , IAccountRepository repoAccount
        , IBrandRepository repoBrand
        , ICategoryRepository repoCategory
        , IChannelRepository repoChannel
        , IEntityRepository repoEntity
        , IActivityRepository repoActivity
        , IDistributorRepository repoDistributor
        , ICancelReasonRepository repoCancelReason
        , IRegionRepository repoRegion
        , IProfitCenterRepository repoProfitCenter
        , ISubChannelRepository repoSubChannel
        , ISubActivityTypeRepository repoSubActivityType
        , IInvestmentTypeRepository repoInvestmentType
        , ISellingPointRepository repoSellingPoint
        , ISubCategoryRepository repoSubCategory
        , IProductRepository repoProduct
        , ISubAccountRepository repoSubAccount
        , ISubActivityRepository repoSubActivity
        , IMechanismRepository repoMechanism
        , IMatrixPromoRepository repoMatrixPromo
        , IGroupBrandRepo repoGroupBrand

        )
        {
            __config = config;
            __repoAccount = repoAccount;
            __repoBrand = repoBrand;
            __repoActivity = repoActivity;
            __repoCategory = repoCategory;
            __repoChannel = repoChannel;
            __repoEntity = repoEntity;
            __repoDistributor = repoDistributor;
            __repoCancelReason = repoCancelReason;
            __repoRegion = repoRegion;
            __repoProfitCenter = repoProfitCenter;
            __repoSubChannel = repoSubChannel;
            __repoSubActivityType = repoSubActivityType;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
            __repoInvestmentType = repoInvestmentType;
            __repoSellingPoint = repoSellingPoint;
            __repoSubCategory = repoSubCategory;
            __repoProduct = repoProduct;
            __repoSubAccount = repoSubAccount;
            __repoSubActivity = repoSubActivity;
            __repoMechanism = repoMechanism;
            __repoMatrixPromo = repoMatrixPromo;
            __repoGroupBrand = repoGroupBrand;
        }
    }
}

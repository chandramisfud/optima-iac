<?php

use App\Modules\Master\Controllers\Entity;
use App\Modules\Master\Controllers\Distributor;
use App\Modules\Master\Controllers\Channel;
use App\Modules\Master\Controllers\SubChannel;
use App\Modules\Master\Controllers\Account;
use App\Modules\Master\Controllers\SubAccount;
use App\Modules\Master\Controllers\Brand;
use App\Modules\Master\Controllers\Sku;
use App\Modules\Master\Controllers\Category;
use App\Modules\Master\Controllers\SubCategory;
use App\Modules\Master\Controllers\Activity;
use App\Modules\Master\Controllers\SubActivity;
use App\Modules\Master\Controllers\SubActivityType;
use App\Modules\Master\Controllers\SellingPoint;
use App\Modules\Master\Controllers\Region;
use App\Modules\Master\Controllers\ProfitCenter;
use App\Modules\Master\Controllers\PromoCancelReason;
use App\Modules\Master\Controllers\InvestmentType;
use App\Modules\Master\Controllers\PromoMechanism;
use App\Modules\Master\Controllers\PromoMatrixApproval;

use Illuminate\Support\Facades\Route;


Route::group(['middleware' => ['web', 'checkSession']], function () {

    // Distributor
    Route::group(['prefix' => 'master/distributor'], function() {
        Route::get('/', [ Distributor::class, 'landingPage'])->name('mst-distributor');
        Route::get('/list/paginate/filter', [ Distributor::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ Distributor::class, 'getDataByID']);
        Route::get('/form', [ Distributor::class, 'distributorFormPage'])->name('mst-distributor.form');
        Route::post('/save', [ Distributor::class, 'save']);
        Route::post('/update', [ Distributor::class, 'update']);
        Route::post('/delete', [ Distributor::class, 'delete']);
        Route::get('/export-xls', [ Distributor::class, 'exportXls']);
    });

    // Entity
    Route::group(['prefix' => 'master/entity'], function() {
        Route::get('/', [ Entity::class, 'landingPage'])->name('mst-entity');
        Route::get('/list/paginate/filter', [ Entity::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ Entity::class, 'getDataByID']);
        Route::get('/form', [ Entity::class, 'entityFormPage'])->name('mst-entity.form');
        Route::post('/save', [ Entity::class, 'save']);
        Route::post('/update', [ Entity::class, 'update']);
        Route::post('/delete', [ Entity::class, 'delete']);
        Route::get('/export-xls', [ Entity::class, 'exportXls']);
    });

    // Channel
    Route::group(['prefix' => 'master/channel'], function() {
        Route::get('/', [ Channel::class, 'landingPage'])->name('mst-channel');
        Route::get('/list/paginate/filter', [ Channel::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ Channel::class, 'getDataByID']);
        Route::get('/form', [ Channel::class, 'channelFormPage'])->name('mst-channel.form');
        Route::post('/save', [ Channel::class, 'save']);
        Route::post('/update', [ Channel::class, 'update']);
        Route::post('/delete', [ Channel::class, 'delete']);
        Route::get('/export-xls', [ Channel::class, 'exportXls']);
    });

    // Sub Channel
    Route::group(['prefix' => 'master/sub-channel'], function() {
        Route::get('/', [ SubChannel::class, 'landingPage'])->name('mst-sub-channel');
        Route::get('/list/paginate/filter', [ SubChannel::class, 'getListPaginateFilter']);
        Route::get('/get-list/channel', [ SubChannel::class, 'getListChannel']);
        Route::get('/data/id', [ SubChannel::class, 'getDataByID']);
        Route::get('/form', [ SubChannel::class, 'subchannelFormPage'])->name('mst-sub-channel.form');
        Route::post('/save', [ SubChannel::class, 'save']);
        Route::post('/update', [ SubChannel::class, 'update']);
        Route::post('/delete', [ SubChannel::class, 'delete']);
        Route::get('/export-xls', [ SubChannel::class, 'exportXls']);
    });

    // Account
    Route::group(['prefix' => 'master/account'], function() {
        Route::get('/', [ Account::class, 'landingPage'])->name('mst-account');
        Route::get('/list/paginate/filter', [ Account::class, 'getListPaginateFilter']);
        Route::get('/get-list/channel', [ Account::class, 'getListChannel']);
        Route::get('/get-data/sub-channel/channel-id', [ Account::class, 'getDataSubchannel']);
        Route::get('/data/id', [ Account::class, 'getDataByID']);
        Route::get('/form', [ Account::class, 'accountFormPage'])->name('mst-account.form');
        Route::post('/save', [ Account::class, 'save']);
        Route::post('/update', [ Account::class, 'update']);
        Route::post('/delete', [ Account::class, 'delete']);
        Route::get('/export-xls', [ Account::class, 'exportXls']);
    });

    // Sub Account
    Route::group(['prefix' => 'master/sub-account'], function() {
        Route::get('/', [ SubAccount::class, 'landingPage'])->name('mst-sub-account');
        Route::get('/list/paginate/filter', [ SubAccount::class, 'getListPaginateFilter']);
        Route::get('/get-list/channel', [ SubAccount::class, 'getListChannel']);
        Route::get('/get-data/sub-channel/channel-id', [ SubAccount::class, 'getDataSubchannel']);
        Route::get('/get-data/account/sub-channel-id', [ SubAccount::class, 'getDataAccount']);
        Route::get('/data/id', [ SubAccount::class, 'getDataByID']);
        Route::get('/form', [ SubAccount::class, 'subAccountFormPage'])->name('mst-sub-account.form');
        Route::post('/save', [ SubAccount::class, 'save']);
        Route::post('/update', [ SubAccount::class, 'update']);
        Route::post('/delete', [ SubAccount::class, 'delete']);
        Route::get('/export-xls', [ SubAccount::class, 'exportXls']);
    });

    // Brand
    Route::group(['prefix' => 'master/brand'], function() {
        Route::get('/', [ Brand::class, 'landingPage'])->name('mst-brand');
        Route::get('/list/paginate/filter', [ Brand::class, 'getListPaginateFilter']);
        Route::get('/get-list/entity', [ Brand::class, 'getListEntity']);
        Route::get('/data/id', [ Brand::class, 'getDataByID']);
        Route::get('/form', [ Brand::class, 'brandFormPage'])->name('mst-brand.form');
        Route::post('/save', [ Brand::class, 'save']);
        Route::post('/update', [ Brand::class, 'update']);
        Route::post('/delete', [ Brand::class, 'delete']);
        Route::get('/export-xls', [ Brand::class, 'exportXls']);
    });

    // SKU
    Route::group(['prefix' => 'master/sku'], function() {
        Route::get('/', [ Sku::class, 'landingPage'])->name('mst-sku');
        Route::get('/list/paginate/filter', [ Sku::class, 'getListPaginateFilter']);
        Route::get('/get-list/entity', [ Sku::class, 'getListEntity']);
        Route::get('/get-data/brand/entity-id', [ Sku::class, 'getDataBrand']);
        Route::get('/data/id', [ Sku::class, 'getDataByID']);
        Route::get('/form', [ Sku::class, 'skuFormPage'])->name('mst-sku.form');
        Route::post('/save', [ Sku::class, 'save']);
        Route::post('/update', [ Sku::class, 'update']);
        Route::post('/delete', [ Sku::class, 'delete']);
        Route::get('/export-xls', [ Sku::class, 'exportXls']);
    });

    // Category Promo
    Route::group(['prefix' => 'master/category'], function() {
        Route::get('/', [ Category::class, 'landingPage'])->name('mst-category');
        Route::get('/list/paginate/filter', [ Category::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ Category::class, 'getDataByID']);
        Route::get('/form', [ Category::class, 'categoryFormPage'])->name('mst-category.form');
        Route::post('/save', [ Category::class, 'save']);
        Route::post('/update', [ Category::class, 'update']);
        Route::post('/delete', [ Category::class, 'delete']);
        Route::get('/export-xls', [ Category::class, 'exportXls']);
    });

    // Sub Category Promo
    Route::group(['prefix' => 'master/sub-category'], function() {
        Route::get('/', [ SubCategory::class, 'landingPage'])->name('mst-sub-category');
        Route::get('/list/paginate/filter', [ SubCategory::class, 'getListPaginateFilter']);
        Route::get('/get-list/category', [ SubCategory::class, 'getListCategory']);
        Route::get('/data/id', [ SubCategory::class, 'getDataByID']);
        Route::get('/form', [ SubCategory::class, 'subCategoryFormPage'])->name('mst-sub-category.form');
        Route::post('/save', [ SubCategory::class, 'save']);
        Route::post('/update', [ SubCategory::class, 'update']);
        Route::post('/delete', [ SubCategory::class, 'delete']);
        Route::get('/export-xls', [ SubCategory::class, 'exportXls']);
    });

    // Activity
    Route::group(['prefix' => 'master/activity'], function() {
        Route::get('/', [ Activity::class, 'landingPage'])->name('mst-activity');
        Route::get('/list/paginate/filter', [ Activity::class, 'getListPaginateFilter']);
        Route::get('/get-list/category', [ Activity::class, 'getListCategory']);
        Route::get('/get-data/sub-category/category-id', [ Activity::class, 'getDataSubCategory']);
        Route::get('/data/id', [ Activity::class, 'getDataByID']);
        Route::get('/form', [ Activity::class, 'activityFormPage'])->name('mst-activity.form');
        Route::post('/save', [ Activity::class, 'save']);
        Route::post('/update', [ Activity::class, 'update']);
        Route::post('/delete', [ Activity::class, 'delete']);
        Route::get('/export-xls', [ Activity::class, 'exportXls']);
    });

    // Sub Activity
    Route::group(['prefix' => 'master/sub-activity'], function() {
        Route::get('/', [ SubActivity::class, 'landingPage'])->name('mst-sub-activity');
        Route::get('/list/paginate/filter', [ SubActivity::class, 'getListPaginateFilter']);
        Route::get('/get-list/category', [ SubActivity::class, 'getListCategory']);
        Route::get('/get-data/sub-category/category-id', [ SubActivity::class, 'getDataSubCategory']);
        Route::get('/get-data/activity/sub-category-id', [ SubActivity::class, 'getDataActivity']);
        Route::get('/get-list/sub-activity-type', [ SubActivity::class, 'getListSubActivityType']);
        Route::get('/data/id', [ SubActivity::class, 'getDataByID']);
        Route::get('/form', [ SubActivity::class, 'subActivityFormPage'])->name('mst-sub-activity.form');
        Route::post('/save', [ SubActivity::class, 'save']);
        Route::post('/update', [ SubActivity::class, 'update']);
        Route::post('/delete', [ SubActivity::class, 'delete']);
        Route::get('/export-xls', [ SubActivity::class, 'exportXls']);
    });

    // Sub Activity Type
    Route::group(['prefix' => 'master/sub-activity-type'], function() {
        Route::get('/', [ SubActivityType::class, 'landingPage'])->name('mst-type-sub-activity');
        Route::get('/list/paginate/filter', [ SubActivityType::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ SubActivityType::class, 'getDataByID']);
        Route::get('/form', [ SubActivityType::class, 'subActivityTypeFormPage'])->name('mst-type-sub-activity.form');
        Route::post('/save', [ SubActivityType::class, 'save']);
        Route::post('/update', [ SubActivityType::class, 'update']);
        Route::post('/delete', [ SubActivityType::class, 'delete']);
        Route::get('/export-xls', [ SubActivityType::class, 'exportXls']);
    });

    // Selling Point
    Route::group(['prefix' => 'master/selling-point'], function() {
        Route::get('/', [ SellingPoint::class, 'landingPage'])->name('mst-selling-point');
        Route::get('/list/paginate/filter', [ SellingPoint::class, 'getListPaginateFilter']);
        Route::get('/get-list/region', [ SellingPoint::class, 'getListRegion']);
        Route::get('/get-list/profit-center', [ SellingPoint::class, 'getListProfitCenter']);
        Route::get('/data/id', [ SellingPoint::class, 'getDataByID']);
        Route::get('/form', [ SellingPoint::class, 'sellingPointFormPage'])->name('mst-selling-point.form');
        Route::post('/save', [ SellingPoint::class, 'save']);
        Route::post('/update', [ SellingPoint::class, 'update']);
        Route::post('/delete', [ SellingPoint::class, 'delete']);
        Route::get('/export-xls', [ SellingPoint::class, 'exportXls']);
    });

    // Region
    Route::group(['prefix' => 'master/region'], function() {
        Route::get('/', [ Region::class, 'landingPage'])->name('mst-region');
        Route::get('/list/paginate/filter', [ Region::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ Region::class, 'getDataByID']);
        Route::get('/form', [ Region::class, 'regionFormPage'])->name('mst-region.form');
        Route::post('/save', [ Region::class, 'save']);
        Route::post('/update', [ Region::class, 'update']);
        Route::post('/delete', [ Region::class, 'delete']);
        Route::get('/export-xls', [ Region::class, 'exportXls']);
    });

    // Profit Center
    Route::group(['prefix' => 'master/profit-center'], function() {
        Route::get('/', [ ProfitCenter::class, 'landingPage'])->name('mst-profit-center');
        Route::get('/list/paginate/filter', [ ProfitCenter::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ ProfitCenter::class, 'getDataByID']);
        Route::get('/form', [ ProfitCenter::class, 'profitCenterFormPage'])->name('mst-profit-center.form');
        Route::post('/save', [ ProfitCenter::class, 'save']);
        Route::post('/update', [ ProfitCenter::class, 'update']);
        Route::post('/delete', [ ProfitCenter::class, 'delete']);
        Route::get('/export-xls', [ ProfitCenter::class, 'exportXls']);
    });

    // Cancel Reason
    Route::group(['prefix' => 'master/promo-cancel-reason'], function() {
        Route::get('/', [ PromoCancelReason::class, 'landingPage'])->name('mst-promo-cancel-reason');
        Route::get('/list/paginate/filter', [ PromoCancelReason::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ PromoCancelReason::class, 'getDataByID']);
        Route::get('/form', [ PromoCancelReason::class, 'promoCancelReasonFormPage'])->name('mst-promo-cancel-reason.form');
        Route::post('/save', [ PromoCancelReason::class, 'save']);
        Route::post('/update', [ PromoCancelReason::class, 'update']);
        Route::post('/delete', [ PromoCancelReason::class, 'delete']);
        Route::get('/export-xls', [ PromoCancelReason::class, 'exportXls']);
    });

    // Investment Type
    Route::group(['prefix' => 'master/investment-type'], function() {
        Route::get('/', [ InvestmentType::class, 'landingPage'])->name('mst-investment-type');
        Route::get('/list/paginate/filter', [ InvestmentType::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ InvestmentType::class, 'getDataByID']);
        Route::get('/form', [ InvestmentType::class, 'investmentTypeFormPage'])->name('mst-investment-type.form');
        Route::get('/mapping', [ InvestmentType::class, 'investmentTypeMappingPage'])->name('mst-investment-type.mapping');
        Route::get('/mapping/list', [ InvestmentType::class, 'getListMapping']);
        Route::get('/mapping/get-list/investment-type', [ InvestmentType::class, 'getListInvestmentType']);
        Route::get('/mapping/get-list/category', [ InvestmentType::class, 'getListCategory']);
        Route::get('/mapping/get-data/sub-category/category-id', [ InvestmentType::class, 'getDataSubCategory']);
        Route::get('/mapping/get-data/activity/sub-category-id', [ InvestmentType::class, 'getDataActivity']);
        Route::get('/mapping/get-data/sub-activity/activity-id', [ InvestmentType::class, 'getDataSubActivity']);
        Route::post('/mapping/save', [ InvestmentType::class, 'saveMapping']);
        Route::post('/mapping/remove', [ InvestmentType::class, 'removeMapping']);
        Route::post('/save', [ InvestmentType::class, 'save']);
        Route::post('/update', [ InvestmentType::class, 'update']);
        Route::post('/deactivate', [ InvestmentType::class, 'deactivate']);
        Route::post('/activate', [ InvestmentType::class, 'activate']);
        Route::get('/export-xls', [ InvestmentType::class, 'exportXls']);
        Route::get('/mapping/export-xls', [ InvestmentType::class, 'mappingExportXls']);
    });

    // Mechanism Promo
    Route::group(['prefix' => 'master/promo-mechanism'], function() {
        Route::get('/', [ PromoMechanism::class, 'landingPage'])->name('mst-promo-mechanism');
        Route::get('/list/paginate/filter', [ PromoMechanism::class, 'getListPaginateFilter']);
        Route::get('/get-list/entity', [ PromoMechanism::class, 'getListEntity']);
        Route::get('/get-data/attribute', [ PromoMechanism::class, 'getDataAttribute']);
        Route::get('/get-list/channel', [ PromoMechanism::class, 'getListChannel']);
        Route::get('/data/id', [ PromoMechanism::class, 'getDataByID']);
        Route::get('/form', [ PromoMechanism::class, 'promoMechanismFormPage'])->name('mst-promo-mechanism.form');
        Route::get('/upload', [ PromoMechanism::class, 'promoMechanismUploadPage'])->name('mst-promo-mechanism.upload');
        Route::post('/save', [ PromoMechanism::class, 'save']);
        Route::post('/update', [ PromoMechanism::class, 'update']);
        Route::post('/delete', [ PromoMechanism::class, 'delete']);
        Route::post('/upload-xls', [ PromoMechanism::class, 'uploadXls']);
        Route::get('/downloadTemplate', [ PromoMechanism::class, 'downloadTemplate']);
        Route::get('/export-xls', [ PromoMechanism::class, 'exportXls']);
    });

    // Matrix Approval
    Route::group(['prefix' => 'master/matrix/promoapproval'], function() {
        Route::get('/', [ PromoMatrixApproval::class, 'landingPage'])->name('mst-approval-promo');
        Route::get('/process', [ PromoMatrixApproval::class, 'processPage']);
        Route::get('/process/list-promo', [ PromoMatrixApproval::class, 'getListPromo']);
        Route::get('/list', [ PromoMatrixApproval::class, 'getList']);
        Route::get('/get-list/entity', [ PromoMatrixApproval::class, 'getListEntity']);
        Route::get('/get-data/distributor/entity-id', [ PromoMatrixApproval::class, 'getDataDistributor']);
        Route::get('/get-list/category', [ PromoMatrixApproval::class, 'getListCategory']);
        Route::get('/get-list/sub-activity-type', [ PromoMatrixApproval::class, 'getListSubActivityType']);
        Route::get('/get-list/channel', [ PromoMatrixApproval::class, 'getListChannel']);
        Route::get('/get-data/sub-channel/channel-id', [ PromoMatrixApproval::class, 'getDataSubChannel']);
        Route::get('/get-list/profile', [ PromoMatrixApproval::class, 'getListProfile']);
        Route::get('/data/id', [ PromoMatrixApproval::class, 'getDataByID']);
        Route::get('/form', [ PromoMatrixApproval::class, 'promoMatrixApprovalFormPage'])->name('mst-approval-promo.form');
        Route::post('/save', [ PromoMatrixApproval::class, 'save']);
        Route::post('/update', [ PromoMatrixApproval::class, 'update']);
        Route::post('/delete', [ PromoMatrixApproval::class, 'delete']);
        Route::get('/export-xls', [ PromoMatrixApproval::class, 'exportXls']);
        Route::get('/export-xls/historical', [ PromoMatrixApproval::class, 'exportXlsHistorical']);
        Route::post('/process/send-email', [ PromoMatrixApproval::class, 'sendEmail']);
        Route::get('/upload-form', [ PromoMatrixApproval::class, 'uploadPage'])->name('mst-approval-promo.upload-form');
        Route::get('/process-upload', [ PromoMatrixApproval::class, 'processUploadPage']);
        Route::get('/process/list-matrix', [ PromoMatrixApproval::class, 'getListMatrix']);
        Route::post('/upload-xls', [ PromoMatrixApproval::class, 'uploadXls']);
    });
});

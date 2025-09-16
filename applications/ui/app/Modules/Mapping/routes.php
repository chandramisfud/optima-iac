<?php

use Illuminate\Support\Facades\Route;
use App\Modules\Mapping\Controllers\DistributorToSubAccount;
use App\Modules\Mapping\Controllers\UserProfileToSubAccount;
use App\Modules\Mapping\Controllers\SKUBlitz;
use App\Modules\Mapping\Controllers\SubAccountBlitz;
use App\Modules\Mapping\Controllers\TaxLevel;
use App\Modules\Mapping\Controllers\PICDNManual;
use App\Modules\Mapping\Controllers\SubActivityPromoRecon;
use App\Modules\Mapping\Controllers\WHTType;

Route::group(['middleware' => ['web', 'checkSession']], function () {

    // Distributor to Sub Account
    Route::group(['prefix' => '/mapping/distributor-to-sub-account'], function () {
        Route::get('/', [ DistributorToSubAccount::class, 'landingPage'])->name('mapping-distributor-to-sub-account');
        Route::get('/list/paginate/filter', [ DistributorToSubAccount::class, 'getListPaginateFilter']);
        Route::get('/form', [ DistributorToSubAccount::class, 'distributorToSubAccountPage'])->name('mapping-distributor-to-sub-account.form');
        Route::get('/list/distributor', [ DistributorToSubAccount::class, 'getListDistributor']);
        Route::get('/list/channel', [ DistributorToSubAccount::class, 'getListChannel']);
        Route::get('/list/sub-channel/channel-id', [ DistributorToSubAccount::class, 'getListSubChannelByChannelId']);
        Route::get('/list/account/sub-channel-id', [ DistributorToSubAccount::class, 'getListAccountBySubChannelId']);
        Route::get('/list/sub-account/account-id', [ DistributorToSubAccount::class, 'getListSubAccountByAccountId']);
        Route::post('/save', [ DistributorToSubAccount::class, 'save']);
        Route::post('/delete', [ DistributorToSubAccount::class, 'delete']);
        Route::get('/export-xls', [ DistributorToSubAccount::class, 'exportXls']);
    });

    // User Profile to Sub Account
    Route::group(['prefix' => '/mapping/user-profile-to-sub-account'], function () {
        Route::get('/', [ UserProfileToSubAccount::class, 'landingPage'])->name('mapping-user-profile-to-sub-account');
        Route::get('/list/paginate/filter', [ UserProfileToSubAccount::class, 'getListPaginateFilter']);
        Route::get('/form', [ UserProfileToSubAccount::class, 'userProfileToSubAccountPage'])->name('mapping-user-profile-to-sub-account.form');
        Route::get('/list/user-profile', [ UserProfileToSubAccount::class, 'getListUserProfile']);
        Route::get('/list/channel', [ UserProfileToSubAccount::class, 'getListChannel']);
        Route::get('/list/sub-channel/channel-id', [ UserProfileToSubAccount::class, 'getListSubChannelByChannelId']);
        Route::get('/list/account/sub-channel-id', [ UserProfileToSubAccount::class, 'getListAccountBySubChannelId']);
        Route::get('/list/sub-account/account-id', [ UserProfileToSubAccount::class, 'getListSubAccountByAccountId']);
        Route::post('/save', [ UserProfileToSubAccount::class, 'save']);
        Route::post('/delete', [ UserProfileToSubAccount::class, 'delete']);
        Route::get('/export-xls', [ UserProfileToSubAccount::class, 'exportXls']);
    });

    // SKU Blitz
    Route::group(['prefix' => '/mapping/sku-blitz'], function () {
        Route::get('/', [ SKUBlitz::class, 'landingPage'])->name('mapping-sku-blitz');
        Route::get('/list/paginate/filter', [ SKUBlitz::class, 'getListPaginateFilter']);
        Route::get('/form', [ SKUBlitz::class, 'SKUBlitzFormPage'])->name('mapping-sku-blitz.form');
        Route::get('/list/entity', [ SKUBlitz::class, 'getListEntity']);
        Route::get('/list/brand/entity-id', [ SKUBlitz::class, 'getListBrandByEntityId']);
        Route::get('/list/sku/brand-id', [ SKUBlitz::class, 'getListSKUByBrandId']);
        Route::post('/save', [ SKUBlitz::class, 'save']);
        Route::post('/delete', [ SKUBlitz::class, 'delete']);
        Route::get('/export-xls', [ SKUBlitz::class, 'exportXls']);
    });

    // Sub Account Blitz
    Route::group(['prefix' => '/mapping/sub-account-blitz'], function () {
        Route::get('/', [ SubAccountBlitz::class, 'landingPage'])->name('mapping-sub-account-blitz');
        Route::get('/list/paginate/filter', [ SubAccountBlitz::class, 'getListPaginateFilter']);
        Route::get('/form', [ SubAccountBlitz::class, 'subAccountBlitzFormPage'])->name('mapping-sub-account-blitz.form');
        Route::get('/list/channel', [ SubAccountBlitz::class, 'getListChannel']);
        Route::get('/list/sub-channel/channel-id', [ SubAccountBlitz::class, 'getListSubChannelByChannelId']);
        Route::get('/list/account/sub-channel-id', [ SubAccountBlitz::class, 'getListAccountBySubChannelId']);
        Route::get('/list/sub-account/account-id', [ SubAccountBlitz::class, 'getListSubAccountByAccountId']);
        Route::post('/save', [ SubAccountBlitz::class, 'save']);
        Route::post('/delete', [ SubAccountBlitz::class, 'delete']);
        Route::get('/export-xls', [ SubAccountBlitz::class, 'exportXls']);
    });

    // Tax Level
    Route::group(['prefix' => '/mapping/tax-level'], function () {
        Route::get('/', [ TaxLevel::class, 'landingPage' ])->name('mapping-tax-level');
        Route::get('/list/paginate/filter', [ TaxLevel::class, 'getListPaginateFilter']);
        Route::get('/form', [ TaxLevel::class, 'taxLevelFormPage'])->name('mapping-tax-level.form');
        Route::get('/list/entity', [ TaxLevel::class, 'getListEntity']);
        Route::post('/save', [ TaxLevel::class, 'save']);
        Route::post('/delete', [ TaxLevel::class, 'delete']);
        Route::get('/export-xls', [ TaxLevel::class, 'exportXls']);
    });

    // PIC DN Manual
    Route::group(['prefix' => '/mapping/pic-dn-manual'], function () {
        Route::get('/', [ PICDNManual::class, 'landingPage'])->name('mapping-pic-dn-manual');
        Route::get('/list/paginate/filter', [ PICDNManual::class, 'getListPaginateFilter']);
        Route::get('/form', [ PICDNManual::class, 'PICDNFormPage'])->name('mapping-pic-dn-manual.form');
        Route::get('/list/channel', [ PICDNManual::class, 'getListChannel']);
        Route::get('/list/sub-channel/channel-id', [ PICDNManual::class, 'getListSubChannelByChannelId']);
        Route::get('/list/account/sub-channel-id', [ PICDNManual::class, 'getListAccountBySubChannelId']);
        Route::get('/list/sub-account/account-id', [ PICDNManual::class, 'getListSubAccountByAccountId']);
        Route::get('/list/user-profile', [ PICDNManual::class, 'getListUserProfile']);
        Route::post('/save', [ PICDNManual::class, 'save']);
        Route::post('/delete', [ PICDNManual::class, 'delete']);
        Route::get('/export-xls', [ PICDNManual::class, 'exportXls']);
    });

    // Sub Activity Promo Recon
    Route::group(['prefix' => '/mapping/sub-activity-promo-recon'], function () {
        Route::get('/', [ SubActivityPromoRecon::class, 'landingPage'])->name('configuration-sub-activity-promo-recon');
        Route::get('/list/paginate/filter', [ SubActivityPromoRecon::class, 'getListPaginateFilter']);
        Route::get('/form', [ SubActivityPromoRecon::class, 'subActivityPromoReconFormPage'])->name('configuration-sub-activity-promo-recon.form');
        Route::get('/upload-form', [ SubActivityPromoRecon::class, 'subActivityPromoReconUploadPage'])->name('configuration-sub-activity-promo-recon.upload');
        Route::get('/list/category', [ SubActivityPromoRecon::class, 'getListCategory']);
        Route::get('/list/sub-category/category-id', [ SubActivityPromoRecon::class, 'getListSubCategoryByCategoryId']);
        Route::get('/list/activity/sub-category-id', [ SubActivityPromoRecon::class, 'getListActivityBySubCategoryId']);
        Route::get('/list/sub-activity/activity-id', [ SubActivityPromoRecon::class, 'getListSubActivityByActivityId']);
        Route::get('/get-data/id', [ SubActivityPromoRecon::class, 'getDataById']);
        Route::post('/save', [ SubActivityPromoRecon::class, 'save']);
        Route::post('/update', [ SubActivityPromoRecon::class, 'update']);
        Route::post('/delete', [ SubActivityPromoRecon::class, 'delete']);
        Route::post('/upload-xls', [ SubActivityPromoRecon::class, 'uploadXls']);
        Route::get('/export-xls', [ SubActivityPromoRecon::class, 'exportXls']);
        Route::get('/download-template', [ SubActivityPromoRecon::class, 'downloadTemplate']);
    });

    // WHT Type
    Route::group(['prefix' => '/mapping/wht-type'], function () {
        Route::get('/', [ WHTType::class, 'landingPage'])->name('mapping-wht-type');
        Route::get('/list/paginate/filter', [ WHTType::class, 'getListPaginateFilter']);
        Route::get('/form', [ WHTType::class, 'WHTTypeFormPage'])->name('mapping-wht-type.form');
        Route::get('/list/distributor', [ WHTType::class, 'getListDistributor']);
        Route::get('/list/sub-activity', [ WHTType::class, 'getListSubActivity']);
        Route::get('/list/sub-account', [ WHTType::class, 'getListSubAccount']);
        Route::get('/list/wht-type', [ WHTType::class, 'getListWHTType']);
        Route::get('/get-data/id', [ WHTType::class, 'getData']);
        Route::post('/save', [ WHTType::class, 'save']);
        Route::post('/update', [ WHTType::class, 'update']);
        Route::post('/delete', [ WHTType::class, 'delete']);
        Route::get('/download-template', [ WHTType::class, 'downloadTemplate']);
        Route::get('/export-xls', [ WHTType::class, 'exportXls']);
        Route::get('/upload-form', [ WHTType::class, 'WHTTypeFormUploadPage'])->name('mapping-wht-type.upload-form');
        Route::post('/upload-xls', [ WHTType::class, 'uploadXls']);
    });

});

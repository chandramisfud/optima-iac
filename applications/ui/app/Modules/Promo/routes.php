<?php

use App\Modules\Promo\Controllers\PromoPlanning;
use App\Modules\Promo\Controllers\PromoPlanningApproval;
use App\Modules\Promo\Controllers\PromoCreation;
use App\Modules\Promo\Controllers\PromoApproval;
use App\Modules\Promo\Controllers\PromoSendBack;
use App\Modules\Promo\Controllers\PromoRecon;
use App\Modules\Promo\Controllers\PromoReconSendBack;
use App\Modules\Promo\Controllers\PromoApprovalRecon;
use App\Modules\Promo\Controllers\ListingPromoCancel;
use App\Modules\Promo\Controllers\PromoCancelRequest;
use App\Modules\Promo\Controllers\PromoDisplay;
use App\Modules\Promo\Controllers\PromoWorkflow;
use App\Modules\Promo\Controllers\PromoClosure;
use App\Modules\Promo\Controllers\SKPValidation;
use App\Modules\Promo\Controllers\PromoMultiPrint;

use Illuminate\Support\Facades\Route;


Route::group(['middleware' => ['web', 'checkSession']], function () {

    // Promo Planning
    Route::group(['prefix' => '/promo/planning'], function() {
        Route::get('/', [ PromoPlanning::class, 'landingPage'])->name('promo-planning');
        Route::get('/form', [ PromoPlanning::class, 'promoPlanningFormPage'])->name('promo-planning.form');

        Route::get('/list/entity', [ PromoPlanning::class, 'getListEntity']);
        Route::get('/list/distributor', [ PromoPlanning::class, 'getListDistributor']);
        Route::get('/list/paginate/filter', [ PromoPlanning::class, 'getListPaginateFilter']);

        Route::get('/cut-off', [ PromoPlanning::class, 'getCutOff']);
        Route::get('/list/sub-category', [ PromoPlanning::class, 'getListSubCategory']);
        Route::get('/list/activity', [ PromoPlanning::class, 'getListActivity']);
        Route::get('/list/sub-activity', [ PromoPlanning::class, 'getListSubActivity']);

        Route::get('/investment-type', [ PromoPlanning::class, 'getInvestmentType']);
        Route::get('/config-roi-cr', [ PromoPlanning::class, 'getConfigRoiCR']);
        Route::get('/baseline', [ PromoPlanning::class, 'getBaseline']);

        Route::get('/list/channel', [ PromoPlanning::class, 'getListChannel']);
        Route::get('/list/edit/channel', [ PromoPlanning::class, 'getListEditChannel']);
        Route::get('/list/sub-channel', [ PromoPlanning::class, 'getListSubChannel']);
        Route::get('/list/edit/sub-channel', [ PromoPlanning::class, 'getListEditSubChannel']);
        Route::get('/list/account', [ PromoPlanning::class, 'getListAccount']);
        Route::get('/list/edit/account', [ PromoPlanning::class, 'getListEditAccount']);
        Route::get('/list/sub-account', [ PromoPlanning::class, 'getListSubAccount']);
        Route::get('/list/edit/sub-account', [ PromoPlanning::class, 'getListEditSubAccount']);

        Route::get('/list/region', [ PromoPlanning::class, 'getListRegion']);
        Route::get('/list/brand', [ PromoPlanning::class, 'getListBrand']);
        Route::get('/list/sku', [ PromoPlanning::class, 'getListSKU']);
        Route::get('/list/mechanism', [ PromoPlanning::class, 'getListMechanism']);

        Route::get('/export-xls', [ PromoPlanning::class, 'exportXls']);
        Route::get('/upload-xls', [ PromoPlanning::class, 'uploadXls']);

        Route::get('/exist', [ PromoPlanning::class, 'getPromoPlanningExist']);
        Route::post('/save', [ PromoPlanning::class, 'save']);
        Route::get('/data/id', [ PromoPlanning::class, 'getDataByID']);
        Route::post('/update', [ PromoPlanning::class, 'update']);
        Route::post('/cancel', [ PromoPlanning::class, 'cancel']);

        //to be created
        Route::get('/to-be-created', [ PromoPlanning::class, 'toBeCreated']);
        Route::get('/list/to-be-created', [ PromoPlanning::class, 'getListToBeCreated']);
        Route::get('/check-form-access', [ PromoPlanning::class, 'checkAccess']);
    });

    // Promo Planning Approval
    Route::group(['prefix' => '/promo/planning-approval'], function() {
        Route::get('/', [ PromoPlanningApproval::class, 'landingPage' ])->name('promo-plan-approval');
        Route::get('/download-template', [ PromoPlanningApproval::class, 'downloadTemplate' ]);
        Route::post('/approve', [ PromoPlanningApproval::class, 'uploadApprove' ]);
    });

    // Promo Creation
    Route::group(['prefix' => '/promo/creation'], function() {
        Route::get('/', [ PromoCreation::class, 'landingPage'])->name('promo-creation');
        Route::get('/form', [ PromoCreation::class, 'promoCreationFormPage'])->name('promo-creation.form');
        Route::get('/form-generate', [ PromoCreation::class, 'generatePromoForm'])->name('promo-creation.form-generate');

        Route::get('/category/category-desc', [ PromoCreation::class, 'getCategoryByCategoryShortDesc' ]);
        Route::get('/promo-attribute', [ PromoCreation::class, 'getPromoAttributeList']);
        Route::get('/filter-generate-promo', [ PromoCreation::class, 'getFilterGenerate']);
        Route::get('/budget', [ PromoCreation::class, 'getDataBudget']);
        Route::get('/mechanism', [ PromoCreation::class, 'getDataMechanism']);
        Route::get('/baseline', [ PromoCreation::class, 'getDataBaseline']);
        Route::get('/total-sales', [ PromoCreation::class, 'getDataTotalSales']);
        Route::get('/total-sales-dc', [ PromoCreation::class, 'getDataTotalSalesDC']);
        Route::get('/cr', [ PromoCreation::class, 'getDataCR']);
        Route::post('/attachment-upload', [ PromoCreation::class, 'uploadFile']);
        Route::post('/attachment-delete', [ PromoCreation::class, 'deleteFile']);
        Route::get('/late-promo-days', [ PromoCreation::class, 'getLatePromoDays']);
        Route::get('/exist', [ PromoCreation::class, 'getPromoExist']);
        Route::get('/id', [ PromoCreation::class, 'getData']);
        Route::get('/dc/id', [ PromoCreation::class, 'getDataDC']);
        Route::get('/recon/id', [ PromoCreation::class, 'getDataRecon']);
        Route::get('/dc-recon/id', [ PromoCreation::class, 'getDataReconDC']);
        Route::get('/display/id', [ PromoCreation::class, 'getDataDisplay']);
        Route::get('/display-recon/id', [ PromoCreation::class, 'getDataDisplayRecon']);
        Route::post('/revamp/save', [ PromoCreation::class, 'savePromo']);
        Route::post('/revamp/save-dc', [ PromoCreation::class, 'savePromoDC']);
        Route::post('/revamp/update', [ PromoCreation::class, 'updatePromo']);
        Route::post('/revamp/update-dc', [ PromoCreation::class, 'updatePromoDC']);
        Route::post('/recon/update', [ PromoCreation::class, 'updatePromoRecon']);
        Route::post('/recon/update-dc', [ PromoCreation::class, 'updatePromoReconDC']);
        Route::post('/generate-promo', [ PromoCreation::class, 'generatePromo']);

        Route::get('/form-cancel-request', [ PromoCreation::class, 'promoCancelRequestFormPage'])->name('promo-creation.form-cancel-request');

        Route::get('/list/category', [ PromoCreation::class, 'getListCategory']);
        Route::get('/list/entity', [ PromoCreation::class, 'getListEntity']);
        Route::get('/list/distributor', [ PromoCreation::class, 'getListDistributor']);
        Route::get('/list/sub-category', [ PromoCreation::class, 'getListSubCategory']);
        Route::get('/list/activity', [ PromoCreation::class, 'getListActivity']);
        Route::get('/list/sub-activity', [ PromoCreation::class, 'getListSubActivity']);

        Route::get('/list/channel', [ PromoCreation::class, 'getListChannel']);
        Route::get('/list/sub-channel', [ PromoCreation::class, 'getListSubChannel']);
        Route::get('/list/account', [ PromoCreation::class, 'getListAccount']);
        Route::get('/list/sub-account', [ PromoCreation::class, 'getListSubAccount']);

        Route::get('/list/edit/channel', [ PromoCreation::class, 'getListEditChannel']);
        Route::get('/list/edit/sub-channel', [ PromoCreation::class, 'getListEditSubChannel']);
        Route::get('/list/edit/account', [ PromoCreation::class, 'getListEditAccount']);
        Route::get('/list/edit/sub-account', [ PromoCreation::class, 'getListEditSubAccount']);

        Route::get('/list/region', [ PromoCreation::class, 'getListRegion']);
        Route::get('/list/brand', [ PromoCreation::class, 'getListBrand']);
        Route::get('/list/sku', [ PromoCreation::class, 'getListSKU']);
        Route::get('/list/mechanism', [ PromoCreation::class, 'getListMechanism']);
        Route::get('/cutoff', [ PromoCreation::class, 'getCutOff']);
        Route::get('/investment-type', [ PromoCreation::class, 'getInvestmentType']);
        Route::get('/config-roi-cr', [ PromoCreation::class, 'getConfigRoiCR']);

        Route::get('/list/source-planning', [ PromoCreation::class, 'getListSourcePromoPlanning']);
        Route::get('/data/promo-planning-id', [ PromoCreation::class, 'getDataByPromoPlanningID']);
        Route::get('/list/source-budget', [ PromoCreation::class, 'getListSourceBudget']);
        Route::get('/list/cancel-reason', [ PromoCreation::class, 'getListCancelReason']);

        Route::get('/list/paginate/filter', [ PromoCreation::class, 'getListPaginateFilter']);
        Route::get('/data/id', [ PromoCreation::class, 'getDataByID']);
        Route::post('/save', [ PromoCreation::class, 'save']);
        Route::post('/update', [ PromoCreation::class, 'update']);
        Route::post('/save-cancel-request', [ PromoCreation::class, 'saveCancelRequest']);
        Route::get('/export-xls', [ PromoCreation::class, 'exportXls']);

        // DC
        Route::get('/list/sub-category/category-id', [ PromoCreation::class, 'getListSubCategoryByCategoryId']);
        Route::get('/list/sub-activity/sub-category-id', [ PromoCreation::class, 'getListSubActivityBySubCategoryId']);
        Route::get('/list/channel-master', [ PromoCreation::class, 'getListChannelMaster']);
        Route::get('/list/sub-channel/channels', [ PromoCreation::class, 'getListSubChannelByChannels']);
        Route::get('/list/account/sub-channels', [ PromoCreation::class, 'getListAccountBySubChannels']);
        Route::get('/list/sub-account/accounts', [ PromoCreation::class, 'getListSubAccountByAccounts']);
        Route::get('/list/group-brand', [ PromoCreation::class, 'getListGroupBrand']);
        Route::get('/list/brand/group-brand-id', [ PromoCreation::class, 'getListBrandByGroupBrandId']);
        Route::get('/exist/dc', [ PromoCreation::class, 'getPromoExistDC']);
    });

    // Promo Approval
    Route::group(['prefix' => '/promo/approval'], function() {
        Route::get('/', [ PromoApproval::class, 'landingPage' ])->name('promo-approval');
        Route::get('/form-approve', [ PromoApproval::class, 'formApprove' ])->name('promo-approval.form-approve');
        Route::get('/id', [ PromoApproval::class, 'getDataPromo' ]);
        Route::get('/dc/id', [ PromoApproval::class, 'getDataPromoDC' ]);
        Route::get('/list', [ PromoApproval::class, 'getList' ]);
        Route::get('/list/category', [ PromoApproval::class, 'getListCategory']);
        Route::get('/list/entity', [ PromoApproval::class, 'getListEntity' ]);
        Route::get('/list/distributor', [ PromoApproval::class, 'getListDistributor' ]);
        Route::get('/list/sub-activity-type', [ PromoApproval::class, 'getListSubactivityType' ]);
        Route::get('/data/id', [ PromoApproval::class, 'getPromoById' ]);
        Route::get('/preview-attachment', [ PromoApproval::class, 'previewAttachment' ]);
        Route::post('/approve', [ PromoApproval::class, 'approve' ]);
        Route::post('/send-back', [ PromoApproval::class, 'sendBack' ]);
    });

    // Promo Send Back
    Route::group(['prefix' => '/promo/send-back'], function() {
        Route::get('/', [ PromoSendBack::class, 'landingPage' ])->name('promo-send-back');
        Route::get('/form', [ PromoSendBack::class, 'form' ])->name('promo-send-back.form');
        Route::get('/list', [ PromoSendBack::class, 'getListPaginateFilter' ]);

        Route::get('/promo-attribute', [ PromoSendBack::class, 'getPromoAttributeList']);
        Route::get('/budget', [ PromoSendBack::class, 'getDataBudget']);
        Route::get('/mechanism', [ PromoSendBack::class, 'getDataMechanism']);
        Route::get('/baseline', [ PromoSendBack::class, 'getDataBaseline']);
        Route::get('/total-sales', [ PromoSendBack::class, 'getDataTotalSales']);
        Route::get('/cr', [ PromoSendBack::class, 'getDataCR']);
        Route::get('/id', [ PromoSendBack::class, 'getData']);
        Route::get('/dc/id', [ PromoSendBack::class, 'getDataDC']);
        Route::get('/recon/id', [ PromoSendBack::class, 'getDataRecon']);
        Route::get('/dc-recon/id', [ PromoSendBack::class, 'getDataReconDC']);
        Route::post('/revamp/update', [ PromoSendBack::class, 'updatePromo']);
        Route::post('/revamp/update-dc', [ PromoSendBack::class, 'updatePromoDC']);
        Route::post('/revamp/update-recon', [ PromoSendBack::class, 'updatePromoRecon']);
        Route::post('/revamp/update-recon-dc', [ PromoSendBack::class, 'updatePromoReconDC']);


        Route::get('/data/id', [ PromoSendBack::class, 'getDataById' ]);

        Route::get('/list/category', [ PromoSendBack::class, 'getListCategory' ]);
        Route::get('/list/entity', [ PromoSendBack::class, 'getListEntity']);
        Route::get('/list/distributor', [ PromoSendBack::class, 'getListDistributor']);

        Route::get('/list/activity', [ PromoSendBack::class, 'getListActivity']);
        Route::get('/list/sub-activity', [ PromoSendBack::class, 'getListSubActivity']);

        Route::get('/list/sub-activity-type', [ PromoSendBack::class, 'getListSubactivityType' ]);

        Route::get('/category/category-desc', [ PromoSendBack::class, 'getCategoryByCategoryShortDesc' ]);

        Route::get('/list/channel-master', [ PromoSendBack::class, 'getListChannelMaster']);
        Route::get('/list/sub-category', [ PromoSendBack::class, 'getListSubCategory']);
        Route::get('/list/group-brand', [ PromoSendBack::class, 'getListGroupBrand']);
        Route::get('/list/brand/groupbrandid', [ PromoSendBack::class, 'getListBrandByGroupBrandId']);

        Route::get('/list/edit/channel', [ PromoSendBack::class, 'getListEditChannel']);
        Route::get('/list/edit/sub-channel', [ PromoSendBack::class, 'getListEditSubChannel']);
        Route::get('/list/edit/account', [ PromoSendBack::class, 'getListEditAccount']);
        Route::get('/list/edit/sub-account', [ PromoSendBack::class, 'getListEditSubAccount']);

        Route::get('/investment-type', [ PromoSendBack::class, 'getInvestmentType']);
        Route::get('/config-roi-cr', [ PromoSendBack::class, 'getConfigRoiCR']);
        Route::get('/cutoff', [ PromoSendBack::class, 'getCutOff']);

        Route::get('/list/source-budget', [ PromoSendBack::class, 'getListSourceBudget']);
        Route::get('/list/mechanism', [ PromoSendBack::class, 'getListMechanism']);
        Route::get('/list/region', [ PromoSendBack::class, 'getListRegion']);
        Route::get('/list/brand', [ PromoSendBack::class, 'getListBrand']);
        Route::get('/list/sku', [ PromoSendBack::class, 'getListSKU']);
        Route::post('/attachment-upload', [ PromoSendBack::class, 'uploadFile']);
        Route::post('/attachment-delete', [ PromoSendBack::class, 'deleteFile']);
        Route::post('/update', [ PromoSendBack::class, 'update']);
        Route::get('/export-xls', [ PromoSendBack::class, 'exportXls']);
    });

    // Promo Reconciliation
    Route::group(['prefix' => '/promo/recon'], function() {
        Route::get('/', [ PromoRecon::class, 'landingPage' ])->name('promo-reconciliation');
        Route::get('/form', [ PromoRecon::class, 'form' ])->name('promo-reconciliation.form');
        Route::get('/form-cancel-request', [ PromoRecon::class, 'promoCancelRequestFormPage'])->name('promo-reconciliation.form-cancel-request');

        Route::get('/list', [ PromoRecon::class, 'getListPaginateFilter' ]);
        Route::get('/export-xls', [ PromoRecon::class, 'exportXls']);

        Route::get('/list/category', [ PromoRecon::class, 'getListCategory']);
        Route::get('/list/entity', [ PromoRecon::class, 'getListEntity']);
        Route::get('/list/distributor', [ PromoRecon::class, 'getListDistributor']);
        Route::get('/list/channel', [ PromoRecon::class, 'getListChannel']);

        Route::get('/category/category-desc', [ PromoRecon::class, 'getCategoryByCategoryShortDesc' ]);
        Route::get('/list/sub-category', [ PromoRecon::class, 'getListSubCategory']);

        Route::get('/list/activity', [ PromoRecon::class, 'getListActivity']);
        Route::get('/list/sub-activity', [ PromoRecon::class, 'getListSubActivity']);

        Route::get('/investment-type', [ PromoRecon::class, 'getInvestmentType']);
        Route::get('/cut-off', [ PromoRecon::class, 'getCutOff']);
        Route::get('/config-roi-cr', [ PromoRecon::class, 'getConfigRoiCR']);
        Route::get('/baseline', [ PromoRecon::class, 'getBaseline']);

        Route::get('/list/edit/channel', [ PromoRecon::class, 'getListEditChannel']);
        Route::get('/list/edit/sub-channel', [ PromoRecon::class, 'getListEditSubChannel']);
        Route::get('/list/edit/account', [ PromoRecon::class, 'getListEditAccount']);
        Route::get('/list/edit/sub-account', [ PromoRecon::class, 'getListEditSubAccount']);

        Route::get('/list/region', [ PromoRecon::class, 'getListRegion']);
        Route::get('/list/brand', [ PromoRecon::class, 'getListBrand']);
        Route::get('/list/sku', [ PromoRecon::class, 'getListSKU']);
        Route::get('/list/mechanism', [ PromoRecon::class, 'getListMechanism']);

        Route::get('/data/id', [ PromoRecon::class, 'getDataById' ]);

        Route::post('/update', [ PromoRecon::class, 'update']);

        Route::post('/attachment-upload', [ PromoRecon::class, 'uploadFile']);
        Route::post('/attachment-delete', [ PromoRecon::class, 'deleteFile']);

        Route::get('/list/cancel-reason', [ PromoRecon::class, 'getListCancelReason']);
        Route::post('/save-cancel-request', [ PromoRecon::class, 'saveCancelRequest']);

        Route::get('/list/source-budget', [ PromoRecon::class, 'getListSourceBudget']);

        // DC
        Route::get('/list/sub-category/category-id', [ PromoRecon::class, 'getListSubCategoryByCategoryId']);
        Route::get('/list/sub-activity/sub-category-id', [ PromoRecon::class, 'getListSubActivityBySubCategoryId']);
        Route::get('/list/channel-master', [ PromoRecon::class, 'getListChannelMaster']);
        Route::get('/list/sub-channel/channels', [ PromoRecon::class, 'getListSubChannelByChannels']);
        Route::get('/list/account/sub-channels', [ PromoRecon::class, 'getListAccountBySubChannels']);
        Route::get('/list/sub-account/accounts', [ PromoRecon::class, 'getListSubAccountByAccounts']);
        Route::get('/list/group-brand', [ PromoRecon::class, 'getListGroupBrand']);
        Route::get('/list/brand/group-brand-id', [ PromoRecon::class, 'getListBrandByGroupBrandId']);
    });

    // Promo Send Back Reconciliation
    Route::group(['prefix' => '/promo/recon-send-back'], function() {
        Route::get('/', [ PromoReconSendBack::class, 'landingPage' ])->name('promo-recon-send-back');
        Route::get('/form', [ PromoReconSendBack::class, 'form' ])->name('promo-recon-send-back.form');
        Route::get('/list', [ PromoReconSendBack::class, 'getListPaginateFilter' ]);
        Route::get('/data/id', [ PromoReconSendBack::class, 'getDataById' ]);

        Route::get('/list/category', [ PromoReconSendBack::class, 'getListCategory']);
        Route::get('/list/entity', [ PromoReconSendBack::class, 'getListEntity']);
        Route::get('/list/distributor', [ PromoReconSendBack::class, 'getListDistributor']);
        Route::get('/list/activity', [ PromoReconSendBack::class, 'getListActivity']);
        Route::get('/list/sub-activity', [ PromoReconSendBack::class, 'getListSubActivity']);

        Route::get('/list/sub-activity/sub-category-id', [ PromoReconSendBack::class, 'getListSubActivityBySubCategoryId']);
        Route::get('/category/category-desc', [ PromoReconSendBack::class, 'getCategoryByCategoryShortDesc' ]);
        Route::get('/list/sub-category', [ PromoReconSendBack::class, 'getListSubCategory']);

        Route::get('/investment-type', [ PromoReconSendBack::class, 'getInvestmentType']);
        Route::get('/config-roi-cr', [ PromoReconSendBack::class, 'getConfigRoiCR']);
        Route::get('/cut-off', [ PromoReconSendBack::class, 'getCutOff']);
        Route::get('/baseline', [ PromoReconSendBack::class, 'getBaseline']);

        Route::get('/list/channel-master', [ PromoReconSendBack::class, 'getListChannelMaster']);
        Route::get('/list/sub-category', [ PromoReconSendBack::class, 'getListSubCategory']);
        Route::get('/list/group-brand', [ PromoReconSendBack::class, 'getListGroupBrand']);
        Route::get('/list/brand/groupbrandid', [ PromoReconSendBack::class, 'getListBrandByGroupBrandId']);

        Route::get('/list/source-budget', [ PromoReconSendBack::class, 'getListSourceBudget']);

        Route::get('/list/edit/channel', [ PromoReconSendBack::class, 'getListEditChannel']);
        Route::get('/list/edit/sub-channel', [ PromoReconSendBack::class, 'getListEditSubChannel']);
        Route::get('/list/edit/account', [ PromoReconSendBack::class, 'getListEditAccount']);
        Route::get('/list/edit/sub-account', [ PromoReconSendBack::class, 'getListEditSubAccount']);
        Route::get('/list/mechanism', [ PromoReconSendBack::class, 'getListMechanism']);
        Route::get('/list/region', [ PromoReconSendBack::class, 'getListRegion']);
        Route::get('/list/brand', [ PromoReconSendBack::class, 'getListBrand']);
        Route::get('/list/sku', [ PromoReconSendBack::class, 'getListSKU']);

        Route::post('/attachment-upload', [ PromoReconSendBack::class, 'uploadFile']);
        Route::post('/attachment-delete', [ PromoReconSendBack::class, 'deleteFile']);

        Route::post('/update', [ PromoReconSendBack::class, 'update']);
        Route::get('/export-xls', [ PromoReconSendBack::class, 'exportXls']);
    });

    // Promo Approval Recon
    Route::group(['prefix' => '/promo/approval-recon'], function() {
        Route::get('/', [ PromoApprovalRecon::class, 'landingPage' ])->name('promo-reconcile-approval');
        Route::get('/form-approve', [ PromoApprovalRecon::class, 'formApprove' ])->name('promo-reconcile-approval.form-approve');
        Route::get('/list', [ PromoApprovalRecon::class, 'getList' ]);
        Route::get('/list/category', [ PromoApprovalRecon::class, 'getListCategory']);
        Route::get('/list/entity', [ PromoApprovalRecon::class, 'getListEntity' ]);
        Route::get('/list/distributor', [ PromoApprovalRecon::class, 'getListDistributor' ]);
        Route::get('/data/id', [ PromoApprovalRecon::class, 'getPromoById' ]);
        Route::get('/id', [ PromoApprovalRecon::class, 'getDataPromo' ]);
        Route::get('/dc/id', [ PromoApprovalRecon::class, 'getDataPromoDC' ]);
        Route::get('/list/dn-claimed', [ PromoApprovalRecon::class, 'getListDNClaimed' ]);
        Route::get('/list/dn-paid', [ PromoApprovalRecon::class, 'getListDNPaid' ]);
        Route::get('/dn', [ PromoApprovalRecon::class, 'dnPage' ])->name('promo-reconcile-approval.form-dn');
        Route::get('/dn/id', [ PromoApprovalRecon::class, 'getDataDN' ]);
        Route::get('/list/tax-level', [ PromoApprovalRecon::class, 'getListTaxLevel' ]);
        Route::get('/preview-attachment', [ PromoApprovalRecon::class, 'previewAttachment' ]);
        Route::post('/approve', [ PromoApprovalRecon::class, 'approve' ]);
        Route::post('/send-back', [ PromoApprovalRecon::class, 'sendBack' ]);
    });

    // Listing Promo Cancel
    Route::group(['prefix' => '/promo/listing-promo-cancel'], function() {
        Route::get('/', [ListingPromoCancel::class, 'landingPage'])->name('promo-listing-promo-cancel');
        Route::get('/list', [ListingPromoCancel::class, 'getList']);
        Route::get('/list/entity', [ListingPromoCancel::class, 'getListEntity']);
        Route::get('/list/distributor', [ListingPromoCancel::class, 'getListDistributor']);
        Route::get('/export-xls', [ListingPromoCancel::class, 'exportXls']);
    });

    // Promo Cancel Request
    Route::group(['prefix' => '/promo/cancel-request'], function() {
        Route::get('/', [PromoCancelRequest::class, 'landingPage'])->name('promo-cancel-request');
        Route::get('/form-cancel', [PromoCancelRequest::class, 'form'])->name('promo-cancel-request.form');
        Route::get('/list', [PromoCancelRequest::class, 'getList']);
        Route::get('/data/id', [PromoCancelRequest::class, 'getDataById']);
        Route::get('/data-revamp/id', [ PromoCancelRequest::class, 'getDataV2ById']);
        Route::get('/id', [PromoCancelRequest::class, 'getDataPromo']);
        Route::get('/recon/id', [PromoCancelRequest::class, 'getDataPromoRecon']);
        Route::get('/list/entity', [PromoCancelRequest::class, 'getListEntity']);
        Route::get('/list/distributor', [PromoCancelRequest::class, 'getListDistributor']);
        Route::get('/preview-attachment', [ PromoCancelRequest::class, 'previewAttachment' ]);
        Route::post('/approve', [ PromoCancelRequest::class, 'approve' ]);
        Route::post('/reject', [ PromoCancelRequest::class, 'reject' ]);
    });

    // Promo Display
    Route::group(['prefix' => '/promo/display'], function() {
        Route::get('/', [PromoDisplay::class, 'landingPage'])->name('promo-display');
        Route::get('/form', [PromoDisplay::class, 'form'])->name('promo-display.form');
        Route::get('/list', [PromoDisplay::class, 'getList']);
        Route::get('/data/id', [PromoDisplay::class, 'getDataById']);
        Route::get('/data-revamp/id', [ PromoDisplay::class, 'getDataV2ById']);
        Route::get('/list/entity', [PromoDisplay::class, 'getListEntity']);
        Route::get('/list/distributor', [PromoDisplay::class, 'getListDistributor']);
        Route::get('/list/sub-activity-type', [ PromoDisplay::class, 'getListSubactivityType' ]);
        Route::get('/preview-attachment', [ PromoDisplay::class, 'previewAttachment' ]);
        Route::get('/export-pdf', [ PromoDisplay::class, 'exportPdf']);
    });

    // Promo Workflow
    Route::group(['prefix' => '/promo/workflow'], function () {
        Route::get('/', [ PromoWorkflow::class, 'landingPage' ])->name('promo-workflow');
        Route::get('/data', [ PromoWorkflow::class, 'getDataPromo' ]);
        Route::get('/data-revamp/refId', [ PromoWorkflow::class, 'getDataPromoByRefId' ]);
        Route::get('/workflow-history', [ PromoWorkflow::class, 'getListWorkflowHistory' ]);
        Route::get('/change-history', [ PromoWorkflow::class, 'getListChangeHistory' ]);
        Route::get('/list-dn', [ PromoWorkflow::class, 'getListDN' ]);
        Route::get('/approval-workflow', [ PromoWorkflow::class, 'getPromoApprovalWorkflow' ]);
        Route::get('/print-pdf', [ PromoWorkflow::class, 'printPdf' ]);
        Route::get('/preview-attachment', [ PromoDisplay::class, 'previewAttachment' ]);
    });

    // Promo Closure
    Route::group(['prefix' => '/promo/closure'], function() {
        Route::get('/', [PromoClosure::class, 'landingPage'])->name('promo-closure');
        Route::get('/form', [PromoClosure::class, 'form'])->name('promo-closure.form');
        Route::get('/upload-form', [ PromoClosure::class, 'uploadPage'])->name('promo-closure.upload-form');
        Route::get('/list', [PromoClosure::class, 'getList']);
        Route::get('/data/id', [PromoClosure::class, 'getDataById']);
        Route::get('/data-revamp/id', [ PromoClosure::class, 'getDataV2ById']);
        Route::post('/open', [PromoClosure::class, 'open']);
        Route::post('/close', [PromoClosure::class, 'close']);
        Route::get('/list/entity', [PromoClosure::class, 'getListEntity']);
        Route::get('/list/distributor', [PromoClosure::class, 'getListDistributor']);
        Route::get('/list/channel', [ PromoClosure::class, 'getListChannel']);
        Route::get('/export-xls', [PromoClosure::class, 'exportXls']);
        Route::get('/download-template', [PromoClosure::class, 'downloadTemplate']);
        Route::post('/upload-xls', [ PromoClosure::class, 'uploadXls']);
        Route::get('/preview-attachment', [ PromoClosure::class, 'previewAttachment' ]);
    });

    // SKP Validation
    Route::group(['prefix' => '/promo/skp-validation'], function() {
        Route::get('/', [SKPValidation::class, 'landingPage'])->name('promo-skp-validation');
        Route::get('/form', [SKPValidation::class, 'form'])->name('promo-skp-validation.form');
        Route::get('/list', [SKPValidation::class, 'getList']);
        Route::get('/data/id', [SKPValidation::class, 'getDataById']);
        Route::get('/list/entity', [SKPValidation::class, 'getListEntity']);
        Route::get('/list/distributor', [SKPValidation::class, 'getListDistributor']);
        Route::get('/list/channel', [ SKPValidation::class, 'getListChannel']);
        Route::get('/flagging', [SKPValidation::class, 'getFlagging'])->name('promo-skp-validation.flagging');
        Route::post('/update', [ SKPValidation::class, 'update' ]);

        Route::get('/export-xls', [SKPValidation::class, 'exportXls']);
    });

    // Promo Multi Print
    Route::group(['prefix' => '/promo/multi-print'], function() {
        Route::get('/', [PromoMultiPrint::class, 'landingPage'])->name('promo-multi-print');
        Route::get('/list', [ PromoMultiPrint::class, 'getList']);
        Route::get('/list/entity', [ PromoMultiPrint::class, 'getListEntity']);
        Route::get('/print-pdf', [ PromoMultiPrint::class, 'printPdf']);

    });
});

Route::group(['prefix' => '/promo/approval'], function() {
    Route::get('/email/send-back', [ PromoApproval::class, 'sendBackPage' ]);
    Route::get('/email/approve', [ PromoApproval::class, 'approvePage' ]);
    Route::get('/email/data/id', [ PromoApproval::class, 'getEncryptedPromo' ]);
    Route::post('/email/approve/submit', [ PromoApproval::class, 'approveLinkEmail' ]);
    Route::post('/email/send-back/submit', [ PromoApproval::class, 'sendBackLinkEmail' ]);
});

Route::group(['prefix' => '/promo/approval-recon'], function() {
    Route::get('/email/send-back', [ PromoApprovalRecon::class, 'sendBackPage' ]);
    Route::get('/email/approve', [ PromoApprovalRecon::class, 'approvePage' ]);
    Route::get('/email/data/id', [ PromoApprovalRecon::class, 'getEncryptedPromo' ]);
    Route::post('/email/approve/submit', [ PromoApprovalRecon::class, 'approveLinkEmail' ]);
    Route::post('/email/send-back/submit', [ PromoApprovalRecon::class, 'sendBackLinkEmail' ]);
});

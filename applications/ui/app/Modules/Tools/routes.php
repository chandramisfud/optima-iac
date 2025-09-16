<?php

use Illuminate\Support\Facades\Route;

use App\Modules\Tools\Controllers\Region;
use App\Modules\Tools\Controllers\Entity;
use App\Modules\Tools\Controllers\Distributor;
use App\Modules\Tools\Controllers\Brand;
use App\Modules\Tools\Controllers\Channel;
use App\Modules\Tools\Controllers\SubActivityType;
use App\Modules\Tools\Controllers\Activity;
use App\Modules\Tools\Controllers\Budget;
use App\Modules\Tools\Controllers\MatrixApproval;
use App\Modules\Tools\Controllers\SAPPayment;
use App\Modules\Tools\Controllers\PromoUploadAttachmentCreator;
use App\Modules\Tools\Controllers\PromoUploadAttachmentAdmin;
use App\Modules\Tools\Controllers\BlitzRawData;
use App\Modules\Tools\Controllers\BudgetAdjustment;
use App\Modules\Tools\Controllers\SAPAccrual;
use App\Modules\Tools\Controllers\XMLPaymentReset;
use App\Modules\Tools\Controllers\NavApp;
use App\Modules\Tools\Controllers\UpdatePromoReconStatus;

Route::group(['middleware' => ['web', 'checkSession']], function () {

    // Region
    Route::group(['prefix' => 'tools/upload-region'], function() {
        Route::get('/', [ Region::class, 'uploadPage'])->name('tools-upload-region');
        Route::post('/upload-xls', [ Region::class, 'uploadXls']);
    });

    // Entity
    Route::group(['prefix' => 'tools/upload-entity'], function() {
        Route::get('/', [ Entity::class, 'uploadPage'])->name('tools-upload-entity');
        Route::post('/upload-xls', [ Entity::class, 'uploadXls']);
    });

    // Distributor
    Route::group(['prefix' => 'tools/upload-distributor'], function() {
        Route::get('/', [ Distributor::class, 'uploadPage'])->name('tools-upload-distributor');
        Route::post('/upload-xls', [ Distributor::class, 'uploadXls']);
    });

    // Brand
    Route::group(['prefix' => 'tools/upload-brand'], function() {
        Route::get('/', [ Brand::class, 'uploadPage'])->name('tools-upload-brand');
        Route::post('/upload-xls', [ Brand::class, 'uploadXls']);
    });

    // Channel
    Route::group(['prefix' => 'tools/upload-channel'], function() {
        Route::get('/', [ Channel::class, 'uploadPage'])->name('tools-upload-channel');
        Route::post('/upload-xls', [ Channel::class, 'uploadXls']);
    });

    // Sub Activity Type
    Route::group(['prefix' => 'tools/upload-sub-activity-type'], function() {
        Route::get('/', [ SubActivityType::class, 'uploadPage'])->name('tools-upload-sub-activity-type');
        Route::post('/upload-xls', [ SubActivityType::class, 'uploadXls']);
    });

    // Activity
    Route::group(['prefix' => 'tools/upload-activity'], function() {
        Route::get('/', [ Activity::class, 'uploadPage'])->name('tools-upload-activity');
        Route::post('/upload-xls', [ Activity::class, 'uploadXls']);
    });

    // Budget
    Route::group(['prefix' => 'tools/upload-budget'], function() {
        Route::get('/', [ Budget::class, 'uploadPage'])->name('tools-upload-budget');
        Route::post('/upload-xls', [ Budget::class, 'uploadXls']);
        Route::post('/upload-xls/dc', [ Budget::class, 'uploadXlsDC']);
    });

    // Matrix Approval
    Route::group(['prefix' => 'tools/upload-matrix-approval'], function() {
        Route::get('/', [ MatrixApproval::class, 'uploadPage'])->name('tools-upload-matrix-approval');
        Route::get('/process', [ MatrixApproval::class, 'processPage']);
        Route::get('/process/list-matrix', [ MatrixApproval::class, 'getListMatrix']);
        Route::get('/process/list-promo', [ MatrixApproval::class, 'getListPromo']);
        Route::post('/upload-xls', [ MatrixApproval::class, 'uploadXls']);
        Route::post('/process/send-email', [ MatrixApproval::class, 'sendEmail']);
    });

    // Transfer to SAP payment
    Route::group(['prefix' => 'tools/sap-payment'], function() {
        Route::get('/', [ SAPPayment::class, 'uploadPage'])->name('tools-sap-payment');
        Route::get('/get-list/upload-history', [ SAPPayment::class, 'getListUploadHistory']);
        Route::get('/get-list/entity', [ SAPPayment::class, 'getListEntity']);
        Route::get('/get-data/distributor/entity-id', [ SAPPayment::class, 'getDataDistributor']);
        Route::get('/get-list/upload-history', [ SAPPayment::class, 'getListUploadHistory']);
        Route::get('/cek-flag', [ SAPPayment::class, 'cekFlag']);
        Route::get('/xml-generate/batch-name', [ SAPPayment::class, 'generateBatchName']);

        Route::get('/xml-generate/data/payment-alone-non-batching', [ SAPPayment::class, 'getDataDistributorPaymentAloneNonBatching' ]);
        Route::get('/xml-generate/data/payment-alone-batching', [ SAPPayment::class, 'getDataDistributorPaymentAloneBatching' ]);
        Route::get('/xml-generate/data/xml-non-batching', [ SAPPayment::class, 'getDataXmlNonBatching' ]);
        Route::get('/xml-generate/data/xml-batching', [ SAPPayment::class, 'getDataXmlBatching' ]);

        Route::get('/xml-generate/download/payment-alone-non-batching', [ SAPPayment::class, 'downloadDistributorPaymentAloneNonBatching' ]);
        Route::get('/xml-generate/download/payment-alone-batching', [ SAPPayment::class, 'downloadDistributorPaymentAloneBatching' ]);
        Route::get('/xml-generate/download/xml-non-batching', [ SAPPayment::class, 'downloadXmlNonBatching' ]);
        Route::get('/xml-generate/download/xml-batching', [ SAPPayment::class, 'downloadXmlBatching' ]);
        Route::get('/xml-generate/flagging-payment', [ SAPPayment::class, 'flaggingPayment' ]);

        Route::post('/upload-xml', [ SAPPayment::class, 'uploadXml']);
    });

    // Upload Attachment Promo Creator
    Route::group(['prefix' => 'tools/upload-attachment-promo-creator'], function() {
        Route::get('/', [ PromoUploadAttachmentCreator::class, 'uploadPage'])->name('tools-upload-promo-attachment-creator');
        Route::get('/download-template', [ PromoUploadAttachmentCreator::class, 'downloadTemplate']);
        Route::post('/temp', [ PromoUploadAttachmentCreator::class, 'temp']);
        Route::post('/temp-delete', [ PromoUploadAttachmentCreator::class, 'tempDelete']);
        Route::post('/importData', [ PromoUploadAttachmentCreator::class, 'importData']);
        Route::post('/import', [ PromoUploadAttachmentCreator::class, 'import']);
        Route::get('/read-excel', [ PromoUploadAttachmentCreator::class, 'readDataExcel']);
        Route::post('/process', [ PromoUploadAttachmentCreator::class, 'process']);
    });

    // Upload Attachment Promo Admin
    Route::group(['prefix' => 'tools/upload-attachment-promo-admin'], function() {
        Route::get('/', [ PromoUploadAttachmentAdmin::class, 'uploadPage'])->name('tools-upload-promo-attachment-admin');
        Route::get('/download-template', [ PromoUploadAttachmentAdmin::class, 'downloadTemplate']);
        Route::post('/temp', [ PromoUploadAttachmentAdmin::class, 'temp']);
        Route::post('/temp-delete', [ PromoUploadAttachmentAdmin::class, 'tempDelete']);
        Route::post('/importData', [ PromoUploadAttachmentAdmin::class, 'importData']);
        Route::post('/import', [ PromoUploadAttachmentAdmin::class, 'import']);
        Route::get('/read-excel', [ PromoUploadAttachmentAdmin::class, 'readDataExcel']);
        Route::post('/process', [ PromoUploadAttachmentAdmin::class, 'process']);
    });

    // Blitz Raw Data
    Route::group(['prefix' => 'tools/blitz-rawdata'], function() {
        Route::get('/', [ BlitzRawData::class, 'landingPage'])->name('tools-blitz-rawdata');
        Route::get('/get-data/raw', [ BlitzRawData::class, 'getDataRaw']);
        Route::get('/export-xls/actual-sales', [ BlitzRawData::class, 'exportXlsActualSales']);
        Route::get('/export-xls/baseline', [ BlitzRawData::class, 'exportXlsRawBaseline']);
    });

    // Budget Adjustment
    Route::group(['prefix' => 'tools/budget-adjustment'], function() {
        Route::get('/', [ BudgetAdjustment::class, 'landingPage'])->name('tools-budget-adjustment');
        Route::get('/list/entity', [ BudgetAdjustment::class, 'getListEntity']);
        Route::get('/get-data/budget/entity-id', [ BudgetAdjustment::class, 'getDataBudgetMaster']);
        Route::get('/download-template', [ BudgetAdjustment::class, 'downloadTemplate']);
        Route::post('/upload-xls', [ BudgetAdjustment::class, 'uploadXls']);
    });

    // Transfer to SAP payment
    Route::group(['prefix' => 'tools/sap-accrual'], function() {
        Route::get('/', [ SAPAccrual::class, 'landingPage'])->name('tools-sap-accrual');
        Route::get('/get-list/upload-history', [ SAPAccrual::class, 'getListUploadHistory']);
        Route::get('/list/report-header', [ SAPAccrual::class, 'getListReportHeader']);
        Route::get('/download-accrual', [ SAPAccrual::class, 'exportXML']);
        Route::get('/download-accrual-nmn', [ SAPAccrual::class, 'exportXls']);
        Route::get('/download-reversal', [ SAPAccrual::class, 'exportXMLReversal']);
        Route::post('/upload-xml', [ SAPAccrual::class, 'uploadXml']);
    });

    // XML Payment Reset
    Route::group(['prefix' => 'tools/xml-payment-reset'], function() {
        Route::get('/', [ XMLPaymentReset::class, 'paymentResetPage' ])->name('tools-xml-payment-reset');
        Route::get('/list', [ XMLPaymentReset::class, 'getList' ]);
        Route::get('/data-detail', [ XMLPaymentReset::class, 'getDataDetail' ]);
        Route::get('/list/entity', [ XMLPaymentReset::class, 'getListEntity' ]);
        Route::get('/list/user-profile', [ XMLPaymentReset::class, 'getListUserProfile' ]);
        Route::get('/list/detail', [ XMLPaymentReset::class, 'getListDetail' ]);
        Route::post('/update', [ XMLPaymentReset::class, 'update' ]);
    });

    Route::get('tools/nav', [ NavApp::class, 'NAVAppPage' ])->name('tools-nav');

    // Update Promo Recon Status
    Route::group(['prefix' => 'tools/update-promo-recon-status'], function() {
        Route::get('/', [ UpdatePromoReconStatus::class, 'uploadPage'])->name('tools-update-promo-recon-status');
        Route::post('/upload', [ UpdatePromoReconStatus::class, 'uploadTemplate' ]);
    });
});

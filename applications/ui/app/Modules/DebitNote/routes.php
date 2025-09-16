<?php

use Illuminate\Support\Facades\Route;
use App\Modules\DebitNote\Controllers\ListingPromoDistributor;
use App\Modules\DebitNote\Controllers\DnCreation;
use App\Modules\DebitNote\Controllers\DnCreationHo;
use App\Modules\DebitNote\Controllers\SendToHo;
use App\Modules\DebitNote\Controllers\SuratJalanToHo;
use App\Modules\DebitNote\Controllers\ValidateByHo;
use App\Modules\DebitNote\Controllers\SendToDanone;
use App\Modules\DebitNote\Controllers\SuratJalanToDanone;
use App\Modules\DebitNote\Controllers\ReceivedByDanone;
use App\Modules\DebitNote\Controllers\DnValidateByFinance;
use App\Modules\DebitNote\Controllers\DnValidateBySales;
use App\Modules\DebitNote\Controllers\InvoiceNotificationByDanone;
use App\Modules\DebitNote\Controllers\InvoiceCreation;
use App\Modules\DebitNote\Controllers\ConfirmDnPaid;
use App\Modules\DebitNote\Controllers\DnMultiPrint;
use App\Modules\DebitNote\Controllers\DnReassignment;
use App\Modules\DebitNote\Controllers\DnAssignment;
use App\Modules\DebitNote\Controllers\ListingDnOverBudget;
use App\Modules\DebitNote\Controllers\DnUpload;
use App\Modules\DebitNote\Controllers\DnUploadAttachment;
use App\Modules\DebitNote\Controllers\DnReassignmentByFinance;
use App\Modules\DebitNote\Controllers\DnSendBack;
use App\Modules\DebitNote\Controllers\DnOverBudget;
use App\Modules\DebitNote\Controllers\PromoMultiPrint;
use App\Modules\DebitNote\Controllers\PromoDisplay;
use App\Modules\DebitNote\Controllers\UploadFaktur;
use App\Modules\DebitNote\Controllers\VATExpired;
use App\Modules\DebitNote\Controllers\DnWorkflow;

Route::group(['middleware' => ['web', 'checkSession']], function () {

    // Listing Promo Distributor
    Route::group(['prefix' => '/dn/rpt-listing-promo-distributor'], function () {
        Route::get('/', [ ListingPromoDistributor::class, 'landingPage'])->name('dn-rpt-listing-promo-distributor');
        Route::get('/list/paginate/filter', [ ListingPromoDistributor::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ ListingPromoDistributor::class, 'getListCategory']);
        Route::get('/list/entity', [ ListingPromoDistributor::class, 'getListEntity']);
        Route::get('/export-xls', [ ListingPromoDistributor::class, 'exportXls']);
    });

    // DN Creation
    Route::group(['prefix' => '/dn/creation'], function () {
        Route::get('/', [ DnCreation::class, 'landingPage'])->name('dn-creation');
        Route::get('/form', [ DnCreation::class, 'formPage'])->name('dn-creation.form');
        Route::get('/dn-display', [ DnCreation::class, 'dnDisplay'])->name('dn-creation.dn-display');
        Route::get('/dn-upload-attach', [ DnCreation::class, 'uploadAttachPage'])->name('dn-creation.upload-attach');
        Route::get('/dn-cancel', [ DnCreation::class, 'cancelPage'])->name('dn-creation.cancel');
        Route::get('/list/paginate/filter', [ DnCreation::class, 'getListPaginateFilter']);
        Route::get('/list/subaccount', [ DnCreation::class, 'getListSubAccount']);
        Route::get('/list/entity', [ DnCreation::class, 'getListEntity']);
        Route::get('/list/channel', [ DnCreation::class, 'getListChannel']);
        Route::get('/list/tax-level', [ DnCreation::class, 'getListTaxLevel']);
        Route::get('/list/sellingpoint', [ DnCreation::class, 'getListSellingPoint']);
        Route::get('/list/wht-type', [ DnCreation::class, 'getListWHTType']);
        Route::get('/get-data/tax-level/entity-id', [ DnCreation::class, 'getDataTaxLevelByEntityId']);
        Route::get('/get-data/promo', [ DnCreation::class, 'getDataPromo']);
        Route::get('/get-data/id', [ DnCreation::class, 'getData']);
        Route::get('/get-data/wht-type/promo-id', [ DnCreation::class, 'getDataWHTTypeByPromoId']);
        Route::get('/print-pdf', [ DnCreation::class, 'printPdf']);
        Route::get('/download-zip', [ DnCreation::class, 'downloadZip']);
        Route::post('/save', [ DnCreation::class, 'save']);
        Route::post('/update', [ DnCreation::class, 'update']);
        Route::post('/cancel', [ DnCreation::class, 'cancel' ]);
        Route::post('/upload', [ DnCreation::class, 'uploadFile']);
        Route::post('/delete-attachment', [ DnCreation::class, 'deleteFile']);
    });

    // DN HO
    Route::group(['prefix' => '/dn/creation-ho'], function () {
        Route::get('/', [ DnCreationHo::class, 'landingPage'])->name('dn-ho-creation');
        Route::get('/form', [ DnCreationHo::class, 'formPage'])->name('dn-ho-creation.form');
        Route::get('/dn-display', [ DnCreationHo::class, 'dnDisplay'])->name('dn-ho-creation.dn-display');
        Route::get('/dn-upload-attach', [ DnCreationHo::class, 'uploadAttachPage'])->name('dn-ho-creation.upload-attach');
        Route::get('/dn-cancel', [ DnCreationHo::class, 'cancelPage'])->name('dn-ho-creation.cancel');
        Route::get('/list/paginate/filter', [ DnCreationHo::class, 'getListPaginateFilter']);
        Route::get('/list/subaccount', [ DnCreationHo::class, 'getListSubAccount']);
        Route::get('/list/entity', [ DnCreationHo::class, 'getListEntity']);
        Route::get('/list/channel', [ DnCreationHo::class, 'getListChannel']);
        Route::get('/list/tax-level', [ DnCreationHo::class, 'getListTaxLevel']);
        Route::get('/list/sellingpoint', [ DnCreationHo::class, 'getListSellingPoint']);
        Route::get('/list/wht-type', [ DnCreationHo::class, 'getListWHTType']);
        Route::get('/get-data/tax-level/entity-id', [ DnCreationHo::class, 'getDataTaxLevelByEntityId']);
        Route::get('/get-data/promo', [ DnCreationHo::class, 'getDataPromo']);
        Route::get('/get-data/id', [ DnCreationHo::class, 'getData']);
        Route::get('/get-data/wht-type/promo-id', [ DnCreationHo::class, 'getDataWHTTypeByPromoId']);
        Route::get('/print-pdf', [ DnCreationHo::class, 'printPdf']);
        Route::get('/export-xls', [ DnCreationHo::class, 'exportXls']);
        Route::get('/download-zip', [ DnCreationHo::class, 'downloadZip']);
        Route::post('/save', [ DnCreationHo::class, 'save']);
        Route::post('/update', [ DnCreationHo::class, 'update']);
        Route::post('/cancel', [ DnCreationHo::class, 'cancel' ]);
        Route::post('/upload', [ DnCreationHo::class, 'uploadFile']);
        Route::post('/delete-attachment', [ DnCreationHo::class, 'deleteFile']);
    });

    // Send To Ho
    Route::group(['prefix' => '/dn/send-to-ho'], function () {
        Route::get('/', [ SendToHo::class, 'landingPage'])->name('dn-send-to-ho');
        Route::get('/list', [ SendToHo::class, 'getList']);
        Route::post('/update', [ SendToHo::class, 'update' ]);
        Route::post('/submit-sj', [ SendToHo::class, 'submitSJ' ]);
    });

    // Surat Jalan dan Tanda Terima HO
    Route::group(['prefix' => '/dn/surat-jalan-ho'], function () {
        Route::get('/', [ SuratJalanToHo::class, 'landingPage'])->name('dn-ho-surat-jalan');
        Route::get('/list', [ SuratJalanToHo::class, 'getList']);
        Route::get('/print-pdf', [ SuratJalanToHo::class, 'printPdf']);
    });

    // DN Received & Approved
    Route::group(['prefix' => '/dn/validate-by-ho'], function () {
        Route::get('/', [ ValidateByHo::class, 'landingPage'])->name('dn-validate-by-ho');
        Route::get('/list', [ ValidateByHo::class, 'getList']);
        Route::get('/approval', [ ValidateByHo::class, 'approvalPage'])->name('dn-validate-by-ho.approval');
        Route::get('/get-data/id', [ ValidateByHo::class, 'getData']);
        Route::post('/save', [ ValidateByHo::class, 'save']);
        Route::post('/approved', [ ValidateByHo::class, 'approved']);
        Route::post('/upload-xls', [ ValidateByHo::class, 'uploadXls']);
    });

    // Send To Danone
    Route::group(['prefix' => '/dn/send-to-danone'], function () {
        Route::get('/', [ SendToDanone::class, 'landingPage'])->name('dn-send-to-danone');
        Route::get('/form', [ SendToDanone::class, 'rejectPage'])->name('dn-send-to-danone.reject');
        Route::get('/list', [ SendToDanone::class, 'getList']);
        Route::get('/list/tax-level', [ SendToDanone::class, 'getListTaxLevel']);
        Route::get('/get-data/id', [ SendToDanone::class, 'getData']);
        Route::post('/update', [ SendToDanone::class, 'update' ]);
        Route::post('/reject', [ SendToDanone::class, 'reject' ]);
        Route::post('/submit-sj', [ SendToDanone::class, 'submitSJ' ]);
    });

    // Surat Jalan dan Tanda Terima Danone
    Route::group(['prefix' => '/dn/surat-jalan-danone'], function () {
        Route::get('/', [ SuratJalanToDanone::class, 'landingPage'])->name('dn-danone-surat-jalan');
        Route::get('/list', [ SuratJalanToDanone::class, 'getList']);
        Route::get('/print-pdf', [ SuratJalanToDanone::class, 'printPdf']);
    });

    // DN Received By Danone
    Route::group(['prefix' => '/dn/received-by-danone'], function () {
        Route::get('/', [ ReceivedByDanone::class, 'landingPage'])->name('dn-received-by-danone');
        Route::get('/form', [ ReceivedByDanone::class, 'rejectPage'])->name('dn-received-by-danone.reject');
        Route::get('/list', [ ReceivedByDanone::class, 'getList']);
        Route::get('/list/tax-level', [ ReceivedByDanone::class, 'getListTaxLevel']);
        Route::get('/list/entity', [ ReceivedByDanone::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ ReceivedByDanone::class, 'getListDistributorByEntityId']);
        Route::get('/get-data/id', [ ReceivedByDanone::class, 'getData']);
        Route::post('/update', [ ReceivedByDanone::class, 'update' ]);
        Route::post('/reject', [ ReceivedByDanone::class, 'reject' ]);
    });

    // DN Validation By Finance
    Route::group(['prefix' => '/dn/validate-by-finance'], function () {
        Route::get('/', [ DnValidateByFinance::class, 'landingPage'])->name('dn-validate-by-finance');
        Route::get('/form', [ DnValidateByFinance::class, 'approvalPage'])->name('dn-validate-by-finance.form');
        Route::get('/list', [ DnValidateByFinance::class, 'getList']);
        Route::get('/list/entity', [ DnValidateByFinance::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ DnValidateByFinance::class, 'getListDistributorByEntityId']);
        Route::get('/list/wht-type', [ DnValidateByFinance::class, 'getListWhtType']);
        Route::get('/get-data/wht-type/promo-id', [ DnValidateByFinance::class, 'getDataWHTTypeByPromoId']);
        Route::get('/get-data/id', [ DnValidateByFinance::class, 'getData']);
        Route::get('/get-data/promo/id', [ DnValidateByFinance::class, 'getDataPromo']);
        Route::get('/view', [ DnValidateByFinance::class, 'viewFile']);
        Route::get('/download-zip', [ DnValidateByFinance::class, 'downloadZip']);
        Route::post('/submit', [ DnValidateByFinance::class, 'submit' ]);
        Route::post('/save', [ DnValidateByFinance::class, 'save' ]);
        Route::post('/vat-expired/update', [ DnValidateByFinance::class, 'vatExpiredUpdate' ]);
        Route::post('/upload-xls', [ DnValidateByFinance::class, 'uploadXls']);
    });

    // DN Validation By Sales
    Route::group(['prefix' => '/dn/validate-by-sales'], function () {
        Route::get('/', [ DnValidateBySales::class, 'landingPage'])->name('dn-validate-by-sales');
        Route::get('/form', [ DnValidateBySales::class, 'approvalPage'])->name('dn-validate-by-sales.form');
        Route::get('/list', [ DnValidateBySales::class, 'getList']);
        Route::get('/list/entity', [ DnValidateBySales::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ DnValidateBySales::class, 'getListDistributorByEntityId']);
        Route::get('/get-data/id', [ DnValidateBySales::class, 'getData']);
        Route::get('/get-data/promo/id', [ DnValidateBySales::class, 'getDataPromo']);
        Route::get('/download-zip', [ DnValidateBySales::class, 'downloadZip']);
        Route::get('/preview-attachment', [ DnValidateBySales::class, 'previewAttachment' ]);
        Route::post('/delete-attachment', [ DnValidateBySales::class, 'deleteFile']);
        Route::post('/submit', [ DnValidateBySales::class, 'submit' ]);
        Route::post('/upload-xls', [ DnValidateBySales::class, 'uploadXls']);
    });

    // Invoice Notification By Danone
    Route::group(['prefix' => '/dn/invoice-notif'], function () {
        Route::get('/', [ InvoiceNotificationByDanone::class, 'landingPage'])->name('dn-notif-invoice');
        Route::get('/form', [ InvoiceNotificationByDanone::class, 'rejectPage'])->name('dn-notif-invoice.reject');
        Route::get('/list', [ InvoiceNotificationByDanone::class, 'getList']);
        Route::get('/list/tax-level', [ InvoiceNotificationByDanone::class, 'getListTaxLevel']);
        Route::get('/list/entity', [ InvoiceNotificationByDanone::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ InvoiceNotificationByDanone::class, 'getListDistributorByEntityId']);
        Route::get('/get-data/id', [ InvoiceNotificationByDanone::class, 'getData']);
        Route::post('/update', [ InvoiceNotificationByDanone::class, 'update' ]);
        Route::post('/reject', [ InvoiceNotificationByDanone::class, 'reject' ]);
    });

    // Invoice Creation
    Route::group(['prefix' => '/dn/invoice'], function () {
        Route::get('/', [ InvoiceCreation::class, 'landingPage'])->name('dn-invoice');
        Route::get('/list', [ InvoiceCreation::class, 'getList']);
        Route::get('/form', [ InvoiceCreation::class, 'formPage'])->name('dn-invoice.form');
        Route::get('/reject', [ InvoiceCreation::class, 'rejectPage'])->name('dn-invoice.reject');
        Route::get('/list/entity', [ InvoiceCreation::class, 'getListEntity']);
        Route::get('/list/category', [ InvoiceCreation::class, 'getListCategory']);
        Route::get('/list/tax-level', [ InvoiceCreation::class, 'getListTaxLevel']);
        Route::get('/get-data/distributor', [ InvoiceCreation::class, 'getDataDistributorCompanyName']);
        Route::get('/get-data/id', [ InvoiceCreation::class, 'getData']);
        Route::get('/get-data/dn', [ InvoiceCreation::class, 'getDataDnByTaxlevel']);
        Route::get('/print-pdf', [ InvoiceCreation::class, 'printPdf']);
        Route::post('/save', [ InvoiceCreation::class, 'save' ]);
        Route::post('/reject', [ InvoiceCreation::class, 'reject' ]);
        Route::post('/update', [ InvoiceCreation::class, 'update' ]);
        Route::post('/upload-xls', [ InvoiceCreation::class, 'uploadXls']);
    });

    // Confirm DN Paid
    Route::group(['prefix' => '/dn/paid'], function () {
        Route::get('/', [ ConfirmDnPaid::class, 'landingPage'])->name('dn-paid');
        Route::get('/list', [ ConfirmDnPaid::class, 'getList']);
        Route::post('/submit', [ ConfirmDnPaid::class, 'submit' ]);
    });

    // DN Multi Print
    Route::group(['prefix' => '/dn/multi-print'], function () {
        Route::get('/', [ DnMultiPrint::class, 'landingPage'])->name('dn-multi-print');
        Route::get('/list/paginate/filter', [ DnMultiPrint::class, 'getListPaginateFilter']);
        Route::get('/list/sub-account', [ DnMultiPrint::class, 'getListSubAccount']);
        Route::get('/print-pdf', [ DnMultiPrint::class, 'printPdf']);
    });

    // DN Reassignment
    Route::group(['prefix' => '/dn/reassignment'], function () {
        Route::get('/', [ DnReassignment::class, 'landingPage'])->name('dn-reassignment');
        Route::get('/list', [ DnReassignment::class, 'getList']);
        Route::get('/form', [ DnReassignment::class, 'formPage'])->name('dn-reassignment.form');
        Route::get('/get-data/id', [ DnReassignment::class, 'getData']);
        Route::get('/get-data/promo', [ DnReassignment::class, 'getDataPromo']);
        Route::get('/download-zip', [ DnReassignment::class, 'downloadZip']);
        Route::post('/save', [ DnReassignment::class, 'save' ]);
    });

    // DN Assignment Promo
    Route::group(['prefix' => '/dn/assignment'], function () {
        Route::get('/', [ DnAssignment::class, 'landingPage'])->name('dn-assignment');
        Route::get('/list', [ DnAssignment::class, 'getList']);
        Route::get('/form', [ DnAssignment::class, 'formPage'])->name('dn-assignment.form');
        Route::get('/get-data/id', [ DnAssignment::class, 'getData']);
        Route::get('/get-data/promo', [ DnAssignment::class, 'getDataPromo']);
        Route::post('/save', [ DnAssignment::class, 'save' ]);
        Route::post('/forward', [ DnAssignment::class, 'forward' ]);
    });

    // Listing DN Over Budget [Reassignment]
    Route::group(['prefix' => '/dn/listing-over-budget'], function () {
        Route::get('/', [ ListingDnOverBudget::class, 'landingPage'])->name('dn-listing-over-budget');
        Route::get('/list', [ ListingDnOverBudget::class, 'getList']);
        Route::get('/form', [ ListingDnOverBudget::class, 'formPage'])->name('dn-listing-over-budget.form');
        Route::get('/get-data/id', [ ListingDnOverBudget::class, 'getData']);
        Route::get('/get-data/promo', [ ListingDnOverBudget::class, 'getDataPromo']);
        Route::post('/save', [ ListingDnOverBudget::class, 'save' ]);
        Route::post('/forward', [ ListingDnOverBudget::class, 'forward' ]);


        // DN Over Budget To Be Settled (Dashboard)
        Route::get('/to-be-settled', [ ListingDnOverBudget::class, 'landingPageToBeSettled' ]);
        Route::get('/list/to-be-settled', [ ListingDnOverBudget::class, 'getListToBeSettled']);
        Route::get('/list/to-be-settled', [ ListingDnOverBudget::class, 'getListToBeSettled']);
        Route::get('/list/to-be-settled/promo-id', [ ListingDnOverBudget::class, 'getDataToBeSettledByPromoId']);
    });

    // DN Upload
    Route::group(['prefix' => '/dn/upload'], function () {
        Route::get('/', [ DnUpload::class, 'landingPage'])->name('dn-upload');
        Route::get('/form', [ DnUpload::class, 'formPage'])->name('dn-upload.form');
        Route::get('/upload-form', [ DnUpload::class, 'uploadPage'])->name('dn-upload.upload-form');
        Route::get('/dn-cancel', [ DnUpload::class, 'cancelPage'])->name('dn-upload.cancel');
        Route::get('/dn-upload-attach', [ DnUpload::class, 'uploadAttachPage'])->name('dn-upload.attach');
        Route::get('/dn-display', [ DnUpload::class, 'dnDisplay'])->name('dn-upload.dn-display');
        Route::get('/list/paginate/filter', [ DnUpload::class, 'getListPaginateFilter']);
        Route::get('/list/subaccount', [ DnUpload::class, 'getListSubAccount']);
        Route::get('/list/entity', [ DnUpload::class, 'getListEntity']);
        Route::get('/list/channel', [ DnUpload::class, 'getListChannel']);
        Route::get('/list/tax-level', [ DnUpload::class, 'getListTaxLevel']);
        Route::get('/list/sellingpoint', [ DnUpload::class, 'getListSellingPoint']);
        Route::get('/list/wht-type', [ DnUpload::class, 'getListWHTType']);
        Route::get('/get-data/tax-level/entity-id', [ DnUpload::class, 'getDataTaxLevelByEntityId']);
        Route::get('/get-data/promo', [ DnUpload::class, 'getDataPromo']);
        Route::get('/get-data/id', [ DnUpload::class, 'getData']);
        Route::get('/get-data/wht-type/promo-id', [ DnUpload::class, 'getDataWHTTypeByPromoId']);
        Route::get('/get-data/distributor', [ DnUpload::class, 'getDataDistributorId']);
        Route::get('/print-pdf', [ DnUpload::class, 'printPdf']);
        Route::get('/download-zip', [ DnUpload::class, 'downloadZip']);
        Route::post('/update', [ DnUpload::class, 'update']);
        Route::post('/cancel', [ DnUpload::class, 'cancel' ]);
        Route::post('/upload', [ DnUpload::class, 'uploadFile']);
        Route::post('/upload-xls', [ DnUpload::class, 'uploadXls']);
        Route::post('/delete-attachment', [ DnUpload::class, 'deleteFile']);
    });

    // DN Upload Attachment
    Route::group(['prefix' => '/dn/upload-attachment'], function() {
        Route::get('/', [ DnUploadAttachment::class, 'uploadPage'])->name('dn-attachment-upload');
        Route::get('/download-template', [ DnUploadAttachment::class, 'downloadTemplate']);
        Route::post('/temp', [ DnUploadAttachment::class, 'temp']);
        Route::post('/temp-delete', [ DnUploadAttachment::class, 'tempDelete']);
        Route::post('/importData', [ DnUploadAttachment::class, 'importData']);
    });

    // DN Reassignment By Finance
    Route::group(['prefix' => '/dn/reassignment-by-finance'], function () {
        Route::get('/', [ DnReassignmentByFinance::class, 'landingPage'])->name('dn-finance-by-reassignment');
        Route::get('/list', [ DnReassignmentByFinance::class, 'getList']);
        Route::get('/form', [ DnReassignmentByFinance::class, 'formPage'])->name('dn-finance-by-reassignment.form');
        Route::get('/get-data/id', [ DnReassignmentByFinance::class, 'getData']);
        Route::get('/get-data/promo', [ DnReassignmentByFinance::class, 'getDataPromo']);
        Route::get('/download-zip', [ DnReassignmentByFinance::class, 'downloadZip']);
        Route::post('/save', [ DnReassignmentByFinance::class, 'save' ]);
    });

    // DN Workflow
    Route::group(['prefix' => '/dn/workflow'], function () {
        Route::get('/', [ DnWorkflow::class, 'landingPage' ])->name('dn-workflow');
        Route::get('/data', [ DnWorkflow::class, 'getDataDN' ]);
        Route::get('/workflow-history', [ DnWorkflow::class, 'getListWorkflowHistory' ]);
        Route::get('/change-history', [ DnWorkflow::class, 'getListChangeHistory' ]);
        Route::get('/print-pdf', [ DnWorkflow::class, 'printPdf' ]);
    });

    // DN SendBack
    Route::group(['prefix' => '/dn/send-back'], function () {
        Route::get('/', [ DnSendBack::class, 'landingPage'])->name('dn-send-back');
        Route::get('/dn-display', [ DnSendBack::class, 'dnDisplay'])->name('dn-send-back.dn-display');
        Route::get('/dn-upload-attach', [ DnSendBack::class, 'uploadAttachPage'])->name('dn-send-back.upload-attach');
        Route::get('/list', [ DnSendBack::class, 'getList']);
        Route::get('/list/subaccount', [ DnSendBack::class, 'getListSubAccount']);
        Route::get('/list/entity', [ DnSendBack::class, 'getListEntity']);
        Route::get('/list/channel', [ DnSendBack::class, 'getListChannel']);
        Route::get('/list/tax-level', [ DnSendBack::class, 'getListTaxLevel']);
        Route::get('/list/sellingpoint', [ DnSendBack::class, 'getListSellingPoint']);
        Route::get('/get-data/tax-level/entity-id', [ DnSendBack::class, 'getDataTaxLevelByEntityId']);
        Route::get('/get-data/id', [ DnSendBack::class, 'getData']);
        Route::get('/print-pdf', [ DnSendBack::class, 'printPdf']);
        Route::get('/download-zip', [ DnSendBack::class, 'downloadZip']);
        Route::post('/update', [ DnSendBack::class, 'update']);
        Route::post('/upload', [ DnSendBack::class, 'uploadFile']);
        Route::post('/delete-attachment', [ DnSendBack::class, 'deleteFile']);
    });

    // DN Over Budget
    Route::group(['prefix'  => '/dn/over-budget'], function() {
        Route::get('/', [ DnOverBudget::class, 'landingPage' ])->name('dn-over-budget');
        Route::get('/list', [ DnOverBudget::class, 'getList' ]);
        Route::post('/refresh', [ DnOverBudget::class, 'refresh' ]);
    });

    // Promo Multi Print
    Route::group(['prefix' => '/dn/multi-print-promo'], function () {
        Route::get('/', [ PromoMultiPrint::class, 'landingPage'])->name('dn-print-multi-promo');
        Route::get('/list', [ PromoMultiPrint::class, 'getList']);
        Route::get('/list/entity', [ PromoMultiPrint::class, 'getListEntity']);
        Route::get('/print-pdf', [ PromoMultiPrint::class, 'printPdf']);
    });

    // Promo Display
    Route::group(['prefix' => '/dn/promo-display'], function() {
        Route::get('/', [PromoDisplay::class, 'landingPage'])->name('dn-promo-display');
        Route::get('/form', [PromoDisplay::class, 'form'])->name('dn-promo-display.form');
        Route::get('/list/paginate/filter', [PromoDisplay::class, 'getListPaginateFilter']);
        Route::get('/get-data/id', [PromoDisplay::class, 'getDataById']);
        Route::get('/list/entity', [PromoDisplay::class, 'getListEntity']);
        Route::get('/export-pdf', [ PromoDisplay::class, 'exportPdf']);
        Route::get('/view-file', [ PromoDisplay::class, 'viewFile']);
    });

    // DN Upload Faktur
    Route::group(['prefix' => '/dn/upload-faktur'], function () {
        Route::get('/', [ UploadFaktur::class, 'landingPage' ])->name('dn-faktur-upload');
        Route::post('/upload', [ UploadFaktur::class, 'upload' ]);
    });

    // VAT Expired
    Route::group(['prefix' => '/dn/vat-expired'], function () {
        Route::get('/', [ VATExpired::class, 'landingPage' ])->name('dn-vat-expired');
        Route::get('/list', [ VATExpired::class, 'getListFilter' ]);
        Route::get('/list/entity', [ VATExpired::class, 'getListEntity' ]);
        Route::get('/list/distributor', [ VATExpired::class, 'getListDistributorByEntityId' ]);
        Route::post('/update', [ VATExpired::class, 'update' ]);
    });
});

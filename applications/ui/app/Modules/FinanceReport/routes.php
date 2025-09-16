<?php

use App\Modules\FinanceReport\Controllers\Accrual;
use App\Modules\FinanceReport\Controllers\DNDetailReporting;
use App\Modules\FinanceReport\Controllers\DNDisplay;
use App\Modules\FinanceReport\Controllers\DocumentCompleteness;
use App\Modules\FinanceReport\Controllers\Investment;
use App\Modules\FinanceReport\Controllers\ListingDN;
use App\Modules\FinanceReport\Controllers\ListingPromoReconciliation;
use App\Modules\FinanceReport\Controllers\ListingPromoReporting;
use App\Modules\FinanceReport\Controllers\PromoHistoricalMovement;
use App\Modules\FinanceReport\Controllers\MatrixApprovalListing;
use App\Modules\FinanceReport\Controllers\PromoApprovalAging;
use App\Modules\FinanceReport\Controllers\PromoDisplay;
use App\Modules\FinanceReport\Controllers\PromoPlanningRepoting;
use App\Modules\FinanceReport\Controllers\SKPValidation;
use App\Modules\FinanceReport\Controllers\SubmissionReport;
use App\Modules\FinanceReport\Controllers\SummaryAgingApproval;
use App\Modules\FinanceReport\Controllers\ListingPromoReportingByMechanism;
use App\Modules\FinanceReport\Controllers\ListingPromoReportingPostRecon;
use App\Modules\FinanceReport\Controllers\TTControl;
use App\Modules\FinanceReport\Controllers\SummaryBudgeting;
use App\Modules\FinanceReport\Controllers\DnReadyToPay;

use Illuminate\Support\Facades\Route;

Route::group(['middleware' => ['web', 'checkSession']], function () {
    // Accrual Report
    Route::group(['prefix' => '/fin-rpt/accrual'], function () {
        Route::get('/', [ Accrual::class, 'landingPage'])->name('fin-rpt-accrual');
        Route::get('/list/paginate/filter', [ Accrual::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ Accrual::class, 'getListEntity']);
        Route::get('/list/report-header', [ Accrual::class, 'getListReportHeader']);
        Route::get('/download/report-header/by-id', [ Accrual::class, 'downloadReportHeaderByID']);
        Route::get('/export-xls', [ Accrual::class, 'exportXls']);
        Route::get('/get-data/gap', [ Accrual::class, 'getDataGap']);
        Route::get('/export-xls/gap', [ Accrual::class, 'exportXlsGap']);

    });

    // DN Detail Reporting
    Route::group(['prefix' => '/fin-rpt/dn-detail-reporting'], function () {
        Route::get('/', [ DNDetailReporting::class, 'landingPage'])->name('fin-rpt-dn-detail-reporting');
        Route::get('/list/paginate/filter', [ DNDetailReporting::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ DNDetailReporting::class, 'getListCategory']);
        Route::get('/list/entity', [ DNDetailReporting::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ DNDetailReporting::class, 'getListDistributorByEntityId']);
        Route::get('/list/sub-account', [ DNDetailReporting::class, 'getListSubAccount']);
        Route::get('/export-csv', [ DNDetailReporting::class, 'exportCsv']);
        Route::get('/get-data/gap', [ DNDetailReporting::class, 'getDataGap']);
        Route::get('/export-xls/gap', [ DNDetailReporting::class, 'exportXlsGap']);
    });

    // DN Display
    Route::group(['prefix' => '/fin-rpt/dn-display'], function () {
        Route::get('/', [ DNDisplay::class, 'landingPage'])->name('fin-rpt-dn-display');
        Route::get('/form', [ DNDisplay::class, 'formPage'])->name('fin-rpt-dn-display.form');
        Route::get('/list/paginate/filter', [ DNDisplay::class, 'getListPaginateFilter']);
        Route::get('/get-data/id', [ DNDisplay::class, 'getDataDNById']);
        Route::get('/list/entity', [ DNDisplay::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ DNDisplay::class, 'getListDistributorByEntityId']);
        Route::get('/list/selling-point', [ DNDisplay::class, 'getListSellingPoint']);
        Route::get('/list/tax-level', [ DNDisplay::class, 'getListTaxLevel']);
        Route::get('/download-zip', [ DNDisplay::class, 'downloadZip']);
        Route::get('/print-pdf', [ DNDisplay::class, 'printPdf']);
    });

    // Document Completeness
    Route::group(['prefix' => '/fin-rpt/doc-completeness'], function () {
        Route::get('/', [ DocumentCompleteness::class, 'landingPage'])->name('fin-rpt-doc-completeness');
        Route::get('/list/paginate/filter', [ DocumentCompleteness::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ DocumentCompleteness::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ DocumentCompleteness::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ DocumentCompleteness::class, 'exportXls']);
    });

    // Investment
    Route::group(['prefix' => '/fin-rpt/investment'], function () {
        Route::get('/', [ Investment::class, 'landingPage'])->name('fin-rpt-investment');
        Route::get('/list/paginate/filter', [ Investment::class, 'getListPaginateFilter']);
        Route::get('/list/budget-allocation', [ Investment::class, 'getListBudgetAllocation']);
        Route::get('/list/entity', [ Investment::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ Investment::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ Investment::class, 'exportXls']);
    });

    // Listing DN
    Route::group(['prefix' => '/fin-rpt/listing-dn'], function () {
        Route::get('/', [ ListingDN::class, 'landingPage'])->name('fin-rpt-listing-dn');
        Route::get('/list/paginate/filter', [ ListingDN::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ ListingDN::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ ListingDN::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ ListingDN::class, 'exportXls']);
    });

    // Listing Promo Reconciliation Reporting
    Route::group(['prefix' => '/fin-rpt/listing-promo-recon-reporting'], function () {
        Route::get('/', [ ListingPromoReconciliation::class, 'landingPage'])->name('fin-rpt-listing-promo-recon-reporting');
        Route::get('/list/paginate/filter', [ ListingPromoReconciliation::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ ListingPromoReconciliation::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ ListingPromoReconciliation::class, 'getListDistributorByEntityId']);
        Route::get('/list/channel', [ ListingPromoReconciliation::class, 'getListChannel']);
        Route::get('/get-data/promo-creator/usergroup-id', [ ListingPromoReconciliation::class, 'getDataPromoCreator']);
        Route::get('/export-xls', [ ListingPromoReconciliation::class, 'exportXls']);
    });

    // Listing Promo Reporting
    Route::group(['prefix' => '/fin-rpt/listing-promo-reporting'], function () {
        Route::get('/', [ ListingPromoReporting::class, 'landingPage'])->name('fin-rpt-listing-promo-reporting');
        Route::get('/promo-approval-reminder', [ ListingPromoReporting::class, 'promoApprovalReminderPage'])->name('fin-rpt-listing-promo-reporting.promo-approval-reminder');
        Route::get('/list/paginate/filter', [ ListingPromoReporting::class, 'getListPaginateFilter']);
        Route::get('/list/promo-approval-reminder', [ ListingPromoReporting::class, 'getListPromoApprovalReminder']);
        Route::get('/list/category', [ ListingPromoReporting::class, 'getListCategory']);
        Route::get('/list/entity', [ ListingPromoReporting::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ ListingPromoReporting::class, 'getListDistributorByEntityId']);
        Route::get('/list/channel', [ ListingPromoReporting::class, 'getListChannel']);
        Route::get('/list/usergroup', [ ListingPromoReporting::class, 'getListUserGroup']);
        Route::get('/get-data/user-list', [ ListingPromoReporting::class, 'getDataUserList']);
        Route::get('/get-data/user-regular', [ ListingPromoReporting::class, 'getDatauserRegular']);
        Route::get('/export-xls', [ ListingPromoReporting::class, 'exportXls']);
        Route::get('/export-csv', [ ListingPromoReporting::class, 'exportCsv']);
        Route::get('/get-data/promo-approval-reminder', [ ListingPromoReporting::class, 'getDataPromoApprovalReminderConfig']);
        Route::get('/export-xls/promo-approval-reminder', [ ListingPromoReporting::class, 'exportXlsPromoApprovalReminder']);
        Route::get('/get-data/gap', [ ListingPromoReporting::class, 'getDataGap']);
        Route::get('/export-xls/gap', [ ListingPromoReporting::class, 'exportXlsGap']);
        Route::post('/send-email', [ ListingPromoReporting::class, 'sendEmail']);
        //auto config
        Route::post('/configuration', [ ListingPromoReporting::class, 'configuration']);
    });

    // Promo Historical Movement
    Route::group(['prefix' => '/fin-rpt/promo-historical-movement'], function () {
        Route::get('/', [ PromoHistoricalMovement::class, 'landingPage'])->name('fin-rpt-promo-historical-movement');
        Route::get('/list/paginate/filter', [ PromoHistoricalMovement::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ PromoHistoricalMovement::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ PromoHistoricalMovement::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ PromoHistoricalMovement::class, 'exportXls']);
    });

    // Matrix Approval Listing
    Route::group(['prefix' => '/fin-rpt/matrix-approval-listing'], function () {
        Route::get('/', [ MatrixApprovalListing::class, 'landingPage'])->name('fin-rpt-matrix-approval-listing');
        Route::get('/list/paginate/filter', [ MatrixApprovalListing::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ MatrixApprovalListing::class, 'getListCategory']);
        Route::get('/list/entity', [ MatrixApprovalListing::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ MatrixApprovalListing::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ MatrixApprovalListing::class, 'exportXls']);
        Route::get('/export-xls/historical', [ MatrixApprovalListing::class, 'exportXlsHistorical']);
    });

    // Promo Approval Aging
    Route::group(['prefix' => '/fin-rpt/promo-approval-aging'], function () {
        Route::get('/', [ PromoApprovalAging::class, 'landingPage'])->name('fin-rpt-promo-approval-aging');
        Route::get('/list/paginate/filter', [ PromoApprovalAging::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ PromoApprovalAging::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ PromoApprovalAging::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ PromoApprovalAging::class, 'exportXls']);
    });

    // Promo Display
    Route::group(['prefix' => '/fin-rpt/promo-display'], function () {
        Route::get('/', [ PromoDisplay::class, 'landingPage'])->name('fin-rpt-promo-display');
        Route::get('/form', [ PromoDisplay::class, 'formPage'])->name('fin-rpt-promo-display.form');
        Route::get('/list/paginate/filter', [ PromoDisplay::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ PromoDisplay::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ PromoDisplay::class, 'getListDistributorByEntityId']);
        Route::get('/data/id', [ PromoDisplay::class, 'getDataByID']);
        Route::get('/export-pdf', [ PromoDisplay::class, 'exportPdf']);
        Route::get('/view-file', [ PromoDisplay::class, 'viewFile']);
        Route::get('/data-revamp/id', [ PromoDisplay::class, 'getDataV2ById']);
        Route::get('/preview-attachment', [ PromoDisplay::class, 'previewAttachment' ]);
    });

    // Promo Planning Reporting
    Route::group(['prefix' => '/fin-rpt/promo-planning-reporting'], function () {
        Route::get('/', [ PromoPlanningRepoting::class, 'landingPage'])->name('fin-rpt-promo-planning-reporting');
        Route::get('/list/paginate/filter', [ PromoPlanningRepoting::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ PromoPlanningRepoting::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ PromoPlanningRepoting::class, 'getListDistributorByEntityId']);
        Route::get('/list/channel', [ PromoPlanningRepoting::class, 'getListChannel']);
        Route::get('/export-xls', [ PromoPlanningRepoting::class, 'exportXls']);
    });

    // SKP Validation Report
    Route::group(['prefix' => '/fin-rpt/skp-validation'], function () {
        Route::get('/', [ SKPValidation::class, 'landingPage'])->name('fin-rpt-skp-validation');
        Route::get('/list/paginate/filter', [ SKPValidation::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ SKPValidation::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ SKPValidation::class, 'getListDistributorByEntityId']);
        Route::get('/list/channel', [ SKPValidation::class, 'getListChannel']);
        Route::get('/export-xls', [ SKPValidation::class, 'exportXls']);
    });

    // Submission Report
    Route::group(['prefix' => '/fin-rpt/submission'], function () {
        Route::get('/', [ SubmissionReport::class, 'landingPage'])->name('fin-rpt-submission');
        Route::get('/list/paginate/filter', [ SubmissionReport::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ SubmissionReport::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ SubmissionReport::class, 'getListDistributorByEntityId']);
        Route::get('/list/channel', [ SubmissionReport::class, 'getListChannel']);
        Route::get('/list/usergroup', [ SubmissionReport::class, 'getListUserGroup']);
        Route::get('/get-data/user-list', [ SubmissionReport::class, 'getDataUserList']);
        Route::get('/get-data/late-promo', [ SubmissionReport::class, 'getDataLatePromo']);
        Route::get('/get-data/exception', [ SubmissionReport::class, 'getDataException']);
        Route::get('/send-email', [ SubmissionReport::class, 'sendEmail']);
        Route::get('/export-xls', [ SubmissionReport::class, 'exportXls']);
        Route::get('/download-template', [ SubmissionReport::class, 'downloadTemplate']);
        Route::post('/upload-xls', [ SubmissionReport::class, 'uploadXls']);
    });

    // Summary Aging Approval
    Route::group(['prefix' => '/fin-rpt/summary-aging-approval'], function () {
        Route::get('/', [ SummaryAgingApproval::class, 'landingPage'])->name('fin-rpt-summary-aging-approval');
        Route::get('/list/paginate/filter', [ SummaryAgingApproval::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ SummaryAgingApproval::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ SummaryAgingApproval::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ SummaryAgingApproval::class, 'exportXls']);
    });

    Route::group(['prefix' => '/fin-rpt/listing-promo-reporting-by-mechanism'], function () {
        Route::get('/', [ ListingPromoReportingByMechanism::class, 'landingPage'])->name('fin-rpt-by-mechanism-listing-promo-reporting');
        Route::get('/list/paginate/filter', [ ListingPromoReportingByMechanism::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ ListingPromoReportingByMechanism::class, 'getListCategory']);
        Route::get('/list/entity', [ ListingPromoReportingByMechanism::class, 'getListEntity']);
        Route::get('/list/distributor', [ ListingPromoReportingByMechanism::class, 'getListDistributor']);
        Route::get('/list/channel', [ ListingPromoReportingByMechanism::class, 'getListChannel']);
        Route::get('/export-xls', [ ListingPromoReportingByMechanism::class, 'exportXls']);
        Route::get('/export-csv', [ ListingPromoReportingByMechanism::class, 'exportCsv']);
    });


    Route::group(['prefix' => '/fin-rpt/listing-promo-reporting-post-recon'], function () {
        Route::get('/', [ ListingPromoReportingPostRecon::class, 'landingPage'])->name('fin-rpt-post-recon-listing-promo-reporting');
        Route::get('/list/paginate/filter', [ ListingPromoReportingPostRecon::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ ListingPromoReportingPostRecon::class, 'getListCategory']);
        Route::get('/list/entity', [ ListingPromoReportingPostRecon::class, 'getListEntity']);
        Route::get('/list/distributor', [ ListingPromoReportingPostRecon::class, 'getListDistributor']);
        Route::get('/list/channel', [ ListingPromoReportingPostRecon::class, 'getListChannel']);
        Route::get('/export-xls', [ ListingPromoReportingPostRecon::class, 'exportXls']);
        Route::get('/export-csv', [ ListingPromoReportingPostRecon::class, 'exportCsv']);
    });

    // TT Control RC
    Route::group(['prefix' => '/fin-rpt/tt-control'], function () {
        Route::get('/', [ TTControl::class, 'landingPage' ])->name('fin-rpt-tt-control');
        Route::get('/list/paginate/filter', [ TTControl::class, 'getListPaginateFilter']);
        Route::get('/list/filter', [ TTControl::class, 'getListFilter']);
        Route::get('/export-xls', [ TTControl::class, 'exportXls']);
    });

    // Summary Budgeting
    Route::group(['prefix' => '/fin-rpt/summary-budget'], function () {
        Route::get('/', [ SummaryBudgeting::class, 'landingPage' ])->name('fin-rpt-summary-budget');
        Route::get('/list/paginate/filter', [ SummaryBudgeting::class, 'getListPaginateFilter']);
        Route::get('/list/filter', [ SummaryBudgeting::class, 'getListFilter']);
        Route::get('/export-xls', [ SummaryBudgeting::class, 'exportXls']);
    });

    // DN Ready to Pay
    Route::group(['prefix' => '/fin-rpt/dn-ready-to-pay'], function () {
        Route::get('/', [ DnReadyToPay::class, 'landingPage'])->name('fin-rpt-dn-ready-to-pay');
        Route::get('/list/paginate/filter', [ DnReadyToPay::class, 'getListPaginateFilter']);
        Route::get('/list/filter', [ DnReadyToPay::class, 'getListFilter']);
        Route::get('/export-xls', [ DnReadyToPay::class, 'exportXls']);
        Route::get('/export-csv', [ DnReadyToPay::class, 'exportCsv']);
    });
    Route::group(['prefix' => '/fin-rpt/listing-promo-reporting-by-mechanism'], function () {
        Route::get('/', [ ListingPromoReportingByMechanism::class, 'landingPage'])->name('fin-rpt-by-mechanism-listing-promo-reporting');
        Route::get('/list/paginate/filter', [ ListingPromoReportingByMechanism::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ ListingPromoReportingByMechanism::class, 'getListCategory']);
        Route::get('/list/entity', [ ListingPromoReportingByMechanism::class, 'getListEntity']);
        Route::get('/list/distributor', [ ListingPromoReportingByMechanism::class, 'getListDistributor']);
        Route::get('/list/channel', [ ListingPromoReportingByMechanism::class, 'getListChannel']);
        Route::get('/export-xls', [ ListingPromoReportingByMechanism::class, 'exportXls']);
        Route::get('/export-csv', [ ListingPromoReportingByMechanism::class, 'exportCsv']);
    });


    Route::group(['prefix' => '/fin-rpt/listing-promo-reporting-post-recon'], function () {
        Route::get('/', [ ListingPromoReportingPostRecon::class, 'landingPage'])->name('fin-rpt-post-recon-listing-promo-reporting');
        Route::get('/list/paginate/filter', [ ListingPromoReportingPostRecon::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ ListingPromoReportingPostRecon::class, 'getListCategory']);
        Route::get('/list/entity', [ ListingPromoReportingPostRecon::class, 'getListEntity']);
        Route::get('/list/distributor', [ ListingPromoReportingPostRecon::class, 'getListDistributor']);
        Route::get('/list/channel', [ ListingPromoReportingPostRecon::class, 'getListChannel']);
        Route::get('/export-xls', [ ListingPromoReportingPostRecon::class, 'exportXls']);
        Route::get('/export-csv', [ ListingPromoReportingPostRecon::class, 'exportCsv']);
    });

    // TT Control RC
    Route::group(['prefix' => '/fin-rpt/tt-control'], function () {
        Route::get('/', [ TTControl::class, 'landingPage' ])->name('fin-rpt-tt-control');
        Route::get('/list/paginate/filter', [ TTControl::class, 'getListPaginateFilter']);
        Route::get('/list/filter', [ TTControl::class, 'getListFilter']);
        Route::get('/export-xls', [ TTControl::class, 'exportXls']);
    });

    // Summary Budgeting
    Route::group(['prefix' => '/fin-rpt/summary-budget'], function () {
        Route::get('/', [ SummaryBudgeting::class, 'landingPage' ])->name('fin-rpt-summary-budget');
        Route::get('/list/paginate/filter', [ SummaryBudgeting::class, 'getListPaginateFilter']);
        Route::get('/list/filter', [ SummaryBudgeting::class, 'getListFilter']);
        Route::get('/export-xls', [ SummaryBudgeting::class, 'exportXls']);
    });

    // DN Ready to Pay
    Route::group(['prefix' => '/fin-rpt/dn-ready-to-pay'], function () {
        Route::get('/', [ DnReadyToPay::class, 'landingPage'])->name('fin-rpt-dn-ready-to-pay');
        Route::get('/list/paginate/filter', [ DnReadyToPay::class, 'getListPaginateFilter']);
        Route::get('/list/filter', [ DnReadyToPay::class, 'getListFilter']);
        Route::get('/export-xls', [ DnReadyToPay::class, 'exportXls']);
        Route::get('/export-csv', [ DnReadyToPay::class, 'exportCsv']);
    });
});

// Download Attachment Submission Report From Email
Route::get('/fin-rpt/submission/export-xls-attachment/{year}/{entity}/{distributor}/{channel}', [ SubmissionReport::class, 'exportXlsAttachment']);


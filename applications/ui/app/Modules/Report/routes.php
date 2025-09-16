<?php

use Illuminate\Support\Facades\Route;
use App\Modules\Report\Controllers\Investment;
use App\Modules\Report\Controllers\MatrixApprovalListing;
use App\Modules\Report\Controllers\PromoHistoricalMovement;
use App\Modules\Report\Controllers\SummaryAgingApproval;
use App\Modules\Report\Controllers\Accrual;
use App\Modules\Report\Controllers\ListingDN;
use App\Modules\Report\Controllers\DNDetailReporting;
use App\Modules\Report\Controllers\DNDisplay;
use App\Modules\Report\Controllers\ListingPromoReconciliation;
use App\Modules\Report\Controllers\PromoPlanningRepoting;
use App\Modules\Report\Controllers\ListingPromoReporting;
use App\Modules\Report\Controllers\SKPValidation;
use App\Modules\Report\Controllers\DocumentCompleteness;
use App\Modules\Report\Controllers\ListingPromoReportingByMechanism;
use App\Modules\Report\Controllers\ListingPromoReportingPostRecon;
use App\Modules\Report\Controllers\ChannelSummary;

Route::group(['middleware' => ['web', 'checkSession']], function () {

    // Investment
    Route::group(['prefix' => '/rpt/investment'], function () {
        Route::get('/', [ Investment::class, 'landingPage'])->name('rpt-investment');
        Route::get('/list/paginate/filter', [ Investment::class, 'getListPaginateFilter']);
        Route::get('/list/budget-allocation', [ Investment::class, 'getListBudgetAllocation']);
        Route::get('/list/entity', [ Investment::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ Investment::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ Investment::class, 'exportXls']);
    });

    // Matrix Approval Listing
    Route::group(['prefix' => '/rpt/matrix-approval-listing'], function () {
        Route::get('/', [ MatrixApprovalListing::class, 'landingPage'])->name('rpt-matrix-approval-listing');
        Route::get('/list/paginate/filter', [ MatrixApprovalListing::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ MatrixApprovalListing::class, 'getListCategory']);
        Route::get('/list/entity', [ MatrixApprovalListing::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ MatrixApprovalListing::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ MatrixApprovalListing::class, 'exportXls']);
        Route::get('/export-xls/historical', [ MatrixApprovalListing::class, 'exportXlsHistorical']);
    });

    // Promo Historical Movement
    Route::group(['prefix' => '/rpt/promo-historical-movement'], function () {
        Route::get('/', [ PromoHistoricalMovement::class, 'landingPage'])->name('rpt-promo-historical-movement');
        Route::get('/list/paginate/filter', [ PromoHistoricalMovement::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ PromoHistoricalMovement::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ PromoHistoricalMovement::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ PromoHistoricalMovement::class, 'exportXls']);
    });

    // Summary Aging Approval
    Route::group(['prefix' => '/rpt/summary-aging-approval'], function () {
        Route::get('/', [ SummaryAgingApproval::class, 'landingPage'])->name('rpt-summary-aging-approval');
        Route::get('/list/paginate/filter', [ SummaryAgingApproval::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ SummaryAgingApproval::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ SummaryAgingApproval::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ SummaryAgingApproval::class, 'exportXls']);
    });

    // Accrual Report
    Route::group(['prefix' => '/rpt/accrual'], function () {
        Route::get('/', [ Accrual::class, 'landingPage'])->name('rpt-accrual');
        Route::get('/list/paginate/filter', [ Accrual::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ Accrual::class, 'getListEntity']);
        Route::get('/export-xls', [ Accrual::class, 'exportXls']);
    });

    // Listing DN
    Route::group(['prefix' => '/rpt/listing-dn'], function () {
        Route::get('/', [ ListingDN::class, 'landingPage'])->name('rpt-listing-dn');
        Route::get('/list/paginate/filter', [ ListingDN::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ ListingDN::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ ListingDN::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ ListingDN::class, 'exportXls']);
    });

    // DN Detail Reporting
    Route::group(['prefix' => '/rpt/dn-detail-reporting'], function () {
        Route::get('/', [ DNDetailReporting::class, 'landingPage'])->name('rpt-dn-detail-reporting');
        Route::get('/list/paginate/filter', [ DNDetailReporting::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ DNDetailReporting::class, 'getListCategory']);
        Route::get('/list/entity', [ DNDetailReporting::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ DNDetailReporting::class, 'getListDistributorByEntityId']);
        Route::get('/list/sub-account', [ DNDetailReporting::class, 'getListSubAccount']);
        Route::get('/export-xls', [ DNDetailReporting::class, 'exportXls']);
    });

    // DN Display
    Route::group(['prefix' => '/rpt/dn-display'], function () {
        Route::get('/', [ DNDisplay::class, 'landingPage'])->name('rpt-dn-display');
        Route::get('/form', [ DNDisplay::class, 'formPage'])->name('rpt-dn-display.form');
        Route::get('/list/paginate/filter', [ DNDisplay::class, 'getListPaginateFilter']);
        Route::get('/get-data/id', [ DNDisplay::class, 'getDataDNById']);
        Route::get('/list/entity', [ DNDisplay::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ DNDisplay::class, 'getListDistributorByEntityId']);
        Route::get('/list/selling-point', [ DNDisplay::class, 'getListSellingPoint']);
        Route::get('/list/tax-level', [ DNDisplay::class, 'getListTaxLevel']);
        Route::get('/download-zip', [ DNDisplay::class, 'downloadZip']);
        Route::get('/print-pdf', [ DNDisplay::class, 'printPdf']);
    });

    // Listing Promo Reconciliation Reporting
    Route::group(['prefix' => '/rpt/listing-promo-recon-reporting'], function () {
        Route::get('/', [ ListingPromoReconciliation::class, 'landingPage'])->name('rpt-listing-promo-recon-reporting');
        Route::get('/list/paginate/filter', [ ListingPromoReconciliation::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ ListingPromoReconciliation::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ ListingPromoReconciliation::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ ListingPromoReconciliation::class, 'exportXls']);
    });

    // Promo Planning Reporting
    Route::group(['prefix' => '/rpt/promo-planning-reporting'], function () {
        Route::get('/', [ PromoPlanningRepoting::class, 'landingPage'])->name('rpt-promo-planning-reporting');
        Route::get('/list/paginate/filter', [ PromoPlanningRepoting::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ PromoPlanningRepoting::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ PromoPlanningRepoting::class, 'getListDistributorByEntityId']);
        Route::get('/list/channel', [ PromoPlanningRepoting::class, 'getListChannel']);
        Route::get('/export-xls', [ PromoPlanningRepoting::class, 'exportXls']);
    });

    // Listing Promo Reporting
    Route::group(['prefix' => '/rpt/listing-promo-reporting'], function () {
        Route::get('/', [ ListingPromoReporting::class, 'landingPage'])->name('rpt-listing-promo-reporting');
        Route::get('/list/paginate/filter', [ ListingPromoReporting::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ ListingPromoReporting::class, 'getListCategory']);
        Route::get('/list/entity', [ ListingPromoReporting::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ ListingPromoReporting::class, 'getListDistributorByEntityId']);
        Route::get('/list/channel', [ ListingPromoReporting::class, 'getListChannel']);
        Route::get('/export-xls', [ ListingPromoReporting::class, 'exportXls']);
        Route::get('/export-csv', [ ListingPromoReporting::class, 'exportCsv']);
    });

    // SKP Validation Report
    Route::group(['prefix' => '/rpt/skp-validation'], function () {
        Route::get('/', [ SKPValidation::class, 'landingPage'])->name('rpt-skp-validation');
        Route::get('/list/paginate/filter', [ SKPValidation::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ SKPValidation::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ SKPValidation::class, 'getListDistributorByEntityId']);
        Route::get('/list/channel', [ SKPValidation::class, 'getListChannel']);
        Route::get('/export-xls', [ SKPValidation::class, 'exportXls']);
    });

    // Document Completeness
    Route::group(['prefix' => '/rpt/doc-completeness'], function () {
        Route::get('/', [ DocumentCompleteness::class, 'landingPage'])->name('rpt-doc-completeness');
        Route::get('/list/paginate/filter', [ DocumentCompleteness::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ DocumentCompleteness::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ DocumentCompleteness::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ DocumentCompleteness::class, 'exportXls']);
    });

    // Listing Promo Reporting by Mechanism
    Route::group(['prefix' => '/rpt/listing-promo-reporting-by-mechanism'], function () {
        Route::get('/', [ ListingPromoReportingByMechanism::class, 'landingPage'])->name('rpt-by-mechanism-listing-promo-reporting');
        Route::get('/list/paginate/filter', [ ListingPromoReportingByMechanism::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ ListingPromoReportingByMechanism::class, 'getListCategory']);
        Route::get('/list/entity', [ ListingPromoReportingByMechanism::class, 'getListEntity']);
        Route::get('/list/distributor', [ ListingPromoReportingByMechanism::class, 'getListDistributor']);
        Route::get('/list/channel', [ ListingPromoReportingByMechanism::class, 'getListChannel']);
        Route::get('/export-xls', [ ListingPromoReportingByMechanism::class, 'exportXls']);
        Route::get('/export-csv', [ ListingPromoReportingByMechanism::class, 'exportCsv']);
    });

    Route::group(['prefix' => '/rpt/listing-promo-reporting-post-recon'], function () {
        Route::get('/', [ ListingPromoReportingPostRecon::class, 'landingPage'])->name('rpt-post-recon-listing-promo-reporting');
        Route::get('/list/paginate/filter', [ ListingPromoReportingPostRecon::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ ListingPromoReportingPostRecon::class, 'getListCategory']);
        Route::get('/list/entity', [ ListingPromoReportingPostRecon::class, 'getListEntity']);
        Route::get('/list/distributor', [ ListingPromoReportingPostRecon::class, 'getListDistributor']);
        Route::get('/list/channel', [ ListingPromoReportingPostRecon::class, 'getListChannel']);
        Route::get('/export-xls', [ ListingPromoReportingPostRecon::class, 'exportXls']);
        Route::get('/export-csv', [ ListingPromoReportingPostRecon::class, 'exportCsv']);
    });

    // Channel Summary
    Route::group(['prefix' => '/rpt/channel-summary'], function () {
        Route::get('/', [ ChannelSummary::class, 'landingPage' ])->name('rpt-channel-summary');
        Route::get('/list/paginate/filter', [ ChannelSummary::class, 'getListPaginateFilter']);
        Route::get('/list/filter', [ ChannelSummary::class, 'getListFilter']);
        Route::get('/export-xls', [ ChannelSummary::class, 'exportXls']);
    });
});

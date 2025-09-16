<?php

use Illuminate\Support\Facades\Route;
use App\Modules\Budget\Controllers\BudgetMaster;
use App\Modules\Budget\Controllers\BudgetAllocation;
use App\Modules\Budget\Controllers\BudgetAssignment;
use App\Modules\Budget\Controllers\BudgetApproval;
use App\Modules\Budget\Controllers\BudgetHistory;
use App\Modules\Budget\Controllers\BudgetConversionRate;
use App\Modules\Budget\Controllers\BudgetVolume;
use App\Modules\Budget\Controllers\BudgetPSInput;
use App\Modules\Budget\Controllers\BudgetTTConsole;
use App\Modules\Budget\Controllers\BudgetApprovalRequest;
use App\Modules\Budget\Controllers\BudgetDeployment;

Route::group(['middleware' => ['web', 'checkSession']], function () {

    // Budget Master
    Route::group(['prefix' => '/budget/master'], function() {
        Route::get('/', [ BudgetMaster::class, 'landingPage'])->name('budget-master');
        Route::get('/list/paginate/filter', [ BudgetMaster::class, 'getListPaginateFilter']);
        Route::get('/form', [ BudgetMaster::class, 'budgetMasterFormPage'])->name('budget-master.form');
        Route::get('/get-data/id', [ BudgetMaster::class, 'getDataByID']);
        Route::get('/list/entity', [ BudgetMaster::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ BudgetMaster::class, 'getListDistributorByEntityId']);
        Route::get('/list/category', [ BudgetMaster::class, 'getListCategory']);
        Route::post('/save', [ BudgetMaster::class, 'save']);
        Route::post('/update', [ BudgetMaster::class, 'update']);
        Route::post('/delete', [ BudgetMaster::class, 'delete']);
        Route::get('/export-xls', [ BudgetMaster::class, 'exportXls']);
    });

    // Budget Allocation
    Route::group(['prefix' => '/budget/allocation'], function() {
        Route::get('/', [ BudgetAllocation::class, 'landingPage'])->name('budget-allocation');
        Route::get('/list/paginate/filter', [ BudgetAllocation::class, 'getListPaginateFilter']);
        Route::get('/form', [ BudgetAllocation::class, 'budgetAllocationFormPage'])->name('budget-allocation.form');
        Route::get('/get-data/id', [ BudgetAllocation::class, 'getDataByID']);
        Route::get('/list/entity', [ BudgetAllocation::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ BudgetAllocation::class, 'getListDistributorByEntityId']);
        Route::get('/list/source', [ BudgetAllocation::class, 'getListBudgetSource']);
        Route::get('/get-data/source/id', [ BudgetAllocation::class, 'getDataBudgetSourceById']);
        Route::get('/list/sub-category', [ BudgetAllocation::class, 'getListSubCategory']);
        Route::get('/list/activity', [ BudgetAllocation::class, 'getListActivity']);
        Route::get('/list/sub-activity', [ BudgetAllocation::class, 'getListSubActivity']);
        Route::get('/list/region', [ BudgetAllocation::class, 'getListRegion']);
        Route::get('/list/channel', [ BudgetAllocation::class, 'getListChannel']);
        Route::get('/list/sub-channel', [ BudgetAllocation::class, 'getListSubChannel']);
        Route::get('/list/account', [ BudgetAllocation::class, 'getListAccount']);
        Route::get('/list/sub-account', [ BudgetAllocation::class, 'getListSubAccount']);
        Route::get('/list/user', [ BudgetAllocation::class, 'getListUser']);
        Route::get('/list/brand', [ BudgetAllocation::class, 'getListBrand']);
        Route::get('/list/sku', [ BudgetAllocation::class, 'getListSku']);
        Route::post('/save', [ BudgetAllocation::class, 'save']);
        Route::post('/update', [ BudgetAllocation::class, 'update']);
        Route::get('/export-xls', [ BudgetAllocation::class, 'exportXls']);
    });

    // Budget Assignment
    Route::group(['prefix' => '/budget/assignment'], function() {
        Route::get('/', [ BudgetAssignment::class, 'landingPage'])->name('budget-assignment');
        Route::get('/list/paginate/filter', [ BudgetAssignment::class, 'getListPaginateFilter']);
        Route::get('/form', [ BudgetAssignment::class, 'budgetAssignmentFormPage'])->name('budget-assignment.form');
        Route::get('/get-data/id', [ BudgetAssignment::class, 'getDataByID']);
        Route::get('/list/entity', [ BudgetAssignment::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ BudgetAssignment::class, 'getListDistributorByEntityId']);
        Route::get('/list/source', [ BudgetAssignment::class, 'getListBudgetSource']);
        Route::get('/get-data/source/id', [ BudgetAssignment::class, 'getDataBudgetSourceById']);
        Route::post('/save', [ BudgetAssignment::class, 'save']);
        Route::post('/update', [ BudgetAssignment::class, 'update']);
        Route::get('/export-xls', [ BudgetAssignment::class, 'exportXls']);
    });

    // Budget Approval
    Route::group(['prefix' => '/budget/approval'], function() {
        Route::get('/', [ BudgetApproval::class, 'landingPage'])->name('budget-approval');
        Route::get('/list/paginate/filter', [ BudgetApproval::class, 'getList']);
        Route::get('/list/entity', [ BudgetApproval::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ BudgetApproval::class, 'getListDistributorByEntityId']);
        Route::get('/list/channel', [ BudgetApproval::class, 'getListChannel']);
        Route::post('/approve', [ BudgetApproval::class, 'approve']);
        Route::post('/unapprove', [ BudgetApproval::class, 'unApprove']);
        Route::get('/export-xls', [ BudgetApproval::class, 'exportXls']);
    });

    // Budget History
    Route::group(['prefix' => '/budget/history'], function() {
        Route::get('/', [ BudgetHistory::class, 'landingPage'])->name('budget-history');
        Route::get('/list/paginate/filter', [ BudgetHistory::class, 'getListPaginateFilter']);
        Route::get('/list/entity', [ BudgetHistory::class, 'getListEntity']);
        Route::get('/list/distributor/entity-id', [ BudgetHistory::class, 'getListDistributorByEntityId']);
        Route::get('/export-xls', [ BudgetHistory::class, 'exportXls']);
    });

    // Budget Conversion Rate
    Route::group(['prefix' => '/budget/ss-input/conversion-rate'], function() {
        Route::get('/', [ BudgetConversionRate::class, 'landingPage'])->name('budget-ss-input-conversion-rate');
        Route::get('/upload-form', [ BudgetConversionRate::class, 'uploadPage'])->name('budget-ss-input-conversion-rate.upload');
        Route::get('/list/paginate/filter', [ BudgetConversionRate::class, 'getListPaginateFilter']);
        Route::get('/list/channel', [ BudgetConversionRate::class, 'getListChannel']);
        Route::get('/list/sub-channel', [ BudgetConversionRate::class, 'getListSubChannel']);
        Route::get('/list/groupBrand', [ BudgetConversionRate::class, 'getListGroupBrand']);
        Route::get('/export-xls', [ BudgetConversionRate::class, 'exportXls']);
        Route::get('/export-csv', [ BudgetConversionRate::class, 'exportCsv']);
        Route::post('/upload-xls', [ BudgetConversionRate::class, 'uploadXls']);
        Route::post('/send-email', [ BudgetConversionRate::class, 'sendEmailApprover']);
        Route::post('/recon/send-email', [ BudgetConversionRate::class, 'sendEmailApproverRecon']);
        Route::get('/download-template', [ BudgetConversionRate::class, 'downloadTemplate']);
    });

    // Budget Volume
    Route::group(['prefix' => '/budget/ss-input/volume'], function() {
        Route::get('/', [ BudgetVolume::class, 'landingPage'])->name('budget-ss-input-volume');
        Route::get('/upload-form', [ BudgetVolume::class, 'uploadPage'])->name('budget-ss-input-volume.upload');
        Route::get('/list/paginate/filter', [ BudgetVolume::class, 'getListPaginateFilter']);
        Route::get('/list/channel', [ BudgetVolume::class, 'getListChannel']);
        Route::get('/list/region', [ BudgetVolume::class, 'getListRegion']);
        Route::get('/list/sub-channel/channel-id', [ BudgetVolume::class, 'getListSubChannel']);
        Route::get('/list/account/sub-channel-id', [ BudgetVolume::class, 'getListAccount']);
        Route::get('/list/sub-account/account-id', [ BudgetVolume::class, 'getListSubAccount']);
        Route::get('/list/groupBrand', [ BudgetVolume::class, 'getListGroupBrand']);
        Route::post('/upload-xls', [ BudgetVolume::class, 'uploadXls']);
        Route::get('/download-template', [ BudgetVolume::class, 'downloadTemplate']);
        Route::get('/export-xls', [ BudgetVolume::class, 'exportXls']);
        Route::get('/export-csv', [ BudgetVolume::class, 'exportCsv']);
        Route::post('/send-email', [ BudgetVolume::class, 'sendEmailApprover']);
        Route::post('/recon/send-email', [ BudgetVolume::class, 'sendEmailApproverRecon']);
    });

    // Budget PS Input
    Route::group(['prefix' => '/budget/ss-input/ps-input'], function() {
        Route::get('/', [ BudgetPSInput::class, 'landingPage' ])->name('budget-ss-input-ps-input');
        Route::get('/upload-form', [ BudgetPSInput::class, 'uploadPage' ])->name('budget-ss-input-ps-input.upload');
        Route::get('/list/paginate/filter', [ BudgetPSInput::class, 'getListPaginateFilter']);
        Route::get('/list/filter', [ BudgetPSInput::class, 'getListFilter']);
        Route::get('/export-xls', [ BudgetPSInput::class, 'exportXls']);
        Route::get('/export-csv', [ BudgetPSInput::class, 'exportCsv']);
        Route::get('/download-template', [ BudgetPSInput::class, 'downloadTemplate']);
        Route::post('/upload-xls', [ BudgetPSInput::class, 'uploadXls']);
        Route::post('/send-email', [ BudgetVolume::class, 'sendEmailApprover']);
        Route::post('/recon/send-email', [ BudgetVolume::class, 'sendEmailApproverRecon']);
    });

    // Budget TT Console
    Route::group(['prefix' => '/budget/tt-console'], function() {
        Route::get('/', [ BudgetTTConsole::class, 'landingPage'])->name('budget-tt-console');
        Route::get('/form', [ BudgetTTConsole::class, 'formPage'])->name('budget-tt-console.form');
        Route::get('/upload-form-rc', [ BudgetTTConsole::class, 'uploadPageRC'])->name('budget-tt-console.upload-rc');
        Route::get('/upload-form-dc', [ BudgetTTConsole::class, 'uploadPageDC'])->name('budget-tt-console.upload-dc');
        Route::get('/list/paginate/filter', [ BudgetTTConsole::class, 'getListPaginateFilter']);
        Route::get('/list/category', [ BudgetTTConsole::class, 'getListCategory']);
        Route::get('/list/sub-category/category-id', [ BudgetTTConsole::class, 'getListSubCategory']);
        Route::get('/list/channel', [ BudgetTTConsole::class, 'getListChannel']);
        Route::get('/list/sub-channel/channel-id', [ BudgetTTConsole::class, 'getListSubChannel']);
        Route::get('/list/account/sub-channel-id', [ BudgetTTConsole::class, 'getListAccount']);
        Route::get('/list/sub-account/account-id', [ BudgetTTConsole::class, 'getListSubAccount']);
        Route::get('/list/distributor', [ BudgetTTConsole::class, 'getListDistributor']);
        Route::get('/list/sub-activity-type/category-id', [ BudgetTTConsole::class, 'getListSubActivityType']);
        Route::get('/list/activity/category-id', [ BudgetTTConsole::class, 'getListActivity']);
        Route::get('/list/sub-activity/category-id-activity-id', [ BudgetTTConsole::class, 'getListSubActivity']);
        Route::get('/list/groupBrand', [ BudgetTTConsole::class, 'getListGroupBrand']);
        Route::post('/upload-xls-rc', [ BudgetTTConsole::class, 'uploadXlsRC']);
        Route::post('/upload-xls-dc', [ BudgetTTConsole::class, 'uploadXlsDC']);
        Route::get('/download-template-rc', [ BudgetTTConsole::class, 'downloadTemplateRC']);
        Route::get('/download-template-dc', [ BudgetTTConsole::class, 'downloadTemplateDC']);
        Route::get('/export-xls', [ BudgetTTConsole::class, 'exportXls']);
        Route::get('/data/id', [ BudgetTTConsole::class, 'getDataByID']);
        Route::post('/save', [ BudgetTTConsole::class, 'save']);
        Route::post('/update', [ BudgetTTConsole::class, 'update']);
        Route::post('/send-email', [ BudgetTTConsole::class, 'sendEmailApprover']);
        Route::post('/recon/send-email', [ BudgetTTConsole::class, 'sendEmailApproverRecon']);
    });

    // Budget Approval Request
    Route::group(['prefix' => '/budget/approval-request'], function() {
        Route::get('/', [ BudgetApprovalRequest::class, 'landingPage'])->name('budget-request-approval');
        Route::get('/list/filter', [ BudgetApprovalRequest::class, 'getListFilter']);
        Route::get('/list-summary', [ BudgetApprovalRequest::class, 'getListSummary']);
        Route::get('/list-detail', [ BudgetApprovalRequest::class, 'getListDetail']);
        Route::get('/download-pdf-above', [ BudgetApprovalRequest::class, 'downloadPdfAbove']);
        Route::get('/download-pdf-below', [ BudgetApprovalRequest::class, 'downloadPdfBelow']);
        Route::get('/check-batch', [ BudgetApprovalRequest::class, 'checkBatchId']);
        Route::get('/download-excel', [ BudgetApprovalRequest::class, 'downloadExcel']);
        Route::post('/send', [ BudgetApprovalRequest::class, 'sendEmailRequest']);
    });

    // Budget Deployment
    Route::group(['prefix' => '/budget/deployment'], function() {
        Route::get('/', [ BudgetDeployment::class, 'landingPage'])->name('budget-deployment');
        Route::get('/list/filter', [ BudgetDeployment::class, 'getListFilter']);
        Route::get('/list', [ BudgetDeployment::class, 'getList']);
        Route::get('/process', [ BudgetDeployment::class, 'processPage']);
        Route::get('/process/list-promo', [ BudgetDeployment::class, 'getListPromo']);
        Route::post('/process/send-email', [ BudgetDeployment::class, 'sendEmail']);
        Route::post('/deploy', [ BudgetDeployment::class, 'deploy']);
        Route::get('/download-excel', [ BudgetDeployment::class, 'downloadExcel']);
    });
});

Route::group(['prefix' => '/budget/approval-request/'], function() {
    Route::get('/action', [ BudgetApprovalRequest::class, 'actionPage']);
    Route::post('/reject', [ BudgetApprovalRequest::class, 'reject']);
    Route::post('/approve', [ BudgetApprovalRequest::class, 'approve']);
});

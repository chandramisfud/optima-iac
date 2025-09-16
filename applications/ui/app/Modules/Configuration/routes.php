<?php

use Illuminate\Support\Facades\Route;
use App\Modules\Configuration\Controllers\Reminder;
use App\Modules\Configuration\Controllers\ROICR;
use App\Modules\Configuration\Controllers\LatePromoCreation;
use App\Modules\Configuration\Controllers\PromoInitiatioReminder;
use App\Modules\Configuration\Controllers\MajorChanges;
use App\Modules\Configuration\Controllers\EditablePromoItems;
use App\Modules\Configuration\Controllers\PromoCalculator;
use App\Modules\Configuration\Controllers\MechanismInputMethod;

Route::group(['middleware' => ['web', 'checkSession']], function () {
    // Reminder
    Route::group(['prefix' => '/configuration/reminder'], function() {
        Route::get('/', [ Reminder::class, 'landingPage'])->name('configuration-reminder');
        Route::get('/get-data', [ Reminder::class, 'getDataConfig']);
        Route::post('/update', [ Reminder::class, 'update']);
    });

    // ROI & CR
    Route::group(['prefix' => '/configuration/roi-cr'], function() {
        Route::get('/', [ ROICR::class, 'landingPage'])->name('configuration-roi-cr');
        Route::get('/list', [ ROICR::class, 'getList']);
        Route::get('/form', [ ROICR::class, 'roiCRFormPage'])->name('configuration-roi-cr.from');
        Route::get('/list/category', [ ROICR::class, 'getListCategory']);
        Route::get('/list/sub-category/category-id', [ ROICR::class, 'getListSubCategoryByCategoryId']);
        Route::get('/list/activity/subcategory-id', [ ROICR::class, 'getListActivityBySubCategoryId']);
        Route::post('/save', [ ROICR::class, 'save']);
        Route::post('/delete', [ ROICR::class, 'delete']);
    });

    // Late Promo Creation
    Route::group(['prefix' => '/configuration/late-promo-creation'], function() {
        Route::get('/', [ LatePromoCreation::class, 'landingPage'])->name('configuration-late-promo-creation');
        Route::get('/get-data', [ LatePromoCreation::class, 'getDataConfig']);
        Route::post('/update', [ LatePromoCreation::class, 'update']);
    });

    // Promo Initiator Reminder
    Route::group(['prefix' => '/configuration/promo-initiator-reminder'], function() {
        Route::get('/', [ PromoInitiatioReminder::class, 'landingPage'])->name('configuration-promo-initiator-reminder');
        Route::get('/get-data', [ PromoInitiatioReminder::class, 'getDataConfig']);
        Route::post('/update', [ PromoInitiatioReminder::class, 'update']);
    });

    // Major Changes
    Route::group(['prefix' => '/configuration/major-changes'], function() {
        Route::get('/', [ MajorChanges::class, 'landingPage'])->name('configuration-major-changes');
        Route::get('/get-data', [ MajorChanges::class, 'getDataConfig']);
        Route::get('/export-xls-history', [ MajorChanges::class, 'exportExcelHistory']);
        Route::post('/submit', [ MajorChanges::class, 'submit']);

        //DC
        Route::get('/get-data-dc', [ MajorChanges::class, 'getDataConfigDC']);
        Route::get('/export-xls-history-dc', [ MajorChanges::class, 'exportExcelHistoryDC']);
        Route::post('/submit-dc', [ MajorChanges::class, 'submitDC']);
    });

    // Editable Promo Item
    Route::group(['prefix' => '/configuration/editable-promo-items'], function() {
        Route::get('/', [ EditablePromoItems::class, 'landingPage'])->name('configuration-editable-promo-items');
        Route::get('/get-data', [ EditablePromoItems::class, 'getDataConfig']);
        Route::get('/export-xls-history', [ EditablePromoItems::class, 'exportExcelHistory']);

        //RC
        Route::post('/submit', [ EditablePromoItems::class, 'submit']);

        //DC
        Route::post('/submit-dc', [ EditablePromoItems::class, 'submitDC']);
    });

    // Promo Calculator
    Route::group(['prefix' => '/configuration/promo-calculator'], function() {
        Route::get('/', [ PromoCalculator::class, 'landingPage'])->name('configuration-promo-calculator');
        Route::get('/list', [ PromoCalculator::class, 'getList']);
        Route::get('/form', [ PromoCalculator::class, 'formPage'])->name('configuration-promo-calculator.form');
        Route::get('/form-upload', [ PromoCalculator::class, 'formUploadPage'])->name('configuration-promo-calculator.form-upload');
        Route::get('/data', [ PromoCalculator::class, 'getData']);
        Route::get('/coverage', [ PromoCalculator::class, 'getCoverage']);
        Route::get('/channel', [ PromoCalculator::class, 'getChannel']);
        Route::get('/filter', [ PromoCalculator::class, 'getFilter']);
        Route::post('/save', [ PromoCalculator::class, 'save']);
        Route::post('/update', [ PromoCalculator::class, 'update']);
        Route::get('/export-xls', [ PromoCalculator::class, 'exportXls']);
    });

    // Mechanism Input Method
    Route::group(['prefix' => '/configuration/mechanism-input-method'], function() {
        Route::get('/', [ MechanismInputMethod::class, 'landingPage'])->name('configuration-mechanism-input-method');
        Route::get('/list', [ MechanismInputMethod::class, 'getList']);
        Route::get('/download-template', [ MechanismInputMethod::class, 'downloadTemplate']);
        Route::post('/upload', [ MechanismInputMethod::class, 'upload']);
    });
});

<?php

use Illuminate\Support\Facades\Route;
use App\Modules\Dashboard\Controllers\DashboardController;
use App\Modules\Dashboard\Controllers\MainDashboard;
use App\Modules\Dashboard\Controllers\SummaryDashboard;
use App\Modules\Dashboard\Controllers\ApproverDashboard;
use App\Modules\Dashboard\Controllers\CreatorDashboard;
use App\Modules\Dashboard\Controllers\PromoCalendar;

Route::group(['middleware' => ['web', 'checkSession']], function () {
    Route::get('/', [ DashboardController::class, 'landingPage'])->name('dashboard');
    Route::post('/dashboard/search', [ DashboardController::class, 'search']);

    //Main Dashboard
    Route::group(['prefix' => '/dashboard/main'], function() {
        Route::get('/', [ MainDashboard::class, 'landingPage' ])->name('dashboard-main');
        Route::get('/list/distributor', [ MainDashboard::class, 'getListDistributor' ]);
        Route::get('/list/channel', [ MainDashboard::class, 'getListChannel' ]);
        Route::get('/list/account', [ MainDashboard::class, 'getListAccount' ]);
        Route::get('/list/category', [ MainDashboard::class, 'getListCategory' ]);
        Route::get('/data', [ MainDashboard::class, 'getDataMain' ]);
        Route::get('/notifications', [ MainDashboard::class, 'getDataNotifications' ]);
        Route::get('/promo-creation-trend', [ MainDashboard::class, 'getDataPromoCreationTrend' ]);
        Route::get('/outstanding-dn', [ MainDashboard::class, 'getDataOutstandingDN' ]);
    });

    // Summary Dashboard
    Route::group(['prefix' => '/dashboard/summary'], function() {
        Route::get('/', [SummaryDashboard::class, 'landingPage'])->name('dashboard-summary');
        Route::get('/data', [ SummaryDashboard::class, 'getDataSummary' ]);
        Route::get('/data/creator-leagues-summary', [ SummaryDashboard::class, 'getDataCreatorLeaguesSummary' ]);
        Route::get('/data/creator-leagues-standing', [ SummaryDashboard::class, 'getDataCreatorStanding' ]);
        Route::get('/data/approver-leagues-standing', [ SummaryDashboard::class, 'getDataApproverStanding' ]);
        Route::get('/export-xls', [ SummaryDashboard::class, 'exportXls' ]);
        Route::get('/export-xls-detail', [ SummaryDashboard::class, 'exportXlsDetail' ]);
    });

    // Approver Dashboard
    Route::group(['prefix' => '/dashboard/approver'], function() {
        Route::get('/', [ ApproverDashboard::class, 'landingPage'])->name('dashboard-approver');
        Route::get('/data', [ ApproverDashboard::class, 'getDataApproverDashboard' ]);
    });

    // Creator Dashboard
    Route::group(['prefix' => '/dashboard/creator'], function() {
        Route::get('/', [ CreatorDashboard::class, 'landingPage'])->name('dashboard-creator');
        Route::get('/data', [ CreatorDashboard::class, 'getDataCreator' ]);
        Route::get('/data/leagues-summary', [ CreatorDashboard::class, 'getDataCreatorLeaguesSummary' ]);
        Route::get('/data/leagues-standing', [ CreatorDashboard::class, 'getDataCreatorStanding' ]);
    });

    // Promo Calendar
    Route::group(['prefix' => '/dashboard/promo-calendar'], function() {
        Route::get('/', [ PromoCalendar::class, 'landingPage' ])->name('dashboard-promo-calendar');
        Route::get('/list/entity', [ PromoCalendar::class, 'getListEntity' ]);
        Route::get('/list/channel', [ PromoCalendar::class, 'getListChannel' ]);
        Route::get('/list/account', [ PromoCalendar::class, 'getListAccount' ]);
        Route::get('/list/category', [ PromoCalendar::class, 'getListCategory' ]);
        Route::get('/list/calendar', [ PromoCalendar::class, 'getListCalendar' ]);
    });

});

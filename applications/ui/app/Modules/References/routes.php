<?php

use Illuminate\Support\Facades\Route;

use App\Modules\References\Controllers\Finance;
use App\Modules\References\Controllers\Sales;
use App\Modules\References\Controllers\Distributor;

Route::group(['middleware' => ['web', 'checkSession']], function () {

    // Finance
    Route::group(['prefix' => '/references/finance'], function () {
        Route::get('/', [ Finance::class, 'landingPage'])->name('references-finance');
        Route::get('/check-file', [ Finance::class, 'checkFile']);
        Route::post('/upload', [ Finance::class, 'upload']);
    });

    // Sales
    Route::group(['prefix' => '/references/sales'], function () {
        Route::get('/', [ Sales::class, 'landingPage'])->name('references-sales');
        Route::get('/check-file', [ Sales::class, 'checkFile']);
        Route::post('/upload', [ Sales::class, 'upload']);
    });

    // Distributor
    Route::group(['prefix' => '/references/distributor'], function () {
        Route::get('/', [ Distributor::class, 'landingPage'])->name('references-distributor');
        Route::get('/check-file', [ Distributor::class, 'checkFile']);
        Route::get('/list/distributor', [ Distributor::class, 'getListDistributor']);
        Route::post('/upload', [ Distributor::class, 'upload']);
    });

});

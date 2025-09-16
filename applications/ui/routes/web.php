<?php

use Illuminate\Support\Facades\Route;
use App\Http\Controllers\Search;
use App\Http\Controllers\AppLog;
use App\Http\Controllers\AppDrive;
use App\Http\Controllers\ResendEmailApproval;

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| contains the "web" middleware group. Now create something great!
|
*/

Route::group(['middleware' => ['web', 'checkSession']], function () {

    // Search
    Route::get('/search', [ Search::class, 'search' ]);

    // Application Log
    Route::get('/app-log', [ AppLog::class, 'landingPage']);
    Route::get('/app-log/list', [ AppLog::class, 'getListLog']);
    Route::get('/app-log/download', [ AppLog::class, 'downloadLog']);

    //Application Drive
    Route::get('/app-drive', [ AppDrive::class, 'landingPage']);
    Route::get('/app-drive/list', [ AppDrive::class, 'getListDrive']);
    Route::get('/app-drive/export-excel', [ AppDrive::class, 'exportExcel']);

    // Resending Approval
    Route::get('/resend-email-approval', [ ResendEmailApproval::class, 'landingPage' ] );
    Route::get('/resend-email-approval/list', [ ResendEmailApproval::class, 'getList' ] );
    Route::get('/resend-email-approval/confirm-key', [ ResendEmailApproval::class, 'confirmKey' ] );
    Route::post('/resend-email-approval/send-email-cycle1', [ ResendEmailApproval::class, 'sendEmailCycle1' ] );
    Route::post('/resend-email-approval/send-email-cycle2', [ ResendEmailApproval::class, 'sendEmailCycle2' ] );
});

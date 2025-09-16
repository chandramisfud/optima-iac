<?php

use Illuminate\Support\Facades\Route;
use App\Modules\Auth\Controllers\AuthController;
use App\Modules\Auth\Controllers\CheckAccess;

Route::group(['middleware' => ['web']], function () {
    Route::get('login-page', [ AuthController::class, 'loginPage'])->middleware('checkLogin')->name('login');
    Route::post('verify-captcha', [ AuthController::class, 'verifyCaptcha']);
    Route::get('check-form-access', [ CheckAccess::class, 'checkAccess']);

    Route::group(['prefix' => 'auth'], function() {
        Route::post('/sign-in', [ AuthController::class, 'signIn']);
        Route::get('/sign-out', [ AuthController::class, 'signOut']);
        Route::post('/is-login-put', [ AuthController::class, 'isLoginPut']);

        Route::post('/profile/push-session', [ AuthController::class, 'profilePushSession']);
        Route::get('/profile', [ AuthController::class, 'getProfileLogin']);
        Route::get('/clear-session', [ AuthController::class, 'clearSession']);

    });

    Route::get('refresh-csrf', function(){
        return csrf_token();
    });
});
Route::get('auth/forgot-password-page', [ AuthController::class, 'forgotPasswordPage']);
Route::post('auth/forgot-password', [ AuthController::class, 'forgotPassword']);
Route::post('auth/change-password', [ AuthController::class, 'changePassword']);
Route::post('auth/change-password-new-user', [ AuthController::class, 'changePasswordNewUser']);

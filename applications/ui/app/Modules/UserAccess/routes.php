<?php

use App\Modules\UserAccess\Controllers\MyProfile;
use App\Modules\UserAccess\Controllers\UsergroupController;
use App\Modules\UserAccess\Controllers\UsergroupRightsController;
use App\Modules\UserAccess\Controllers\UserController;
use App\Modules\UserAccess\Controllers\UserProfileController;
use App\Modules\UserAccess\Controllers\UserAdminReport;
use Illuminate\Support\Facades\Route;

Route::group(['middleware' => ['web', 'checkSession']], function () {

    // My Profile
    Route::get('/my-profile', [ MyProfile::class, 'myProfilePage' ]);
    Route::post('/my-profile/picture/store', [ MyProfile::class, 'myProfilePictureStore' ]);
    Route::post('/my-profile/change-password', [ MyProfile::class, 'changeOldPassword' ]);

    // Usergroup
    Route::group(['prefix' => '/useraccess/group-menu'], function() {
        Route::get('/', [ UsergroupController::class, 'landingPage'])->name('useraccess-groupmenu');
        Route::get('/list/paginate/filter', [ UsergroupController::class, 'getListPaginateFilter']);
        Route::get('/form', [ UsergroupController::class, 'usergroupFormPage'])->name('useraccess-groupmenu.form');
        Route::get('/get-data/id', [ UsergroupController::class, 'getDataByID']);
        Route::get('/get-data/groupmenupermission', [ UsergroupController::class, 'getGroupMenuPermission']);
        Route::post('/save', [ UsergroupController::class, 'save']);
        Route::post('/update', [ UsergroupController::class, 'update']);
        Route::post('/delete', [ UsergroupController::class, 'delete']);
        Route::get('/form-groupmenu-config', [ UsergroupController::class, 'usergroupMenuConfigFormPage'])->name('useraccess-groupmenu.form-groupmenu');
        Route::post('/save-groupmenu-config', [ UsergroupController::class, 'saveGroupMenu']);
        Route::get('/list/menu/usergroupid', [ UsergroupController::class, 'getListMenuByUserGroup']);
        Route::get('/form-grouprights-config', [ UsergroupController::class, 'usergroupRightsConfigFormPage'])->name('useraccess-groupmenu.form-grouprights');
        Route::get('/list/userlevel/usergroupid', [ UsergroupController::class, 'getListUserLevelByUserGroup']);
        Route::get('/list/menu/userlevel', [ UsergroupController::class, 'getListMenuByUserLevel']);
        Route::get('/get-data/userrights', [ UsergroupController::class, 'getDataUserRightsByMenu']);
        Route::post('/save-grouprights-config', [ UsergroupController::class, 'saveGroupRights']);
        Route::get('/export-xls', [ UsergroupController::class, 'exportXls']);
    });


    // User Group Rights
    Route::group(['prefix' => 'useraccess/group-rights'], function() {
        Route::get('/', [ UsergroupRightsController::class, 'landingPage'])->name('useraccess-rights');
        Route::get('/list/paginate/filter', [ UsergroupRightsController::class, 'getListPaginateFilter']);
        Route::get('/get-data/id', [ UsergroupRightsController::class, 'getDataByID']);
        Route::get('/get-data/usergroupmenu', [ UsergroupRightsController::class, 'getUserGroupMenu']);
        Route::get('/form', [ UsergroupRightsController::class, 'formPage'])->name('useraccess-rights.form');
        Route::post('/save', [ UsergroupRightsController::class, 'save']);
        Route::post('/update', [ UsergroupRightsController::class, 'update']);
        Route::post('/delete', [ UsergroupRightsController::class, 'delete']);
        Route::get('/export-xls', [ UsergroupRightsController::class, 'exportXls']);
    });

    // User
    Route::group(['prefix' => 'useraccess/user'], function() {
        Route::get('/', [ UserController::class, 'landingPage'])->name('useraccess-users');
        Route::get('/list/paginate/filter', [ UserController::class, 'getListPaginateFilter']);
        Route::get('/form', [ UserController::class, 'userFormPage'])->name('useraccess-users.form');
        Route::get('/list/profile', [ UserController::class, 'getListProfile']);
        Route::get('/get-data/id', [ UserController::class, 'getDataByID']);
        Route::post('/save', [ UserController::class, 'save']);
        Route::post('/update', [ UserController::class, 'update']);
        Route::post('/delete', [ UserController::class, 'delete']);
        Route::post('/activate', [ UserController::class, 'activate']);
        Route::post('/reset-password', [ UserController::class, 'resetPassword']);
        Route::get('/export-xls', [ UserController::class, 'exportXls']);

    });

    // User Profile
    Route::group(['prefix' => 'useraccess/profile'], function() {
        Route::get('/', [ UserProfileController::class, 'landingPage'])->name('useraccess-profile');
        Route::get('/list/paginate/filter', [ UserProfileController::class, 'getListPaginateFilter']);
        Route::get('/get-data/id', [ UserProfileController::class, 'getDataByID']);
        Route::get('/usergroupmenu', [ UserProfileController::class, 'getListUserGroup']);
        Route::get('/usergrouprights/usergroupid', [ UserProfileController::class, 'getListUserRightsByUserGroupId']);
        Route::get('/distributor', [ UserProfileController::class, 'getListDistributor']);
        Route::get('/category', [ UserProfileController::class, 'getListCategory']);
        Route::get('/channel', [ UserProfileController::class, 'getListChannel']);
        Route::get('/form', [ UserProfileController::class, 'formPage'])->name('useraccess-profile.form');
        Route::post('/save', [ UserProfileController::class, 'save']);
        Route::post('/update', [ UserProfileController::class, 'update']);
        Route::post('/delete', [ UserProfileController::class, 'delete']);
        Route::post('/activate', [ UserProfileController::class, 'activate']);
        Route::get('/export-xls', [ UserProfileController::class, 'exportXls']);
    });

    // User Admin Report
    Route::group(['prefix' => 'useraccess/user-admin-report'], function() {
        Route::get('/', [ UserAdminReport::class, 'landingPage'])->name('useraccess-user-admin-report');
        Route::get('/list/paginate/filter', [ UserAdminReport::class, 'getListPaginateFilter']);
        Route::get('/export-xls', [ UserAdminReport::class, 'exportXls']);
    });
});

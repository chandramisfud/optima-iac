<div class="d-flex align-items-stretch flex-shrink-0">
    <div class="d-flex align-items-stretch ms-1 ms-lg-3">
        @include('panels.search')
    </div>
    <div class="d-flex align-items-center ms-1 ms-lg-3" id="kt_header_user_menu_toggle">
        <span class="text-white fw-bold d-mprofile_pictured-inline align-middle me-1">Hi, {{ @Session::get('name') . ' - ' . @Session::get('profile') }}</span>
        <div class="cursor-pointer symbol symbol-30px symbol-md-30px" data-kt-menu-trigger="click" data-kt-menu-attach="parent" data-kt-menu-placement="bottom-end">
            <img src="{{ '/assets/media/users/' . @Session::get('userid') . '.png' }}" onerror="this.onerror=null; this.src='{{ asset('assets/media/custom/user.png') }}'" alt="image">
        </div>
        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg menu-state-primary fw-bold pb-4 fs-6 w-400px" data-kt-menu="true">
            <div class="menu-item px-0">
                <div class="menu-content d-flex align-items-center h-lg-100px px-3 menu-topbar-profile">
                    <div class="symbol symbol-70px me-5">
                        <img class="rounded rounded-circle" src="{{ '/assets/media/users/' . @Session::get('userid') . '.png' }}" onerror="this.onerror=null; this.src='{{ asset('assets/media/custom/user.png') }}'" alt="image">
                    </div>
                    <div class="d-flex flex-column">
                        <div class="d-flex align-items-center fs-4 text-white">{{ @Session::get('name') }}
                        </div>
                        <a href="#" class="fw-bold text-white fs-5">as {{ @Session::get('profile') }}</a>
                    </div>
                </div>
            </div>
            <div class="separator my-2"></div>
            <div class="menu-item px-5">
                <a href="/my-profile" class="menu-link px-5">
                    <span class="fa fa-user text-success me-3"></span>
                    My Profile
                </a>
            </div>
            <div class="separator my-2"></div>
            <div class="menu-item px-5 my-1">
                <a href="javascript:void(0);" class="menu-link px-5" id="btn_switch_profile">
                    <span class="fa fa-users text-success me-3"></span>
                    Profile Change
                </a>
            </div>
            <div class="menu-item px-5">
                <a href="/auth/sign-out" class="menu-link px-5">
                    <span class="fa fa-sign-out-alt text-success me-3"></span>
                    Sign Out
                </a>
            </div>
        </div>
    </div>
</div>

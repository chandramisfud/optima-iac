@extends('layouts/layoutMaster')

@section('title', 'My Profile')

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/useraccess/my-profile/css/my-profile-methods.css') }}"  rel="stylesheet" type="text/css" />
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">User
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Profile</small>
    </span>
@endsection

@section('button-toolbar-left')

@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-lg-4 col-12 mb-3">
            <div class="card shadow-sm card_form" id="card_myprofile">
                <div class="card-body">
                    <div class="d-flex flex-wrap flex-sm-nowrap">
                        <div class="me-7 mb-4">
                            <div class="image-input image-input-outline" id="kt_image_input_control" style="background-image: url('assets/media/svg/avatars/blank.svg')">
                                <div class="image-input-wrapper w-125px h-125px" style="background-image: url({{ 'assets/media/users/' . @Session::get('userid') . '.png' }}); background-size: 100%;background-position-x: center;background-position-y: center;"></div>
                                <label class="btn btn-icon btn-circle btn-outline-optima w-25px h-25px bg-body shadow" data-kt-image-input-action="change" data-bs-toggle="tooltip" title="" data-bs-original-title="Change avatar">
                                    <i class="bi bi-pencil-fill fs-6"></i>
                                    <input type="file" id="avatar" name="avatar" accept=".png, .jpg, .jpeg">
                                    <input type="hidden" name="avatar_remove">
                                </label>
                            </div>
                        </div>
                        <div class="flex-grow-1">
                            <div class="d-flex justify-content-between align-items-start flex-wrap mb-2">
                                <div class="d-flex flex-column">
                                    <div class="d-flex align-items-center mb-2">
                                        <a href="#" class="text-gray-700 text-hover-optima fs-3 fw-bold me-1" title="User name">{{ @Session::get('name') }}</a>
                                        <a href="#">
                                            <span class="svg-icon svg-icon-1 svg-icon-primary">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="20px" height="20px" viewBox="0 0 24 24">
                                                    <path d="M10.0813 3.7242C10.8849 2.16438 13.1151 2.16438 13.9187 3.7242V3.7242C14.4016 4.66147 15.4909 5.1127 16.4951 4.79139V4.79139C18.1663 4.25668 19.7433 5.83365 19.2086 7.50485V7.50485C18.8873 8.50905 19.3385 9.59842 20.2758 10.0813V10.0813C21.8356 10.8849 21.8356 13.1151 20.2758 13.9187V13.9187C19.3385 14.4016 18.8873 15.491 19.2086 16.4951V16.4951C19.7433 18.1663 18.1663 19.7433 16.4951 19.2086V19.2086C15.491 18.8873 14.4016 19.3385 13.9187 20.2758V20.2758C13.1151 21.8356 10.8849 21.8356 10.0813 20.2758V20.2758C9.59842 19.3385 8.50905 18.8873 7.50485 19.2086V19.2086C5.83365 19.7433 4.25668 18.1663 4.79139 16.4951V16.4951C5.1127 15.491 4.66147 14.4016 3.7242 13.9187V13.9187C2.16438 13.1151 2.16438 10.8849 3.7242 10.0813V10.0813C4.66147 9.59842 5.1127 8.50905 4.79139 7.50485V7.50485C4.25668 5.83365 5.83365 4.25668 7.50485 4.79139V4.79139C8.50905 5.1127 9.59842 4.66147 10.0813 3.7242V3.7242Z" fill="#0abb87"></path>
                                                    <path class="permanent" d="M14.8563 9.1903C15.0606 8.94984 15.3771 8.9385 15.6175 9.14289C15.858 9.34728 15.8229 9.66433 15.6185 9.9048L11.863 14.6558C11.6554 14.9001 11.2876 14.9258 11.048 14.7128L8.47656 12.4271C8.24068 12.2174 8.21944 11.8563 8.42911 11.6204C8.63877 11.3845 8.99996 11.3633 9.23583 11.5729L11.3706 13.4705L14.8563 9.1903Z" fill="white"></path>
                                                </svg>
                                            </span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mt-5">
                        <div class="col-md-12 col-12">
                            <div class="d-flex justify-content-between mb-2">
                                <div class="fw-bold">Email: </div>
                                <div class="text-gray-700">{{ @Session::get('email') }}</div>
                            </div>
                            <div class="d-flex justify-content-between mb-3">
                                <div class="fw-bold">Contact Info: </div>
                                <div class="text-gray-700">{{ @Session::get('contact_info') }}</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-8 col-12 mb-3">
            <div class="card shadow-sm card_form">
                <div class="card-header">
                    <span class="card-title fw-normal fs-5">Change Password <span class="fs-6 ms-3 text-gray-700">change or reset your account password</span></span>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-12">
                            <form id="form_change_password" class="form" autocomplete="off">
                                @csrf
                                <div class="row fv-row mb-3 mt-1">
                                    <label class="col-lg-3 col-12 col-form-label text-lg-end">New Password</label>
                                    <div class="col-lg-6 col-12">
                                        <div class="inner-addon left-addon right-addon">
                                            <input type="password" class="form-control form-control-sm" placeholder="New Password" aria-label="New Password" name="new_password" id="new_password" />
                                            <span toggle="#new_password" class="fa fa-eye fs-4 toggle-new-password"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-3 col-12 col-form-label text-lg-end">Verify Password</label>
                                    <div class="col-lg-6 col-12">
                                        <div class="inner-addon left-addon right-addon">
                                            <input type="password" class="form-control form-control-sm" placeholder="Verify Password" aria-label="Verify Password" name="confirm_password" id="confirm_password" />
                                            <span toggle="#confirm_password" class="fa fa-eye fs-4 toggle-confirm-password"></span>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
                <div class="card-footer text-end">
                    <button type="button" class="btn btn-optima btn-sm" id="btn_submit_change_password">
                        <span class="indicator-label">
                            <span class="fa fa-check me-1"></span> Change Password
                        </span>
                        <span class="indicator-progress">Loading...
                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                        </span>
                    </button>
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/pages/useraccess/my-profile/js/my-profile-methods.js') }}"></script>
@endsection

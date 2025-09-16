<!DOCTYPE html>
<html lang="en">
<!--begin::Head-->
<head>
    <title>Optima System - Reset Password</title>
    <meta charset="utf-8" />
    <meta name="description" content="Optima System" />
    <meta name="keywords" content="Optima System" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta property="og:locale" content="en_US" />
    <meta property="og:type" content="article" />
    <meta property="og:title" content="Optima System" />
    <meta property="og:site_name" content="Optima System" />
    <meta name="csrf-token" content="{{ csrf_token() }}" />
    <meta http-equiv="Content-Security-Policy" content="
        default-src 'self';
        img-src * 'self' data: https:;;
        base-uri 'self';
        object-src 'none';
        script-src 'self';
        style-src 'self' 'unsafe-inline' https://fonts.googleapis.com;
        font-src 'self' https://fonts.gstatic.com;"
    />

    <link rel="shortcut icon" href="{{ asset('assets/media/logos/logo.ico') }}" />
    <!--begin::Fonts-->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700" />
    <!--end::Fonts-->
    <!--begin::Global Stylesheets Bundle(used by all pages)-->
    <link href="{{ asset('assets/plugins/global/plugins.bundle.css') }}" rel="stylesheet" type="text/css" />
    <link href="{{ asset('assets/css/style.bundle.css') }}" rel="stylesheet" type="text/css" />
    <!--end::Global Stylesheets Bundle-->
    <link href="{{ asset('assets/pages/auth/css/forgot-password.css?v=1') }}" rel="stylesheet" type="text/css" />
</head>
<!--end::Head-->
<!--begin::Body-->
<body class="bg-body">
<div class="container-fluid">
    <div class="row">
        <div class="d-flex justify-content-center">
            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                <div class="app-sidebar-logo px-6 my-3">
                    <img alt="Logo" src="{{ asset('assets/media/logos/sidebar-top.png') }}" class="w-lg-25 w-40 app-sidebar-logo-default">
                </div>
                <div class="card shadow-sm card_form">
                    <div class="card-header">
                        <span class="card-title fw-normal fs-5">Reset Password </span>
                    </div>
                    <div class="card-body">
                        <form id="form_reset_password" class="form" autocomplete="off">
                            @csrf
                            <div class="row">
                                <div class="col-12">
                                    <span class="fw-light text-gray-700">
                                        Hi {{ @$email }}, you order to reset password associated with your account, please enter a valid password into the "New Password" field. Valid passwords are more than eight (8) characters long. Enter that password again into the "Confirm Password" field and click button to save your new password.
                                    </span>
                                </div>
                            </div>
                            <div class="separator border-4 my-3"></div>
                            <div class="row">
                                <div class="col-12">
                                    <form id="form_reset_password" class="form" autocomplete="off">
                                        @csrf
                                        <div class="row fv-row">
                                            <label class="col-lg-3 col-12 col-form-label text-lg-end required">New Password</label>
                                            <div class="col-lg-9 col-12">
                                                <div class="inner-addon left-addon right-addon">
                                                    <input type="password" class="form-control form-control-sm" placeholder="New Password" aria-label="New Password" name="new_password" id="new_password" />
                                                    <span toggle="#new_password" class="fa fa-eye fs-4 toggle-new-password"></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-3 col-12 col-form-label text-lg-end required">Confirm Password</label>
                                            <div class="col-lg-9 col-12">
                                                <div class="inner-addon left-addon right-addon">
                                                    <input type="password" class="form-control form-control-sm" placeholder="Confirm Password" aria-label="Confirm Password" name="confirm_password" id="confirm_password" />
                                                    <span toggle="#confirm_password" class="fa fa-eye fs-4 toggle-confirm-password"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </form>
                    </div>
                    <div class="card-footer text-end">
                        <button type="button" class="btn btn-info btn-sm" id="btn_submit_reset_password">
                            <span class="indicator-label">
                                <span class="fa fa-check me-1"></span> Submit
                            </span>
                            <span class="indicator-progress">Loading...
                                <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                            </span>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script src="{{ asset('assets/plugins/global/plugins.bundle.js') }}"></script>
<script src="{{ asset('assets/js/scripts.bundle.js') }}"></script>
<script src="{{ asset('assets/pages/auth/js/forgot-password.js') }}"></script>
</body>
</html>

<!DOCTYPE html>
<html lang="en">
<!--begin::Head-->
<head>
    <title>Optima System</title>
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
        default-src 'self' https://www.google.com;
        img-src * 'self' data: https:;
        base-uri 'self';
        object-src 'none';
        script-src 'self' 'unsafe-inline' 'unsafe-eval' https://www.gstatic.com https://www.google.com;
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
    <link href="{{ asset('assets/pages/auth/css/login.css?v=4') }}" rel="stylesheet" type="text/css" />
</head>
<!--end::Head-->
<!--begin::Body-->
<body class="bg-body">
    <div class="d-flex flex-column flex-root" id="login_page">
        <div class="d-flex flex-column flex-root">
            <div class="d-flex flex-column flex-lg-row flex-column-fluid container_custom">
                <div class="d-flex flex-column flex-lg-row-auto w-xl-900px w-lg-600px bg_custom">
                    <div class="d-flex flex-row-reverse float-start p-20">
                        <div class="w-300px logo_blue">
                            <img alt="Logo" src="{{ asset('assets/media/logos/optima-blue.png') }}" class="w-100">
                        </div>
                    </div>
                    <div class="d-flex flex-column position-fixed top-0 bottom-0 w-xxl-900px w-xl-800px w-100 wave">
                        <div class="d-flex flex-row-fluid flex-column ps-lg-10 ps-5 pe-5 ps-1200-10">
                            <div class="w-xl-400px w-lg-400px w-md-400px px-10 my-auto z-index-2">
                                <div class="text-start mb-5 mt-11rem" id="txt_welcome">
                                    <h1 class="welcome">WELCOME !</h1>
                                </div>
                                <div id="block_form_login">
                                    <form class="form w-100" novalidate="novalidate" id="form_login" autocomplete="off">
                                        <div class="fv-row mb-7">
                                            <div class="inner-addon left-addon right-addon">
                                                <span class="fa fa-envelope fs-4 mt-comma-15"></span>
                                                <input type="email" class="form-control" placeholder="Email" aria-label="Email" name="email" id="email">
                                            </div>
                                        </div>
                                        <div class="fv-row mb-5">
                                            <div class="inner-addon left-addon right-addon">
                                                <span class="fa fa-lock fs-4 mt-comma-15"></span>
                                                <input type="password" class="form-control" placeholder="Password" aria-label="Password" name="password" id="password" />
                                                <span toggle="#password" class="fa fa-eye fs-4 toggle-password"></span>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-stack mb-5">
                                            <div class="form-check form-check-sm form-check-custom ms-3">
                                                <input class="form-check-input custom-checkbox" type="checkbox" id="remember_me">
                                                <label class="form-check-label" id="txt_remember_me">Remember Me</label>
                                            </div>
                                            <a href="javascript:void(0)" class="link-primary fs-6 me-3 forgot_pass" name="forgot_password" id="forgot_password">Forgot Password ?</a>
                                        </div>

                                        <div class="text-center">
                                            <button type="button" id="btn_signin" class="btn btn-lg btn-primary w-100 mb-5">
                                                <span class="indicator-label fs-4">Verify Captcha</span>
                                                <span class="indicator-progress">Please wait...
                                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                                </span>
                                            </button>
                                        </div>
                                    </form>
                                </div>

                                <div id="block_form_profile" class="d-none mb-5">
                                    <select class="form-select form-select-sm" data-control="select2" name="user_profile_id" id="user_profile_id" data-placeholder="Select a Profile"  data-allow-clear="true">
                                        <option></option>
                                    </select>
                                </div>
                                <div class="fv-row" id="alert_login"></div>
                            </div>
                            <div class="d-flex flex-row-auto bgi-no-repeat bgi-position-x-start bgi-size-contain bgi-position-y-bottom min-h-50px min-h-lg-50px ms-10 z-index-1 mb-50px" style="background-image: url({{ asset('/assets/media/logos/skp_logo.png') }})"></div>
                        </div>
                    </div>
                </div>
                <div class="d-flex flex-column flex-lg-row-fluid py-10 pe-20 bg_custom_right">
                    <div class="d-flex flex-row-reverse float-end">
                        <div class="w-lg-250px">
                            <img alt="Logo" src="{{ asset('assets/media/logos/optima-white.png') }}" class="w-100" />
                        </div>
                    </div>
                    <div class="d-flex flex-row-reverse py-20 my-auto float-end">
                        <div class="w-lg-800px">
                            <img alt="Logo" src="{{ asset('assets/media/custom/imgright42.png') }}" class="w-100" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    @include('Auth::popup-forgot-password')
    @include('Auth::popup-change-password')

    <script src="{{ asset('assets/plugins/global/plugins.bundle.js') }}"></script>
    <script src="{{ asset('assets/js/scripts.bundle.js') }}"></script>
    <script src="{{ asset('assets/pages/auth/js/login.js?v=' . microtime()) }}"></script>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lem7GgcAAAAAFGYul6CBmkto4MrZHs0kIKc4NZa"></script>
</body>
</html>

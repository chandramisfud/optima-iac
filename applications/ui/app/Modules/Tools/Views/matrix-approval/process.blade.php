<!DOCTYPE html>
<html lang="en">
<!--begin::Head-->
<head>
    <title>Optima System - Process Update Matrix Approval</title>
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
    <link href="{{ asset('assets/pages/tools/matrix-approval/css/matrix-approval-process.css') }}" rel="stylesheet" type="text/css" />
</head>
<!--end::Head-->
<!--begin::Body-->
<body class="bg-body">
<div class="container-fluid">
    <div class="row">
        <div class="d-flex justify-content-center">
            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                <div class="app-sidebar-logo px-6 my-3">
                    <img alt="Logo" src="{{ asset('assets/media/logos/sidebar-top.png') }}" class="w-lg-25 w-md-400px w-300px app-sidebar-logo-default">
                </div>
                <div class="card shadow card_form">
                    <div class="card-body">
                        <div class="row mb-5">
                            <div class="col-lg-12 col-12">
                                <div class="d-flex flex-column align-items-center">
                                    <span class="fs-2qx text-gray-800">Updating Matrix Promo Approval</span>
                                    <span class="fs-1 text-gray-800">Please do not close or refresh this page until the process has finished</span>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-5">
                            <div class="col-lg-12 col-12">
                                <div class="text-center">
                                    <span class="spinner-border align-middle" id="matrix_loading"></span>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-5">
                            <div class="col-lg-12 col-12">
                                <div class="text-center">
                                    <span class="fs-1rem text-gray-800">Matrix Promo Approval (<span id="process_matrix">0</span>/<span id="tot_matrix">0</span>)</span>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-12 col-12" id="info_sending_progress">
                                <span class="text-gray-800 info_sending_email">Processing</span>
                            </div>
                        </div>
                        <div class="row mb-5">
                            <div class="col-lg-12 col-12">
                                <div class="d-flex flex-column w-100 me-2">
                                    <div class="progress h-15px w-100 bg-secondary">
                                        <div class="progress-bar bg-optima" id="progress_bar" role="progressbar" style="width: 0" aria-valuenow="5" aria-valuemin="0" aria-valuemax="100">
                                            <div class="d-flex flex-center">
                                                <span class="text-white fs-7 fw-bold text-center" id="text_progress">0%</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script src="{{ asset('assets/plugins/global/plugins.bundle.js') }}"></script>
<script src="{{ asset('assets/js/scripts.bundle.js') }}"></script>
<script src="{{ asset('assets/pages/tools/matrix-approval/js/matrix-approval-process.js') }}"></script>
</body>
</html>

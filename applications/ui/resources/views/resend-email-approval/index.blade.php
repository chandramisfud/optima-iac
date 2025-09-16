@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/css/log.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">List</small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-6 col-12">
            <div class="card shadow-sm card_form1">
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-12">
                            <span class="card-title text-gray-800 fs-4">Promo Cycle 1
                            </span>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-lg-6 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <div class="inner-addon left-addon right-addon">
                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_promo_cycle1_search" autocomplete="off">
                            </div>
                        </div>

                        <div class="col-lg-6 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <div class="text-end">
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_send_cycle1">
                                <span class="indicator-label">
                                    <span class="fa fa-paper-plane"></span> Send Email
                                    </span>
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                            </div>
                        </div>
                    </div>

                    <div class="row d-none" id="d_progress_cycle1">
                        <div class="col-md-12 col-12">
                            <div class="d-flex flex-column w-100 me-2">
                                <div class="progress h-10px w-100">
                                    <div class="progress-bar bg-optima" id="progress_bar_cycle1" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                        <div class="d-flex flex-center">
                                            <span class="text-white fs-7 fw-bold text-center" id="text_progress_cycle1">100%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_promo_cycle1" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-12">
            <div class="card shadow-sm card_form2">
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-12">
                            <span class="card-title text-gray-800 fs-4">Promo Cycle 2
                            </span>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-lg-6 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <div class="inner-addon left-addon right-addon">
                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_promo_cycle2_search" autocomplete="off">
                            </div>
                        </div>

                        <div class="col-lg-6 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <div class="text-end">
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_send_cycle2">
                                <span class="indicator-label">
                                    <span class="fa fa-paper-plane"></span> Send Email
                                    </span>
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                            </div>
                        </div>
                    </div>


                    <div class="row d-none" id="d_progress_cycle2">
                        <div class="col-md-12 col-12">
                            <div class="d-flex flex-column w-100 me-2">
                                <div class="progress h-10px w-100">
                                    <div class="progress-bar bg-optima" id="progress_bar_cycle2" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                        <div class="d-flex flex-center">
                                            <span class="text-white fs-7 fw-bold text-center" id="text_progress_cycle2">100%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_promo_cycle2" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/js/resend-email-approval.js?v=' . microtime()) }}"></script>
@endsection

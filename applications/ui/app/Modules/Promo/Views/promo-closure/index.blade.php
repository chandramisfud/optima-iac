@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet"
          type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-closure/css/promo-closure-methods.css') }}" rel="stylesheet"
          type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method">List</small>
    </span>
@endsection

@section('button-toolbar-left')
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-export-back-submit')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <div class="row mb-2">
                        <div class="col-lg-10 col-md-12 col-sm-12">
                            <div class="row">
                                <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-1 mb-2">
                                    <div class="inner-addon left-addon right-addon">
                                    <span
                                        class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6"
                                        style="padding-top: 32px">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                             viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2"
                                                  rx="1" transform="rotate(45 17.0365 15.1223)"
                                                  fill="currentColor"></rect>
                                            <path
                                                d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z"
                                                fill="currentColor"></path>
                                        </svg>
                                    </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search"
                                               value="" placeholder="Search" id="dt_promo_closure_search"
                                               autocomplete="off">
                                    </div>
                                </div>

                                <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-1 mb-2">
                                    <select class="form-select form-select-sm" data-control="select2"
                                            name="filter_distributor" id="filter_distributor"
                                            data-placeholder="Select a Distributor" data-allow-clear="true">
                                        <option></option>
                                    </select>
                                </div>

                                <div class="col-lg-6 col-md-12 col-sm-12 mb-lg-1 mb-2">
                                    <div class="row">
                                        <label class="col-lg-5 text-lg-end pt-1062-2 pe-lg-0 pt-lg-0">Activity
                                            Start</label>
                                        <div class="col-lg-7">
                                            <div class="input-group input-group-sm">
                                                <input type="text" class="form-control form-control-sm cursor-pointer"
                                                       id="filter_activity_start"
                                                       value="{{ @date('Y-m-d',strtotime(date('Y-01-01'))) }}"
                                                       autocomplete="off">
                                                <span class="input-group-text">to</span>
                                                <input type="text" class="form-control form-control-sm cursor-pointer"
                                                       id="filter_activity_end"
                                                       value="{{ @date('Y-m-d',strtotime(date('Y-12-31'))) }}"
                                                       autocomplete="off">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                    <select class="form-select form-select-sm" data-control="select2"
                                            name="filter_entity" id="filter_entity" data-placeholder="Select an Entity"
                                            data-allow-clear="true">
                                        <option></option>
                                    </select>
                                </div>

                                <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                    <select class="form-select form-select-sm" data-control="select2"
                                            name="filter_channel" id="filter_channel"
                                            data-placeholder="Select a Channel" data-allow-clear="true">
                                        <option></option>
                                    </select>
                                </div>

                                <div class="col-lg-6 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                    <div class="row">
                                        <label class="col-lg-5 text-lg-end pt-1062-2 pe-lg-0 pt-lg-0">Balance (Investment - DN Paid)</label>
                                        <div class="col-lg-7">
                                            <select class="form-select form-select-sm" data-control="select2"
                                                    name="remaining_budget" id="remaining_budget" data-placeholder="Select an Entity"
                                                    title="(Investment - DN Paid)" data-allow-clear="true">
                                                <option value="ALL">All</option>
                                                <option value="_0To10M">0 - 10 mio IDR</option>
                                                <option value="_10To100M">10 mio - 100 mio IDR</option>
                                                <option value="_100To500M">100 mio - 500 mio IDR</option>
                                                <option value="_500To1B">500 mio - 1 bio IDR</option>
                                                <option value="Above1B">above 1 bio IDR</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <div class="col-lg-2 col-md-12 col-sm-12 mb-lg-1 mb-2">
                            <div class="row mb-lg-1">
                                <div class="text-end">
                                    <button type="button"
                                            class="btn btn-sm btn-outline-optima w-lg-auto w-100 mb-lg-0 mb-2"
                                            id="dt_promo_closure_view">
                                        <span class="indicator-label">
                                            <span class="fa fa-search"></span> View
                                        </span>
                                        <span class="indicator-progress">
                                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                        </span>
                                    </button>
                                    <a href="javascript:void(0)" class="btn btn-sm btn-icon me-2" id="btn_download_template" title="Download Template Excel">
                                        <i class="fa fa-download" style="font-size: 1.8rem; color: black"></i>
                                    </a>
                                    <a href="javascript:void(0)" class="btn btn-sm btn-icon me-2" id="btn-upload" title="Upload Excel">
                                        <i class="fa fa-cloud-upload-alt" style="font-size: 2.0rem; color: black"></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row d-none" id="row_upload">
                        <form id="form_budget" class="form" autocomplete="off">
                            @csrf
                            <div class="row fv-row">
                                <div class="input-group input-group-sm">
                                    <input class="form-control form-control-sm field_upload" id="file" name="file"
                                           type="file" data-stripe="file" placeholder="Choose File"/>
                                    <span class="input-group-text">Upload File</span>
                                </div>
                            </div>
                            <div class="separator my-2 border-2"></div>
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-6 col-6 mb-2">
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6 col-6">
                                    <div class="text-end">
                                        <button type="button" class="btn btn-sm btn-optima fw-bolder" id="btn_upload">
                                            <span class="indicator-label">
                                                <span class="bi bi-upload"></span> Upload File
                                            </span>
                                            <span class="indicator-progress">Uploading...
                                                <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                            </span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>

                    <div class="row d-none" id="d_progress">
                        <div class="col-md-12 col-12">
                            <div class="d-flex flex-column w-100 me-2">
                                <div class="progress h-10px w-100">
                                    <div class="progress-bar bg-optima" id="progress_bar" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                        <div class="d-flex flex-center">
                                            <span class="text-white fs-7 fw-bold text-center" id="text_progress">100%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_promo_closure"
                                   class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script
        src="{{ asset('assets/pages/promo/promo-closure/js/promo-closure-methods.js?v=9') }}"></script>
@endsection


@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/tools/budget-adjustment/css/budget-adjustment-methods.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row card_form mb-3">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm">
                <div class="card-body">
                    <form id="form_budget_adjustment" class="form" autocomplete="off">
                        @csrf
                        <div class="row mb-3">
                            <div class="d-flex flex-lg-row flex-column justify-content-between">
                                <div class="min-w-lg-70px mw-lg-70px w-100 mb-3">
                                    <label class="pt-lg-2">Period</label>
                                </div>

                                <div class="min-w-lg-100px mw-lg-150px w-100 mx-lg-5 mb-3">
                                    <div class="input-group input-group-sm" id="dialer_period">
                                        <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                            <i class="fa fa-minus fs-2"></i>
                                        </button>

                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="filter_period" id="filter_period" value="{{ @date('Y') }}" autocomplete="off" maxlength="4"/>

                                        <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                            <i class="fa fa-plus fs-2"></i>
                                        </button>
                                    </div>
                                </div>

                                <div class="min-w-lg-90px w-lg-100 mx-lg-5 mb-3">
                                    <div class="fv-row">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_entity" id="filter_entity" data-placeholder="Select an Entity"  data-allow-clear="true">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>

                                <div class="min-w-lg-90px w-lg-100 ms-lg-5 mb-3">
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_budget" id="filter_budget" data-placeholder="Select budget master name"  data-allow-clear="true">
                                        <option></option>
                                    </select>
                                </div>

                                <div class="min-w-lg-175px ms-lg-5 mb-3">
                                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-2 w-100" id="btn_download">
                                        <span class="indicator-label">
                                            <span class="fa fa-cloud-download-alt"></span> Download Template
                                        </span>
                                        <span class="indicator-progress">
                                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                        </span>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="d-flex flex-lg-row flex-column justify-content-between">
                                <div class="w-100 mb-3">
                                    <div class="fv-row">
                                        <div class="input-group input-group-sm">
                                            <input class="form-control form-control-sm field_upload" id="file" name="file" type="file" data-stripe="file" placeholder="Choose File"/>
                                            <span class="input-group-text input-group-sm">Upload File</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="min-w-lg-175px ms-lg-5 mb-3">
                                    <button type="button" class="btn btn-sm btn-outline-optima w-100" id="btn_upload">
                                        <span class="indicator-label">
                                            <span class="fa fa-cloud-upload-alt"></span> Upload
                                        </span>
                                        <span class="indicator-progress">
                                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                        </span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <div class="row d-none" id="card_budget_adjustment_issue">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <div class="row">
                                <div class="card-title">
                                     <span class="d-flex align-items-center fs-3 my-1">Budget adjustment issues
                                        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
                                        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">List</small>
                                    </span>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                    <table id="dt_budget_adjustment_issue" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/tools/budget-adjustment/js/budget-adjustment-methods.js?v=1') }}"></script>
@endsection

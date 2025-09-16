@extends('layouts/layoutMaster')

@section('title', 'Transfer To SAP - Payment')

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/tools/sap-payment/css/sap-payment-upload.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method"></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-7">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_sap_payment" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <div class="input-group mb-5">
                                <input class="form-control field_upload" id="file" name="file" type="file" data-stripe="file" placeholder="Choose File"/>
                                <span class="input-group-text">Upload File</span>
                            </div>
                        </div>
                        <div class="separator my-2 border-2 mb-8"></div>
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-12 mb-2">
                                <select class="form-select form-select-sm" data-control="select2" name="filter_entity" id="filter_entity" data-placeholder="Select a Entity"  data-allow-clear="true" tabindex="1">
                                    <option></option>
                                </select>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-12 mb-2">
                                <select class="form-select form-select-sm" data-control="select2" name="filter_distributor" id="filter_distributor" data-placeholder="Select a Distributor"  data-allow-clear="true" multiple="multiple" tabindex="2"></select>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-12 mb-2">
                                <button type="button" class="btn btn-sm btn-optima" id="btn_save_action" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-kt-menu-offset="30px, 0px">
                                <span class="indicator-label">
                                    <i class="la la-check text-white"></i>
                                    Generate
                                    <i class="la la-angle-down fs-8 text-white rotate-180 ms-3"></i>
                                </span>
                                                            <span class="indicator-progress">Generating...
                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                </span>
                                </button>
                                <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-150px" data-kt-menu="true">
                                    <div class="menu-item px-3 mt-3">
                                        <a href="javascript:void(0)" class="menu-link px-3 fw-bold text-gray-700" id="btn_generate_xml">
                                            <i class="bi bi-download me-3 fs-4"></i>
                                            XML
                                        </a>
                                    </div>
                                    <div class="menu-item px-3 mb-3">
                                        <a href="javascript:void(0)" class="menu-link px-3" id="btn_generate_xml_batch_name">
                                            <i class="bi bi-download me-3 fs-4"></i>
                                            Batch Name
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-12 mb-2">
                                <div class="col-lg-6 col-lg-12 col-12" align="right">
                                    <button type="button" class="btn btn-sm btn-optima fw-bolder" id="btn_upload">
                                        <span class="indicator-label">
                                            <span class="fa fa-cloud-upload-alt"></span> Upload
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
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm">
                <div class="card-body">
                    <div class="row">
                        <span class="text-gray-700 fs-4 mb-3">Upload History<small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method"></small></span>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-12">
                            <table id="dt_upload_history" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/js/generate-uuid.js') }}"></script>
    <script src="{{ asset('assets/pages/tools/sap-payment/js/sap-payment-upload.js?v=5') }}"></script>
@endsection

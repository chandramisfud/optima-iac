@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/css/log.css') }}" rel="stylesheet" type="text/css"/>
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
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <div class="row">
                                <div class="position-relative w-lg-300px w-fhd-300px mb-0">
                                    <div class="inner-addon left-addon right-addon">
                                        <div class="input-group input-group-sm mb-3">
                                            <span class="input-group-text px-2 fs-12px">Log Date</span>
                                            <input type="text" class="form-control form-control-sm" name="filter_date" id="filter_date" placeholder="All" value="" autocomplete="off">
                                            <span class="la la-close fs-4" id="btn_clear_date"></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="position-relative w-lg-100px w-fhd-100px ps-lg-0 float-lg-end ms-lg-3 mb-2 mb-lg-0">
                                    <button type="button" class="btn btn-sm btn-outline-optima w-100" id="dt_log_view">
                                <span class="indicator-label">
                                    <span class="fa fa-search"></span> View
                                </span>
                                        <span class="indicator-progress">
                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                </span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_log" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/js/log.js') }}"></script>
@endsection

@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-planning/css/promo-planning-methods.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method">Approval</small>
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
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form mb-3">
                <div class="card-body">
                    <form id="form_approval" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <div class="input-group input-group-sm">
                                <input class="form-control form-control-sm field_upload" id="file" name="file" type="file" data-stripe="file" placeholder="Choose File"/>
                                <span class="input-group-text">Upload File</span>
                            </div>
                        </div>
                        <div class="separator my-2 border-2"></div>
                        <div class="row">
                            <div class="col-lg-7 col-md-12 col-12 mb-lg-0 mb-2 text-start">
                                <div class="row">
                                    <div class="position-relative w-lg-175px w-fhd-250px mb-2">
                                        <button type="button" class="btn btn-sm btn-outline-optima fw-bolder w-lg-auto w-100" id="btn_download_template">
                                            <span class="fa fa-cloud-download-alt"></span> Download Template
                                        </button>
                                    </div>

                                    <label class="position-relative w-lg-50px text-lg-end px-lg-0 pt-lg-2">Period</label>

                                    <div class="position-relative w-lg-125px w-fhd-150px ps-lg-0 ms-lg-2 mb-2">
                                        <div class="input-group input-group-sm" data-kt-dialer="true" data-kt-dialer-min="2015" data-kt-dialer-step="1">
                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                <i class="fa fa-minus fs-2"></i>
                                            </button>

                                            <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="filter_period" id="filter_period" value="{{ @date('Y') }}" autocomplete="off"/>

                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                <i class="fa fa-plus fs-2"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-5 col-md-12 col-12 mb-lg-0 mb-2 text-end">
                                <button type="button" class="btn btn-sm btn-outline-optima fw-bolder w-lg-auto w-100" id="btn_upload">
                                    <span class="indicator-label">
                                        <span class="fa fa-cloud-upload-alt"></span> Upload
                                    </span>
                                    <span class="indicator-progress">Uploading...
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>

            <div class="card shadow-sm d-none" id="card_result_approved">
                <div class="card-body">
                    <div class="row">
                        <div class="col-12">
                            <span class="fw-bold fs-4 me-2">Approval Promo Planning</span>
                            <span class="text-gray-700 fs-5">List</span>
                        </div>
                        <div class="separator my-2 border-2"></div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <table id="dt_promo_planning_approval" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/pages/promo/promo-planning-approval/js/promo-planning-approval-methods.js?v=1') }}"></script>
@endsection

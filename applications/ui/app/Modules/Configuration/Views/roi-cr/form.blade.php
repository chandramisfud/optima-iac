@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/configuration/roi-cr/css/roi-cr-form.css') }}"  rel="stylesheet" type="text/css" />
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Configuration</small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-save-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_config_roi_cr" class="form" autocomplete="off">
                        @csrf
                        <div class="row">
                            <div class="col-lg-6 col-md-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-5 col-form-label required">Minimum ROI (%)</label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm text-end" type="text" name="minimumROI" id="minimumROI" placeholder="Enter Min ROI" autocomplete="off" tabindex="1" />
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-5 col-form-label required">Maximum ROI (%)</label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm text-end" type="text" name="maksimumROI" id="maksimumROI" placeholder="Enter Max ROI" autocomplete="off" tabindex="2" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-5 col-form-label required">Minimum Cost Ratio (%)</label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm text-end" type="text" name="minimumCostRatio" id="minimumCostRatio"  placeholder="Enter Min Cost Ratio" autocomplete="off" tabindex="3" />
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-5 col-form-label required">Maximum Cost Ratio (%)</label>
                                    <div class="col-lg-6">
                                            <input class="form-control form-control-sm text-end" type="text" name="maksimumCostRatio" id="maksimumCostRatio" placeholder="Enter Max Cost Ratio" autocomplete="off" tabindex="4" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                    <div class="separator my-2 border-2"></div>
                    <div class="row">
                        <span class="text-gray-700 fs-4 mb-3">Configuration Details</span>
                    </div>
                    <div class="row">
                        <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <div class="inner-addon left-addon right-addon">
                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_roi_cr_search" autocomplete="off">
                            </div>
                        </div>

                        <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <select class="form-select form-select-sm" data-control="select2" name="filter_category" id="filter_category" data-placeholder="Select a Category"  data-allow-clear="true">
                                <option></option>
                            </select>
                        </div>

                        <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <select class="form-select form-select-sm" data-control="select2" name="filter_sub_category" id="filter_sub_category" data-placeholder="Select a Sub Category"  data-allow-clear="true">
                                <option></option>
                            </select>
                        </div>

                        <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <select class="form-select form-select-sm" data-control="select2" name="filter_activity" id="filter_activity" data-placeholder="Select an Activity"  data-allow-clear="true">
                                <option></option>
                            </select>
                        </div>

                        <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <div class="text-end">
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="dt_roi_cr_view">
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
{{--                    <div class="row">--}}
{{--                        <div class="col-lg-2 col-md-2 col-sm-12 mb-2">--}}
{{--                            <div class="position-relative w-100 me-md-2">--}}
{{--                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-50 translate-middle ms-6">--}}
{{--                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">--}}
{{--                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>--}}
{{--                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>--}}
{{--                                    </svg>--}}
{{--                                </span>--}}
{{--                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_roi_cr_search">--}}
{{--                            </div>--}}
{{--                        </div>--}}
{{--                        <div class="col-lg-3 col-md-3 col-sm-12 mb-2">--}}
{{--                            <select class="form-select form-select-sm" data-control="select2" name="filter_category" id="filter_category" data-placeholder="Select a Category"  data-allow-clear="true">--}}
{{--                                <option></option>--}}
{{--                            </select>--}}
{{--                        </div>--}}
{{--                        <div class="col-lg-3 col-md-3 col-sm-12 mb-2">--}}
{{--                            <select class="form-select form-select-sm" data-control="select2" name="filter_sub_category" id="filter_sub_category" data-placeholder="Select a Sub Category"  data-allow-clear="true">--}}
{{--                                <option></option>--}}
{{--                            </select>--}}
{{--                        </div>--}}
{{--                        <div class="col-lg-3 col-md-3 col-sm-12 mb-2">--}}
{{--                            <select class="form-select form-select-sm" data-control="select2" name="filter_activity" id="filter_activity" data-placeholder="Select an Activity"  data-allow-clear="true">--}}
{{--                                <option></option>--}}
{{--                            </select>--}}
{{--                        </div>--}}
{{--                        <div class="col-lg-1 col-md-1 col-sm-12 mb-2">--}}
{{--                            <button type="button" class="btn btn-sm btn-outline-optima  w-100" id="dt_roi_cr_view">--}}
{{--                                <span class="indicator-label">--}}
{{--                                    <span class="fa fa-search"></span> View--}}
{{--                                </span>--}}
{{--                                <span class="indicator-progress">--}}
{{--                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>--}}
{{--                                </span>--}}
{{--                            </button>--}}
{{--                        </div>--}}
{{--                    </div>--}}
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_roi_cr" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/configuration/roi-cr/js/roi-cr-form.js?v=1') }}"></script>
@endsection

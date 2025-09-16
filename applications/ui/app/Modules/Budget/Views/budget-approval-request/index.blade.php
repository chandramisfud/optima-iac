@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')

@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">List</small>
    </span>
@endsection

@section('button-toolbar-right')
    <div class="d-none" id="btn_send">
        <button type="button" class="btn btn-sm btn-outline-optima me-1 text-nowrap" id="btn_send_email" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-kt-menu-offset="30px, 0px">
            Send Email Request
            <span class="indicator-progress">
                <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
            </span>
        </button>
        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-150px" data-kt-menu="true">
            <div class="menu-item px-3 mt-3">
                <a href="javascript:void(0)" class="menu-link px-3 fw-bold text-gray-700 btn_send_email" id="btn_send_email_rc">
                    Retailer Cost
                </a>
            </div>
            <div class="menu-item px-3 mb-3">
                <a href="javascript:void(0)" class="menu-link px-3 fw-bold text-gray-700 btn_send_email" id="btn_send_email_dc">
                    Distributor Cost
                </a>
            </div>
        </div>
    </div>
    <button type="button" class="btn btn-sm btn-outline-optima text-hover-white mx-1" title="Download PDF" id="btn_pdf">
        PDF
    </button>
    <button type="button" class="btn btn-sm btn-outline-optima text-hover-white" title="Download" id="btn_export_excel">
        <span class="bi bi-download"></span>
    </button>

    <button type="button" class="btn btn-sm btn-clean" id="btn_back">
        <span class="fa fa-arrow-left"></span> Back
    </button>
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
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="row">
                                <div class="d-flex justify-content-between flex-lg-row flex-column">
                                    <div class="col-lg-2 col-md-12 col-sm-12 px-lg-1 mb-lg-0 mb-2">
                                        <div class="row">
                                            <label class="col-lg-3 text-lg-end px-lg-0 pt-lg-2">Period</label>
                                            <div class="col-lg-9">
                                                <div class="input-group input-group-sm" id="dialer_period">
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="filter_period" id="filter_period" value="" autocomplete="off"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="flex-grow-1 mx-lg-1 mb-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_month" id="filter_month" data-placeholder="Select a Month"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                        </select>
                                    </div>

                                    <div class="flex-grow-1 mx-lg-1 mb-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_category" id="filter_category" data-placeholder="Select a Category"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                        </select>
                                    </div>

                                    <div class="flex-grow-1 mx-lg-1 mb-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_group_brand" id="filter_group_brand" data-placeholder="Select a Brand" multiple="multiple" data-allow-clear="true">
                                        </select>
                                    </div>

                                    <div class="flex-grow-1 mx-lg-1 mb-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_channel" id="filter_channel" data-placeholder="Select a Channel"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                        </select>
                                    </div>

                                    <div class="flex-grow-1 mx-lg-1 mb-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_five_bio" id="filter_five_bio" data-placeholder="Above/Below 5 Bio"  data-allow-clear="true" autocomplete="off">
                                            <option value=""></option>
                                            <option value="1">Above 5 Bio</option>
                                            <option value="0">Below 5 Bio</option>
                                        </select>
                                    </div>

                                    <div class="flex-grow-1 mx-lg-1 mb-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_status" id="filter_status" data-placeholder="Select a Status"  multiple="multiple" data-allow-clear="true" autocomplete="off">
                                        </select>
                                    </div>
                                    <div class="flex-grow-1">
                                        <div class="text-end">
                                            <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100 text-nowrap mb-2 mb-lg-0" id="btn_view">
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
                        </div>
                    </div>
                    <form>@csrf</form>
                    <div class="row">
                        <div class="col-lg-4 col-12">
                            <div class="row">
                                <div class="col-lg-6 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                    <div class="inner-addon left-addon right-addon">
                                            <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                    <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                    <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                </svg>
                                            </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_budget_approval_request_summary_search" autocomplete="off">
                                    </div>
                                </div>
                            </div>
                            <table id="dt_budget_approval_request_summary" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                        <div class="col-lg-8 col-12">
                            <div class="row">
                                <div class="col-lg-4 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                    <div class="inner-addon left-addon right-addon">
                                            <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                    <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                    <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                </svg>
                                            </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_budget_approval_request_detail_search" autocomplete="off">
                                    </div>
                                </div>
                            </div>
                            <table id="dt_budget_approval_request_detail" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    @include('Budget::budget-approval-request.modal-print-pdf')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-approval-request/js/budget-approval-request-methods.js?v=11') }}"></script>
@endsection

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
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-export-back')
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
                        <div class="d-flex justify-content-between flex-lg-row flex-column">
                            <div class="row w-lg-100">
                                <div class="col-lg-2 col-12">
                                    <div class="inner-addon left-addon right-addon">
                                        <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                            </svg>
                                        </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_summary_budgeting_search" autocomplete="off">
                                    </div>
                                </div>

                                <div class="col-lg-2 col-12">
                                    <div class="row">
                                        <label class="col-lg-3 text-lg-end px-lg-0 pt-lg-2">Period</label>
                                        <div class="col-lg-9">
                                            <div class="input-group input-group-sm" id="dialer_period">
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

                                <div class="col-lg-2 col-12">
                                    <div class="mx-lg-1 mb-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_category" id="filter_category" data-placeholder="Select a Category"  data-allow-clear="true" autocomplete="off">
                                        </select>
                                    </div>
                                </div>

                                <div class="col-lg-2 col-12">
                                    <div class="mx-lg-1 mb-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_group_brand" id="filter_group_brand" data-placeholder="Select a Brand"  data-allow-clear="true" autocomplete="off" multiple>
                                        </select>
                                    </div>
                                </div>

                                <div class="col-lg-2 col-12">
                                    <div class="mx-lg-1 mb-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_channel" id="filter_channel" data-placeholder="Select a Channel"  data-allow-clear="true" autocomplete="off" multiple>
                                        </select>
                                    </div>
                                </div>
                            </div>

                            <div class="ms-5">
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto text-nowrap w-100 mb-2" id="dt_summary_budgeting_view">
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
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_summary_budgeting" class="table table-striped table-row-bordered table-responsive table-sm table-hover">
                                <thead>
                                <tr>
                                    <th rowspan="2" style="background-color: #5867dd;">Brand</th>
                                    <th rowspan="2" style="background-color: #5867dd;">Channel</th>
                                    <th rowspan="2" style="background-color: #5867dd;">Sub Channel</th>
                                    <th rowspan="2" style="background-color: #5867dd;">Account</th>
                                    <th rowspan="2" style="background-color: #5867dd;">Sub Account</th>
                                    <th colspan="4" style="background-color: #5867dd; vertical-align: middle; text-align: center;"></th>
                                    <th colspan="10" style="background-color: #5867dd; vertical-align: middle; text-align: center;">Distributor</th>
                                    <th colspan="9" style="background-color: #5867dd; vertical-align: middle; text-align: center;">Retailer</th>
                                    <th colspan="2" style="background-color: #5867dd; vertical-align: middle; text-align: center;">Trade Spend Deployed</th>
                                    <th colspan="2" style="background-color: #5867dd; vertical-align: middle; text-align: center;">Warchest</th>
                                    <th colspan="2" style="background-color: #5867dd; vertical-align: middle; text-align: center;">Total TS (Deployed + Warchest)</th>
                                </tr>
                                <tr>
                                    <th>SS Volume (tons)</th>
                                    <th>PS Volume (tons)</th>
                                    <th>SS</th>
                                    <th>PS</th>
                                    <th>KPI</th>
                                    <th>KPI %</th>
                                    <th>RGA</th>
                                    <th>RGA %</th>
                                    <th>Transport</th>
                                    <th>Transport %</th>
                                    <th>Other Cost</th>
                                    <th>Other Cost (%)</th>
                                    <th>Total Distributor Cost</th>
                                    <th>Total Distributor Cost (%)</th>
                                    <th>TT</th>
                                    <th>% TT to SS</th>
                                    <th>% TT to PS</th>
                                    <th>Adhoc</th>
                                    <th>% Adhoc to SS</th>
                                    <th>% Adhoc to PS</th>
                                    <th>TT + Adhoc</th>
                                    <th>% TT + Adhoc to SS</th>
                                    <th>% TT + Adhoc to PS</th>
                                    <th>Total Trade Spend</th>
                                    <th>% to PS</th>
                                    <th>Warchest</th>
                                    <th>% to PS</th>
                                    <th>Total TS</th>
                                    <th>% to PS</th>
                                </tr>
                                </thead>
                            </table>
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
    <script src="{{ asset('assets/pages/finance-report/summary-budgeting/js/summary-budgeting-methods.js?v=4') }}"></script>
@endsection

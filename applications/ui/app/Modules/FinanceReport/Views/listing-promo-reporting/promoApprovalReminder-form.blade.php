@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/finance-report/listing-promo-reporting/css/listing-promo-reporting-methods.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"><a class="text-optima gapnotif_promo_count" href="javascript:void(0)" data-toggle="tooltip" title="Click here to see the variance">Gap of Promo Id = <span id="gapnotif_promo_count">0</span></a> <a class="text-optima gapnotif_promo_investment" href="javascript:void(0)" data-toggle="tooltip" title="Click here to see the variance">, Gap of Investment = <span id="gapnotif_promo_investment">0</span></a></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-config-send-email')
    <button type="button" class="btn btn-sm btn-outline-optima text-hover-white" title="Download" id="btn_export_excel_promo_approval_reminder">
        <span class="bi bi-download"></span>
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
                    <form id="form_promo_approval" class="form" autocomplete="off">
                        @csrf
                        <div class="row">
                            <div class="col-lg-2 col-md-12 col-sm-12 px-lg-1 mb-lg-0 mb-2">
                                <div class="row">
                                    <label class="col-lg-3 text-lg-end px-lg-0 pt-lg-2">Period</label>
                                    <div class="col-lg-9">
                                        <div class="input-group input-group-sm" id="dialer_period">
                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                <i class="fa fa-minus fs-2"></i>
                                            </button>

                                            <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="filter_year" id="filter_year" value="{{ @date('Y') }}" autocomplete="off"/>

                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                <i class="fa fa-plus fs-2"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-0 mb-2 px-lg-5">
                                <div class="row">
                                    <label class="col-lg-4 text-lg-end px-lg-0 pt-lg-2">Month Start</label>
                                    <div class="col-lg-8">
                                        <select class="form-select form-select-sm" data-control="select2" name="filter_month_start" id="filter_month_start" data-placeholder="Select a Month"  data-allow-clear="false">
                                            <option value="1">January</option>
                                            <option value="2">February</option>
                                            <option value="3">March</option>
                                            <option value="4">April</option>
                                            <option value="5">May</option>
                                            <option value="6">June</option>
                                            <option value="7">July</option>
                                            <option value="8">August</option>
                                            <option value="9">September</option>
                                            <option value="10">October</option>
                                            <option value="11">November</option>
                                            <option value="12">December</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-1 col-md-12 col-sm-12 mb-lg-0 mb-2 px-lg-5 pt-3 text-center">
                                <span class="align-center">Or</span>
                            </div>
                            <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-0 mb-2 px-lg-5 fv-row">
                                <select class="form-select form-select-sm" data-control="select2" name="filter_month_end" id="filter_month_end" data-placeholder="Select a Month"  data-allow-clear="false">
                                    <option value="1">January</option>
                                    <option value="2">February</option>
                                    <option value="3">March</option>
                                    <option value="4">April</option>
                                    <option value="5">May</option>
                                    <option value="6">June</option>
                                    <option value="7">July</option>
                                    <option value="8">August</option>
                                    <option value="9">September</option>
                                    <option value="10">October</option>
                                    <option value="11">November</option>
                                    <option value="12">December</option>
                                </select>
                            </div>

                            <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                <div class="text-end">
                                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="dt_promo_approval_reminder_view">
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
                    </form>
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_promo_approval_reminder" class="table table-striped table-row-bordered table-responsive table-sm table-hover">
                                <thead>
                                <tr>
                                    <th rowspan="3" class="text-center align-middle">Channel Head</th>
                                    <th rowspan="3" class="text-center align-middle">Channel</th>
                                    <th rowspan="3" class="text-center align-middle">Status Group</th>
                                    <th rowspan="3" class="text-center align-middle">PIC</th>
                                    <th rowspan="3" class="text-center align-middle">Pending Action</th>
                                    <th id="month1" colspan="4" class="text-center">Month1</th>
                                    <th id="month2" colspan="4" class="text-center">Month2</th>
                                    <th rowspan="3" class="text-center align-middle">Total Count of Promo ID</th>
                                    <th rowspan="3" class="text-center align-middle">Total Sum of Investment</th>
                                </tr>
                                <tr>
                                    <th colspan="2" class="text-center">Biweekly 1 (Start 1-15)</th>
                                    <th colspan="2" class="text-center">Biweekly 2 (Start 16-31)</th>
                                    <th colspan="2" class="text-center">Biweekly 1 (Start 1-15)</th>
                                    <th colspan="2" class="text-center">Biweekly 2 (Start 16-31)</th>
                                </tr>
                                <tr>
                                    <th class="text-center">Count of Promo ID</th>
                                    <th class="text-center">Sum of Investment</th>
                                    <th class="text-center">Count of Promo ID</th>
                                    <th class="text-center">Sum of Investment</th>
                                    <th class="text-center">Count of Promo ID</th>
                                    <th class="text-center">Sum of Investment</th>
                                    <th class="text-center">Count of Promo ID</th>
                                    <th class="text-center">Sum of Investment</th>
                                </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    @include('FinanceReport::listing-promo-reporting.send-email-report')
    @include('FinanceReport::listing-promo-reporting.send-email-auto-config')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/finance-report/listing-promo-reporting/js/promo-approval-reminder.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/finance-report/listing-promo-reporting/js/send-email.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/finance-report/listing-promo-reporting/js/send-email-auto-config.js?v=' . microtime()) }}"></script>
@endsection

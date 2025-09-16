@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/tools/blitz-rawdata/css/blitz-rawdata-methods.css?v=' . microtime()) }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-actual-sales-raw-baseline-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="position-lg-sticky sticky-lg-top" style="z-index: 1">
        <div class="row mb-2 card_form">
            <div class="col-md-12 col-12">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="row mb-2">
                            <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                <div class="inner-addon left-addon right-addon">
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_promo" id="filter_promo" autocomplete="off" data-placeholder="Select a promo"  data-allow-clear="true">
                                        <option value="0">Promo ID</option>
                                        <option value="1">Promo Planning</option>
                                    </select>
                                </div>
                            </div>

                            <div class="col-lg-5 col-md-12 col-sm-12 mb-lg-0 mb-2 px-lg-1">
                                <div class="row px-lg-1">
                                    <label class="col-lg-4 text-lg-end px-lg-1 pt-1326-2" id="txt_type_promo">Promo ID</label>
                                    <div class="col-lg-8 px-lg-1">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="promorefid" id="promorefid" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 col-md-12 col-sm-12 mb-lg-0 mb-2 px-lg-1">
                                <div class="row px-lg-1">
                                    <label class="col-lg-4 text-lg-end px-lg-1 pt-1326-2 ">Planning Creation</label>
                                    <div class="col-lg-8 px-lg-1">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="planningcreationdate" id="planningcreationdate" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                <div class="inner-addon left-addon right-addon">
                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                    <input type="text" class="form-control form-control-sm ps-10" name="refid" value="" placeholder="Search" id="refid" autocomplete="off">
                                </div>
                            </div>

                            <div class="col-lg-5 col-md-12 col-sm-12 mb-lg-0 mb-2 px-lg-1">
                                <div class="row px-lg-1">
                                    <label class="col-lg-4 text-lg-end px-lg-1 pt-1326-2 ">Period</label>
                                    <div class="col-lg-8 px-lg-1">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="period" id="period" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 col-md-12 col-sm-12 mb-lg-0 mb-2 px-lg-1">
                                <div class="row px-lg-1">
                                    <label class="col-lg-4 text-lg-end px-lg-1 pt-1326-2 ">Promo Period</label>
                                    <div class="col-lg-8 px-lg-1">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="promoperiod" id="promoperiod" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row mb-2">
        <div class="col-lg-8 col-12">
            <div class="row mb-2 mb-lg-0">
                <div class="col-md-12 col-12">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <div class="row">
                                <div class="card-title">
                                    <h5>Promo ID Creation Information</h5>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                    <table id="dt_promo_creation_info" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-4 col-12">
            <div class="row mb-2">
                <div class="col-md-12 col-12">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <div class="row">
                                <div class="card-title">
                                    <h5 style="">Baseline Calculation</h5>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                    <table id="dt_baseline_calculation" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-12">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-lg-12 col-12">
                                    <div class="row">
                                        <div class="card-title">
                                            <h5>Result</h5>
                                        </div>
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                            <table id="dt_result_raw" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <div class="row">
                                <div class="card-title">
                                    <h5>Sales Calculation</h5>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                    <table id="dt_sales_calculation" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/pages/tools/blitz-rawdata/js/blitz-rawdata-methods.js?v=' . microtime()) }}"></script>
@endsection

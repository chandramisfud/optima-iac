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
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method">Add New</small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-save-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <form class="form" id="formConfiguration" autocomplete="off">
        @csrf
        <div class="row">
            <div class="col-lg-12 col-12 pe-lg-1">
                <div class="row mb-2">
                    <div class="col-12">
                        <div class="card shadow-sm card_main_activity">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-12">
                                        <div class="row fv-row">
                                            <label class="col-lg-1 col-md-2 col-sm-12 col-12 col-form-label required" for="mainActivity">Main Activity</label>
                                            <div class="col-lg-11 col-md-10 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm" name="mainActivity" id="mainActivity" autocomplete="off"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-12">
                        <div class="card shadow-sm card_sub_activity_coverage">
                            <div class="card-body">
                                <div class="row mb-5">
                                    <div class="col-12">
                                        <span class="fs-3">Sub Activity Coverage</span>
                                        <div class="separator border-2 my-2 border-secondary"></div>
                                    </div>
                                </div>
                                <div class="row mb-5">
                                    <div class="d-flex justify-content-between flex-lg-row flex-column">
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                            <div class="row me-lg-5">
                                                <div class="d-flex justify-content-between flex-lg-row flex-column">
                                                    <div class="w-100 align-self-end mb-sm-2">
                                                        <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                            <input type="text" class="form-control form-control-sm ps-10" value="" placeholder="Search" id="dt_sub_activity_coverage_search" autocomplete="off">
                                                            <label class="d-none" for="dt_sub_activity_coverage_search"></label>
                                                        </div>
                                                    </div>
                                                    <div class="w-100 mx-lg-2 mb-sm-2">
                                                        <select class="form-select form-select-sm" data-control="select2" id="dt_sub_activity_coverage_category" data-placeholder="Select a Category"  data-allow-clear="true" autofocus>
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                    <div class="w-100 mx-lg-2 mb-sm-2">
                                                        <select class="form-select form-select-sm" data-control="select2" id="dt_sub_activity_coverage_sub_category" data-placeholder="Select a Sub Category"  data-allow-clear="true" autofocus>
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                    <div class="w-100">
                                                        <select class="form-select form-select-sm" data-control="select2" id="dt_sub_activity_coverage_activity" data-placeholder="Select an Activity"  data-allow-clear="true" autofocus>
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <table id="dt_sub_activity_coverage" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                            <div class="row ms-lg-5">
                                                <div class="d-flex justify-content-between flex-lg-row flex-column">
                                                    <div class="w-100 align-self-end mb-sm-2">
                                                        <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                            <input type="text" class="form-control form-control-sm ps-10" value="" placeholder="Search" id="dt_sub_activity_selected_search" autocomplete="off">
                                                            <label class="d-none" for="dt_sub_activity_selected_search"></label>
                                                        </div>
                                                    </div>
                                                    <div class="w-100 mx-lg-2 mb-sm-2">
                                                        <select class="form-select form-select-sm" data-control="select2" id="dt_sub_activity_selected_category" data-placeholder="Select a Category"  data-allow-clear="true" autofocus>
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                    <div class="w-100 mx-lg-2 mb-sm-2">
                                                        <select class="form-select form-select-sm" data-control="select2" id="dt_sub_activity_selected_sub_category" data-placeholder="Select a Sub Category"  data-allow-clear="true" autofocus>
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                    <div class="w-100">
                                                        <select class="form-select form-select-sm" data-control="select2" id="dt_sub_activity_selected_activity" data-placeholder="Select an Activity"  data-allow-clear="true" autofocus>
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="col-12">
                                                    <table id="dt_sub_activity_selected" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
        </div>
        <div class="row">
            <div class="col-lg-12 col-12 pe-lg-1">
                <div class="row mb-2">
                    <div class="col-12">
                        <div class="card shadow-sm card_configuration">
                            <div class="card-body">
                                <div class="row mb-5">
                                    <div class="col-12">
                                        <span class="fs-3">Channel Calculator</span>
                                        <div class="separator border-2 my-2 border-secondary"></div>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex justify-content-between flex-lg-row flex-column">
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                            <div class="row me-lg-5">
                                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 mb-md-5 mb-sm-5">
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <span class="fs-3">Calculator Creation</span>
                                                        </div>
                                                    </div>
                                                    <div class="col-12">
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="baseline">Baseline</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="baseline" id="baseline">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="uplift">Uplift</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="uplift" id="uplift">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="totalSales">Total Sales</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="totalSales" id="totalSales">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="salesContribution">Sales Contribution</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="salesContribution" id="salesContribution">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="storesCoverage">Stores Coverage</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="storesCoverage" id="storesCoverage">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="redemptionRate">Redemption Rate</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="redemptionRate" id="redemptionRate">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="cr">CR</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="cr" id="cr">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="cost">Cost</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="cost" id="cost">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <span class="fs-3">Calculator Reconciliation</span>
                                                        </div>
                                                    </div>
                                                    <div class="col-12">
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="baselineRecon">Baseline</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="baselineRecon" id="baselineRecon">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="upliftRecon">Uplift</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="upliftRecon" id="upliftRecon">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="totalSalesRecon">Total Sales</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="totalSalesRecon" id="totalSalesRecon">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="salesContributionRecon">Sales Contribution</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="salesContributionRecon" id="salesContributionRecon">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="storesCoverageRecon">Stores Coverage</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="storesCoverageRecon" id="storesCoverageRecon">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="redemptionRateRecon">Redemption Rate</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="redemptionRateRecon" id="redemptionRateRecon">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="crRecon">CR</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="crRecon" id="crRecon">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                        <div class="row fv-row">
                                                            <label class="col-lg-5 col-md-12 col-sm-12 col-12 col-form-label" for="costRecon">Cost</label>
                                                            <div class="col-lg-7 col-md-12 col-sm-12 col-12">
                                                                <select class="form-select form-select-sm" data-control="select2" name="costRecon" id="costRecon">
                                                                    <option value="0">Disabled</option>
                                                                    <option value="1">Enabled</option>
                                                                    <option value="2">Auto</option>
                                                                </select>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                            <div class="row ms-lg-5">
                                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 mb-md-5 mb-sm-5">
                                                    <div class="w-50 align-self-end mb-sm-2">
                                                        <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                            <input type="text" class="form-control form-control-sm ps-10" value="" placeholder="Search" id="dt_channel_source_search" autocomplete="off">
                                                            <label class="d-none" for="dt_channel_source_search"></label>
                                                        </div>
                                                    </div>
                                                    <div class="row me-lg-2">
                                                        <table id="dt_channel_source" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="w-50 align-self-end mb-sm-2 ms-lg-5">
                                                        <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                            <input type="text" class="form-control form-control-sm ps-10" value="" placeholder="Search" id="dt_channel_selected_search" autocomplete="off">
                                                            <label class="d-none" for="dt_channel_selected_search"></label>
                                                        </div>
                                                    </div>
                                                    <div class="row ms-lg-2">
                                                        <table id="dt_channel_selected" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row p-3">
                                    <button type="button" class="btn btn-sm btn-optima" id="btn_update_configuration">
                                       Update Configuration
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-12 pe-lg-1">
                <div class="row mb-2">
                    <div class="col-12">
                        <div class="card shadow-sm card_channel_calculator_configuration">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-12">
                                        <table id="dt_channel_calculator_configuration" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/js/generate-uuid.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/configuration/promo-calculator/js/promo-calculator-form.js?v=11') }}"></script>
@endsection

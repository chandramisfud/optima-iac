@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/debit-note/dn-workflow/css/dn-workflow-methods.css?v=' . microtime()) }}" rel="stylesheet" type="text/css"/>
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
    <div class="px-lg-0 sticky-top-header">
        <div class="row mb-3">
            <div class="col-12">
                <div class="card card-flush">
                    <div class="card-body">
                        <div class="row mb-10">
                            <div class="col-12">
                                <div class="d-flex flex-lg-row flex-column">
                                    <div class="w-lg-225px w-100 mb-lg-0 mb-2">
                                        <input type="text" class="form-control form-control-sm" id="filter_dn_id" placeholder="Search DN ID" autocomplete="off">
                                    </div>
                                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100 mb-lg-0 mb-2 ms-lg-5 me-lg-10" id="btn_view">
                                        <span class="indicator-label">
                                            <span class="fa fa-search"></span> View
                                        </span>
                                        <span class="indicator-progress">
                                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                        </span>
                                    </button>
                                    <div class="w-lg-400px w-100 p-1 mb-lg-0 mb-2">
                                        <div class="d-flex flex-row align-items-center justify-content-around">
                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                <input class="form-check-input" type="checkbox" role="switch" id="workflow_history" checked="" autocomplete="off">
                                                <label class="form-check-label" for="workflow_history">Workflow History</label>
                                            </div>
                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                <input class="form-check-input" type="checkbox" role="switch" id="change_history" checked="" autocomplete="off">
                                                <label class="form-check-label" for="change_history">Change History</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-12">
                                <div class="d-flex flex-lg-row flex-column">
                                    <div class="d-flex align-items-center w-lg-130px w-100 border-bottom border-7 h-40px me-lg-5 mb-lg-0 mb-2 border-distributor" id="flow_distributor">
                                        <span class="fw-bolder fs-6 ms-1">DISTRIBUTOR</span>
                                        <span class="tooltip-distributor">created</br> send_to_dist_ho</br>validate_by_dist_ho</span>
                                    </div>
                                    <div class="d-flex align-items-center w-lg-130px w-100 border-bottom border-7 h-40px me-lg-5 mb-lg-0 mb-2 border-waiting-validation" id="flow_waiting_validation">
                                        <span class="fw-bolder fs-6 ms-1">WAITING VALIDATION</span>
                                        <span class="tooltip-waiting-validation">Sent_to_danone</br>received_by_danone</br>validate_by_finance</br>validate_by_sales</span>
                                    </div>
                                    <div class="d-flex align-items-center w-lg-130px w-100 border-bottom border-7 h-40px me-lg-5 mb-lg-0 mb-2 border-payment-process" id="flow_payment_process">
                                        <span class="fw-bolder fs-6 ms-1">PAYMENT PROCESS</span>
                                        <span class="tooltip-payment-process">validate_by_danone</br>ready_to_invoice</br>invoice</span>
                                    </div>
                                    <div class="d-flex align-items-center w-lg-130px w-100 border-bottom border-7 h-40px me-lg-5 mb-lg-0 mb-2 border-paid" id="flow_paid">
                                        <span class="fw-bolder fs-6 ms-1">PAID</span>
                                        <span class="tooltip-paid">confirm_paid</span>
                                    </div>
                                    <div class="d-flex align-items-center w-lg-130px w-100 border-bottom border-7 h-40px mb-lg-0 mb-2 border-canceled" id="flow_canceled">
                                        <span class="fw-bolder fs-6 ms-1">CANCELED</span>
                                        <span class="tooltip-canceled">cancelled</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row mb-3">
        <div class="col-12">
            <div class="card card-flush shadow card_data_dn">
                <div class="card-body">
                    <form id="form_dn">
                        <div class="row">
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="period">Year</label>
                                    <div class="col-lg-4 col-12">
                                        <input type="text" class="form-control form-control-sm" id="year" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-12 col-form-label" for="subAccountId">Sub Account</label>
                                    <div class="col-lg-4 col-12">
                                        <input type="text" class="form-control form-control-sm" id="subAccount" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="sellingPointId">Selling Point</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="sellingPoint" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="promoRefId">Promo ID</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="promoRefId" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="entityId">Entity</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="entity" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row mb-1">
                                    <label class="col-lg-4 col-12 col-form-label" for="entityAddress">Address</label>
                                    <div class="col-lg-8 col-12">
                                        <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="entityAddress" rows="3" autocomplete="off" readonly></textarea>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="entityUp">Up. </label>
                                    <div class="col-lg-8 col-12">
                                        <input class="form-control form-control-sm" type="text" id="entityUp" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row mb-1">
                                    <label class="col-lg-4 col-12 col-form-label" for="activityDesc">Description </label>
                                    <div class="col-lg-8 col-12">
                                        <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="activityDesc" rows="3" autocomplete="off" readonly></textarea>
                                    </div>
                                </div>
                                <div class="row mb-1">
                                    <label class="col-lg-4 col-12 col-form-label" for="feeDesc">Fee Description </label>
                                    <div class="col-lg-8 col-12">
                                        <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="feeDesc" rows="3" autocomplete="off" readonly></textarea>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="fpNumber">FP Number </label>
                                    <div class="col-lg-8 col-12">
                                        <input class="form-control form-control-sm" type="text" id="fpNumber" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="fpDate">FP Date </label>
                                    <div class="col-lg-8 col-12">
                                        <input class="form-control form-control-sm" type="text" id="fpDate" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-12 col-form-label" for="whtType">WHT Type </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="whtType" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-12 col-form-label" for="taxLevel">Tax Level </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="taxLevel" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="deductionDate">VAT Expired </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="vatExpired" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="deductionDate">DN Promo </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="isDNPromo" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="deductionDate">Deduction Date </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="deductionDate" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="memDocNo">Memorial Doc. No </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="memDocNo" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="intDocNo">Internal Doc. No </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="intDocNo" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="period">Period </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="period" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="dnAmount">DN Amount </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="dnAmount" autocomplete="off" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="feeAmount">Fee </label>
                                    <div class="col-lg-8 col-12">
                                        <div class="input-group input-group-sm">
                                            <div class="col-3">
                                                <input type="text" class="form-control form-control-sm text-end" value="0" id="feePct" autocomplete="off" readonly/>
                                            </div>
                                            <span class="input-group-text px-5">%</span>
                                            <input class="form-control form-control-sm text-end" type="text" value="0" id="feeAmount" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-lg-0 mb-3">
                                    <label class="col-lg-4 col-12 col-form-label" for="dpp">DPP </label>
                                    <div class="col-lg-8 col-12 mb-lg-0 mb-3">
                                        <input type="text" class="form-control form-control-sm text-end" id="dpp" autocomplete="off" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row mb-lg-1 mb-3">
                                    <div class="col-lg-4 col-12 mb-lg-0 mb-3">
                                        <input type="text" class="form-control form-control-sm" id="statusPPN" autocomplete="off" readonly/>
                                    </div>
                                    <div class="col-lg-8 col-12 mb-lg-0 mb-3">
                                        <div class="input-group input-group-sm">
                                            <div class="col-3">
                                                <input type="text" class="form-control form-control-sm text-end" value="0" id="ppnPct" autocomplete="off" readonly/>
                                            </div>
                                            <span class="input-group-text px-5">%</span>
                                            <input class="form-control form-control-sm text-end" type="text" value="0" id="ppnAmount" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-lg-1 mb-3">
                                    <div class="col-lg-4 col-12 mb-lg-0 mb-3">
                                        <input type="text" class="form-control form-control-sm" id="statusPPH" autocomplete="off" readonly/>
                                    </div>
                                    <div class="col-lg-8 col-12">
                                        <div class="input-group input-group-sm">
                                            <div class="col-3">
                                                <input type="text" class="form-control form-control-sm text-end" value="0" id="pphPct" autocomplete="off" readonly/>
                                            </div>
                                            <span class="input-group-text px-5">%</span>
                                            <input class="form-control form-control-sm text-end" type="text" value="0" id="pphAmount" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="total">Total </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="total" autocomplete="off" value="0" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <label class="col-lg-4 col-form-label">Attachment File </label>
                        </div>
                        <div class="row">
                            <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                                <div class="row mb-3">
                                    <div class="d-flex w-lg-85">
                                        <label class="col-form-label mt-1 me-5 px-0 py-0 pt-2">1.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3" type="text" id="row1" autocomplete="off" readonly/>
                                        <button class="btn btn-sm btn-optima btn_download" id="btn_download1" title="Download" value="1">
                                            <span class="fa fa-download text-white"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex w-lg-85">
                                        <label class="col-form-label mt-1 me-5 px-0 py-0 pt-2">2.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3" type="text" id="row2" autocomplete="off" readonly/>
                                        <button class="btn btn-sm btn-optima btn_download" id="btn_download2" title="Download" value="2">
                                            <span class="fa fa-download text-white"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex w-lg-85">
                                        <label class="col-form-label mt-1 me-5 px-0 py-0 pt-2">3.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3" type="text" id="row3" autocomplete="off" readonly/>
                                        <button class="btn btn-sm btn-optima btn_download" id="btn_download3" title="Download" value="3">
                                            <span class="fa fa-download text-white"> </span>
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                                <div class="row mb-3">
                                    <div class="d-flex w-lg-85">
                                        <label class="col-form-label mt-1 me-5 px-0 py-0 pt-2">4.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3" type="text" id="row4" autocomplete="off" readonly/>
                                        <button class="btn btn-sm btn-optima btn_download" id="btn_download4" title="Download" value="4">
                                            <span class="fa fa-download text-white"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex w-lg-85">
                                        <label class="col-form-label mt-1 me-5 px-0 py-0 pt-2">5.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3" type="text" id="row5" autocomplete="off" readonly/>
                                        <button class="btn btn-sm btn-optima btn_download" id="btn_download5" title="Download" value="5">
                                            <span class="fa fa-download text-white"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex w-lg-85">
                                        <label class="col-form-label mt-1 me-5 px-0 py-0 pt-2">6.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3" type="text" id="row6" autocomplete="off" readonly/>
                                        <button class="btn btn-sm btn-optima btn_download" id="btn_download6" title="Download" value="6">
                                            <span class="fa fa-download text-white"> </span>
                                        </button>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                                <div class="row mb-3">
                                    <div class="d-flex w-lg-85">
                                        <label class="col-form-label mt-1 me-5 px-0 py-0 pt-2">7.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3" type="text" id="row7" autocomplete="off" readonly/>
                                        <button class="btn btn-sm btn-optima btn_download" id="btn_download7" title="Download" value="7">
                                            <span class="fa fa-download text-white"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex w-lg-85">
                                        <label class="col-form-label mt-1 me-5 px-0 py-0 pt-2">8.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3" type="text" id="row8" autocomplete="off" readonly/>
                                        <button class="btn btn-sm btn-optima btn_download" id="btn_download8" title="Download" value="8">
                                            <span class="fa fa-download text-white"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex w-lg-85">
                                        <label class="col-form-label mt-1 me-5 px-0 py-0 pt-2">9.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3" type="text" id="row9" autocomplete="off" readonly/>
                                        <button class="btn btn-sm btn-optima btn_download" id="btn_download9" title="Download" value="9">
                                            <span class="fa fa-download text-white"> </span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <div class="row mb-3" id="row_workflow_history">
        <div class="col-12">
            <div class="card card-flush shadow card_workflow_history">
                <div class="card-header pt-3 px-5">
                    <span class="fs-4">Workflow History</span>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-2 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <div class="inner-addon left-addon right-addon">
                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_workflow_history_search" autocomplete="off">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <table id="dt_workflow_history" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row mb-3" id="row_change_history">
        <div class="col-12">
            <div class="card card-flush shadow card_change_history">
                <div class="card-header pt-3 px-5">
                    <span class="fs-4">Change History</span>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-2 col-md-12 col-sm-12 mb-lg-0 mb-2">
                            <div class="inner-addon left-addon right-addon">
                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_change_history_search" autocomplete="off">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <table id="dt_change_history" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/pages/debit-note/dn-workflow/js/dn-workflow-methods.js?v=' . microtime()) }}"></script>
@endsection

@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-workflow/css/promo-workflow-methods.css?v=' . microtime()) }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/pages/promo/promo-workflow/css/promo-workflow-revamp.css?v=' . microtime()) }}" rel="stylesheet" type="text/css"/>

@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-print-back')
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
                                    <div class="w-lg-250px w-100 mb-lg-0 mb-2">
                                        <input type="text" class="form-control form-control-sm" id="filter_promo_id" placeholder="Search Promo ID" autocomplete="off">
                                    </div>
                                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100 mb-lg-0 mb-2 ms-lg-5 me-lg-10" id="btn_view">
                                        <span class="indicator-label">
                                            <span class="fa fa-search"></span> View
                                        </span>
                                        <span class="indicator-progress">
                                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                        </span>
                                    </button>
                                    <div class="w-lg-500px w-100 p-1 mb-lg-0 mb-2">
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
                                    <div class="d-flex align-items-center w-lg-150px w-100 border-bottom border-7 h-40px me-lg-5 mb-lg-0 mb-2 border-ongoing-approval" id="flow_ongoing_approval">
                                        <span class="fw-bolder fs-6 ms-1" id="txt_ongoing_approval">ONGOING APPROVAL</span>
                                    </div>
                                    <div class="d-flex align-items-center w-lg-150px w-100 border-bottom border-7 h-40px me-lg-5 mb-lg-0 mb-2 border-fully-approved" id="flow_fully_approved">
                                        <span class="fw-bolder fs-6 ms-1" id="txt_fully_approved">FULLY APPROVED</span>
                                    </div>
                                    <div class="d-flex align-items-center w-lg-150px w-100 border-bottom border-7 h-40px me-lg-5 mb-lg-0 mb-2 border-claim-process" id="flow_claim_process">
                                        <span class="fw-bolder fs-6 ms-1" id="txt_claim_process">CLAIM PROCESS</span>
                                    </div>
                                    <div class="d-flex align-items-center w-lg-150px w-100 border-bottom border-7 h-40px me-lg-5 mb-lg-0 mb-2 border-closed" id="flow_closed">
                                        <span class="fw-bolder fs-6 ms-1" id="txt_closed">CLOSED</span>
                                    </div>
                                    <div class="d-flex align-items-center w-lg-150px w-100 border-bottom border-7 h-40px mb-lg-0 mb-2 border-canceled" id="flow_canceled">
                                        <span class="fw-bolder fs-6 ms-1" id="txt_canceled">CANCELED</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row mb-3 card_data_promo_revamp d-none" id="card_form_rc_new">
        <div class="col-lg-8 col-12 pe-lg-1">
            <div class="d-flex justify-content-between flex-column h-100">
                <div class="flex-grow-1 mb-2">
                    <div class="row h-100">
                        <div class="col-12">
                            <div class="card shadow-sm card_header h-100">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-lg-6 col-12">
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entityLongDescRCNew">Entity</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="entityLongDescRCNew" id="entityLongDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="groupBrandLongDescRCNew">Brand</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="groupBrandLongDescRCNew" id="groupBrandLongDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subCategoryLongDescRCNew">Sub Category</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="subCategoryLongDescRCNew" id="subCategoryLongDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="activityLongDescRCNew">Activity</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="activityLongDescRCNew" id="activityLongDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityLongDescRCNew">Sub Activity</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="subActivityLongDescRCNew" id="subActivityLongDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="activityDescRCNew">Activity Name</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <textarea maxlength="255" class="form-control form-control-sm" name="activityDescRCNew" id="activityDescRCNew" rows="4" readonly></textarea>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="startPromoRCNew">Activity Period</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <div class="input-group input-group-sm">
                                                        <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromoRCNew" id="startPromoRCNew" readonly>
                                                        <span class="input-group-text">to</span>
                                                        <label for="endPromoRCNew"></label>
                                                        <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromoRCNew" id="endPromoRCNew" readonly>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="offset-lg-1 col-lg-5 col-12">
                                            <div class="row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="periodRCNew">Period</label>
                                                <div class="col-lg-5 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="periodRCNew" id="periodRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="distributorLongDescRCNew">Distributor</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="distributorLongDescRCNew" id="distributorLongDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="channelDescRCNew">Channel</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="channelDescRCNew" id="channelDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right col-form-label-over-space" for="subChannelDescRCNew">Sub Channel</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="subChannelDescRCNew" id="subChannelDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="accountDescRCNew">Account</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="accountDescRCNew" id="accountDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right col-form-label-over-space" for="subAccountDescRCNew">Sub Account</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="subAccountDescRCNew" id="subAccountDescRCNew" value="" readonly/>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="flex-grow-1">
                    <div class="row h-100">
                        <div class="col-12">
                            <div class="card shadow-sm card_mechanism h-100">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-lg-12 col-12">
                                            <table id="dt_mechanism_revamp" class="table table-striped table-row-bordered table-responsive table-sm table-hover">
                                                <thead class="fw-bold fs-6 text-gray-800 bg-optima text-white"></thead>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-4 col-12 ps-lg-1">
            <div class="d-flex justify-content-between flex-column h-100">
                <div class="flex-grow-1 mb-2">
                    <div class="row h-100">
                        <div class="col-12">
                            <div class="card shadow-sm card_budget h-100">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetSourceNameRCNew">Budget Source</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" name="budgetSourceNameRCNew" id="budgetSourceNameRCNew" autocomplete="off" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="remainingBudgetRCNew">Remaining Budget</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                    <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="remainingBudgetRCNew" id="remainingBudgetRCNew" autocomplete="off" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalCostRCNew">Total Cost</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                    <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="totalCostRCNew" id="totalCostRCNew" autocomplete="off" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-12 col-form-label" for="totalClaimRCNew">DN Claim</label>
                                                <div class="col-lg-8 col-12">
                                                    <input type="text" class="form-control form-control-sm text-end" id="totalClaimRCNew" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-12 col-form-label" for="totalPaidRCNew">DN Paid</label>
                                                <div class="col-lg-8 col-12">
                                                    <input type="text" class="form-control form-control-sm text-end" id="totalPaidRCNew" readonly/>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="flex-grow-1 mb-2">
                    <div class="row h-100">
                        <div class="col-12">
                            <div class="card shadow-sm card_region h-100">
                                <div class="card-body">
                                    <div class="row mb-2">
                                        <div class="col-12">
                                            <label class="fw-bold text-gray-800 fs-4" for="regionId">Region</label> <span class="text-danger fs-8 d-none" id="regionInvalid">Please select a region</span>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12">
                                            <span id="txtInfoRegion">-</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="flex-grow-1 mb-2">
                    <div class="row h-100">
                        <div class="col-12">
                            <div class="card shadow-sm card_sku h-100">
                                <div class="card-body">
                                    <div class="row mb-2">
                                        <div class="col-12">
                                            <span class="fw-bold text-gray-800 fs-4">SKU</span>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-12">
                                            <div class="row">
                                                <div class="col-lg-12 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                    <div class="inner-addon left-addon right-addon">
                                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                    </svg>
                                                </span>
                                                        <input type="text" class="form-control form-control-sm ps-10" value="" placeholder="Search" id="dt_sku_search" autocomplete="off">
                                                        <label class="d-none" for="dt_sku_search"></label>
                                                    </div>
                                                </div>
                                            </div>
                                            <table id="dt_sku" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="flex-grow-1">
                    <div class="row h-100">
                        <div class="col-12">
                            <div class="card shadow-sm card_attachment h-100">
                                <div class="card-body">
                                    <div class="col-12 h-150px" style="overflow-y: auto; overflow-x: hidden">
                                        <div class="row mb-3">
                                            <div class="d-flex align-items-center w-lg-85">
                                                <label class="me-5" for="review_file_label_1">1.</label>
                                                <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_rc_new_1" readonly/>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_rc_new1" name="btn_download_rc_new1" value="1" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_revamp1" name="btn_view_revamp1" value="1" disabled>
                                                    <span class="fa fa-eye"> </span>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="row mb-3">
                                            <div class="d-flex align-items-center w-lg-85">
                                                <label class="me-5" for="review_file_label_2">2.</label>
                                                <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_rc_new_2" readonly/>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_rc_new2" name="btn_download_rc_new2" value="2" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_revamp2" name="btn_view_revamp2" value="2" disabled>
                                                    <span class="fa fa-eye"> </span>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="row mb-3">
                                            <div class="d-flex align-items-center w-lg-85">
                                                <label class="me-5" for="review_file_label_3">3.</label>
                                                <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_rc_new_3" readonly/>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_rc_new3" name="btn_download_rc_new3" value="3" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_revamp3" name="btn_view_revamp3" value="3" disabled>
                                                    <span class="fa fa-eye"> </span>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="row mb-3">
                                            <div class="d-flex align-items-center w-lg-85">
                                                <label class="me-5" for="review_file_label_4">4.</label>
                                                <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_rc_new_4" readonly/>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_rc_new4" name="btn_download_rc_new4" value="4" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_revamp4" name="btn_view_revamp4" value="4" disabled>
                                                    <span class="fa fa-eye"> </span>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="row mb-3">
                                            <div class="d-flex align-items-center w-lg-85">
                                                <label class="me-5" for="review_file_label_5">5.</label>
                                                <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_rc_new_5" readonly/>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_rc_new5" name="btn_download_rc_new5" value="5" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_revamp5" name="btn_view_revamp5" value="5" disabled>
                                                    <span class="fa fa-eye"> </span>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="row mb-3">
                                            <div class="d-flex align-items-center w-lg-85">
                                                <label class="me-5" for="review_file_label_6">6.</label>
                                                <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_rc_new_6" readonly/>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_rc_new6" name="btn_download_rc_new6" value="6" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_revamp6" name="btn_view_revamp6" value="6" disabled>
                                                    <span class="fa fa-eye"> </span>
                                                </button>
                                            </div>
                                        </div>
                                        <div class="row mb-3">
                                            <div class="d-flex align-items-center w-lg-85">
                                                <label class="me-5" for="review_file_label_7">7.</label>
                                                <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_rc_new_7" readonly/>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_rc_new7" name="btn_download_rc_new7" value="7" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_revamp7" name="btn_view_revamp7" value="7" disabled>
                                                    <span class="fa fa-eye"> </span>
                                                </button>
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

    <div class="row mb-3 card_data_promo_revamp d-none" id="card_form_dc_new">
        <div class="col-lg-7 col-12 pe-lg-1">
            <div class="d-flex justify-content-between flex-column h-100">
                <div class="flex-grow-1 mb-2">
                    <div class="row h-100">
                        <div class="col-12">
                            <div class="card shadow-sm card_header h-100">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-lg-12 col-12">
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="periodDCNew">Period</label>
                                                <div class="col-lg-3 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="periodDCNew" id="periodDCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entityLongDescDCNew">Entity</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="entityLongDescDCNew" id="entityLongDescDCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="groupBrandLongDescDCNew">Brand</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="groupBrandLongDescDCNew" id="groupBrandLongDescDCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="distributorLongDescDCNew">Distributor</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="distributorLongDescDCNew" id="distributorLongDescDCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityTypeDescDCNew">Sub Activity Type</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="subActivityTypeDescDCNew" id="subActivityTypeDescDCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityLongDescDCNew">Sub Activity</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="subActivityLongDescDCNew" id="subActivityLongDescDCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="channelDescDCNew">Channel</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <input type="text" class="form-control form-control-sm" name="channelDescDCNew" id="channelDescDCNew" value="" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="startPromoDCNew">Activity Period</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <div class="input-group input-group-sm">
                                                        <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromoDCNew" id="startPromoDCNew" readonly>
                                                        <span class="input-group-text">to</span>
                                                        <label for="endPromoDCNew"></label>
                                                        <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromoDCNew" id="endPromoDCNew" readonly>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="mechanismDCNew">Mechanism</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                    <input type="text" class="form-control form-control-sm" name="mechanismDCNew" id="mechanismDCNew" autocomplete="off" readonly/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotesDCNew">Initiator Notes</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                    <input type="text" class="form-control form-control-sm" name="initiatorNotesDCNew" id="initiatorNotesDCNew" autocomplete="off" readonly/>
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
        <div class="col-lg-5 col-12 ps-lg-1">
            <div class="d-flex h-100 flex-column justify-content-between">
                <div class="row mb-2 flex-shrink-1 h-100">
                    <div class="col-12">
                        <div class="card shadow-sm h-100 card_budget">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-12">
                                        <div class="row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetSourceNameDCNew">Budget Source</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" name="budgetSourceNameDCNew" id="budgetSourceNameDCNew" autocomplete="off" readonly/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="remainingBudgetDCNew">Remaining Budget</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="remainingBudgetDCNew" id="remainingBudgetDCNew" autocomplete="off" readonly/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalCostDCNew">Total Cost</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="totalCostDCNew" id="totalCostDCNew" autocomplete="off" readonly/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-4 col-12 col-form-label" for="totalClaimDCNew">DN Claim</label>
                                            <div class="col-lg-8 col-12">
                                                <input type="text" class="form-control form-control-sm text-end" id="totalClaimDCNew" readonly/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-4 col-12 col-form-label" for="totalPaidDCNew">DN Paid</label>
                                            <div class="col-lg-8 col-12">
                                                <input type="text" class="form-control form-control-sm text-end" id="totalPaidDCNew" readonly/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mb-2 flex-shrink-1 h-100">
                    <div class="col-12">
                        <div class="card shadow-sm h-100 card_attachment">
                            <div class="card-body">
                                <div class="col-12">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-12">
                                            <span class="fw-bold text-gray-700 fs-4">File attachments</span>
                                            <div class="separator border-2 my-2 border-secondary"></div>
                                        </div>
                                    </div>

                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_1">1.</label>
                                            <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_dc_new_1" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_dc_new1" name="btn_download_dc_new1" value="1" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_dc_new1" name="btn_view_dc_new1" value="1" disabled>
                                                <span class="fa fa-eye"> </span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_2">2.</label>
                                            <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_dc_new_2" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_dc_new2" name="btn_download_dc_new2" value="2" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_dc_new2" name="btn_view_dc_new2" value="2" disabled>
                                                <span class="fa fa-eye"> </span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_3">3.</label>
                                            <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_dc_new_3" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_dc_new3" name="btn_download_dc_new3" value="3" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_dc_new3" name="btn_view_dc_new3" value="3" disabled>
                                                <span class="fa fa-eye"> </span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_4">4.</label>
                                            <input type="text" class="form-control form-control-sm review_file_label_revamp" id="review_file_label_dc_new_4" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_revamp" title="Download" id="btn_download_dc_new4" name="btn_download_dc_new4" value="4" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view_revamp" title="Preview" id="btn_view_dc_new4" name="btn_view_dc_new4" value="4" disabled>
                                                <span class="fa fa-eye"> </span>
                                            </button>
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

    <div class="row mb-3 d-none" id="card_form_rc">
        <div class="col-12">
            <div class="card card-flush shadow-sm card_data_promo">
                <div class="card-body">
                    <div class="row">
                        <div class="col-12">
                            <span class="fw-bold fs-3">Detail</span>
                            <div class="separator border-2 border-secondary my-2"></div>
                        </div>
                    </div>
                    <form id="form_promo_dtl" autocomplete="off">
                        <div class="row">
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="periode">Budget Year</label>
                                    <div class="col-lg-2 col-12">
                                        <input type="text" class="form-control form-control-sm" id="periode" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="promoPlanRefId">Promo Planning</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="promoPlanRefId" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="allocationRefId">Budget Source</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="allocationRefId" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="allocationDesc">Budget Allocation</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="allocationDesc" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="activityLongDesc">Activity</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="activityLongDesc" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="subActivityLongDesc">Sub Activity</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="subActivityLongDesc" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="activityDesc">Activity Name</label>
                                    <div class="col-lg-8 col-12 mb-1">
                                        <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="activityDesc" rows="3" readonly></textarea>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="startPromo">Activity Period</label>
                                    <div class="col-lg-8 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm" id="startPromo" readonly>
                                            <span class="input-group-text">to</span>
                                            <input type="text" class="form-control form-control-sm" id="endPromo" readonly>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="offset-lg-1 col-lg-5 col-md-12 col-sm-12 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="tsCoding">TS Code</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="tsCoding" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="subCategoryDesc">Sub Category</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="subCategoryDesc" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="principalName">Entity</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="principalName" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="distributorName">Distributor</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm" id="distributorName" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="budgetAmount">Budget Deployed</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="budgetAmount" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="remainingBudget">Remaining Budget</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="remainingBudget" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="totalClaim">DN Claim</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="totalClaim" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="totalPaid">DN Paid</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="totalPaid" value="0" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <div class="separator border-1 border-secondary my-3"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="normalSales">Baseline Sales</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="normalSales" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="incrSales">Sales Increment</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="incrSales" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="investment">Investment </label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="investment" value="0" readonly/>
                                    </div>
                                </div>
                            </div>
                            <div class="offset-lg-1 col-lg-5 col-md-12 col-sm-12 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="totSales">Total Sales</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="totSales" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="totInvestment">Total Investment</label>
                                    <div class="col-lg-8 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="totInvestment" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="roi">ROI</label>
                                    <div class="col-lg-8 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm text-end" id="roi" value="0" readonly/>
                                            <span class="input-group-text">%</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-12 col-form-label" for="costRatio">Cost Ratio</label>
                                    <div class="col-lg-8 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm text-end" id="costRatio" value="0" readonly/>
                                            <span class="input-group-text">%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <div class="separator border-1 border-secondary my-3"></div>
                            </div>
                        </div>
                    </form>

                    <div class="row">
                        <div class="col-12">
                            <span class="fw-bold fs-3">Detail Attribute</span>
                            <div class="separator border-2 border-secondary my-2"></div>
                        </div>
                    </div>

                    <form id="form_promo_dtl_attribute" autocomplete="off">
                        <div class="row mb-3">
                            <div class="col-lg-3 col-md-12 col-sm-12 col-12">
                                <label class="col-form-label col-lg-12 pb-1" for="channel">Channel</label>
                                <div class="col-lg-12">
                                    <input type="text" class="form-control form-control-sm" id="channel" readonly/>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-12 col-sm-12 col-12">
                                <label class="col-form-label col-lg-12 pb-1" for="channel">Sub Channel</label>
                                <div class="col-lg-12">
                                    <input type="text" class="form-control form-control-sm" id="subChannel" readonly/>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-12 col-sm-12 col-12">
                                <label class="col-form-label col-lg-12 pb-1" for="account">Account</label>
                                <div class="col-lg-12">
                                    <input type="text" class="form-control form-control-sm" id="account" readonly/>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-12 col-sm-12 col-12">
                                <label class="col-form-label col-lg-12 pb-1" for="subAccount">Sub Account</label>
                                <div class="col-lg-12">
                                    <input type="text" class="form-control form-control-sm" id="subAccount" readonly/>
                                </div>
                            </div>
                        </div>


                        <div class="row mb-3">
                            <div class="col-lg-4 col-12">
                                <div class="card card-flush mb-3" id="card_region">
                                    <div class="card-header collapsible cursor-pointer rotate collapsed" data-bs-toggle="collapse" data-bs-target="#kt_region_card_collapsible" aria-expanded="false">
                                        <span class="fw-bold fs-5 my-auto">Region</span>
                                        <div class="card-toolbar rotate-180">
                                            <i class="fa fa-chevron-down fs-3"></i>
                                        </div>
                                    </div>
                                    <div id="kt_region_card_collapsible" class="collapse" style="">
                                        <div class="card-body">
                                            <div class="px-2" id="card_list_region">

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 col-12">
                                <div class="card card-flush mb-3" id="card_brand">
                                    <div class="card-header collapsible cursor-pointer rotate collapsed" data-bs-toggle="collapse" data-bs-target="#kt_brand_card_collapsible" aria-expanded="false">
                                        <span class="fw-bold fs-5 my-auto">Sub Brand</span>
                                        <div class="card-toolbar rotate-180">
                                            <i class="fa fa-chevron-down fs-3"></i>
                                        </div>
                                    </div>
                                    <div id="kt_brand_card_collapsible" class="collapse" style="">
                                        <div class="card-body">
                                            <div class="px-2" id="card_list_brand">

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 col-12">
                                <div class="card card-flush mb-3" id="card_sku">
                                    <div class="card-header collapsible cursor-pointer rotate collapsed" data-bs-toggle="collapse" data-bs-target="#kt_sku_card_collapsible" aria-expanded="false">
                                        <span class="fw-bold fs-5 my-auto">SKU</span>
                                        <div class="card-toolbar rotate-180">
                                            <i class="fa fa-chevron-down fs-3"></i>
                                        </div>
                                    </div>
                                    <div id="kt_sku_card_collapsible" class="collapse" style="">
                                        <div class="card-body">
                                            <div class="px-2" id="card_list_sku">

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row mb-3">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                <table id="dt_mechanism" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>

                                <div class="separator border-1 border-secondary my-2"></div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-12">
                                <span class="fw-bold fs-3">Attachment</span>
                                <div class="separator border-2 border-secondary my-2"></div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-4 col-md-12 col-12">
                                <div class="row">
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">1.</label>
                                        <input type="text" class="form-control form-control-sm" id="row1" data-row="1" readonly>
                                        <button class="btn btn-sm btn-optima invisible flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download1" value="1">
                                            <span class="fa fa-download"></span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">2.</label>
                                        <input type="text" class="form-control form-control-sm" id="row2" data-row="2" readonly>
                                        <button class="btn btn-sm btn-optima invisible flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download2" value="2">
                                            <span class="fa fa-download"></span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">3.</label>
                                        <input type="text" class="form-control form-control-sm" id="row3" data-row="3" readonly>
                                        <button class="btn btn-sm btn-optima invisible flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download3" value="3">
                                            <span class="fa fa-download"></span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">4.</label>
                                        <input type="text" class="form-control form-control-sm" id="row4" data-row="4" readonly>
                                        <button class="btn btn-sm btn-optima invisible flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download4" value="4">
                                            <span class="fa fa-download"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div class="offset-lg-2 col-lg-4 col-md-12 col-12">
                                <div class="row">
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">5.</label>
                                        <input type="text" class="form-control form-control-sm" id="row5" data-row="5" readonly>
                                        <button class="btn btn-sm btn-optima invisible flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download5" value="5">
                                            <span class="fa fa-download"></span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">6.</label>
                                        <input type="text" class="form-control form-control-sm" id="row6" data-row="6" readonly>
                                        <button class="btn btn-sm btn-optima invisible flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download6" value="6">
                                            <span class="fa fa-download"></span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">7.</label>
                                        <input type="text" class="form-control form-control-sm" id="row7" data-row="7" readonly>
                                        <button class="btn btn-sm btn-optima invisible flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download7" value="7">
                                            <span class="fa fa-download"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>

                    <div class="row">
                        <div class="col-12">
                            <div class="separator border-1 border-secondary my-2"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row mb-3 d-none" id="card_form_dc">
        <div class="col-12">
            <div class="card card-flush shadow-sm card_data_promo_dc">
                <div class="card-body">
                    <div class="row">
                        <div class="col-12">
                            <span class="fw-bold fs-3">Detail</span>
                            <div class="separator border-2 border-secondary my-2"></div>
                        </div>
                    </div>
                    <form id="form_promo_dc_dtl" autocomplete="off">
                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="periodDC">Budget Year</label>
                                    <div class="col-lg-2 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="period" id="periodDC" autocomplete="off" tabindex="0" readonly>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entityDC">Entity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="entity" id="entityDC" autocomplete="off" tabindex="1" readonly>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="brandDC">Brand</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="brand" id="brandDC" autocomplete="off" tabindex="2" readonly>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="distributorDC">Distributor</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="distributor" id="distributorDC" autocomplete="off" tabindex="3" readonly>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subCategoryDC">Sub Activity Type</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="subCategory" id="subCategoryDC" autocomplete="off" tabindex="4" readonly>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityDC">Sub Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="subActivity" id="subActivityDC" autocomplete="off" tabindex="5" readonly>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="channelDC">Channel</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="channel" id="channelDC" autocomplete="off" tabindex="6" readonly>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationRefIdDC">Budget Source</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="allocationRefId" id="allocationRefIdDC" autocomplete="off" tabindex="7" readonly>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="startPromoDC">Activity Period</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm" name="startPromo" id="startPromoDC" autocomplete="off" tabindex="8" readonly>
                                            <span class="input-group-text">to</span>
                                            <input type="text" class="form-control form-control-sm" name="endPromo" id="endPromoDC" autocomplete="off" tabindex="9" readonly>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="mechanism1DC">Mechanism</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="mechanism1" id="mechanism1DC" tabindex="10" readonly>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="investmentDC">Investment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="investment" id="investmentDC" autocomplete="off" value="0" inputmode="decimal" onClick="this.select();" tabindex="11" readonly/>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotesDC">Initiator Notes</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <input type="text" class="form-control form-control-sm" data-kt-dialer-control="input" name="initiatorNotes" id="initiatorNotesDC" autocomplete="off" tabindex="12" readonly/>
                                    </div>
                                </div>
                            </div>


                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationDescDC">Budget Allocation</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="allocationDesc" id="allocationDescDC" autocomplete="off" readonly />
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetDeployedDC">Budget Deployed</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetDeployed" id="budgetDeployedDC" autocomplete="off" inputmode="decimal" readonly />
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetRemainingDC">Remaining Budget</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetRemaining" id="budgetRemainingDC" autocomplete="off" inputmode="decimal" readonly />
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalClaimDC">DN Claim</label>
                                    <div class="col-lg-6 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="totalClaimDC" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalPaidDC">DN Paid</label>
                                    <div class="col-lg-6 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" id="totalPaidDC" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="bottom-0 w-lg-50 w-100 position-lg-absolute pb-5">
                                        <div class="col-12">
                                            <div class="row mt-5">
                                                <div class="offset-lg-2 col-lg-10 col-md-12 col-12">
                                                    <span class="fw-bold text-gray-700 fs-4">File attachments</span>
                                                    <div class="separator border-2 my-2 border-secondary"></div>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <div class="offset-lg-2 col-lg-10 col-12">
                                                    <div class="row">
                                                        <div class="d-flex align-items-center">
                                                            <label class="col-form-label me-5">1.</label>
                                                            <input type="text" class="form-control form-control-sm" id="row1DC" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 invisible btn_download" title="Download" id="btn_download1DC" name="btn_download1" value="1">
                                                                <span class="fa fa-download"></span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <div class="offset-lg-2 col-lg-10 col-12">
                                                    <div class="row">
                                                        <div class="d-flex align-items-center">
                                                            <label class="col-form-label me-5">2.</label>
                                                            <input type="text" class="form-control form-control-sm" id="row2DC" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 invisible btn_download" title="Download" id="btn_download2DC" name="btn_download2" value="2">
                                                                <span class="fa fa-download"></span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <div class="offset-lg-2 col-lg-10 col-12">
                                                    <div class="row">
                                                        <div class="d-flex align-items-center">
                                                            <label class="col-form-label me-5">3.</label>
                                                            <input type="text" class="form-control form-control-sm" id="row3DC" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 invisible btn_download" title="Download" id="btn_download3DC" name="btn_download3" value="3">
                                                                <span class="fa fa-download"></span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="offset-lg-2 col-lg-10 col-12">
                                                    <div class="row">
                                                        <div class="d-flex align-items-center">
                                                            <label class="col-form-label me-5">4.</label>
                                                            <input type="text" class="form-control form-control-sm" id="row4DC" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 invisible btn_download" title="Download" id="btn_download4DC" name="btn_download4" value="4">
                                                                <span class="fa fa-download"></span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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
                    <span class="fs-4" id="txt_workflow_history">Workflow History</span>
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

    <div class="row mb-3" id="row_dn_list">
        <div class="col-12">
            <div class="card card-flush shadow card_dn_list">
                <div class="card-header pt-3 px-5">
                    <span class="fs-4">Debit Note List</span>
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
                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_dn_list_search" autocomplete="off">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <table id="dt_dn_list" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    @include('Promo::promo-workflow.timeline-workflow')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-workflow/js/promo-workflow-methods.js?v=24') }}"></script>
@endsection

@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-approval/css/promo-approval-form-recon.css?v=1') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold my-1 ms-1" id="txt_info_method">Promo ID </small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-2">
        <div class="col-lg-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-12">
                            <h5 class="fw-normal">Notes</h5>
                        </div>
                    </div>
                    <form id="form_promo_approval" class="form" autocomplete="off">
                        @csrf
                        <div class="row mb-3">
                            <div class="col-lg-6 col-md-12 col-12">
                                <div class="row fv-row mb-1">
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="approvalStatusCode" id="approvalStatusCode" data-placeholder="Select Approval Action"  data-allow-clear="true">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row mb-1">
                                    <div class="col-lg-12">
                                        <textarea type="text" class="form-control form-control-sm" id="notes" name="notes" rows="3" autocomplete="off"></textarea>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label pt-0" for="sendback_notes">Last Sendback Notes <br> <span id="txt_sendback_notes"></span></label>
                                    <div class="col-lg-8 col-md-12 col-12">
                                        <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="sendback_notes" name="sendback_notes" rows="3" autocomplete="off" readonly></textarea>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6 col-md-12 col-12">
                                <button type="button" class="btn btn-sm btn-outline-optima fw-bolder" id="btn_submit">
                                <span class="indicator-label">
                                    <span class="la la-check me-1"></span> Submit
                                </span>
                                    <span class="indicator-progress">Submitting...
                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                </span>
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <div class="row mb-2">
        <div class="col-lg-8 col-12 pe-lg-1">
            <div class="d-flex flex-column h-100">
                <div class="row mb-2">
                    <div class="col-12">
                        <div class="card shadow-sm card_header">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-lg-6 col-12">
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entityLongDesc">Entity</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="entityLongDesc" id="entityLongDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="groupBrandLongDesc">Brand</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="groupBrandLongDesc" id="groupBrandLongDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subCategoryLongDesc">Sub Category</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="subCategoryLongDesc" id="subCategoryLongDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="activityLongDesc">Activity</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="activityLongDesc" id="activityLongDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityLongDesc">Sub Activity</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="subActivityLongDesc" id="subActivityLongDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="activityDesc">Activity Name</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <textarea maxlength="255" class="form-control form-control-sm" name="activityDesc" id="activityDesc" rows="4" readonly></textarea>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="startPromo">Activity Period</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <div class="input-group input-group-sm">
                                                    <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromo" id="startPromo" readonly>
                                                    <span class="input-group-text">to</span>
                                                    <label for="endPromo"></label>
                                                    <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromo" id="endPromo" readonly>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="offset-lg-1 col-lg-5 col-12">
                                        <div class="row fv-row">
                                            <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="period">Period</label>
                                            <div class="col-lg-5 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="period" id="period" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="distributorLongDesc">Distributor</label>
                                            <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="distributorLongDesc" id="distributorLongDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="channelDesc">Channel</label>
                                            <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="channelDesc" id="channelDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="subChannelDesc">Sub Channel</label>
                                            <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="subChannelDesc" id="subChannelDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="accountDesc">Account</label>
                                            <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="accountDesc" id="accountDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right" for="subAccountDesc">Sub Account</label>
                                            <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="subAccountDesc" id="subAccountDesc" value="" readonly/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row h-100">
                    <div class="col-12">
                        <div class="card shadow-sm card_mechanism h-100">
                            <div class="card-body">
                                <div class="row h-100">
                                    <div class="col-lg-12 col-12">
                                        <div id="tblMechanismCalculatorFreeText" class="d-none">
                                            <table id="dt_mechanism_calculator" class="table table-row-bordered table-responsive table-sm table-hover">
                                                <thead class="fw-bold fs-6 text-gray-800 bg-optima text-white"></thead>
                                            </table>
                                        </div>
                                        <div id="tblMechanismFreeText">
                                            <table id="dt_mechanism" class="table table-row-bordered table-responsive table-sm table-hover">
                                                <thead class="fw-bold fs-6 text-gray-800 bg-optima text-white"></thead>
                                            </table>
                                        </div>
                                        <div id="tblMechanismList" class="d-none">
                                            <table id="dt_mechanism_list" class="table table-row-bordered table-responsive table-sm table-hover">
                                                <thead class="fw-bold fs-6 text-gray-800 bg-optima text-white">
                                                <tr>
                                                    <th class="align-middle text-nowrap text-white text-center">No</th>
                                                    <th class="align-middle text-nowrap text-white" colspan="5">Mechanism</th>
                                                    <th class="align-middle text-nowrap text-white text-end" colspan="4">Notes</th>
                                                </tr>
                                                <tr>
                                                    <th class="align-middle text-nowrap text-white" style="width: 0"></th>
                                                    <th class="align-middle text-nowrap text-white text-end" style=" width: 11.11%">Baseline</th>
                                                    <th class="align-middle text-nowrap text-white text-end" style=" width: 11.11%">Uplift</th>
                                                    <th class="align-middle text-nowrap text-white text-end" style=" width: 11.11%">Total Sales</th>
                                                    <th class="align-middle text-nowrap text-white text-end" style=" width: 11.11%">Sales Contribution</th>
                                                    <th class="align-middle text-nowrap text-white text-end" style=" width: 11.11%">Stores Coverage</th>
                                                    <th class="align-middle text-nowrap text-white text-end" style=" width: 11.11%">Redemption Rate</th>
                                                    <th class="align-middle text-nowrap text-white text-end" style=" width: 11.11%">CR</th>
                                                    <th class="align-middle text-nowrap text-white text-end" style=" width: 11.11%">ROI</th>
                                                    <th class="align-middle text-nowrap text-white text-end" style=" width: 11.11%">Cost</th>
                                                </tr>
                                                </thead>
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
            <div class="row mb-2">
                <div class="col-12">
                    <div class="card shadow-sm card_budget">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-12">
                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetSourceName">Budget Source</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" name="budgetSourceName" id="budgetSourceName" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="remainingBudget">Remaining Budget</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="remainingBudget" id="remainingBudget" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalCost">Total Cost</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="totalCost" id="totalCost" autocomplete="off" readonly/>
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
                    <div class="card shadow-sm card_region">
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
            <div class="row mb-2">
                <div class="col-12">
                    <div class="card shadow-sm card_sku">
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
            <div class="row">
                <div class="col-12">
                    <div class="card shadow-sm card_attachment">
                        <div class="card-body">
                            <div class="col-12 h-175px" style="overflow-y: auto; overflow-x: hidden">
                                <div class="row mb-3">
                                    <div class="d-flex align-items-center w-lg-85">
                                        <label class="me-5" for="review_file_label_1">1.</label>
                                        <input type="text" class="form-control form-control-sm" id="review_file_label_1" readonly/>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download1" name="btn_download1" value="1" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view1" name="btn_view1" value="1" disabled>
                                            <span class="fa fa-eye"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex align-items-center w-lg-85">
                                        <label class="me-5" for="review_file_label_2">2.</label>
                                        <input type="text" class="form-control form-control-sm" id="review_file_label_2" readonly/>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download2" name="btn_download2" value="2" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view2" name="btn_view2" value="2" disabled>
                                            <span class="fa fa-eye"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex align-items-center w-lg-85">
                                        <label class="me-5" for="review_file_label_3">3.</label>
                                        <input type="text" class="form-control form-control-sm" id="review_file_label_3" readonly/>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download3" name="btn_download3" value="3" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view3" name="btn_view3" value="3" disabled>
                                            <span class="fa fa-eye"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex align-items-center w-lg-85">
                                        <label class="me-5" for="review_file_label_4">4.</label>
                                        <input type="text" class="form-control form-control-sm" id="review_file_label_4" readonly/>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download4" name="btn_download4" value="4" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view4" name="btn_view4" value="4" disabled>
                                            <span class="fa fa-eye"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex align-items-center w-lg-85">
                                        <label class="me-5" for="review_file_label_5">5.</label>
                                        <input type="text" class="form-control form-control-sm" id="review_file_label_5" readonly/>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download5" name="btn_download5" value="5" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view5" name="btn_view5" value="5" disabled>
                                            <span class="fa fa-eye"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex align-items-center w-lg-85">
                                        <label class="me-5" for="review_file_label_6">6.</label>
                                        <input type="text" class="form-control form-control-sm" id="review_file_label_6" readonly/>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download6" name="btn_download6" value="6" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view6" name="btn_view6" value="6" disabled>
                                            <span class="fa fa-eye"> </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex align-items-center w-lg-85">
                                        <label class="me-5" for="review_file_label_7">7.</label>
                                        <input type="text" class="form-control form-control-sm" id="review_file_label_7" readonly/>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download7" name="btn_download7" value="7" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view7" name="btn_view7" value="7" disabled>
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

    <button id="sticky_notes" class="explore-toggle btn btn-sm btn-icon-danger btn-light-danger shadow-sm position-fixed fw-bolder top-25 mt-10 end-0 rounded-end-0" title="Notes">
        <i class="fa fa-comments fs-3 my-3"></i>
    </button>

    <div class="modal modal-sticky-bottom-right" id="modal_notes" data-bs-backdrop="false" data-bs-focus="false" data-bs-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header py-2">
                    <div class="row">
                        <h4 class="modal-title fw-normal text-gray-800">Promo</h4>
                        <div class="mb-0 lh-1">
                            <span class="badge badge-success badge-circle w-8px h-8px me-1"></span>
                            <span class="fs-7 fw-semibold text-muted">Notes</span>
                        </div>
                    </div>
                    <div class="btn btn-icon btn-sm btn-active-light-primary ms-2" data-bs-dismiss="modal" aria-label="Close">
                        <span class="svg-icon svg-icon-1">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="blue"></rect>
                                <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="blue"></rect>
                            </svg>
                        </span>
                    </div>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="d-flex justify-content-start">
                            <div class="d-flex flex-column align-items-start">
                                <div class="p-5 rounded bg-light-primary fw-semibold mw-lg-400px text-start" data-kt-element="message-text" id="notes_message">

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
    <script src="{{ asset('assets/pages/promo/promo-approval-recon/js/promo-approval-form-revamp.js') }}"></script>
@endsection

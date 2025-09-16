@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-creation/css/dc-promo-creation-promo-cancel-request-form-recon.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold my-1 ms-1" id="txt_info_method">Promo ID </small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back-export-pdf')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-2">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_promo_cancel_request" class="form" autocomplete="off">
                        @csrf
                        <div class="row mb-2">
                            <div class="col-lg-6">
                                <small class="fs-5 fw-bold mt-1 ms-1">Notes</small>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <div class="col-lg-6 mb-2">
                                <select class="form-select form-select-sm" data-control="select2" name="cancelReason" id="cancelReason" data-placeholder="Cancellation Reason" autocomplete="off" data-allow-clear="true">
                                    <option></option>
                                </select>
                            </div>
                            <div class="col-lg-6 mb-2">
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_cancel_request">
                                    <span class="indicator-label">
                                        <span class="la la-check"></span> Cancel Request
                                    </span>
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <div class="col-lg-6 mb-2">
                                <textarea type="text" class="form-control form-control-sm d-none" id="reason" name="reason" rows="3" autocomplete="off"></textarea>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-7 col-12 pe-lg-1">
            <div class="row mb-2">
                <div class="col-12">
                    <div class="card shadow-sm card_header">
                        <form id="form_promo" class="form" autocomplete="off">
                            @csrf
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-lg-12 col-12">
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="period">Period</label>
                                            <div class="col-lg-3 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="period" id="period" value="" readonly/>
                                            </div>
                                        </div>
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
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="distributorLongDesc">Distributor</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="distributorLongDesc" id="distributorLongDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityTypeDesc">Sub Activity Type</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="subActivityTypeDesc" id="subActivityTypeDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityLongDesc">Sub Activity</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="subActivityLongDesc" id="subActivityLongDesc" value="" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="channelDesc">Channel</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <input type="text" class="form-control form-control-sm" name="channelDesc" id="channelDesc" value="" readonly/>
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
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="mechanism">Mechanism</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm" name="mechanism" id="mechanism" autocomplete="off" readonly/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotes">Initiator Notes</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm" name="initiatorNotes" id="initiatorNotes" autocomplete="off" readonly/>
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
        <div class="col-lg-5 col-12 ps-lg-1">
            <div class="d-flex h-100 flex-column justify-content-between">
                <div class="row mb-2 flex-shrink-1 h-100">
                    <div class="col-12">
                        <div class="card shadow-sm h-100 card_budget">
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-12">
            <div class="card shadow-sm card_promo_calculator">
                <div class="card-body">
                    <div class="row mb-2">
                        <div class="col-12">
                            <span class="fs-3 fw-bolder mt-1 me-auto">Promo Calculator</span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <div class="d-flex justify-content-between flex-lg-row flex-column">
                                <div class="w-100 mb-2 me-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="baseline">Baseline</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="baseline" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="uplift">Uplift</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="uplift" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="totalSales">Total Sales</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="totalSales" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="salesContribution">Sales Contribution</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="salesContribution" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="storesCoverage">Stores Coverage</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="storesCoverage" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="redemptionRate">Redemption Rate</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="redemptionRate" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="cr">CR</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="cr" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="roi">ROI</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black form-control-solid-bg border-bg-optima" id="roi" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="cost">Cost</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="cost" readonly>
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
    <script src="{{ asset('assets/pages/promo/promo-creation/js/dc-promo-creation-form-cancel-request-recon.js?v=3') }}"></script>
@endsection

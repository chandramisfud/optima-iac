@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-send-back/css/dc-promo-send-back-form-recon.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method">Edit</small>
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
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="period">Period</label>
                                            <div class="col-lg-3 col-md-12 col-sm-12 col-12">
                                                <div class="input-group input-group-sm" id="dialer_period">
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="period" id="period" value="{{ @date('Y') }}"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="entityId">Entity</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="entityId" id="entityId" data-placeholder="Select an Entity"  data-allow-clear="true" autofocus>
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="groupBrandId">Brand</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="groupBrandId" id="groupBrandId" data-placeholder="Select a Brand"  data-allow-clear="true">
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="distributorId">Distributor</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="distributorId" id="distributorId" data-placeholder="Select a Distributor"  data-allow-clear="true">
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subActivityTypeId">Sub Activity Type</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="subActivityTypeId" id="subActivityTypeId" data-placeholder="Select a Sub Activity Type"  data-allow-clear="true">
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subActivityId">Sub Activity</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="subActivityId" id="subActivityId" data-placeholder="Select a Sub Activity"  data-allow-clear="true">
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="channelId">Channel</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="channelId" id="channelId" data-placeholder="Select a Channel"  data-allow-clear="true">
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="startPromo">Activity Period</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                <div class="input-group input-group-sm">
                                                    <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromo" id="startPromo">
                                                    <span class="input-group-text">to</span>
                                                    <label for="endPromo"></label>
                                                    <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromo" id="endPromo">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="mechanism">Mechanism</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm" name="mechanism" id="mechanism" autocomplete="off"/>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotes">Initiator Notes</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm" name="initiatorNotes" id="initiatorNotes" autocomplete="off"/>
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

                                    <div class="row fv-row">
                                        <div class="d-flex align-items-start">
                                            <label class="col-form-label me-5">1.</label>
                                            <div class="input-group input-group-sm w-70 custom-file-button">
                                                <label class="input-group-text fs-12px" for="attachment1" role="button">Browse...</label>
                                                <label for="attachment1" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_1" role="button"></label>
                                                <input type="file" class="d-none input_file" id="attachment1" name="attachment1" data-row="1">
                                            </div>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete1" value="1" disabled>
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download1" name="btn_download1" value="1" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <div class="invisible mt-2" id="info1">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <div class="d-flex align-items-start">
                                            <label class="col-form-label me-5">2.</label>
                                            <div class="input-group input-group-sm w-70 custom-file-button">
                                                <label class="input-group-text fs-12px" for="attachment2" role="button">Browse...</label>
                                                <label for="attachment2" class="form-control form-control-sm text-nowrap text-truncate text-gray-700 fs-12px review_file_label me-3" id="review_file_label_2" role="button"></label>
                                                <input type="file" class="d-none input_file" id="attachment2" name="attachment2" data-row="2">
                                            </div>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete2" value="2" disabled>
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download2" name="btn_download2" value="2" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <div class="invisible mt-2" id="info2">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <div class="d-flex align-items-center">
                                            <label class="col-form-label me-5">3.</label>
                                            <div class="input-group input-group-sm w-70 custom-file-button">
                                                <label class="input-group-text fs-12px" for="attachment3" role="button">Browse...</label>
                                                <label for="attachment3" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_3" role="button"></label>
                                                <input type="file" class="d-none input_file" id="attachment3" name="attachment3" data-row="3">
                                            </div>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete3" value="3" disabled>
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download3" name="btn_download3" value="3" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <div class="invisible mt-2" id="info3">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <div class="d-flex align-items-center">
                                            <label class="col-form-label me-5">4.</label>
                                            <div class="input-group input-group-sm w-70 custom-file-button">
                                                <label class="input-group-text fs-12px" for="attachment4" role="button">Browse...</label>
                                                <label for="attachment4" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_4" role="button"></label>
                                                <input type="file" class="d-none input_file" id="attachment4" name="attachment4" data-row="4">
                                            </div>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete4" value="4" disabled>
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download4" name="btn_download4" value="4" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <div class="invisible mt-2" id="info4">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
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
    <div class="row mb-2">
        <div class="col-12">
            <div class="card shadow-sm card_promo_calculator_before">
                <div class="card-body">
                    <div class="row mb-2">
                        <div class="col-12">
                            <span class="fs-3 fw-bolder mt-1 me-auto">Promo Calculator - Creation</span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <div class="d-flex justify-content-between flex-lg-row flex-column">
                                <div class="w-100 mb-2 me-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="baselineBefore">Baseline</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="baselineBefore" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="upliftBefore">Uplift</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="upliftBefore" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="totalSalesBefore">Total Sales</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="totalSalesBefore" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="salesContributionBefore">Sales Contribution</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="salesContributionBefore" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="storesCoverageBefore">Stores Coverage</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="storesCoverageBefore" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="redemptionRateBefore">Redemption Rate</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="redemptionRateBefore" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="crBefore">CR</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="crBefore" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="roiBefore">ROI</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black form-control-solid-bg border-bg-optima" id="roiBefore" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="costBefore">Cost</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="costBefore" readonly>
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
                            <span class="fs-3 fw-bolder mt-1 me-auto">Promo Calculator - Reconciliation</span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <div class="d-flex justify-content-between flex-lg-row flex-column">
                                <div class="w-100 mb-2 me-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="baseline">Baseline</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="baseline">
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="uplift">Uplift</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="uplift">
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="totalSales">Total Sales</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="totalSales">
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="salesContribution">Sales Contribution</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="salesContribution">
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="storesCoverage">Stores Coverage</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="storesCoverage">
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="redemptionRate">Redemption Rate</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="redemptionRate">
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="cr">CR</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="cr">
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="roi">ROI</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black form-control-solid-bg border-bg-optima" id="roi" readonly>
                                </div>
                                <div class="w-100 mb-2 mx-1">
                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="cost">Cost Recon</label>
                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="cost">
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

@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/promo-send-back/js/dc-promo-send-back-form-recon.js?v=19') }}"></script>
@endsection

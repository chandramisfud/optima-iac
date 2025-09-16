@extends('layouts/layoutMaster')

@section('title', 'Promo Approval')

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-approval-recon/css/promo-approval-recon-form.css?v=' . microtime()) }}" rel="stylesheet" type="text/css"/>
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
    <div class="row">
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
                                    <label class="col-lg-4 col-form-label pt-0">Last Sendback Notes <br> <span id="txt_senback_notes"></span></label>
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
                    <div class="row mb-5">
                        <div class="col-12">
                            <div class="separator border-1 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-12">
                            <h5 class="fw-normal">Detail</h5>
                            <div class="separator border-2 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-md-6 col-12">
                            <div class="row">
                                <label class="col-md-4 col-form-label">Budget Year</label>
                                <div class="col-md-3">
                                    <input type="text" class="form-control form-control-sm" name="period" id="period" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Budget Source</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm" name="allocationRefId" id="allocationRefId" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Activity</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm" name="activityLongDesc" id="activityLongDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Sub Activity</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm" name="subActivityLongDesc" id="subActivityLongDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row mb-1">
                                <label class="col-md-4 col-form-label">Activity Name</label>
                                <div class="col-md-8">
                                    <textarea maxlength="255" class="form-control form-control-sm" name="activityDesc" id="activityDesc" autocomplete="off" readonly></textarea>
                                </div>
                            </div>
                            <div class="row mb-2">
                                <label class="col-md-4 col-form-label">Activity Period</label>
                                <div class="col-md-8">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromo" id="startPromo" autocomplete="off" readonly>
                                        <span class="input-group-text">to</span>
                                        <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromo" id="endPromo" autocomplete="off" readonly>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6 col-12">
                            <div class="row">
                                <label class="col-md-4 col-form-label">Sub Category</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm" name="subCategoryDesc" id="subCategoryDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Entity</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm" name="principalName" id="principalName" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Budget Allocation</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm" name="allocationDesc" id="allocationDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">TS Code</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm" name="tsCoding" id="tsCoding" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Budget Amount</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="budgetAmount" id="budgetAmount" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Remaining Budget</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="remainingBudget" id="remainingBudget" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row mb-1">
                                <label class="col-md-4 col-form-label">Initiator Notes</label>
                                <div class="col-md-8">
                                    <textarea maxlength="255" class="form-control form-control-sm" name="initiator_notes" id="initiator_notes" autocomplete="off" readonly></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-5">
                        <div class="col-12">
                            <div class="separator border-1 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-12">
                            <h5 class="fw-normal" id="txt_promoPlanRefId">Current</h5>
                            <div class="separator border-2 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-md-6 col-12">
                            <div class="row">
                                <label class="col-md-4 col-form-label">Baseline Sales</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="planNormalSales" id="planNormalSales" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Incremental Sales</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="planIncrSales" id="planIncrSales" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Total Sales</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="planTotalSales" id="planTotalSales" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Total Investment</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="planInvestment" id="planInvestment" autocomplete="off" readonly/>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 col-12">
                            <div class="row">
                                <label class="col-md-4 col-form-label">ROI</label>
                                <div class="col-md-8">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm text-end" name="planRoi" id="planRoi" autocomplete="off" readonly/>
                                        <span class="input-group-text">%</span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Cost Ratio</label>
                                <div class="col-md-8">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm text-end" name="planCostRatio" id="planCostRatio" autocomplete="off" readonly/>
                                        <span class="input-group-text">%</span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Total DN Claimed</label>
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="d-flex">
                                            <input class="form-control form-control-sm form-control-solid-bg me-3 text-end" type="text" name="totalClaim" id="totalClaim" autocomplete="off" readonly/>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0" id="btn_list_dn_claim" title="List DN Claimed">
                                                <span class="indicator-label">
                                                    <span class="fa fa-list fs-5"></span>
                                                     DN List
                                                </span>
                                                <span class="indicator-progress">
                                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                                </span>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Total DN Paid</label>
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="d-flex">
                                            <input class="form-control form-control-sm form-control-solid-bg me-3 text-end" type="text" name="totalPaid" id="totalPaid" autocomplete="off" readonly/>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0" id="btn_list_dn_paid" title="List DN Paid">
                                               <span class="indicator-label">
                                                    <span class="fa fa-list fs-5"></span>
                                                     DN List
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
                    <div class="row mb-5">
                        <div class="col-12">
                            <div class="separator border-1 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-12">
                            <h5 class="fw-normal">Previous</h5>
                            <div class="separator border-2 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-md-6 col-12">
                            <div class="row">
                                <label class="col-md-4 col-form-label">Baseline Sales</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="normalSales" id="normalSales" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Incremental Sales</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="incrSales" id="incrSales" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Total Sales</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="prevTotSales" id="prevTotSales" autocomplete="off" readonly/>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 col-12">
                            <div class="row">
                                <label class="col-md-4 col-form-label">Total Investment</label>
                                <div class="col-md-8">
                                    <input type="text" class="form-control form-control-sm text-end" name="investment" id="investment" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">ROI</label>
                                <div class="col-md-8">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm text-end" name="roi" id="roi" autocomplete="off" readonly/>
                                        <span class="input-group-text">%</span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 col-form-label">Cost Ratio</label>
                                <div class="col-md-8">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm text-end" name="costRatio" id="costRatio" autocomplete="off" readonly/>
                                        <span class="input-group-text">%</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-5">
                        <div class="col-12">
                            <div class="separator border-1 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-12">
                            <h5 class="fw-normal">Detail Attribute</h5>
                            <div class="separator border-2 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-md-3 col-12">
                            <label class="mb-1">Channel</label>
                            <input type="text" class="form-control form-control-sm" name="channel" id="channel" autocomplete="off" readonly/>
                        </div>
                        <div class="col-md-3 col-12">
                            <label class="mb-1">Sub Channel</label>
                            <input type="text" class="form-control form-control-sm" name="subChannels" id="subChannels" autocomplete="off" readonly/>
                        </div>
                        <div class="col-md-3 col-12">
                            <label class="mb-1">Account</label>
                            <input type="text" class="form-control form-control-sm" name="accounts" id="accounts" autocomplete="off" readonly/>
                        </div>
                        <div class="col-md-3 col-12">
                            <label class="mb-1">Sub Account</label>
                            <input type="text" class="form-control form-control-sm" name="subAccounts" id="subAccounts" autocomplete="off" readonly/>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-4">
                            <div class="card card-flush mb-2" id="card_region">
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

                        {{-- CARD BRAND --}}
                        <div class="col-lg-4">
                            <div class="card card-flush mb-2" id="card_brand">
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

                        {{-- CARD SKU --}}
                        <div class="col-lg-4">
                            <div class="card card-flush mb-2" id="card_sku">
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
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_mechanism" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                    </div>

                    <div class="row mb-5">
                        <div class="col-12">
                            <div class="separator border-1 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-12">
                            <h5 class="fw-normal">File Attachments</h5>
                            <div class="separator border-2 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-5 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex align-items-center w-lg-85">
                                    <label class="me-5">1.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="review_file_label_1" id="review_file_label_1" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" id="btn_download1" title="Download" value="1" disabled>
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-3" id="btn_view1" title="Preview" value="1" disabled>
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center w-lg-85">
                                    <label class="me-5">2.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="review_file_label_2" id="review_file_label_2" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" id="btn_download2" title="Download" value="2" disabled>
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-3" id="btn_view2" title="Preview" value="2" disabled>
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center w-lg-85">
                                    <label class="me-5">3.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="review_file_label_3" id="review_file_label_3" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" id="btn_download3" title="Download" value="3" disabled>
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-3" id="btn_view3" title="Preview" value="3" disabled>
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center w-lg-85">
                                    <label class="me-5">4.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="review_file_label_4" id="review_file_label_4" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" id="btn_download4" title="Download" value="4" disabled>
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-3" id="btn_view4" title="Preview" value="4" disabled>
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-5 offset-lg-1 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex align-items-center w-lg-85">
                                    <label class="me-5">5.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="review_file_label_5" id="review_file_label_5" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" id="btn_download5" title="Download" value="5" disabled>
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-3" id="btn_view5" title="Preview" value="5" disabled>
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center w-lg-85">
                                    <label class="me-5">6.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="review_file_label_6" id="review_file_label_6" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" id="btn_download6" title="Download" value="6" disabled>
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-3" id="btn_view6" title="Preview" value="6" disabled>
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center w-lg-85">
                                    <label class="me-5">7.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="review_file_label_7" id="review_file_label_7" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" id="btn_download7" title="Download" value="7" disabled>
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-3" id="btn_view7" title="Preview" value="7" disabled>
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
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

    @include('Promo::promo-approval-recon.list-dn')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-approval-recon/js/promo-approval-recon-form.js?v=6') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-approval-recon/js/dn-list.js?v=1') }}"></script>
@endsection

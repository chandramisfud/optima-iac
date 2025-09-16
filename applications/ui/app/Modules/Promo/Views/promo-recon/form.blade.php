@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-recon/css/promo-recon-form.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold my-1 ms-1" id="txt_info_method">Edit </small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-save-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-5">
        <div class="col-md-12 col-12">
            <form id="form_promo" class="form" autocomplete="off">
                @csrf
                <div class="card shadow-sm mb-3 card_form">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="period">Budget Year</label>
                                    <div class="col-lg-2 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="period" id="period" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="promoPlanRefId">Promo Planning</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="promoPlanRefId" name="promoPlanRefId" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationRefId">Budget Source</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm has-validation">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="allocationRefId" name="allocationRefId" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly="">
                                            <span class="input-group-text cursor-pointer btn-outline-optima" id="btn_search_budget">
                                                <span class="fa fa-search"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationDesc">Budget Allocation</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="allocationDesc" name="allocationDesc" autocomplete="off" readonly />
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="activityId">Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12" id="dynamicElActivity">
                                        <select class="form-select form-select-sm" data-control="select2" name="activityId" id="activityId" data-placeholder="Select an Activity"  data-allow-clear="true" tabindex="1">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subActivityId">Sub Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12" id="dynamicElSubActivity">
                                        <select class="form-select form-select-sm" data-control="select2" name="subActivityId" id="subActivityId" data-placeholder="Select a Sub Activity"  data-allow-clear="true" tabindex="2">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="startPromo">Activity Period</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm" name="startPromo" id="startPromo" autocomplete="off" />
                                            <span class="input-group-text">to</span>
                                            <input type="text" class="form-control form-control-sm" name="endPromo" id="endPromo" autocomplete="off" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="activityDesc">Activity Name</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <textarea maxlength="255" class="form-control form-control-sm" name="activityDesc" id="activityDesc" autocomplete="off" tabindex="3"></textarea>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="tsCoding">TS Code</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="tsCoding" name="tsCoding" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subCategoryId">Sub Category</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <select class="form-select form-select-sm" data-control="select2" name="subCategoryId" id="subCategoryId" data-placeholder="Select a Sub Cateogry"  data-allow-clear="true">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="entity">Entity</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="entity" name="entity" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="distributor">Distributor</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="distributor" name="distributor" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetAmount">Budget Deployed</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="budgetAmount" name="budgetAmount" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="remainingBudget">Remaining Budget</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="remainingBudget" name="remainingBudget" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotes">Initiator Notes</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <textarea maxlength="255" class="form-control form-control-sm" name="initiatorNotes" id="initiatorNotes" autocomplete="off" tabindex="4"></textarea>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="investmentType">Investment Type</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="investmentType" name="investmentType" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="separator border-2 my-2 border-secondary"></div>
                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="baselineSales">Baseline Sales</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="baselineSales" id="baselineSales" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="incrementSales">Sales Increment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" id="incrementSales" name="incrementSales" autocomplete="off" value="0" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="investment">Investment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" id="investment" name="investment" autocomplete="off" value="0" readonly/>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalSales">Total Sales</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="totalSales" name="totalSales" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalInvestment">Total Investment</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="totalInvestment" name="totalInvestment" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="roi">ROI</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="roi" name="roi" autocomplete="off" readonly/>
                                            <span class="input-group-text" id="addon-roi">%</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="costRatio">Cost Ratio</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="costRatio" name="costRatio" autocomplete="off" readonly/>
                                            <span class="input-group-text" id="addon-cr">%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-12 col-12">
                        <div class="card shadow-sm card_recon">
                            <div class="card-body">
                                <div class="d-flex py-1 flex-grow-1 justify-content-between">
                                    <span class="fw-bold text-gray-700 fs-4">Reconciliation</span>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-12">
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="baselineSalesRecon">Baseline Sales</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <div class="input-group input-group-sm">
                                                    <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="baselineSalesRecon" id="baselineSalesRecon" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                                    <span class="input-group-text" id="btn_get_baseline_sales" title="Get baseline sales" tabindex="5">
                                                        <span class="fa fa-sync"></span>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row fv-row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="incrementSalesRecon">Increment Sales</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm text-end" id="incrementSalesRecon" name="incrementSalesRecon" autocomplete="off" value="0" inputmode="decimal" tabindex="6"/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="totalSalesRecon">Total Sales</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm text-end" id="totalSalesRecon" name="totalSalesRecon" autocomplete="off" value="0" readonly/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="estimatedInvestment">Estimated Investment</label>
                                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm text-end" id="estimatedInvestment" name="estimatedInvestment" autocomplete="off" value="0" readonly/>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-6 col-12">
                                        <div class="row fv-row">
                                            <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Final Investment Amount</label>
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm text-end" id="investmentRecon" name="investmentRecon" autocomplete="off" value="0" inputmode="decimal" tabindex="7"/>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="costRatioRecon">Post Cost Ratio</label>
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                                <div class="input-group input-group-sm">
                                                    <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="costRatioRecon" name="costRatioRecon" autocomplete="off" readonly/>
                                                    <span class="input-group-text" id="addon-cr">%</span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="roiRecon">Post ROI</label>
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                                <div class="input-group input-group-sm">
                                                    <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="roiRecon" name="roiRecon" autocomplete="off" readonly/>
                                                    <span class="input-group-text" id="addon-roi">%</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="actualDNClaimed">Actual DN Claimed</label>
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="actualDNClaimed" name="actualDNClaimed" autocomplete="off" readonly/>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-lg-12 col-12">
                        <div class="card shadow-sm card_attribute">
                            <div class="card-body">
                                <div class="d-flex py-1 flex-grow-1 justify-content-between">
                                    <span class="fw-bold text-gray-700 fs-4">Detail Attribute</span>
                                    <div class="d-flex align-items-center py-1">
                                        <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_attribute" tabindex="8">
                                        <span class="indicator-label">
                                            <span class="fa fa-edit"></span> Attributes
                                        </span>
                                            <span class="indicator-progress">
                                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                        </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="separator border-2 my-1 border-secondary"></div>
                                <div class="row mb-5">
                                    <div class="col-lg-3 col-12">
                                        <div class="row fv-row">
                                            <label class="col-form-label col-lg-12 col-12 pb-1 required" for="channelId">Channel</label>
                                            <div class="col-lg-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="channelId" id="channelId" data-placeholder="Select a Channel"  data-allow-clear="true" tabindex="13">
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-12">
                                        <div class="row fv-row">
                                            <label class="col-form-label col-lg-12 col-12 pb-1 required" for="subChannelId">Sub Channel</label>
                                            <div class="col-lg-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="subChannelId" id="subChannelId" data-placeholder="Select a Sub Channel"  data-allow-clear="true" tabindex="14">
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-12">
                                        <div class="row fv-row">
                                            <label class="col-form-label col-lg-12 col-12 pb-1 required" for="accountId">Account</label>
                                            <div class="col-lg-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="accountId" id="accountId" data-placeholder="Select an Account"  data-allow-clear="true" tabindex="15">
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-12">
                                        <div class="row fv-row">
                                            <label class="col-form-label col-lg-12 col-12 pb-1 required" for="subAccountId">Sub Account</label>
                                            <div class="col-lg-12 col-12">
                                                <select class="form-select form-select-sm" data-control="select2" name="subAccountId" id="subAccountId" data-placeholder="Select a Sub Account"  data-allow-clear="true" tabindex="16">
                                                    <option></option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
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

                                    <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                        <table id="dt_mechanism" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </form>

            <div class="row card_attachment">
                <div class="col-md-12 col-12">
                    <div class="card shadow-sm">
                        <div class="card-body">
                            <div class="d-flex py-1 flex-grow-1 justify-content-between">
                                <span class="fw-bold text-gray-700 fs-4">File attachments</span>
                            </div>
                            <div class="separator border-2 my-2 border-secondary"></div>
                            <div class="row">
                                <div class="col-lg-5 col-md-12 col-12">
                                    <div class="row fv-row">
                                        <div class="d-flex align-items-start">
                                            <label class="col-form-label me-5">1.</label>
                                            <div class="input-group input-group-sm w-70 custom-file-button">
                                                <label class="input-group-text fs-12px" for="attachment1" role="button">Browse...</label>
                                                <label for="attachment1" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_1" role="button">For SKP Draft</label>
                                                <input type="file" class="d-none input_file" id="attachment1" name="attachment1" data-row="1">
                                            </div>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete1" value="1" disabled>
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download1" name="btn_download1" value="1" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <div class="invisible" id="info1">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <div class="d-flex align-items-start">
                                            <label class="col-form-label me-5">2.</label>
                                            <div class="input-group input-group-sm w-70 custom-file-button">
                                                <label class="input-group-text fs-12px" for="attachment2" role="button">Browse...</label>
                                                <label for="attachment2" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_2" role="button">For SKP Fully Approved</label>
                                                <input type="file" class="d-none input_file" id="attachment2" name="attachment2" data-row="2">
                                            </div>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete2" value="2" disabled>
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download2" name="btn_download2" value="2" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <div class="invisible" id="info2">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="d-flex align-items-start">
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
                                            <div class="invisible" id="info3">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="d-flex align-items-start">
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
                                            <div class="invisible" id="info4">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="offset-lg-1 col-lg-5 col-md-12 col-12">
                                    <div class="row">
                                        <div class="d-flex align-items-start">
                                            <label class="col-form-label me-5">5.</label>
                                            <div class="input-group input-group-sm w-70 custom-file-button">
                                                <label class="input-group-text fs-12px" for="attachment5" role="button">Browse...</label>
                                                <label for="attachment5" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_5" role="button"></label>
                                                <input type="file" class="d-none input_file" id="attachment5" name="attachment5" data-row="5">
                                            </div>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete5" value="5" disabled>
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download5" name="btn_download5" value="5" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <div class="invisible" id="info5">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="d-flex align-items-start">
                                            <label class="col-form-label me-5">6.</label>
                                            <div class="input-group input-group-sm w-70 custom-file-button">
                                                <label class="input-group-text fs-12px" for="attachment6" role="button">Browse...</label>
                                                <label for="attachment6" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_6" role="button"></label>
                                                <input type="file" class="d-none input_file" id="attachment6" name="attachment6" data-row="6">
                                            </div>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete6" value="6" disabled>
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download6" name="btn_download6" value="6" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <div class="invisible" id="info6">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="d-flex align-items-start">
                                            <label class="col-form-label me-5">7.</label>
                                            <div class="input-group input-group-sm w-70 custom-file-button">
                                                <label class="input-group-text fs-12px" for="attachment7" role="button">Browse...</label>
                                                <label for="attachment7" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_7" role="button"></label>
                                                <input type="file" class="d-none input_file" id="attachment7" name="attachment7" data-row="7">
                                            </div>
                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete7" value="7" disabled>
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download7" name="btn_download7" value="7" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <div class="invisible" id="info7">
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
    @include('Promo::promo-recon.source-budget-list')
    @include('Promo::promo-recon.form-attribute')
@endsection


@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-recon/js/promo-recon-form.js?v=30') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-recon/js/source-budget-list.js?v=1') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-recon/js/promo-recon-form-attribute.js?v=17') }}"></script>
@endsection


@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-creation/css/promo-creation-form.css') }}" rel="stylesheet" type="text/css"/>
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
    <form id="form_promo" class="form" autocomplete="off">
        @csrf
        <div class="row mb-5 card_form">
            <div class="col-md-12 col-12">
                {{-- Form Promo --}}
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="row">
                            {{-- Form Promo Left--}}
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="period">Budget Year</label>
                                    <div class="col-lg-4 col-md-6 col-sm-12 col-12">
                                        <div class="input-group input-group-sm" id="dialer_period">
                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                <i class="fa fa-minus fs-2"></i>
                                            </button>

                                            <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="period" id="period" value="{{ @date('Y') }}" autocomplete="off" tabindex="0"/>

                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                <i class="fa fa-plus fs-2"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="promoPlanningRefId">Promo Planning</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm has-validation">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="promoPlanningRefId" name="promoPlanningRefId" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly="">
                                            <span class="input-group-text cursor-pointer btn-outline-optima" id="btn_search_promo_planning">
                                                <span class="fa fa-search"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="allocationRefId">Budget Source</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm has-validation">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="allocationRefId" name="allocationRefId" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly="">
                                            <span class="input-group-text cursor-pointer btn-outline-optima" id="btn_search_budget">
                                                <span class="fa fa-search"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationDesc">Budget Allocation</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="allocationDesc" id="allocationDesc" autocomplete="off" readonly />
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="activityId">Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="activityId" id="activityId" data-placeholder="Select an Activity"  data-allow-clear="true" tabindex="3">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subActivityId">Sub Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subActivityId" id="subActivityId" data-placeholder="Select a Sub Activity"  data-allow-clear="true" tabindex="4">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="startPromo">Activity Period</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromo" id="startPromo" autocomplete="off" tabindex="5">
                                            <span class="input-group-text">to</span>
                                            <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromo" id="endPromo" autocomplete="off" tabindex="6">
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="activityDesc">Activity Name</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <textarea maxlength="255" class="form-control form-control-sm" name="activityDesc" id="activityDesc" tabindex="7" autocomplete="off"></textarea>
                                    </div>
                                </div>
                            </div>

                            {{-- Form Promo Plannning Right--}}
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="tsCode">TS Code</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="tsCode" id="tsCode" autocomplete="off" readonly />
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subCategoryId">Sub Category</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subCategoryId" id="subCategoryId" data-placeholder="Select a Sub Category"  data-allow-clear="true" tabindex="8">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="entity">Entity</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="entity" id="entity" autocomplete="off" readonly />
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="distributor">Distributor</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="distributor" id="distributor" autocomplete="off" readonly />
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetDeployed">Budget Deployed</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetDeployed" id="budgetDeployed" autocomplete="off" inputmode="decimal" readonly />
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetRemaining">Remaining Budget</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetRemaining" id="budgetRemaining" autocomplete="off" inputmode="decimal" readonly />
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotes">Initiator Notes</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <textarea maxlength="255" class="form-control form-control-sm mb-2" name="initiatorNotes" id="initiatorNotes" tabindex="9" autocomplete="off"></textarea>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="investmentType">Investment Type</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="investmentType" id="investmentType" autocomplete="off" readonly />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row d-none">
                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="col-lg-3 col-form-label "></label>
                                    <div class="col-lg-9 " id='show_desc'></div>
                                </div>
                            </div>
                        </div>

                        <div class="separator border-3 my-2 border-secondary"></div>
                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="baselineSales">Baseline Sales</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="baselineSales" id="baselineSales" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                            <span class="input-group-text" id="btn_get_baseline_sales" title="Get baseline sales" tabindex="10">
                                                <span class="fa fa-sync"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="incrementSales">Sales Increment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="incrementSales" id="incrementSales" autocomplete="off" value="0" inputmode="decimal" onClick="this.select();" tabindex="11"/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="investment">Investment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="investment" id="investment" autocomplete="off" value="0" inputmode="decimal" onClick="this.select();" tabindex="12"/>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalSales">Total Sales</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="totalSales" id="totalSales" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalInvestment">Total Investment</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="totalInvestment" id="totalInvestment" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="roi">ROI</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="roi" id="roi" autocomplete="off" value="0" inputmode="decimal" onClick="this.select();" readonly/>
                                            <span class="input-group-text" id="addoncr">%</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="costRatio">Cost Ratio</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="costRatio" id="costRatio" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                            <span class="input-group-text" id="addoncr">%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-5 card_attribute">
            <div class="col-lg-12 col-12">
                {{-- Detail Attribute --}}
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex py-1 flex-grow-1 justify-content-between">
                            <span class="fw-bold text-gray-700 fs-4">Detail Attribute</span>
                            <div class="d-flex align-items-center py-1">
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_attribute" tabindex="17">
                                    <span class="indicator-label">
                                        <span class="fa fa-edit"></span> Attributes
                                    </span>
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                            </div>
                        </div>
                        <div class="separator border-3 my-2 border-secondary"></div>
                        <div class="row mb-3">
                            <div class="col-lg-3">
                                <div class="row fv-row">
                                    <label class="col-form-label col-lg-12 required" for="channelId">Channel</label>
                                    <div class="col-lg-12">
                                            <select class="form-select form-select-sm" data-control="select2" name="channelId" id="channelId" data-placeholder="Select a Channel"  data-allow-clear="true" tabindex="13">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="row fv-row">
                                    <label class="col-form-label col-lg-12 required" for="subChannelId">Sub Channel</label>
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subChannelId" id="subChannelId" data-placeholder="Select a Sub Channel"  data-allow-clear="true" tabindex="14">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="row fv-row">
                                    <label class="col-form-label col-lg-12 required" for="accountId">Account</label>
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="accountId" id="accountId" data-placeholder="Select an Account"  data-allow-clear="true" tabindex="15">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="row fv-row">
                                    <label class="col-form-label col-lg-12 required" for="subAccountId">Sub Account</label>
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subAccountId" id="subAccountId" data-placeholder="Select a Sub Account"  data-allow-clear="true" tabindex="16">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {{-- CARD REGION --}}
                        <div class="row">
                            <div class="col-lg-4">
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

                            {{-- CARD BRAND --}}
                            <div class="col-lg-4">
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

                            {{-- CARD SKU --}}
                            <div class="col-lg-4">
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
                                        <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete1" title="Delete" value="1" disabled>
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
                                            <label for="attachment2" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_2" role="button">For SKP Fully Approved</label>
                                            <input type="file" class="d-none input_file" id="attachment2" name="attachment2" data-row="2">
                                        </div>
                                        <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete2" title="Delete" value="2" disabled>
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

                                <div class="row">
                                    <div class="d-flex align-items-start">
                                        <label class="col-form-label me-5">3.</label>
                                        <div class="input-group input-group-sm w-70 custom-file-button">
                                            <label class="input-group-text fs-12px" for="attachment3" role="button">Browse...</label>
                                            <label for="attachment3" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_3" role="button"></label>
                                            <input type="file" class="d-none input_file" id="attachment3" name="attachment3" data-row="3">
                                        </div>
                                        <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete3" title="Delete" value="3" disabled>
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

                                <div class="row">
                                    <div class="d-flex align-items-start">
                                        <label class="col-form-label me-5">4.</label>
                                        <div class="input-group input-group-sm w-70 custom-file-button">
                                            <label class="input-group-text fs-12px" for="attachment4" role="button">Browse...</label>
                                            <label for="attachment4" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_4" role="button"></label>
                                            <input type="file" class="d-none input_file" id="attachment4" name="attachment4" data-row="4">
                                        </div>
                                        <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete4" title="Delete" value="4" disabled>
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
                            <div class="offset-lg-1 col-lg-5 col-md-12 col-12">
                                <div class="row">
                                    <div class="d-flex align-items-start">
                                        <label class="col-form-label me-5">5.</label>
                                        <div class="input-group input-group-sm w-70 custom-file-button">
                                            <label class="input-group-text fs-12px" for="attachment5" role="button">Browse...</label>
                                            <label for="attachment5" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_5" role="button"></label>
                                            <input type="file" class="d-none input_file" id="attachment5" name="attachment5" data-row="5">
                                        </div>
                                        <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete5" title="Delete" value="5" disabled>
                                            <span class="fa fa-trash-alt"> </span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download5" name="btn_download5" value="5" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <div class="invisible mt-2" id="info5">
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
                                        <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete6" title="Delete" value="6" disabled>
                                            <span class="fa fa-trash-alt"> </span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download6" name="btn_download6" value="6" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <div class="invisible mt-2" id="info6">
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
                                        <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete7" title="Delete" value="7" disabled>
                                            <span class="fa fa-trash-alt"> </span>
                                        </button>
                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download7" name="btn_download7" value="7" disabled>
                                            <span class="fa fa-download"></span>
                                        </button>
                                        <div class="invisible mt-2" id="info7">
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
    </form>
    @include('Promo::promo-creation.source-planning-list')
    @include('Promo::promo-creation.source-budget-list')
    @include('Promo::promo-creation.form-attribute')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/js/generate-uuid.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/promo-creation/js/promo-creation-form.js?v=23') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-creation/js/source-planning-list.js?v=5') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-creation/js/source-budget-list.js?v=3') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-creation/js/promo-creation-form-attribute.js?v=9') }}"></script>
@endsection

@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-send-back/css/promo-send-back-form.css') }}" rel="stylesheet" type="text/css"/>
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
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="period">Budget Year</label>
                                        <div class="col-lg-2 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="period" id="period" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="promoPlanRefId">Promo Planning</label>
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
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <select class="form-select form-select-sm" data-control="select2" name="activityId" id="activityId" data-placeholder="Select an Activity"  data-allow-clear="true" tabindex="1">
                                                <option></option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subActivityId">Sub Activity</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <select class="form-select form-select-sm" data-control="select2" name="subActivityId" id="subActivityId" data-placeholder="Select a Sub Activity"  data-allow-clear="true" tabindex="2">
                                                <option></option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Activity Period</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <div class="input-group input-group-sm">
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" name="startPromo" id="startPromo" autocomplete="off" readonly/>
                                                <span class="input-group-text">to</span>
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" name="endPromo" id="endPromo" autocomplete="off" readonly/>
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
                                    <div class="row">
                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subCategoryDesc">Sub Category</label>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="subCategoryDesc" name="subCategoryDesc" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entityDesc">Entity</label>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="entityDesc" name="entity" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="distributorDesc">Distributor</label>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="distributorDesc" name="distributorDesc" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetDeployed">Budget Deployed</label>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="budgetDeployed" name="budgetDeployed" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetRemaining">Remaining Budget</label>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="budgetRemaining" name="budgetRemaining" autocomplete="off" readonly/>
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
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="baselineSales">Baseline Sales</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <div class="input-group input-group-sm">
                                                <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="baselineSales" id="baselineSales" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="incrementSales">Sales Increment</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm text-end" id="incrementSales" name="incrementSales" autocomplete="off" value="0" tabindex="6"/>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="investment">Investment</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm text-end" id="investment" name="investment" autocomplete="off" value="0" tabindex="7"/>
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

                    <div class="row mb-3 card_attribute">
                        <div class="col-lg-12 col-12">
                            <div class="card shadow-sm">
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
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">1.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment1" role="button">Browse...</label>
                                                    <label for="attachment1" class="form-control form-control-sm text-gray-600 fs-12px review_file_label me-3" id="review_file_label_1" role="button">For SKP Draft</label>
                                                    <input type="file" class="d-none input_file" id="attachment1" name="attachment1" data-row="1">
                                                </div>
                                                <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete1" title="Delete" value="1" disabled>
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
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">2.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment2" role="button">Browse...</label>
                                                    <label for="attachment2" class="form-control form-control-sm text-gray-600 fs-12px review_file_label me-3" id="review_file_label_2" role="button">For SKP Fully Approved</label>
                                                    <input type="file" class="d-none input_file" id="attachment2" name="attachment2" data-row="2">
                                                </div>
                                                <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete2" title="Delete" value="2" disabled>
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
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">3.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment3" role="button">Browse...</label>
                                                    <label for="attachment3" class="form-control form-control-sm text-gray-600 fs-12px review_file_label me-3" id="review_file_label_3" role="button"></label>
                                                    <input type="file" class="d-none input_file" id="attachment3" name="attachment3" data-row="3">
                                                </div>
                                                <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete3" title="Delete" value="3" disabled>
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
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">4.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment4" role="button">Browse...</label>
                                                    <label for="attachment4" class="form-control form-control-sm text-gray-600 fs-12px review_file_label me-3" id="review_file_label_4" role="button"></label>
                                                    <input type="file" class="d-none input_file" id="attachment4" name="attachment4" data-row="4">
                                                </div>
                                                <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" id="btn_delete4" title="Delete" value="4" disabled>
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
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">5.</label>
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
                                                <div class="invisible" id="info5">
                                                    <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">6.</label>
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
                                                <div class="invisible" id="info6">
                                                    <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">7.</label>
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
                            <textarea maxlength="255" class="form-control form-control-sm" name="notes_message" id="notes_message" value=""></textarea>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    @include('Promo::promo-send-back.source-budget-list')
    @include('Promo::promo-send-back.form-attribute')
@endsection


@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/js/generate-uuid.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/promo-send-back/js/promo-send-back-form.js?v=15') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-send-back/js/source-budget-list.js?v=1') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-send-back/js/promo-send-back-form-attribute.js?v=9') }}"></script>
@endsection


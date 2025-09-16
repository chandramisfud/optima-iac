@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-creation/css/promo-creation-form-revamp.css?v=3') }}" rel="stylesheet" type="text/css"/>
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
    <div id="primaryForm">
        <div class="row">
            <div class="col-lg-8 col-12 pe-lg-1">
                <div class="row mb-2">
                    <div class="col-12">
                        <div class="card shadow-sm card_header">
                            <form id="form_promo" class="form" autocomplete="off">
                                @csrf
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-lg-6 col-12">
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
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subCategoryId">Sub Category</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <select class="form-select form-select-sm" data-control="select2" name="subCategoryId" id="subCategoryId" data-placeholder="Select a Sub Category"  data-allow-clear="true">
                                                        <option></option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="row fv-row">
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="activityId">Activity</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <select class="form-select form-select-sm" data-control="select2" name="activityId" id="activityId" data-placeholder="Select an Activity"  data-allow-clear="true">
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
                                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="activityDesc">Activity Name</label>
                                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                    <textarea maxlength="255" class="form-control form-control-sm" name="activityDesc" id="activityDesc" rows="4"></textarea>
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
                                        </div>

                                        <div class="offset-lg-1 col-lg-5 col-12">
                                            <div class="row fv-row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right required" for="period">Period</label>
                                                <div class="col-lg-5 col-md-12 col-sm-12 col-12">
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
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right required" for="distributorId">Distributor</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <select class="form-select form-select-sm" data-control="select2" name="distributorId" id="distributorId" data-placeholder="Select a Distributor"  data-allow-clear="true">
                                                        <option></option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="row fv-row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right required" for="channelId">Channel</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <select class="form-select form-select-sm" data-control="select2" name="channelId" id="channelId" data-placeholder="Select a Channel"  data-allow-clear="true">
                                                        <option></option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="row fv-row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right col-form-label-over-space required" for="subChannelId">Sub Channel</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <select class="form-select form-select-sm" data-control="select2" name="subChannelId" id="subChannelId" data-placeholder="Select a Sub Channel"  data-allow-clear="true">
                                                        <option></option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="row fv-row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right required" for="accountId">Account</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <select class="form-select form-select-sm" data-control="select2" name="accountId" id="accountId" data-placeholder="Select an Account"  data-allow-clear="true">
                                                        <option></option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="row fv-row">
                                                <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label-right col-form-label-over-space required" for="subAccountId">Sub Account</label>
                                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                                    <select class="form-select form-select-sm" data-control="select2" name="subAccountId" id="subAccountId" data-placeholder="Select a Sub Account"  data-allow-clear="true">
                                                        <option></option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="card shadow-sm card_mechanism">
                            <div class="card-body">
                                <div class="row mb-2">
                                    <div class="col-lg-12 col-12">
                                        <button type="button" class="btn btn-sm btn-outline-optima w-100" id="btn_mechanism_edit">
                                            <span class="fa fa-edit"></span> Mechanism
                                        </button>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-12 col-12">
                                        <table id="dt_mechanism" class="table table-striped table-row-bordered table-responsive table-sm table-hover">
                                            <thead class="fw-bold fs-6 text-gray-800 bg-optima text-white"></thead>
                                        </table>
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
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="totalCost" id="totalCost" autocomplete="off" value="0" readonly/>
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
                                        <select class="form-select form-select-sm" data-control="select2" name="regionId" id="regionId" data-placeholder="Select Region"  data-allow-clear="true" multiple="multiple">
                                            <option></option>
                                        </select>
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
                                        <span class="fw-bold text-gray-800 fs-4">SKU</span> <span class="text-danger fs-8 d-none" id="skuInvalid">Please select a sku</span>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 col-12">
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="d-flex justify-content-between">
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


                                                    <button type="button" class="btn btn-sm btn-outline-optima text-nowrap ms-1 w-auto" id="btn_select_all_sku">
                                                        All <span class="fa fa-chevron-right"></span>
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                        <table id="dt_sku" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                    </div>
                                    <div class="col-lg-6 col-12">
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="d-flex justify-content-between">

                                                    <button type="button" class="btn btn-sm btn-outline-optima text-nowrap me-1 w-auto" id="btn_deselect_all_sku">
                                                        <span class="fa fa-chevron-left"></span> All
                                                    </button>

                                                    <div class="inner-addon left-addon right-addon">
                                                        <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                                <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                                <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                            </svg>
                                                        </span>
                                                        <input type="text" class="form-control form-control-sm ps-10" value="" placeholder="Search" id="dt_sku_selected_search" autocomplete="off">
                                                        <label class="d-none" for="dt_sku_selected_search"></label>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                        <table id="dt_sku_selected" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-12">
                        <div class="card shadow-sm card_attachment">
                            <div class="card-body">
                                <div class="col-12 h-150px" style="overflow-y: auto; overflow-x: hidden">
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
                                                <label for="attachment2" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_2" role="button"></label>
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
                                    <div class="row fv-row">
                                        <div class="d-flex align-items-center">
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
                                            <div class="invisible mt-2" id="info5">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <div class="d-flex align-items-center">
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
                                            <div class="invisible mt-2" id="info6">
                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <div class="d-flex align-items-center">
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
        </div>
    </div>
    <div class="d-none" id="MechanismMatrixForm">
        <form id="form_mechanism_matrix" class="form" autocomplete="off">
            <div class="px-lg-0 sticky-top-header-mechanism">
                <div class="header-dashboard" id="header-dashboard">
                    <div class="row mb-2">
                        <div class="col-lg-12 col-12">
                            <div class="card shadow-sm card_mechanism_button">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-12 d-flex justify-content-between">
                                            <span class="fs-3 fw-bolder mt-1 me-auto">Mechanism</span>
                                            <button type="button" class="btn btn-sm btn-outline-optima" id="btn_mechanism_save">
                                                <span class="fa fa-edit"></span> Submit
                                            </button>
                                            <div class="btn btn-icon btn-sm btn-active-light-primary ms-2" aria-label="Close" id="btn_mechanism_cancel">
                                        <span class="svg-icon svg-icon-1">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="black"></rect>
                                                <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="black"></rect>
                                            </svg>
                                        </span>
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
                <div class="container" id="autoMechanismFormSection">
                    <div class="row mb-2">
                        <div class="col-lg-8 col-12 pe-lg-1">
                            <div class="row mb-2 d-none" id="autoMechanismSourceSection">
                                <div class="col-12">
                                    <div class="card shadow-sm card_auto_mechanism">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="row">
                                                        <div class="col-lg-12 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                            <div class="inner-addon left-addon right-addon">
                                                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                                    </svg>
                                                                </span>
                                                                <input type="text" class="form-control form-control-sm ps-10" value="" placeholder="Search" id="dt_mechanism_source_search" autocomplete="off">
                                                                <label class="d-none" for="dt_mechanism_source_search"></label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <table id="dt_mechanism_source" class="table table-striped table-row-bordered table-responsive table-sm table-hover">
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-lg-0 mb-2 d-none" id="autoMechanismInputSection">
                                <div class="col-12">
                                    <div class="card shadow-sm card_auto_mechanism_input">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="row fv-row">
                                                        <label class="col-lg-2 col-md-12 col-sm-12 col-12 col-form-label" for="skuDesc">SKU</label>
                                                        <div class="col-lg-10 col-md-12 col-sm-12 col-12 ">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" name="skuDesc" id="skuDesc" autocomplete="off" readonly/>
                                                        </div>
                                                    </div>
                                                    <div class="row fv-row">
                                                        <label class="col-lg-2 col-md-12 col-sm-12 col-12 col-form-label" for="mechanism">Mechanism</label>
                                                        <div class="col-lg-10 col-md-12 col-sm-12 col-12 ">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" name="mechanism" id="mechanism" autocomplete="off" readonly/>
                                                        </div>
                                                    </div>
                                                    <div class="row fv-row">
                                                        <label class="col-lg-2 col-md-12 col-sm-12 col-12 col-form-label" for="notes">Notes</label>
                                                        <div class="col-lg-10 col-md-12 col-sm-12 col-12 ">
                                                            <input type="text" class="form-control form-control-sm" name="notes" id="notes" autocomplete="off"/>
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
                            <div class="card shadow-sm h-100">
                                <div class="card-header py-0" id="info_details_header">
                                    <h3 class="card-title fw-bolder"><span id="mainActivityText"></span></h3>
                                </div>
                                <div class="card-body  overflow-auto h-286px">
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Brand</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoGroupBrand">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Distributor</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoDistributor">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Sub Category</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoSubCategory">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Sub Activity</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoSubActivity">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Activity Name</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoActivityDesc">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Activity Period</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoActivityPeriod">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Channel</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoChannel">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Account</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoAccount">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Sub Account</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoSubAccount">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">SKU</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoSKU"></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-12">
                    <div class="row mb-2">
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
                                                    <label class="col-form-label text-nowrap py-0 fw-bolder text-optima pe-1" for="cost">Cost</label>
                                                    <input type="text" class="form-control form-control-sm text-end text-black border-bg-optima" id="cost">
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-12">
                                            <button type="button" class="btn btn-sm btn-optima w-100" id="btn_calculator_save">
                                                Save
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-2 d-none" id="autoMechanismListSection">
                        <div class="col-12">
                            <div class="card shadow-sm card_auto_mechanism_input">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-lg-12 col-12">
                                            <table id="dt_mechanism_input" class="table table-striped table-row-bordered table-responsive table-sm table-hover">
                                                <thead class="fw-bold fs-6 text-gray-800 bg-optima text-white">
                                                </thead>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-2" id="manualMechanismInputSection">
                        <div class="col-lg-8 col-12 pe-lg-1">
                            <div class="card shadow-sm card_manual_mechanism">
                                <div class="card-body">
                                    <div id="manualMechanismList">
                                        <div class="row mb-3" id="mechanismRow1">
                                            <div class="col-12">
                                                <div class="d-flex justify-content-between">
                                                    <label class="text-nowrap my-auto me-10" for="manual_mechanism_1">Mechanism 1</label>
                                                    <input type="text" class="form-control form-control-sm" name="manual_mechanism_1" id="manual_mechanism_1" autocomplete="off"/>
                                                    <button class="btn btn-sm btn-outline-optima ms-2 invisible" id="btn_manual_mechanism_delete_1" title="Delete" value="1">
                                                        <span class="fa fa-trash-alt"> </span>
                                                    </button>
                                                    <button class="btn btn-sm btn-outline-optima ms-2" id="btn_manual_mechanism_add_1" title="Add Row">
                                                        <span class="fa fa-plus"></span>
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-12 ps-lg-1">
                            <div class="card shadow-sm">
                                <div class="card-header py-0" id="info_details_header_input">
                                    <h3 class="card-title fw-bolder"><span id="mainActivityTextInput"></span></h3>
                                </div>
                                <div class="card-body  overflow-auto h-286px">
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Brand</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoGroupBrandInput">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Distributor</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoDistributorInput">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Sub Category</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoSubCategoryInput">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Sub Activity</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoSubActivityInput">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Activity Name</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoActivityDescInput">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Activity Period</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoActivityPeriodInput">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Channel</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoChannelInput">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Account</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoAccountInput">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">Sub Account</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoSubAccountInput">-</span>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-lg-4 col-12">
                                            <span class="fw-normal">SKU</span>
                                        </div>
                                        <div class="col-lg-8 col-12">
                                            <span id="txtInfoSKUInput"></span>
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

    <div id="info_details" class="bg-body" data-kt-drawer="true" data-kt-drawer-name="explore" data-kt-drawer-activate="true"
         data-kt-drawer-overlay="true" data-kt-drawer-width="{default:'350px', 'lg': '475px'}"
         data-kt-drawer-direction="end" data-kt-drawer-toggle="#info_details_toggle" data-kt-drawer-close="#info_details_close">
        <div class="card shadow-none rounded-0 w-100">
            <div class="card-header" id="info_details_header">
                <h3 class="card-title fw-bolder text-gray-700" id="mainActivityText"></h3>
                <div class="card-toolbar">
                    <button type="button" class="btn btn-icon btn btn-sm btn-clean me-n5" id="info_details_close">
                        <span class="svg-icon svg-icon-2">
								<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
                                     fill="none">
									<rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1"
                                          transform="rotate(-45 6 17.3137)" fill="black"/>
									<rect x="7.41422" y="6" width="16" height="2" rx="1"
                                          transform="rotate(45 7.41422 6)" fill="black"/>
								</svg>
							</span>
                    </button>
                </div>
            </div>
            <div class="card-body" id="info_details_body">
                <div id="kt_explore_scroll" data-kt-scroll-height="auto" data-kt-scroll-wrappers="#info_details_body">
                    <div class="mb-0">
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Period</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoPeriod">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Entity</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoEntity">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Brand</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoGroupBrand">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Sub Category</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoSubCategory">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Activity</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoActivity">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Sub Activity</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoSubActivity">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Activity Desc</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoActivityDesc">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Activity Period</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoActivityPeriod">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Distributor</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoDistributor">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Channel</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoChannel">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Sub Channel</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoSubChannel">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Account</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoAccount">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Sub Account</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoSubAccount">-</span>
                            </div>
                        </div>
                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">Region</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoRegion">-</span>
                            </div>
                        </div>

                        <div class="row mb-2">
                            <div class="col-lg-4 col-12">
                                <span class="fw-normal">SKU</span>
                            </div>
                            <div class="col-lg-8 col-12">
                                <span id="txtInfoSKU"></span>
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
    <script src="{{ asset('assets/js/generate-uuid.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/promo-creation/js/promo-creation-form-revamp.js?v=69') }}"></script>
@endsection

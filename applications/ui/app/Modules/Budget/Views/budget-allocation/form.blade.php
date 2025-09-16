@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/budget/budget-allocation/css/budget-source-list.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Add New</small>
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
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_budget_allocation" class="form" autocomplete="off">
                        @csrf
                        <div class="row">
                            <div class="col-lg-6 col-md-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required">Budget Year</label>
                                    <div class="col-lg-4">
                                        <div class="input-group input-group-sm" id="dialer_period">
                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                <i class="fa fa-minus fs-2"></i>
                                            </button>

                                            <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="period" id="period" value="{{ @date('Y') }}" autocomplete="off" tabindex="1"/>

                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                <i class="fa fa-plus fs-2"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required">Select Budget Source</label>
                                    <div class="col-lg-8">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="budget_source" name="budget_source" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly>
                                                <span class="input-group-text cursor-pointer btn-outline-optima" id="btn_search_budget_source">
                                                <span class="fa fa-search"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label">Budget Master</label>
                                    <div class="col-lg-8">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="budgetMaster" id="budgetMaster" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required">Budget Name</label>
                                    <div class="col-lg-8">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" id="budgetName" name="budgetName" autocomplete="off" tabindex="3" readonly/>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6 col-md-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label">Entity</label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="entityLongDesc" id="entityLongDesc" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label">Budget Assigned</label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="budgetAssigned" id="budgetAssigned" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label">Remaining Budget</label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="budgetRemaining" id="budgetRemaining" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-12">
                                <div class="separator border-3 my-2"></div>
                            </div>
                        </div>

                        <div class="row mt-2" id="tab_budget_allocation">
                            <div class="col-12">
                                <ul class="nav nav-tabs">
                                    <li class="nav-item">
                                        <a class="nav-link active" data-bs-toggle="tab" data-bs-target="#detail" id="tabDetail">Detail</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link required" data-bs-toggle="tab" data-bs-target="#region" id="tabRegion">Region</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" data-bs-target="#channel" id="tabChannel">Channel</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" data-bs-target="#subchannel" id="tabSubChannel">Sub Channel</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link required" data-bs-toggle="tab" data-bs-target="#account" id="tabAccount">Account</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link required" data-bs-toggle="tab" data-bs-target="#subaccount" id="tabSubAccount">Sub Account</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link required" data-bs-toggle="tab" data-bs-target="#user" id="tabUser">User</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link required" data-bs-toggle="tab" data-bs-target="#brand" id="tabBrand">Brand</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" data-bs-target="#sku" id="tabSku">SKU</a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="tab_content_budget_allocation">
                                    <div class="tab-pane show active" id="detail">
                                        <div class="row mt-5">
                                            <div class="col-lg-6 col-12">
                                                <div class="row fv-row">
                                                    <label class="col-lg-4 col-form-label required">Category</label>
                                                    <div class="col-lg-8" id="fieldCategory">
                                                        <select class="form-select form-select-sm" data-control="select2" name="categoryId" id="categoryId" data-placeholder="Select a Category"  data-allow-clear="true" tabindex="4">
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="row fv-row">
                                                    <label class="col-lg-4 col-form-label required">Sub Category</label>
                                                    <div class="col-lg-8" id="fieldSubCategory">
                                                        <select class="form-select form-select-sm" data-control="select2" name="subCategoryId" id="subCategoryId" data-placeholder="Select a Sub Category"  data-allow-clear="true" tabindex="5">
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="row fv-row">
                                                    <label class="col-lg-4 col-form-label required">Budget Amount</label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm text-end" type="text" name="budgetAmount" id="budgetAmount" value="0" autocomplete="off" tabindex="6" readonly/>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 col-12">
                                                <div class="row fv-row">
                                                    <label class="col-lg-4 col-form-label">Activity</label>
                                                    <div class="col-lg-8" id="fieldActivity">
                                                        <select class="form-select form-select-sm" data-control="select2" name="activityId" id="activityId" data-placeholder="Select an Activity"  data-allow-clear="true" tabindex="7">
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="row fv-row">
                                                    <label class="col-lg-4 col-form-label">Sub Activity</label>
                                                    <div class="col-lg-8" id="fieldSubActivity">
                                                        <select class="form-select form-select-sm" data-control="select2" name="subActivityId" id="subActivityId" data-placeholder="Select a Sub Activity"  data-allow-clear="true" tabindex="8">
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="region">
                                        <div class="row mt-2">
                                            <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                    <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_region_search" autocomplete="off">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="dt_region" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="channel">
                                        <div class="row mt-2">
                                            <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                    <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_channel_search" autocomplete="off">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="dt_channel" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="subchannel">
                                        <div class="row mt-2">
                                            <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                    <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_subchannel_search" autocomplete="off">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="dt_subchannel" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="account">
                                        <div class="row mt-2">
                                            <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                    <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_account_search" autocomplete="off">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="dt_account" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="subaccount">
                                        <div class="row mt-2">
                                            <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                    <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_subaccount_search" autocomplete="off">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="dt_subaccount" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="user">
                                        <div class="row mt-2">
                                            <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                    <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_user_search" autocomplete="off">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="dt_user" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="brand">
                                        <div class="row mt-2">
                                            <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                    <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_brand_search" autocomplete="off">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="dt_brand" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="sku">
                                        <div class="row mt-2">
                                            <div class="col-lg-2-5 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                                <div class="inner-addon left-addon right-addon">
                                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                        </svg>
                                                    </span>
                                                    <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_sku_search" autocomplete="off">
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="dt_sku" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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

    @include('Budget::budget-allocation.budget-source-list')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/region-list.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/channel-list.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/sub-channel-list.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/account-list.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/sub-account-list.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/user-list.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/brand-list.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/sku-list.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/budget-allocation-form.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-allocation/js/budget-source-list.js?v=' . microtime()) }}"></script>
@endsection

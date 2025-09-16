@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/budget/budget-tt-console/css/budget-tt-console-methods.css?v=' . microtime()) }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">List</small>
    </span>
@endsection

@section('button-toolbar-left')
    @php @$profileCategory = Session::get('profileCategories'); @endphp
    @if (count($profileCategory) > 1)
        <div>
            <button type="button" class="btn btn-sm btn-outline-optima text-hover-white" id="btn_create" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-kt-menu-offset="30px, 0px">
                <span class="fa fa-plus"></span> Add New
                <i class="la la-angle-down fs-6 text-optima rotate-180 ms-1"></i>
            </button>
            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-auto" data-kt-menu="true">
                @foreach($profileCategory as $category)
                    <div class="menu-item px-3 py-2">
                        <a href="javascript:void(0)" class="menu-link px-3 fw-bold text-gray-700 record-create" data-category-id = "{{ $category->categoryId }}" data-category="{{ encrypt($category->categoryLongDesc) }}">
                            {{ $category->categoryLongDesc }}
                        </a>
                    </div>
                @endforeach
            </div>
        </div>
    @else
        @foreach($profileCategory as $category)
            <button type="button" class="btn btn-sm btn-outline-optima text-hover-white record-create" id="btn_create" data-category-id = "{{ $category->categoryId }}" data-category="{{ encrypt($category->categoryLongDesc) }}">
                <span class="fa fa-plus"></span> Add New
            </button>
        @endforeach
    @endif
@endsection

@section('button-toolbar-right')
    @php @$profileCategory = Session::get('profileCategories'); @endphp

    <div>
        <button type="button" class="btn btn-sm btn-outline-optima text-hover-white" id="btn_export_multi" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-kt-menu-offset="30px, 0px">
            <span class="bi bi-download"></span>
        </button>
        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-150px" data-kt-menu="true">
            <div class="menu-item px-3 py-2" data-kt-menu-trigger="hover" data-kt-menu-placement="right-start">
                <!--begin::Menu item-->
                <a href="#" class="menu-link px-2">
                    <span class="menu-title">Current</span>
                    <span class="menu-arrow"></span>
                </a>
                <div class="menu-sub menu-sub-dropdown w-150px">
                    @foreach($profileCategory as $category)
                        <div class="menu-item px-3 py-2">
                            <a href="javascript:void(0)" class="menu-link px-3 fw-bold text-gray-700 export-excel" data-type="Current" data-shortdesc="{{ $category->categoryShortDesc }}" data-category="{{ $category->categoryId }}">
                                {{ $category->categoryLongDesc }}
                            </a>
                        </div>
                    @endforeach
                </div>
            </div>


            <div class="menu-item px-3 py-2" data-kt-menu-trigger="hover" data-kt-menu-placement="right-start">
                <!--begin::Menu item-->
                <a href="#" class="menu-link px-2">
                    <span class="menu-title">Historical</span>
                    <span class="menu-arrow"></span>
                </a>
                <div class="menu-sub menu-sub-dropdown w-150px">
                    @foreach($profileCategory as $category)
                        <div class="menu-item px-3 py-2">
                            <a href="javascript:void(0)" class="menu-link px-3 fw-bold text-gray-700 export-excel" data-type="Historical" data-shortdesc="{{ $category->categoryShortDesc }}" data-category="{{ $category->categoryId }}">
                                {{ $category->categoryLongDesc }}
                            </a>
                        </div>
                    @endforeach
                </div>
            </div>
        </div>
    </div>

    <button type="button" class="btn btn-sm btn-clean me-2" id="btn_back">
        <span class="fa fa-arrow-left"></span> Back
    </button>
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 mb-lg-0 mb-2">
                            <div class="row">
                                <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-1 mb-2">
                                    <div class="inner-addon left-addon right-addon">
                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_budget_tt_console_search" autocomplete="off">
                                    </div>
                                </div>
                                <div class="col-lg-9 col-md-12 col-sm-12 col-12">
                                    @php @$profileCategory = Session::get('profileCategories'); @endphp
                                    <div class="d-flex justify-content-end">
                                        <div>
                                            <button id="filter_toggle"
                                                    class="btn btn-sm btn-outline-optima text-hover-white me-2" title="Filter Data">
                                                <span id="filter_toggle_label" class="bi bi-filter"></span> Filter Data
                                                <span id="filter_count_badge" class="badge bg-gray-500"> 0</span>
                                            </button>
                                        </div>
                                        @if (count($profileCategory) > 1)
                                            <div>
                                                <button type="button" class="btn btn-sm btn-outline-optima text-hover-white me-2 d-none" id="btn_download_template" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-kt-menu-offset="30px, 0px">
                                                    <span class="bi bi-download"></span> Download Template
                                                    <i class="la la-angle-down fs-6 text-optima rotate-180 ms-1"></i>
                                                </button>
                                                <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-auto" data-kt-menu="true">
                                                    @foreach($profileCategory as $category)
                                                        <div class="menu-item px-3 py-2">
                                                            <a href="javascript:void(0)" class="menu-link px-3 fw-bold text-gray-700 download-template" data-shortdesc="{{ $category->categoryShortDesc }}" data-category="{{ $category->categoryId }}">
                                                                {{ $category->categoryLongDesc }}
                                                            </a>
                                                        </div>
                                                    @endforeach
                                                </div>
                                            </div>
                                        @else
                                            @foreach($profileCategory as $category)
                                                <button type="button" class="btn btn-sm btn-outline-optima text-hover-white download-template me-1 d-none" id="btn_download_template" data-shortdesc="{{ $category->categoryShortDesc }}" data-category="{{ $category->categoryId }}">
                                                    <span class="fa fa-download"></span> Download Template
                                                </button>
                                            @endforeach
                                        @endif
                                        @if (count($profileCategory) > 1)
                                            <div>
                                                <button type="button" class="btn btn-sm btn-outline-optima text-hover-white me-2 d-none" id="btn_upload" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-kt-menu-offset="30px, 0px">
                                                    <span class="bi bi-upload"></span> Upload
                                                    <i class="la la-angle-down fs-6 text-optima rotate-180 ms-1"></i>
                                                </button>
                                                <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-auto" data-kt-menu="true">
                                                    @foreach($profileCategory as $category)
                                                        <div class="menu-item px-3 py-2">
                                                            <a href="javascript:void(0)" class="menu-link px-3 fw-bold text-gray-700 upload-template" data-shortdesc="{{ $category->categoryShortDesc }}" data-category="{{ $category->categoryId }}">
                                                                {{ $category->categoryLongDesc }}
                                                            </a>
                                                        </div>
                                                    @endforeach
                                                </div>
                                            </div>
                                        @else
                                            @foreach($profileCategory as $category)
                                                <button type="button" class="btn btn-sm btn-outline-optima text-hover-white upload-template" id="btn_upload" data-shortdesc="{{ $category->categoryShortDesc }}" data-category="{{ $category->categoryId }}">
                                                    <span class="fa fa-download"></span> Upload
                                                </button>
                                            @endforeach
                                        @endif
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <table id="dt_budget_tt_console" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="filter" class="bg-body" data-kt-drawer="true" data-kt-drawer-name="explore" data-kt-drawer-activate="true"
         data-kt-drawer-overlay="true" data-kt-drawer-width="{default:'350px', 'lg': '430px'}"
         data-kt-drawer-direction="end" data-kt-drawer-toggle="#filter_toggle" data-kt-drawer-close="#filter_close">
        <div class="card shadow-none rounded-0 w-100 overflow-hidden">
            <div class="card-header" id="filter_header">
                <h3 class="card-title fw-bolder text-gray-700">Filter Data Budget TT Consol</h3>
                <div class="card-toolbar">
                    <button type="button" class="btn btn-icon btn btn-sm btn-clean me-n5" id="filter_close">
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
            <div class="card-body overflow-auto" id="filter_body">
                <div id="kt_explore_scroll" data-kt-scroll="true"
                     data-kt-scroll-height="auto" data-kt-scroll-wrappers="#filter_body"
                     data-kt-scroll-dependencies="#filter_header, #filter_footer" data-kt-scroll-offset="5px">
                    <div class="mb-0">
                        <form id="form_budget_tt_console_filter" class="form" autocomplete="off">
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_period">Budget Year</label>
                                    <div class="input-group input-group-sm" id="dialer_period">
                                        <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                            <i class="fa fa-minus fs-2"></i>
                                        </button>
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="filter_period" id="filter_period" value="{{ @date('Y') }}" autocomplete="off"/>

                                        <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                            <i class="fa fa-plus fs-2"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_category">Category</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_category" id="filter_category" data-placeholder="Select a Category"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_channel">Channel</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_channel" id="filter_channel" data-placeholder="Select a Channel"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                                <div class="col-lg-6 col-md-6 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_subchannel">Sub Channel</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_subchannel" id="filter_subchannel" data-placeholder="Select a Sub Channel"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_account">Account</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_account" id="filter_account" data-placeholder="Select an Account"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                                <div class="col-lg-6 col-md-6 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_subaccount">Sub Account</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_subaccount" id="filter_subaccount" data-placeholder="Select a Sub Account"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_distributor">Distributor</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_distributor" id="filter_distributor" data-placeholder="Select a Distributor"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_groupbrand">Brand</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_groupbrand" id="filter_groupbrand" data-placeholder="Select a Brand"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_subactivity_type">Sub Activity Type</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_subactivity_type" id="filter_subactivity_type" data-placeholder="Select a Sub Activity Type"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                                <div class="col-lg-6 col-md-6 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_subcategory">Sub Category</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_subcategory" id="filter_subcategory" data-placeholder="Select a Sub Category"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_activity">Activity</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_activity" id="filter_activity" data-placeholder="Select an Activity"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                                <div class="col-lg-6 col-md-6 col-12 mb-3">
                                    <label class="form-label fw-bolder text-dark" for="filter_subactivity">Sub Activity</label>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_subactivity" id="filter_subactivity" data-placeholder="Select a Sub Activity"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
            <div class="card-footer ps-9" id="filter_footer">
                <div class="row">
                    <div class="col-md-12 col-12">
                        <button type="button" class="btn btn-sm btn-optima" id="btn_filter">
                            <i class="fa fa-filter" style="color: white"> </i> Filter
                        </button>

                        <button type="button" class="btn btn-sm btn-secondary" id="btn_reset_filter">
                            <i class="fa fa-undo"> </i> Reset Filter
                        </button>
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
    <script src="{{ asset('assets/pages/budget/budget-tt-console/js/budget-tt-console-methods.js?v=20') }}"></script>
@endsection

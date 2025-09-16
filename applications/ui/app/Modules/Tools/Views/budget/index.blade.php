@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')

@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method">Upload Data</small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-5">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_budget" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <div class="input-group mb-2">
                                <input class="form-control field_upload" id="file" name="file" type="file" data-stripe="file" placeholder="Choose File"/>
                                <span class="input-group-text">Upload File</span>
                            </div>
                        </div>
                        <div class="separator my-2 border-2"></div>
                        <div class="row">
                            @php @$profileCategory = Session::get('profileCategories'); @endphp
                            @if (count($profileCategory) > 1)
                                <div class="col-lg-6 col-md-6 col-sm-6 col-6 mb-2">
                                    <button type="button" class="btn btn-sm btn-optima fw-bolder" id="btn_export" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-kt-menu-offset="30px, 0px">
                                        <span class="fa fa-download"></span> Download
                                        <i class="la la-angle-down fs-6 rotate-180 ms-1 text-white"></i>
                                    </button>
                                    <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-800 menu-state-bg-light-primary fw-semibold w-auto" data-kt-menu="true">
                                        @foreach($profileCategory as $category)
                                            <div class="menu-item px-3 py-2">
                                                <a href="javascript:void(0)" class="menu-link px-3 fw-bold text-gray-700 btn_download" data-category="{{  $category->categoryLongDesc }}">
                                                    {{ $category->categoryLongDesc }}
                                                </a>
                                            </div>
                                        @endforeach
                                    </div>
                                </div>
                            @else
                                @foreach($profileCategory as $category)
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-6 mb-2">
                                        <div class="text-start">
                                            <button type="button" class="btn btn-sm btn-optima fw-bolder btn_download" data-category="{{  $category->categoryLongDesc }}">
                                                <span class="indicator-label">
                                                    <span class="fa fa-download"></span> Download
                                                </span>
                                                <span class="indicator-progress">Download...
                                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                                </span>
                                            </button>
                                        </div>
                                    </div>
                                @endforeach
                            @endif
                            <div class="col-lg-6 col-md-6 col-sm-6 col-6">
                                <div class="text-end">
                                    <button type="button" class="btn btn-sm btn-optima fw-bolder" id="btn_upload">
                                        <span class="indicator-label">
                                            <span class="fa fa-cloud-upload-alt"></span> Upload
                                        </span>
                                        <span class="indicator-progress">Uploading...
                                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                        </span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <div class="row d-none" id="card_budget">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <div class="row">
                                <div class="card-title">
                                     <span class="d-flex align-items-center fs-3 my-1">Master Budget
                                        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
                                        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">List</small>
                                    </span>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                    <table id="dt_budget" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/plugins/custom/jszip/jszip.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/jszip/xlsx.full.min.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/tools/budget/js/budget-upload.js?v=3') }}"></script>
@endsection

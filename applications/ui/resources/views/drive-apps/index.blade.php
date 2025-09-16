@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/css/log.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">List</small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-export-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-12">
            <div class="card shadow shadow-sm card-flush">
                <div class="card-body card-body-top-data p-0">
                    <div class="row d-flex flex-lg-row flex-column align-items-center mb-lg-1 mb-3">
                        <div class="w-auto">
                            <div class="position-relative mt-2 mb-lg-0 mb-2">
                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-17 translate-middle ms-6">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                    </svg>
                                </span>
                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_drive_search" autocomplete="off">
                            </div>
                        </div>
                        <div class="w-auto">
                            <span class="mb-lg-0 mb-2">Attachment Type</span>
                        </div>
                        <div class="w-lg-200px w-100 mb-lg-0 mb-2 me-lg-5 me-0">
                            <select class="form-select form-select-sm" data-control="select2" name="attachment_type" id="attachment_type" data-placeholder="Select an Attachment Type"  data-allow-clear="true">
                                <option></option>
                                <option value="promo">Promo</option>
                                <option value="debitnote">Debit Note</option>
                            </select>
                        </div>
                        <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100 me-lg-auto mb-lg-0 mb-2" id="btn_view">
                                <span class="indicator-label">
                                    <span class="fa fa-search"></span> View
                                </span>
                            <span class="indicator-progress">
                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                </span>
                        </button>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <table id="dt_drive" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/js/drive.js?v=' . microtime()) }}"></script>
@endsection

@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/budget/budget-tt-console/css/budget-tt-console-upload-xls.css') }}" rel="stylesheet" type="text/css" />
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Upload File</small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-3">
        <div class="col-12">
            <div class="card card-flush shadow card_form">
                <div class="card-body">
                    <div class="row">
                        <div class="col-12">
                            <form id="form_upload" class="form" autocomplete="off">
                                @csrf
                                <div class="row fv-row">
                                    <div class="input-group input-group-sm mb-3">
                                        <input class="form-control form-control-sm field_upload" id="file" name="file" type="file" data-stripe="file" placeholder="Choose File"/>
                                        <span class="input-group-text">Upload File</span>
                                    </div>
                                </div>
                                <div class="separator my-2 border-2"></div>
                                <div class="row">
                                    <div class="text-end">
                                        <button type="button" class="btn btn-sm btn-optima" id="btn_upload">
                                            <span class="indicator-label">
                                                <span class="fa fa-cloud-upload-alt"></span> Upload
                                            </span>
                                            <span class="indicator-progress">Uploading...
                                                <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                            </span>
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div class="row d-none" id="d_progress">
                        <div class="col-md-12 col-12">
                            <div class="row mb-2">
                                <div class="col-lg-12 col-12" id="info_sending_progress">
                                    <span class="text-gray-800 info_sending_email">Processing</span>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-12">
                                    <div class="d-flex flex-column w-100 me-2">
                                        <div class="progress h-10px w-100">
                                            <div class="progress-bar bg-optima" id="progress_bar" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                <div class="d-flex flex-center">
                                                    <span class="text-white fs-7 fw-bold text-center" id="text_progress">0%</span>
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
    </div>

    <div class="row" id="list_uploaded">
        <div class="col-12">
            <div class="card shadow">
                <div class="card-header px-5">
                    <span class="card-title fs-3">Budget TT Consol Upload <small class="text-muted fs-6 my-0 fw-bold ms-5">List</small></span>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <strong>
                                <span id="upload_result"></span>
                            </strong>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <table id="dt_list_upload" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-tt-console/js/budget-tt-console-upload-xls-rc.js?v=4') }}"></script>
@endsection

@extends('layouts/layoutMaster')

@section('title', 'Master Matrix Approval')

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet"
          type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/master/promo-matrix-approval/css/promo-matrix-approval-upload.css') }}" rel="stylesheet"
          type="text/css"/>
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
    <div class="row mb-3">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_matrix_approval" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <div class="input-group mb-3">
                                <input class="form-control field_upload" id="file" name="file" type="file"
                                       data-stripe="file" placeholder="Choose File"/>
                                <span class="input-group-text">Upload File</span>
                            </div>
                        </div>
                        <div class="separator my-2 border-2 mb-3"></div>
                        <div class="row">
                            <div class="d-flex justify-content-between">
                                <button type="button" class="btn btn-sm btn-optima fw-bolder" id="btn_download">
                                    <span class="indicator-label">
                                        <span class="bi bi-download"></span> Download Template
                                    </span>
                                    <span class="indicator-progress">Downloading...
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
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
                    </form>
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
    <script src="{{ asset('assets/pages/master/promo-matrix-approval/js/promo-matrix-approval-upload.js') }}"></script>
@endsection

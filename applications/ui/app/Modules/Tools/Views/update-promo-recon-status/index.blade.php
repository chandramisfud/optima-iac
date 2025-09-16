@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/tools/update-promo-recon-status/css/update-promo-recon-status-methods.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method">Mass Upload</small>
    </span>
@endsection

@section('button-toolbar-left')

@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection


@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form mb-3">
                <div class="card-body">
                    <form id="form_upload" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <div class="input-group input-group-sm">
                                <input class="form-control form-control-sm field_upload" id="file" name="file" type="file" data-stripe="file" placeholder="Choose File"/>
                                <span class="input-group-text">Upload File</span>
                            </div>
                        </div>
                        <div class="separator my-2 border-2"></div>
                        <div class="row">
                            <div class="col-lg-7 col-md-12 col-12 mb-lg-0 mb-2 text-start">
                                <div class="row">
                                    <div class="position-relative w-lg-175px w-fhd-250px mb-2">
                                        <button type="button" class="btn btn-sm btn-outline-optima fw-bolder w-lg-auto w-100" id="btn_download_template">
                                            <span class="fa fa-cloud-download-alt"></span> Download Template
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-5 col-md-12 col-12 mb-lg-0 mb-2 text-end">
                                <button type="button" class="btn btn-sm btn-outline-optima fw-bolder w-lg-auto w-100" id="btn_upload">
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

@endsection

@section('page-script')
    <script src="{{ asset('assets/pages/tools/update-promo-recon-status/js/update-promo-recon-status-methods.js?v=1') }}"></script>
@endsection

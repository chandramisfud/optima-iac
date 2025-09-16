@extends('layouts/layoutMaster')

@section('title', 'Upload Debit Note')

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection
<style>
    .dropzone {
        height: auto;
        background-color: white !important;
    }
</style>
@section('page-style')

@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method"></small>
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
                    <div class="d-flex align-items-center rounded py-5 px-5 mb-5" style="background-color: #fd397a">
                        <span class="la la-warning" style="color: white; font-size: xxx-large"></span>
                        <div class="alert alert-info1" role="alert" id="formEntry_msg">
                            <div class="alert-text">
                                <h3 class="text-white">
                                    Special Character Rules: <br>
                                    1. Allowed Special Character on filename : `~!@$^&()-_=+[]{} <br>
                                    2. Not Allowed Special Character on filename : /\:*"<>?|#%
                                </h3>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                        <form class="form_dn_upload_attachment" method="post" enctype="multipart/form-data">
                            @csrf
                            <!--begin::Input group-->
                            <div class="fv-row">
                                <!--begin::Dropzone-->
                                <div class="dropzone" id="dropzone_dn">
                                    <!--begin::Message-->
                                    <div class="dz-message">
                                        <i class="ki-duotone ki-file-up fs-3x text-primary"><span class="path1"></span><span class="path2"></span></i>
                                        <!--begin::Info-->
                                        <div class="col-lg-12 col-md-12 col-12 ms-4 text-center">
                                            <h3 class="fs-lg-1 fw-bold text-gray-900 mb-lg-15">Upload Multiple File By Click On Box</h3>
                                            <span class="fs-3 fw-semibold text-gray-400">Drop files here to upload</span>
                                        </div>
                                        <!--end::Info-->
                                    </div>
                                </div>
                                <!--end::Dropzone-->
                            </div>
                            <!--end::Input group-->
                        </form>
                    </div>
                    <div class="separator my-2 border-2 my-3"></div>
                    <div class="row mb-3">
                        <div class="d-flex flex-row justify-content-between">
                            <button type="button" id="btn_download" class="btn btn-optima">
                                <i class="la la-cloud-download" style="color: white; font-size: x-large"></i>
                                <span>Download Template</span>
                            </button>
                            <button type="button" id="btn_upload" class="btn btn-optima">
                                <i class="la la-cloud-upload" style="color: white; font-size: x-large"></i>
                                <span>Upload</span>
                            </button>
                        </div>
                    </div>
                    <div class="row d-none" id="detail_result">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_dn_upload_attachment" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
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
    <script src="{{ asset('assets/pages/debit-note/dn-upload-attachment/js/dn-upload-attachment-methods.js?v=' . microtime()) }}"></script>
@endsection

@extends('layouts/layoutMaster')

@section('title', 'Master Entity')

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
    <div class="row mb-7">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_entity" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <div class="input-group mb-5">
                                <input class="form-control field_upload" id="file" name="file" type="file" data-stripe="file" placeholder="Choose File"/>
                                <span class="input-group-text">Upload File</span>
                            </div>
                        </div>
                        <div class="separator my-2 border-2"></div>
                        <div class="row">
                            <div class="col-lg-6 col-lg-12 col-12" align="right">
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
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/tools/entity/js/entity-upload.js') }}"></script>
@endsection

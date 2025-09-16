@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/references/distributor/css/distributor-methods.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Distributor</small>
    </span>
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
            <div class="card shadow-sm card_reference">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <h5 class="fw-normal">Files</h5>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-lg-3 col-md-12 col-sm-12 col-12">
                            <select class="form-select form-select-sm" data-control="select2" name="filter_distributor" id="filter_distributor" data-placeholder="Select a Distributor" autocomplete="off">
                            </select>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-12">
                            <div class="separator border-2"></div>
                        </div>
                    </div>

                    @for ($i=1; $i<=5;$i++)
                    <div class="row">
                        <div class="col-lg-1 col-md-1 col-sm-1 col-1 text-end">
                            <label class="col-form-label">{{ @$i }}.</label>
                        </div>
                        <div class="col-lg-4 col-md-6 col-sm-6 col-6">
                            <div class="input-group input-group-sm custom-file-button">
                                <label class="input-group-text fs-12px" for="file_{{ @$i }}" role="button">Browse...</label>
                                <label for="file_{{ @$i }}" class="form-control form-control-sm text-gray-700 fs-12px text-overflow-ellipsis overflow-hidden text-nowrap review_file_label" id="review_file_label_{{ @$i }}" role="button">Choose File</label>
                                <input type="file" class="d-none input_file" id="file_{{ @$i }}" data-row-file="{{ @$i }}">
                            </div>
                        </div>
                        <div class="col-lg-7 col-md-5 col-sm-5 col-5">
                            <button type="button" class="btn btn-sm btn-optima btn_download" value="{{ @$i }}">
                                <span class="fa fa-download"></span> Download
                            </button>
                        </div>
                    </div>
                    @endfor
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')

@endsection

@section('page-script')
    <script src="{{ asset('assets/pages/references/distributor/js/distributor-methods.js?v=' . microtime()) }}"></script>
@endsection

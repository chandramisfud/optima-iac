@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/mapping/wht-type/css/wht-type-form.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Add New</small>
    </span>
@endsection

@section('button-toolbar-right')
    @if(isset($_GET['method']) && $_GET['method'] === 'update')
        @include('toolbars.btn-save-back')
    @else
        @include('toolbars.btn-back-save-exit-add-new')
    @endif
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_mapping_wht_type" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Distributor </label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="distributor" id="distributor" data-placeholder="Select a Distributor"  data-allow-clear="true" tabindex="1">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Sub Activity</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="subActivity" id="subActivity" data-placeholder="Select Sub Activity"  data-allow-clear="true" tabindex="2">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Sub Account</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="subAccount" id="subAccount" data-placeholder="Select Sub Account"  data-allow-clear="true" tabindex="3">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">WHT Type</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="WHTType" id="WHTType" data-placeholder="Select WHT Type"  data-allow-clear="true" tabindex="4">
                                    <option></option>
                                </select>
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
    <script src="{{ asset('assets/pages/mapping/wht-type/js/wht-type-form.js?v=1') }}"></script>
@endsection

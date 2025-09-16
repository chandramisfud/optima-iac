@extends('layouts/layoutMaster')

@section('title', 'Form Mechanism')

@section('vendor-style')

@endsection

@section('page-style')

@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method">Add New</small>
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
                    <form id="form_mechanism" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Entity</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm" data-control="select2" name="entity" id="entity" data-placeholder="Select a Entity"  data-allow-clear="true" tabindex="1">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Sub Category</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm select-subcategory" data-control="select2" name="subcategory" id="subcategory" data-placeholder="Select a Sub Category"  data-allow-clear="true" tabindex="2">
                                    <option value="0">ALL</option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Activity</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm select-activity" data-control="select2" name="activity" id="activity" data-placeholder="Select a Activity"  data-allow-clear="true" tabindex="3">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Sub Activity</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm select-sub-activity" data-control="select2" name="subactivity" id="subactivity" data-placeholder="Select a Sub Activity"  data-allow-clear="true" tabindex="4">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Channel</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm select-channel" data-control="select2" name="channel" id="channel" data-placeholder="Select a Channel"  data-allow-clear="true" tabindex="5">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">SKU</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm select-sku" data-control="select2" name="sku" id="sku" data-placeholder="Select a SKU"  data-allow-clear="true" tabindex="6">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Period</label>
                            <div class="col-lg-6">
                                <div class="input-group input-group-sm">
                                    <input type="text" class="form-control form-control-sm" name="start_date" id="start_date" value="{{ @date('d-m-Y',strtotime(date('Y-01-01'))) }}" autocomplete="off">
                                    <span class="input-group-text fs-12px">to</span>
                                    <input type="text" class="form-control form-control-sm" name="end_date" id="end_date" value="{{ @date('d-m-Y',strtotime(date('Y-12-31'))) }}" autocomplete="off">
                                </div>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Mechanism</label>
                            <div class="col-lg-6">
                                <input class="form-control form-control-sm" type="text" name="mechanism" id="mechanism" autocomplete="off" tabindex="7" />
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
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/master/promo-mechanism/js/promo-mechanism-form.js?v=2') }}"></script>
@endsection

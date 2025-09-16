@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/report/dn-display/css/dn-display-form.css?v=1') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-save-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_invoice" class="form" autocomplete="off">
                        @csrf
                        <div class="row mb-5">
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required" for="dnPeriod">Period</label>
                                    <div class="col-lg-4">
                                        <div class="input-group input-group-sm" id="dialer_period">
                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                <i class="fa fa-minus fs-2"></i>
                                            </button>

                                            <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="dnPeriod" id="dnPeriod" value="{{ @date('Y') }}" autocomplete="off" tabindex="1"/>

                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                <i class="fa fa-plus fs-2"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required" for="invoiceDesc">Description </label>
                                    <div class="col-lg-8">
                                        <input class="form-control form-control-sm" type="text" name="invoiceDesc" id="invoiceDesc" autocomplete="off" tabindex="2"/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required" for="entityId">Entity </label>
                                    <div class="col-lg-8" id="dynamicElEntity">
                                        <select class="form-select form-select-sm" data-control="select2" name="entityId" id="entityId" data-placeholder="Select an Entity"  data-allow-clear="true" tabindex="3">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-form-label required" for="distributorId">Distributor </label>
                                    <div class="col-lg-8">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="distributorId" id="distributorId" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row" id="rowCategory">
                                    <label class="col-lg-4 col-form-label required" id="labelCategory" for="categoryId">Category </label>
                                    <div class="col-lg-8" id="dynamicElCategory">
                                        <select class="form-select form-select-sm" data-control="select2" name="categoryId" id="categoryId" data-placeholder="Select a Category"  data-allow-clear="true" tabindex="4">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label required" for="taxLevel">Tax Level </label>
                                    <div class="col-lg-6" id="dynamicElTaxLevel">
                                        <select class="form-select form-select-sm" data-control="select2" name="taxLevel" id="taxLevel" data-placeholder="Select a Tax Level"  data-allow-clear="true" tabindex="5">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="dppAmount">DPP Amount </label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" value="0" name="dppAmount" id="dppAmount" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label required" for="ppn">PPN </label>
                                    <div class="col-lg-6">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm text-end" name="ppn" id="ppn" value="0" autocomplete="off" tabindex="4"/>
                                            <span class="input-group-text px-5">%</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="invoiceAmount">Invoice Amount </label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" value="0" name="invoiceAmount" id="invoiceAmount" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                    <div class="separator border-3 my-2 border-secondary mb-2"></div>
                    <div class="row mt-2">
                        <span class="d-flex align-items-center fs-3 my-1">Detail Invoice </span>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <div class="row">
                                <div class="col-lg-6 col-12">
                                    <div class="position-relative w-lg-150px w-fhd-250px mb-2">
                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-17 translate-middle ms-6">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_dn_detail_search" autocomplete="off">
                                    </div>
                                </div>
                                <div class="col-lg-6 col-12">
                                    <div class="text-end">
                                        <a href="javascript:void(0)" class="btn btn-sm btn-icon me-2" id="btn_download" title="Download Template Excel">
                                            <i class="fa fa-download" style="font-size: 1.8rem; color: black"></i>
                                        </a>
                                        <a href="javascript:void(0)" class="btn btn-sm btn-icon me-2" id="btn-upload" title="Upload Excel For Validate">
                                            <i class="fa fa-cloud-upload-alt" style="font-size: 2.0rem; color: black"></i>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <form id="form_upload_validate" class="form" autocomplete="off">
                                @csrf
                                <div class="row fv-row" id="form-upload">
                                    <div class="input-group input-group-sm">
                                        <input class="form-control form-control-sm field_upload" id="file" name="file" type="file" data-stripe="file" placeholder="Choose File"/>
                                        <span class="input-group-text">Upload File</span>
                                    </div>
                                    <div class="col-lg-6 col-lg-12 col-12 text-end">
                                        <button type="button" class="btn btn-sm btn-optima fw-bolder mb-2" id="btn_upload">
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
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_dn_detail" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/debit-note/invoice-creation/js/invoice-creation-form.js?v=6') }}"></script>
@endsection

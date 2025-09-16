@extends('layouts/layoutMaster')

@section('title', 'Matrix Promo Approval')

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
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
                    <form id="form_matrix_approval" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Entity</label>
                            <div class="col-lg-3">
                                <select class="form-select form-select-sm" data-control="select2" name="entity" id="entity" data-placeholder="Select an Entity"  data-allow-clear="true" tabindex="2">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Distributor</label>
                            <div class="col-lg-3">
                                <select class="form-select form-select-sm" data-control="select2" name="distributor" id="distributor" data-placeholder="Select a Distributor"  data-allow-clear="true" tabindex="3">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Category</label>
                            <div class="col-lg-3">
                                <select class="form-select form-select-sm" data-control="select2" name="category" id="category" data-placeholder="Select a Category"  data-allow-clear="true" tabindex="4">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Sub Activity Type</label>
                            <div class="col-lg-3">
                                <select class="form-select form-select-sm" data-control="select2" name="sub_activity_type" id="sub_activity_type" data-placeholder="Select a Sub Activity Type"  data-allow-clear="true" tabindex="5">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Channel</label>
                            <div class="col-lg-3">
                                <select class="form-select form-select-sm" data-control="select2" name="channel" id="channel" data-placeholder="Select a Channel"  data-allow-clear="true" tabindex="6">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label">Sub Channel</label>
                            <div class="col-lg-3">
                                <select class="form-select form-select-sm" data-control="select2" name="sub_channel" id="sub_channel" data-placeholder="Select a Sub Channel"  data-allow-clear="true" tabindex="7">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Initiator</label>
                            <div class="col-lg-3">
                                <select class="form-select form-select-sm" data-control="select2" name="initiator" id="initiator" data-placeholder="Select an Initiator"  data-allow-clear="true" tabindex="8">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Min. Investment</label>
                            <div class="col-lg-3">
                                <input class="form-control form-control-sm text-end" type="text" name="min_investment" id="min_investment" placeholder="Enter Min Investment" autocomplete="off" tabindex="9" />
                            </div>
                        </div>

                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Max. Investment</label>
                            <div class="col-lg-3">
                                <input class="form-control form-control-sm text-end" type="text" name="max_investment" id="max_investment" placeholder="Enter Max Investment" autocomplete="off" tabindex="10" />
                            </div>
                        </div>
                    </form>
                    <div class="separator my-2 border-2"></div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="row">
                                <div class="col-lg-3 col-md-3 col-sm-12 mb-2">
                                    <div class="position-relative w-100 me-md-2">
                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-50 translate-middle ms-6">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                    </svg>
                                </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_profile_search" autocomplete="off">
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-5">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                    <table id="dt_profile" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="row">
                                <div class="col-lg-3 col-md-3 col-sm-12 mb-2">
                                    <div class="position-relative w-100 me-md-2">
                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-50 translate-middle ms-6">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                    </svg>
                                </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_matrix_approver_search" autocomplete="off">
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                                    <table id="dt_approver" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    @include('Master::promo-matrix-approval.popup-edit-approver')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/master/promo-matrix-approval/js/promo-matrix-approval-form.js?v=4') }}"></script>
@endsection

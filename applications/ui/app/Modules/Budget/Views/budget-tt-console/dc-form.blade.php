@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/budget/budget-tt-console/css/budget-tt-console-dc-form.css?v=' . microtime()) }}" rel="stylesheet" type="text/css"/>
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
    <div class="row d-none" id="d_progress">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm mb-2">
                <div class="card-body">
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
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_budget_tt_console" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-md-12 col-sm-12 col-12 col-form-label required" for="period">Budget Year</label>
                            <div class="col-lg-2 col-md-3 col-sm-12 col-12">
                                <div class="input-group input-group-sm" id="dialer_period">
                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                        <i class="fa fa-minus fs-2"></i>
                                    </button>

                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="period" id="period" value="" autocomplete="off" tabindex="0"/>

                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                        <i class="fa fa-plus fs-2"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="channel">Channel</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm" data-control="select2" name="channel" id="channel" data-placeholder="Select a Channel"  data-allow-clear="true" tabindex="1">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="distributor">Distributor</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm" data-control="select2" name="distributor" id="distributor" data-placeholder="Select a Distributor"  data-allow-clear="true" tabindex="2">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="groupbrand">Brand</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm" data-control="select2" name="groupbrand" id="groupbrand" data-placeholder="Select a Brand"  data-allow-clear="true" tabindex="3">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="sub_activity_type">Sub Activity Type</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm" data-control="select2" name="sub_activity_type" id="sub_activity_type" data-placeholder="Select a Sub Activity Type"  data-allow-clear="true" tabindex="4">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="sub_category">Sub Category</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm" data-control="select2" name="sub_category" id="sub_category" data-placeholder="Select a Sub Category"  data-allow-clear="true" tabindex="5">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="activity">Activity</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm" data-control="select2" name="activity" id="activity" data-placeholder="Select an Activity"  data-allow-clear="true" tabindex="6">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label" for="sub_activity">Sub Activity</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm" data-control="select2" name="sub_activity" id="sub_activity" data-placeholder="Select a Sub Activity"  data-allow-clear="true" tabindex="7">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="ttPercent">TT %</label>
                            <div class="col-lg-6">
                                <div class="input-group input-group-sm">
                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="ttPercent" id="ttPercent" autocomplete="off" value="0" inputmode="decimal" onClick="this.select();" tabindex="8" />
                                    <span class="input-group-text">%</span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-lg-3 col-form-label" for="budget_name">Budget Name</label>
                            <div class="col-lg-6">
                                <input class="form-control form-control-sm form-control-solid-bg" type="text" name="budget_name" id="budget_name" autocomplete="off" tabindex="9" readonly/>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/js/generate-uuid.js?v=' . microtime()) }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/pages/budget/budget-tt-console/js/budget-tt-console-form-dc.js?v=20') }}"></script>
@endsection

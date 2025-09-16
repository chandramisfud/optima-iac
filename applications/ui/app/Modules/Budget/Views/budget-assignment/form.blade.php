@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')

@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Add New</small>
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
                    <form id="form_budget_assignment" class="form" autocomplete="off">
                        @csrf
                        <div class="row">
                            <div class="col-lg-6 col-md-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required">Budget Year</label>
                                    <div class="col-lg-4">
                                        <div class="input-group input-group-sm" id="dialer_period">
                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                <i class="fa fa-minus fs-2"></i>
                                            </button>

                                            <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="period" id="period" value="{{ @date('Y') }}" autocomplete="off" tabindex="1"/>

                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                <i class="fa fa-plus fs-2"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required">Select Budget Source</label>
                                    <div class="col-lg-8">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="longDesc" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly>
                                            <span class="input-group-text cursor-pointer btn-outline-optima" id="btn_search_budget_source">
                                                <span class="fa fa-search"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label">Budget Owner</label>
                                    <div class="col-lg-8">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="fromOwnerName" id="fromOwnerName" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6 col-md-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label">Entity</label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="entityLongDesc" id="entityLongDesc" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label">Budget Assigned</label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="budgetAssigned" id="budgetAssigned" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label">Budget Amount</label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="budgetAmount" id="budgetAmount" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>

                    <div class="row">
                        <div class="col-12">
                            <div class="separator border-3 my-3"></div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <h5 class="fw-normal">Detail Assignment</h5>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-6 col-12">
                            <form id="form_budget_assignment_dtl" class="form" autocomplete="off">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required">Assign Name</label>
                                    <div class="col-lg-8">
                                        <select class="form-select form-select-sm" data-control="select2" name="profileId" id="profileId" data-placeholder="Select a User"  data-allow-clear="true">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required">Description</label>
                                    <div class="col-lg-8">
                                        <textarea type="text" class="form-control form-control-sm" id="description" name="description" rows="2" autocomplete="off"></textarea>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required">Budget Amount</label>
                                    <div class="col-lg-8">
                                        <input class="form-control form-control-sm text-end" type="text" name="budgetAmountDtl" id="budgetAmountDtl" value="0" autocomplete="off"/>
                                    </div>
                                </div>
                            </form>
                            <div class="row">
                                <div class="col-lg-8 offset-lg-4">
                                    <button type="button" class="btn btn-sm btn-optima text-hover-white" id="btn_add_dtl">
                                        <span class="la la-plus"></span> Add
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <table id="dt_budget_assignment_dtl" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    @include('Budget::budget-assignment.budget-source-list')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-assignment/js/budget-assignment-form.js') }}"></script>
    <script src="{{ asset('assets/pages/budget/budget-assignment/js/budget-source-list.js') }}"></script>
@endsection

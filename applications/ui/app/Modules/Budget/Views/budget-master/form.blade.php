@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

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
                    <form id="form_budget_master" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Budget Name</label>
                            <div class="col-lg-4">
                                <input class="form-control form-control-sm" type="text" name="longDesc" id="longDesc" placeholder="Enter Budget Name" autocomplete="off" tabindex="1" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Entity</label>
                            <div class="col-lg-4">
                                <select class="form-select form-select-sm" data-control="select2" name="entityId" id="entityId" data-placeholder="Select an Entity"  data-allow-clear="true" tabindex="2">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Distributor</label>
                            <div class="col-lg-4">
                                <select class="form-select form-select-sm" data-control="select2" name="distributorId" id="distributorId" data-placeholder="Select a Distributor"  data-allow-clear="true" tabindex="3">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Category</label>
                            <div class="col-lg-4">
                                <select class="form-select form-select-sm" data-control="select2" name="categoryId" id="categoryId" data-placeholder="Select a Category"  data-allow-clear="true" tabindex="4">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Year</label>
                            <div class="col-lg-2">
                                <div class="input-group input-group-sm" id="dialer_period">
                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                        <i class="fa fa-minus fs-2"></i>
                                    </button>

                                    <input class="form-control form-control-sm" type="text" name="year" id="year" placeholder="Enter Year Budget" autocomplete="off" tabindex="5" value="{{ date('Y') }}"/>

                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                        <i class="fa fa-plus fs-2"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Total Budget</label>
                            <div class="col-lg-4">
                                <input class="form-control form-control-sm text-end" type="text" name="budgetAmount" id="budgetAmount" placeholder="Enter Budget Amount" autocomplete="off" tabindex="6"/>
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
    <script src="{{ asset('assets/pages/budget/budget-master/js/budget-master-form.js?v=2') }}"></script>
@endsection


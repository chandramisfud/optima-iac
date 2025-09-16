@extends('layouts/layoutMaster')

@section('title', 'Form Sub Activity Promo')

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
                    <form id="form_subactivity" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label">Ref ID</label>
                            <div class="col-lg-6">
                                <input class="form-control form-control-sm form-control-solid-bg" type="text" name="refId" id="refId" placeholder="Generate by System" disabled/>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Category</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm" data-control="select2" name="category" id="category" data-placeholder="Select a Category"  data-allow-clear="true" tabindex="1">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Sub Category</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm select-subcategory" data-control="select2" name="subcategory" id="subcategory" data-placeholder="Select a Sub Category"  data-allow-clear="true" tabindex="2">
                                    <option></option>
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
                            <label class="col-lg-3 col-form-label required">Sub Activity Type</label>
                            <div class="col-lg-6">
                                <select class="form-select form-select-sm select-activitytype" data-control="select2" name="activitytype" id="activitytype" data-placeholder="Select a Sub Activity Type"  data-allow-clear="true" tabindex="4">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label">Short Desc</label>
                            <div class="col-lg-6">
                                <input class="form-control form-control-sm" type="text" name="shortDesc" id="shortDesc" autocomplete="off" tabindex="5" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">Long Desc</label>
                            <div class="col-lg-6">
                                <input class="form-control form-control-sm" type="text" name="longDesc" id="longDesc" autocomplete="off" tabindex="6" />
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
    <script src="{{ asset('assets/pages/master/sub-activity/js/sub-activity-form.js?v=2') }}"></script>
@endsection

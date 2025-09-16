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
                    <form id="form_mapping_sub_activity_promo_recon" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="categoryId">Category </label>
                            <div class="col-lg-5" id="elCategoryId">
                                <select class="form-select form-select-sm" data-control="select2" name="categoryId" id="categoryId" data-placeholder="Select a Category"  data-allow-clear="true" tabindex="1">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="subCategoryId">Sub Category</label>
                            <div class="col-lg-5" id="elSubCategoryId">
                                <select class="form-select form-select-sm" data-control="select2" name="subCategoryId" id="subCategoryId" data-placeholder="Select a Sub Category"  data-allow-clear="true" tabindex="2">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="activityId">Activity</label>
                            <div class="col-lg-5" id="elActivityId">
                                <select class="form-select form-select-sm" data-control="select2" name="activityId" id="activityId" data-placeholder="Select an Activity"  data-allow-clear="true" tabindex="3">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="subActivityId">Sub Activity</label>
                            <div class="col-lg-5" id="elSubActivityId">
                                <select class="form-select form-select-sm" data-control="select2" name="subActivityId" id="subActivityId" data-placeholder="Select a Sub Activity"  data-allow-clear="true" tabindex="4">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="allowEdit">Action</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="allowEdit" id="allowEdit" data-placeholder="Select an Action"  data-allow-clear="true" tabindex="5">
                                    <option></option>
                                    <option value="0">During promo period: not allow edit (still allow cancel)</option>
                                    <option value="1">During promo period: allow edit</option>
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
    <script src="{{ asset('assets/pages/mapping/sub-activity-promo-recon/js/sub-activity-promo-recon-form.js?v=?v=' . microtime()) }}"></script>
@endsection

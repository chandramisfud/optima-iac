@extends('layouts/layoutMaster')

@section('title', 'Form User Group Rights Configuration')

@section('vendor-style')
    <link href="{{ asset('/assets/plugins/custom/jstree/jstree.bundle.css') }}" rel="stylesheet" type="text/css" />
@endsection

@section('page-style')
    <link href="{{ asset('/assets/pages/useraccess/user-group/css/usergrouprights-form.css') }}" rel="stylesheet" type="text/css" />
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
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
        <div class="col-md-6 col-12 mb-5">
            <div class="card shadow-sm card_menu">
                <div class="card-body">
                    <div class="row justify-content-center mb-3">
                        <div class="col-lg-3 col-md-3 col-sm-12 col-12 my-auto text-center text-sm-center text-lg-end text-md-end">
                            User Group Rights
                        </div>
                        <div class="col-lg-7 col-md-9 col-sm-12">
                            <div class="row">
                                <div class="col-lg-10 col-md-10 col-12">
                                    <select class="form-select form-select-sm" data-control="select2" name="usergrouprights" id="usergrouprights" data-placeholder="Select a User Group Rights"  data-allow-clear="true" tabindex="1">
                                        <option></option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row fv-row overflow-auto h-375px">
                        <div class="col-lg-12 col-12" id="el_jstree">
                            <div id="tree-container" class="tree-demo"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-12 mb-3">
            <div class="card shadow-sm d-none card_rights" id="card_rights">
                <div class="card-header">
                    <h3 class="card-title" id="title_hak_akses">Configuration Menu</h3>
                </div>
                <div class="card-body">
                    <div class="col-12 col-form-label">
                        <form class="form" id="form_user_rights">
                            <div class="d-flex align-items-center mb-5">
                                <label class="form-check form-check-custom me-5" id="checkbox_create">
                                    <input class="form-check-input h-20px w-20px" type="checkbox" name="create_rec" id="create_rec">
                                    <span class="form-check-label fw-bold">Create</span>
                                </label>
                                <label class="form-check form-check-custom me-5" id="checkbox_update">
                                    <input class="form-check-input h-20px w-20px" type="checkbox" name="update_rec" id="update_rec">
                                    <span class="form-check-label fw-bold">Modify</span>
                                </label>
                                <label class="form-check form-check-custom me-5" id="checkbox_delete">
                                    <input class="form-check-input h-20px w-20px" type="checkbox" name="delete_rec" id="delete_rec">
                                    <span class="form-check-label fw-bold">Delete</span>
                                </label>
                                <label class="form-check form-check-custom me-5" id="checkbox_approve">
                                    <input class="form-check-input h-20px w-20px" type="checkbox" name="approve_rec" id="approve_rec">
                                    <span class="form-check-label fw-bold">Approve</span>
                                </label>
                            </div>
                        </form>
                        <span class="form-text text-muted">Please put thick mark to enable the access</span>
                    </div>
                </div>
                <div class="card-footer text-end">
                    <button type="button" class="btn btn-sm btn-optima" id="btn_save">
                        <span class="indicator-label">
                            <span class="fa fa-check"></span> Submit
                        </span>
                        <span class="indicator-progress">Saving...
                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                        </span>
                    </button>

                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/jstree/jstree.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/pages/useraccess/user-group/js/usergrouprights-form.js?v=1') }}"></script>
@endsection

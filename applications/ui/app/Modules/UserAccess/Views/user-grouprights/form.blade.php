@extends('layouts/layoutMaster')

@section('title', 'Form User Group Rights')

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
                    <form id="form_groupuserrights" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">User Group Rights ID</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="userlevel" id="userlevel" placeholder="Enter your User Group Rights ID" autocomplete="off" tabindex="1" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">User Group Menu</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="usergroupid" id="usergroupid" data-placeholder="Select a User Group Menu"  data-allow-clear="true" tabindex="2">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required">User Group Rights Name</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="levelname" id="levelname" placeholder="Enter your User Group Rights Name" autocomplete="off" tabindex="3" />
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
    <script src="{{ asset('assets/pages/useraccess/user-grouprights/js/usergrouprights-form.js?v=4') }}"></script>
@endsection

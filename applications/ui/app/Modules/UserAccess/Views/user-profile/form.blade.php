@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/useraccess/user-profile/css/userprofile-form.css') }}"  rel="stylesheet" type="text/css" />
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
                    <form id="form_userprofile" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="profileid">Profile ID</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="profileid" id="profileid" autocomplete="off" placeholder="Enter your Profile ID" tabindex="1" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="profilename">Profile Name</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="profilename" id="profilename" placeholder="Enter your Profile Name" autocomplete="off" tabindex="2" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="email">Email</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="email" id="email" placeholder="Enter your Email" autocomplete="off" tabindex="3" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="department">Department</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="department" id="department" placeholder="Enter your Department" autocomplete="off" tabindex="4" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="jobtitle">Job Title</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="jobtitle" id="jobtitle" placeholder="Enter your Job Title" autocomplete="off" tabindex="5" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="usergroupid">User Group Menu</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="usergroupid" id="usergroupid" data-placeholder="Select a Group Menu"  data-allow-clear="true" tabindex="6">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="usergrouplevel">User Group Rights</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="usergrouplevel" id="usergrouplevel" data-placeholder="Select a Group Rights"  data-allow-clear="true" tabindex="7">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="distributor">Coverage</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="distributor" id="distributor" data-placeholder="Select a Distributor"  data-allow-clear="true" multiple="multiple" tabindex="8">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="category">Category</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="category" id="category" data-placeholder="Select a Category"  data-allow-clear="true" multiple="multiple" tabindex="8">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label" for="channelId">Channel</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="channelId" id="channelId" data-placeholder="Select a Channel"  data-allow-clear="true" multiple="multiple" tabindex="9">
                                    <option></option>
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
    <script src="{{ asset('assets/pages/useraccess/user-profile/js/userprofile-form.js?v=3') }}"></script>
@endsection

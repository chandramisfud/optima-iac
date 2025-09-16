@extends('layouts/layoutMaster')

@section('title', 'Form User')

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
            <div class="card shadow-sm card_form mb-2">
                <div class="card-body">
                    <form id="form_user" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-12 col-form-label required">Email</label>
                            <div class="col-lg-5  col-12">
                                <input class="form-control form-control-sm" type="text" name="email" id="email" autocomplete="off" placeholder="Enter your email" tabindex="1" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3  col-12 col-form-label required">Name</label>
                            <div class="col-lg-5  col-12">
                                <input class="form-control form-control-sm" type="text" name="username" id="username" autocomplete="off" placeholder="Enter your name" tabindex="2" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3  col-12 col-form-label required">Contact Info</label>
                            <div class="col-lg-5  col-12">
                                <input class="form-control form-control-sm" type="text" name="contactinfo" id="contactinfo" autocomplete="off" placeholder="Enter your contact info" tabindex="3" />
                            </div>
                        </div>
                    </form>
                </div>
            </div>

            <div class="card shadow-sm card_form_profile">
                <div class="card-body">
                    <div class="row fv-row">
                        <div class="col-lg-6 col-12">
                            <div class="row">
                                <div class="col-lg-12 col-12">
                                    <div class="position-relative w-md-400px me-md-2">
                                        <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-50 translate-middle ms-6">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                            </svg>
                                        </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_users_mstprofile_Search" autocomplete="off">
                                    </div>
                                </div>
                            </div>
                            <table id="dt_users_mstprofile" class="table table-striped table-row-bordered table-responsive table-sm table-hover">
                            </table>
                        </div>
                        <div class="col-lg-6  col-12">
                            <div class="row">
                                <div class="col-lg-12 col-12">
                                    <div class="position-relative w-md-400px me-md-2">
                                        <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-50 translate-middle ms-6">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                            </svg>
                                        </span>
                                        <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_users_profile_Search" autocomplete="off">
                                    </div>
                                </div>
                            </div>
                            <table id="dt_users_profile" class="table table-striped table-row-bordered table-responsive table-sm table-hover">
                            </table>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/pages/useraccess/user/js/user-form.js?v=3') }}"></script>
@endsection

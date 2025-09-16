@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/flatpickr/flatpickr-month-select.css') }}"  rel="stylesheet" type="text/css" />
    <link href="{{ asset('assets/plugins/custom/fullcalendar/core/main.css') }}" rel="stylesheet" type="text/css" />
    <link href="{{ asset('assets/plugins/custom/fullcalendar/timeline/main.css') }}" rel="stylesheet" type="text/css" />
    <link href="{{ asset('assets/plugins/custom/fullcalendar/resource-timeline/main.css') }}" rel="stylesheet" type="text/css" />
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/dashboard/promo-calendar/css/index.css') }}"  rel="stylesheet" type="text/css" />
@endsection

@section('toolbar')

@endsection

@section('content')
    <div class="header-dashboard mb-3" id="header-dashboard">
        <div class="row">
            <div class="card shadow-sm card-flush card-px-0 rounded-0 card_dashboard_header">
                <div class="card-body card-body-top-data p-0">
                    <div class="row">
                        <div class="col-lg-2 col-md-12 col-12 mb-2">
                            <div class="row">
                                <label class="col-form-label col-lg-12 col-12 pb-1 pt-0">View Mode</label>
                                <div class="col-lg-12 col-12 ">
                                    <select class="form-select form-select-sm" name="viewMode" id="viewMode">
                                        <option value="YTD" selected>YTD</option>
                                        <option value="MTD">MTD</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-12 col-12 mb-2">
                            <div class="row">
                                <label class="col-form-label col-lg-12 col-12 pb-1 pt-0">Year/Month</label>
                                <div class="col-lg-12 col-12">
                                    <input class="form-control form-control-sm" type="text" name="year_month" id="year_month" data-td-toggle="datetimepicker" autocomplete="off" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-12 col-12 mb-2">
                            <div class="row">
                                <label class="col-form-label col-lg-12 col-12 pb-1 pt-0">Entity</label>
                                <div class="col-lg-12 col-12">
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_entity" id="filter_entity" data-placeholder="Select an Entity"  data-allow-clear="true" autocomplete="off">
                                        <option></option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-12 col-12 mb-2">
                            <div class="row">
                                <label class="col-form-label col-lg-12 col-12 pb-1 pt-0">Channel</label>
                                <div class="col-lg-12 col-12">
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_channel" id="filter_channel" data-placeholder="Select a Channel"  data-allow-clear="true" autocomplete="off">
                                        <option></option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-12 col-12 mb-2">
                            <div class="row">
                                <label class="col-form-label col-lg-12 col-12 pb-1 pt-0">Account</label>
                                <div class="col-lg-12 col-12">
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_account" id="filter_account" data-placeholder="Select an Account"  data-allow-clear="true" autocomplete="off">
                                        <option></option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-12 col-12 mb-2">
                            <div class="row">
                                <label class="col-form-label col-lg-12 col-12 pb-1 pt-0">Category</label>
                                <div class="col-lg-12 col-12">
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_category" id="filter_category" data-placeholder="Select a Category"  data-allow-clear="true" autocomplete="off">
                                        <option></option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="body-dashboard" id="body-dashboard">
        <div class="row">
            <div class="col-12">
                <div class="card shadow-sm card-flush card-px-0 card_dashboard_calendar">
                    <div class="card-body card-body-top-data p-0">
                        <div class="row mb-3">
                            <div class="col-lg-8 col-12">
                                <div class="d-flex flex-lg-row flex-column">
                                    <label class="col-form-label fw-bolder px-lg-5 px-0 me-10">Status : </label>
                                    <label class="col-form-label fw-bolder px-5 align-middle text-center w-100 w-lg-14-2-perc" id="lbl_promo_plan">PROMO PLAN </label>
                                    <label class="col-form-label fw-bolder px-5 align-middle text-center w-100 w-lg-14-2-perc" id="lbl_approval">APPROVAL </label>
                                    <label class="col-form-label fw-bolder px-5 align-middle text-center w-100 w-lg-14-2-perc" id="lbl_approved">APPROVED </label>
                                    <label class="col-form-label fw-bolder px-5 align-middle text-center w-100 w-lg-14-2-perc" id="lbl_claimed">CLAIMED </label>
                                    <label class="col-form-label fw-bolder px-5 align-middle text-center w-100 w-lg-14-2-perc" ><i class="fa fa-exclamation-circle text-yellow fs-5"></i> Warning</label>
                                    <label class="col-form-label fw-bolder px-5 align-middle text-center w-100 w-lg-14-2-perc" ><i class="fa fa-exclamation-circle text-red fs-5"></i> Critical</label>
                               </div>
                            </div>
                            <div class="col-lg-4 col-12">
                                <div class="d-flex flex-lg-row flex-column justify-content-center align-items-center">
                                    <div class="inner-addon left-addon right-addon pt-lg-1 w-100">
                                        <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                            </svg>
                                        </span>
                                        <input type="text" class="form-control form-control-sm my-auto ps-10" name="search" value="" placeholder="Search" id="search_activity_desc" autocomplete="off">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <div class="col-12">
                                <div id="promo_calendar"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/flatpickr/flatpickr-month-select.js') }}"></script>
    <script src="{{ asset ('assets/plugins/custom/fullcalendar/core/main.js') }}"></script>
    <script src="{{ asset ('assets/plugins/custom/fullcalendar/timeline/main.js') }}"></script>
    <script src="{{ asset ('assets/plugins/custom/fullcalendar/resource-common/main.js') }}"></script>
    <script src="{{ asset ('assets/plugins/custom/fullcalendar/resource-timeline/main.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/dashboard/promo-calendar/js/index.js') }}"></script>
@endsection

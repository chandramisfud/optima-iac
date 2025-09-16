@extends('layouts/layoutMaster')

@section('title', 'Approval Reminder Configuration')

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/configuration/reminder/css/reminder-methods.css') }}"  rel="stylesheet" type="text/css" />
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Form</small>
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
                <div class="card-header">
                    <span class="card-title text-gray-800 fs-4 py-2">Reminder Configuration
                    <small class="text-muted fs-6 fw-bold fs-4 mt-1 ms-3">for Approver & Creator</small>
                    </span>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="table-responsive">
                            <table class="table table-sm w-100">
                                <thead>
                                <tr>
                                    <th class="min-w-150px min-w-lg-150px min-w-md-150px min-w-sm-150px text-nowrap align-middle bg-optima text-white ps-2 py-1">
                                        Status
                                    </th>
                                    <th class="min-w-450px min-w-lg-450px min-w-md-450px min-w-sm-450px text-nowrap align-middle bg-optima text-white py-1">
                                        Reminder Limit
                                    </th>
                                    <th class="min-w-250px min-w-lg-250px min-w-md-250px min-w-sm-250px text-nowrap align-middle bg-optima text-white py-1 ps-4">
                                        Notif Frequency
                                    </th>
                                    <th class="min-w-250px min-w-lg-250px min-w-md-250px min-w-sm-250px text-nowrap align-middle bg-optima text-white py-1 ps-6">
                                        Last Updated
                                    </th>
                                </tr>
                                </thead>
                                <tbody>
                                <tr>
                                    <td class="p-0">
                                        <div class="separator border-secondary border-3 my-2"></div>
                                        <label class="col-md-12 ms-1">
                                            <div class="col-lg-7 pt-2">
                                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon">
                                                    <g id="g4746">
                                                        <path style="fill:none;fill-rule:evenodd;stroke:none;stroke-width:1" d="M 0,0 H 24 V 24 H 0 Z" id="bound" />
                                                        <path style="opacity:1;fill:#00e600;fill-opacity:0.99574471;fill-rule:evenodd;stroke:none;stroke-width:1" d="M 22,12 A 10,10 0 0 1 12,22 10,10 0 0 1 2,12 10,10 0 0 1 12,2 10,10 0 0 1 22,12 Z" id="Oval-5" />
                                                        <path style="fill:#ffffff;fill-opacity:1;fill-rule:evenodd;stroke:none;stroke-width:1" d="m 12,7 c 0.554,0 1,0.446 1,1 v 6 c 0,0.554 -0.446,1 -1,1 -0.554,0 -1,-0.446 -1,-1 V 8 c 0,-0.554 0.446,-1 1,-1 z" id="Rectangle-9" />
                                                        <path style="fill:#ffffff;fill-opacity:1;fill-rule:evenodd;stroke:none;stroke-width:1" d="m 12,16 c 0.554,0 1,0.446 1,1 0,0.554 -0.446,1 -1,1 -0.554,0 -1,-0.446 -1,-1 0,-0.554 0.446,-1 1,-1 z" id="Rectangle-9-Copy" />
                                                    </g>
                                                </svg>Pre-Caution
                                            </div>
                                        </label>
                                    </td>
                                    <td class="p-0">
                                        <div class="separator border-secondary border-3 my-2"></div>
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                            <div class="input-group image-input-sm">
                                                <div class="input-group input-group-sm w-md-200px w-200px" id="dialer_pre_caution_limit_from">
                                                    <span class="input-group-text" id="basic-addon1">From</span>
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="pre_caution_from" id="pre_caution_from" value="0" tabindex="1"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary input-dialer-from" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                </div>
                                                <div class="input-group input-group-sm w-md-250px w-250px" id="dialer_pre_caution_limit_to">
                                                    <span class="input-group-text input-dialer-to" id="basic-addon1">From</span>
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="pre_caution_to" id="pre_caution_to" value="0" tabindex="2"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>

                                                    <span class="input-group-text" id="basic-addon1">Days</span>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="p-0">
                                        <div class="separator border-secondary border-3 my-2"></div>
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                            <div class="input-group image-input-sm ms-3">
                                                <div class="input-group input-group-sm w-md-250px w-250px" id="dialer_pre_caution_notif_frequency">
                                                    <span class="input-group-text" id="basic-addon1">Every</span>
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="pre_caution_frequency" id="pre_caution_frequency" value="0" tabindex="3"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                    <span class="input-group-text" id="basic-addon1">Days</span>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="p-0">
                                        <div class="separator border-secondary border-3 my-2"></div>
                                        <label class="col-md-12 ms-1 ps-4" id="pre_caution_info_update"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p-0">
                                        <label class="col-md-12 ms-1">
                                            <div class="col-lg-7 pt-2">
                                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon">
                                                    <g id="g4746">
                                                        <path style="fill:none;fill-rule:evenodd;stroke:none;stroke-width:1" d="M 0,0 H 24 V 24 H 0 Z" id="bound" />
                                                        <path style="opacity:1;fill:#ffff00fe;fill-opacity:0.99574471;fill-rule:evenodd;stroke:none;stroke-width:1" d="M 22,12 A 10,10 0 0 1 12,22 10,10 0 0 1 2,12 10,10 0 0 1 12,2 10,10 0 0 1 22,12 Z" id="Oval-5" />
                                                        <path style="fill:#808080ff;fill-opacity:1;fill-rule:evenodd;stroke:none;stroke-width:1" d="m 12,7 c 0.554,0 1,0.446 1,1 v 6 c 0,0.554 -0.446,1 -1,1 -0.554,0 -1,-0.446 -1,-1 V 8 c 0,-0.554 0.446,-1 1,-1 z" id="Rectangle-9" />
                                                        <path style="fill:#808080ff;fill-opacity:1;fill-rule:evenodd;stroke:none;stroke-width:1" d="m 12,16 c 0.554,0 1,0.446 1,1 0,0.554 -0.446,1 -1,1 -0.554,0 -1,-0.446 -1,-1 0,-0.554 0.446,-1 1,-1 z" id="Rectangle-9-Copy" />
                                                    </g>
                                                </svg> Warning
                                            </div>
                                        </label>
                                    </td>
                                    <td class="p-0">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                            <div class="input-group image-input-sm">
                                                <div class="input-group input-group-sm w-md-200px w-200px" id="dialer_warning_limit_from">
                                                    <span class="input-group-text" id="basic-addon1">From</span>
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="warning_from" id="warning_from" value="0" tabindex="4"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary input-dialer-from" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                </div>
                                                <div class="input-group input-group-sm w-md-250px w-250px" id="dialer_warning_limit_to">
                                                    <span class="input-group-text input-dialer-to" id="basic-addon1">From</span>
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="warning_to" id="warning_to" value="0" tabindex="5"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>

                                                    <span class="input-group-text" id="basic-addon1">Days</span>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="p-0">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                            <div class="input-group image-input-sm ms-3">
                                                <div class="input-group input-group-sm w-md-250px w-250px" id="dialer_warning_notif_frequency">
                                                    <span class="input-group-text" id="basic-addon1">Every</span>
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="warning_frequency" id="warning_frequency" value="0" tabindex="6"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                    <span class="input-group-text" id="basic-addon1">Days</span>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="p-0">
                                        <label class="col-md-12 ms-1 ps-4" id="warning_info_update"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p-0">
                                        <label class="col-md-12 ms-1">
                                            <div class="col-lg-7 pt-2">
                                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon">
                                                    <g id="g4746">
                                                        <path style="fill:none;fill-rule:evenodd;stroke:none;stroke-width:1" d="M 0,0 H 24 V 24 H 0 Z" id="bound" />
                                                        <path style="opacity:1;fill:#ff0000fe;fill-opacity:0.99574471;fill-rule:evenodd;stroke:none;stroke-width:1" d="M 22,12 A 10,10 0 0 1 12,22 10,10 0 0 1 2,12 10,10 0 0 1 12,2 10,10 0 0 1 22,12 Z" id="Oval-5" />
                                                        <path style="fill:#ffffff;fill-opacity:1;fill-rule:evenodd;stroke:none;stroke-width:1" d="m 12,7 c 0.554,0 1,0.446 1,1 v 6 c 0,0.554 -0.446,1 -1,1 -0.554,0 -1,-0.446 -1,-1 V 8 c 0,-0.554 0.446,-1 1,-1 z" id="Rectangle-9" />
                                                        <path style="fill:#ffffff;fill-opacity:1;fill-rule:evenodd;stroke:none;stroke-width:1" d="m 12,16 c 0.554,0 1,0.446 1,1 0,0.554 -0.446,1 -1,1 -0.554,0 -1,-0.446 -1,-1 0,-0.554 0.446,-1 1,-1 z" id="Rectangle-9-Copy" />
                                                    </g>
                                                </svg> Critical
                                            </div>
                                        </label>
                                    </td>
                                    <td class="p-0">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                            <div class="input-group image-input-sm">
                                                <div class="input-group input-group-sm w-md-450px w-450px" id="dialer_danger_limit">
                                                    <span class="input-group-text" id="basic-addon1">More than or equal to</span>
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="critical_days" id="critical_days" value="0" tabindex="7"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary input-dialer-from" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                    <span class="input-group-text" id="basic-addon1">Days</span>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="p-0">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                            <div class="input-group image-input-sm ms-3">
                                                <div class="input-group input-group-sm w-md-250px w-250px" id="dialer_danger_notif_frequency">
                                                    <span class="input-group-text" id="basic-addon1">Every</span>
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="critical_frequency" id="critical_frequency" value="0" tabindex="8"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                    <span class="input-group-text" id="basic-addon1">Days</span>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="p-0">
                                        <label class="col-md-12 ms-1 ps-4" id="critical_info_update"></label>
                                    </td>
                                </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')

@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/configuration/reminder/js/reminder-methods.js?v=3') }}"></script>
@endsection

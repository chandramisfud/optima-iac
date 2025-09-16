@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/flatpickr/flatpickr-month-select.css') }}"  rel="stylesheet" type="text/css" />
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/dashboard/main/css/index.css?v=' . microtime()) }}"  rel="stylesheet" type="text/css" />
@endsection

@section('toolbar')

@endsection

@section('content')
    <div class="px-lg-0 sticky-top-header-dashboard">
        <div class="header-dashboard" id="header-dashboard">
            <div class="row mb-5">
                <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                    <div class="card shadow-sm card-flush card-px-0 card_dashboard_header">
                        <div class="card-body card-body-top-data p-0">
                            <div class="d-lg-flex justify-content-between align-items-center flex-wrap flex-lg-fill flex-md-fill flex-sm-fill d-grid gap-2">
                                <div class="d-flex align-items-center ms-1 me-1 w-lg-17">
                                    <img src="{{ asset('/assets/media/custom/wallet.png') }}" class="m-3 w-35px" alt="">
                                    <div class="m-0">
                                        <span class="fw-semibold text-dark-blue fw-bolder d-block text-wrap fs-optima-12">BUDGET DEPLOYMENT</span>
                                        <span class="fw-bolder text-light-blue fs-optima-12" id="txt_budget_deployment">0</span>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center me-1 w-lg-17">
                                    <img src="{{ asset('/assets/media/custom/planning.png') }}" class="m-3 w-35px" alt="">
                                    <div class="m-0">
                                        <span class="fw-semibold text-dark-blue fw-bolder d-block fs-optima-12">PROMO PLANNING</span>
                                        <span class="fw-bolder text-light-blue fs-optima-12" id="txt_promo_planning">0</span>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center me-1 w-lg-17">
                                    <img src="{{ asset('/assets/media/custom/calendar.png') }}" class="m-3 w-35px" alt="">
                                    <div class="m-0">
                                        <span class="fw-semibold text-dark-blue fw-bolder d-block fs-optima-12">PROMO CREATION</span>
                                        <span class="fw-bolder text-light-blue fs-optima-12" id="txt_promo_creation">0</span>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center me-1 w-lg-17">
                                    <img src="{{ asset('/assets/media/custom/document.png') }}" class="m-3 w-35px" alt="">
                                    <div class="m-0">
                                        <span class="fw-semibold text-dark-blue fw-bolder d-block fs-optima-12">TOTAL CLAIMS</span>
                                        <span class="fw-bolder text-light-blue text-hover-primary fs-optima-12" id="txt_total_claims">0</span>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center me-1 w-lg-17">
                                    <img src="{{ asset('/assets/media/custom/hand-shake.png') }}" class="m-3 w-35px" alt="">
                                    <div class="m-0">
                                        <span class="fw-semibold text-dark-blue fw-bolder d-block fs-optima-12">PAID CLAIMS</span>
                                        <span class="fw-bolder text-light-blue text-hover-primary fs-optima-12" id="txt_paid_claims">0</span>
                                    </div>
                                </div>
                                <div class="d-flex align-items-end me-0 me-lg-2">
                                    <button class="btn btn-active-icon-optima btn-text-primary ps-3 pe-1" id="btn_show_filter">
                                        <i class="fa fa-cog fs-2rem text-gear-color"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row" id="card_filter">
                <div class="col-lg-12 col-md-12 col-sm-12 col-12 mb-5">
                    <div class="card shadow-sm card-flush card-px-0 card_filter_header">
                        <div class="card-body px-2">
                            <div class="d-lg-flex align-items-end flex-wrap flex-lg-fill flex-md-fill flex-sm-fill d-grid gap-2 justify-content-lg-between">
                                <div class="d-flex flex-column flex-fill align-items-start ms-1 me-1 w-lg-10">
                                    <span class="text-dark-blue fs-6 fw-bold ms-1">View Mode</span>
                                    <select class="form-select form-select-sm" name="viewMode" id="viewMode" autocomplete="off">
                                        <option value="YTD" selected>YTD</option>
                                        <option value="MTD">MTD</option>
                                    </select>
                                </div>
                                <div class="d-flex flex-column flex-fill align-items-start ms-1 me-1 w-lg-10">
                                    <span class="text-dark-blue fs-6 fw-bold">Year/Month</span>
                                    <input class="form-control form-control-sm" type="text" name="year_month" id="year_month" data-td-toggle="datetimepicker" autocomplete="off" />
                                </div>
                                <div class="d-flex flex-column flex-fill align-items-start ms-1 me-1 w-lg-20">
                                    <span class="text-dark-blue fs-6 fw-bold">Channel</span>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_channel" id="filter_channel" data-placeholder="Select a Channel"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                                <div class="d-flex flex-column flex-fill align-items-start ms-1 me-1 w-lg-20">
                                    <span class="text-dark-blue fs-6 fw-bold">Account</span>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_account" id="filter_account" data-placeholder="Select an Account"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                                <div class="d-flex flex-column flex-fill align-items-start ms-1 me-1 w-lg-20">
                                    <span class="text-dark-blue fs-6 fw-bold">Category</span>
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_category" id="filter_category" data-placeholder="Select a Category"  data-allow-clear="true" multiple="multiple" autocomplete="off">
                                    </select>
                                </div>
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_view_header">
                                    <span class="indicator-label">
                                        <span class="fa fa-search"></span> View
                                    </span>
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="mb-lg-3 px-lg-0">
        <div class="body-dashboard">
            <div class="row content">
                <div class="col-lg-62 h-100">
                    <div class="row mb-0 mb-lg-5">
                        <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-lg-0 mb-3">
                            <div class="card shadow-sm card-flush card-px-0 card_promo_created_vs_budget">
                                <div class="card-header">
                                    <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap my-3 fs-optima-12">Promo Created VS Budget</span>
                                </div>
                                <div class="separator border-5 line-card"></div>
                                <div class="card-body px-0 text-center">
                                    <div class="position-relative h-125px w-100" id="chart_promo_created"></div>
                                </div>
                                <div class="card-footer pb-0 pt-0">
                                    <div class="separator border-3 line-card"></div>
                                    <div class="d-flex">
                                        <div class="d-flex align-middle w-20 align-items-center">
                                            <span class="fw-semibold my-auto mx-auto text-dark-blue fw-bolder d-block text-wrap fs-optima-12" id="txt_promo_created">0</span>
                                        </div>
                                        <div class="vr w-3px"></div>
                                        <div class="d-flex py-1 flex-column align-middle w-60 align-items-center">
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap fs-optima-12">Promo ID </span>
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap fs-optima-12">Created</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-lg-0 mb-3">
                            <div class="card shadow-sm card-flush card-px-0 card_promo_approved">
                                <div class="card-header">
                                    <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  my-3 fs-optima-12">Promo Approved</span>
                                </div>
                                <div class="separator border-5 line-card"></div>
                                <div class="card-body px-0 text-center">
                                    <div class="position-relative h-125px w-100" id="chart_promo_approved"></div>
                                </div>
                                <div class="card-footer pb-0 pt-0">
                                    <div class="separator border-3 line-card"></div>
                                    <div class="d-flex">
                                        <div class="d-flex align-middle w-20 align-items-center">
                                            <span class="fw-semibold my-auto mx-auto text-dark-blue fw-bolder d-block text-wrap fs-optima-12" id="txt_promo_approved">0</span>
                                        </div>
                                        <div class="vr w-3px"></div>
                                        <div class="d-flex py-1 flex-column align-middle w-60 align-items-center">
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap fs-optima-12">Promo ID </span>
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap fs-optima-12">Approved</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-lg-0 mb-3">
                            <div class="card shadow-sm card-flush card-px-0 card_promo_reconciled">
                                <div class="card-header">
                                    <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  fs-optima-12  my-3 fs-optima-12">Promo Reconciled</span>
                                </div>
                                <div class="separator border-5 line-card"></div>
                                <div class="card-body px-0 text-center">
                                    <div class="position-relative h-125px w-100" id="chart_promo_reconciled"></div>
                                </div>
                                <div class="card-footer pb-0 pt-0">
                                    <div class="separator border-3 line-card"></div>
                                    <div class="d-flex">
                                        <div class="d-flex align-middle w-20 align-items-center">
                                            <span class="fw-semibold my-auto mx-auto text-dark-blue fw-bolder d-block text-wrap fs-optima-12" id="txt_promo_reconciled">0</span>
                                        </div>
                                        <div class="vr w-3px"></div>
                                        <div class="d-flex py-1 flex-column align-middle w-60 align-items-center">
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap fs-optima-12">Promo ID </span>
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap fs-optima-12">Reconciled</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-lg-0 mb-3">
                            <div class="card shadow-sm card-flush card-px-0 card_promo_created_on_time">
                                <div class="card-header">
                                    <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  fs-optima-12  my-3">Promo Created On-Time</span>
                                </div>
                                <div class="separator border-5 line-card"></div>
                                <div class="card-body px-0 text-center">
                                    <div class="position-relative h-125px w-100" id="chart_promo_created_ontime"></div>
                                </div>
                                <div class="card-footer pb-0 pt-0">
                                    <div class="separator border-3 line-card"></div>
                                    <div class="d-flex">
                                        <div class="d-flex align-middle w-20 align-items-center">
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  fs-optima-12 mb-2" id="txt_avg_days_created_bfr_promo_start">0</span>
                                        </div>
                                        <div class="vr w-3px"></div>
                                        <div class="d-flex  h-40px flex-column align-middle w-60 align-items-center">
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-nowrap fs-optima-12">Average Days Created</span>
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-nowrap fs-optima-12">Before Promo Start</span>
                                        </div>
                                        <div class="d-flex  h-40px flex-column align-middle w-20 align-items-center">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-lg-0 mb-3">
                            <div class="card shadow-sm card-flush card-px-0 card_promo_approved_on_time">
                                <div class="card-header">
                                    <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  fs-optima-12 my-3">Promo Approved On-Time</span>
                                </div>
                                <div class="separator border-5 line-card"></div>
                                <div class="card-body px-0 text-center">
                                    <div class="position-relative h-125px w-100" id="chart_promo_approved_ontime"></div>
                                </div>
                                <div class="card-footer pb-0 pt-0">
                                    <div class="separator border-3 line-card"></div>
                                    <div class="d-flex">
                                        <div class="d-flex align-middle w-20 align-items-center">
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  fs-optima-12 mb-2" id="txt_avg_promo">0</span>
                                        </div>
                                        <div class="vr w-3px"></div>
                                        <div class="d-flex h-40px flex-column align-middle w-60 align-items-center">
                                            <span class="fw-semibold text-dark-blue fw-bolder d-block text-wrap  fs-optima-12">Average Promo</span>
                                            <span class="fw-semibold text-dark-blue fw-bolder d-block text-wrap  fs-optima-12">Approved Days</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-lg-0 mb-3">
                            <div class="card shadow-sm card-flush card-px-0 card_promo_submitted_claim">
                                <div class="card-header">
                                    <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  fs-optima-12  my-3">Submitted Claim</span>
                                </div>
                                <div class="separator border-5 line-card"></div>
                                <div class="card-body px-0 text-center">
                                    <div class="position-relative h-125px w-100" id="chart_promo_claim_received"></div>
                                </div>
                                <div class="card-footer pb-0 pt-0">
                                    <div class="separator border-3 line-card"></div>
                                    <div class="d-flex">
                                        <div class="d-flex align-middle w-20 align-items-center">
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  fs-optima-12 mb-2" id="txt_claim_received">0</span>
                                        </div>
                                        <div class="vr w-3px"></div>
                                        <div class="d-flex h-40px align-middle w-60 align-items-center">
                                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  fs-optima-12">Claim Received</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-38 col-md-12 col-sm-12 col-12">
                    <div class="card shadow-sm card-flush card-px-0 h-100 card_dashboard_notif">
                        <div class="card-header">
                            <span class="fw-semibold mx-auto text-dark-blue fw-bolder d-block text-wrap  fs-optima-12  my-3">Task To Do</span>
                        </div>
                        <div class="separator border-5 line-card"></div>
                        <div class="card-body px-2 scroll-y h-lg-100px">
                            <a href="/promo/planning/to-be-created" class="d-flex flex-stack p-3 row_notif">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon text-info">
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                        <rect id="bound" x="0" y="0" width="24" height="24"/>
                                        <path d="M3,16 L5,16 C5.55228475,16 6,15.5522847 6,15 C6,14.4477153 5.55228475,14 5,14 L3,14 L3,12 L5,12 C5.55228475,12 6,11.5522847 6,11 C6,10.4477153 5.55228475,10 5,10 L3,10 L3,8 L5,8 C5.55228475,8 6,7.55228475 6,7 C6,6.44771525 5.55228475,6 5,6 L3,6 L3,4 C3,3.44771525 3.44771525,3 4,3 L10,3 C10.5522847,3 11,3.44771525 11,4 L11,19 C11,19.5522847 10.5522847,20 10,20 L4,20 C3.44771525,20 3,19.5522847 3,19 L3,16 Z" id="Combined-Shape" fill="#5d78ff" opacity="0.3"/>
                                        <path d="M16,3 L19,3 C20.1045695,3 21,3.8954305 21,5 L21,15.2485298 C21,15.7329761 20.8241635,16.200956 20.5051534,16.565539 L17.8762883,19.5699562 C17.6944473,19.7777745 17.378566,19.7988332 17.1707477,19.6169922 C17.1540423,19.602375 17.1383289,19.5866616 17.1237117,19.5699562 L14.4948466,16.565539 C14.1758365,16.200956 14,15.7329761 14,15.2485298 L14,5 C14,3.8954305 14.8954305,3 16,3 Z" id="Rectangle-102-Copy" fill="#5d78ff"/>
                                    </g>
                                </svg>
                                <div class="d-flex align-items-center flex-row-fluid flex-wrap">
                                    <div class="flex-grow-1 me-2 ms-5">
                                        <span class="text-gray-800 text-hover-primary fw-bolder fs-6 text_header_notif">You have Promo to be created</span>
                                        <span class="text-gray-600 fw-bolder d-block fs-6" id="txt_notif_promo_plan">0 Promo Plan</span>
                                    </div>
                                    <span class="btn btn-sm btn-icon btn-active-color-primary w-30px h-30px">
                                        <i class="fa fa-chevron-right fs-7">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                    </span>
                                </div>
                            </a>
                            @if(Session::get('role') !== '104')
                            <a href="/promo/approval"  class="d-flex flex-stack p-3 row_notif">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon kt-svg-icon--success" id="icon_promo_approval">
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd" id="icon_promo_approval">
                                        <rect id="bound" x="0" y="0" width="24" height="24"></rect>
                                        <path d="M12.9835977,18 C12.7263047,14.0909841 9.47412135,11 5.5,11 C4.98630124,11 4.48466491,11.0516454 4,11.1500272 L4,7 C4,5.8954305 4.8954305,5 6,5 L20,5 C21.1045695,5 22,5.8954305 22,7 L22,16 C22,17.1045695 21.1045695,18 20,18 L12.9835977,18 Z M19.1444251,6.83964668 L13,10.1481833 L6.85557487,6.83964668 C6.4908718,6.6432681 6.03602525,6.77972206 5.83964668,7.14442513 C5.6432681,7.5091282 5.77972206,7.96397475 6.14442513,8.16035332 L12.6444251,11.6603533 C12.8664074,11.7798822 13.1335926,11.7798822 13.3555749,11.6603533 L19.8555749,8.16035332 C20.2202779,7.96397475 20.3567319,7.5091282 20.1603533,7.14442513 C19.9639747,6.77972206 19.5091282,6.6432681 19.1444251,6.83964668 Z" id="Combined-Shape" fill="#0abb87"></path>
                                        <path d="M8.4472136,18.1055728 C8.94119209,18.3525621 9.14141644,18.9532351 8.89442719,19.4472136 C8.64743794,19.9411921 8.0467649,20.1414164 7.5527864,19.8944272 L5,18.618034 L5,14.5 C5,13.9477153 5.44771525,13.5 6,13.5 C6.55228475,13.5 7,13.9477153 7,14.5 L7,17.381966 L8.4472136,18.1055728 Z" id="Path-85" fill="#0abb87" fill-rule="nonzero" opacity="0.3"></path>
                                    </g>
                                </svg>
                                <div class="d-flex align-items-center flex-row-fluid flex-wrap">
                                    <div class="flex-grow-1 me-2 ms-5">
                                        <span class="text-gray-800 text-hover-primary fw-bolder fs-6 text_header_notif">You have pending promo approval</span>
                                        <span class="text-gray-600 fw-bolder d-block fs-6" id="txt_notif_pending_promo_approval">0 Promo</span>
                                    </div>
                                    <span class="btn btn-sm btn-icon btn-active-color-primary w-30px h-30px">
                                        <i class="fa fa-chevron-right fs-7">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                    </span>
                                </div>
                            </a>
                            <a href="/promo/approval-recon"  class="d-flex flex-stack p-3 row_notif">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon kt-svg-icon--success" id="icon_promo_approval">
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd" id="icon_promo_approval">
                                        <rect id="bound" x="0" y="0" width="24" height="24"></rect>
                                        <path d="M12.9835977,18 C12.7263047,14.0909841 9.47412135,11 5.5,11 C4.98630124,11 4.48466491,11.0516454 4,11.1500272 L4,7 C4,5.8954305 4.8954305,5 6,5 L20,5 C21.1045695,5 22,5.8954305 22,7 L22,16 C22,17.1045695 21.1045695,18 20,18 L12.9835977,18 Z M19.1444251,6.83964668 L13,10.1481833 L6.85557487,6.83964668 C6.4908718,6.6432681 6.03602525,6.77972206 5.83964668,7.14442513 C5.6432681,7.5091282 5.77972206,7.96397475 6.14442513,8.16035332 L12.6444251,11.6603533 C12.8664074,11.7798822 13.1335926,11.7798822 13.3555749,11.6603533 L19.8555749,8.16035332 C20.2202779,7.96397475 20.3567319,7.5091282 20.1603533,7.14442513 C19.9639747,6.77972206 19.5091282,6.6432681 19.1444251,6.83964668 Z" id="Combined-Shape" fill="#5d78ff"></path>
                                        <path d="M8.4472136,18.1055728 C8.94119209,18.3525621 9.14141644,18.9532351 8.89442719,19.4472136 C8.64743794,19.9411921 8.0467649,20.1414164 7.5527864,19.8944272 L5,18.618034 L5,14.5 C5,13.9477153 5.44771525,13.5 6,13.5 C6.55228475,13.5 7,13.9477153 7,14.5 L7,17.381966 L8.4472136,18.1055728 Z" id="Path-85" fill="#5d78ff" fill-rule="nonzero" opacity="0.3"></path>
                                    </g>
                                </svg>
                                <div class="d-flex align-items-center flex-row-fluid flex-wrap">
                                    <div class="flex-grow-1 me-2 ms-5">
                                        <span class="text-gray-800 text-hover-primary fw-bolder fs-6 text_header_notif">You have pending promo approval reconcile</span>
                                        <span class="text-gray-600 fw-bolder d-block fs-6" id="txt_notif_pending_promo_recon_approval">0 Promo</span>
                                    </div>
                                    <span class="btn btn-sm btn-icon btn-active-color-primary w-30px h-30px">
                                        <i class="fa fa-chevron-right fs-7">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                    </span>
                                </div>
                            </a>
                            @endif
                            <a href="/promo/send-back" class="d-flex flex-stack p-3 row_notif">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon">
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                        <rect id="bound" x="0" y="0" width="24" height="24"/>
                                        <path d="M8.29606274,4.13760526 L1.15599693,10.6152626 C0.849219196,10.8935795 0.826147139,11.3678924 1.10446404,11.6746702 C1.11907213,11.6907721 1.13437346,11.7062312 1.15032466,11.7210037 L8.29039047,18.333467 C8.59429669,18.6149166 9.06882135,18.596712 9.35027096,18.2928057 C9.47866909,18.1541628 9.55000007,17.9721616 9.55000007,17.7831961 L9.55000007,4.69307548 C9.55000007,4.27886191 9.21421363,3.94307548 8.80000007,3.94307548 C8.61368984,3.94307548 8.43404911,4.01242035 8.29606274,4.13760526 Z" id="Shape-Copy" fill="#5d78ff" fill-rule="nonzero" opacity="0.3"/>
                                        <path d="M23.2951173,17.7910156 C23.2951173,16.9707031 23.4708985,13.7333984 20.9171876,11.1650391 C19.1984376,9.43652344 16.6261719,9.13671875 13.5500001,9 L13.5500001,4.69307548 C13.5500001,4.27886191 13.2142136,3.94307548 12.8000001,3.94307548 C12.6136898,3.94307548 12.4340491,4.01242035 12.2960627,4.13760526 L5.15599693,10.6152626 C4.8492192,10.8935795 4.82614714,11.3678924 5.10446404,11.6746702 C5.11907213,11.6907721 5.13437346,11.7062312 5.15032466,11.7210037 L12.2903905,18.333467 C12.5942967,18.6149166 13.0688214,18.596712 13.350271,18.2928057 C13.4786691,18.1541628 13.5500001,17.9721616 13.5500001,17.7831961 L13.5500001,13.5 C15.5031251,13.5537109 16.8943705,13.6779456 18.1583985,14.0800781 C19.9784273,14.6590944 21.3849749,16.3018455 22.3780412,19.0083314 L22.3780249,19.0083374 C22.4863904,19.3036749 22.7675498,19.5 23.0821406,19.5 L23.3000001,19.5 C23.3000001,19.0068359 23.2951173,18.2255859 23.2951173,17.7910156 Z" id="Shape" fill="#5d78ff" fill-rule="nonzero"/>
                                    </g>
                                </svg>
                                <div class="d-flex align-items-center flex-row-fluid flex-wrap">
                                    <div class="flex-grow-1 me-2 ms-5">
                                        <span class="text-gray-800 text-hover-primary fw-bolder fs-6 text_header_notif">You have promo send back</span>
                                        <span class="text-gray-600 fw-bolder d-block fs-6" id="txt_notif_promo_send_back">0 Promo</span>
                                    </div>
                                    <span class="btn btn-sm btn-icon btn-active-color-primary w-30px h-30px">
                                        <i class="fa fa-chevron-right fs-7">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                    </span>
                                </div>
                            </a>
                            <a href="/promo/recon-send-back" class="d-flex flex-stack p-3 row_notif">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon">
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                        <rect id="bound" x="0" y="0" width="24" height="24"/>
                                        <path d="M8.29606274,4.13760526 L1.15599693,10.6152626 C0.849219196,10.8935795 0.826147139,11.3678924 1.10446404,11.6746702 C1.11907213,11.6907721 1.13437346,11.7062312 1.15032466,11.7210037 L8.29039047,18.333467 C8.59429669,18.6149166 9.06882135,18.596712 9.35027096,18.2928057 C9.47866909,18.1541628 9.55000007,17.9721616 9.55000007,17.7831961 L9.55000007,4.69307548 C9.55000007,4.27886191 9.21421363,3.94307548 8.80000007,3.94307548 C8.61368984,3.94307548 8.43404911,4.01242035 8.29606274,4.13760526 Z" id="Shape-Copy" fill="#5d78ff" fill-rule="nonzero" opacity="0.3"/>
                                        <path d="M23.2951173,17.7910156 C23.2951173,16.9707031 23.4708985,13.7333984 20.9171876,11.1650391 C19.1984376,9.43652344 16.6261719,9.13671875 13.5500001,9 L13.5500001,4.69307548 C13.5500001,4.27886191 13.2142136,3.94307548 12.8000001,3.94307548 C12.6136898,3.94307548 12.4340491,4.01242035 12.2960627,4.13760526 L5.15599693,10.6152626 C4.8492192,10.8935795 4.82614714,11.3678924 5.10446404,11.6746702 C5.11907213,11.6907721 5.13437346,11.7062312 5.15032466,11.7210037 L12.2903905,18.333467 C12.5942967,18.6149166 13.0688214,18.596712 13.350271,18.2928057 C13.4786691,18.1541628 13.5500001,17.9721616 13.5500001,17.7831961 L13.5500001,13.5 C15.5031251,13.5537109 16.8943705,13.6779456 18.1583985,14.0800781 C19.9784273,14.6590944 21.3849749,16.3018455 22.3780412,19.0083314 L22.3780249,19.0083374 C22.4863904,19.3036749 22.7675498,19.5 23.0821406,19.5 L23.3000001,19.5 C23.3000001,19.0068359 23.2951173,18.2255859 23.2951173,17.7910156 Z" id="Shape" fill="#5d78ff" fill-rule="nonzero"/>
                                    </g>
                                </svg>
                                <div class="d-flex align-items-center flex-row-fluid flex-wrap">
                                    <div class="flex-grow-1 me-2 ms-5">
                                        <span class="text-gray-800 text-hover-primary fw-bolder fs-6 text_header_notif">You have promo send back reconcile</span>
                                        <span class="text-gray-600 fw-bolder d-block fs-6" id="txt_notif_promo_send_back_recon">0 Promo</span>
                                    </div>
                                    <span class="btn btn-sm btn-icon btn-active-color-primary w-30px h-30px">
                                        <i class="fa fa-chevron-right fs-7">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                    </span>
                                </div>
                            </a>
                            <a href="/dn/assignment" class="d-flex flex-stack p-3 row_notif">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon kt-svg-icon--brand">
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                        <rect id="Rectangle-10" x="0" y="0" width="24" height="24" />
                                        <path d="M16.3740377,19.9389434 L22.2226499,11.1660251 C22.4524142,10.8213786 22.3592838,10.3557266 22.0146373,10.1259623 C21.8914367,10.0438285 21.7466809,10 21.5986122,10 L17,10 L17,4.47708173 C17,4.06286817 16.6642136,3.72708173 16.25,3.72708173 C15.9992351,3.72708173 15.7650616,3.85240758 15.6259623,4.06105658 L9.7773501,12.8339749 C9.54758575,13.1786214 9.64071616,13.6442734 9.98536267,13.8740377 C10.1085633,13.9561715 10.2533191,14 10.4013878,14 L15,14 L15,19.5229183 C15,19.9371318 15.3357864,20.2729183 15.75,20.2729183 C16.0007649,20.2729183 16.2349384,20.1475924 16.3740377,19.9389434 Z" id="Path-3" fill="#5d78ff" />
                                        <path d="M4.5,5 L9.5,5 C10.3284271,5 11,5.67157288 11,6.5 C11,7.32842712 10.3284271,8 9.5,8 L4.5,8 C3.67157288,8 3,7.32842712 3,6.5 C3,5.67157288 3.67157288,5 4.5,5 Z M4.5,17 L9.5,17 C10.3284271,17 11,17.6715729 11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L4.5,20 C3.67157288,20 3,19.3284271 3,18.5 C3,17.6715729 3.67157288,17 4.5,17 Z M2.5,11 L6.5,11 C7.32842712,11 8,11.6715729 8,12.5 C8,13.3284271 7.32842712,14 6.5,14 L2.5,14 C1.67157288,14 1,13.3284271 1,12.5 C1,11.6715729 1.67157288,11 2.5,11 Z" id="Combined-Shape" fill="#5d78ff" opacity="0.3" />
                                    </g>
                                </svg>
                                <div class="d-flex align-items-center flex-row-fluid flex-wrap">
                                    <div class="flex-grow-1 me-2 ms-5">
                                        <span class="text-gray-800 text-hover-primary fw-bolder fs-6 text_header_notif">You have DN Manual to be settled</span>
                                        <span class="text-gray-600 fw-bolder d-block fs-6" id="txt_notif_dn_manual">0 DN Manual</span>
                                    </div>
                                    <span class="btn btn-sm btn-icon btn-active-color-primary w-30px h-30px">
                                        <i class="fa fa-chevron-right fs-7">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                    </span>
                                </div>
                            </a>
                            <a href="/dn/listing-over-budget/to-be-settled" class="d-flex flex-stack p-3 row_notif">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon">
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                        <rect id="bound" x="0" y="0" width="24" height="24"/>
                                        <path d="M11.1669899,4.49941818 L2.82535718,19.5143571 C2.557144,19.9971408 2.7310878,20.6059441 3.21387153,20.8741573 C3.36242953,20.9566895 3.52957021,21 3.69951446,21 L21.2169432,21 C21.7692279,21 22.2169432,20.5522847 22.2169432,20 C22.2169432,19.8159952 22.1661743,19.6355579 22.070225,19.47855 L12.894429,4.4636111 C12.6064401,3.99235656 11.9909517,3.84379039 11.5196972,4.13177928 C11.3723594,4.22181902 11.2508468,4.34847583 11.1669899,4.49941818 Z" id="Path-117" fill="#5d78ff" opacity="0.3"/>
                                        <rect id="Rectangle-9" fill="#5d78ff" x="11" y="9" width="2" height="7" rx="1"/>
                                        <rect id="Rectangle-9-Copy" fill="#5d78ff" x="11" y="17" width="2" height="2" rx="1"/>
                                    </g>
                                </svg>
                                <div class="d-flex align-items-center flex-row-fluid flex-wrap">
                                    <div class="flex-grow-1 me-2 ms-5">
                                        <span class="text-gray-800 text-hover-primary fw-bolder fs-6 text_header_notif">You have DN Over budget to be settled</span>
                                        <span class="text-gray-600 fw-bolder d-block fs-6" id="txt_notif_dn_over_budget">0 DN Over Budget</span>
                                    </div>
                                    <span class="btn btn-sm btn-icon btn-active-color-primary w-30px h-30px">
                                        <i class="fa fa-chevron-right fs-7">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                    </span>
                                </div>
                            </a>
                            <a href="/dn/validate-by-sales" class="d-flex flex-stack p-3 row_notif">
                                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1" class="kt-svg-icon">
                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                                        <rect id="bound" x="0" y="0" width="24" height="24"/>
                                        <path d="M3.5,21 L20.5,21 C21.3284271,21 22,20.3284271 22,19.5 L22,8.5 C22,7.67157288 21.3284271,7 20.5,7 L10,7 L7.43933983,4.43933983 C7.15803526,4.15803526 6.77650439,4 6.37867966,4 L3.5,4 C2.67157288,4 2,4.67157288 2,5.5 L2,19.5 C2,20.3284271 2.67157288,21 3.5,21 Z" id="Combined-Shape" fill="#5d78ff" opacity="0.3"/>
                                        <path d="M10.875,16.75 C10.6354167,16.75 10.3958333,16.6541667 10.2041667,16.4625 L8.2875,14.5458333 C7.90416667,14.1625 7.90416667,13.5875 8.2875,13.2041667 C8.67083333,12.8208333 9.29375,12.8208333 9.62916667,13.2041667 L10.875,14.45 L14.0375,11.2875 C14.4208333,10.9041667 14.9958333,10.9041667 15.3791667,11.2875 C15.7625,11.6708333 15.7625,12.2458333 15.3791667,12.6291667 L11.5458333,16.4625 C11.3541667,16.6541667 11.1145833,16.75 10.875,16.75 Z" id="check-path" fill="#5d78ff"/>
                                    </g>
                                </svg>
                                <div class="d-flex align-items-center flex-row-fluid flex-wrap">
                                    <div class="flex-grow-1 me-2 ms-5">
                                        <span class="text-gray-800 text-hover-primary fw-bolder fs-6 text_header_notif">You have DN to be validated</span>
                                        <span class="text-gray-600 fw-bolder d-block fs-6" id="txt_notif_dn_validate_by_sales">0 DN</span>
                                    </div>
                                    <span class="btn btn-sm btn-icon btn-active-color-primary w-30px h-30px">
                                        <i class="fa fa-chevron-right fs-7">
                                            <span class="path1"></span>
                                            <span class="path2"></span>
                                        </i>
                                    </span>
                                </div>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="px-lg-0 mb-13">
        <div class="chart-dashboard" id="chart-dashboard">
            <div class="row">
                <div class="col-lg-6 col-12">
                    <div class="card shadow-sm card-flush h-100 card_dashboard_promo_creation_trend">
                        <div class="card-header py-3">
                            <a href="/promo/creation" class="fw-semibold text-dark-blue fw-bolder d-block text-wrap my-2 fs-optima-12">Promo Creation Trend</a>
                        </div>
                        <div class="separator border-5 line-card"></div>
                        <div class="card-body px-0 text-center my-2">
                            <div class="row">
                                <div id="legenddivtrend"></div>
                                <div id="chartdiv_promo_creation_trend"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6 col-12">
                    <div class="card shadow-sm card-flush h-100 card_dashboard_outstanding_dn">
                        <div class="card-header py-3">
                            <a href="/fin-rpt/dn-detail-reporting" class="fw-semibold text-dark-blue fw-bolder d-block text-wrap my-2 fs-optima-12">Outstanding DN</a>
                            <button class="btn btn-active-icon-optima btn-text-primary p-0" id="btn_show_filter_outstanding_dn">
                                <i class="fa fa-cog fs-2rem text-gear-color"></i>
                            </button>
                        </div>
                        <div class="separator border-5 line-card"></div>
                        <div class="card-body px-0 text-center my-2">
                            <div class="row d-none" id="filter_outstanding_dn">
                                <div class="col-lg-6 col-12 mb-3">
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_distributor" id="filter_distributor" data-placeholder="Select a Distributor"  data-allow-clear="true" autocomplete="off">
                                        <option></option>
                                    </select>
                                </div>
                                <div class="col-lg-6 col-12 mb-3">
                                    <select class="form-select form-select-sm" data-control="select2" name="filter_ispromo" id="filter_ispromo" autocomplete="off">
                                        <option value="0">All</option>
                                        <option value="1">Promo</option>
                                        <option value="2">Non Promo</option>
                                    </select>
                                </div>
                                <div class="separator border-1"></div>
                            </div>
                            <div class="row">
                                <div id="legenddivdn"></div>
                                <div id="chartdiv_outstanding"></div>
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
    <script src="{{ asset('assets/plugins/custom/select2-multi-checkboxes/select2.multi-checkboxes.js') }}"></script>
    <script src="{{ asset('/assets/plugins/custom/amcharts4/core.js') }}" type="text/javascript"></script>
    <script src="{{ asset('/assets/plugins/custom/amcharts4/charts.js') }}" type="text/javascript"></script>
    <script src="{{ asset('/assets/plugins/custom/amcharts4/themes/animated.js') }}" type="text/javascript"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/dashboard/main/js/index.js?v=' . microtime()) }}"></script>

    <script>
        let expiredChangeStr = "{{ @Session::get('password_change') }}";
        let expiredChange = new Date(expiredChangeStr).getTime();
        let now = new Date().getTime();
        if (Math.floor((now-expiredChange)/(24*3600*1000)) >= 50) {
            let expiredInterval = 60 - Math.floor((now-expiredChange)/(24*3600*1000));
            swal.fire({
                icon: "warning",
                title: 'Warning',
                text: "Your password expired in " + expiredInterval + " days, please change your password",
                allowOutsideClick: false,
                allowEscapeKey: false,
            });
        }
    </script>
@endsection

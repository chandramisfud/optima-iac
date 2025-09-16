@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/dashboard/approver/css/index.css') }}"  rel="stylesheet" type="text/css" />
@endsection

@section('toolbar')

@endsection

@section('content')
    <div class="px-lg-0 sticky-top-header-dashboard">
        <div class="header-dashboard mb-3" id="header-dashboard">
            <div class="row">
                <div class="card shadow shadow-sm card-flush card-px-0 rounded-0 card_dashboard_header">
                    <div class="card-body card-body-top-data p-0">
                        <div class="row">
                            <div class="col-lg-auto col-md-12 col-sm-12 mb-lg-0 mb-2">
                                <div class="row">
                                    <label class="col-lg-auto pe-lg-0 mt-1161px-2">Monitoring Period</label>
                                    <div class="col-lg-auto col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm cursor-pointer" id="filter_period_start" value="{{ @date('Y-m-d',strtotime(date('Y-01-01'))) }}" autocomplete="off">
                                            <span class="input-group-text">to</span>
                                            <input type="text" class="form-control form-control-sm cursor-pointer" id="filter_period_end" value="{{ @date('Y-m-d',strtotime(date('Y-12-31'))) }}" autocomplete="off">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-auto col-md-12 col-sm-12 mb-lg-0 mb-2 pe-lg-0">
                                <div class="d-flex justify-content-between">
                                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100 me-lg-0 me-3" id="btn_view">
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
    </div>

    <div class="body-dashboard">
        <div class="row mb-5">
            <div class="col-lg-3 col-12 mb-5 mb-lg-0">
                <div class="card shadow shadow-sm h-175px card_approval_request" id="approval_request">
                    <div class="card-body">
                        <div class="row">
                            <span class="fw-semibold text-gray-800 d-block fs-5">Approval Request</span>
                            <span class="fw-bold text-gray-700 fs-6">Within Monitoring Period</span>
                        </div>

                        <div class="row val_approval_request">
                            <span class="card_value text-5578eb" id="txt_approval_request">0</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-12 mb-5 mb-lg-0">
                <div class="card shadow shadow-sm h-175px card_approval_request_response_time" id="approval_request_response_time">
                    <div class="card-body">
                        <div class="row">
                            <span class="fw-semibold text-gray-800 d-block fs-5">Approval Request Response Time</span>
                            <span class="fw-bold text-gray-700 fs-6">&nbsp;</span>
                        </div>

                        <div class="row mt-1726-4 val_approval_request_response_time">
                            <span class="card_value text-febc31" id="txt_approval_request_response_time">0.00 <span class="fs-2 text-gray-600 fw-normal">Days</span> </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-12 mb-lg-0">
                <div class="card shadow shadow-sm h-100 card_promo_approval" id="promo_approval">
                    <div class="card-body">
                        <div class="row">
                            <span class="fw-semibold text-gray-800 d-block fs-5">% Promo Approval</span>
                            <span class="fw-bold text-gray-700 fs-6">Timeliness SLA max.5 days for aprrover request</span>
                        </div>
                        <div class="row">
                            <span class="card_value text-58dd91" id="txt_promo_approval">0.00<span class="text-gray-600 fw-normal ms-2">%</span></span>
                        </div>
                        <div class="row">
                            <div class="col-md-12 col-12">
                                <div class="d-flex flex-column w-100 me-2">
                                    <div class="progress bg-ebedf2 h-10px w-100">
                                        <div class="progress-bar bg-58dd91" id="progress_bar" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                            <div class="d-flex flex-center">
                                                <span class="text-gray-900 fs-7 fw-bold text-center" id="text_progress">0%</span>
                                            </div>
                                        </div>
                                    </div>
                                    <span class="fs-4 text-end" id="txt_sub_score">Sub Score: 0.00/5</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row mb-lg-1 mb-3">
        <div class="col-12">
            <div class="card card-flush shadow shadow-sm h-100 card_approver_league" id="approver_league">
                <div class="card-header px-3">
                    <div class="d-flex align-items-center">
                        <img class="w-40px h-40px" src="{{ asset('/assets/media/bg/trafo.png') }}">

                        <div class="ms-3">
                            <span class="text-start fs-2">Approver League Top & Bottom</span>
                        </div>
                    </div>
                </div>
                <div class="separator border-1"></div>
                <div class="card-body">
                    <div class="row mt-2">
                        <div class="col-lg-6 col-12 mb-lg-0 mb-5">
                            <div class="row">
                                <div class="col-lg-10 col-12">
                                    <div class="row px-3">
                                        <div class="col-lg-6 col-6">
                                            <span class="fw-bolder">Top 5</span>
                                        </div>
                                        <div class="col-lg-2 col-2 text-end">
                                            <span class="fw-bolder">Score</span>
                                        </div>
                                        <div class="col-lg-2 col-2 text-end">
                                            <span class="fw-bolder">No.oF Optima</span>
                                        </div>
                                        <div class="col-lg-2 col-2 text-end">
                                            <span class="fw-bolder">Avg Days</span>
                                        </div>
                                    </div>
                                    <div class="separator border-1 border-04ff4b"></div>

                                    <div id="list_top5">
                                        <div class="row px-3 py-3">
                                            <div class="col-lg-6 col-6">
                                                <span class="fw-normal">-</span>
                                            </div>
                                            <div class="col-lg-2 col-2 text-end">
                                                <span class="fw-normal">0.00%</span>
                                            </div>
                                            <div class="col-lg-2 col-2 text-end">
                                                <span class="fw-normal">0.00</span>
                                            </div>
                                            <div class="col-lg-2 col-2 text-end">
                                                <span class="fw-normal">0.00</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-6 col-12 mb-lg-0 mb-5">
                            <div class="row">
                                <div class="offset-lg-2 col-lg-10 col-12">
                                    <div class="row px-3">
                                        <div class="col-lg-6 col-6">
                                            <span class="fw-bolder">Bottom 5</span>
                                        </div>
                                        <div class="col-lg-2 col-6 text-end">
                                            <span class="fw-bolder">Score</span>
                                        </div>
                                        <div class="col-lg-2 col-6 text-end">
                                            <span class="fw-bolder">No.oF Optima</span>
                                        </div>
                                        <div class="col-lg-2 col-6 text-end">
                                            <span class="fw-bolder">Avg Days</span>
                                        </div>
                                    </div>
                                    <div class="separator border-1 border-ff3434"></div>

                                    <div id="list_bottom5">
                                        <div class="row px-3 py-3">
                                            <div class="col-lg-6 col-6">
                                                <span class="fw-normal">-</span>
                                            </div>
                                            <div class="col-lg-2 col-6 text-end">
                                                <span class="fw-normal">0.00%</span>
                                            </div>
                                            <div class="col-lg-2 col-6 text-end">
                                                <span class="fw-normal">0.00</span>
                                            </div>
                                            <div class="col-lg-2 col-6 text-end">
                                                <span class="fw-normal">0.00</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/dashboard/approver/js/index.js?v=' . microtime()) }}"></script>

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


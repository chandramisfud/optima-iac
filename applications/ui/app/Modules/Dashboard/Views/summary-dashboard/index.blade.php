@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/dashboard/summary/css/index.css?v=' . microtime()) }}"  rel="stylesheet" type="text/css" />
@endsection

@section('toolbar')

@endsection

@section('content')
    <div class="px-lg-0 sticky-top-header-dashboard">
        <div class="header-dashboard mb-3" id="header-dashboard">
            <div class="row">
                <div class="col-12">
                    <div class="card shadow shadow-sm card-flush card-px-0 rounded-0 card_dashboard_header">
                        <div class="card-body card-body-top-data p-0">
                            <div class="d-flex flex-lg-row flex-column align-items-lg-center">
                                <label class="me-5 mb-lg-0 mb-2">Monitoring Period</label>
                                <div class="w-lg-200px w-100 mb-lg-0 mb-2 me-lg-10 me-0">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm cursor-pointer" id="filter_period_start" value="{{ @date('Y-m-d',strtotime(date('Y-01-01'))) }}" autocomplete="off">
                                        <span class="input-group-text">to</span>
                                        <input type="text" class="form-control form-control-sm cursor-pointer" id="filter_period_end" value="{{ @date('Y-m-d') }}" autocomplete="off">
                                    </div>
                                </div>
                                <div class="form-check mb-lg-0 mb-2 me-lg-5 me-0">
                                    <input class="form-check-input" type="checkbox" value="" id="checkbox_promo_plan" autocomplete="off"/>
                                    <label class="form-check-label" for="checkbox_promo_plan">
                                        Promo Plan H-60 Start Period Monitoring
                                    </label>
                                </div>
                                <div class="w-lg-100px w-100 mb-lg-0 mb-2 me-lg-5 me-0">
                                    <input type="text" class="form-control form-control-sm cursor-pointer" id="filter_date_promo_plan" value="{{ @date('Y-m-d',strtotime(date('Y-01-01'))) }}" autocomplete="off">
                                </div>
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100 me-lg-auto mb-lg-0 mb-2" id="btn_view">
                                    <span class="indicator-label">
                                        <span class="fa fa-search"></span> View
                                    </span>
                                        <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                                <div class="d-flex flex-lg-row flex-column-reverse">
                                    <div class="d-none" id="export_list">
                                        <button type="button" class="btn btn-sm btn-secondary w-lg-auto w-100 ms-lg-0 mb-lg-0 mb-2" id="btn_download_xls_dtl">
                                            XLS Detail
                                        </button>
                                        <button type="button" class="btn btn-sm btn-secondary w-lg-auto w-100 ms-lg-0 mb-lg-0 mb-2" id="btn_download_xls">
                                            XLS
                                        </button>
                                        <button type="button" class="btn btn-sm btn-secondary w-lg-auto w-100 ms-lg-0 mb-lg-0 mb-2" id="btn_download_pdf">
                                            PDF
                                        </button>
                                    </div>
                                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100 ms-lg-2 mb-lg-0 mb-2" id="btn_download">
                                        <span class="fa fa-download"></span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="print">
        <div class="header-summary mb-3" id="header-summary">
            <div class="row">
                <div class="col-12">
                    <div class="card shadow shadow-sm card-flush">
                        <div class="card-body">
                            <div class="row">
                                <div class="d-flex flex-lg-row flex-column align-items-lg-center">
                                    <div class="fs-2">SUMMARY</div>
                                    <div class="fs-2 mx-lg-5 d-none d-lg-block">|</div>
                                    <div class="fs-5 ms-lg-10 me-lg-5">KPI Legends :</div>
                                    <div class="d-inline-flex ms-lg-5 mb-lg-0 mb-2">
                                        <div class="rounded-circle w-20px h-20px bg-ff5a5a"></div>
                                        <div class="fs-5 ms-2">Below 60%</div>
                                    </div>
                                    <div class="d-inline-flex ms-lg-5 mb-lg-0 mb-2">
                                        <div class="rounded-circle w-20px h-20px bg-ffb822"></div>
                                        <div class="fs-5 ms-2">60% - 80% </div>
                                    </div>
                                    <div class="d-inline-flex ms-lg-5 mb-lg-0 mb-2">
                                        <div class="rounded-circle w-20px h-20px bg-58dd91"></div>
                                        <div class="fs-5 ms-2">80% and Above</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="body-dashboard mb-3" id="body-dashboard">
            <div class="row">
                <div class="col-lg-2 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row1_promo_plan">
                        <div class="card-body">
                            <div class="row">
                                <div class="d-flex align-items-center">
                                    <div class="symbol symbol-35px symbol-circle me-3">
                                        <span class="symbol-label bg-D4DDFAFF">
                                            <i class="fa fa-hashtag fs-1 text-5578eb"></i>
                                        </span>
                                    </div>

                                    <div class="m-0">
                                        <span class="fw-semibold text-gray-800 d-block fs-5">Promo Plan</span>
                                        <span class="fw-bold text-gray-700 fs-9">Within Monitoring Period</span>
                                    </div>
                                </div>
                            </div>
                            <div class="row val_promo_plan">
                                <span class="card_value text-5578eb" id="txtPromoPlanEvaluated">0</span>
                                <span class="">Document</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row1_overall_score">
                        <div class="card-body">
                            <div class="row">
                                <div class="d-flex align-items-center">
                                    <div class="symbol symbol-35px symbol-circle me-3">
                                    <span class="symbol-label bg-58DD913F">
                                        <i class="fa fa-chart-pie fs-1 text-58dd91"></i>
                                    </span>
                                    </div>

                                    <div class="m-0">
                                        <span class="fw-semibold text-gray-800 d-block fs-5">Overall Score</span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <span class="card_value text-5578eb" id="txtOverallScorePct">0.00<span class="text-gray-600 fw-normal ms-2">%</span></span>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-12">
                                    <div class="d-flex flex-column w-100 me-2">
                                        <div class="progress bg-ebedf2 h-15px w-100">
                                            <span class="text-gray-900 fs-8 mt-0 ms-1 fw-bold text-left position-absolute" id="txtProgressOverallScorePct">0%</span>
                                            <div class="progress-bar bg-58dd91" role="progressbar" id="progress_overall_score" style="width:0%" aria-valuenow="0.0" aria-valuemin="0" aria-valuemax="100">
                                            </div>
                                        </div>
                                        <span class="text-end">100%</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row1_sub_score">
                        <div class="card-body">
                            <div class="row">
                                <div class="d-flex align-items-center">
                                    <div class="symbol symbol-35px symbol-circle me-3">
                                        <span class="symbol-label bg-FEBC313F">
                                            <i class="fa fa-clipboard-list fs-1 text-febc31"></i>
                                        </span>
                                    </div>

                                    <div class="m-0">
                                        <span class="fw-semibold text-gray-800 d-block fs-5">Sub-Score</span>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-3">
                                <div class="col-lg-6 col-12">
                                    <div class="row">
                                        <span>Promo plan</span>
                                    </div>
                                    <div class="row">
                                        <span class="text-febc31" id="txtSubScorePromoPlan">0.00 <span class="text-gray-900">/ 2</span></span>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-12">
                                    <div class="row">
                                        <span>Accuracy SKP</span>
                                    </div>
                                    <div class="row">
                                        <span class="text-febc31" id="txtSubScoreAccuracySKP">0.00 <span class="text-gray-900">/ 4</span></span>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-3">
                                <div class="col-lg-6 col-12">
                                    <div class="row">
                                        <span>Accuracy promo</span>
                                    </div>
                                    <div class="row">
                                        <span class="text-febc31" id="txtSubScoreAccuracyPromo">0.00 <span class="text-gray-900">/ 5</span></span>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-12">
                                    <div class="row">
                                        <span>Recon monitoring</span>
                                    </div>
                                    <div class="row">
                                        <span class="text-febc31" id="txtSubScoreReconMonitoring">0.00 <span class="text-gray-900">/ 7</span></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row1_approval_request" id="approval_request">
                        <div class="card-body">
                            <div class="row">
                                <span class="fw-semibold text-gray-800 d-block fs-5">Approval Request</span>
                                <span class="fw-bold text-gray-700 fs-9">Within Monitoring Period</span>
                            </div>

                            <div class="row val_approval_request">
                                <span class="card_value text-5578eb" id="txt_approval_request">0</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row1_approval_request_response_time" id="approval_request_response_time">
                        <div class="card-body">
                            <div class="row">
                                <span class="fw-semibold text-gray-800 d-block fs-5">Approval Request Response Time</span>
                                <span class="fw-bold text-gray-700 fs-9"></span>
                            </div>

                            <div class="row mt-1726-4 val_approval_request_response_time">
                                <span class="card_value text-5578eb" id="txt_approval_request_response_time">0.00 <span class="fs-4 text-gray-500 fw-normal ms-2">Days</span> </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-2 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row2_promo_plan" id="promo_plan_submitted">
                        <div class="card-body">
                            <div class="row">
                                <span class="fw-semibold text-gray-800 d-block fs-5">Promo Plan</span>
                                <span class="fw-bold text-gray-700 fs-9">submitted H-60 before start period</span>
                            </div>
                            <div class="row mt-4">
                                <span class="card_value text-ff1616 text-center" id="txtPromoPlanSubmittedBfrQuarterStart90_Score">0.00%</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row2_accuracy_promo" id="accuracy_promo_plan">
                        <div class="card-body">
                            <div class="row">
                                <div class="d-flex justify-content-between">
                                    <div class="col-auto">
                                        <span class="fw-semibold text-gray-800 d-block fs-5">Accuracy</span>
                                        <span class="fw-bold text-gray-700 fs-9">on OPTIMA input vs Promo plan</span>
                                    </div>
                                    <div class="col-auto ">
                                        <button class="btn btn-sm btn-icon btn-clin" id="btn_accuracy_promo_plan_hidden">
                                            <span class="fa fa-arrow-alt-circle-right fs-4"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-2">
                                <span class="card_value text-ff1616 text-center" id="txtScoreboardAccuracy_OptimaInput_vs_PromoPlan_pct">0.00%</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row2_accuracy_skp" id="accuracy_skp">
                        <div class="card-body">
                            <div class="row">
                                <div class="d-flex justify-content-between">
                                    <div class="col-auto">
                                        <span class="fw-semibold text-gray-800 d-block fs-5">Accuracy</span>
                                        <span class="fw-bold text-gray-700 fs-9">on SKP</span>
                                    </div>
                                    <div class="col-auto ">
                                        <button class="btn btn-sm btn-icon btn-clin" id="btn_accuracy_skp_hidden">
                                            <span class="fa fa-arrow-alt-circle-right fs-4"></span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-2">
                                <span class="card_value text-ff1616 text-center" id="txtScoreboardAccuracy_SKP_vs_Optima_pct">0.00%</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row2_recon_monitoring" id="recon_monitoring">
                        <div class="card-body">
                            <div class="row">
                                <span class="fw-semibold text-gray-800 d-block fs-5">Reconciliation Monitoring</span>
                            </div>
                            <div class="row mt-1387-8 mt-max-1386-8">
                                <span class="card_value text-ff1616 text-center" id="txtRecon_pct">0.00%</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-12 mb-5">
                    <div class="card shadow shadow-sm h-100 card_row2_promo_approval_sla" id="promo_approval">
                        <div class="card-body">
                            <div class="row">
                                <span class="fw-semibold text-gray-800 d-block fs-5">% Promo Approval</span>
                                <span class="fw-bold text-gray-700 fs-9">Timeliness SLA max.5 days for aprrover request</span>
                            </div>
                            <div class="row">
                                <span class="card_value text-58dd91" id="txt_promo_approval_sla">0.00<span class="text-gray-600 fw-normal ms-2">%</span></span>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-12">
                                    <div class="d-flex flex-column w-100 me-2">
                                        <div class="progress bg-ebedf2 h-15px w-100">
                                            <span class="text-gray-900 fs-7 mt-0 ms-1 fw-bold text-left position-absolute" id="txt_progress_promo_approval_sla">0.00%</span>
                                            <div class="progress-bar bg-58dd91" role="progressbar" id="progress_promo_approval_sla" style="width:0%" aria-valuenow="0.0" aria-valuemin="0" aria-valuemax="100">
                                            </div>
                                        </div>
                                        <span class="fs-4 text-end" id="txt_promo_approval_sla_sub_score">Sub Score: 0.00/0</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="d-none" id="accuracy_promo_plan_hidden">
                <div class="row mb-5">
                    <div class="col-lg-12 col-12">
                        <div class="row">
                            <div class="col-lg-12 col-12">
                                <div class="d-flex flex-column min-h-50px w-100 bg-98a9ff rounded-top ps-5">
                                    <span class="fs-2 fw-normal align-middle">Accuracy</span>
                                    <span class="fw-normal fs-7">on OPTIMA input vs Promo plan</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row mb-5">
                    <div class="col-lg-40perc col-12">
                        <div class="row mb-5">
                            <div class="col-lg-12 col-12">
                                <div class="card card-flush shadow shadow-sm accuracy_hidden">
                                    <div class="card-body d-flex flex-row">
                                        <div class="d-flex align-items-stretch justify-content-center rounded bg-98a9ff min-h-90px w-100px">
                                            <div class="align-middle align-self-center">
                                                <i class="fa fa-hourglass-end text-white fs-5rem"></i>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center ps-15">
                                            <span>Activity Period Matching</span>
                                            <span class="fs-3rem fw-bolder" id="txtOptimaPeriodMatch_Score">0.00<span class="fw-normal">/ 0</span></span>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center ms-auto">
                                            <span class="fs-2 text-ff1616" id="txtOptimaPeriodMatch_pct">0.00%</span>
                                        </div>
                                        <div class="d-flex">
                                            <div class="symbol symbol-30px symbol-circle m-1">
                                                <span class="symbol-label bg-ff1616" id="symOptimaPeriodMatch_pct">
                                                    <i class="fa fa-exclamation fs-2 text-white"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-5">
                            <div class="col-lg-12 col-12">
                                <div class="card card-flush shadow shadow-sm accuracy_hidden">
                                    <div class="card-body d-flex flex-row">
                                        <div class="d-flex align-items-stretch justify-content-center rounded bg-98a9ff min-h-100px w-100px">
                                            <div class="align-middle align-self-center">
                                                <i class="fa fa-list fs-5rem text-white"></i>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center ps-15">
                                            <span>Activity Desc & Mechanism Matching</span>
                                            <span class="fs-3rem fw-bolder" id="txtOptimaDescAndMechanismMatch_Score">0.00<span class="fw-normal">/ 0</span></span>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center ms-auto">
                                            <span class="fs-2 text-ff1616" id="txtOptimaDescAndMechanismMatch_pct">0.00%</span>
                                        </div>
                                        <div class="d-flex">
                                            <div class="symbol symbol-30px symbol-circle m-1">
                                                <span class="symbol-label bg-ff1616" id="symOptimaDescAndMechanismMatch_pct">
                                                    <i class="fa fa-exclamation fs-2 text-white"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-20perc col-12">
                        <div class="row mb-5">
                            <div class="col-12" id="containerChartAccuracy">
                                <div id="chartAccuracy" class="mx-auto h-200px w-200px"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-40perc col-12">
                        <div class="row mb-5">
                            <div class="col-lg-12 col-12">
                                <div class="card card-flush shadow shadow-sm accuracy_hidden">
                                    <div class="card-body d-flex flex-row-reverse">
                                        <div class="d-flex align-items-stretch justify-content-center rounded bg-98a9ff min-h-90px w-100px">
                                            <div class="align-middle align-self-center">
                                                <i class="fa fa-dollar-sign text-white fs-5rem"></i>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center pe-15 text-end">
                                            <span>Budget Amount Matching</span>
                                            <span class="fs-3rem fw-bolder" id="txtOptimaAmountMatch_Score">0.00<span class="fw-normal">/ 0</span></span>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center me-auto">
                                            <span class="fs-2 text-ff1616" id="txtOptimaAmountMatch_pct">0.00%</span>
                                        </div>
                                        <div class="d-flex">
                                            <div class="symbol symbol-30px symbol-circle m-1">
                                                <span class="symbol-label bg-ff1616" id="symOptimaAmountMatch_pct">
                                                    <i class="fa fa-exclamation fs-2 text-white"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-5">
                            <div class="col-lg-12 col-12">
                                <div class="card card-flush shadow shadow-sm accuracy_hidden">
                                    <div class="card-body d-flex flex-row-reverse">
                                        <div class="d-flex align-items-stretch justify-content-center rounded bg-98a9ff min-h-90px w-100px">
                                            <div class="align-middle align-self-center">
                                                <i class="fa fa-list text-white fs-5rem"></i>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center pe-15 text-end">
                                            <span>SKP draft H-30 from start period</span>
                                            <span class="fs-3rem fw-bolder" id="txtOptimaCreationBrfActivityStart60_Score">0.00<span class="fw-normal">/ 0</span></span>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center me-auto">
                                            <span class="fs-2 text-ff1616" id="txtOptimaCreationBrfActivityStart60_pct">0.00%</span>
                                        </div>
                                        <div class="d-flex">
                                            <div class="symbol symbol-30px symbol-circle m-1">
                                                <span class="symbol-label bg-ff1616" id="symOptimaCreationBrfActivityStart60_pct">
                                                    <i class="fa fa-exclamation fs-2 text-white"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="d-none" id="accuracy_skp_hidden">
                <div class="row mb-5">
                    <div class="col-lg-12 col-12">
                        <div class="row">
                            <div class="col-lg-12 col-12">
                                <div class="d-flex flex-column min-h-50px w-100 bg-fff598 rounded-top ps-5">
                                    <span class="fs-2 fw-normal align-middle">Accuracy</span>
                                    <span class="fw-normal fs-7">on OPTIMA input vs Promo plan</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row mb-5">
                    <div class="col-lg-40perc col-12">
                        <div class="row mb-5">
                            <div class="col-lg-12 col-12">
                                <div class="card card-flush shadow shadow-sm accuracy_hidden">
                                    <div class="card-body d-flex flex-row">
                                        <div class="d-flex align-items-stretch justify-content-center rounded bg-fff598 min-h-90px w-100px">
                                            <div class="align-middle align-self-center">
                                                <i class="fa fa-hourglass-end text-white fs-5rem"></i>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center ps-15">
                                            <span>Activity Period Matching</span>
                                            <span class="fs-3rem fw-bolder" id="txtSKPPeriodMatch_Score">0.00<span class="fw-normal">/ 0</span></span>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center ms-auto">
                                            <span class="fs-2 text-ff1616" id="txtSKPPeriodMatch_pct">0.00%</span>
                                        </div>
                                        <div class="d-flex">
                                            <div class="symbol symbol-30px symbol-circle m-1">
                                                <span class="symbol-label bg-ff1616" id="symSKPPeriodMatch_pct">
                                                    <i class="fa fa-exclamation fs-2 text-white"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-5">
                            <div class="col-lg-12 col-12">
                                <div class="card card-flush shadow shadow-sm accuracy_hidden">
                                    <div class="card-body d-flex flex-row">
                                        <div class="d-flex align-items-stretch justify-content-center rounded bg-fff598 min-h-100px w-100px">
                                            <div class="align-middle align-self-center">
                                                <i class="fa fa-list fs-5rem text-white"></i>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center ps-15">
                                            <span>Activity Desc & Mechanism Matching</span>
                                            <span class="fs-3rem fw-bolder" id="txtSKPDescAndMechanismMatch_Score">0.00<span class="fw-normal">/ 0</span></span>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center ms-auto">
                                            <span class="fs-2 text-ff1616" id="txtSKPDescAndMechanismMatch_pct">0.00%</span>
                                        </div>
                                        <div class="d-flex">
                                            <div class="symbol symbol-30px symbol-circle m-1">
                                                <span class="symbol-label bg-ff1616">
                                                    <i class="fa fa-exclamation fs-2 text-white" id="symSKPDescAndMechanismMatch_pct"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-20perc col-12">
                        <div class="row mb-5">
                            <div class="col-12" id="containerChartSKP">
                                <div id="chartSKP" class="mx-auto h-200px w-200px"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-40perc col-12">
                        <div class="row mb-5">
                            <div class="col-lg-12 col-12">
                                <div class="card card-flush shadow shadow-sm accuracy_hidden">
                                    <div class="card-body d-flex flex-row-reverse">
                                        <div class="d-flex align-items-stretch justify-content-center rounded bg-fff598 min-h-90px w-100px">
                                            <div class="align-middle align-self-center">
                                                <i class="fa fa-dollar-sign text-white fs-5rem"></i>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center pe-15 text-end">
                                            <span>Budget Ammount Matching</span>
                                            <span class="fs-3rem fw-bolder" id="txtSKPAmountMatch_Score">0.00<span class="fw-normal">/ 0</span></span>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center me-auto">
                                            <span class="fs-2 text-ff1616" id="txtSKPAmountMatch_pct">0.00%</span>
                                        </div>
                                        <div class="d-flex">
                                            <div class="symbol symbol-30px symbol-circle m-1">
                                                <span class="symbol-label bg-ff1616" id="symSKPAmountMatch_pct">
                                                    <i class="fa fa-exclamation fs-2 text-white"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-5">
                            <div class="col-lg-12 col-12">
                                <div class="card card-flush shadow shadow-sm accuracy_hidden">
                                    <div class="card-body d-flex flex-row-reverse">
                                        <div class="d-flex align-items-stretch justify-content-center rounded bg-fff598 min-h-90px w-100px">
                                            <div class="align-middle align-self-center">
                                                <i class="fa fa-list text-white fs-5rem"></i>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center pe-15 text-end">
                                            <span>SKP draft H-30 from start period</span>
                                            <span class="fs-3rem fw-bolder" id="">0.00<span class="fw-normal">/ 0</span></span>
                                        </div>
                                        <div class="d-flex flex-column justify-content-center me-auto">
                                            <span class="fs-2 text-ff1616" id="">0.00%</span>
                                        </div>
                                        <div class="d-flex">
                                            <div class="symbol symbol-30px symbol-circle m-1">
                                                <span class="symbol-label bg-ff1616" id="">
                                                    <i class="fa fa-exclamation fs-2 text-white"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mb-5">
                <div class="col-12">
                    <div class="card card-flush shadow shadow-sm h-100 card_creator_summary" id="">
                        <div class="card-header px-3">
                            <div class="d-flex align-items-center">
                                <img class="w-40px h-40px" src="{{ asset('/assets/media/bg/trophy.png') }}">

                                <div class="ms-3">
                                    <span class="text-start fs-2">Creator League Summary</span>
                                </div>
                            </div>
                        </div>
                        <div class="separator border-1"></div>
                        <div class="card-body d-lg-block d-none" id="creator_league_summary">
                            <div class="row">
                                <div class="offset-4 col-lg-2 px-10px">
                                    <div class="row">
                                        <div class="col-12 text-center">
                                            <span class="fw-boldest">LKA</span>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="separator border-3 border-secondary"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2 px-10px">
                                    <div class="row">
                                        <div class="col-12 text-center">
                                            <span class="fw-boldest">MM, HySu, WSKA</span>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="separator border-3 border-secondary"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2 px-10px">
                                    <div class="row">
                                        <div class="col-12 text-center">
                                            <span class="fw-boldest">GT, MTI, WSGT</span>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="separator border-3 border-secondary"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2 px-10px">
                                    <div class="row">
                                        <div class="col-12 text-center">
                                            <span class="fw-boldest">Pharma, MBS, E-Comm</span>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="separator border-3 border-secondary"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row my-2">
                                <div class="col-lg-4">
                                    <div class="row">
                                        <div class="col-12">
                                            <span class="fs-2">Promo Plan</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_promo_plan_lka">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_promo_plan_lka" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_promo_plan_mm">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_promo_plan_mm" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_promo_plan_gt">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_promo_plan_gt" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_promo_plan_medical">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_promo_plan_medical" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row my-2">
                                <div class="col-lg-4">
                                    <div class="row">
                                        <div class="col-12">
                                            <span class="fs-2">Accuracy promo</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_promo_lka">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_promo_lka" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_promo_mm">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_promo_mm" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_promo_gt">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_promo_gt" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_promo_medical">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_promo_medical" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row my-2">
                                <div class="col-lg-4">
                                    <div class="row">
                                        <div class="col-12">
                                            <span class="fs-2">Accuracy SKP</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_skp_lka">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_skp_lka" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_skp_mm">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_skp_mm" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_skp_gt">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_skp_gt" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_skp_medical">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_skp_medical" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row my-2">
                                <div class="col-lg-4">
                                    <div class="row">
                                        <div class="col-12">
                                            <span class="fs-2">Recon monitoring</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_recon_lka">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_recon_lka" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_recon_mm">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_recon_mm" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_recon_gt">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_recon_gt" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_recon_medical">0.00%</span>
                                                <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_recon_medical" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row my-2">
                                <div class="col-lg-4">
                                    <div class="row">
                                        <div class="col-12">
                                            <span class="fs-2">Total</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="d-flex flex-start">
                                                <span class="ms-2 text-gray-900 fs-3 fw-bold text-center" id="text_progress_total_lka">0.00%</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="d-flex flex-start">
                                                <span class="ms-2 text-gray-900 fs-3 fw-bold text-center" id="text_progress_total_mm">0.00%</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="d-flex flex-start">
                                                <span class="ms-2 text-gray-900 fs-3 fw-bold text-center" id="text_progress_total_gt">0.00%</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="d-flex flex-start">
                                                <span class="ms-2 text-gray-900 fs-3 fw-bold text-center" id="text_progress_total_medical">0.00%</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body d-lg-none d-block" id="creator_league_summary_mobile">
                            <div class="row mb-3">
                                <div class="col-12">
                                    <div class="row mb-2">
                                        <div class="col-12 text-center">
                                            <span class="fw-boldest">LKA</span>
                                            <div class="separator border-2"></div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Promo Plan</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_promo_plan_lka_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_promo_plan_lka_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Accuracy promo</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_promo_lka_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_promo_lka_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Accuracy SKP</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_skp_lka_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_skp_lka_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Recon monitoring</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_recon_lka_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_recon_lka_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Total</span>
                                        </div>
                                        <div class="col-6">
                                            <span class="fw-bold fs-3 ms-2" id="text_progress_total_lka_mobile">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-12">
                                    <div class="row mb-2">
                                        <div class="col-12 text-center">
                                            <span class="fw-boldest">MM, HySu, WSKA</span>
                                            <div class="separator border-2"></div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Promo Plan</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_promo_plan_mm_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_promo_plan_mm_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Accuracy promo</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_promo_mm_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_promo_mm_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Accuracy SKP</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_skp_mm_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_skp_mm_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Recon monitoring</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_recon_mm_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_recon_mm_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Total</span>
                                        </div>
                                        <div class="col-6">
                                            <span class="fw-bold fs-3 ms-2" id="text_progress_total_mm_mobile">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-12">
                                    <div class="row mb-2">
                                        <div class="col-12 text-center">
                                            <span class="fw-boldest">GT, MTI, WSGT</span>
                                            <div class="separator border-2"></div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Promo Plan</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_promo_plan_gt_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_promo_plan_gt_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Accuracy promo</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_promo_gt_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_promo_gt_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Accuracy SKP</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_skp_gt_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_skp_gt_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Recon monitoring</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_recon_gt_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_recon_gt_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Total</span>
                                        </div>
                                        <div class="col-6">
                                            <span class="fw-bold fs-3 ms-2" id="text_progress_total_gt_mobile">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="col-12">
                                    <div class="row mb-2">
                                        <div class="col-12 text-center">
                                            <span class="fw-boldest">Pharma, MBS, E-Comm</span>
                                            <div class="separator border-2"></div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Promo Plan</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_promo_plan_medical_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_promo_plan_medical_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Accuracy promo</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_promo_medical_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_promo_medical_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Accuracy SKP</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_accuracy_skp_medical_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_accuracy_skp_medical_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Recon monitoring</span>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-12">
                                                    <div class="progress bg-secondary h-25px w-100 rounded-0">
                                                        <span class="text-gray-900 fs-3 mt-0 ms-2 fw-bold text-left position-absolute" id="text_progress_recon_medical_mobile">0.00%</span>
                                                        <div class="progress-bar bg-58dd91" role="progressbar" id="progress_bar_recon_medical_mobile" style="width:0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-2">
                                        <div class="col-6 text-end py-1">
                                            <span class="fw-bold">Total</span>
                                        </div>
                                        <div class="col-6">
                                            <span class="fw-bold fs-3 ms-2" id="text_progress_total_medical_mobile">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mb-5">
            <div class="col-12">
                <div class="card card-flush shadow shadow-sm h-100 card_creator_standing" id="">
                    <div class="card-header px-3">
                        <div class="d-flex align-items-center">
                            <img class="w-40px h-40px" src="{{ asset('/assets/media/bg/win.png') }}">

                            <div class="ms-3">
                                <span class="text-start fs-2">Creator League Standing</span>
                            </div>
                        </div>
                    </div>
                    <div class="separator border-1"></div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-6 col-12 mb-5">
                                <div class="row">
                                    <div class="col-lg-12 col-12">
                                        <div class="d-table min-h-50px w-100 bg-04ff4b rounded-top text-center">
                                            <span class="d-table-cell fs-1 my-auto fw-bolder align-middle">
                                                TOP 3
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div id="list_top3_creator_league">
                                    <div class="row mt-3">
                                        <div class="col-lg-9 col-9 px-5">
                                            <span class="fs-6">-</span>
                                        </div>
                                        <div class="col-lg-3 col-3 px-5 text-end">
                                            <span class="fs-6">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-12 mb-5">
                                <div class="row">
                                    <div class="col-lg-12 col-12">
                                        <div class="d-table min-h-50px w-100 bg-ff1515 rounded-top text-center">
                                            <span class="d-table-cell fs-1 my-auto fw-bolder align-middle">
                                                BOTTOM 3
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div id="list_bottom3_creator_league">
                                    <div class="row mt-3">
                                        <div class="col-lg-9 col-9 px-5">
                                            <span class="fs-6">-</span>
                                        </div>
                                        <div class="col-lg-3 col-3 px-5 text-end">
                                            <span class="fs-6">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-3 col-12 mb-5">
                                <div class="row">
                                    <div class="col-lg-12 col-12">
                                        <div class="d-table min-h-40px w-100 rounded-top border border-dark text-center">
                                            <span class="d-table-cell fs-3 my-auto align-middle">
                                                1. LKA
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="list_header_lka" id="list_header_lka">
                                    <div class="row mt-3">
                                        <div class="col-lg-7 col-9 px-5">
                                            <span class="fs-6 fw-bolder">Name</span>
                                        </div>
                                        <div class="col-lg-5 col-3 px-5 text-end">
                                            <span class="fs-6 fw-bolder">Score</span>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-lg-7 col-7 px-5">
                                            <span class="fs-6">-</span>
                                        </div>
                                        <div class="col-lg-5 col-5 px-5 text-end">
                                            <span class="fs-6">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-12 mb-5">
                                <div class="row">
                                    <div class="col-lg-12 col-12">
                                        <div class="d-table min-h-40px w-100 rounded-top border border-dark text-center">
                                            <span class="d-table-cell fs-3 my-auto align-middle">
                                                2. MM, HySu, WSKA
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="list_header_mm" id="list_header_mm">
                                    <div class="row mt-3">
                                        <div class="col-lg-7 col-9 px-5">
                                            <span class="fs-6 fw-bolder">Name</span>
                                        </div>
                                        <div class="col-lg-5 col-3 px-5 text-end">
                                            <span class="fs-6 fw-bolder">Score</span>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-lg-7 col-9 px-5">
                                            <span class="fs-6">-</span>
                                        </div>
                                        <div class="col-lg-5 col-5 px-5 text-end">
                                            <span class="fs-6">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-12 mb-5">
                                <div class="row">
                                    <div class="col-lg-12 col-12">
                                        <div class="d-table min-h-40px w-100 rounded-top border border-dark text-center">
                                            <span class="d-table-cell fs-3 my-auto align-middle">
                                                3. GT, MTI, WSGT
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="list_header_gt" id="list_header_gt">
                                    <div class="row mt-3">
                                        <div class="col-lg-7 col-9 px-5">
                                            <span class="fs-6 fw-bolder">Name</span>
                                        </div>
                                        <div class="col-lg-5 col-3 px-5 text-end">
                                            <span class="fs-6 fw-bolder">Score</span>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-lg-7 col-9 px-5">
                                            <span class="fs-6">-</span>
                                        </div>
                                        <div class="col-lg-5 col-5 px-5 text-end">
                                            <span class="fs-6">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-12 mb-5">
                                <div class="row">
                                    <div class="col-lg-12 col-12">
                                        <div class="d-table min-h-40px w-100 rounded-top border border-dark text-center">
                                            <span class="d-table-cell fs-3 my-auto align-middle">
                                                4. Pharma, MBS, E-Comm
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="list_header_medical" id="list_header_medical">
                                    <div class="row mt-3">
                                        <div class="col-lg-7 col-9 px-5">
                                            <span class="fs-6 fw-bolder">Name</span>
                                        </div>
                                        <div class="col-lg-5 col-3 px-5 text-end">
                                            <span class="fs-6 fw-bolder">Score</span>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-lg-7 col-9 px-5">
                                            <span class="fs-6">-</span>
                                        </div>
                                        <div class="col-lg-5 col-5 px-5 text-end">
                                            <span class="fs-6">0.00%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

            <div class="row mb-5">
            <div class="col-12">
                <div class="card card-flush shadow shadow-sm h-100 card_approver_standing" id="">
                    <div class="card-header px-3">
                        <div class="d-flex align-items-center">
                            <img class="w-40px h-40px" src="{{ asset('/assets/media/bg/win.png') }}">

                            <div class="ms-3">
                                <span class="text-start fs-2">Approver League Top & Bottom</span>
                            </div>
                        </div>
                    </div>
                    <div class="separator border-1"></div>
                    <div class="card-body">
                        <div class="row mt-3">
                            <div class="col-lg-6 col-12 mb-5">
                                <div class="row">
                                    <div class="col-lg-10 col-12">
                                        <div class="row px-3">
                                            <div class="col-lg-6 col-6">
                                                <span class="fw-bolder">Top 5</span>
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
                                        <div class="separator border-1 border-04ff4b"></div>

                                        <div id="top5_approver">
                                            <div class="row px-3 py-3">
                                                <div class="col-lg-6 col-6">
                                                    <span class="fw-normal">-</span>
                                                </div>
                                                <div class="col-lg-2 col-6 text-end">
                                                    <span class="fw-normal">0.00%</span>
                                                </div>
                                                <div class="col-lg-2 col-6 text-end">
                                                    <span class="fw-normal">0.0</span>
                                                </div>
                                                <div class="col-lg-2 col-6 text-end">
                                                    <span class="fw-normal">0.0</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6 col-12 mb-5">
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

                                        <div id="bottom5_approver">
                                            <div class="row px-3 py-3">
                                                <div class="col-lg-6 col-6">
                                                    <span class="fw-normal">-</span>
                                                </div>
                                                <div class="col-lg-2 col-6 text-end">
                                                    <span class="fw-normal">0.00%</span>
                                                </div>
                                                <div class="col-lg-2 col-6 text-end">
                                                    <span class="fw-normal">0.0</span>
                                                </div>
                                                <div class="col-lg-2 col-6 text-end">
                                                    <span class="fw-normal">0.0</span>
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
        </div>
    </div>
@endsection

@section('vendor-script')
    <script src="{{ asset('/assets/plugins/custom/amcharts4/core.js') }}" type="text/javascript"></script>
    <script src="{{ asset('/assets/plugins/custom/amcharts4/charts.js') }}" type="text/javascript"></script>
    <script src="{{ asset('/assets/plugins/custom/amcharts4/themes/animated.js') }}" type="text/javascript"></script>
    <script src="{{ asset('/assets/plugins/custom/jspdf/jspdf.min.js') }}"></script>
    <script src="{{ asset('/assets/plugins/custom/html2canvas/html2canvas.min.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/dashboard/summary/js/index.js?v=' . microtime()) }}"></script>
@endsection

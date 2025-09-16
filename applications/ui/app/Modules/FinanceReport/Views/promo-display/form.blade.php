@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/finance-report/promo-display/css/promo-display-form.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back-export-pdf')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-5">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Year </label>
                                <div class="col-lg-4">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="year" id="year" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Promo Planning </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="promoPlanRefId" id="promoPlanRefId" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Select Budget Source </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="allocationRefId" id="allocationRefId" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Budget Allocation </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="allocationDesc" id="allocationDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Activity </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="activityLongDesc" id="activityLongDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Sub Activity </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="subActivityLongDesc" id="subActivityLongDesc" autocomplete="off"
                                           readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Activity Period </label>
                                <div class="col-lg-8">
                                    <div class="input-group input-group-sm mb-2">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg"
                                               name="startPromo" id="startPromo" readonly/>
                                        <span class="input-group-text">to</span>
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg"
                                               name="endPromo" id="endPromo" readonly/>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <label class="col-lg-4 col-form-label">Activity Name </label>
                                <div class="col-lg-8">
                                    <textarea type="text" class="form-control form-control-sm form-control-solid-bg"
                                              id="activityDesc" name="activityDesc" rows="3" autocomplete="off"
                                              readonly></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">TS Code </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="tsCoding" id="tsCoding" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Sub Category </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="subCategoryDesc" id="subCategoryDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Entity </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="principalName" id="principalName" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Distributor </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="distributorname" id="distributorname" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Budget Deployed </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end"
                                           type="text" name="budgetAmount" id="budgetAmount" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Remaining Budget</label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end"
                                           type="text" name="remainingBudget" id="remainingBudget" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Initiator </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text"
                                           name="initiator" id="initiator" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row mb-1">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Initiator Notes </label>
                                <div class="col-lg-6">
                                    <textarea type="text" class="form-control form-control-sm form-control-solid-bg"
                                              id="initiator_notes" name="initiator_notes" rows="3" autocomplete="off"
                                              readonly></textarea>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Investment Type </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg mb-1 mb-lg-0 text-end"
                                           type="text" name="investmentTypeDesc" id="investmentTypeDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="separator border-3 my-2 border-secondary"></div>

                    <div class="row mt-3">
                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Baseline Sales </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end"
                                           type="text" name="normalSales" id="normalSales" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Sales Increment </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end"
                                           type="text" name="incrSales" id="incrSales" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Investment </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end"
                                           type="text" name="investment" id="investment" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Investment Before Close </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end"
                                           type="text" name="investmentBfrClose" id="investmentBfrClose" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Investment Closed Balance </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end"
                                           type="text" name="investmentClosedBalance" id="investmentClosedBalance" autocomplete="off" readonly/>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Total Sales </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end" type="text"
                                           name="totSales" id="totSales" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Total Investment </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end" type="text"
                                           name="totInvestment" id="totInvestment" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">ROI </label>
                                <div class="col-lg-6">
                                    <div class="input-group input-group-sm">
                                        <input type="text"
                                               class="form-control form-control-sm form-control-solid-bg text-end"
                                               name="roi" id="roi" readonly/>
                                        <span class="input-group-text px-5">%</span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Cost Ratio </label>
                                <div class="col-lg-6">
                                    <div class="input-group input-group-sm">
                                        <input type="text"
                                               class="form-control form-control-sm form-control-solid-bg text-end"
                                               name="costRatio" id="costRatio" readonly/>
                                        <span class="input-group-text px-5">%</span>
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
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <span class="fw-bold text-gray-700 fs-4">Detail Attribute</span>
                    <div class="separator border-3 my-2 border-secondary"></div>
                    <div class="row mb-3">
                        <div class="col-lg-3">
                            <label class="col-form-label mt-1 me-5">Channel</label>
                            <input class="form-control form-control-sm form-control-solid-bg" type="text" name="channelDesc" id="channelDesc" autocomplete="off" readonly/>
                        </div>
                        <div class="col-lg-3">
                            <label class="col-form-label mt-1 me-5">Sub Channel</label>
                            <input class="form-control form-control-sm form-control-solid-bg" type="text" name="subchannelDesc" id="subchannelDesc" autocomplete="off" readonly/>
                        </div>
                        <div class="col-lg-3">
                            <label class="col-form-label mt-1 me-5">Account</label>
                            <input class="form-control form-control-sm form-control-solid-bg" type="text" name="accountDesc" id="accountDesc" autocomplete="off" readonly/>
                        </div>
                        <div class="col-lg-3">
                            <label class="col-form-label mt-1 me-5">Sub Account</label>
                            <input class="form-control form-control-sm form-control-solid-bg" type="text" name="subaccountDesc" id="subaccountDesc" autocomplete="off" readonly/>
                        </div>
                    </div>

{{--                    CARD REGION--}}
                    <div class="row">
                        <div class="col-lg-4">
                            <div class="card card-flush mb-3" id="card_region">
                                <div class="card-header collapsible cursor-pointer rotate collapsed" data-bs-toggle="collapse" data-bs-target="#kt_region_card_collapsible" aria-expanded="false">
                                    <span class="fw-bold fs-5 my-auto">Region</span>
                                    <div class="card-toolbar rotate-180">
                                        <i class="fa fa-chevron-down fs-3"></i>
                                    </div>
                                </div>
                                <div id="kt_region_card_collapsible" class="collapse" style="">
                                    <div class="card-body">
                                        <div class="px-2" id="card_list_region">

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

{{--                        CARD BRAND--}}
                        <div class="col-lg-4">
                            <div class="card card-flush mb-3" id="card_brand">
                                <div class="card-header collapsible cursor-pointer rotate collapsed" data-bs-toggle="collapse" data-bs-target="#kt_brand_card_collapsible" aria-expanded="false">
                                    <span class="fw-bold fs-5 my-auto">Sub Brand</span>
                                    <div class="card-toolbar rotate-180">
                                        <i class="fa fa-chevron-down fs-3"></i>
                                    </div>
                                </div>
                                <div id="kt_brand_card_collapsible" class="collapse" style="">
                                    <div class="card-body">
                                        <div class="px-2" id="card_list_brand">

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

{{--                        CARD SKU--}}
                        <div class="col-lg-4">
                            <div class="card card-flush mb-3" id="card_sku">
                                <div class="card-header collapsible cursor-pointer rotate collapsed" data-bs-toggle="collapse" data-bs-target="#kt_sku_card_collapsible" aria-expanded="false">
                                    <span class="fw-bold fs-5 my-auto">SKU</span>
                                    <div class="card-toolbar rotate-180">
                                        <i class="fa fa-chevron-down fs-3"></i>
                                    </div>
                                </div>
                                <div id="kt_sku_card_collapsible" class="collapse" style="">
                                    <div class="card-body">
                                        <div class="px-2" id="card_list_sku">

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                            <table id="dt_mechanism" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row mb-5">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <span class="fw-bold text-gray-700 fs-4">File Attachments</span>
                    <div class="separator border-3 my-2 border-secondary"></div>
                    <div class="row mt-5">
                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">1.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                           type="text" name="attachment1" id="attachment1" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download me-2"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download"
                                            value="1">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                            value="1">
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">2.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                           type="text" name="attachment2" id="attachment2" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download me-2"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download"
                                            value="2">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                            value="2">
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">3.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                           type="text" name="attachment3" id="attachment3" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download me-2"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download"
                                            value="3">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show" value="3">
                                        <span class="fa fa-eye text-white fs-12px"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">4.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                           type="text" name="attachment4" id="attachment4" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download me-2"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download"
                                            value="4">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                            value="4">
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">3.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                           type="text" name="attachment5" id="attachment5" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download me-2"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download"
                                            value="5">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                            value="5">
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">6.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                           type="text" name="attachment6" id="attachment6" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download me-2"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download"
                                            value="6">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                            value="6">
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">7.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                           type="text" name="attachment7" id="attachment7" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download me-2"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download"
                                            value="7">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_view"
                                            data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                            value="7">
                                        <span class="fa fa-eye text-white"> </span>
                                    </button>
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
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/finance-report/promo-display/js/promo-display-form.js?v=1') }}"></script>
@endsection

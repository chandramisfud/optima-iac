@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/debit-note/promo-display/css/promo-display-form.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold my-1 ms-1" id="txt_info_method">View </small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back-export-pdf')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <form id="form_promo" class="form" autocomplete="off">
        @csrf
        <div class="row mb-5 card_form">
            <div class="col-md-12 col-12">
                <div class="card shadow-sm mb-3">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Budget Year</label>
                                    <div class="col-lg-2 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="period" id="period" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Promo Planning</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="promoPlanRefId" name="promoPlanRefId" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Budget Source</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="allocationRefId" name="allocationRefId" autocomplete="off" readonly />
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Budget Allocation</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="allocationDesc" name="allocationDesc" autocomplete="off" readonly />
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="activityDesc" name="activityDesc" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Sub Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="subActivityDesc" name="subActivityDesc" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Activity Period</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" name="startPromo" id="startPromo" autocomplete="off" readonly/>
                                            <span class="input-group-text">to</span>
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" name="endPromo" id="endPromo" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Activity Name</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <textarea maxlength="255" class="form-control form-control-sm" name="activityDesc" id="activityDesc" autocomplete="off" tabindex="3" readonly></textarea>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">TS Code</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="tsCoding" name="tsCoding" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Sub Category</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="subCategoryDesc" name="subCategoryDesc" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Entity</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="entity" name="entity" autocomplete="off" readonly/>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Budget Deployed</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="budgetAmount" name="budgetAmount" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Remaining Budget</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="remainingBudget" name="remainingBudget" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Initiator Notes</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <textarea maxlength="255" class="form-control form-control-sm" name="initiatorNotes" id="initiatorNotes" autocomplete="off" tabindex="4" readonly></textarea>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Investment Type</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" id="investmentType" name="investmentType" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="separator border-2 my-2 border-secondary"></div>
                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Baseline Sales</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="baselineSales" id="baselineSales" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Sales Increment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" id="incrementSales" name="incrementSales" autocomplete="off" value="0" tabindex="6" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Investment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" id="investment" name="investment" autocomplete="off" value="0" tabindex="7" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Investment Before Close</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" id="investmentBfrClose" name="investmentBfrClose" autocomplete="off" value="0" tabindex="7" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required">Investment Closed Balance</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" id="investmentClosedBalance" name="investmentClosedBalance" autocomplete="off" value="0" tabindex="7" readonly/>
                                    </div>
                                </div>

                            </div>

                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Total Sales</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="totalSales" name="totalSales" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Total Investment</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="totalInvestment" name="totalInvestment" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">ROI</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="roi" name="roi" autocomplete="off" readonly/>
                                            <span class="input-group-text" id="addon-roi">%</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Cost Ratio</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="costRatio" name="costRatio" autocomplete="off" readonly/>
                                            <span class="input-group-text" id="addon-cr">%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row mb-3 card_attribute">
                    <div class="col-lg-12 col-12">
                        <div class="card shadow-sm">
                            <div class="card-body">
                                <div class="d-flex py-1 flex-grow-1 justify-content-between">
                                    <span class="fw-bold text-gray-700 fs-4">Detail Attribute</span>
                                </div>
                                <div class="separator border-2 my-1 border-secondary"></div>
                                <div class="row mb-5">
                                    <div class="col-lg-3 col-12">
                                        <div class="row fv-row">
                                            <label class="col-form-label col-lg-12 col-12 pb-1 required">Channel</label>
                                            <div class="col-lg-12 col-12">
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" id="channel" name="channel" autocomplete="off" readonly/>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-12">
                                        <div class="row fv-row">
                                            <label class="col-form-label col-lg-12 col-12 pb-1 required">Sub Channel</label>
                                            <div class="col-lg-12 col-12">
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" id="subChannel" name="subChannel" autocomplete="off" readonly/>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-12">
                                        <div class="row fv-row">
                                            <label class="col-form-label col-lg-12 col-12 pb-1 required">Account</label>
                                            <div class="col-lg-12 col-12">
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" id="account" name="account" autocomplete="off" readonly/>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-12">
                                        <div class="row fv-row">
                                            <label class="col-form-label col-lg-12 col-12 pb-1 required">Sub Account</label>
                                            <div class="col-lg-12 col-12">
                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" id="subAccount" name="subAccount" autocomplete="off" readonly/>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-4 col-12">
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

                                    <div class="col-lg-4 col-12">
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

                                    <div class="col-lg-4 col-12">
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

                <div class="row card_attachment">
                    <div class="col-md-12 col-12">
                        <div class="card shadow-sm">
                            <div class="card-body">
                                <div class="d-flex py-1 flex-grow-1 justify-content-between">
                                    <span class="fw-bold text-gray-700 fs-4">File attachments</span>
                                </div>
                                <div class="separator border-2 my-2 border-secondary"></div>
                                <div class="row">
                                    <div class="col-lg-5 col-md-12 col-12">
                                        <div class="row">
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">1.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment1" role="button">Browse...</label>
                                                    <label for="attachment1" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3 form-control-solid-bg" id="review_file_label_1" role="button">Choose File</label>
                                                </div>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" id="btn_download1" name="btn_download1" value="1" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view " id="btn_view1" name="btn_view1"
                                                        data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                                        value="1" disabled>
                                                    <span class="fa fa-eye text-white"> </span>
                                                </button>

                                                <div class="invisible" id="info1">
                                                    <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">2.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment2" role="button">Browse...</label>
                                                    <label for="attachment2" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3 form-control-solid-bg" id="review_file_label_2" role="button">Choose File</label>
                                                </div>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" id="btn_download2" name="btn_download2" value="2" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view " id="btn_view2" name="btn_view2"
                                                        data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                                        value="2" disabled>
                                                    <span class="fa fa-eye text-white"> </span>
                                                </button>
                                                <div class="invisible" id="info2">
                                                    <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">3.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment3" role="button">Browse...</label>
                                                    <label for="attachment3" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3 form-control-solid-bg" id="review_file_label_3" role="button">Choose File</label>
                                                </div>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" id="btn_download3" name="btn_download3" value="3" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view " id="btn_view3" name="btn_view3"
                                                        data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                                        value="3" disabled>
                                                    <span class="fa fa-eye text-white"> </span>
                                                </button>

                                                <div class="invisible" id="info3">
                                                    <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">4.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment4" role="button">Browse...</label>
                                                    <label for="attachment4" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3 form-control-solid-bg" id="review_file_label_4" role="button">Choose File</label>
                                                </div>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" id="btn_download4" name="btn_download4" value="4" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view " id="btn_view4" name="btn_view4"
                                                        data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                                        value="4" disabled>
                                                    <span class="fa fa-eye text-white"> </span>
                                                </button>

                                                <div class="invisible" id="info4">
                                                    <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="offset-lg-1 col-lg-5 col-md-12 col-12">
                                        <div class="row">
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">5.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment5" role="button">Browse...</label>
                                                    <label for="attachment5" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3 form-control-solid-bg" id="review_file_label_5" role="button">Choose File</label>
                                                </div>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" id="btn_download5" name="btn_download5" value="5" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view " id="btn_view5" name="btn_view5"
                                                        data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                                        value="5" disabled>
                                                    <span class="fa fa-eye text-white"> </span>
                                                </button>

                                                <div class="invisible" id="info5">
                                                    <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">6.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment6" role="button">Browse...</label>
                                                    <label for="attachment6" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3 form-control-solid-bg" id="review_file_label_6" role="button">Choose File</label>
                                                </div>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" id="btn_download6" name="btn_download6" value="6" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view " id="btn_view6" name="btn_view6"
                                                        data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                                        value="6" disabled>
                                                    <span class="fa fa-eye text-white"> </span>
                                                </button>

                                                <div class="invisible" id="info6">
                                                    <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="d-flex align-items-center">
                                                <label class="col-form-label mt-1 me-5">7.</label>
                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                    <label class="input-group-text fs-12px" for="attachment7" role="button">Browse...</label>
                                                    <label for="attachment7" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3 form-control-solid-bg" id="review_file_label_7" role="button">Choose File</label>
                                                </div>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" id="btn_download7" name="btn_download7" value="7" disabled>
                                                    <span class="fa fa-download"></span>
                                                </button>
                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view " id="btn_view7" name="btn_view7"
                                                        data-bs-toggle="tooltip" data-bs-placement="bottom" title="Show"
                                                        value="7" disabled>
                                                    <span class="fa fa-eye text-white"> </span>
                                                </button>

                                                <div class="invisible" id="info7">
                                                    <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
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
    </form>
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/debit-note/promo-display/js/promo-display-form.js?v=2') }}"></script>
@endsection


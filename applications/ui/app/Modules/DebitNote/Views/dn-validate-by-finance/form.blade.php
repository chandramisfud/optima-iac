@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/debit-note/dn-validate-by-finance/css/dn-validate-by-finance-form.css?v=' . microtime()) }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <div class="row">
                        <form id="form_validate_by_finance" class="form" autocomplete="off">
                            @csrf
                            <div class="row mb-2">
                                <div class="col-lg-6">
                                    <small class="fs-5 fw-bold mt-1 ms-1">Notes</small>
                                </div>
                            </div>
                            <div class="row fv-row">
                                <div class="col-lg-6">
                                    <select class="form-select form-select-sm" data-control="select2" name="approvalStatusCode" id="approvalStatusCode" data-placeholder="Select validation status" autocomplete="off" data-allow-clear="true">
                                        <option></option>
                                    </select>
                                </div>
                                <div class="col-lg-6">
                                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100 mb-2" id="btn_submit">
                                    <span class="indicator-label">
                                    <span class="fa fa-check"></span> Submit
                                    </span>
                                        <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row fv-row mb-2">
                                <div class="col-lg-6 mb-2">
                                    <textarea type="text" class="form-control form-control-sm" id="notes" name="notes" rows="3" autocomplete="off"></textarea>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 offset-lg-12">
                                    <div class="separator border-3 my-2 border-secondary"></div>
                                </div>
                                <small class="fs-5 fw-bold mt-1 ms-1">Document Completeness</small>
                                <div class="row">
                                    <div class="col-lg-3 col-12 p-5">
                                        <div class="col-lg-12 col-form-label">
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" id="Original_Invoice_from_retailers" name="Original_Invoice_from_retailers" autocomplete="off"/>
                                                <label class="form-check-label" for="Original_Invoice_from_retailers">
                                                    Original Invoice from retailers
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-lg-12 col-form-label">
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" id="Tax_Invoice" name="Tax_Invoice" autocomplete="off"/>
                                                <label class="form-check-label" for="Tax_Invoice">
                                                    Tax Invoice
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-lg-12 col-form-label">
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" id="Promotion_Agreement_Letter" name="Promotion_Agreement_Letter" autocomplete="off"/>
                                                <label class="form-check-label" for="Promotion_Agreement_Letter">
                                                    Promotion Agreement Letter
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-lg-12 col-form-label">
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" id="Trading_Term" name="Trading_Term" autocomplete="off"/>
                                                <label class="form-check-label" for="Trading_Term">
                                                    Trading Term
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-12 p-5">
                                        <div class="col-lg-12 col-form-label">
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" id="Sales_Data" name="Sales_Data" autocomplete="off"/>
                                                <label class="form-check-label" for="Sales_Data">
                                                    Sales Data (sell in/sell out)
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-lg-12 col-form-label">
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" id="Copy_of_mailer" name="Copy_of_mailer" autocomplete="off"/>
                                                <label class="form-check-label" for="Copy_of_mailer">
                                                    Copy of mailer
                                                </label>
                                            </div>
                                        </div>
                                        <div class="col-lg-12 col-form-label">
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" id="Copy_of_photo_doc" name="Copy_of_photo_doc" autocomplete="off"/>
                                                <label class="form-check-label" for="Copy_of_photo_doc">
                                                    Copy of photo documentation
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row d-none" id="btn-save-document-completeness">
                                    <div class="col-lg-3 col-12 p-5">
                                        <button type="button" class="btn btn-sm btn-optima w-lg-auto w-100 mb-2" id="btn_save">
                                        <span class="indicator-label">
                                        <span class="fa fa-save"></span> Save
                                        </span>
                                            <span class="indicator-progress">
                                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                        </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="col-lg-12 offset-lg-12">
                                    <div class="separator border-3 my-2 border-secondary"></div>
                                </div>
                            </div>
                            <div class="row mt-2 ms-1" id="tab_detail_validate_by_finance">
                                <ul class="nav nav-tabs">
                                    <li class="nav-item">
                                        <a class="nav-link active" data-bs-toggle="tab" data-bs-target="#detail" id="tabDetail">Detail</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" data-bs-toggle="tab" data-bs-target="#promo" id="tabPromo">Promo</a>
                                    </li>
                                </ul>
                                <div class="tab-content" id="tab_detail_validate_by_finance">
                                    <div class="tab-pane show active" id="detail">
                                        <div class="row mt-5 me-3">
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="year">Year </label>
                                                    <div class="col-lg-4">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="year" id="year" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row fv-row">
                                                    <label class="col-lg-4 col-form-label required" id='subAccountID-iconRequired'>Sub Account</label>
                                                    <div class="col-lg-8">
                                                        <select class="form-select form-select-sm" data-control="select2" name="subAccountId" id="subAccountId" data-placeholder="Select a Sub Account"  data-allow-clear="true" tabindex="2">
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="sellingPoint">Selling Point </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="sellingPoint" id="sellingPoint" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row fv-row">
                                                    <label class="col-lg-4 col-form-label" for="promoRefId">Promo ID</label>
                                                    <div class="col-lg-8">
                                                        <div class="input-group input-group-sm">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="promoRefId" name="promoRefId" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly>
                                                            <button class="input-group-text cursor-pointer btn-outline-optima d-none" id="btn_search_promo" tabindex="4">
                                                                <span class="fa fa-search"></span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="entityLongDesc">Entity </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="entityLongDesc" id="entityLongDesc" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="entityAddress">Address </label>
                                                    <div class="col-lg-8">
                                                        <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="entityAddress" name="entityAddress" rows="3" autocomplete="off" readonly></textarea>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="entityUp">Up. </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="entityUp" id="entityUp" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="activityDesc">Description </label>
                                                    <div class="col-lg-8">
                                                        <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="activityDesc" name="activityDesc" rows="3" autocomplete="off" readonly></textarea>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="feeDesc">Fee Description </label>
                                                    <div class="col-lg-8">
                                                        <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="feeDesc" name="feeDesc" rows="3" autocomplete="off" readonly></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                <div class="row d-none" id="is-dn-promo">
                                                    <label class="col-lg-10 offset-lg-2 col-form-label">
                                                        <div class="form-check">
                                                            <input class="form-check-input" type="checkbox" value="1" id="isDNPromo" name="isDNPromo" checked />
                                                            <label class="form-check-label" for="isDNPromo">
                                                                Trade Promo
                                                            </label>
                                                        </div>
                                                    </label>
                                                </div>
                                                <div class="row fv-row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="whtType">WHT Type </label>
                                                    <div class="col-lg-6">
                                                        <div class="row">
                                                            <div class="d-flex flex-row align-items-center">
                                                                <select class="form-select form-select-sm" data-control="select2" name="whtType" id="whtType" data-placeholder="Select a WHT Type"  data-allow-clear="true">
                                                                    <option></option>
                                                                </select>
                                                                <div class="tooltip_custom d-none" id="icon_info_wht">
                                                                    <span class="fa fa-info-circle fs-2 mt-1 ms-3 text-danger cursor-pointer" id="info_wht_type"></span>
                                                                    <span class="tooltip_text">WHT No Deduct will exclude PPH value in Total Claim calculation</span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row fv-row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="taxLevel">Tax Level </label>
                                                    <div class="col-lg-6">
                                                        <select class="form-select form-select-sm" data-control="select2" name="taxLevel" id="taxLevel" data-placeholder="Select a Tax Level"  data-allow-clear="true">
                                                            <option></option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="deductionDate">Deduction Date </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="deductionDate" id="deductionDate" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="memDocNo">Memorial Doc. No </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="memDocNo" id="memDocNo" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="intDocNo">Internal Doc. No </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="intDocNo" id="intDocNo" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="periode">Period </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="periode" id="periode" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="dnAmount">DN Amount </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="dnAmount" id="dnAmount" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="feePct">Fee </label>
                                                    <div class="col-lg-6">
                                                        <div class="input-group input-group-sm">
                                                            <div class="col-3">
                                                                <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="feePct" id="feePct" readonly/>
                                                            </div>
                                                            <span class="input-group-text px-5">%</span>
                                                            <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="feeAmount" id="feeAmount" autocomplete="off" readonly/>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="dpp">DPP </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg mb-1 mb-lg-0 text-end" value="0" type="text" name="dpp" id="dpp" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-4 offset-lg-2">
                                                        <input class="form-control form-control-sm form-control-solid-bg mb-1 mb-lg-0" type="text" name="statusPPN" id="statusPPN" autocomplete="off" readonly/>
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="input-group input-group-sm">
                                                            <div class="col-3">
                                                                <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="ppnPct" id="ppnPct" readonly/>
                                                            </div>
                                                            <span class="input-group-text px-5">%</span>
                                                            <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="ppnAmt" id="ppnAmt" autocomplete="off" readonly/>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-4 offset-lg-2">
                                                        <select class="form-select form-select-sm" data-control="select2" name="statusPPH" id="statusPPH" autocomplete="off" data-allow-clear="true" readonly>
                                                            <option value="">&nbsp;</option>
                                                            <option value="DN Amount PPH">PPH - DN Amount</option>
                                                            <option value="FEE PPH">PPH - Fee</option>
                                                            <option value="DPP PPH">PPH - DPP</option>
                                                        </select>
{{--                                                        <input class="form-control form-control-sm form-control-solid-bg mb-1 mb-lg-0" type="text" name="statusPPH" id="statusPPH" autocomplete="off" readonly/>--}}
                                                    </div>
                                                    <div class="col-lg-6">
                                                        <div class="input-group input-group-sm">
                                                            <div class="col-3">
                                                                <input type="text" class="form-control form-control-sm text-end" name="pphPct" id="pphPct" readonly/>
                                                            </div>
                                                            <span class="input-group-text px-5">%</span>
                                                            <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="pphAmt" id="pphAmt" autocomplete="off" readonly/>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-6 offset-lg-6">
                                                        <div class="separator border-3 my-2 border-secondary"></div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label">Total </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg text-end fw-bold fs-3" value="0" type="text" name="total" id="total" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row mt-3">
                                                <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                    <div class="row">
                                                        <label class="col-lg-4 col-form-label" for="fpNumber">FP Number </label>
                                                        <div class="col-lg-6">
                                                            <input class="form-control form-control-sm form-control-solid-bg" type="text" name="fpNumber" id="fpNumber" autocomplete="off" readonly/>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <label class="col-lg-4 col-form-label" for="fpDate">FP Date </label>
                                                        <div class="col-lg-6">
                                                            <input class="form-control form-control-sm form-control-solid-bg" type="text" name="fpDate" id="fpDate" autocomplete="off" readonly/>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row mt-3">
                                                <label class="col-lg-4 col-form-label">Attachment </label>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachment1">1.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment1" id="attachment1" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" value="1">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-2"
                                                                    title="Preview"
                                                                    value="1">
                                                                <span class="fa fa-eye text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachment2">2.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment2" id="attachment2" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" value="2">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-2"
                                                                    title="Preview"
                                                                    value="2">
                                                                <span class="fa fa-eye text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachment3">3.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment3" id="attachment3" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" value="3">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-2"
                                                                    title="Preview"
                                                                    value="3">
                                                                <span class="fa fa-eye text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachment4">4.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment4" id="attachment4" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" value="4">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-2"
                                                                    title="Preview"
                                                                    value="4">
                                                                <span class="fa fa-eye text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachment5">5.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment5" id="attachment5" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" value="5">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-2"
                                                                    title="Preview"
                                                                    value="5">
                                                                <span class="fa fa-eye text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachment6">6.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment6" id="attachment6" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" value="6">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-2"
                                                                    title="Preview"
                                                                    value="6">
                                                                <span class="fa fa-eye text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachment7">7.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment7" id="attachment7" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" value="7">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-2"
                                                                    title="Preview"
                                                                    value="7">
                                                                <span class="fa fa-eye text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachment8">8.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment8" id="attachment8" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" value="8">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-2"
                                                                    title="Preview"
                                                                    value="8">
                                                                <span class="fa fa-eye text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachment9">9.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment9" id="attachment9" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" title="Download" value="9">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_view ms-2"
                                                                    title="Preview"
                                                                    value="9">
                                                                <span class="fa fa-eye text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 ms-8 w-100" for="download_all">Download ALL (ZIP)</label>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" id="download_all" title="Download" value="all">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane" id="promo">
                                        <div class="row mt-2" id="detailPromoRC">
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                <div class="row mt-3">
                                                    <label class="col-lg-4 col-form-label" for="yearPromo">Year </label>
                                                    <div class="col-lg-4">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="yearPromo" id="yearPromo" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="promoPlanRefId">Promo Planning </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="promoPlanRefId" id="promoPlanRefId" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="allocationRefId">Select Budget Source </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="allocationRefId" id="allocationRefId" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="allocationDesc">Budget Allocation </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="allocationDesc" id="allocationDesc" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="activityLongDesc">Activity </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="activityLongDesc" id="activityLongDesc" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="subActivityLongDesc">Sub Activity </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="subActivityLongDesc" id="subActivityLongDesc" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label">Activity Period </label>
                                                    <div class="col-lg-8">
                                                        <div class="input-group input-group-sm mb-2">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" name="startPromo" id="startPromo" readonly/>
                                                            <span class="input-group-text">to</span>
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" name="endPromo" id="endPromo" readonly/>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="tsCoding">TS Code </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="tsCoding" id="tsCoding" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="subCategoryDesc">Sub Category </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="subCategoryDesc" id="subCategoryDesc" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="principalName">Entity </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="principalName" id="principalName" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="distributorname">Distributor </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="distributorname" id="distributorname" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="budgetAmount">Budget Deployed </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetAmount" id="budgetAmount" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="remainingBudget">Remaining Budget</label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="remainingBudget" id="remainingBudget" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="separator border-3 my-2 border-secondary"></div>
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="normalSales">Baseline Sales </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg text-end"
                                                               type="text" name="normalSales" id="normalSales" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="incrSales">Sales Increment </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg text-end"
                                                               type="text" name="incrSales" id="incrSales" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 col-form-label" for="investment">Investment </label>
                                                    <div class="col-lg-8">
                                                        <input class="form-control form-control-sm form-control-solid-bg text-end"
                                                               type="text" name="investment" id="investment" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="totSales">Total Sales </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text"
                                                               name="totSales" id="totSales" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="totInvestment">Total Investment </label>
                                                    <div class="col-lg-6">
                                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text"
                                                               name="totInvestment" id="totInvestment" autocomplete="off" readonly/>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="roi">ROI </label>
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
                                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="costRatio">Cost Ratio </label>
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
                                            <div class="separator border-3 my-2 border-secondary"></div>
                                                <span class="fw-bold text-gray-700 fs-4">Detail Attribute</span>
                                            <div class="separator border-3 my-2 border-secondary"></div>
                                            <div class="row mb-3">
                                                <div class="col-lg-3">
                                                    <label class="col-form-label mt-1 me-5" for="channelDesc">Channel</label>
                                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="channelDesc" id="channelDesc" autocomplete="off" readonly/>
                                                </div>
                                                <div class="col-lg-3">
                                                    <label class="col-form-label mt-1 me-5" for="subchannelDesc">Sub Channel</label>
                                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="subchannelDesc" id="subchannelDesc" autocomplete="off" readonly/>
                                                </div>
                                                <div class="col-lg-3">
                                                    <label class="col-form-label mt-1 me-5" for="accountDesc">Account</label>
                                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="accountDesc" id="accountDesc" autocomplete="off" readonly/>
                                                </div>
                                                <div class="col-lg-3">
                                                    <label class="col-form-label mt-1 me-5" for="subaccountDesc">Sub Account</label>
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
                                                            <span class="fw-bold fs-5 my-auto">Brand</span>
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
                                            <div class="separator border-3 my-2 border-secondary"></div>
                                            <span class="fw-bold text-gray-700 fs-4">Attachments</span>
                                            <div class="separator border-3 my-2 border-secondary"></div>
                                            <div class="row card_attachment mt-5">
                                                <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachmentPromo1">1.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                                                   type="text" name="attachmentPromo1" id="attachmentPromo1" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download_promo me-2"
                                                                    title="Download"
                                                                    value="1">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachmentPromo2">2.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                                                   type="text" name="attachmentPromo2" id="attachmentPromo2" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download_promo me-2"
                                                                    title="Download"
                                                                    value="2">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachmentPromo3">3.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                                                   type="text" name="attachmentPromo3" id="attachmentPromo3" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download_promo me-2"
                                                                    title="Download"
                                                                    value="3">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachmentPromo4">4.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                                                   type="text" name="attachmentPromo4" id="attachmentPromo4" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download_promo me-2"
                                                                    title="Download"
                                                                    value="4">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachmentPromo5">3.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                                                   type="text" name="attachmentPromo5" id="attachmentPromo5" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download_promo me-2"
                                                                    title="Download"
                                                                    value="5">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachmentPromo6">6.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                                                   type="text" name="attachmentPromo6" id="attachmentPromo6" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download_promo me-2"
                                                                    title="Download"
                                                                    value="6">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="row mb-3">
                                                        <div class="d-flex w-lg-85">
                                                            <label class="col-form-label mt-1 me-5" for="attachmentPromo7">7.</label>
                                                            <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1"
                                                                   type="text" name="attachmentPromo7" id="attachmentPromo7" autocomplete="off" readonly/>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 btn_download_promo me-2"
                                                                    title="Download"
                                                                    value="7">
                                                                <span class="fa fa-download text-white"> </span>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row mt-2" id="detailPromoDC">
                                            <div class="row">
                                                <div class="col-lg-6 col-12">
                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="period">Budget Year</label>
                                                        <div class="col-lg-4 col-md-6 col-sm-12 col-12">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="period" id="period" value="" autocomplete="off" tabindex="1" readonly/>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-lg-6 col-12">
                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entityDesc">Entity</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="entityDesc" id="entityDesc" value="" autocomplete="off" tabindex="2" readonly/>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="groupBrandDescDC">Brand</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="groupBrandDescDC" id="groupBrandDescDC" value="" autocomplete="off" tabindex="3" readonly/>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="distributorNameDC">Distributor</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="distributorNameDC" id="distributorNameDC" value="" autocomplete="off" tabindex="4" readonly/>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subCategoryDescDC">Sub Activity Type</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="subCategoryDescDC" id="subCategoryDescDC" value="" autocomplete="off" tabindex="5" readonly/>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityLongDescDC">Sub Activity</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="subActivityLongDescDC" id="subActivityLongDescDC" value="" autocomplete="off" tabindex="6" readonly/>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="channelDescDC">Channel</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="channelDescDC" id="channelDescDC" autocomplete="off" tabindex="7" readonly/>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="btn_search_budget">Budget Source</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                            <div class="input-group input-group-sm has-validation">
                                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" id="allocationRefIdDC" name="allocationRefIdDC" autocomplete="off" tabindex="8" readonly>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label">Activity Period</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                            <div class="input-group input-group-sm">
                                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" id="startPromoDC" name="startPromoDC" autocomplete="off" tabindex="9" readonly>
                                                                <span class="input-group-text">to</span>
                                                                <input type="text" class="form-control form-control-sm form-control-solid-bg" id="endPromoDC" name="endPromoDC" autocomplete="off" tabindex="10" readonly>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="mechanismDC">Mechanism</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="mechanismDC" name="mechanismDC" autocomplete="off" tabindex="11" readonly>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="investmentDC">Investment</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="investmentDC" id="investmentDC" autocomplete="off" tabindex="12" readonly/>
                                                        </div>
                                                    </div>

                                                    <div class="row fv-row">
                                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotesDC">Initiator Notes</label>
                                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                                            <input class="form-control form-control-sm" type="text" name="initiatorNotesDC" id="initiatorNotesDC" tabindex="13" autocomplete="off" readonly/>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-lg-6 col-12">
                                                    <div class="row fv-row">
                                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationDescDC">Budget Allocation</label>
                                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                            <input class="form-control form-control-sm form-control-solid-bg" type="text" name="allocationDescDC" id="allocationDescDC" autocomplete="off" readonly />
                                                        </div>
                                                    </div>
                                                    <div class="row fv-row">
                                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetDeployedDC">Budget Deployed</label>
                                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                            <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetDeployedDC" id="budgetDeployedDC" autocomplete="off" inputmode="decimal" readonly />
                                                        </div>
                                                    </div>
                                                    <div class="row fv-row">
                                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetRemainingDC">Remaining Budget</label>
                                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                                            <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetRemainingDC" id="budgetRemainingDC" autocomplete="off" inputmode="decimal" readonly />
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="bottom-0 w-lg-50 w-100 position-lg-absolute pb-5">
                                                            <div class="col-12">
                                                                <div class="row mt-5">
                                                                    <div class="offset-lg-2 col-lg-10 col-md-12 col-12">
                                                                        <span class="fw-bold text-gray-700 fs-4">File attachments</span>
                                                                        <div class="separator border-2 my-2 border-secondary"></div>
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="offset-lg-2 col-lg-10 col-12">
                                                                        <div class="row fv-row">
                                                                            <div class="d-flex align-items-start">
                                                                                <label class="col-form-label me-5">1.</label>
                                                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                                                    <label for="attachment1" class="form-control form-control-sm form-control-solid-bg text-gray-700 fs-12px review_file_label me-3" id="review_file_label_1" role="button">For SKP Draft</label>
                                                                                </div>
                                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_dc" title="Download" id="btn_download_dc1" name="btn_download1" value="1" disabled>
                                                                                    <span class="fa fa-download"></span>
                                                                                </button>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="offset-lg-2 col-lg-10 col-12">
                                                                        <div class="row fv-row">
                                                                            <div class="d-flex align-items-start">
                                                                                <label class="col-form-label me-5">2.</label>
                                                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                                                    <label for="attachment2" class="form-control form-control-sm form-control-solid-bg text-gray-700 fs-12px review_file_label me-3" id="review_file_label_2" role="button">For SKP Fully Approved</label>
                                                                                </div>
                                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_dc" title="Download" id="btn_download_dc2" name="btn_download2" value="2" disabled>
                                                                                    <span class="fa fa-download"></span>
                                                                                </button>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="row mb-1">
                                                                    <div class="offset-lg-2 col-lg-10 col-12">
                                                                        <div class="row fv-row">
                                                                            <div class="d-flex align-items-center">
                                                                                <label class="col-form-label me-5">3.</label>
                                                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                                                    <label for="attachment3" class="form-control form-control-sm form-control-solid-bg text-gray-700 fs-12px review_file_label me-3" id="review_file_label_3" role="button"></label>
                                                                                </div>
                                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_dc" title="Download" id="btn_download_dc3" name="btn_download3" value="3" disabled>
                                                                                    <span class="fa fa-download"></span>
                                                                                </button>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="row">
                                                                    <div class="offset-lg-2 col-lg-10 col-12">
                                                                        <div class="row fv-row">
                                                                            <div class="d-flex align-items-center">
                                                                                <label class="col-form-label me-5">4.</label>
                                                                                <div class="input-group input-group-sm w-70 custom-file-button">
                                                                                    <label for="attachment4" class="form-control form-control-sm form-control-solid-bg text-gray-700 fs-12px review_file_label me-3" id="review_file_label_4" role="button"></label>
                                                                                </div>
                                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download_dc" title="Download" id="btn_download_dc4" name="btn_download4" value="4" disabled>
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
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>

    @include('DebitNote::dn-validate-by-finance.promo-list')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/debit-note/dn-validate-by-finance/js/dn-validate-by-finance-form.js?v=41') }}"></script>
    <script src="{{ asset('assets/pages/debit-note/dn-validate-by-finance/js/promo-list.js?v=12') }}"></script>
@endsection

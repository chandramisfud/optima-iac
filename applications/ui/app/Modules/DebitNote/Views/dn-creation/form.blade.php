@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/debit-note/dn-creation/css/dn-creation-form.css') }}" rel="stylesheet" type="text/css"/>
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
                    <form id="form_dn" class="form" autocomplete="off">
                        @csrf
                        <div class="row">
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required" for="period">Year</label>
                                    <div class="col-lg-4">
                                        <div class="input-group input-group-sm" id="dialer_period">
                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                <i class="fa fa-minus fs-2"></i>
                                            </button>

                                            <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="period" id="period" value="{{ @date('Y') }}" autocomplete="off" tabindex="1"/>

                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                <i class="fa fa-plus fs-2"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required" id='subAccountID-iconRequired' for="subAccountId">Sub Account</label>
                                    <div class="col-lg-8">
                                        <select class="form-select form-select-sm" data-control="select2" name="subAccountId" id="subAccountId" data-placeholder="Select a Sub Account"  data-allow-clear="true" tabindex="2">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 col-form-label" for="sellingPointId">Selling Point</label>
                                    <div class="col-lg-8">
                                        <select class="form-select form-select-sm" data-control="select2" name="sellingPointId" id="sellingPointId" data-placeholder="Select a Selling Point"  data-allow-clear="true" tabindex="3">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label" for="promoRefId">Promo ID</label>
                                    <div class="col-lg-8">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="promoRefId" name="promoRefId" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly>
                                            <span class="input-group-text cursor-pointer btn-outline-optima" id="btn_search_promo" tabindex="4">
                                                    <span class="fa fa-search"></span>
                                                </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required" for="entityId">Entity</label>
                                    <div class="col-lg-8" id="dynamicElEntity">
                                        <select class="form-select form-select-sm" data-control="select2" name="entityId" id="entityId" data-placeholder="Select an Entity"  data-allow-clear="true" tabindex="5">
                                            <option></option>
                                        </select>
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
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label required" for="activityDesc">Description </label>
                                    <div class="col-lg-8">
                                        <textarea type="text" class="form-control form-control-sm" id="activityDesc" name="activityDesc" rows="3" autocomplete="off" tabindex="6"></textarea>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label" for="feeDesc">Fee Description </label>
                                    <div class="col-lg-8">
                                        <textarea type="text" class="form-control form-control-sm" id="feeDesc" name="feeDesc" rows="3" autocomplete="off" tabindex="7"></textarea>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="whtType">WHT Type </label>
                                    <div class="col-lg-6">
                                        <div class="row">
                                            <div class="d-flex flex-row align-items-center">
                                                <select class="form-select form-select-sm" data-control="select2" name="whtType" id="whtType" data-placeholder="Select a WHT Type"  data-allow-clear="true" tabindex="8">
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
                                        <select class="form-select form-select-sm" data-control="select2" name="taxLevel" id="taxLevel" data-placeholder="Select a Tax Level"  data-allow-clear="true" tabindex="9">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="deductionDate">Deduction Date </label>
                                    <div class="col-lg-6">
                                        <input type="text" class="form-control form-control-sm" name="deductionDate" id="deductionDate" value="{{ @date('Y-m-d') }}" autocomplete="off" tabindex="10">
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="memDocNo">Memorial Doc. No </label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm" type="text" name="memDocNo" id="memDocNo" autocomplete="off" tabindex="11"/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="intDocNo">Internal Doc. No </label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm" type="text" name="intDocNo" id="intDocNo" autocomplete="off" tabindex="12"/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="periode">Period </label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="periode" id="periode" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="dnAmount">DN Amount </label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm text-end" type="text" value="0" inputmode="decimal" onClick="this.select();" name="dnAmount" id="dnAmount" autocomplete="off" tabindex="13"/>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="feeAmount">Fee </label>
                                    <div class="col-lg-6">
                                        <div class="input-group input-group-sm">
                                            <div class="col-3">
                                                <input type="text" class="form-control form-control-sm text-end mask-four-digits" value="0" inputmode="decimal" onClick="this.select();" name="feePct" id="feePct"  autocomplete="off" />
                                            </div>
                                            <span class="input-group-text px-5">%</span>
                                            <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" value="0" name="feeAmount" id="feeAmount" autocomplete="off" readonly/>
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
                                        <select class="form-select form-select-sm" data-control="select2" name="statusPPN" id="statusPPN" data-placeholder="Select PPN" autocomplete="off" data-allow-clear="true">
                                            <option></option>
                                            <option value="PPN DN AMOUNT">PPN - DN Amount</option>
                                            <option value="PPN FEE">PPN - Fee</option>
                                            <option value="PPN DPP">PPN - DPP</option>
                                        </select>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="input-group input-group-sm">
                                            <div class="col-3">
                                                <input type="numeric" class="form-control form-control-sm text-end" value="0" inputmode="decimal" onClick="this.select();" name="ppnPct" id="ppnPct" value="0" autocomplete="off"/>
                                            </div>
                                            <span class="input-group-text px-5">%</span>
                                            <input class="form-control form-control-sm form-control-solid-bg text-end" value="0" type="text" name="ppnAmount" id="ppnAmount" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4 offset-lg-2">
                                        <select class="form-select form-select-sm" data-control="select2" name="statusPPH" id="statusPPH" data-placeholder="Select PPH" autocomplete="off" data-allow-clear="true">
                                            <option></option>
                                            <option value="DN Amount PPH">PPH - DN Amount</option>
                                            <option value="FEE PPH">PPH - Fee</option>
                                            <option value="DPP PPH">PPH - DPP</option>
                                        </select>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="input-group input-group-sm">
                                            <div class="col-3">
                                                <input type="text" class="form-control form-control-sm text-end" value="0" inputmode="decimal" onClick="this.select();" name="pphPct" id="pphPct" autocomplete="off"/>
                                            </div>
                                            <span class="input-group-text px-5">%</span>
                                            <input class="form-control form-control-sm form-control-solid-bg text-end" value="0" type="text" name="pphAmount" id="pphAmount" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-6 offset-lg-6">
                                        <div class="separator border-3 my-2 border-secondary"></div>
                                    </div>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label" for="total">Total </label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm form-control-solid-bg text-end fw-bold fs-3" value="0" type="text" name="total" id="total" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label" for="fpNumber">FP Number </label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm" type="text" name="fpNumber" id="fpNumber" autocomplete="off" tabindex="14"/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-form-label" for="fpDate">FP Date </label>
                                    <div class="col-lg-6">
                                        <input class="form-control form-control-sm" type="text" name="fpDate" id="fpDate" autocomplete="off" tabindex="15"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>
                    <div class="row mt-3">
                        <label class="col-lg-4 col-form-label">Upload File </label>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex align-items-center">
                                    <label class="col-form-label mt-1 me-5">1.</label>
                                    <div class="input-group input-group-sm w-70 custom-file-button">
                                        <label class="input-group-text fs-12px" for="attachment1" role="button">Browse...</label>
                                        <label for="attachment1" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_1" role="button">Choose File</label>
                                        <input type="file" class="d-none input_file" id="attachment1" name="attachment1" data-row="1">
                                    </div>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_delete" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Delete" value="1">
                                        <span class="fa fa-trash-alt text-white"> </span>
                                    </button>
                                    <div>
                                        <span class="badge badge-circle badge-success d-none ms-2" id="info1"><i class="fa fa-check text-white"></i></span>
                                    </div>
                                    <div class="pe-11" id="offset1"></div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center">
                                    <label class="col-form-label mt-1 me-5">2.</label>
                                    <div class="input-group input-group-sm w-70 custom-file-button">
                                        <label class="input-group-text fs-12px" for="attachment2" role="button">Browse...</label>
                                        <label for="attachment2" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_2" role="button">Choose File</label>
                                        <input type="file" class="d-none input_file" id="attachment2" name="attachment2" data-row="2">
                                    </div>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_delete" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Delete" value="2">
                                        <span class="fa fa-trash-alt text-white"> </span>
                                    </button>
                                    <div>
                                        <span class="badge badge-circle badge-success d-none ms-2" id="info2"><i class="fa fa-check text-white"></i></span>
                                    </div>
                                    <div class="pe-11" id="offset2"></div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center">
                                    <label class="col-form-label mt-1 me-5">3.</label>
                                    <div class="input-group input-group-sm w-70 custom-file-button">
                                        <label class="input-group-text fs-12px" for="attachment3" role="button">Browse...</label>
                                        <label for="attachment3" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_3" role="button">Choose File</label>
                                        <input type="file" class="d-none input_file" id="attachment3" name="attachment3" data-row="3">
                                    </div>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_delete" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Delete" value="3">
                                        <span class="fa fa-trash-alt text-white"> </span>
                                    </button>
                                    <div>
                                        <span class="badge badge-circle badge-success d-none ms-2" id="info3"><i class="fa fa-check text-white"></i></span>
                                    </div>
                                    <div class="pe-11" id="offset3"></div>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex align-items-center">
                                    <label class="col-form-label mt-1 me-5">4.</label>
                                    <div class="input-group input-group-sm w-70 custom-file-button">
                                        <label class="input-group-text fs-12px" for="attachment4" role="button">Browse...</label>
                                        <label for="attachment4" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_4" role="button">Choose File</label>
                                        <input type="file" class="d-none input_file" id="attachment4" name="attachment4" data-row="4">
                                    </div>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_delete" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Delete" value="4">
                                        <span class="fa fa-trash-alt text-white"> </span>
                                    </button>
                                    <div>
                                        <span class="badge badge-circle badge-success d-none ms-2" id="info4"><i class="fa fa-check text-white"></i></span>
                                    </div>
                                    <div class="pe-11" id="offset4"></div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center">
                                    <label class="col-form-label mt-1 me-5">5.</label>
                                    <div class="input-group input-group-sm w-70 custom-file-button">
                                        <label class="input-group-text fs-12px" for="attachment5" role="button">Browse...</label>
                                        <label for="attachment5" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_5" role="button">Choose File</label>
                                        <input type="file" class="d-none input_file" id="attachment5" name="attachment5" data-row="5">
                                    </div>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_delete" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Delete" value="5">
                                        <span class="fa fa-trash-alt text-white"> </span>
                                    </button>
                                    <div>
                                        <span class="badge badge-circle badge-success d-none ms-2" id="info5"><i class="fa fa-check text-white"></i></span>
                                    </div>
                                    <div class="pe-11" id="offset5"></div>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center">
                                    <label class="col-form-label mt-1 me-5">6.</label>
                                    <div class="input-group input-group-sm w-70 custom-file-button">
                                        <label class="input-group-text fs-12px" for="attachment6" role="button">Browse...</label>
                                        <label for="attachment6" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_6" role="button">Choose File</label>
                                        <input type="file" class="d-none input_file" id="attachment6" name="attachment6" data-row="6">
                                    </div>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_delete" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Delete" value="6">
                                        <span class="fa fa-trash-alt text-white"> </span>
                                    </button>
                                    <div>
                                        <span class="badge badge-circle badge-success d-none ms-2" id="info6"><i class="fa fa-check text-white"></i></span>
                                    </div>
                                    <div class="pe-11" id="offset6"></div>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex align-items-center">
                                    <label class="col-form-label mt-1 me-5" for="attachment7">7.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment7" id="attachment7" data-row="7" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center">
                                    <label class="col-form-label mt-1 me-5" for="attachment8">8.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment8" id="attachment8" data-row="8" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex align-items-center">
                                    <label class="col-form-label mt-1 me-5" for="attachment9">9.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment9" id="attachment9" data-row="9" autocomplete="off" readonly/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row d-none" id="download-zip">
                        <div class="col-lg-4 col-md-4 col-sm-4 col-4">
                            <div class="row">
                                <div class="d-flex">
                                    <label class="col-form-label">Download ALL (ZIP)</label>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download ms-3 w-45px" id="download_all" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download" value="all">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                    <div class="pe-5"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    @include('DebitNote::dn-creation.promo-list')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/debit-note/dn-creation/js/dn-creation-form.js?v=28') }}"></script>
    <script src="{{ asset('assets/pages/debit-note/dn-creation/js/promo-list.js?v=5') }}"></script>
@endsection

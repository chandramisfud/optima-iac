@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/report/dn-upload/css/dn-upload-form.css?v=1') }}" rel="stylesheet" type="text/css"/>
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
                        <form id="form_cancel" class="form" autocomplete="off">
                            @csrf
                            <div class="row mb-2">
                                <div class="col-lg-6">
                                    <small class="fs-5 fw-bold mt-1 ms-1">Notes</small>
                                </div>
                            </div>
                            <div class="row fv-row mb-2">
                                <div class="col-lg-6 mb-2">
                                    <textarea type="text" class="form-control form-control-sm" id="notes" name="notes" rows="3" autocomplete="off"></textarea>
                                </div>
                                <div class="col-lg-6">
                                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_cancel">
                                    <span class="indicator-label">
                                    <span class="la la-check"></span> Cancel
                                    </span>
                                        <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                    </button>
                                </div>
                            </div>
                        </form>
                        <div class="row">
                            <div class="col-lg-12 offset-lg-12">
                                <div class="separator border-3 my-2 border-secondary"></div>
                            </div>
                            <small class="fs-5 fw-bold mt-1 ms-1">Detail</small>
                            <div class="col-lg-12 offset-lg-12">
                                <div class="separator border-3 my-2 border-secondary"></div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Year </label>
                                <div class="col-lg-4">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="year" id="year" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Sub Account </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="accountDesc" id="accountDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Selling Point </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="sellingPoint" id="sellingPoint" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Promo ID </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="promoRefId" id="promoRefId" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Entity </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="entityLongDesc" id="entityLongDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Address </label>
                                <div class="col-lg-8">
                                    <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="entityAddress" name="entityAddress" rows="3" autocomplete="off" readonly></textarea>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Up. </label>
                                <div class="col-lg-8">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="entityUp" id="entityUp" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Description </label>
                                <div class="col-lg-8">
                                    <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="activityDesc" name="activityDesc" rows="3" autocomplete="off" readonly></textarea>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">Fee Description </label>
                                <div class="col-lg-8">
                                    <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="feeDesc" name="feeDesc" rows="3" autocomplete="off" readonly></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">WHT Type </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="whtType" id="whtType" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Tax Level </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="taxLevel" id="taxLevel" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Deduction Date </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="deductionDate" id="deductionDate" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Memorial Doc. No </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="memDocNo" id="memDocNo" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Internal Doc. No </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="intDocNo" id="intDocNo" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Period </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="period" id="period" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">DN Amount </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="dnAmount" id="dnAmount" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 offset-lg-2 col-form-label">Fee </label>
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
                                <label class="col-lg-4 offset-lg-2 col-form-label">DPP </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg mb-1 mb-lg-0 text-end" type="text" name="dpp" id="dpp" autocomplete="off" readonly/>
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
                                    <input class="form-control form-control-sm form-control-solid-bg mb-1 mb-lg-0" type="text" name="statusPPH" id="statusPPH" autocomplete="off" readonly/>
                                </div>
                                <div class="col-lg-6">
                                    <div class="input-group input-group-sm">
                                        <div class="col-3">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" name="pphPct" id="pphPct" readonly/>
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
                                    <input class="form-control form-control-sm form-control-solid-bg text-end fw-bold fs-3" type="text" name="total" id="total" autocomplete="off" readonly/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                            <div class="row">
                                <label class="col-lg-4 col-form-label">FP Number </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="fpNumber" id="fpNumber" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-form-label">FP Date </label>
                                <div class="col-lg-6">
                                    <input class="form-control form-control-sm form-control-solid-bg" type="text" name="fpDate" id="fpDate" autocomplete="off" readonly/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row mt-3">
                        <label class="col-lg-4 col-form-label">Upload File </label>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">1.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment1" id="attachment1" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download" value="1">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">2.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment2" id="attachment2" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download" value="2">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">3.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment3" id="attachment3" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download" value="3">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">4.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment4" id="attachment4" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download" value="4">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">5.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment5" id="attachment5" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download" value="5">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 me-5">6.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment6" id="attachment6" autocomplete="off" readonly/>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download" value="6">
                                        <span class="fa fa-download text-white"> </span>
                                    </button>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex">
                                    <label class="col-form-label mt-1 me-5">7.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-lg-3 flex-grow-1" type="text" name="7" id="attachment7" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex">
                                    <label class="col-form-label mt-1 me-5">8.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-lg-3 flex-grow-1" type="text" name="attachment8" id="attachment8" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row mb-3">
                                <div class="d-flex">
                                    <label class="col-form-label mt-1 me-5">9.</label>
                                    <input class="form-control form-control-sm form-control-solid-bg me-lg-3 flex-grow-1" type="text" name="attachment9" id="attachment9" autocomplete="off" readonly/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                            <div class="row mb-3">
                                <div class="d-flex w-lg-85">
                                    <label class="col-form-label mt-1 ms-8 w-100">Download ALL (ZIP)</label>
                                    <button class="btn btn-sm btn-optima flex-shrink-0 btn_download" id="download_all" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Download" value="all">
                                        <span class="fa fa-download text-white"> </span>
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
    <script src="{{ asset('assets/pages/debit-note/dn-upload/js/dn-upload-cancel.js?v=' .microtime()) }}"></script>
@endsection

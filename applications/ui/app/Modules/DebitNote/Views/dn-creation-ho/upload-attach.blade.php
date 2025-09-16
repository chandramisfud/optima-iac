@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/debit-note/dn-creation-ho/css/dn-creation-ho-form.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method"></small>
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
                    <div class="d-flex align-items-center rounded d-none py-3 px-3 mb-5" id="text-warning" style="background-color: darkblue">
                        <span class="la la-warning" style="color: white; font-size: xx-large"></span>
                        <div class="col-lg-12 col-md-12 col-sm-12 col-6 ms-5">
                            <div class="badge badge-square" role="alert">
                                <div class="alert-text">
                                    <h3 class="text-white align-center">
                                        Unthick Trade Promo for DN Non Trade Promo and select at least 1 Sub Account
                                    </h3>
                                </div>
                            </div>
                        </div>
                    </div>
                    <form id="form_dn" class="form" autocomplete="off">
                        @csrf
                        <div class="row">
                            <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-form-label">Year </label>
                                    <div class="col-lg-4">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="period" id="period" autocomplete="off" readonly/>
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
                                <div class="row d-none" id="trade-promo">
                                    <label class="col-lg-10 offset-lg-2 col-form-label">
                                        <div class="form-check">
                                            <input class="form-check-input" type="checkbox" value="1" id="isDNPromo" name="isDNPromo" checked />
                                            <label class="form-check-label" for="flexCheckChecked">
                                                Trade Promo
                                            </label>
                                        </div>
                                    </label>
                                </div>
                                <div class="row">
                                    <label class="col-lg-4 offset-lg-2 col-form-label">WHT Type</label>
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
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="periode" id="periode" autocomplete="off" readonly/>
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
                                            <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="pphAmount" id="pphAmount" autocomplete="off" readonly/>
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
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">1.</label>
                                        <div class="input-group input-group-sm w-70 custom-file-button">
                                            <label class="input-group-text fs-12px" for="attachment1" role="button">Browse...</label>
                                            <label for="attachment1" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_1" role="button">Choose File</label>
                                            <input type="file" class="d-none input_file" id="attachment1" data-row="1">
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
                                            <input type="file" class="d-none input_file" id="attachment2" data-row="2">
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
                                            <input type="file" class="d-none input_file" id="attachment3" data-row="3">
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
                                            <input type="file" class="d-none input_file" id="attachment4" data-row="4">
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
                                            <input type="file" class="d-none input_file" id="attachment5" data-row="5">
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
                                            <input type="file" class="d-none input_file" id="attachment6" data-row="6">
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
                                        <label class="col-form-label mt-1 me-5">7.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment7" id="attachment7" data-row="7" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">8.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment8" id="attachment8" data-row="8" autocomplete="off" readonly/>
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <div class="d-flex align-items-center">
                                        <label class="col-form-label mt-1 me-5">9.</label>
                                        <input class="form-control form-control-sm form-control-solid-bg me-3 flex-grow-1" type="text" name="attachment9" id="attachment9" data-row="9" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row d-none">
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
                    </form>
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
    <script src="{{ asset('assets/pages/debit-note/dn-creation-ho/js/dn-creation-ho-upload-attach.js?v=4') }}"></script>
@endsection

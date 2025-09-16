@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/skp-validation/css/dc-skp-validation-form.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold my-1 ms-1" id="txt_info_method">View </small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back-submit')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-5">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm mb-3 card_form">
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-12">
                            <h5 class="fw-normal">SKP Validation Status</h5>
                        </div>
                    </div>
                    <form id="form_skp_validation" class="form" autocomplete="off">
                        @csrf
                        <div class="row mb-3">
                            <div class="col-lg-6 col-md-12 col-12">
                                <div class="row fv-row mb-1">
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="skpStatus" id="skpStatus" data-placeholder="Select a SKP Status"  data-allow-clear="true">
                                            <option value="0">New</option>
                                            <option value="1">Pending</option>
                                            <option value="2">Final</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row mb-1">
                                    <label class="col-lg-12 col-form-label" for="skpNotes">Remarks</label>
                                    <div class="col-lg-12">
                                        <textarea type="text" class="form-control form-control-sm" id="skpNotes" name="skpNotes" rows="3" autocomplete="off"></textarea>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 border">
                                <h6>Draft</h6>
                                <div class="row">
                                    <div class="col-md-8 col-sm-12">
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_skpDraftAvail" name="lbl_skpDraftAvail">
                                                <input class="form-check-input me-2" type="checkbox" id="skpDraftAvail" name="skpDraftAvail" readonly="readonly"/> SKP Draft Availability
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_skpDraftAvailBfrAct60" name="lbl_skpDraftAvailBfrAct60">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="skpDraftAvailBfrAct60" name="skpDraftAvailBfrAct60" readonly="readonly"/> SKP Draft Availability Before Activity Start H-30
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_entityDraftMatch" name="lbl_entityDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="entityDraftMatch" name="entityDraftMatch" readonly="readonly"/> Entity
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_brandDraftMatch" name="lbl_brandDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="brandDraftMatch" name="brandDraftMatch" readonly="readonly"/> SKU
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_periodDraftMatch" name="lbl_periodDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="periodDraftMatch" name="periodDraftMatch" readonly="readonly"/> Period
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_activityDescDraftMatch" name="lbl_activityDescDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="activityDescDraftMatch" name="activityDescDraftMatch" readonly="readonly"/> Activity Desc
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-12">
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_mechanismDraftMatch" name="lbl_mechanismDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="mechanismDraftMatch" name="mechanismDraftMatch" readonly="readonly"/> Mechanism
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_investmentDraftMatch" name="lbl_investmentDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="investmentDraftMatch" name="investmentDraftMatch" readonly="readonly"/> Investment
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_distributorDraft" name="lbl_distributorDraft">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="distributorDraftMatch" name="distributorDraftMatch" readonly="readonly"/> Distributor
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_channelDraft" name="lbl_channelDraft">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="channelDraftMatch" name="channelDraftMatch" readonly="readonly"/> Channel
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_storeNameDraft" name="lbl_storeNameDraft">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="storeNameDraftMatch" name="storeNameDraftMatch" readonly="readonly"/> Store Name
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 border">
                                <h6>Final</h6>
                                <div class="row">
                                    <div class="col-md-8 col-sm-12 kt-margin-b-10-tablet-and-mobile">
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_entityMatch" name="lbl_entityMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="entityMatch" name="entityMatch" readonly="readonly"/> Entity
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_brandMatch" name="lbl_brandMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="brandMatch" name="brandMatch" readonly="readonly"/> SKU
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_periodMatch" name="lbl_periodMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="periodMatch" name="periodMatch" readonly="readonly"/> Period
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_activityDescMatch" name="lbl_activityDescMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="activityDescMatch" name="activityDescMatch" readonly="readonly"/> Activity Desc
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_mechanismMatch" name="lbl_mechanismMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="mechanismMatch" name="mechanismMatch" readonly="readonly"/> Mechanism
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-12 kt-margin-b-10-tablet-and-mobile">
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_investmentMatch" name="lbl_investmentMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="investmentMatch" name="investmentMatch" readonly="readonly"/> Investment
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_skpSign7" name="lbl_skpSign7">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="skpSign7Match" name="skpSign7Match" readonly="readonly"/> SKP Signed H-7
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_distributor" name="lbl_distributor">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="distributorMatch" name="distributorMatch" readonly="readonly"/> Distributor
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_channel" name="lbl_channel">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="channelMatch" name="channelMatch" readonly="readonly"/> Channel
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_storeName" name="lbl_storeName">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="storeNameMatch" name="storeNameMatch" readonly="readonly"/> Store Name
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </form>
                </div>
            </div>

            <div class="card shadow-sm mb-3 card_detail">
                <div class="card-body">
                    <div class="d-flex py-1 flex-grow-1 justify-content-between">
                        <span class="fw-bold text-gray-700 fs-4">Detail</span>
                    </div>
                    <div class="separator border-2 my-1 border-secondary"></div>
                    <div class="row">
                        <div class="col-lg-12 col-12">
                            <div class="row">
                                <div class="col-lg-6 col-12">
                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="period">Budget Year</label>
                                        <div class="col-lg-2 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm mb-2" name="period" id="period" autocomplete="off" tabindex="0" readonly>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-12">
                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entity">Entity</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm mb-2" name="entity" id="entity" autocomplete="off" tabindex="1" readonly>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="brand">Brand</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm mb-2" name="brand" id="brand" autocomplete="off" tabindex="2" readonly>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="distributor">Distributor</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm mb-2" name="distributor" id="distributor" autocomplete="off" tabindex="3" readonly>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subCategory">Sub Activity Type</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm mb-2" name="subCategory" id="subCategory" autocomplete="off" tabindex="4" readonly>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivity">Sub Activity</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm mb-2" name="subActivity" id="subActivity" autocomplete="off" tabindex="5" readonly>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="channel">Channel</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm mb-2" name="channel" id="channel" autocomplete="off" tabindex="6" readonly>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationRefId">Budget Source</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm mb-2" name="allocationRefId" id="allocationRefId" autocomplete="off" tabindex="7" readonly>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="startPromo">Activity Period</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                            <div class="input-group input-group-sm">
                                                <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromo" id="startPromo" autocomplete="off" tabindex="8" readonly>
                                                <span class="input-group-text">to</span>
                                                <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromo" id="endPromo" autocomplete="off" tabindex="9" readonly>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="mechanism1">Mechanism</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm mb-2" name="mechanism1" id="mechanism1" tabindex="10" readonly>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="investment">Investment</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                            <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="investment" id="investment" autocomplete="off" value="0" inputmode="decimal" onClick="this.select();" tabindex="11" readonly/>
                                        </div>
                                    </div>

                                    <div class="row fv-row">
                                        <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotes">Initiator Notes</label>
                                        <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                            <input type="text" class="form-control form-control-sm" data-kt-dialer-control="input" name="initiatorNotes" id="initiatorNotes" autocomplete="off" tabindex="12" readonly/>
                                        </div>
                                    </div>
                                </div>


                                <div class="col-lg-6 col-12">
                                    <div class="row fv-row">
                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationDesc">Budget Allocation</label>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                            <input class="form-control form-control-sm form-control-solid-bg" type="text" name="allocationDesc" id="allocationDesc" autocomplete="off" readonly />
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetDeployed">Budget Deployed</label>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                            <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetDeployed" id="budgetDeployed" autocomplete="off" inputmode="decimal" readonly />
                                        </div>
                                    </div>
                                    <div class="row fv-row">
                                        <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetRemaining">Remaining Budget</label>
                                        <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                            <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetRemaining" id="budgetRemaining" autocomplete="off" inputmode="decimal" readonly />
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

                                                <div class="row mb-3">
                                                    <div class="offset-lg-2 col-lg-10 col-12">
                                                        <div class="row fv-row">
                                                            <div class="d-flex align-items-center">
                                                                <label class="col-form-label me-5" for="review_file_label_1">1.</label>
                                                                <input type="text" class="form-control form-control-sm" id="review_file_label_1" readonly/>
                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download1" name="btn_download1" value="1" disabled>
                                                                    <span class="fa fa-download"></span>
                                                                </button>
                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view1" name="btn_view1" value="1" disabled>
                                                                    <span class="fa fa-eye"> </span>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row mb-3">
                                                    <div class="offset-lg-2 col-lg-10 col-12">
                                                        <div class="row fv-row">
                                                            <div class="d-flex align-items-center">
                                                                <label class="col-form-label me-5" for="review_file_label_2">2.</label>
                                                                <input type="text" class="form-control form-control-sm" id="review_file_label_2" readonly/>
                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download2" name="btn_download2" value="2" disabled>
                                                                    <span class="fa fa-download"></span>
                                                                </button>
                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view2" name="btn_view2" value="2" disabled>
                                                                    <span class="fa fa-eye"> </span>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row mb-3">
                                                    <div class="offset-lg-2 col-lg-10 col-12">
                                                        <div class="row fv-row">
                                                            <div class="d-flex align-items-center">
                                                                <label class="col-form-label me-5" for="review_file_label_3">3.</label>
                                                                <input type="text" class="form-control form-control-sm" id="review_file_label_3" readonly/>
                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download3" name="btn_download3" value="3" disabled>
                                                                    <span class="fa fa-download"></span>
                                                                </button>
                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view3" name="btn_view3" value="3" disabled>
                                                                    <span class="fa fa-eye"> </span>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="offset-lg-2 col-lg-10 col-12">
                                                        <div class="row fv-row">
                                                            <div class="d-flex align-items-center">
                                                                <label class="col-form-label me-5" for="review_file_label_4">4.</label>
                                                                <input type="text" class="form-control form-control-sm" id="review_file_label_4" readonly/>
                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download4" name="btn_download4" value="4" disabled>
                                                                    <span class="fa fa-download"></span>
                                                                </button>
                                                                <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view4" name="btn_view4" value="4" disabled>
                                                                    <span class="fa fa-eye"> </span>
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
        </div>
    </div>
@endsection

@section('vendor-script')

@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/skp-validation/js/dc-skp-validation-form.js?v=1') }}"></script>
@endsection


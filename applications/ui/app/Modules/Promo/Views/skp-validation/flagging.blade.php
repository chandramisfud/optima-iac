@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/skp-validation/css/skp-validation-form.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold my-1 ms-1" id="txt_info_method">View </small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-5 card_form">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm mb-3">
                <div class="card-body">

                    <form id="form_skp_flagging" class="form" autocomplete="off">
                        @csrf
                        <div class="row mb-3">
                            <div class="col-12">
                                <h5 class="fw-normal">SKP Validate</h5>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 border">
                                <div class="d-flex flex-lg-row flex-column align-items-lg-center mt-5">
                                    <h6>Draft</h6>
                                    <div class="form-check ms-2">
                                        <input class="form-check-input" type="checkbox" value="1" id="draft_all" name="draft_all" />
                                        <label class="form-check-label" for="flexCheckChecked">
                                            All
                                        </label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8 col-sm-12">
                                        <div class="row form-check mb-2">
                                            <label class="form-check-label" id="lbl_skpDraftAvail" name="lbl_skpDraftAvail">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="skpDraftAvail_preview" name="skpDraftAvail_preview"/> SKP Draft Availability
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_skpDraftAvailBfrAct60" name="lbl_skpDraftAvailBfrAct60">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="skpDraftAvailBfrAct60_preview" name="skpDraftAvailBfrAct60_preview"/> SKP Draft Availability Before Activity Start H-30
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_entityDraftMatch" name="lbl_entityDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="entityDraftMatch_preview" name="entityDraftMatch_preview"/> Entity
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_brandDraftMatch" name="lbl_brandDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="brandDraftMatch_preview" name="brandDraftMatch_preview"/> SKU
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_periodDraftMatch" name="lbl_periodDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="periodDraftMatch_preview" name="periodDraftMatch_preview"/> Period
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_activityDescDraftMatch" name="lbl_activityDescDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="activityDescDraftMatch_preview" name="activityDescDraftMatch_preview"/> Activity Desc
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-12">
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_mechanismDraftMatch" name="lbl_mechanismDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="mechanismDraftMatch_preview" name="mechanismDraftMatch_preview"/> Mechanism
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_investmentDraftMatch" name="lbl_investmentDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="investmentDraftMatch_preview" name="investmentDraftMatch_preview"/> Investment
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_distributorDraft" name="lbl_distributorDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="distributorDraftMatch_preview" name="distributorDraftMatch_preview"/> Distributor
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_channelDraft" name="lbl_channelDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="channelDraftMatch_preview" name="channelDraftMatch_preview"/> Channel
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_storeNameDraft" name="lbl_storeNameDraftMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="storeNameDraftMatch_preview" name="storeNameDraftMatch_preview"/> Store Name
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 border">
                                <div class="d-flex flex-lg-row flex-column align-items-lg-center mt-5">
                                    <h6>Final</h6>
                                    <div class="form-check ms-2">
                                        <input class="form-check-input" type="checkbox" value="1" id="final_all" name="final_all" />
                                        <label class="form-check-label" for="flexCheckChecked">
                                            All
                                        </label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8 col-sm-12 kt-margin-b-10-tablet-and-mobile">
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_entityMatch" name="lbl_entityMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="entityMatch_preview" name="entityMatch_preview"/> Entity
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_brandMatch" name="lbl_brandMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="brandMatch_preview" name="brandMatch_preview"/> SKU
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_periodMatch" name="lbl_periodMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="periodMatch_preview" name="periodMatch_preview"/> Period
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_activityDescMatch" name="lbl_activityDescMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="activityDescMatch_preview" name="activityDescMatch_preview"/> Activity Desc
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_mechanismMatch" name="lbl_mechanismMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="mechanismMatch_preview" name="mechanismMatch_preview"/> Mechanism
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-12 kt-margin-b-10-tablet-and-mobile">
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_investmentMatch" name="lbl_investmentMatch">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="investmentMatch_preview" name="investmentMatch_preview"/> Investment
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_skpSign7" name="lbl_skpSign7">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="skpSign7Match_preview" name="skpSign7_preview"/> SKP Signed H-7
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_distributor" name="lbl_distributor">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="distributorMatch_preview" name="distributor_preview"/> Distributor
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_channel" name="lbl_channel">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="channelMatch_preview" name="channel_preview"/> Channel
                                            </label>
                                        </div>
                                        <div class="row form-check mb-2">
                                            <label class=" form-check-label" id="lbl_storeName" name="lbl_storeName">
                                                <input class="form-check-input cursor-pointer me-2" type="checkbox" id="storeNameMatch_preview" name="storeName_preview"/> Store Name
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <h5>File Preview</h5>
                        </div>
                        <div class="row" id="view_file">
                            <iframe class="frame-file" src="" width="100%" height="400px" id="iframe_file" name="iframe_file"></iframe>
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
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/skp-validation/js/flagging-form.js?v=1') }}"></script>
@endsection


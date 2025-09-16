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
                        <div class="col-lg-6 col-12">
                            <div class="row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="period">Budget Year</label>
                                <div class="col-lg-2 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" data-kt-dialer-control="input" name="period" id="period" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="promoPlanRefId">Promo Planning</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" id="promoPlanRefId" name="promoPlanRefId" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationRefId">Budget Source</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" id="allocationRefId" name="allocationRefId" autocomplete="off" readonly />
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationDesc">Budget Allocation</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" id="allocationDesc" name="allocationDesc" autocomplete="off" readonly />
                                </div>
                            </div>
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="activity">Activity</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" id="activity" name="activity" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityDesc">Sub Activity</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" id="subActivityDesc" name="subActivityDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="startPromo">Activity Period</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" name="startPromo" id="startPromo" autocomplete="off" readonly/>
                                        <span class="input-group-text">to</span>
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg" name="endPromo" id="endPromo" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="activityDesc">Activity Name</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <textarea maxlength="255" class="form-control form-control-sm" name="activityDesc" id="activityDesc" autocomplete="off" tabindex="3" readonly></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-12">
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="tsCoding">TS Code</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" id="tsCoding" name="tsCoding" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subCategoryDesc">Sub Category</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" id="subCategoryDesc" name="subCategoryDesc" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entity">Entity</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" id="entity" name="entity" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="distributorName">Distributor</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg" id="distributorName" name="distributorName" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetAmount">Budget Deployed</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="budgetAmount" name="budgetAmount" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="remainingBudget">Remaining Budget</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="remainingBudget" name="remainingBudget" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row fv-row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotes">Initiator Notes</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                    <textarea maxlength="255" class="form-control form-control-sm" name="initiatorNotes" id="initiatorNotes" autocomplete="off" tabindex="4" readonly></textarea>
                                </div>
                            </div>
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="investmentType">Investment Type</label>
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
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="baselineSales">Baseline Sales</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="baselineSales" id="baselineSales" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                    </div>
                                </div>
                            </div>
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="incrementSales">Sales Increment</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm text-end" id="incrementSales" name="incrementSales" autocomplete="off" value="0" tabindex="6" readonly/>
                                </div>
                            </div>
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalSales">Total Sales</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="totalSales" name="totalSales" autocomplete="off" readonly/>
                                </div>
                            </div>

                        </div>

                        <div class="col-lg-6 col-12">
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalInvestment">Total Investment</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="totalInvestment" name="totalInvestment" autocomplete="off" readonly/>
                                </div>
                            </div>
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="roi">ROI</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" id="roi" name="roi" autocomplete="off" readonly/>
                                        <span class="input-group-text" id="addon-roi">%</span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="costRatio">Cost Ratio</label>
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
                                        <label class="col-form-label col-lg-12 col-12 pb-1" for="channel">Channel</label>
                                        <div class="col-lg-12 col-12">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="channel" name="channel" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-12">
                                    <div class="row fv-row">
                                        <label class="col-form-label col-lg-12 col-12 pb-1" for="subChannel">Sub Channel</label>
                                        <div class="col-lg-12 col-12">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="subChannel" name="subChannel" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-12">
                                    <div class="row fv-row">
                                        <label class="col-form-label col-lg-12 col-12 pb-1" for="account">Account</label>
                                        <div class="col-lg-12 col-12">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="account" name="account" autocomplete="off" readonly/>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-12">
                                    <div class="row fv-row">
                                        <label class="col-form-label col-lg-12 col-12 pb-1" for="subAccount">Sub Account</label>
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
                                <div class="col-lg-5 col-md-12 col-sm-12 col-12">
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_1">1.</label>
                                            <input type="text" class="form-control form-control-sm" id="review_file_label_1" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download1" name="btn_download1" value="1" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view1" name="btn_view1" value="1" disabled>
                                                <span class="fa fa-eye"> </span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_2">2.</label>
                                            <input type="text" class="form-control form-control-sm" id="review_file_label_2" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download2" name="btn_download2" value="2" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view2" name="btn_view2" value="2" disabled>
                                                <span class="fa fa-eye"> </span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_3">3.</label>
                                            <input type="text" class="form-control form-control-sm" id="review_file_label_3" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download3" name="btn_download3" value="3" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view3" name="btn_view3" value="3" disabled>
                                                <span class="fa fa-eye"> </span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_4">4.</label>
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

                                <div class="col-lg-5 offset-lg-1 col-md-12 col-sm-12 col-12">
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_5">5.</label>
                                            <input type="text" class="form-control form-control-sm" id="review_file_label_5" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download5" name="btn_download5" value="5" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view5" name="btn_view5" value="5" disabled>
                                                <span class="fa fa-eye"> </span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_6">6.</label>
                                            <input type="text" class="form-control form-control-sm" id="review_file_label_6" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download6" name="btn_download6" value="6" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view6" name="btn_view6" value="6" disabled>
                                                <span class="fa fa-eye"> </span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="d-flex align-items-center w-lg-85">
                                            <label class="me-5" for="review_file_label_7">7.</label>
                                            <input type="text" class="form-control form-control-sm" id="review_file_label_7" readonly/>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download7" name="btn_download7" value="7" disabled>
                                                <span class="fa fa-download"></span>
                                            </button>
                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_view" title="Preview" id="btn_view7" name="btn_view7" value="7" disabled>
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
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/skp-validation/js/skp-validation-form.js?v=2') }}"></script>
@endsection


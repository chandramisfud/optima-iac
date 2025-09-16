@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-approval/css/dc-promo-approval-form.css?v=' . microtime()) }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold my-1 ms-1" id="txt_info_method">Promo ID </small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-5">
        <div class="col-lg-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-12">
                            <h5 class="fw-normal">Notes</h5>
                        </div>
                    </div>
                    <form id="form_promo_approval" class="form" autocomplete="off">
                        @csrf
                        <div class="row mb-3">
                            <div class="col-lg-6 col-md-12 col-12">
                                <div class="row fv-row mb-1">
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="approvalStatusCode" id="approvalStatusCode" data-placeholder="Select Approval Action"  data-allow-clear="true">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row mb-1">
                                    <div class="col-lg-12">
                                        <textarea type="text" class="form-control form-control-sm" id="notes" name="notes" rows="3" autocomplete="off"></textarea>
                                    </div>
                                </div>
                                <div class="row fv-row mb-1">
                                    <label class="col-lg-4 col-form-label pt-0">Last Sendback Notes <br> <span id="txt_senback_notes"></span></label>
                                    <div class="col-lg-8 col-md-12 col-12">
                                        <textarea type="text" class="form-control form-control-sm form-control-solid-bg" id="sendback_notes" name="sendback_notes" rows="3" autocomplete="off" readonly></textarea>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6 col-md-12 col-12">
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_submit">
                                <span class="indicator-label">
                                    <span class="la la-check me-1"></span> Submit
                                </span>
                                    <span class="indicator-progress">Submitting...
                                    <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                </span>
                                </button>
                            </div>
                        </div>
                    </form>
                    <div class="row mb-3">
                        <div class="col-12">
                            <div class="separator border-1 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-12">
                            <h5 class="fw-normal">Detail</h5>
                            <div class="separator border-2 border-secondary"></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6 col-12">
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="period">Budget Year</label>
                                <div class="col-lg-2 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm mb-2" name="period" id="period" autocomplete="off" tabindex="0" readonly>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6 col-12">
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="entity">Entity</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm mb-2" name="entity" id="entity" autocomplete="off" tabindex="1" readonly>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="entity">Brand</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm mb-2" name="brand" id="brand" autocomplete="off" tabindex="2" readonly>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="distributor">Distributor</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm mb-2" name="distributor" id="distributor" autocomplete="off" tabindex="3" readonly>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subCategory">Sub Activity Type</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm mb-2" name="subCategory" id="subCategory" autocomplete="off" tabindex="4" readonly>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subActivity">Sub Activity</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm mb-2" name="subActivity" id="subActivity" autocomplete="off" tabindex="5" readonly>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="channel">Channel</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm mb-2" name="channel" id="channel" autocomplete="off" tabindex="6" readonly>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="allocationRefId">Budget Source</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm mb-2" name="allocationRefId" id="allocationRefId" autocomplete="off" tabindex="7" readonly>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="startPromo">Activity Period</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromo" id="startPromo" autocomplete="off" tabindex="8" readonly>
                                        <span class="input-group-text">to</span>
                                        <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromo" id="endPromo" autocomplete="off" tabindex="9" readonly>
                                    </div>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="mechanism1">Mechanism</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm mb-2" name="mechanism1" id="mechanism1" tabindex="10" readonly>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="investment">Investment</label>
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

                                        <div class="row mb-2">
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

                                        <div class="row mb-2">
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

                                        <div class="row mb-2">
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
    <button id="sticky_notes" class="explore-toggle btn btn-sm btn-icon-danger btn-light-danger shadow-sm position-fixed fw-bolder top-25 mt-10 end-0 rounded-end-0" title="Notes">
        <i class="fa fa-comments fs-3 my-3"></i>
    </button>

    <div class="modal modal-sticky-bottom-right" id="modal_notes" data-bs-backdrop="false" data-bs-focus="false" data-bs-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header py-2">
                    <div class="row">
                        <h4 class="modal-title fw-normal text-gray-800">Promo</h4>
                        <div class="mb-0 lh-1">
                            <span class="badge badge-success badge-circle w-8px h-8px me-1"></span>
                            <span class="fs-7 fw-semibold text-muted">Notes</span>
                        </div>
                    </div>
                    <div class="btn btn-icon btn-sm btn-active-light-primary ms-2" data-bs-dismiss="modal" aria-label="Close">
                        <span class="svg-icon svg-icon-1">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="blue"></rect>
                                <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="blue"></rect>
                            </svg>
                        </span>
                    </div>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="d-flex justify-content-start">
                            <div class="d-flex flex-column align-items-start">
                                <div class="p-5 rounded bg-light-primary fw-semibold mw-lg-400px text-start" data-kt-element="message-text" id="notes_message">

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
    <script src="{{ asset('assets/pages/promo/promo-approval/js/dc-promo-approval-form.js?v=9') }}"></script>
@endsection

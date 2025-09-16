@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-recon/css/dc-promo-creation-form-cancel-request.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold my-1 ms-1" id="txt_info_method">Edit </small>
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
                    <form id="form_promo_cancel_request" class="form" autocomplete="off">
                        @csrf
                        <div class="row mb-2">
                            <div class="col-lg-6">
                                <small class="fs-5 fw-bold mt-1 ms-1">Notes</small>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <div class="col-lg-6 mb-2">
                                <select class="form-select form-select-sm" data-control="select2" name="cancelReason" id="cancelReason" data-placeholder="Cancellation Reason" autocomplete="off" data-allow-clear="true">
                                    <option></option>
                                </select>
                            </div>
                            <div class="col-lg-6 mb-2">
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_cancel_request">
                                    <span class="indicator-label">
                                        <span class="la la-check"></span> Cancel Request
                                    </span>
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <div class="col-lg-6 mb-2">
                                <textarea type="text" class="form-control form-control-sm d-none" id="reason" name="reason" rows="3" autocomplete="off"></textarea>
                            </div>
                        </div>
                    </form>
                    <div class="separator border-2 my-1 border-secondary mb-5"></div>
                    <div class="d-flex py-1 flex-grow-1 justify-content-between">
                        <span class="fw-bold text-gray-700 fs-4">Detail</span>
                    </div>
                    <div class="separator border-2 my-1 border-secondary"></div>

                    <div class="row">
                        <div class="col-lg-6 col-12">
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="period">Budget Year</label>
                                <div class="col-lg-2 col-md-6 col-sm-12 col-12">
                                    <input type="text" class="form-control form-control-sm text-end" name="period" id="period" autocomplete="off" readonly/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6 col-12">
                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entity">Entity</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <input type="text" class="form-control form-control-sm" name="entity" id="entity" autocomplete="off" readonly/>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="groupBrand">Brand</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <input type="text" class="form-control form-control-sm" name="groupBrand" id="groupBrand" autocomplete="off" readonly/>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="distributor">Distributor</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <input type="text" class="form-control form-control-sm" name="distributor" id="distributor" autocomplete="off" readonly/>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subCategoryId">Sub Activity Type</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <input type="text" class="form-control form-control-sm" name="subCategory" id="subCategory" autocomplete="off" readonly/>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subActivityDesc">Sub Activity</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <input type="text" class="form-control form-control-sm" name="subActivityDesc" id="subActivityDesc" autocomplete="off" readonly/>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="channel">Channel</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <input type="text" class="form-control form-control-sm" name="channel" id="channel" autocomplete="off" readonly/>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="btn_search_budget">Budget Source</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <input type="text" class="form-control form-control-sm" name="allocationRefId" id="allocationRefId" autocomplete="off" readonly/>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="startPromo">Activity Period</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <div class="input-group input-group-sm">
                                        <input type="text" class="form-control form-control-sm" name="startPromo" id="startPromo" autocomplete="off" readonly/>
                                        <span class="input-group-text">to</span>
                                        <input type="text" class="form-control form-control-sm" name="endPromo" id="endPromo" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="mechanism1">Mechanism</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm" name="mechanism1" id="mechanism1" autocomplete="off" readonly/>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="investment">Investment</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                    <input type="text" class="form-control form-control-sm text-end" name="investment" id="investment" autocomplete="off" readonly/>
                                </div>
                            </div>

                            <div class="row fv-row">
                                <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiator_notes">Initiator Notes</label>
                                <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                    <textarea class="form-control form-control-sm mb-2" name="initiator_notes" id="initiator_notes" readonly></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-6 col-12">
                            <div class="row fv-row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="allocationDesc">Budget Allocation</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                    <input class="form-control form-control-sm" type="text" name="allocationDesc" id="allocationDesc" autocomplete="off" readonly />
                                </div>
                            </div>
                            <div class="row fv-row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetAmount">Budget Deployed</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                    <input class="form-control form-control-sm text-end" type="text" name="budgetAmount" id="budgetAmount" autocomplete="off" readonly />
                                </div>
                            </div>
                            <div class="row fv-row">
                                <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="remainingBudget">Remaining Budget</label>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                    <input class="form-control form-control-sm text-end" type="text" name="remainingBudget" id="remainingBudget" autocomplete="off" readonly />
                                </div>
                            </div>

                            <div class="row">
                                <div class="bottom-0  w-lg-50 w-100 position-lg-absolute pb-5">
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
                                                    <div class="d-flex align-items-center">
                                                        <label class="col-form-label me-5" for="review_file_label_1">1.</label>
                                                        <input type="text" class="form-control form-control-sm" id="review_file_label_1" readonly/>
                                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download1" name="btn_download1" value="1" disabled>
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
                                                        <label class="col-form-label me-5" for="review_file_label_2">2.</label>
                                                        <input type="text" class="form-control form-control-sm" id="review_file_label_2" readonly/>
                                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download2" name="btn_download2" value="2" disabled>
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
                                                        <label class="col-form-label me-5" for="review_file_label_3">3.</label>
                                                        <input type="text" class="form-control form-control-sm" id="review_file_label_3" readonly/>
                                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download3" name="btn_download3" value="3" disabled>
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
                                                        <label class="col-form-label me-5" for="review_file_label_4">4.</label>
                                                        <input type="text" class="form-control form-control-sm" id="review_file_label_4" readonly/>
                                                        <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download4" name="btn_download4" value="4" disabled>
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
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/promo-recon/js/dc-promo-recon-form-cancel-request.js?v=2') }}"></script>

@endsection


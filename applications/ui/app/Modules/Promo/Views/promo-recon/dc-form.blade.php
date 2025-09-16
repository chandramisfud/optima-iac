@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-recon/css/dc-promo-recon-form.css?v=1') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold my-1 ms-1" id="txt_info_method">Edit </small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-save-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row mb-5 card_form">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm mb-3">
                <div class="card-body">
                    <form id="form_promo" class="form" autocomplete="off">
                        @csrf
                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="period">Budget Year</label>
                                    <div class="col-lg-4 col-md-6 col-sm-12 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="period" id="period" autocomplete="off" readonly/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="entityId">Entity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="entityId" id="entityId" data-placeholder="Select an Entity" data-allow-clear="true">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="groupBrandId">Brand</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="groupBrandId" id="groupBrandId" data-placeholder="Select a Brand"  data-allow-clear="true">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="distributorId">Distributor</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="distributorId" id="distributorId" data-placeholder="Select a Distributor"  data-allow-clear="true">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="subCategoryId">Sub Activity Type</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subCategoryId" id="subCategoryId" data-placeholder="Select a Sub Activity Type"  data-allow-clear="true">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subActivityId">Sub Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subActivityId" id="subActivityId" data-placeholder="Select a Sub Activity"  data-allow-clear="true" tabindex="1">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="channelId">Channel</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12" id="dynamicElChannel">
                                        <select class="form-select form-select-sm" data-control="select2" name="channelId" id="channelId" data-placeholder="Select a Channel"  data-allow-clear="true" multiple="multiple" tabindex="2">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="btn_search_budget">Budget Source</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm has-validation">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg" id="allocationRefId" name="allocationRefId" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly="">
                                            <span class="input-group-text cursor-pointer btn-outline-optima" id="btn_search_budget" tabindex="7">
                                                <span class="fa fa-search"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="startPromo">Activity Period</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromo" id="startPromo" autocomplete="off" tabindex="3">
                                            <span class="input-group-text">to</span>
                                            <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromo" id="endPromo" autocomplete="off" tabindex="4">
                                        </div>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="mechanism1">Mechanism</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm mb-2" name="mechanism1" id="mechanism1" tabindex="5">
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="investment">Investment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="investment" id="investment" autocomplete="off" value="0" readonly/>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="actualDNClaimed">Actual DN Claimed</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="actualDNClaimed" id="actualDNClaimed" autocomplete="off" value="0" readonly/>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="investmentRecon">Final Investment Amount</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12 ">
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="investmentRecon" id="investmentRecon" autocomplete="off" value="0"/>
                                    </div>
                                </div>

                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotes">Initiator Notes</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm" type="text" name="initiatorNotes" id="initiatorNotes" tabindex="6" autocomplete="off" />
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
                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetDeployed" id="budgetDeployed" value="0" autocomplete="off" inputmode="decimal" readonly />
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="budgetRemaining">Remaining Budget</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg text-end" type="text" name="budgetRemaining" id="budgetRemaining" value="0" autocomplete="off" inputmode="decimal" readonly />
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
                                                                <label class="input-group-text fs-12px" for="attachment1" role="button">Browse...</label>
                                                                <label for="attachment1" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_1" role="button">For SKP Draft</label>
                                                                <input type="file" class="d-none input_file" id="attachment1" name="attachment1" data-row="1" tabindex="13">
                                                            </div>
                                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete1" value="1" disabled>
                                                                <span class="fa fa-trash-alt"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download1" name="btn_download1" value="1" disabled>
                                                                <span class="fa fa-download"></span>
                                                            </button>
                                                            <div class="invisible mt-2" id="info1">
                                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                            </div>
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
                                                                <label class="input-group-text fs-12px" for="attachment2" role="button">Browse...</label>
                                                                <label for="attachment2" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_2" role="button">For SKP Fully Approved</label>
                                                                <input type="file" class="d-none input_file" id="attachment2" name="attachment2" data-row="2" tabindex="14">
                                                            </div>
                                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete2" value="2" disabled>
                                                                <span class="fa fa-trash-alt"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download2" name="btn_download2" value="2" disabled>
                                                                <span class="fa fa-download"></span>
                                                            </button>
                                                            <div class="invisible mt-2" id="info2">
                                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                            </div>
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
                                                                <label class="input-group-text fs-12px" for="attachment3" role="button">Browse...</label>
                                                                <label for="attachment3" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_3" role="button"></label>
                                                                <input type="file" class="d-none input_file" id="attachment3" name="attachment3" data-row="3" tabindex="15">
                                                            </div>
                                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete3" value="3" disabled>
                                                                <span class="fa fa-trash-alt"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download3" name="btn_download3" value="3" disabled>
                                                                <span class="fa fa-download"></span>
                                                            </button>
                                                            <div class="invisible mt-2" id="info3">
                                                                <span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>
                                                            </div>
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
                                                                <label class="input-group-text fs-12px" for="attachment4" role="button">Browse...</label>
                                                                <label for="attachment4" class="form-control form-control-sm text-gray-700 fs-12px review_file_label me-3" id="review_file_label_4" role="button"></label>
                                                                <input type="file" class="d-none input_file" id="attachment4" name="attachment4" data-row="4" tabindex="16">
                                                            </div>
                                                            <button class="btn btn-sm btn-outline-optima flex-shrink-0 btn_delete" title="Delete" id="btn_delete4" value="4" disabled>
                                                                <span class="fa fa-trash-alt"> </span>
                                                            </button>
                                                            <button class="btn btn-sm btn-optima flex-shrink-0 ms-2 btn_download" title="Download" id="btn_download4" name="btn_download4" value="4" disabled>
                                                                <span class="fa fa-download"></span>
                                                            </button>
                                                            <div class="invisible mt-2" id="info4">
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
                    </form>
                </div>
            </div>
        </div>
    </div>
    @include('Promo::promo-recon.source-budget-list')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/js/generate-uuid.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/promo-recon/js/dc-promo-recon-form.js?v=20') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-recon/js/dc-source-budget-list.js?v=1') }}"></script>
@endsection

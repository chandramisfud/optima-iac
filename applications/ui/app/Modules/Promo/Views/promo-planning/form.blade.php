@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')
    <link href="{{ asset('assets/plugins/custom/datatables/datatables.bundle.css') }}" rel="stylesheet" type="text/css"/>
    <link href="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/promo/promo-planning/css/promo-planning-form.css') }}" rel="stylesheet" type="text/css"/>
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-7 fw-bold my-1 ms-1" id="txt_info_method">Add New</small>
    </span>
@endsection

@section('button-toolbar-right')
    @include('toolbars.btn-save-back')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <form id="form_promo_planning" class="form" autocomplete="off">
        @csrf
        <div class="row mb-5 card_form">
            <div class="col-md-12 col-12">
                {{-- Form Promo Plannning --}}
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="row">
                            {{-- Form Promo Plannning Left--}}
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="period">Budget Year</label>
                                    <div class="col-lg-4 ">
                                        <div class="input-group input-group-sm" id="dialer_period">
                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                <i class="fa fa-minus fs-2"></i>
                                            </button>

                                            <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="period" id="period" value="{{ @date('Y') }}" autocomplete="off" tabindex="0"/>

                                            <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                <i class="fa fa-plus fs-2"></i>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subCategoryId">Sub Category</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subCategoryId" id="subCategoryId" data-placeholder="Select a Sub Category"  data-allow-clear="true" tabindex="1">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="activityId">Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="activityId" id="activityId" data-placeholder="Select an Activity"  data-allow-clear="true" tabindex="2">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="subActivityId">Sub Activity</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subActivityId" id="subActivityId" data-placeholder="Select a Sub Activity"  data-allow-clear="true" tabindex="3">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="startPromo">Activity Period</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm cursor-pointer" name="startPromo" id="startPromo" autocomplete="off" tabindex="4">
                                            <span class="input-group-text">to</span>
                                            <input type="text" class="form-control form-control-sm cursor-pointer" name="endPromo" id="endPromo" autocomplete="off" tabindex="5">
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="activityDesc">Activity Name</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <textarea maxlength="255" class="form-control form-control-sm" name="activityDesc" id="activityDesc" tabindex="6"></textarea>
                                    </div>
                                </div>
                            </div>

                            {{-- Form Promo Plannning Right--}}
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="entityId">Entity</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12 ">
                                        <select class="form-select form-select-sm" data-control="select2" name="entityId" id="entityId" data-placeholder="Select an Entity"  data-allow-clear="true" tabindex="7">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label  required" for="distributorId">Distributor</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="distributorId" id="distributorId" data-placeholder="Select a Distributor"  data-allow-clear="true" tabindex="8">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="initiatorNotes">Initiator Notes</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <textarea maxlength="255" rows="5" class="form-control form-control-sm" name="initiatorNotes" id="initiatorNotes" tabindex="9"></textarea>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="investmentType">Investment Type</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input class="form-control form-control-sm form-control-solid-bg" type="text" name="investmentType" id="investmentType" autocomplete="off" readonly />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row d-none" id="row_show_desc">
                            <div class="col-lg-6 col-12">
                                <div class="row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label "></label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12" id='show_desc'></div>
                                </div>
                            </div>
                        </div>

                        <div class="separator border-2 my-2 border-secondary"></div>

                        <div class="row">
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="baselineSales">Baseline Sales</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="baselineSales" id="baselineSales" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                            <span class="input-group-text cursor-pointer btn-outline-optima" id="btn_get_baseline_sales" title="Get baseline sales" tabindex="10">
                                                <span class="fa fa-sync"></span>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="incrementSales">Sales Increment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="incrementSales" id="incrementSales" autocomplete="off" value="0" inputmode="decimal" onClick="this.select();" tabindex="11"/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="col-lg-4 col-md-12 col-sm-12 col-12 col-form-label required" for="investment">Investment</label>
                                    <div class="col-lg-8 col-md-12 col-sm-12 col-12">
                                        <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="investment" id="investment" autocomplete="off" value="0" inputmode="decimal" onClick="this.select();" tabindex="12"/>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-12">
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalSales">Total Sales</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="totalSales" id="totalSales" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="totalInvestment">Total Investment</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="totalInvestment" id="totalInvestment" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="roi">ROI</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="roi" id="roi" autocomplete="off" value="0" inputmode="decimal" onClick="this.select();" readonly/>
                                            <span class="input-group-text" id="addoncr">
                                                %
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row fv-row">
                                    <label class="offset-lg-2 col-lg-4 col-md-12 col-sm-12 col-12 col-form-label" for="costRatio">Cost Ratio</label>
                                    <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                        <div class="input-group input-group-sm">
                                            <input type="text" class="form-control form-control-sm form-control-sm form-control-solid-bg text-end" data-kt-dialer-control="input" name="costRatio" id="costRatio" autocomplete="off" value="0" inputmode="decimal" readonly/>
                                            <span class="input-group-text" id="addoncr">
                                                %
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-3 card_attribute">
            <div class="col-lg-12 col-12">
                {{-- Detail Attribute --}}
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex py-1 flex-grow-1 justify-content-between">
                            <span class="fw-bold text-gray-700 fs-4">Detail Attribute</span>
                            <div class="d-flex align-items-center py-1">
                                <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_attribute">
                                    <span class="indicator-label">
                                        <span class="fa fa-edit"></span> Attributes
                                    </span>
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                            </div>
                        </div>
                        <div class="separator border-2 my-2 border-secondary"></div>
                        <div class="row mb-3">
                            <div class="col-lg-3">
                                <div class="row fv-row">
                                    <label class="col-form-label col-lg-12 required" for="channelId">Channel</label>
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="channelId" id="channelId" data-placeholder="Select a Channel"  data-allow-clear="true" tabindex="13">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="row fv-row">
                                    <label class="col-form-label col-lg-12 required" for="subChannelId">Sub Channel</label>
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subChannelId" id="subChannelId" data-placeholder="Select a Sub Channel"  data-allow-clear="true" tabindex="14">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="row fv-row">
                                    <label class="col-form-label col-lg-12 required" for="accountId">Account</label>
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="accountId" id="accountId" data-placeholder="Select an Account"  data-allow-clear="true" tabindex="15">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="row fv-row">
                                    <label class="col-form-label col-lg-12 required" for="subAccountId">Sub Account</label>
                                    <div class="col-lg-12">
                                        <select class="form-select form-select-sm" data-control="select2" name="subAccountId" id="subAccountId" data-placeholder="Select an Account"  data-allow-clear="true" tabindex="16">
                                            <option></option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {{-- CARD REGION --}}
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

                            {{-- CARD BRAND --}}
                            <div class="col-lg-4">
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

                            {{-- CARD SKU --}}
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
                            <div class="col-sm-12 col-12">
                                <table id="dt_mechanism" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    @include('Promo::promo-planning.form-attribute')
@endsection

@section('vendor-script')
    <script src="{{ asset('assets/plugins/custom/datatables/datatables.bundle.js') }}"></script>
    <script src="{{ asset('assets/plugins/custom/datatables/dataTables.checkboxes.js') }}"></script>
@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>

    <script src="{{ asset('assets/pages/promo/promo-planning/js/promo-planning-form.js?v=10') }}"></script>
    <script src="{{ asset('assets/pages/promo/promo-planning/js/promo-planning-form-attribute.js?v=7') }}"></script>
@endsection

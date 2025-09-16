@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')
    <link href="{{ asset('assets/pages/configuration/editable-promo-items/css/editable-promo-items-methods.css?v=' . microtime()) }}"  rel="stylesheet" type="text/css" />
@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Configuration</small>
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
        <div class="col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <div class="row mt-2" id="tab__editable_promo_item">
                        <div class="col-12">
                            <ul class="nav nav-tabs">
                                <li class="nav-item">
                                    <a class="nav-link active" data-bs-toggle="tab" data-bs-target="#rc" id="tabRc">Retailer Cost</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" data-bs-toggle="tab" data-bs-target="#dc" id="tabDc">Distributor Cost</a>
                                </li>
                            </ul>
                            <div class="tab-content" id="tab_content_editable_promo_item">
                                <div class="tab-pane show active" id="rc">
                                    <div class="row mt-5">
                                        <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                            <div class="w-auto">
                                                <span class="">Period for Download</span>
                                            </div>
                                            <div class="w-auto w-100 mx-lg-10">
                                                <div class="input-group input-group-sm" id="dialer_period">
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="filter_period" id="filter_period" value="{{ @date('Y') }}" autocomplete="off"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="w-auto ms-lg-auto">
                                                <button type="button" class="btn btn-sm btn-clean me-lg-2 my-2 w-100 w-lg-auto" id="btn_export_excel">
                                                    <span class="bi bi-download"></span>
                                                </button>

                                                <button type="button" class="btn btn-sm btn-optima fw-bold w-100 w-lg-auto" id="btn_save">
                                                    <span class="indicator-label">
                                                        <span class="fa fa-save"></span> Save
                                                    </span>
                                                    <span class="indicator-progress">Saving...
                                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                                    </span>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="separator my-3"></div>
                                    <form id="form_config">
                                        <div class="row mt-3">
                                            <div class="col-lg-6 col-12">
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="budgetYear" class="">Budget Year</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="budgetYear" name="budgetYear" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="promoPlanning" class="">Promo Planning</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="promoPlanning" name="promoPlanning" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="budgetSource" class="">Budget Source</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="budgetSource" name="budgetSource" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="subCategory" class="">Sub Category</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="subCategory" name="subCategory" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="activity" class="">Activity</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="activity" name="activity" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="subActivity" class="">Sub Activity</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="subActivity" name="subActivity" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="startPromo" class="">Start Promo</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="startPromo" name="startPromo" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="endPromo" class="">End Promo</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="endPromo" name="endPromo" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="activityName" class="">Activity Description</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="activityName" name="activityName" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="initiatorNotes" class="">Initiator Notes</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="initiatorNotes" name="initiatorNotes" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="incrSales" class="">Increment Sales</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="incrSales" name="incrSales" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="investment" class="">Investment</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="investment" name="investment" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="roi" class="">ROI</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="roi" name="roi" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="cr" class="">CR</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="cr" name="cr" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 col-12">
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="channel" class="">Channel</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="channel" name="channel" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="subChannel" class="">Sub Channel</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="subChannel" name="subChannel" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="account" class="">Account</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="account" name="account" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="subAccount" class="">Sub Account</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="subAccount" name="subAccount" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="region" class="">Region</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="region" name="region" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="brand" class="">Brand</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="groupBrand" name="groupBrand" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="sku" class="">SKU</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="sku" name="sku" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="mechanism" class="">Mechanism</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="mechanism" name="mechanism" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="attachment" class="">Attachment</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="attachment" name="attachment" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                                <div class="tab-pane show" id="dc">
                                    <div class="row mt-5">
                                        <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                            <div class="w-auto">
                                                <span class="">Period for Download</span>
                                            </div>
                                            <div class="w-auto w-100 mx-lg-10">
                                                <div class="input-group input-group-sm" id="dialer_period_dc">
                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="decrease">
                                                        <i class="fa fa-minus fs-2"></i>
                                                    </button>

                                                    <input type="text" class="form-control form-control-sm text-end" data-kt-dialer-control="input" name="filter_period_dc" id="filter_period_dc" value="{{ @date('Y') }}" autocomplete="off"/>

                                                    <button class="btn btn-sm btn-icon btn-outline btn-outline-secondary btn-active-color-primary" type="button" data-kt-dialer-control="increase">
                                                        <i class="fa fa-plus fs-2"></i>
                                                    </button>
                                                </div>
                                            </div>
                                            <div class="w-auto ms-lg-auto">
                                                <button type="button" class="btn btn-sm btn-clean me-lg-2 my-2 w-100 w-lg-auto" id="btn_export_excel_dc">
                                                    <span class="bi bi-download"></span>
                                                </button>

                                                <button type="button" class="btn btn-sm btn-optima fw-bold w-100 w-lg-auto" id="btn_save_dc">
                                                    <span class="indicator-label">
                                                        <span class="fa fa-save"></span> Save
                                                    </span>
                                                    <span class="indicator-progress">Saving...
                                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                                    </span>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="separator my-3"></div>
                                    <form id="form_config_dc">
                                        <div class="row mt-3">
                                            <div class="col-lg-6 col-12">
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="budgetYearDC" class="">Budget Year</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="budgetYearDC" name="budgetYear" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="entityDC" class="">Entity</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="entityDC" name="entity" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="groupBrandDC" class="">Brand</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="groupBrandDC" name="groupBrand" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="distributorDC" class="">Distributor</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="distributorDC" name="distributor" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="subActivityTypeDC" class="">Sub Activity Type</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="subActivityTypeDC" name="subActivityType" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="subActivityDC" class="">Sub Activity</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="subActivityDC" name="subActivity" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="channelDC" class="">Channel</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="channelDC" name="channel" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-6 col-12">
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="budgetSourceDC" class="">Budget Source</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="budgetSourceDC" name="budgetSource" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="startPromoDC" class="">Start Promo</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="startPromoDC" name="startPromo" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="endPromoDC" class="">End Promo</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                    <input class="form-check-input" type="checkbox" role="switch" id="endPromoDC" name="endPromo" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="mechanismDC" class="">Mechanism</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="mechanismDC" name="mechanism" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="investmentDC" class="">Investment</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="investmentDC" name="investment" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="initiatorNotesDC" class="">Initiator Notes</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                    <input class="form-check-input" type="checkbox" role="switch" id="initiatorNotesDC" name="initiatorNotes" autocomplete="off">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row mb-3">
                                                    <div class="d-flex flex-lg-row flex-column w-100 align-items-lg-center">
                                                        <div class="w-lg-33 w-auto">
                                                            <label for="entityDC" class="">Attachment</label>
                                                        </div>
                                                        <div class="w-lg-50 w-auto">
                                                            <div class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                                                <input class="form-check-input" type="checkbox" role="switch" id="attachmentDC" name="attachment" autocomplete="off">
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
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')

@endsection

@section('page-script')
    <script src="{{ asset('assets/js/format.js?v=' . microtime()) }}"></script>
    <script src="{{ asset('assets/pages/configuration/editable-promo-items/js/editable-promo-items-methods.js?v=8') }}"></script>
@endsection

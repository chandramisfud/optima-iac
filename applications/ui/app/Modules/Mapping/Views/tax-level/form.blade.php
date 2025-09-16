@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')

@endsection

@section('breadcrumb')
    <span class="d-flex align-items-center fs-3 my-1">@yield('title')
        <span class="h-20px border-gray-200 border-start ms-3 mx-2"></span>
        <small class="text-muted fs-6 fw-bold mt-1 ms-1" id="txt_info_method">Add New</small>
    </span>
@endsection

@section('button-toolbar-right')
    @if(isset($_GET['method']) && $_GET['method'] === 'update')
        @include('toolbars.btn-save-back')
    @else
        @include('toolbars.btn-back-save-exit-add-new')
    @endif
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-body">
                    <form id="form_mapping_tax_level" class="form" autocomplete="off">
                        @csrf
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="materialNumber">Material Number</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="materialNumber" id="materialNumber" placeholder="Enter Material Number" autocomplete="off" tabindex="1" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="description">Description</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="description" id="description" placeholder="Enter Description" autocomplete="off" tabindex="2" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="whT_Type">WHT Type</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="whT_Type" id="whT_Type" placeholder="Enter WHT Type" autocomplete="off" tabindex="3" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="whT_Code">WHT Code</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="whT_Code" id="whT_Code" placeholder="Enter WHT Code" autocomplete="off" tabindex="4" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="purpose">Purpose</label>
                            <div class="col-lg-5">
                                <input class="form-control form-control-sm" type="text" name="purpose" id="purpose" placeholder="Enter Purpose" autocomplete="off" tabindex="5" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label required" for="entityId">Entity</label>
                            <div class="col-lg-5">
                                <select class="form-select form-select-sm" data-control="select2" name="entityId" id="entityId" data-placeholder="Select an Entity"  data-allow-clear="true" tabindex="6">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label" for="ppnPct">PPN (%)</label>
                            <div class="col-lg-2">
                                <input class="form-control form-control-sm text-end" type="text" name="ppnPct" id="ppnPct" value="0" autocomplete="off" tabindex="7" />
                            </div>
                        </div>
                        <div class="row fv-row">
                            <label class="col-lg-3 col-form-label" for="pphPct">PPH (%)</label>
                            <div class="col-lg-2">
                                <input class="form-control form-control-sm text-end" type="text" name="pphPct" id="pphPct" value="0" autocomplete="off" tabindex="8" />
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
@endsection

@section('vendor-script')

@endsection

@section('page-script')
    <script src="{{ asset('assets/pages/mapping/tax-level/js/tax-level-form.js?v=' . microtime()) }}"></script>
@endsection

@extends('layouts/layoutMaster')

@section('title', @$title)

@section('vendor-style')

@endsection

@section('page-style')

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
                <div class="card-header">
                    <span class="card-title text-gray-800 fs-4 py-2">Late Creation Configuration
                    <small class="text-muted fs-6 fw-bold fs-4 mt-1 ms-3">for Initiator</small>
                    </span>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="table-responsive">
                            <form id="form_config_late_promo_creation" class="form" autocomplete="off">
                                <table class="table table-sm w-100">
                                    <thead>
                                    <tr>
                                        <th class="min-w-150px min-w-lg-100px text-nowrap align-middle bg-optima text-white ps-2 py-1">
                                            Status
                                        </th>
                                        <th class="min-w-100px min-w-lg-200px text-nowrap align-middle bg-optima text-white py-1">
                                            Reminder Date
                                        </th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class="p-0">
                                                <div class="separator border-secondary border-3 my-2"></div>
                                                <label class="col-md-12 ms-1 required">Late Promo Planning</label>
                                            </td>
                                            <td class="p-0">
                                                <div class="separator border-secondary border-3 my-2"></div>
                                                    <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                                                        <div class="input-group input-group-sm mb-3">
                                                            <input type="text" class="form-control form-control-sm text-end" placeholder="" name="config_planning" id="config_501" value="0" />
                                                            <span class="input-group-text px-2">Days</span>
                                                        </div>
                                                    </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p-0">
                                                <label class="col-md-12 ms-1 required">Late Promo Creation</label>
                                            </td>
                                            <td class="p-0">
                                                <div class="fv-row">
                                                    <div class="col-lg-4 col-md-12 col-sm-12 col-12">
                                                        <div class="input-group input-group-sm mb-3">
                                                            <input type="text" class="form-control form-control-sm text-end" placeholder="" name="config_502" id="config_502" value="0" />
                                                            <span class="input-group-text px-2">Days</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </form>
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
    <script src="{{ asset('assets/pages/configuration/late-promo-creation/js/late-promo-creation-methods.js') }}"></script>
@endsection

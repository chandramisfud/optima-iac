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

@section('button-toolbar')
    @include('toolbars.btn-save')
@endsection

@section('toolbar')
    @include('toolbars.toolbar')
@endsection

@section('content')
    <div class="row">
        <div class="col-md-12 col-12">
            <div class="card shadow-sm card_form">
                <div class="card-header">
                    <span class="card-title text-gray-800 fs-4 py-2">Reminder Configuration
                    <small class="text-muted fs-6 fw-bold fs-4 mt-1 ms-3">for Initiator</small>
                    </span>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="table-responsive">
                            <form id="form_config_promo_initiator_reminder" class="form" autocomplete="off">
                                <table class="table table-sm w-100">
                                    <thead>
                                    <tr>
                                        <th class="min-w-200px min-w-lg-200px min-w-md-200px min-w-sm-200px text-nowrap align-middle bg-optima text-white ps-2 py-1">
                                            Status
                                        </th>
                                        <th class="min-w-250px min-w-lg-600px min-w-md-500px min-w-sm-300px text-nowrap align-middle bg-optima text-white py-1">
                                            Reminder Date
                                        </th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    <tr>
                                        <td class="p-0">
                                            <div class="separator border-secondary border-3 my-2"></div>
                                            <label class="col-md-12 ms-1 mt-2">Reminder Promo Plan Creation Q1</label>
                                        </td>
                                        <td class="p-0 pt-2">
                                            <div class="separator border-secondary border-3"></div>
                                            <div class="row">
                                                <div class="col-lg-4 col-md-12 col-sm-12 col-12 my-2 mb-2">
                                                    <input type="text" class="form-control form-control-sm" placeholder="" name="promo_plan_creation_q1" id="promo_plan_creation_q1" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p-0">
                                            <label class="col-md-12 ms-1 mt-2">Reminder Promo Plan Creation Q2</label>
                                        </td>
                                        <td class="p-0">
                                            <div class="row">
                                                <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-2">
                                                    <input type="text" class="form-control form-control-sm" placeholder="" name="promo_plan_creation_q2" id="promo_plan_creation_q2" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p-0">
                                            <label class="col-md-12 ms-1 mt-2">Reminder Promo Plan Creation Q3</label>
                                        </td>
                                        <td class="p-0">
                                            <div class="row">
                                                <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-2">
                                                    <input type="text" class="form-control form-control-sm" placeholder="" name="promo_plan_creation_q3" id="promo_plan_creation_q3" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p-0">
                                            <label class="col-md-12 ms-1 mt-2">Reminder Promo Plan Creation Q4</label>
                                        </td>
                                        <td class="p-0">
                                            <div class="row">
                                                <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-2">
                                                    <input type="text" class="form-control form-control-sm" placeholder="" name="promo_plan_creation_q4" id="promo_plan_creation_q4" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p-0">
                                            <label class="col-md-12 ms-1 mt-2">Reminder Promo Reconciliation</label>
                                        </td>
                                        <td class="p-0">
                                            <div class="row">
                                                <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-2">
                                                    <input type="text" class="form-control form-control-sm" placeholder="" name="promo_recon" id="promo_recon" />
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
    <script src="{{ asset('assets/js/format.js') }}"></script>
    <script src="{{ asset('assets/pages/configuration/promo-initiator-reminder/js/promo-initiator-reminder-methods.js') }}"></script>
@endsection

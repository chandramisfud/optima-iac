<div class="modal fade" id="modal-send-email-auto-config" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title fw-normal text-gray-800">Configuration Promo Approval Reminder</h4>
                <div class="btn btn-icon btn-sm btn-active-light-primary ms-2" data-bs-dismiss="modal" aria-label="Close">
                    <span class="svg-icon svg-icon-1">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="black"></rect>
                            <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="black"></rect>
                        </svg>
                    </span>
                </div>
            </div>
            <div class="modal-body">
                <form id="form_promo_approval_config" class="form" autocomplete="off">
                    @csrf
                    <div class="row">
                        <div class="col-md-12 col-12">
                            <div class="row">
                                <label class="col-lg-2 col-form-label">Autorun</label>
                                <div class="col-lg-3">
                                    <div class="text-start">
                                        <span class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                        <label>
                                        <input type="checkbox" class="form-check-input" id="autorun" name="autorun" data-toggle="toggle" autocomplete="off">
                                        </label>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <label class="col-lg-2 col-form-label">Date Reminder 1</label>
                                <div class="col-lg-1 col-md-1 col-2 fv-row">
                                    <div class="text-start">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg numberonly" id="dt1" name="dt1" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly>
                                    </div>
                                </div>
                            </div>
                            <div class="row mb-5">
                                <label class="col-lg-2 col-form-label">Date Reminder 2</label>
                                <div class="col-lg-1 mb-2">
                                    <label class="col-form-label">End of Month</label>
                                </div>
                                <div class="col-lg-1 mb-2">
                                    <span class="form-check form-switch form-check-custom form-check-success form-check-solid">
                                        <label>
                                        <input type="checkbox" class="form-check-input" id="end_of_month" name="end_of_month" data-toggle="toggle" autocomplete="off" disabled>
                                        </label>
                                    </span>
                                </div>
                                <div class="col-lg-1 col-md-1 col-2 fv-row">
                                    <div class="text-start">
                                        <input type="text" class="form-control form-control-sm form-control-solid-bg numberonly" id="dt2" name="dt2" placeholder="" aria-label="" aria-describedby="basic-addon2" autocomplete="off" readonly>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                    <div class="row">
                                        <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-3">
                                            <div class="position-relative w-100 me-md-2">
                                            <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-50 translate-middle ms-6">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                    <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                    <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                </svg>
                                            </span>
                                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_profile_config_search" autocomplete="off">
                                            </div>
                                        </div>
                                        <div class="col-lg-5 col-md-12 col-sm-12 col-12 mb-3">
                                            <select class="form-select form-select-sm" data-control="select2" name="filter_groupuser_config" id="filter_groupuser_config" data-placeholder="Select an User Group Menu"  data-allow-clear="true">
                                                <option></option>
                                            </select>
                                        </div>
                                        <div class="col-lg-3 col-md-12 col-sm-12 col-12 mb-3">
                                            <button type="button" class="btn btn-sm btn-outline-optima text-hover-white float-end w-100" id="btn_add_all_config" disabled>
                                                <span>Add All</span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12 mb-3">
                                            <table id="dt_profile_config" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-md-12 col-sm-12 col-12">
                                    <div class="row">
                                        <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-3">
                                            <div class="position-relative w-100 me-md-2">
                                            <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-50 translate-middle ms-6">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                    <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                    <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                </svg>
                                            </span>
                                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_send_email_config_search" autocomplete="off">
                                            </div>
                                        </div>
                                        <div class="col-lg-5 col-md-12 col-sm-12 col-12 mb-3">
                                            <select class="form-select form-select-sm" data-control="select2" name="filter_groupuser_grouping_config" id="filter_groupuser_grouping_config" data-placeholder="Select an User Group Menu"  data-allow-clear="true">
                                                <option></option>
                                            </select>
                                        </div>
                                        <div class="col-lg-3 col-md-12 col-sm-12 col-12 mb-3">
                                            <button type="button" class="btn btn-sm btn-outline-optima text-hover-white w-100 float-end" id="btn_remove_config" disabled>
                                                <span id="btn_del_config">Remove All</span>
                                            </button>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-12 mb-3 dt_send_email_config">
                                            <table id="dt_send_email_config" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="separator border-3 my-2 border-secondary"></div>
                                <div class="text-end">
                                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="dt_send_email_config_submit">
                                            <span class="indicator-label">
                                                Submit
                                            </span>
                                        <span class="indicator-progress">
                                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                        </span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>


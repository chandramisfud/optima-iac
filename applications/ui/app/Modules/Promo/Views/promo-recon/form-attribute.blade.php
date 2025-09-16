<div class="modal fade" id="modal_attribute" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <div class="d-flex py-1 flex-grow-1 justify-content-between">
                    <h4 class="modal-title fw-normal text-gray-800">Detail Attributes</h4>
                    <div class="d-flex align-items-center py-1">
                        <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_attribute_submit">
                            <span class="indicator-label">
                                <span class="fa fa-edit"></span> Submit
                            </span>
                            <span class="indicator-progress">
                                <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                            </span>
                        </button>
                    </div>
                </div>
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
                <div class="kt-form kt-form--label-right">
                    <div class="row">
                        <div class="col-12">
                            <div class="row">
                                <div class="col-md-4" id="attribute_region">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                            <div class="inner-addon left-addon right-addon">
                                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                    </svg>
                                                </span>
                                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_region_search" autocomplete="off">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <table id="dt_region" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                    </div>
                                </div>

                                <div class="col-md-4" id="attribute_brand">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                            <div class="inner-addon left-addon right-addon">
                                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                    </svg>
                                                </span>
                                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_brand_search" autocomplete="off">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <table id="dt_brand" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                    </div>
                                </div>

                                <div class="col-md-4" id="attribute_sku">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                            <div class="inner-addon left-addon right-addon">
                                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                    </svg>
                                                </span>
                                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_sku_search" autocomplete="off">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <table id="dt_sku" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <label class="col-form-label col-form-label-sm col-lg-2 col-sm-12 required">Mechanism(s)</label>
                        </div>
                        <div class="row" id="manualMechanism">
                            <div class="col-lg-6 col-md-6 col-12">
                                <input type="text" class="form-control form-control-sm mb-2" name="mechanisme1" id="mechanisme1" autocomplete="off">
                                <span class="form-text text-muted"></span>
                                <input type="text" class="form-control form-control-sm mb-2" name="mechanisme2" id="mechanisme2" autocomplete="off">
                                <span class="form-text text-muted"></span>
                            </div>

                            <div class="col-lg-6 col-md-6 col-12">
                                <input type="text" class="form-control form-control-sm mb-2" name="mechanisme3" id="mechanisme3" autocomplete="off">
                                <span class="form-text text-muted"></span>
                                <input type="text" class="form-control form-control-sm mb-2" name="mechanisme4" id="mechanisme4" autocomplete="off">
                                <span class="form-text text-muted"></span>
                            </div>
                        </div>
                        <div class="row" id="newMechanism">
                            <div class="col-lg-4 col-md-4 col-12" id="mechanism_source">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                        <div class="inner-addon left-addon right-addon">
                                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                    </svg>
                                                </span>
                                            <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_mechanism_source_search" autocomplete="off">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <table id="dt_mechanism_source" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                </div>
                            </div>
                            <div class="col-lg-8 col-md-8 col-12" id="mechanism_result">
                                <div class="row">
                                    <div class="col-lg-12 col-md-12 col-sm-12 mb-lg-0 mb-2">
                                        <div class="inner-addon left-addon right-addon">
                                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                                    </svg>
                                                </span>
                                            <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_mechanism_result_search" autocomplete="off">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <table id="dt_mechanism_result" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_mechanism_notes" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-md">
        <div class="modal-content">
            <div class="modal-header">
                <div class="d-flex py-1 flex-grow-1 justify-content-between">
                    <h4 class="modal-title fw-normal text-gray-800">Mechanism Notes</h4>
                </div>
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
                <div class="row">
                    <label class="col-form-label col-form-label-sm col-lg-2 col-sm-12 ">Notes</label>
                    <div class="col-lg-10 col-md-10 col-sm-12">
                        <textarea rows="5" maxlength="255" class="form-control form-control-sm" name="mechanism_notes" id="mechanism_notes" value="" autocomplete="off"></textarea>
                        <span class="form-text text-muted"></span>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <div class="d-flex align-items-center py-1">
                    <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="btn_updatenotes">
                        <span class="indicator-label">
                            <span class="fa fa-save"></span> Save
                        </span>
                        <span class="indicator-progress">
                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                        </span>
                    </button>
                </div>
            </div>
        </div>
    </div>
</div>

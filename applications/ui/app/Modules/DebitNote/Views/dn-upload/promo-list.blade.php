<div class="modal fade" id="modal_list_promo" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-xl modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title fw-normal text-gray-800">Promo List</h4>
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
                <div class="row mb-3">
                    <div class="col-lg-2 col-md-12 col-sm-12 mb-lg-0 mb-2">
                        <div class="inner-addon left-addon right-addon">
                                <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute translate-middle ms-6" style="padding-top: 32px">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                        <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                        <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                    </svg>
                                </span>
                            <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_promo_list_search" autocomplete="off">
                        </div>
                    </div>

                    <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-0 mb-2">
                        <select class="form-select form-select-sm" data-control="select2" name="filter_entity" id="filter_entity" data-placeholder="Select an Entity"  data-allow-clear="true">
                            <option></option>
                        </select>
                    </div>

                    <div class="col-lg-3 col-md-12 col-sm-12 mb-lg-0 mb-2">
                        <select class="form-select form-select-sm" data-control="select2" name="filter_distributor" id="filter_channel" data-placeholder="Select a Channel"  data-allow-clear="true">
                            <option></option>
                        </select>
                    </div>

                    <div class="col-lg-1-5 offset-lg-20-percent col-md-12 col-sm-12 mb-lg-0 mb-2">
                        <div class="text-end">
                            <button type="button" class="btn btn-sm btn-outline-optima w-lg-auto w-100" id="dt_promo_list_view">
                                <span class="indicator-label">
                                    <span class="fa fa-search"></span> View
                                    </span>
                                <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                        <table id="dt_promo_list" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


<div class="modal fade" id="modal_list_budget_source" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title fw-normal text-gray-800">Budget Allocation List</h4>
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
                    <div class="col-lg-11 col-12">
                        <div class="row">
                            <div class="position-relative w-lg-150px w-fhd-250px mb-2">
                                    <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-17 translate-middle ms-6">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                            <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                            <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                        </svg>
                                    </span>
                                <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_budget_source_list_search" autocomplete="off">
                            </div>

                            <div class="position-relative w-lg-300px w-fhd-250px ms-lg-3 mb-2">
                                <select class="form-select form-select-sm" data-control="select2" name="filter_entity" id="filter_entity" data-dropdown-parent="#modal_list_budget_source" data-placeholder="Select an Entity"  data-allow-clear="true">
                                    <option></option>
                                </select>
                            </div>

                            <div class="position-relative w-lg-300px w-fhd-250px ms-lg-3 mb-2">
                                <select class="form-select form-select-sm" data-control="select2" name="filter_distributor" id="filter_distributor" data-dropdown-parent="#modal_list_budget_source" data-placeholder="Select a Distributor"  data-allow-clear="true">
                                    <option></option>
                                </select>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-1 col-12">
                        <div class="position-relative w-lg-75px w-fhd-100px ps-lg-0 float-lg-end ms-lg-3 mb-2">
                            <button type="button" class="btn btn-sm btn-outline-optima w-100" id="dt_budget_source_list_view">
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
                        <table id="dt_budget_source_list" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


<div class="modal fade" id="modal-exception" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-xl">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title fw-normal text-gray-800">Exception List</h4>
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
                    <form id="form_exception" class="form" autocomplete="off">
                        @csrf
                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <div class="row">
                                <div class="col-lg-10 col-md-12 col-sm-12 col-12 fv-row">
                                    <div class="input-group mb-5">
                                        <input class="form-control field_upload" id="file" name="file" type="file" data-stripe="file" placeholder="Choose File"/>
                                        <span class="input-group-text">Upload File</span>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-md-12 col-sm-12 col-12" align="right">
                                    <a href="javascript:void(0)" class="btn btn-icon" name="uploadxls" id="uploadxls" title="Upload Excel">
                                        <i class="fa fa-cloud-upload-alt" style="font-size: 2rem; color: black"></i>
                                    </a>
                                    <a href="javascript:void(0)" class="btn btn-icon btn-icon" name="download_template" id="download_template" title="Download Template Excel">
                                        <i class="fa fa-download icon" style="font-size: 2rem; color: black"></i>
                                    </a>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-lg-4 col-md-12 col-sm-12 col-12 mb-3">
                                <div class="position-relative w-100 me-md-2">
                                        <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-50 translate-middle ms-6">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                                <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>
                                                <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>
                                            </svg>
                                        </span>
                                    <input type="text" class="form-control form-control-sm ps-10" name="search" value="" placeholder="Search" id="dt_exception_search" autocomplete="off">
                                </div>
                            </div>
                            <div class="col-lg-8 col-md-12 col-sm-12 col-12 mb-3" align="right">
                                <button type="button" class="btn btn-sm btn-outline-optima text-hover-white w-85" id="btn_send">
                                    <span> Clear </span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12 col-12">
                        <table id="dt_exception" class="table table-striped table-row-bordered table-responsive table-sm table-hover"></table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


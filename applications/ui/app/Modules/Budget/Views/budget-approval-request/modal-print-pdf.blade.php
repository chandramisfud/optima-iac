<div class="modal fade" id="modal_print_pdf" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title fw-normal text-gray-800">Export PDF</h4>
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
                <form id="printPDF">
                    <div class="fv-row">
                        <div class="col-12">
                            <div class="d-flex">
                                <input class="form-control form-control-sm" type="text" name="batchId" id="batchId" placeholder="Batch ID" autocomplete="off"/>

                                <button type="button" class="btn btn-sm btn-optima ms-2 flex-shrink-0" id="btn_export_pdf">
                                    <span class="fa fa-file-pdf"></span> Export PDF
                                    <span class="indicator-progress">
                                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                                    </span>
                                </button>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_edit_approver" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" >
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title fw-bold">Profile</h5>
                <div class="btn btn-sm btn-icon btn-active-color-primary" data-bs-dismiss="modal">
                    <span class="svg-icon svg-icon-1">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="currentColor"></rect>
                            <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="currentColor"></rect>
                        </svg>
                    </span>
                </div>
            </div>

            <div class="modal-body">
                <form id="form_approver" class="form" novalidate="novalidate" autocomplete="off">
                    @csrf
                    <div class="row fv-row">
                        <label class="col-lg-3 col-form-label required">Profile</label>
                        <div class="col-lg-9">
                            <select class="form-select form-select-sm" data-control="select2" name="profile" id="profile" data-dropdown-parent="#modal_edit_approver" data-placeholder="Select a profile"  data-allow-clear="true" tabindex="1">
                                <option></option>
                            </select>
                        </div>
                    </div>
                </form>
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-sm btn-optima fw-bolder" id="btn_submit">
                    <span class="indicator-label">
                        <span class="fa fa-save"></span> Submit
                    </span>
                    <span class="indicator-progress">Submiting...
                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                    </span>
                </button>
            </div>
        </div>
    </div>
</div>

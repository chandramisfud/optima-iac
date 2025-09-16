<div class="modal fade" id="modal_forgot_password" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" >
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title fw-bold">Forgot Password</h5>
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
                <form id="form_forgot_password" class="form" novalidate="novalidate" autocomplete="off">
                    @csrf
                    <div class="fv-row">
                        <div class="col-md-12 col-12">
                            <div class="inner-addon left-addon right-addon">
                                <span class="fa fa-envelope fs-4 mt-comma-15"></span>
                                <input type="email" class="form-control" placeholder="Email" aria-label="Email" name="email_forgot" id="email_forgot">
                            </div>
                        </div>
                    </div>
                </form>
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-sm mb-3 profile-login" id="btn_close_forgot_password" data-bs-dismiss="modal">
                    Close
                </button>
                <button type="button" class="btn btn-sm mb-3 profile-login" id="btn_submit_forgot_password">
                    <span class="indicator-label">
                        Submit
                    </span>
                    <span class="indicator-progress">Processing...
                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                    </span>
                </button>
            </div>
        </div>
    </div>
</div>

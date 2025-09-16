<div class="modal fade" id="modal_popup_login" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Form Login</h5>
            </div>

            <div class="modal-body">
                <form id="form_popup_login" class="form">
                    @csrf
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-12 col-12 fv-row mb-3">
                                <label class="form-label fw-bolder text-dark">User ID</label>
                                <input class="form-control form-control-sm" type="text" name="popup_userid" id="popup_userid"/>
                            </div>

                            <div class="col-md-12 col-12 fv-row mb-3">
                                <label class="form-label fw-bolder text-dark">Password</label>
                                <div class="input-group mb-5">
                                    <input class="form-control form-control-sm" type="password" name="popup_password" id="popup_password" autocomplete="off"/>
                                    <button class="btn btn-sm btn-secondary" id="show-pass"><i class="fa fa-eye"> </i></button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="fv-row" id="alert_login">

                    </div>
                </form>
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-sm btn-primary mb-3" id="btn_popup_login">
                    <span class="indicator-label">
                        <span class="fa fa-sign-in-alt"></span> Login
                    </span>
                    <span class="indicator-progress">Processing...
                        <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                    </span>
                </button>
                <button type="button" class="btn btn-sm btn-secondary mb-3" id="btn_popup_logout">
                    <span class="fa fa-reply"></span> Logout
                </button>
            </div>
        </div>
    </div>
</div>
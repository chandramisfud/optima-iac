<div class="modal fade" id="modal_popup_profile_change" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Select Profile</h5>                
                <div class="btn btn-sm btn-icon btn-active-color-primary" data-bs-dismiss="modal">
                    <!--begin::Svg Icon | path: icons/duotune/arrows/arr061.svg-->
                    <span class="svg-icon svg-icon-1">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="currentColor"></rect>
                            <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="currentColor"></rect>
                        </svg>
                    </span>
                    <!--end::Svg Icon-->
                </div>
            </div>

            <div class="modal-body">
                <form id="form_popup_login" class="form">
                    @csrf
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-12 col-12 fv-row mb-3">
                                <label class="form-label fw-bolder text-dark">Profile</label>
                                <select class="form-select form-select-sm" data-control="select2" name="profile" id="profile" data-placeholder="Select a profile"  data-allow-clear="true" tabindex="0">
                                    <option></option>
                                </select>

                            </div>
                        </div>
                    </div>
                </form>
                {{-- <div class="fv-row" id="alert_select_profile"></div> --}}
            </div>

            <div class="modal-footer">
                <div class="d-flex align-items-end align-self-start flex-column mb-3">
                    <button type="button" class="btn btn-sm mb-3 profile-login" id="btn_popup_profile_change_login">
                        <span class="indicator-label">
                            <span class="fa fa-sign-in-alt"></span> Submit
                        </span>
                        <span class="indicator-progress">Processing...
                            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                        </span>
                    </button>
                </div>
                {{-- <div class="row">
                    <div class="col-lg-12 col-md-12 col-12">
                        <button type="button" class="btn btn-sm btn-primary mb-3" id="btn_popup_profile_change_login">
                            <span class="indicator-label">
                                <span class="fa fa-sign-in-alt"></span> Submit
                            </span>
                            <span class="indicator-progress">Processing...
                                <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
                            </span>
                        </button>
                    </div>
                </div>       --}}
                {{-- <div class="row">
                    <div class="col-lg-12 col-md-12 col-12">
                        <div class="fv-row" id="alert_select_profile">
                    </div>
                </div>               --}}
            </div>
        </div>
    </div>
</div>
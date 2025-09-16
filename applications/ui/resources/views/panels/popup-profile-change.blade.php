<div class="modal fade" id="modal_popup_profile_change" data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content card_switch_profile">
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
                        <label class="col-lg-3 col-form-label">Profile</label>
                        <div class="col-lg-9 py-auto">
                            <select class="form-select form-select-sm" data-control="select2" name="change_profile" id="change_profile" data-placeholder="Select a profile" data-dropdown-parent="#modal_popup_profile_change"  data-allow-clear="true" tabindex="0">
                                <option></option>
                            </select>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>

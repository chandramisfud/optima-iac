'use strict';
var KTImageInput;
KTImageInput.createInstances();
var imageInputElement = document.querySelector("#kt_image_input_control");
var imageInput = new KTImageInput(imageInputElement);


var validator_pass,KTPasswordMeter;
let form_chpass = document.querySelector("#kt_new_password_form");
KTPasswordMeter.createInstances();
var passwordMeterElement = document.querySelector("#kt_password_meter_control");
var passwordMeter = new KTPasswordMeter(passwordMeterElement);

var dt_userprofile, dt_usercoverage;
var target = document.querySelector("#card_myprofile");
var blockUI = new KTBlockUI(target, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    validator_pass = FormValidation.formValidation(form_chpass, {
        fields: {
            password: {
                validators: {
                    notEmpty: {
                        message: "Password must be input"
                    },
                    stringLength: {
                        min: 8,
                        message: 'Use 8 or more characters with a mix of letters, numbers & symbols'
                    },
                }
            },
            confirm_password: {
                validators: {
                    identical: {
                        compare: function () {
                            return form_chpass.querySelector('[name="password"]').value;
                        },
                        message: 'Password does not match',
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

    form_chpass.querySelector('[name="password"]').addEventListener('input', function () {
        validator_pass.revalidateField('confirm_password');
    });
    
    dt_userprofile = $('#dt_userprofile').DataTable({
        dom: 
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row tes'<'col-sm-2'l><'col-sm-8'i><'col-sm-2'p>>",
        // dom: "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
        //     "<'row'<'col-sm-12'tr>>" +
        //     "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        order: [[0, 'asc']],
        ajax: {
            url: '/auth/profile',
            type: 'get',
        },
        // serverSide: true,
        processing: true,
        paging: true,
        searching: true,
        scrollX: true,
        scrollY: "30vh",
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'profileid',
                className: 'text-nowrap align-left',
            },

        ],
        // language: {
        //     url: '/data/datatables/Indonesian.json'
        // },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $("#dt_userprofile_wrapper div.dt_toolbar").html('\
        <div class="position-relative w-md-400px me-md-2">\
            <span class="svg-icon svg-icon-3 svg-icon-gray-500 position-absolute top-50 translate-middle ms-6">\
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">\
                    <rect opacity="0.5" x="17.0365" y="15.1223" width="8.15546" height="2" rx="1" transform="rotate(45 17.0365 15.1223)" fill="currentColor"></rect>\
                    <path d="M11 19C6.55556 19 3 15.4444 3 11C3 6.55556 6.55556 3 11 3C15.4444 3 19 6.55556 19 11C19 15.4444 15.4444 19 11 19ZM11 5C7.53333 5 5 7.53333 5 11C5 14.4667 7.53333 17 11 17C14.4667 17 17 14.4667 17 11C17 7.53333 14.4667 5 11 5Z" fill="currentColor"></path>\
                </svg>\
            </span>\
            <input type="text" class="form-control form-control-sm form-control-solid ps-10" name="search" value="" placeholder="Search" id="dt_userprofile_Search">\
        </div>\
    ');
    $('#dt_userprofile_Search').on('keyup', function () {
        dt_userprofile.search(this.value).draw();
        // tableMechanism_result
        //     .column(1).search(this.value)
        //     .column(2).search(this.value)
        //     .column(3).search(this.value)
        //     .draw()
    });

    dt_usercoverage = $('#dt_usercoverage').DataTable({
        dom: 
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row tes'<'col-sm-2'l><'col-sm-8'i><'col-sm-2'p>>",
        // dom: "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
        //     "<'row'<'col-sm-12'tr>>" +
        //     "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        order: [[0, 'asc']],
        ajax: {
            url: '/useraccess/user/myprofile-coverage',
            type: 'get',
        },
        // serverSide: true,
        processing: true,
        paging: true,
        searching: true,
        scrollX: true,
        scrollY: "30vh",
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                className: 'text-nowrap align-left',
                render: function (data, type, full, meta) {
                    return full.shortDesc + " - " + full.longDesc;
                }

            },

        ],
        // language: {
        //     url: '/data/datatables/Indonesian.json'
        // },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "Ok",
            customClass: {confirmButton: "btn btn-optima"},
            // showCancelButton: true,
            closeOnConfirm: false,
            showLoaderOnConfirm: true,
            // confirmButtonText: 'Save',
            // cancelButtonText: 'Cancel',
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,
            // reverseButtons: true,

        });
    };
});

$('#show_oldpassword').on('touchstart mousedown', function (e) {
    $('#old_password').css('text-transform', 'none');
    let toggle = e.target.parentNode.querySelector('i').classList.toggle("bi-eye");
    if (!toggle) {
        $('#old_password').attr("type", "password");
    } else {
        $('#old_password').attr("type", "text");
    }
});

$('#show_confirm_password').on('touchstart mousedown', function (e) {
    $('#confirm_password').css('text-transform', 'none');
    let toggle = e.target.parentNode.querySelector('i').classList.toggle("bi-eye");
    if (!toggle) {
        $('#confirm_password').attr("type", "password");
    } else {
        $('#confirm_password').attr("type", "text");
    }
});

$('#modal_change_password').on('shown.bs.modal', function () {
    $("#kt_new_password_form").trigger("reset");
    $('#alert_chpass').html('');
});

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    let cek = passwordMeter.getScore();
    let email = $(this).val();
    let oldpassword = $('#old_password').val();
    let password = $('#password').val();
    if(cek>=80){
        validator_pass.validate().then(function (status) {
            if (status === "Valid") {
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                    }
                });
                $.ajax({
                    url         : '/auth/profile/change-password',
                    data        : {email : email, oldpassword: oldpassword, password : password},
                    type        : 'POST',
                    async       : true,
                    dataType    : 'JSON',
                    // cache       : false,
                    // contentType : false,
                    // processData : false,
                    beforeSend: function() {
                        e.setAttribute("data-kt-indicator", "on");
                        e.disabled = !0;
                    },
                    success: function(result, status, xhr, $form) {
                        if(typeof result.islogin != 'undefined'){
                            window.location.href = '/login-page?islogin=1';
                        }else{
                            if (!result.error) {
                                $('#alert_chpass').html('\
                                        <div class="alert alert-dismissible bg-success d-flex flex-column flex-sm-row w-100 p-5 mb-10">\
                                            <span class="svg-icon svg-icon-2hx svg-icon-light me-4 mb-5 mb-sm-0">\
                                                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">\
                                                    <path opacity="0.3" d="M2 4V16C2 16.6 2.4 17 3 17H13L16.6 20.6C17.1 21.1 18 20.8 18 20V17H21C21.6 17 22 16.6 22 16V4C22 3.4 21.6 3 21 3H3C2.4 3 2 3.4 2 4Z" fill="currentColor"></path>\
                                                    <path d="M18 9H6C5.4 9 5 8.6 5 8C5 7.4 5.4 7 6 7H18C18.6 7 19 7.4 19 8C19 8.6 18.6 9 18 9ZM16 12C16 11.4 15.6 11 15 11H6C5.4 11 5 11.4 5 12C5 12.6 5.4 13 6 13H15C15.6 13 16 12.6 16 12Z" fill="currentColor"></path>\
                                                </svg>\
                                            </span>\
                                            <div class="d-flex flex-column text-light pe-0 pe-sm-10">\
                                                <h4 class="mb-2 text-light">Change Password</h4>\
                                                <span>' + result.message + ', please click <a href="/auth/sign-out">here</a> to login</span>\
                                            </div>\
                                            <button type="button" class="position-absolute position-sm-relative m-2 m-sm-0 top-0 end-0 btn btn-icon ms-sm-auto" data-bs-dismiss="alert">\
                                                <span class="svg-icon svg-icon-2x svg-icon-light">\
                                                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">\
                                                        <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="currentColor"></rect>\
                                                        <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="currentColor"></rect>\
                                                    </svg>\
                                                </span>\
                                            </button>\
                                        </div>');
                                e.removeAttribute("data-kt-indicator");
                                e.disabled = !1;
                            } else {
                                $('#alert_chpass').html('\
                                        <div class="alert alert-dismissible bg-danger d-flex flex-column flex-sm-row w-100 p-5 mb-10">\
                                            <span class="svg-icon svg-icon-2hx svg-icon-light me-4 mb-5 mb-sm-0">\
                                                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">\
                                                    <path opacity="0.3" d="M2 4V16C2 16.6 2.4 17 3 17H13L16.6 20.6C17.1 21.1 18 20.8 18 20V17H21C21.6 17 22 16.6 22 16V4C22 3.4 21.6 3 21 3H3C2.4 3 2 3.4 2 4Z" fill="currentColor"></path>\
                                                    <path d="M18 9H6C5.4 9 5 8.6 5 8C5 7.4 5.4 7 6 7H18C18.6 7 19 7.4 19 8C19 8.6 18.6 9 18 9ZM16 12C16 11.4 15.6 11 15 11H6C5.4 11 5 11.4 5 12C5 12.6 5.4 13 6 13H15C15.6 13 16 12.6 16 12Z" fill="currentColor"></path>\
                                                </svg>\
                                            </span>\
                                            <div class="d-flex flex-column text-light pe-0 pe-sm-10">\
                                                <h4 class="mb-2 text-light">Change Password Failed</h4>\
                                                <span>' + result.message + '</span>\
                                            </div>\
                                            <button type="button" class="position-absolute position-sm-relative m-2 m-sm-0 top-0 end-0 btn btn-icon ms-sm-auto" data-bs-dismiss="alert">\
                                                <span class="svg-icon svg-icon-2x svg-icon-light">\
                                                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">\
                                                        <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="currentColor"></rect>\
                                                        <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="currentColor"></rect>\
                                                    </svg>\
                                                </span>\
                                            </button>\
                                        </div>');
                                e.removeAttribute("data-kt-indicator");
                                e.disabled = !1;
                            }
                        }
                    },
                    complete: function() {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown);
                        Swal.fire({
                            title: "Change Password",
                            text: "Change Password Failed",
                            icon: "error",
                            confirmButtonText: "Ok",
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                });
        
            }
        })    
    }else{
        $('#alert_chpass').html('\
                <div class="alert alert-dismissible bg-danger d-flex flex-column flex-sm-row w-100 p-5 mb-10">\
                    <span class="svg-icon svg-icon-2hx svg-icon-light me-4 mb-5 mb-sm-0">\
                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">\
                            <path opacity="0.3" d="M2 4V16C2 16.6 2.4 17 3 17H13L16.6 20.6C17.1 21.1 18 20.8 18 20V17H21C21.6 17 22 16.6 22 16V4C22 3.4 21.6 3 21 3H3C2.4 3 2 3.4 2 4Z" fill="currentColor"></path>\
                            <path d="M18 9H6C5.4 9 5 8.6 5 8C5 7.4 5.4 7 6 7H18C18.6 7 19 7.4 19 8C19 8.6 18.6 9 18 9ZM16 12C16 11.4 15.6 11 15 11H6C5.4 11 5 11.4 5 12C5 12.6 5.4 13 6 13H15C15.6 13 16 12.6 16 12Z" fill="currentColor"></path>\
                        </svg>\
                    </span>\
                    <div class="d-flex flex-column text-light pe-0 pe-sm-10">\
                        <h4 class="mb-2 text-light">Change Password Failed</h4>\
                        <span>Use 8 or more characters with a mix of letters, numbers &amp; symbols.</span>\
                    </div>\
                    <button type="button" class="position-absolute position-sm-relative m-2 m-sm-0 top-0 end-0 btn btn-icon ms-sm-auto" data-bs-dismiss="alert">\
                        <span class="svg-icon svg-icon-2x svg-icon-light">\
                            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">\
                                <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="currentColor"></rect>\
                                <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="currentColor"></rect>\
                            </svg>\
                        </span>\
                    </button>\
                </div>');
        e.removeAttribute("data-kt-indicator");
        e.disabled = !1;

    }  
})

imageInput.on("kt.imageinput.changed", function(e) {
	var input = document.getElementById('avatar');
	var files = input.files;
	var formData = new FormData();
	for (var i = 0; i != files.length; i++) {
		formData.append("avatar", files[i]);
	}

	let url = '/auth/profile/picture/store';
	$.ajax({
		xhr: function () {
			var xhr = new window.XMLHttpRequest();
			//Upload progress
			xhr.upload.addEventListener("progress", function (evt) {
				if (evt.lengthComputable) {
					var percentComplete = evt.loaded / evt.total;
					//Do something with upload progress
					//  //console.log(percentComplete);
				}
			}, false);
			//Download progress
			xhr.addEventListener("progress", function (evt) {
				if (evt.lengthComputable) {
					var percentComplete = evt.loaded / evt.total;
					//Do something with download progress
					//  //console.log(percentComplete);
				}
			}, false);
			return xhr;
		},
		url: url,
		// crossDomain: true,
		data: formData,
		processData: false,
		contentType: false,
		cache: false,
		method: "POST",
		enctype: "multipart/form-data",
        beforeSend: function() {
            // e.setAttribute("data-kt-indicator", "on");
            // disabledButtonSave();
            // e.disabled = !0;
            // blockUI.block();
        },
        success: function(result, status, xhr, $form) {
            if(typeof result.islogin != 'undefined'){
                window.location.href = '/login-page?islogin=1';
            }else{
                if (!result.error) {
                    Swal.fire({
                        title: "User Profile",
                        text: result.message,
                        icon: "success",
                        confirmButtonText: "Ok",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        window.location.href = '/useraccess/user/myprofile';
                    });
                } else {
                    Swal.fire({
                        title: "User Profile",
                        text: result.message,
                        icon: "warning",
                        confirmButtonText: "Ok",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            }
        },
        complete: function() {
            // e.setAttribute("data-kt-indicator", "off");
            // enableButtonSave();
            // e.disabled = !1;
            // blockUI.release();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.message)
            Swal.fire({
                title: "User Profile",
                text: "User Profile Failed",
                icon: "error",
                confirmButtonText: "Ok",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});
'use strict';
var KTImageInput;
KTImageInput.createInstances();
var imageInputElement = document.querySelector("#kt_image_input_control");
var imageInput = new KTImageInput(imageInputElement);

var validator;
var form_reset_password = document.querySelector("#form_change_password");

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('form').submit(false);

    const v_pass = function () {
        return {
            validate: function () {
                let new_password = $('#new_password').val();
                let lower_case = hasLowerCase(new_password);
                let upper_case = hasUpperCase(new_password);
                let number = hasNumber(new_password);
                let special_char = hasSpecialChar(new_password);
                let length_char = hasLength8(new_password);
                let str;
                let arr_condition = ["uppercase", "lowercase", "number", "special character", "8 characters minimum"];
                let valid = false;
                if (lower_case) {
                    arr_condition.remove('lowercase');
                }
                if (upper_case) {
                    arr_condition.remove('uppercase');
                }
                if (number) {
                    arr_condition.remove('number');
                }
                if (special_char) {
                    arr_condition.remove('special character');
                }

                if (length_char) {
                    arr_condition.remove('8 characters minimum');
                }

                (arr_condition.includes("8 characters minimum") && arr_condition.length === 1) ? str = "Password must contains at least " : str = "Password must contains at least one";

                str += " " +  arr_condition.reduce((text, value, i, array) => text + (i < array.length - 1 ? ', ' : ' and ') + value);

                return {
                    valid: valid,
                    message: str
                }
            }
        }
    }

    FormValidation.validators.password_contains = v_pass;

    validator = FormValidation.formValidation(form_reset_password, {
        fields: {
            new_password: {
                validators: {
                    notEmpty: {
                        message: "Password must not be empty"
                    },
                    password_contains: {

                    }
                }
            },
            confirm_password: {
                validators: {
                    identical: {
                        compare: function () {
                            return form_reset_password.querySelector('[name="new_password"]').value;
                        },
                        message: "Password does not match",
                    }
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
            icon: new FormValidation.plugins.Icon({
                valid: '',
                invalid: '',
                validating: 'fa fa-refresh',
            }),
        }
    });
});

$('#new_password').on('keyup', function () {
    validator.revalidateField('new_password');
    if ($('#confirm_password').val() !== "") validator.revalidateField('confirm_password');
});

$('#confirm_password').on('keyup', function () {
    validator.revalidateField('confirm_password');
});

$(".toggle-new-password").click(function() {
    $(this).toggleClass("fa-eye fa-eye-slash");
    let input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});

$(".toggle-confirm-password").click(function() {
    $(this).toggleClass("fa-eye fa-eye-slash");
    let input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});

$('#btn_submit_change_password').on('click', function () {
    let e = document.querySelector("#btn_submit_change_password");
    validator.validate().then(function (status) {
        if (status === "Valid") {
            $.get('/refresh-csrf').done(function(data) {
                $('meta[name="csrf-token"]').attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                    }
                });
                let formData = new FormData($('#form_change_password')[0]);
                $.ajax({
                    url         : '/my-profile/change-password',
                    data        : formData,
                    type        : 'POST',
                    async       : true,
                    dataType    : 'JSON',
                    cache       : false,
                    contentType : false,
                    processData : false,
                    beforeSend: function() {
                        e.setAttribute("data-kt-indicator", "on");
                        e.disabled = !0;
                    },
                    success: function(result, status, xhr, $form) {
                        if (!result.error) {
                            Swal.fire({
                                title: "Change Password",
                                text: "After you change your password, it will last for 60 days",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                icon: "success",
                                confirmButtonText: "OK",
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(function() {
                                window.location.href = '/my-profile';
                            });
                        } else {
                            Swal.fire({
                                title: "Change Password",
                                text: "Change password failed",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                icon: "error",
                                confirmButtonText: "OK",
                                customClass: {confirmButton: "btn btn-optima"}
                            });
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
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            icon: "error",
                            confirmButtonText: "OK",
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                });
            });
        }
    });
});

imageInput.on("kt.imageinput.changed", function(e) {
    var input = document.getElementById('avatar');
    var files = input.files;
    var formData = new FormData();
    for (var i = 0; i != files.length; i++) {
        formData.append("avatar", files[i]);
    }

    let url = '/my-profile/picture/store';
    $.ajax({
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total;
                }
            }, false);
            //Download progress
            xhr.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total;
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
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        window.location.href = '/my-profile';
                    });
                } else {
                    Swal.fire({
                        title: "User Profile",
                        text: result.message,
                        icon: "warning",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            }
        },
        complete: function() {

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.message)
            Swal.fire({
                title: "User Profile",
                text: "User Profile Failed",
                icon: "error",
                allowOutsideClick: false,
                allowEscapeKey: false,
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

const hasLowerCase = (str) => {
    return (/[a-z]/.test(str));
}

const hasUpperCase = (str) => {
    return (/[A-Z]/.test(str));
}

const hasNumber = (str) => {
    return (/[0-9]/.test(str));
}

const hasSpecialChar = (str) => {
    return (/[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/.test(str));
}

const hasLength8 = (str) => {
    return (str.length >= 8);
}

Array.prototype.remove = function() {
    var what, a = arguments, L = a.length, ax;
    while (L && this.length) {
        what = a[--L];
        while ((ax = this.indexOf(what)) !== -1) {
            this.splice(ax, 1);
        }
    }
    return this;
};

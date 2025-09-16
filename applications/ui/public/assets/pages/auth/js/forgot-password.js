'use strict';
var toggleElement = null;
var toggle = KTToggle.getInstance(toggleElement);

var validator, i;
var form_reset_password = document.querySelector("#form_reset_password");

$(document).ready(function () {
    $('form').submit(false);
    let url_str = new URL(window.location.href);
    i = url_str.searchParams.get("i");

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

$('#btn_submit_reset_password').on('click', function () {
    let e = document.querySelector("#btn_submit_reset_password");
    validator.validate().then(function (status) {
        if (status === "Valid") {
            if (i !== "") {
                let formData = new FormData($('#form_reset_password')[0]);
                formData.append('i', i);
                $.get('/refresh-csrf').done(function(data) {
                    $('meta[name="csrf-token"]').attr('content', data)
                    $.ajaxSetup({
                        headers: {
                            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                        }
                    });
                    $.ajax({
                        url         : '/auth/change-password',
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
                                    window.location.href = '/login-page';
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
            } else {
                Swal.fire({
                    title: "Change Password",
                    text: "Your email is not valid",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    icon: "error",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
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

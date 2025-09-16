'use strict';
let toggleElement = null;
let toggle = KTToggle.getInstance(toggleElement);

let targetProfile = document.querySelector("#login_page");
let blockUIProfile = new KTBlockUI(targetProfile, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...<div>',
});

let verify=false, validator, validatorForgotPassword, validatorChangePassword;
const form_change_password = document.querySelector("#form_change_password");

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    let isLogin = url_str.searchParams.get("islogin");

    if (isLogin === "1") {
        $('#alert_login').html('\
        <div class="alert alert-dismissible bg-danger d-flex flex-column flex-sm-row w-100 p-5 mb-10">\
            <div class="d-flex flex-column text-light pe-0 pe-sm-10">\
                <h4 class="mb-2 text-light">You have been <br>logged out</h4>\
                <span>Your User ID is currently being used on another device or browser</span>\
            </div>\
        </div>');
    }

    FormValidation.validators.password_contains = function () {
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
    };

    validatorChangePassword = FormValidation.formValidation(form_change_password, {
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
                            return form_change_password.querySelector('[name="new_password"]').value;
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

    validator = FormValidation.formValidation(document.querySelector("#form_login"), {
        fields: {
            email: {
                validators: {
                    notEmpty: {
                        message: "User email must not be empty"
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_.+-]+@(?:(?:[a-zA-Z0-9-]+\.)?[a-zA-Z]+\.)?(danone.com|tigaraksa.co.id|thetempogroup.com|aladdincommerce.co.id|acommerce.asia|orami.com|aplcare.com|samb.co.id|zuelligpharma.com|xvautomation.com)$/i,
                        message: "Sorry, I've enabled very strict email validation"
                    }
                }
            },
            password: {
                validators: {
                    notEmpty: {
                        message: "Password must not be empty"
                    }
                }
            }
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

    validatorForgotPassword = FormValidation.formValidation(document.querySelector("#form_forgot_password"), {
        fields: {
            email_forgot: {
                validators: {
                    notEmpty: {
                        message: "User email must not be empty"
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_.+-]+@(?:(?:[a-zA-Z0-9-]+\.)?[a-zA-Z]+\.)?(danone.com|tigaraksa.co.id|thetempogroup.com|aladdincommerce.co.id|acommerce.asia|orami.com|aplcare.com|samb.co.id|zuelligpharma.com|xvautomation.com)$/i,
                        message: "Sorry, I've enabled very strict email validation"
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

    (localStorage.getItem('email')) ? $('#email').val(localStorage.getItem('email')) : $('#email').val('');
    (localStorage.getItem('password')) ? $('#password').val(window.atob(localStorage.getItem('password'))) : $('#password').val('');
    (localStorage.getItem('checkbox') === "1") ? $('#remember_me').attr('checked', true) : $('#remember_me').attr('checked', false);
});

$('#email, #password').on('keydown', function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        $('#btn_signin').trigger('click');
    }
});

$('#btn_signin').on('click', function() {
    if (!verify) {
        let e = document.querySelector("#btn_signin");
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        grecaptcha.ready(function() {
            grecaptcha.execute('6Lem7GgcAAAAAFGYul6CBmkto4MrZHs0kIKc4NZa', {action: 'submit'}).then(function(token) {
                let formData = new FormData();
                formData.append('token', token);
                $.get('/refresh-csrf').done(function(data) {
                    $('meta[name="csrf-token"]').attr('content', data)
                    $.ajaxSetup({
                        headers: {
                            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                        }
                    });
                    $.ajax({
                        url: '/verify-captcha',
                        data: formData,
                        type: 'POST',
                        async: true,
                        dataType: 'JSON',
                        cache: false,
                        contentType: false,
                        processData: false,
                        beforeSend: function () {
                        },
                        success: function (result, status, xhr, $form) {
                            if (result.success) {
                                verify = true;
                                $('#btn_signin .indicator-label').text('Login');
                                toastr.options = {
                                    "closeButton": false,
                                    "debug": false,
                                    "newestOnTop": false,
                                    "progressBar": false,
                                    "positionClass": "toastr-bottom-right",
                                    "preventDuplicates": false,
                                    "onclick": null,
                                    "showDuration": "300",
                                    "hideDuration": "300",
                                    "timeOut": "2000",
                                    "extendedTimeOut": "1000",
                                    "showEasing": "swing",
                                    "hideEasing": "linear",
                                    "showMethod": "fadeIn",
                                    "hideMethod": "fadeOut"
                                };

                                toastr.success("Verified, you are not robot!", "Verify Catpcha");
                            } else {
                                verify = false;
                                toastr.options = {
                                    "closeButton": false,
                                    "debug": false,
                                    "newestOnTop": false,
                                    "progressBar": false,
                                    "positionClass": "toastr-bottom-right",
                                    "preventDuplicates": false,
                                    "onclick": null,
                                    "showDuration": "300",
                                    "hideDuration": "300",
                                    "timeOut": "2000",
                                    "extendedTimeOut": "1000",
                                    "showEasing": "swing",
                                    "hideEasing": "linear",
                                    "showMethod": "fadeIn",
                                    "hideMethod": "fadeOut"
                                };

                                toastr.error(result.message, "Verify Catpcha");
                            }
                        },
                        complete: function () {
                            e.setAttribute("data-kt-indicator", "off");
                            e.disabled = !1;
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(errorThrown);
                            toastr.options = {
                                "closeButton": false,
                                "debug": false,
                                "newestOnTop": false,
                                "progressBar": false,
                                "positionClass": "toastr-bottom-right",
                                "preventDuplicates": false,
                                "onclick": null,
                                "showDuration": "300",
                                "hideDuration": "300",
                                "timeOut": "2000",
                                "extendedTimeOut": "1000",
                                "showEasing": "swing",
                                "hideEasing": "linear",
                                "showMethod": "fadeIn",
                                "hideMethod": "fadeOut"
                            };

                            toastr.error("Verifying captcha failed", "Verify Catpcha");
                        }
                    });
                });
            });
        });
    } else {
        validator.validate().then(function (status) {
            if (status === "Valid") {
                let e = document.querySelector("#btn_signin");
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
                $('#alert_login').html('');
                let formData = new FormData($('#form_login')[0]);
                $.get('/refresh-csrf').done(function(data) {
                    $('meta[name="csrf-token"]').attr('content', data)
                    $.ajaxSetup({
                        headers: {
                            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                        }
                    });
                    $.ajax({
                        url: '/auth/sign-in',
                        data: formData,
                        type: 'POST',
                        async: true,
                        dataType: 'JSON',
                        cache: false,
                        contentType: false,
                        processData: false,
                        beforeSend: function () {
                        },
                        success: function (result, status, xhr, $form) {
                            if (!result.error) {
                                let values = result.data;
                                if (result.data.userNew) {
                                    $("#user_profile_id").empty();
                                    let data = [];
                                    for (let j = 0, len = values.profile.length; j < len; ++j) {
                                        data.push({
                                            id: values.profile[j].profileid,
                                            text: values.profile[j].profileid,
                                            usergroupid: values.profile[j].usergroupid
                                        });
                                    }
                                    $('#user_profile_id').select2({
                                        placeholder: "Select a Profile",
                                        width: '100%',
                                        data: data
                                    });
                                    $('#modal_change_password').modal('show');
                                } else {
                                    if (result.data.loginFailedCount >= 3) {
                                        $('#email').prop('readonly', true);
                                        $('#password').prop('readonly', true);
                                        e.removeAttribute("data-kt-indicator");
                                        e.disabled = 1;

                                        $('#alert_login').html(
                                            '<div class="alert alert-dismissible bg-danger d-flex flex-column flex-sm-row w-100 p-5 mb-10">' +
                                            '<div class="d-flex flex-column text-light pe-0">' +
                                            '<h4 class="mb-2 text-light">Login Failed</h4>' +
                                            '<span>You have failed to login 3 times in a row</span>' +
                                            '<span>Please try to login again after ' + (15 - parseInt(result.data.loginFreezeTime)).toString() + ' minutes</span>' +
                                            '</div>' +
                                            '<button type="button" class="position-absolute position-sm-relative m-2 m-sm-0 end-0 btn btn-icon ms-sm-auto btn_alert_close" data-bs-dismiss="alert">' +
                                            '<span class="svg-icon svg-icon-2x svg-icon-light">' +
                                            '<svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">' +
                                            '<rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="currentColor"></rect>' +
                                            '<rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="currentColor"></rect>' +
                                            '</svg>' +
                                            '</span>' +
                                            '</button>' +
                                            '</div>');
                                    } else if (result.data.isLogin > 0 && result.data.errCode == 99) {
                                        // If is login
                                        swal.fire({
                                            icon: "warning",
                                            title: "Warning",
                                            text: result.data.errMessage,
                                            showCancelButton: true,
                                            confirmButtonText: "Yes, Continue",
                                            cancelButtonText: "No, cancel",
                                            allowOutsideClick: false,
                                            allowEscapeKey: false,
                                            customClass: {
                                                confirmButton: "btn btn-sm btn-optima",
                                                cancelButton: "btn btn-sm btn-secondary"
                                            }
                                        }).then(function (res) {
                                            if (res.isConfirmed) {
                                                $('#txt_welcome').removeClass('mt-11rem');
                                                $("#user_profile_id").empty();
                                                let data = [];
                                                for (let j = 0, len = values.profile.length; j < len; ++j) {
                                                    data.push({
                                                        id: values.profile[j].profileid,
                                                        text: values.profile[j].profileid,
                                                        usergroupid: values.profile[j].usergroupid
                                                    });
                                                }
                                                $('#user_profile_id').select2({
                                                    placeholder: "Select a Profile",
                                                    width: '100%',
                                                    data: data
                                                });

                                                setRememberMe();
                                                $('#user_profile_id').val('').trigger('change');
                                                $('#block_form_login').addClass('d-none');
                                                $('#block_form_profile').removeClass('d-none');
                                            } else {
                                                e.disabled = !1;
                                            }
                                        });
                                    } else {
                                        $('#txt_welcome').removeClass('mt-11rem');
                                        $("#user_profile_id").empty();
                                        let data = [];
                                        for (let j = 0, len = values.profile.length; j < len; ++j) {
                                            data.push({
                                                id: values.profile[j].profileid,
                                                text: values.profile[j].profileid,
                                                usergroupid: values.profile[j].usergroupid
                                            });
                                        }
                                        $('#user_profile_id').select2({
                                            placeholder: "Select a Profile",
                                            width: '100%',
                                            data: data
                                        });

                                        setRememberMe();
                                        $('#user_profile_id').val('').trigger('change');
                                        $('#block_form_login').addClass('d-none');
                                        $('#block_form_profile').removeClass('d-none');
                                    }
                                }
                            } else {
                                // If fail 3 times
                                if (result.data.failedCount >= 3) {
                                    $('#email').prop('readonly', true);
                                    $('#password').prop('readonly', true);
                                    e.removeAttribute("data-kt-indicator");
                                    e.disabled = 1;
                                } else {
                                    $('#email').prop('readonly', false);
                                    $('#password').prop('readonly', false);

                                    e.removeAttribute("data-kt-indicator");
                                    e.disabled = !1;
                                }

                                $('#alert_login').html('\
                                        <div class="alert alert-dismissible bg-danger d-flex flex-column flex-sm-row w-100 p-5 mb-10">\
                                            <div class="d-flex flex-column text-light pe-0">\
                                                <h4 class="mb-2 text-light">Login Failed</h4>\
                                                <span>' + result.message + '</span>\
                                            </div>\
                                            <button type="button" class="position-absolute position-sm-relative m-2 m-sm-0 top-0 end-0 btn btn-icon ms-sm-auto btn_alert_close" data-bs-dismiss="alert">\
                                                <span class="svg-icon svg-icon-2x svg-icon-light">\
                                                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">\
                                                        <rect opacity="0.5" x="6" y="17.3137" width="16" height="2" rx="1" transform="rotate(-45 6 17.3137)" fill="currentColor"></rect>\
                                                        <rect x="7.41422" y="6" width="16" height="2" rx="1" transform="rotate(45 7.41422 6)" fill="currentColor"></rect>\
                                                    </svg>\
                                                </span>\
                                            </button>\
                                        </div>');
                            }
                        },
                        complete: function () {
                            e.setAttribute("data-kt-indicator", "off");
                            e.disabled = !1;
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(errorThrown);
                            toastr.options = {
                                "closeButton": false,
                                "debug": false,
                                "newestOnTop": false,
                                "progressBar": false,
                                "positionClass": "toastr-bottom-right",
                                "preventDuplicates": false,
                                "onclick": null,
                                "showDuration": "300",
                                "hideDuration": "300",
                                "timeOut": "2000",
                                "extendedTimeOut": "1000",
                                "showEasing": "swing",
                                "hideEasing": "linear",
                                "showMethod": "fadeIn",
                                "hideMethod": "fadeOut"
                            };

                            toastr.error("Login Error", "Login");
                        }
                    });
                });
            }
        });
    }
});

$('#user_profile_id').on('change', async function () {
    let profileId = $(this).val();
    if (profileId) {
        blockUIProfile.block();
        await selectProfile(profileId);
        await isLogin(profileId);
        window.location.href = "/";
    }
});

$('#btn_submit_forgot_password').on('click', function () {
    let e = document.querySelector("#btn_submit_forgot_password");
    let email = $('#email_forgot').val();
    validatorForgotPassword.validate().then(function (status) {
        if (status === "Valid") {
            let formData = new FormData();
            formData.append('email', email);
            $.get('/refresh-csrf').done(function(data) {
                $('meta[name="csrf-token"]').attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                    }
                });
                $.ajax({
                    url         : '/auth/forgot-password',
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
                            if (result.data.code === 404) {
                                Swal.fire({
                                    title: "Forgot Password",
                                    text: result.data.message,
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    icon: "warning",
                                    confirmButtonText: "OK",
                                    customClass: {confirmButton: "btn btn-optima"}
                                });
                            } else {
                                Swal.fire({
                                    title: "Forgot Password",
                                    text: result.data.message,
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    icon: "success",
                                    confirmButtonText: "OK",
                                    customClass: {confirmButton: "btn btn-optima"}
                                }).then(function() {
                                    $('#modal_forgot_password').modal('hide');
                                });
                            }
                        } else {
                            Swal.fire({
                                title: "Forgot Password",
                                text: result.message,
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
                            title: "Forgot Password",
                            text: "Forgot Password Failed",
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

$('#forgot_password').on('click', function() {
    $('#email_forgotl').val('');
    $('#modal_forgot_password').modal('show');
});

$(".toggle-password").click(function() {

    $(this).toggleClass("fa-eye fa-eye-slash");
    let input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});

$('#txt_remember_me').on('click', function() {
    let checkedRemember = document.getElementById('remember_me');
    (!checkedRemember.checked) ? $('#remember_me').attr('checked', true) : $('#remember_me').attr('checked', false);
});

$('#new_password').on('keyup', function () {
    validatorChangePassword.revalidateField('new_password');
});

$('#confirm_password').on('keyup', function () {
    validatorChangePassword.revalidateField('confirm_password');
});

$('#btn_submit_change_password').on('click', function () {
    let e = document.querySelector("#btn_submit_change_password");
    validatorChangePassword.validate().then(function (status) {
        if (status === "Valid") {
            $.get('/refresh-csrf').done(function(data) {
                $('meta[name="csrf-token"]').attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                    }
                });

                let formData = new FormData();
                formData.append('email', $('#email').val());
                formData.append('new_password', $('#new_password').val());
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
                $.ajax({
                    url         : '/auth/change-password-new-user',
                    data        : formData,
                    type        : 'POST',
                    async       : true,
                    dataType    : 'JSON',
                    cache       : false,
                    contentType : false,
                    processData : false,
                    beforeSend: function() {
                    },
                    success : function(result) {
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
                                $('#txt_welcome').removeClass('mt-11rem');

                                setRememberMe();
                                $('#user_profile_id').val('').trigger('change');
                                $('#block_form_login').addClass('d-none');
                                $('#block_form_profile').removeClass('d-none');
                                $('#modal_change_password').modal('hide');
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
                    },
                    complete: function() {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                    }
                });
            });
        }
    });
});

const sortResults = (arr, prop, asc) => {
    arr.sort(function(a, b) {
        if (asc) {
            return (a[prop] > b[prop]) ? 1 : ((a[prop] < b[prop]) ? -1 : 0);
        } else {
            return (b[prop] > a[prop]) ? 1 : ((b[prop] < a[prop]) ? -1 : 0);
        }
    });
};

const selectProfile = (profileId) => {
    return new Promise((resolve, reject) => {
        $.get('/refresh-csrf').done(function(data) {
            $('meta[name="csrf-token"]').attr('content', data)
            $.ajaxSetup({
                headers: {
                    'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                }
            });

            let formData = new FormData();
            formData.append('profileId', profileId);
            $.ajax({
                url         : '/auth/profile/push-session',
                data        : formData,
                type        : 'POST',
                async       : true,
                dataType    : 'JSON',
                cache       : false,
                contentType : false,
                processData : false,
                beforeSend: function() {

                },
                success : function(result) {
                    if (!result.error) {
                        return resolve();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                    return reject();
                },
                complete: function() {

                }
            });
        });
    });
}

const isLogin = (profileId) => {
    return new Promise( (resolve, reject) => {
        $.get('/refresh-csrf').done(function(data) {
            $('meta[name="csrf-token"]').attr('content', data)
            $.ajaxSetup({
                headers: {
                    'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                }
            });

            let formData = new FormData();
            formData.append('profileId', profileId);

            $.ajax({
                url         : '/auth/is-login-put',
                data        : formData,
                type        : 'POST',
                async       : true,
                dataType    : 'JSON',
                cache       : false,
                contentType : false,
                processData : false,
                beforeSend: function() {
                },
                success : function(result) {
                    return resolve();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                    return reject();
                },
                complete: function() {

                }
            });
        });
    });
}

const setRememberMe = () => {
    let remember_me = $('#remember_me');
    if (remember_me.is(':checked')) {
        localStorage.setItem('email', $('#email').val());
        localStorage.setItem('password', window.btoa($('#password').val()));
        localStorage.setItem('checkbox', '1');
    } else {
        localStorage.setItem('email', '');
        localStorage.setItem('password', '');
        localStorage.setItem('checkbox', '0');
    }

}

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

'use strict'

var validator_popup_login;

// $( document ).idleTimer( 2000 );
// 60000 = 1 menit
let mIdleTime = 60000 * 15
$( document ).idleTimer( mIdleTime );

$( document ).on( "idle.idleTimer", function(event, elem, obj){
    // $('#modal_popup_login').modal('show');

    $.get('/refresh-csrf').done(function(data) {
        $('meta[name="csrf-token"]').attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            }
        });
        $.ajax({
            url         : '/auth/clear-session',
            type        : 'GET',
            async       : false,
            dataType    : 'JSON',
            cache       : false,
            contentType : false,
            processData : false,
            beforeSend: function() {
            },
            success : function(result) {
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            },
            complete: function() {

            }
        });
    });

    Swal.fire({
        text: "Your Session is expired",
        icon: "warning",
        buttonsStyling: !1,
        confirmButtonText: "Ok",
        customClass: {confirmButton: "btn btn-optima"},
        closeOnConfirm: false,
        showLoaderOnConfirm: true,
        closeOnClickOutside: false,
        closeOnEsc: false,
        allowOutsideClick: false,
    }).then(function (result) {
        if (result.value) { window.location.href = "/auth/sign-out"; }
    });
});

$( document ).on( "active.idleTimer", function(event, elem, obj, triggerevent){

});

$(document).ready(function() {

})

$('#modal_popup_login').on('shown.bs.modal', function() {
    $('form').submit(false);

    validator_popup_login = FormValidation.formValidation(document.querySelector("#form_popup_login"), {
        fields: {
            popup_userid: {
                validators: {
                    notEmpty: {message: "User email must not be empty"},
                }
            },
            popup_password: {
                validators: {
                    notEmpty: {message: "Password must not be empty"
                    }
                }
            }
        }
    });
})

$('#show-pass').on('click', function (e) {
    let toggle = e.target.parentNode.querySelector('i').classList.toggle("fa-eye-slash");
    if (!toggle) {
        $('#popup_password').attr("type", "password");
    } else {
        $('#popup_password').attr("type", "text");
    }
});

$('#popup_password, #popup_userid').on('keyup', function(e) {
    let keycode = (e.keyCode ? event.keyCode : e.which);
    if(keycode == '13'){
        $('#btn_popup_login').trigger('click');
    }
})

$('#btn_popup_login').on('click', function() {
    $('#alert_login').html('')
    let e = document.querySelector("#btn_popup_login");
    let formData = new FormData();
    formData.append('userid', $('#popup_userid').val());
    formData.append('password', $('#popup_password').val());
    validator_popup_login.validate().then(function (status) {
        if (status == "Valid") {
            $.get('/refresh-csrf').done(function(data) {
                $('meta[name="csrf-token"]').attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                    }
                });
                $.ajax({
                    url         : '/sing-in',
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
                    success : function(result) {
                        if (result.error) {
                            $('#alert_login').html('<div class="alert alert-danger alert-dismissible">\n' +
                                '                            <div class="d-flex flex-column">\n' +
                                '                                <h5 class="mb-1 text-danger">Login Gagal</h5>\n' +
                                '                                <span>'+ result.message +'</span>\n' +
                                '                            </div>\n' +
                                '                        </div>');
                            e.removeAttribute("data-kt-indicator");
                        } else {
                            $('#modal_popup_login').modal('hide');
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $('#alert_login').html('<div class="alert alert-danger alert-dismissible">\n' +
                            '                            <div class="d-flex flex-column">\n' +
                            '                                <h5 class="mb-1 text-danger">Login Gagal</h5>\n' +
                            '                                <span>'+ textStatus +'</span>\n' +
                            '                            </div>\n' +
                            '                        </div>');
                        e.removeAttribute("data-kt-indicator");
                        console.log(errorThrown);
                    },
                    complete: function() {
                        e.disabled = !1;

                    }
                });
            });
        }
    });
})

$('#btn_popup_logout').on('click', function() {
    window.location.href = "/sing-out";
})

$('#panel_profile_change').on('click', function() {
    $('#modal_popup_profile_change').modal('show');
})


$('#btn_popup_profile_change_login').on('click', function() {
    $('#alert_login').html('')
    let e = document.querySelector("#btn_popup_profile_change_login");
    $.get('/refresh-csrf').done(function(data) {
        $('meta[name="csrf-token"]').attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            }
        });
        let reqdata = { usergroupid : '999.999', userlevel: 999, usergroupname: 'admin' }
        $.ajax({
            url         : '/auth/profile/pushsession',
            data        : reqdata,
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
            success : function(result) {
                if(typeof result.islogin != 'undefined'){
                    window.location.href = '/login-page?islogin=1';
                }else{
                    if (result.error) {
                        $('#alert_login').html('<div class="alert alert-danger alert-dismissible">\n' +
                            '                            <div class="d-flex flex-column">\n' +
                            '                                <h5 class="mb-1 text-danger">Select Profile Failed</h5>\n' +
                            '                                <span>'+ result.message +'</span>\n' +
                            '                            </div>\n' +
                            '                        </div>');
                        e.removeAttribute("data-kt-indicator");
                    } else {
                        $('#modal_popup_profile_change').modal('hide');
                        let url
                        if (result.redirect == 'null' || result.redirect == null || typeof result.redirect === 'undefined') {
                            url = '/';
                        } else {
                            url = result.redirect
                        }
                        window.location.href = url;

                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $('#alert_login').html('<div class="alert alert-danger alert-dismissible">\n' +
                    '                            <div class="d-flex flex-column">\n' +
                    '                                <h5 class="mb-1 text-danger">Select Profile Failed</h5>\n' +
                    '                                <span>'+ textStatus +'</span>\n' +
                    '                            </div>\n' +
                    '                        </div>');
                e.removeAttribute("data-kt-indicator");
                console.log(errorThrown);
            },
            complete: function() {
                e.disabled = !1;

            }
        });
    });
})

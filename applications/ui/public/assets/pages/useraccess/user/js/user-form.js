'use strict';

var validator, dt_users_mstprofile, dt_users_profile, method, userid;
var swalTitle = "User Management";

var target_profile = document.querySelector(".card_form_profile");
var blockUI_profile = new KTBlockUI(target_profile, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    userid = url_str.searchParams.get("userid");

    if (method === "update") {
        blockUI.block();
        blockUI_profile.block();
        disableButtonSave();
        Promise.all([getData(userid)]).then(async () => {
            blockUI.release();
            blockUI_profile.release();
            enableButtonSave();
        });
    }

    validator = FormValidation.formValidation(document.querySelector("#form_user"), {
        fields: {
            email: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_.+-]+@(?:(?:[a-zA-Z0-9-]+\.)?[a-zA-Z]+\.)?(danone.com|tigaraksa.co.id|thetempogroup.com|aladdincommerce.co.id|acommerce.asia|orami.com|aplcare.com|samb.co.id|zuelligpharma.com|xvautomation.com)$/i,
                        message: "Sorry, I've enabled very strict email validation"
                    }
                }
            },
            username: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    }
                }
            },
            contactinfo: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    }
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

    dt_users_mstprofile =  $('#dt_users_mstprofile').DataTable({
        dom: "<'row'<'col-sm-12'Rtr>>",
        order: [[0, 'asc']],
        ajax: {
            url: '/useraccess/user/list/profile',
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "54vh",
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Master Profile',
                data: 'id',
                className: 'text-nowrap align-middle cursor-pointer',
            },

        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $('#dt_users_mstprofile_Search').on('keyup', function () {
        dt_users_mstprofile.search(this.value).draw();
    });

    $('#dt_users_mstprofile').on( 'dblclick', 'tr', function () {
        var data_profile = dt_users_mstprofile.row( this ).data();
        let data = dt_users_profile.rows().data().toArray();
        if (dt_users_profile.rows().data().length >= 1) {
            let filter = {
                profileid: data_profile.id
            };
            data = data.filter(function (item) {
                for (let key in filter) {
                    if (item[key] === undefined || item[key] !== filter[key]) {
                        return false;
                    }
                }
                return true;
            });
            if (data.length >= 1) {
                Swal.fire({
                    title: swalTitle,
                    text: "Profile already exist",
                    icon: "warning",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            } else {
                let profiles = [];
                profiles['profileid'] = data_profile.id;
                dt_users_profile.row.add(profiles).draw();
            }
        } else {
            let profiles = [];
            profiles['profileid'] = data_profile.id;
            dt_users_profile.row.add(profiles).draw();
        }
    });

    dt_users_profile =  $('#dt_users_profile').DataTable({
        dom: "<'row'<'col-sm-12'Rtr>>",
        processing: false,
        serverSide: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "54vh",
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'User Profile',
                data: 'profileid',
                className: 'text-nowrap align-middle cursor-pointer',
            },

        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $('#dt_users_profile_Search').on('keyup', function () {
        dt_users_profile.search(this.value).draw();
    });

    $('#dt_users_profile').on( 'dblclick', 'tr', function () {
        let tr = this.closest("tr");
        let trindex = dt_users_profile.row(tr).index();
        dt_users_profile.row(trindex).remove().draw();
    });
});

$('#btn_back').on('click', function () {
    window.location.href = '/useraccess/user';
});

$('#btn_save').on('click', function () {
    let trdata = dt_users_profile.rows().data();
    let e = document.querySelector("#btn_save");
    validator.validate().then(function (status) {
        if (status == "Valid") {
            if (trdata.length < 1) {
                Swal.fire({
                    title: swalTitle,
                    text: "Select one or more profile",
                    icon: "warning",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            } else {
                let dataProfiles = [];
                $.each(trdata, function (index, value) {
                    dataProfiles.push(value['profileid']);
                });

                let formData = new FormData($('#form_user')[0]);
                formData.append('profile', JSON.stringify(dataProfiles));
                let url = '/useraccess/user/save';
                if (method === 'update') {
                    url = '/useraccess/user/update';
                    formData.append('id', userid);
                }
                $.get('/refresh-csrf').done(function(data) {
                    $('meta[name="csrf-token"]').attr('content', data)
                    $.ajaxSetup({
                        headers: {
                            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                        }
                    });
                    $.ajax({
                        url: url,
                        data: formData,
                        type: 'POST',
                        async: true,
                        dataType: 'JSON',
                        cache: false,
                        contentType: false,
                        processData: false,
                        beforeSend: function () {
                            e.setAttribute("data-kt-indicator", "on");
                            e.disabled = !0;
                            blockUI.block();
                            blockUI_profile.block();
                        },
                        success: function (result, status, xhr, $form) {
                            if (!result.error) {
                                Swal.fire({
                                    title: swalTitle,
                                    text: result.message,
                                    icon: "success",
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"}
                                }).then(function () {
                                    window.location.href = '/useraccess/user';
                                });
                            } else {
                                Swal.fire({
                                    title: swalTitle,
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    text: result.message,
                                    icon: "warning",
                                    confirmButtonText: "OK",
                                    customClass: {confirmButton: "btn btn-optima"}
                                });
                            }
                        },
                        complete: function () {
                            e.setAttribute("data-kt-indicator", "off");
                            e.disabled = !1;
                            blockUI.release();
                            blockUI_profile.release();
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(jqXHR.message);
                            if (jqXHR.status == 403) {
                                Swal.fire({
                                    title: swalTitle,
                                    text: jqXHR.responseJSON.message,
                                    icon: "warning",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    confirmButtonText: "OK",
                                    customClass: {confirmButton: "btn btn-optima"}
                                }).then(function () {
                                    window.location.href = '/login-page';
                                });
                            } else {
                                Swal.fire({
                                    title: swalTitle,
                                    text: "Error from ajax",
                                    icon: "error",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    confirmButtonText: "OK",
                                    customClass: {confirmButton: "btn btn-optima"}
                                });
                            }
                        }
                    });
                });
            }
        }
    });
});

const getData = (userid) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/useraccess/user/get-data/id",
            type: "GET",
            data: {userid: userid},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {
                    let value = result.data;
                    $('#email').val(value.email);
                    $('#username').val(value.userName);
                    $('#contactinfo').val(value.contactInfo);
                    dt_users_profile.rows.add(value.profiles).draw();
                }
            },
            complete: function(result) {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                return reject(jqXHR.responseText);
                console.log(jqXHR.responseText);
                if (jqXHR.status == 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR.responseJSON.message,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-optima" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
            },
        });
    });
}

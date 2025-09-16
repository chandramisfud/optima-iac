'use strict';

var dt_users;
var swalTitle = "User Management";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    let active = $('#filter_active').val();

    (active === "" || active == null) ? active = "ALL" : active = active;
    dt_users = $('#dt_users').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/useraccess/user/list/paginate/filter?active=' + active,
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 10,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    let popMenu;
                    if(full.status==='Active') {
                        popMenu = '\
                            <div class="me-0">\
                                <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                                </a>\
                                <div class="menu menu-sub menu-sub-dropdown w-180px w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                    <a class="dropdown-item text-start edit-record" href="/useraccess/user/form?method=update&userid=' + full.id + '"><i class="fa fa-edit fs-6"></i> Edit User</a>\
                                    <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Deactivate User</a>\
                                    <a class="dropdown-item text-start reset-password" href="#"><i class="fa fa-key fs-6"></i> Reset Password User</a>\
                                </div>\
                            </div>\
                        ';
                    }else{
                        popMenu = '\
                            <div class="me-0">\
                                <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                                </a>\
                                <div class="menu menu-sub menu-sub-dropdown w-180px w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                    <a class="dropdown-item text-start edit-record" href="/useraccess/user/form?method=update&userid=' + full.id + '"><i class="fa fa-edit fs-6"></i> Edit User</a>\
                                    <a class="dropdown-item text-start activate-record" href="#"><i class="fa fa-check fs-6"></i> Activate User</a>\
                                </div>\
                            </div>\
                        ';
                    }

                    return popMenu;
                }
            },
            {
                targets: 1,
                width: 240,
                title: 'Email',
                data: 'email',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                width: 240,
                title: 'User Name',
                data: 'userName',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                width: 240,
                title: 'Contact Info',
                data: 'contactInfo',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                width: 240,
                title: 'Status',
                data: 'status',
                className: "text-nowrap align-middle",
            },


        ],
        initComplete: function( settings, json ) {

        },
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
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            allowOutsideClick: false,
            allowEscapeKey: false
        });
    };

    $("#dt_users").on('click', '.delete-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_users.row(tr).data();
        checkFormAccess('delete_rec', trdata.id, '/useraccess/user/delete', "Are your sure to deactivate data " +trdata.userName, "Yes, deactivate it");
    });

    $("#dt_users").on('click', '.reset-password', function () {
        let tr = this.closest("tr");
        let trdata = dt_users.row(tr).data();
        $.ajax({
            url: "/check-form-access",
            type: "GET",
            data: {menuid: menu_id, access_name: 'update_rec'},
            dataType: "JSON",
            async: true,
            success: function (result) {
                if (!result.error) {
                    Swal.fire({
                        title: swalTitle,
                        text: "Are you sure to reset password user " + trdata.email + "?",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#AAAAAA',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        cancelButtonText: 'No, cancel',
                        confirmButtonText: 'Yes, reset it',
                        reverseButtons: true
                    }).then((result) => {
                        if (result.isConfirmed) {
                            let formData = new FormData();
                            formData.append('id', trdata.id);
                            $.get('/refresh-csrf').done(function(data) {
                                $('meta[name="csrf-token"]').attr('content', data)
                                $.ajaxSetup({
                                    headers: {
                                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                                    }
                                });
                                $.ajax({
                                    url: '/useraccess/user/reset-password',
                                    data: formData,
                                    type: 'POST',
                                    async: true,
                                    dataType: 'JSON',
                                    cache: false,
                                    contentType: false,
                                    processData: false,
                                    beforeSend: function () {
                                        blockUI.block();
                                    },
                                    success: function (result) {
                                        if (!result.error) {
                                            Swal.fire({
                                                title: 'Reset Password',
                                                text: result.message,
                                                icon: "success",
                                                confirmButtonText: "OK",
                                                allowOutsideClick: false,
                                                allowEscapeKey: false,
                                                customClass: {confirmButton: "btn btn-optima"}
                                            }).then(function () {
                                                dt_users.ajax.reload();
                                            });
                                        } else {
                                            Swal.fire({
                                                title: 'Reset Password',
                                                text: result.message,
                                                icon: "warning",
                                                buttonsStyling: !1,
                                                confirmButtonText: "OK",
                                                allowOutsideClick: false,
                                                allowEscapeKey: false,
                                                customClass: {confirmButton: "btn btn-optima"}
                                            });
                                        }
                                    },
                                    complete: function () {
                                        blockUI.release();
                                    },
                                    error: function (jqXHR, textStatus, errorThrown) {
                                        Swal.fire({
                                            title: 'Reset Password',
                                            text: textStatus,
                                            icon: "error",
                                            buttonsStyling: !1,
                                            confirmButtonText: "OK",
                                            allowOutsideClick: false,
                                            allowEscapeKey: false,
                                            customClass: {confirmButton: "btn btn-optima"}
                                        });
                                    }
                                });
                            });
                        }
                    })
                } else {
                    Swal.fire({
                        title: 'Reset Password',
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status == 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR.responseJSON.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-optima" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                reject(jqXHR.responseText);
                console.log(jqXHR.responseText);
            },
        });
    });

    $("#dt_users").on('click', '.activate-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_users.row(tr).data();
        $.ajax({
            url: "/check-form-access",
            type: "GET",
            data: {menuid: menu_id, access_name: 'update_rec'},
            dataType: "JSON",
            async: true,
            success: function (result) {
                if (!result.error) {
                    Swal.fire({
                        title: swalTitle,
                        text: "Are you sure to activate user " + trdata.email + "?",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#AAAAAA',
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        cancelButtonText: 'No, cancel',
                        confirmButtonText: 'Yes, activated it',
                        reverseButtons: true
                    }).then((result) => {
                        if (result.isConfirmed) {
                            let formData = new FormData();
                            formData.append('id', trdata.id);
                            $.get('/refresh-csrf').done(function(data) {
                                $('meta[name="csrf-token"]').attr('content', data)
                                $.ajaxSetup({
                                    headers: {
                                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                                    }
                                });
                                $.ajax({
                                    url: '/useraccess/user/activate',
                                    data: formData,
                                    type: 'POST',
                                    async: true,
                                    dataType: 'JSON',
                                    cache: false,
                                    contentType: false,
                                    processData: false,
                                    beforeSend: function () {
                                        blockUI.block();
                                    },
                                    success: function (result) {
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
                                                dt_users.ajax.reload();
                                            });
                                        } else {
                                            Swal.fire({
                                                title: swalTitle,
                                                text: result.message,
                                                icon: "warning",
                                                buttonsStyling: !1,
                                                confirmButtonText: "OK",
                                                allowOutsideClick: false,
                                                allowEscapeKey: false,
                                                customClass: {confirmButton: "btn btn-optima"}
                                            });
                                        }
                                    },
                                    complete: function () {
                                        blockUI.release();
                                    },
                                    error: function (jqXHR, textStatus, errorThrown) {
                                        Swal.fire({
                                            title: swalTitle,
                                            text: textStatus,
                                            icon: "error",
                                            buttonsStyling: !1,
                                            confirmButtonText: "OK",
                                            allowOutsideClick: false,
                                            allowEscapeKey: false,
                                            customClass: {confirmButton: "btn btn-optima"}
                                        });
                                    }
                                });
                            });
                        }
                    })
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status == 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR.responseJSON.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-optima" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                reject(jqXHR.responseText);
                console.log(jqXHR.responseText);
            },
        });
    });
});

$('#dt_users_search').on('keyup', function () {
    dt_users.search(this.value).draw();
});

$('#dt_users_view').on('click', function() {
    let active = $('#filter_active').val();
    if (active === "" || active == null) active = "ALL";
    let url = '/useraccess/user/list/paginate/filter?active=' + active;
    let e = document.getElementById('dt_users_view');
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    dt_users.clear().draw();
    dt_users.ajax.url(url).load(function () {
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
    });
});

$('#btn_create').on('click', function() {
    checkFormAccess('create_rec', "", '/useraccess/user/form', "");
});

$('#btn_export_excel').on('click', function() {
    let active = $('#filter_active').val();
    if (active === "" || active == null) active = "ALL";
    let url =  "/useraccess/user/export-xls?active=" + active;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const fDeleteRecord = (id) => {
    let formData = new FormData();
    formData.append('id', id);
    $.get('/refresh-csrf').done(function(data) {
        $('meta[name="csrf-token"]').attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            }
        });
        $.ajax({
            url: '/useraccess/user/delete',
            data: formData,
            type: 'POST',
            async: true,
            dataType: 'JSON',
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                blockUI.block();
            },
            success: function (result) {
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
                        dt_users.ajax.reload();
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete: function () {
                blockUI.release();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                Swal.fire({
                    title: swalTitle,
                    text: textStatus,
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        });
    });
}

'use strict';

var dt_userprofile;
var swalTitle = "User Profile";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getUserGroup();

    let usergroupid = $('#filter_usergroup').val();
    let userlevel = $('#filter_usergrouprights').val();
    let active = $('#filter_active').val();

    if (usergroupid === "" || usergroupid == null) usergroupid = "";
    if (userlevel === "" || userlevel == null) userlevel = "";
    if (active === "")  active = 'ALL';

    dt_userprofile = $('#dt_userprofile').DataTable({
        dom:
        // "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/useraccess/profile/list/paginate/filter?usergroupid=' + usergroupid + '&userlevel=' + userlevel + '&status=' + active,
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
                    if(full.status=='Active') {
                        popMenu = '\
                            <div class="me-0">\
                                <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                                </a>\
                                <div class="menu menu-sub menu-sub-dropdown w-180px w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                    <a class="dropdown-item text-start edit-record" href="/useraccess/profile/form?method=update&profileid=' + data + '"><i class="fa fa-edit fs-6"></i> Edit Profile</a>\
                                    <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Deactivate Profile</a>\
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
                                    <a class="dropdown-item text-start edit-record" href="/useraccess/profile/form?method=update&profileid=' + data + '"><i class="fa fa-edit fs-6"></i> Edit Profile</a>\
                                    <a class="dropdown-item text-start activate-record" href="#"><i class="fa fa-check fs-6"></i> Activate Profile</a>\
                                </div>\
                            </div>\
                        ';
                    }
                    return popMenu;
                }
            },
            {
                targets: 1,
                width: 100,
                title: "Profile ID",
                data: 'id',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                width: 150,
                title: "Profile Name",
                data: 'username',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                width: 250,
                title: "Email",
                data: 'email',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                width: 100,
                title: "Department",
                data: 'department',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                width: 400,
                title: "Job Title",
                data: 'jobtitle',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: "User Group Menu",
                data: 'usergroupname',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: "User Group Rights",
                data: 'levelname',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: "Category",
                data: 'profileCategory',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: "Status",
                data: 'status',
                className: 'text-nowrap align-middle',
            }
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $('#dt_userprofile_search').on('keyup', function () {
        dt_userprofile.search(this.value).draw();
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
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,

        });
    };

    $("#dt_userprofile").on('click', '.delete-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_userprofile.row(tr).data();
        checkFormAccess('delete_rec', trdata.id, '', "Are your sure to deactivate data " + trdata.id, "Yes, deactivate it")
    });

    $("#dt_userprofile").on('click', '.activate-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_userprofile.row(tr).data();
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
                        text: "Are you sure to activate profile " + trdata.id + "?",
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
                                    url: '/useraccess/profile/activate',
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
                                                dt_userprofile.ajax.reload();
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

$('#btn_create').on('click', function() {
    checkFormAccess('create_rec', '', '/useraccess/profile/form', '');
});

$('#dt_userprofile_view').on('click', function() {
    let usergroupid = $('#filter_usergroup').val();
    let userlevel = $('#filter_usergrouprights').val();
    let active = $('#filter_active').val();

    if (usergroupid === "" || usergroupid == null) usergroupid = "";
    if (userlevel === "" || userlevel == null) userlevel = "";
    if (active === "")  active = 'ALL';

    let url =  '/useraccess/profile/list/paginate/filter?usergroupid=' + usergroupid + '&userlevel=' + userlevel + '&status=' + active;
    let e = document.getElementById('dt_userprofile_view');
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    dt_userprofile.clear().draw();
    dt_userprofile.ajax.url(url).load(function () {
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let usergroupid = $('#filter_usergroup').val();
    let userlevel = $('#filter_usergrouprights').val();
    let active = $('#filter_active').val();

    if (usergroupid === "" || usergroupid == null) usergroupid = "";
    if (userlevel === "" || userlevel == null) userlevel = "";
    if (active === "")  active = 'ALL';

    let url = '/useraccess/profile/export-xls?usergroupid=' + usergroupid + '&userlevel=' + userlevel + '&status=' + active;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#filter_usergroup').select2().on('change', async function(e) {
    let usergroupid = $(this).val();
    blockUI.block();
    $('#filter_usergrouprights').empty();
    if ($(this).val() !== "") await getUserRights(usergroupid);
    blockUI.release();
    $('#filter_usergrouprights').val('').trigger('change');
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
            url: '/useraccess/profile/delete',
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
                        confirmButtonText: "Ok",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_userprofile.ajax.reload();
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
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
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        });
    });
}

const getUserGroup = () => {
    $.ajax({
        url         : "/useraccess/profile/usergroupmenu",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j){
                data.push({
                    id: result.data[j].usergroupid,
                    text: result.data[j].usergroupname
                });
            }
            $('#filter_usergroup').select2({
                placeholder: "Select a User Group Menu",
                width: '100%',
                data: data
            });
        },
        complete: function() {

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(jqXHR.responseText);
        }
    });
}

const getUserRights = (usergroupid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/useraccess/profile/usergrouprights/usergroupid",
            type        : "GET",
            dataType    : 'json',
            data        : {usergroupid: usergroupid},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].userlevelid,
                        text: result.data[j].userlevelname
                    });
                }
                $('#filter_usergrouprights').select2({
                    placeholder: "Select a User Group Rights",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

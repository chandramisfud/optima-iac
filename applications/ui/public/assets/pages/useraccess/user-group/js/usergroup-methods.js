'use strict';

var dt_usergroup;
var swalTitle = "User Group Menu";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_usergroup = $('#dt_usergroup').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/useraccess/group-menu/list/paginate/filter',
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
                data: 'userGroupId',
                width: 10,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-200px w-md-200px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start groupmenu-config" href="/useraccess/group-menu/form-groupmenu-config?method=update&usergroupid=' + data + '"><i class="fa fa-bezier-curve fs-6"></i> User Group Menu Config</a>\
                                <a class="dropdown-item text-start groupmenu-rights" href="/useraccess/group-menu/form-grouprights-config?method=update&usergroupid=' + data + '"><i class="fa fa-users-cog fs-6"></i> User Group Rights Config</a>\
                                <a class="dropdown-item text-start edit-record" href="/useraccess/group-menu/form?method=update&usergroupid=' + data + '"><i class="fa fa-edit fs-6"></i> Edit Data</a>\
                                <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Delete Data</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'ID',
                data: 'userGroupId',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Name',
                data: 'userGroupName',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Group Menu Permission',
                data: 'name',
                className: 'text-nowrap align-middle',
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
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,

        });
    };

    $("#dt_usergroup").on('click', '.delete-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_usergroup.row(tr).data();
        checkFormAccess('delete_rec', trdata.userGroupId, '', "Are your sure to delete data " + trdata.userGroupId)
    });
});

$('#dt_usergroup_Search').on('keyup', function () {
    dt_usergroup.search(this.value).draw();
});

$('#btn_create').on('click', function() {
    checkFormAccess('create_rec', '', '/useraccess/group-menu/form', '')
});

const fDeleteRecord = (userGroupId) => {
    let formData = new FormData();
    formData.append('usergroupid', userGroupId);
    $.get('/refresh-csrf').done(function(data) {
        $('meta[name="csrf-token"]').attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            }
        });
        $.ajax({
            url: '/useraccess/group-menu/delete',
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
                        dt_usergroup.ajax.reload();
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

$('#btn_export_excel').on('click', function() {
    let e = document.querySelector("#btn_export_excel");
    let url = "/useraccess/group-menu/export-xls"
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

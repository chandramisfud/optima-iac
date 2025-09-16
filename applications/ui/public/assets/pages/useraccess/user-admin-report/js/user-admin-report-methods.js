'use strict';

var dt_user_admin_report;
var swalTitle = "User Administration Report";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_user_admin_report = $('#dt_user_admin_report').DataTable({
        dom:
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/useraccess/user-admin-report/list/paginate/filter',
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        ordering: false,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                width: 100,
                title: "User ID",
                data: 'id',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                width: 150,
                title: "User Name",
                data: 'username',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                width: 250,
                title: "Email",
                data: 'email',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                width: 100,
                title: "Department",
                data: 'department',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                width: 400,
                title: "Job Title",
                data: 'jobtitle',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                width: 150,
                title: "Contact Info",
                data: 'contactinfo',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: "Distributor",
                width: 100,
                data: 'distributorid',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                width: 100,
                title: "User Group ID",
                data: 'usergroupid',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                width: 100,
                title: "User Group Menu",
                data: 'usergroupname',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                width: 100,
                title: "User Level",
                data: 'userlevel',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 10,
                width: 100,
                title: "User Menu Level",
                data: 'levelname',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 11,
                width: 50,
                title: "Menu ID",
                data: 'menuid',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 12,
                width: 100,
                title: "Menu",
                data: 'menu',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 13,
                width: 100,
                title: "Sub Menu",
                data: 'submenu',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 14,
                width: 20,
                title: "Flag",
                data: 'flag',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 15,
                width: 20,
                title: "CRUD",
                data: 'crud',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 16,
                width: 20,
                title: "Approve",
                data: 'approve',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 17,
                width: 20,
                title: "Create",
                data: 'create_rec',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 18,
                width: 20,
                title: "Read",
                data: 'read_rec',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 19,
                width: 20,
                title: "Update",
                data: 'update_rec',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 20,
                width: 20,
                title: "Delete",
                data: 'delete_rec',
                className: 'text-nowrap align-middle',
            }
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        console.log(helpPage)
        if (strmessage === "") strmessage = "Please contact your vendor"
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

});

$('#dt_user_admin_report_search').on('keyup', function () {
    dt_user_admin_report.search(this.value).draw();
});

$('#btn_export_excel').on('click', function() {
    let search = $('#dt_user_admin_report_search').val();

    if(search=="" || search==null) { search = ""; }

    let url = '/useraccess/user-admin-report/export-xls?search=' + search;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

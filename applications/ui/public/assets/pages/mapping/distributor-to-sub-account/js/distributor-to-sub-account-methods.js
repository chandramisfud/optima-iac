'use strict';

var dt_distributor_to_sub_account;
var swalTitle = "Mapping Distributor to Sub Account";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_distributor_to_sub_account = $('#dt_distributor_to_sub_account').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/mapping/distributor-to-sub-account/list/paginate/filter',
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
                    return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-200px w-md-200px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Remove Mapping</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'Distributor',
                data: 'distributor',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Channel',
                data: 'channel',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Sub Channel',
                data: 'subChannel',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Account',
                data: 'account',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Sub Account',
                data: 'subAccount',
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

    $("#dt_distributor_to_sub_account").on('click', '.delete-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_distributor_to_sub_account.row(tr).data();
        checkFormAccess('delete_rec', trdata.id, '', "Are your sure to remove mapping " + trdata.distributor + "-" + trdata.subAccount, 'Yes, remove it');
    });
});

$('#dt_distributor_to_sub_account_search').on('keyup', function () {
    dt_distributor_to_sub_account.search(this.value).draw();
});

$('#btn_create').on('click', function() {
    checkFormAccess('create_rec', '', '/mapping/distributor-to-sub-account/form', '');
});

$('#btn_export_excel').on('click', function() {
    let url = "/mapping/distributor-to-sub-account/export-xls";
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
    $.ajax({
        url: '/mapping/distributor-to-sub-account/delete',
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
                    dt_distributor_to_sub_account.ajax.reload();
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
}

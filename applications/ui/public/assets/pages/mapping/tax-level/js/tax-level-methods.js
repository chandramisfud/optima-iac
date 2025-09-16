'use strict';

var dt_tax_level;
var swalTitle = "Mapping Material Tax Level";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_tax_level = $('#dt_tax_level').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/mapping/tax-level/list/paginate/filter',
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
                            <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Remove Mapping</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'Material Number',
                data: 'materialNumber',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Description',
                data: 'description',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'WHT Type',
                data: 'whT_Type',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'WHT Code',
                data: 'whT_Code',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Purpose',
                data: 'purpose',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Entity',
                data: 'entity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'PPN (%)',
                data: 'ppnPct',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 2,".", ",");
                }
            },
            {
                targets: 8,
                title: 'PPH (%)',
                data: 'pphPct',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 2,".", ",");
                }
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

    $("#dt_tax_level").on('click', '.delete-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_tax_level.row(tr).data();
        checkFormAccess('delete_rec', trdata.id, '', "Are your sure to remove mapping " + trdata.materialNumber, 'Yes, remove it');
    });
});

$('#dt_tax_level_search').on('keyup', function () {
    dt_tax_level.search(this.value).draw();
});

$('#btn_create').on('click', function() {
    checkFormAccess('create_rec', '', '/mapping/tax-level/form', '');
});

$('#btn_export_excel').on('click', function() {
    let url = "/mapping/tax-level/export-xls";
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
        url: '/mapping/tax-level/delete',
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
                    dt_tax_level.ajax.reload();
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

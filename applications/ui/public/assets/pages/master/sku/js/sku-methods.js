'use strict';

var dt_sku, skuid;
var swalTitle = "Master SKU";
var heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_sku = $('#dt_sku').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row tes'<'col-sm-1'l><'col-sm-7'i><'col-sm-4'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/master/sku/list/paginate/filter',
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'productId',
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
                            <div class="menu menu-sub menu-sub-dropdown w-125px w-md-125px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start edit-record" href="/master/sku/form?method=update&skuid=' + data + '"><i class="fa fa-edit fs-6"></i> Edit Data</a>\
                                <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Delete Data</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'SKU ID',
                data: 'productRefId',
                className: 'text-nowrap align-middle',
                width: 150
            },
            {
                targets: 2,
                title: 'Entity',
                data: 'entityLongDesc',
                className: 'text-nowrap align-middle',
                width: 200
            },
            {
                targets: 3,
                title: 'Brand',
                data: 'brandLongDesc',
                className: 'text-nowrap align-middle',
                width: 150
            },
            {
                targets: 4,
                title: 'Short Desc',
                data: 'productShortDesc',
                className: 'text-nowrap align-middle',
                width: 150
            },
            {
                targets: 5,
                title: 'Long Desc',
                data: 'productLongDesc',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $('#dt_sku_Search').on('keyup', function () {
        dt_sku.search(this.value).draw();
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
});

$('#filter_collapsible').on('hidden.bs.collapse', function () {
    $('div.dataTables_scrollBody').height( "63vh" );
})

$('#filter_collapsible').on('shown.bs.collapse', function () {
    $('div.dataTables_scrollBody').height( "55vh" );
})

$('#btn_create').on('click', function() {
    checkFormAccess('create_rec', '', '/master/sku/form', '')
});

$("#dt_sku").on('click', '.delete-record', function () {
    let tr = this.closest("tr");
    let trdata = dt_sku.row(tr).data();
    checkFormAccess('delete_rec', trdata.productId, '', "Are your sure to delete data " + trdata.productRefId)
});

const fDeleteRecord = (skuid) => {
    let formData = new FormData();
    formData.append('id', skuid);
    $.ajax({
        url: '/master/sku/delete',
        data: formData,
        type: 'POST',
        async: true,
        dataType: 'JSON',
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            if (!result.error) {
                Swal.fire({
                    title: swalTitle,
                    text: result.message,
                    icon: "success",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    dt_sku.ajax.reload();
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
}

$('#btn_export_excel').on('click', function() {
    let a = document.createElement("a");
    a.href = "/master/sku/export-xls";
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

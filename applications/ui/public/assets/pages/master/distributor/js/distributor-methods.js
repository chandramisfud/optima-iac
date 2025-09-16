'use strict';

var dt_distributor;
var swalTitle = "Master Distributor";
var heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_distributor = $('#dt_distributor').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/master/distributor/list/paginate/filter',
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
                            <div class="menu menu-sub menu-sub-dropdown w-125px w-md-125px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start edit-record" href="/master/distributor/form?method=update&distributorid=' + data +'"><i class="fa fa-edit fs-6"></i> Edit Data</a>\
                                <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Delete Data</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'Distributor ID',
                data: 'refId',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Company Name',
                data: 'companyName',
                orderable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Short Desc',
                data: 'shortDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Long Desc',
                data: 'longDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'NPWP',
                data: 'npwp',
                orderable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Address',
                data: 'address',
                orderable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Claim Manager',
                data: 'claimManager',
                orderable: false,
                className: 'text-nowrap align-middle',
            },

        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $('#dt_distributor_Search').on('keyup', function () {
        dt_distributor.search(this.value).draw();
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "Ok",
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
    checkFormAccess('create_rec', '', '/master/distributor/form', '')
});

$("#dt_distributor").on('click', '.delete-record', function () {
    let tr = this.closest("tr");
    let trdata = dt_distributor.row(tr).data();
    checkFormAccess('delete_rec', trdata.id, '', "Are your sure to delete data " + trdata.refId)
});

const fDeleteRecord = (distributorid) => {
    let formData = new FormData();
    formData.append('id', distributorid);
    $.ajax({
        url: '/master/distributor/delete',
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
                    confirmButtonText: "Ok",
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    dt_distributor.ajax.reload();
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
    a.href = "/master/distributor/export-xls";
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

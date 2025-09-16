'use strict';

var dt_investment_type;
var swalTitle = "Investment Type";
var heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_investment_type = $('#dt_investment_type').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/master/investment-type/list/paginate/filter',
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
                    if (full.isDeleted === 1) {
                        return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-200px w-md-200px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start edit-record" href="/master/investment-type/form?method=update&investmenttypeid=' + data + '"><i class="fa fa-edit fs-6"></i> Edit Investment Type</a>\
                                <a class="dropdown-item text-start activate-record" href="#"><i class="fa fa-trash fs-6"></i> Activate Investment Type</a>\
                            </div>\
                        </div>\
                    ';
                    } else {
                        return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-200px w-md-200px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start edit-record" href="/master/investment-type/form?method=update&investmenttypeid=' + data + '"><i class="fa fa-edit fs-6"></i> Edit Investment Type</a>\
                                <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Deactivate Investment Type</a>\
                            </div>\
                        </div>\
                    ';
                    }

                }
            },
            {
                targets: 1,
                title: 'Investment Type Code',
                data: 'refId',
                className: 'text-nowrap align-middle',
                width: 150
            },
            {
                targets: 2,
                title: 'Investment Type',
                data: 'longDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Status',
                data: 'isDeleted',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data === 1){
                        return 'Inactive';
                    } else {
                        return 'Active';
                    }
                }
            },
            {
                targets: 4,
                title: 'Status Update On',
                data: 'modifiedOn',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (full.isDeleted === 1){
                        if(full.deleteOn == null || full.deleteOn == ''){
                            return '';
                        } else {
                            return formatDate(full.deleteOn);
                        }
                    } else {
                        if(full.modifiedOn == null || full.modifiedOn == ''){
                            return '';
                        } else {
                            return formatDate(full.modifiedOn);
                        }
                    }
                }
            },
            {
                targets: 5,
                title: 'Status Update By',
                data: 'modifiedBy',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (full.isDeleted === 1){
                        if(full.deleteBy == null || full.deleteBy == ''){
                            return '';
                        } else {
                            return full.deleteBy;
                        }
                    } else {
                        if(full.modifiedBy == null || full.modifiedBy == ''){
                            return '';
                        } else {
                            return full.modifiedBy;
                        }
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $('#dt_investment_type_Search').on('keyup', function () {
        dt_investment_type.search(this.value).draw();
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
    checkFormAccess('create_rec', '', '/master/investment-type/form', '')
});

$('#btn_mapping').on('click', function() {
    checkFormAccess('create_rec', '', '/master/investment-type/mapping', '')
});

$("#dt_investment_type").on('click', '.delete-record', function () {
    let tr = this.closest("tr");
    let trdata = dt_investment_type.row(tr).data();
    checkFormAccess('delete_rec', trdata.id, '', "Are your sure to deactivate data " + trdata.refId, "Yes, deactivate", "deactivate")
});

$("#dt_investment_type").on('click', '.activate-record', function () {
    let tr = this.closest("tr");
    let trdata = dt_investment_type.row(tr).data();
    checkFormAccess('delete_rec', trdata.id, '', "Are your sure to activate data " + trdata.refId, "Yes, activate", "activate")
});

const fDeactivateRecord = (id) => {
    let formData = new FormData();
    formData.append('id', id);
    $.ajax({
        url: '/master/investment-type/deactivate',
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
                    dt_investment_type.ajax.reload();
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

const fActivateRecord = (id) => {
    let formData = new FormData();
    formData.append('id', id);
    $.ajax({
        url: '/master/investment-type/activate',
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
                    dt_investment_type.ajax.reload();
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
    a.href = "/master/investment-type/export-xls";
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

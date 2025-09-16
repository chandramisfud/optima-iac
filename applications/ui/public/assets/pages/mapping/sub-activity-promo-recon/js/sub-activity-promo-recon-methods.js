'use strict';

var dt_sub_activity_promo_recon;
var swalTitle = "Mapping Sub Activity Promo Recon";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_sub_activity_promo_recon = $('#dt_sub_activity_promo_recon').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/mapping/sub-activity-promo-recon/list/paginate/filter',
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
                                <a class="dropdown-item text-start edit-record" href="/mapping/sub-activity-promo-recon/form?method=update&id=' + data + '"><i class="fa fa-edit fs-6"></i> Edit Data</a>\
                                <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Delete Data</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'Sub Activity ID',
                data: 'refid',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Category',
                data: 'category',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Sub Category',
                data: 'subCategory',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Activity',
                data: 'activity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Type',
                data: 'subActivityType',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Sub Activity',
                data: 'subActivity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Action',
                data: 'allowEdit',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data === 1) {
                        return "During promo period: allow edit";
                    } else {
                        return "During promo period: not allow edit (still allow cancel)";
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

    $("#dt_sub_activity_promo_recon").on('click', '.delete-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_sub_activity_promo_recon.row(tr).data();
        checkFormAccess('delete_rec', trdata.id, '', "Are your sure to remove mapping " + trdata.refid, 'Yes, remove it');
    });
});

$('#dt_sub_activity_promo_recon_search').on('keyup', function () {
    dt_sub_activity_promo_recon.search(this.value).draw();
});

$('#btn_create').on('click', function() {
    checkFormAccess('create_rec', '', '/mapping/sub-activity-promo-recon/form', '');
});

$('#btn_export_excel').on('click', function() {
    let url = "/mapping/sub-activity-promo-recon/export-xls";
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_download').on('click', function() {
    let url = "/mapping/sub-activity-promo-recon/download-template";
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#dt_sub_activity_promo_recon_view').on('keyup', function () {
    dt_sub_activity_promo_recon.ajax.reload();
});

$('#btn-upload').on('click', function() {
    let url = '/mapping/sub-activity-promo-recon/upload-form';
    window.location.href = url;
});

const fDeleteRecord = (id) => {
    let formData = new FormData();
    formData.append('id', id);
    $.ajax({
        url: '/mapping/sub-activity-promo-recon/delete',
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
                    dt_sub_activity_promo_recon.ajax.reload();
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

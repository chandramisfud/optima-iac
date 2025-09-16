'use strict';

var dt_wht_type;
var swalTitle = "Mapping WHT Type";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListDistributor()
    getListSubActivity();
    getListSubAccount();
    getListWHTType();

    dt_wht_type = $('#dt_wht_type').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/mapping/wht-type/list/paginate/filter',
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
                render: function () {
                    return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6"></i> Remove Mapping</a>\
                            </div>\
                            <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start edit-record" href="#"><i class="fa fa-edit fs-6"></i> Edit Mapping</a>\
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
                title: 'Sub Activity',
                data: 'subActivity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Sub Account',
                data: 'subAccount',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'WHT Type',
                data: 'WHTType',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Modified On',
                data: 'modifiedOn',
                className: 'text-nowrap align-middle',
                render: function (data) {
                    return formatDateTime(data);
                }
            },
            {
                targets: 6,
                title: 'Modified By',
                data: 'modifiedBy',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
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

    $("#dt_wht_type").on('click', '.delete-record', function () {
        let tr = this.closest("tr");
        let trData = dt_wht_type.row(tr).data();
        checkFormAccess('delete_rec', trData.id, '', "Are your sure to remove mapping " + trData.distributor + "-" + trData.subActivity + "-" + trData.subAccount + "-" + trData.WHTType, 'Yes, remove it');
    });

    $("#dt_wht_type").on('click', '.edit-record', function () {
        let tr = this.closest("tr");
        let trData = dt_wht_type.row(tr).data();

        let url = '/mapping/wht-type/form?method=update&id=' + trData.id;
        checkFormAccess('update_rec', '', url, '')
    });

});

$('#dt_wht_type_search').on('keyup', function () {
    dt_wht_type.search(this.value).draw();
});

$('#btn_create').on('click', function() {
    checkFormAccess('create_rec', '', '/mapping/wht-type/form', '');
});

$('#btn-upload').on('click', function() {
    window.location.href = "/mapping/wht-type/upload-form";
});

$('#dt_wht_type_view').on('click', function (){
    let btn = document.getElementById('dt_wht_type_view');
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_subactivity = ($('#filter_subactivity').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";
    let filter_whttype = ($('#filter_whttype').val()) ?? "";

    let url = "/mapping/wht-type/list/paginate/filter?distributor=" + filter_distributor + "&subActivity=" + filter_subactivity + "&subAccount=" + filter_subaccount + "&WHTType=" + filter_whttype;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_wht_type.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_subactivity = ($('#filter_subactivity').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";
    let filter_whttype = ($('#filter_whttype').val()) ?? "";
    let url = "/mapping/wht-type/export-xls?distributor=" + filter_distributor + "&subActivity=" + filter_subactivity + "&subAccount=" + filter_subaccount + "&WHTType=" + filter_whttype;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_download').on('click', function() {
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_subactivity = ($('#filter_subactivity').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";
    let filter_whttype = ($('#filter_whttype').val()) ?? "";
    let url = "/mapping/wht-type/download-template?distributor=" + filter_distributor + "&subActivity=" + filter_subactivity + "&subAccount=" + filter_subaccount + "&WHTType=" + filter_whttype;
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
        url: '/mapping/wht-type/delete',
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
                    dt_wht_type.ajax.reload();
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
            console.log(errorThrown)
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

const getListDistributor = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/mapping/wht-type/list/distributor",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                $('#filter_distributor').select2({
                    width: '100%',
                    data: result.data
                });
                resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    });
}

const getListSubActivity = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/mapping/wht-type/list/sub-activity",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                $('#filter_subactivity').select2({
                    width: '100%',
                    data: result.data
                });
                resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    });
}

const getListSubAccount = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/mapping/wht-type/list/sub-account",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                $('#filter_subaccount').select2({
                    width: '100%',
                    data: result.data
                });
                resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    });
}

const getListWHTType = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/mapping/wht-type/list/wht-type",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                $('#filter_whttype').select2({
                    width: '100%',
                    data: result.data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    });
}

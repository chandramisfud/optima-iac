'use strict';

var validator, dt_roi_cr;
var swalTitle = "ROI & CR Configuration";
heightContainer = 280;

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    getCategory();

    dt_roi_cr = $('#dt_roi_cr').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/configuration/roi-cr/list',
            type: 'get',
        },
        processing: true,
        serverSide: false,
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
                    let popMenu= '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-180px w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start delete-record" href="#"><i class="fa fa-trash fs-6 me-1"></i> Delete Data</a>\
                            </div>\
                        </div>\
                    ';
                    return popMenu;
                }
            },
            {
                targets: 1,
                title: 'Sub Activity Id',
                data: 'refId',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Sub Activity',
                data: 'longDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Minimum ROI (%)',
                data: 'minimumROI',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 4,
                title: 'Maximum ROI (%)',
                data: 'maksimumROI',
                className: "text-nowrap align-middle text-end",
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 5,
                title: 'Minimum Cost Ratio (%)',
                data: 'minimumCostRatio',
                className: "text-nowrap align-middle text-end",
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 6,
                title: 'Maximum Cost Ratio (%)',
                data: 'maksimumCostRatio',
                className: "text-nowrap align-middle text-end",
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
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
            allowOutsideClick: false,
            allowEscapeKey: false
        });
    };

    $("#dt_roi_cr").on('click', '.delete-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_roi_cr.row(tr).data();
        checkFormAccess('delete_rec', trdata.id, '', "Are your sure to delete data " + trdata.refId + "?");
    });
});

$('#dt_roi_cr_search').on('keyup', function() {
    dt_roi_cr.search(this.value).draw();
});

$('#btn_config').on('click', function () {
    checkFormAccess('create_rec', "", '/configuration/roi-cr/form', "");
});

$('#filter_category').on('change', async function () {
    blockUI.block();
    $('#filter_sub_category').empty();
    if ($(this).val() !== "") await getSubCategory($(this).val());
    blockUI.release();
    $('#filter_sub_category').val('').trigger('change');
});

$('#filter_sub_category').on('change', async function () {
    blockUI.block();
    $('#filter_activity').empty();
    if ($(this).val() !== "") await getActivity($(this).val());
    blockUI.release();
    $('#filter_activity').val('').trigger('change');
});

$('#dt_roi_cr_view').on('click', function (){
    let e = document.getElementById('dt_roi_cr_view');
    let categoryId = ($('#filter_category').val()) ?? "";
    let subCategoryId = ($('#filter_sub_category').val()) ?? "";
    let activityId = ($('#filter_activity').val()) ?? "";
    let url = "/configuration/roi-cr/list?categoryId=" + categoryId + "&subCategoryId=" + subCategoryId + "&activityId=" + activityId;
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    dt_roi_cr.ajax.url(url).load(function () {
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
    });
});

const fDeleteRecord = (id) => {
    let formData = new FormData();
    formData.append('id', id);
    $.ajax({
        url: '/configuration/roi-cr/delete',
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
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    dt_roi_cr.ajax.reload();
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

const getCategory = () => {
    $.ajax({
        url         : "/configuration/roi-cr/list/category",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j){
                data.push({
                    id: result.data[j].id,
                    text: result.data[j].longDesc
                });
            }
            $('#filter_category').select2({
                placeholder: "Select a Category",
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

const getSubCategory = (categoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/roi-cr/list/sub-category/category-id",
            type        : "GET",
            dataType    : 'json',
            data        : {categoryId: categoryId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_sub_category').select2({
                    placeholder: "Select a Sub Category",
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

const getActivity = (subCategoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/roi-cr/list/activity/subcategory-id",
            type        : "GET",
            dataType    : 'json',
            data        : {subCategoryId: subCategoryId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_activity').select2({
                    placeholder: "Select an Activity",
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

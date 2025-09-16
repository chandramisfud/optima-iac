'use strict';

var dt_mapping, categoryId, subCategoryId, activityId, subActivityId;
var swalTitle = "Investment Type Mapping";
var heightContainer = 335;

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    getInvestmentType();
    getCategory();

    dt_mapping = $('#dt_mapping').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/master/investment-type/mapping/list',
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'subactivityId',
                width: 50,
                searchable: false,
                className: 'text-nowrap dt-body-start',
                checkboxes: {
                    'selectRow': false
                }
            },
            {
                targets: 1,
                title: 'Category',
                data: 'category',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Activity',
                data: 'activity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Sub Activity Id',
                data: 'subactivityId',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Sub Activity',
                data: 'subactivity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Investment Type Code',
                data: 'investmentTypeCode',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Investment Type',
                data: 'investmentType',
                className: "text-nowrap align-middle",
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if (strmessage === "") strmessage = "Please contact your vendor"
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
});

$('#dt_mapping_search').on('keyup', function () {
    dt_mapping.search(this.value).draw();
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

$('#filter_activity').on('change', async function () {
    blockUI.block();
    $('#filter_sub_activity').empty();
    if ($(this).val() !== "") await getSubActivity($(this).val());
    blockUI.release();
    $('#filter_sub_activity').val('').trigger('change');
});

$('#dt_mapping_view').on('click', function () {
    let e = document.getElementById('dt_mapping_view');
    let categoryId = ($('#filter_category').val()) ?? "";
    let subCategoryId = ($('#filter_sub_category').val()) ?? "";
    let activityId = ($('#filter_activity').val()) ?? "";
    let subActivityId = ($('#filter_sub_activity').val()) ?? "";
    let url = "/master/investment-type/mapping/list?categoryId=" + categoryId + "&subCategoryId=" + subCategoryId + "&activityId=" + activityId + "&subActivityId=" + subActivityId;
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    $('#btn_mapping_submit').attr('disabled', true);
    $('#btn_mapping_remove').attr('disabled', true);
    dt_mapping.ajax.url(url).load(function () {
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
        $('#btn_mapping_submit').attr('disabled', false);
        $('#btn_mapping_remove').attr('disabled', false);
    });
});

$('#btn_back').on('click', function () {
    window.location.href = "/master/investment-type";
});

$('#btn_mapping_submit').on('click', function () {
    let e = document.querySelector("#btn_mapping_submit");
    if ($('#investmenttype').val() != "") {
        let dataRowsChecked = [];
        var rows_selected = dt_mapping.column(0).checkboxes.selected();

        $.each(rows_selected, function (index, value) {
            dataRowsChecked.push({
                subActivityId: value,
                investmentTypeId: $('#investmenttype').val(),
            });
        });

        if (dataRowsChecked.length > 0) {
            let formData = new FormData();
            formData.append('investmentMap', JSON.stringify(dataRowsChecked));
            let url = "/master/investment-type/mapping/save";
            $.ajax({
                url: url,
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function () {
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                },
                success: function (result, status, xhr, $form) {
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
                            window.location.href = '/master/investment-type/mapping';
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "error",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                },
                complete: function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.message)
                    Swal.fire({
                        title: swalTitle,
                        text: "Failed to save data, an error occurred in the process",
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        } else {
            Swal.fire({
                title: swalTitle,
                text: "Please select one or more items",
                icon: "warning",
                buttonsStyling: !1,
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please select Investment Type",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

$('#btn_mapping_remove').on('click', function () {
    let e = document.querySelector("#btn_mapping_remove");
    let dataRowsChecked = [];
    var rows_selected = dt_mapping.column(0).checkboxes.selected();

    $.each(rows_selected, function (index, value) {
        dataRowsChecked.push({
            subActivityId: value,
            investmentTypeId: 0,
        });
    });

    if (dataRowsChecked.length > 0) {
        let formData = new FormData();
        formData.append('investmentMap', JSON.stringify(dataRowsChecked));
        let url = "/master/investment-type/mapping/remove";
        $.ajax({
            url: url,
            data: formData,
            type: 'POST',
            async: true,
            dataType: 'JSON',
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
            },
            success: function (result, status, xhr, $form) {
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
                        window.location.href = '/master/investment-type/mapping';
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete: function () {
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.message)
                Swal.fire({
                    title: swalTitle,
                    text: "Failed to save data, an error occurred in the process",
                    icon: "error",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        });
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please select one or more items",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

const getInvestmentType = () => {
    $.ajax({
        url: "/master/investment-type/mapping/get-list/investment-type",
        type: "GET",
        dataType: 'json',
        async: true,
        success: function (result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j) {
                data.push({
                    id: result.data[j].id,
                    text: result.data[j].refId + " - " + result.data[j].longDesc
                });
            }
            $('#investmenttype').select2({
                placeholder: "Select a Investment Type",
                width: '100%',
                data: data
            });
        },
        complete: function () {

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.responseText);
            return reject(jqXHR.responseText);
        }
    });
}

const getCategory = () => {
    $.ajax({
        url: "/master/investment-type/mapping/get-list/category",
        type: "GET",
        dataType: 'json',
        async: true,
        success: function (result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j) {
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
        complete: function () {

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.responseText);
            return reject(jqXHR.responseText);
        }
    });
}

const getSubCategory = (categoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/investment-type/mapping/get-data/sub-category/category-id",
            type: "GET",
            dataType: 'json',
            data: {CategoryId: categoryId},
            async: true,
            success: function (result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j) {
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
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getActivity = (subCategoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/investment-type/mapping/get-data/activity/sub-category-id",
            type: "GET",
            dataType: 'json',
            data: {subCategoryId: subCategoryId},
            async: true,
            success: function (result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j) {
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
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getSubActivity = (activityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/investment-type/mapping/get-data/sub-activity/activity-id",
            type: "GET",
            dataType: 'json',
            data: {ActivityId: activityId},
            async: true,
            success: function (result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j) {
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_sub_activity').select2({
                    placeholder: "Select an Sub Activity",
                    width: '100%',
                    data: data
                });
            },
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

$('#btn_export_excel').on('click', function () {
    let category = $('#filter_category').val();
    let subCategory = $('#filter_sub_category').val();
    let activity = $('#filter_activity').val();
    let subActivity = $('#filter_sub_activity').val();

    let url = "/master/investment-type/mapping/export-xls?category=" + category + "&subCategory=" + subCategory + "&activity=" + activity + "&subActivity=" + subActivity;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

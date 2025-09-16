'use strict';

var validator, dt_roi_cr;
var swalTitle = "ROI & CR Configuration";

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        digits: 0,
        groupSeparator: ",",
    }).mask("#minimumROI, #maksimumROI");

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        onfocus: "",
        digits: 0,
        groupSeparator: ",",
    }).mask("#minimumCostRatio, #maksimumCostRatio");

    getCategory();

    validator =  FormValidation.formValidation(document.getElementById('form_config_roi_cr'), {
        fields: {
            minimumROI: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            maksimumROI: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            minimumCostRatio: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            maksimumCostRatio: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
        }
    });

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
        scrollY: "50vh",
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 50,
                searchable: false,
                className: 'text-nowrap dt-body-start',
                checkboxes: {
                    'selectRow': false
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
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
            tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl)
            });
            $('#dt_roi_cr_search').trigger('keyup');
        },
        drawCallback: function( settings, json ) {
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
            tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl)
            });
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
});

$('#dt_roi_cr_search').on('keyup', function() {
    dt_roi_cr.search(this.value).draw();
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
    $('#btn_save').attr('disabled', true);
    dt_roi_cr.ajax.url(url).load(function () {
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
        $('#btn_save').attr('disabled', false);
    });
});

$('#btn_back').on('click', function () {
    window.location.href = "/configuration/roi-cr";
});

$('#btn_save').on('click', function() {
    let e = document.querySelector("#btn_save");
    validator.validate().then(function (status) {
        if (status == "Valid") {
            let dataRowsChecked = [];
            var rows_selected = dt_roi_cr.column(0).checkboxes.selected();
            $.each(rows_selected, function (index, value) {
                dataRowsChecked.push({
                    id: value,
                    minimumROI: $('#minimumROI').val().replace(/,/g, ''),
                    maksimumROI: $('#maksimumROI').val().replace(/,/g, ''),
                    minimumCostRatio: $('#minimumCostRatio').val().replace(/,/g, ''),
                    maksimumCostRatio: $('#maksimumCostRatio').val().replace(/,/g, ''),
                });
            });

            if (dataRowsChecked.length > 0) {
                let formData = new FormData();
                formData.append('config', JSON.stringify(dataRowsChecked));
                let url = "/configuration/roi-cr/save";
                $.ajax({
                    url         : url,
                    data        : formData,
                    type        : 'POST',
                    async       : true,
                    dataType    : 'JSON',
                    cache       : false,
                    contentType : false,
                    processData : false,
                    beforeSend: function() {
                        e.setAttribute("data-kt-indicator", "on");
                        e.disabled = !0;
                    },
                    success: function(result, status, xhr, $form) {
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
                                window.location.href = '/configuration/roi-cr';
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
                    complete: function() {
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
                    text: "Please tick detail to configure",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        }
    });
});

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
            return reject(jqXHR.responseText);
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
        return reject(e);
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
        return reject(e);
    });
}

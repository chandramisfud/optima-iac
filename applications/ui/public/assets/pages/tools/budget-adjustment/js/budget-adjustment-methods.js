'use strict';

let validator, validator_upload, dt_budget_adjustment_issue, onLoadPage = true;
let swalTitle = "Budget Adjustment";
heightContainer = 425;
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListEntity()]).then(async function () {
    });

    const form = document.getElementById('form_budget_adjustment');

    validator = FormValidation.formValidation(form, {
        fields: {
            filter_entity: {
                validators: {
                    notEmpty: {
                        message: "Please enter Entity"
                    },
                }
            },
        },

        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })

    validator_upload = FormValidation.formValidation(form, {
        fields: {
            file: {
                selector: '[data-stripe="file"]',
                validators: {
                    notEmpty: {
                        message: 'Choose file..',
                    },
                },
            },
        },

        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })

    dt_budget_adjustment_issue = $('#dt_budget_adjustment_issue').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        ordering: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'budget',
                title: 'Budget',
                className: 'align-middle',
            },
            {
                targets: 1,
                title: 'Status',
                data: 'status',
                className: 'align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

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

});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }

});

$('#filter_period').on('change', async function () {
    let periode = $(this).val();
    let entity = $('#filter_entity').val();
    if (periode.length === 4) {
        let elBudget = $('#filter_budget');
        elBudget.empty();
        $(this).attr('disabled', true);

        await getListBudget(periode, entity);

        $(this).attr('disabled', false).focus();
        elBudget.val('').trigger('change');
    }
});

$('#filter_entity').on('change', async function () {
    let elBudget = $('#filter_budget');
    elBudget.empty();
    let entity = $(this).val();
    let periode = $('#filter_period').val();

    if ($(this).val() !== "") await getListBudget(periode,entity);
    validator.revalidateField('filter_entity');
    elBudget.val('').trigger('change');
});

$('#btn_download').on('click', function (){
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_download");
            let filter_period = ($('#filter_period').val()) ?? "";
            let filter_entity = ($('#filter_entity').val()) ?? "0";
            let filter_budget = ($('#filter_budget').val()) ?? "";

            let url = "/tools/budget-adjustment/download-template?period=" + filter_period + "&entityId=" + filter_entity + "&budgetName=" + encodeURIComponent(filter_budget);
            let a = document.createElement("a");
            a.href = url;
            let evt = document.createEvent("MouseEvents");
            evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
                true, false, false, false, 0, null);
            a.dispatchEvent(evt);
        }
    });
});

$('.field_upload').on('change', function() {
    let file_size = $(this)[0].files[0].size;
    if (file_size > 10000000) {
        Swal.fire({
            title: "Budget Adjustment ",
            text: "Ukuran file lebih dari 10Mb, mohon pilih file yang ukurannya kurang dari 10Mb",
            icon: "warning",
            confirmButtonText: "Ok",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: { confirmButton: "btn btn-optima" }
        }).then(function () {
            $('.field_upload').val(null);
            validator_upload.resetForm(true);
        });
    }
});

$('#btn_upload').on('click', function() {
    validator_upload.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            let formData = new FormData($('#form_budget_adjustment')[0]);
            let url = '/tools/budget-adjustment/upload-xls';
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
                },
                success: function (result, status, xhr, $form) {
                    if (!result.error) {
                        if (result.code === 200) {
                            Swal.fire({
                                title: swalTitle,
                                text: 'Files Uploaded!',
                                icon: "success",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(async function () {
                                $("#card_budget_adjustment_issue").removeClass("d-none");
                                dt_budget_adjustment_issue.clear().draw();
                                dt_budget_adjustment_issue.rows.add(result.data).draw();
                            });
                        }
                        if (result.code === 409) {
                            Swal.fire({
                                title: swalTitle,
                                text: result.message,
                                icon: "warning",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(async function () {
                                $("#card_budget_adjustment_issue").removeClass("d-none");
                                dt_budget_adjustment_issue.clear().draw();
                                dt_budget_adjustment_issue.rows.add(result.data).draw();
                            });
                        }
                    } else {
                        Swal.fire({
                            title: 'Files Upload Failed!',
                            text: result.message,
                            icon: "warning",
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
                        text: "Files Upload Failed",
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
                text: 'Choose file...',
                icon: "warning",
                confirmButtonText: "Ok",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/tools/budget-adjustment/list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].shortDesc + " - " + result.data[j].longDesc,
                        longDesc: result.data[j].longDesc
                    });
                }
                $('#filter_entity').select2({
                    placeholder: "Select an Entity",
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

const getListBudget = (periode, entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/tools/budget-adjustment/get-data/budget/entity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {periode: periode, entityId: entityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].longDesc,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_budget').select2({
                    placeholder: "Select budget master name",
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

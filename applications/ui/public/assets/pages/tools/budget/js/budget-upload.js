'use strict';

var validator, dt_budget;
var swalTitle = "Upload Data Budget";
heightContainer = 430;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    validator = FormValidation.formValidation(document.querySelector("#form_budget"), {
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
    });

    dt_budget = $('#dt_budget').DataTable({
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
        orderable: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'budget',
                title: 'Budget',
                orderable: false,
                className: 'align-middle',
            },
            {
                targets: 1,
                title: 'Status',
                data: 'status',
                orderable: false,
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

$('.btn_download').on('click', function() {
    let url, file_name;
    let category = $(this).data('category');
    if (category === "Distributor Cost") {
        url = file_host + '/assets/media/templates/Template_Budget_DC.xlsx';
        file_name = "Template Budget DC.xlsx"
    } else if (category === "Retailer Cost") {
        url = file_host + '/assets/media/templates/Template_Budget.xlsx';
        file_name = "Template Budget.xlsx"
    } else {
        return Swal.fire({
            title: "Budget Upload",
            text: "Template budget "+ category +" category not found",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"},
        });
    }
    fetch(url).then((resp) => {
        if (resp.ok) {
            resp.blob().then(blob => {
                const url_blob = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.style.display = 'none';
                a.href = url_blob;
                a.download = file_name;
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url_blob);
                blockUI.release();
            })
                .catch(e => {
                    blockUI.release();
                    console.log(e);
                    Swal.fire({
                        text: "Download template budget failed",
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"},
                    });
                });
        } else {
            blockUI.release();
            Swal.fire({
                title: "Budget Upload",
                text: "Template not found",
                icon: "warning",
                buttonsStyling: !1,
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"},
            });
        }
    });
});

$('.field_upload').on('change', function() {
    let file_size = $(this)[0].files[0].size;
    if (file_size > 10000000) {
        Swal.fire({
            title: "Mechanism",
            text: "Ukuran file lebih dari 10Mb, mohon pilih file yang ukurannya kurang dari 10Mb",
            icon: "warning",
            confirmButtonText: "Ok",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: { confirmButton: "btn btn-optima" }
        }).then(function () {
            $('.field_upload').val(null);
            validator.resetForm(true);
        });
    }
});

$('#btn_upload').on('click', function() {
    let fileUpload = document.getElementById("file");
    let url;
    validator.validate().then(function (status) {
        if (status === "Valid") {
            if (typeof (FileReader) != "undefined") {
                let reader = new FileReader();

                //For Browsers other than IE.
                if (reader.readAsBinaryString) {
                    reader.onload = function (e) {
                        //Read the Excel File data.
                        let data = e.target.result;
                        let workbook = XLSX.read(data, {
                            type: 'binary'
                        });

                        let firstSheet = workbook.SheetNames[0];

                        if (workbook.SheetNames.length === 1 && firstSheet === "DC-Budget Template") {
                            url = '/tools/upload-budget/upload-xls/dc';
                        } else if (isTemplateRC(workbook.SheetNames)) {
                            url = '/tools/upload-budget/upload-xls';
                        } else {
                            return Swal.fire({
                                title: swalTitle,
                                text: "The uploaded file is not RC or DC budget template",
                                icon: "warning",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-primary"}
                            });
                        }
                        uploadTemplate(url)
                    };
                    reader.readAsBinaryString(fileUpload.files[0]);
                }
            } else {
                Swal.fire({
                    title: swalTitle,
                    text: "This browser does not support HTML5.",
                    icon: "warning",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-primary"}
                });
            }
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

const uploadTemplate = (url) => {
    let e = document.querySelector("#btn_upload");
    let formData = new FormData($('#form_budget')[0]);
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    blockUI.block();
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
        success: function (result) {
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
                        $("#card_budget").removeClass("d-none");
                        dt_budget.clear().draw();
                        dt_budget.rows.add(result.data).draw();
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
                        $("#card_budget").removeClass("d-none");
                        dt_budget.clear().draw();
                        dt_budget.rows.add(result.data).draw();
                    });
                }
            } else {
                Swal.fire({
                    title: swalTitle,
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
            blockUI.release();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.message)
            Swal.fire({
                title: swalTitle,
                text: "Files Upload Failed",
                icon: "error",
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
}

const isTemplateRC = (sheet) => {
    if (sheet[0] === "REGION" &&
        sheet[1] === "BRAND SKU" &&
        sheet[2] === "MASTER CHANNEL" &&
        sheet[3] === "SUB ACTIVITY TYPE" &&
        sheet[4] === "CATEGORY" &&
        sheet[5] === "DISTRIBUTOR" &&
        sheet[6] === "PRINCIPAL" &&
        sheet[7] === "ALLOCATION BUDGET" &&
        sheet[8] === "DERIVATIVE BUDGET" &&
        sheet[9] === "ALLOCATION REGION" &&
        sheet[10] === "ALLOCATION ACCOUNT" &&
        sheet[11] === "ALLOCATION BRAND" &&
        sheet[12] === "ALLOCATION USER" &&
        sheet[13] === "DERIVATIVE USER"
    ) {
        return true;
    } else {
        return false;
    }
}

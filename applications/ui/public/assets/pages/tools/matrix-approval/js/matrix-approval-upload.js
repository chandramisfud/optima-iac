'use strict';

var validator;
var swalTitle = "Upload Data Master Matrix Approval";

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    validator = FormValidation.formValidation(document.querySelector("#form_matrix_approval"), {
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
});

$('#btn_download').on('click', function() {
    let url = file_host + '/assets/media/templates/Template_Matrix_Promo.xlsx?v=' + new Date();
    let file_name = "Template Matrix Promo.xlsx"
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
                        title: swalTitle,
                        text: "Download template matrix approval failed",
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
                title: swalTitle,
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
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            var formData = new FormData($('#form_matrix_approval')[0]);
            let url = '/tools/upload-matrix-approval/upload-xls';
            $.ajax({
                url: url,
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                // enctype: "multipart/form-data",
                beforeSend: function () {
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                },
                success: function (result, status, xhr, $form) {
                    if (!result.error) {
                        Swal.fire({
                            title: swalTitle,
                            html: 'Upload Success! <br/> Update matrix promo will be processing on new tab',
                            icon: "success",
                            confirmButtonText: "Ok",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            if (result['processId'] !== 0) {
                                let handle = window.open('/tools/upload-matrix-approval/process?i=' + result['processId']);
                                handle.blur();
                                window.focus();
                            }
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "Ok",
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

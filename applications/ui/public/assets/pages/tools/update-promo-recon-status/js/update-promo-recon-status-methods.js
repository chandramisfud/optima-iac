'use strict';

let swalTitle = "Update Promo Reconciliation Status";
let validator;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    validator = FormValidation.formValidation(document.querySelector("#form_upload"), {
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

$('#btn_download_template').on('click', function () {
    let url = file_host + '/assets/media/templates/Template_Update_Promo_Reconciliation_Status.xlsx?v=' + new Date();
    let file_name = "Template Update Promo Reconciliation Status.xlsx"
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
                        text: "Download template update promo recon failed",
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

$('#btn_upload').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            let formData = new FormData($('#form_upload')[0]);
            let url = '/tools/update-promo-recon-status/upload';
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
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
                        Swal.fire({
                            title: swalTitle,
                            html: 'Upload Success!',
                            icon: "success",
                            confirmButtonText: "Ok",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                        $('#form_upload')[0].reset();
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
                error: function (jqXHR) {
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

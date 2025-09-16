'use strict';
let validator, method, promoId, promoRefId, isClose, isCancel, isCancelLocked;
let dt_mechanism;
let swalTitle = "Promo Creation - Cancel Request";
heightContainer = 500;

const form = document.getElementById('form_promo_cancel_request');
let url_str = new URL(window.location.href);
method = url_str.searchParams.get("method");
promoId = url_str.searchParams.get("promoId");

validator = FormValidation.formValidation(form, {
    fields: {
        cancelReason: {
            validators: {
                notEmpty: {
                    message: "Please fill in a reason"
                },
            }
        },
    },
    plugins: {
        trigger: new FormValidation.plugins.Trigger,
        bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
    }
})

$(document).ready(function () {
    $('form').submit(false);

    blockUI.block();
    if (method === 'cancelrequest') {
        Promise.all([getData(promoId)]).then(async () => {
            $('#txt_info_method').text('Promo ID ' + promoRefId);
            await getCancelReason();
            blockUI.release();

            if (isClose || isCancel || isCancelLocked) {
                disableButtonSave();
                let txtReason = ''
                if (isCancelLocked) txtReason = ', because this promo has cancelled request';
                if (isCancel) txtReason = ', because this promo is cancel';
                if (isClose) txtReason = ', because this promo is close';
                Swal.fire({
                    title: swalTitle,
                    text: 'Promo ' + promoRefId + ' can not request' + txtReason,
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    window.location.href = '/promo/creation';
                });
            }

        });
    }
});

$('#cancelReason').on('change', async function () {
    validator.addField('reason', {
        validators: {
            notEmpty: {
                message: "Please fill in a reason"
            },
        }
    });

    let elReason = $('#reason');
    if ($(this).val() === "Others") {
        elReason.removeClass('d-none');
        elReason.val('');
        validator.revalidateField('reason');
    } else {
        elReason.val($(this).val());
        elReason.addClass('d-none');
        validator.removeField('reason');
    }
    validator.revalidateField('cancelReason');
});

$('#btn_cancel_request').on('click', function () {
    let e = document.querySelector("#btn_cancel_request");
    validator.validate().then(async function (status) {
        if (status === "Valid") {
            blockUI.block();
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;

            let formData = new FormData($('#form_promo_cancel_request')[0]);
            formData.append('promoId', promoId);

            let url = "/promo/creation/save-cancel-request";
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
                            text: result.message,
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.location.href = '/promo/creation';
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            html: result.message,
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
                error: function (jqXHR) {
                    console.log(jqXHR)
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

        }
    });
});

const getData = (promoId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/creation/data/id",
            type: "GET",
            data: {id: promoId},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;

                    promoRefId = values.promoHeader.refId;

                    isCancel = values.promoHeader.isCancel;
                    isClose = values.promoHeader.isClose;
                    isCancelLocked = values.promoHeader.isCancelLocked;

                    $('#period').val(new Date(values.promoHeader.startPromo).getFullYear());
                    $('#entity').val(values.promoHeader.principalName);
                    if (values.groupBrand) {
                        for (let i = 0; i < values.groupBrand.length; i++) {
                            if (values.groupBrand[i].flag) $('#groupBrand').val(values.groupBrand[i].longDesc);
                        }
                    }
                    $('#distributor').val(values.promoHeader.distributorName);
                    $('#subCategory').val(values.promoHeader.subCategoryDesc);
                    $('#subActivityDesc').val(values.promoHeader.subActivityLongDesc);
                    let strChannel = "";
                    if (values.channels) {
                        let arrChannels = [];
                        for (let i = 0; i < values.channels.length; i++) {
                            if (values.channels[i].flag) {
                                arrChannels.push(values.channels[i].longDesc);
                            }
                        }
                        strChannel += arrChannels.reduce((text, value, i, array) => text + (i < array.length - 1 ? ', ' : ', ') + value);
                    }
                    $('#channel').val(strChannel);
                    $('#allocationRefId').val(values.promoHeader.allocationRefId);
                    $('#startPromo').val(formatDateOptima(values.promoHeader.startPromo));
                    $('#endPromo').val(formatDateOptima(values.promoHeader.endPromo));
                    for (let i = 0; i < values.mechanisms.length; i++) {
                        $('#mechanism1').val(values.mechanisms[i].mechanism);
                    }
                    $('#investment').val(formatMoney(values.promoHeader.investment, 0));
                    $('#initiator_notes').val(values.promoHeader.initiator_notes);

                    $('#allocationDesc').val(values.promoHeader.allocationDesc);
                    $('#budgetAmount').val(formatMoney(values.promoHeader.budgetAmount, 0));
                    $('#remainingBudget').val(formatMoney(values.promoHeader.remainingBudget, 0));

                    if (values.attachments) {
                        values.attachments.forEach((item, index, arr) => {
                            if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).val(item.fileName);
                                $('#btn_download' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                            }
                        });
                    }

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
                return resolve();
            },
            error: function (jqXHR) {
                console.log(jqXHR);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getCancelReason = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/list/cancel-reason",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].longDesc,
                        text: result.data[j].longDesc
                    });
                }
                data.push({
                    id: "Others",
                    text: "Others, please specify ..."
                });

                $('#cancelReason').select2({
                    placeholder: "Cancellation Reason",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, errorThrown)
            {
                console.log(jqXHR);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#review_file_label_' + row);
        let url = file_host + "/assets/media/promo/" + promoId + "/row" + row + "/" + attachment.val();
        if (attachment.val() !== "") {
            fetch(url)
                .then((resp) => {
                    if (resp.ok) {
                        resp.blob().then(blob => {
                            const url_blob = window.URL.createObjectURL(blob);
                            const a = document.createElement('a');
                            a.style.display = 'none';
                            a.href = url_blob;
                            a.download = attachment.val();
                            document.body.appendChild(a);
                            a.click();
                            window.URL.revokeObjectURL(url_blob);
                        })
                        .catch(e => {
                            console.log(e);
                            Swal.fire({
                                text: "Download attachment failed",
                                icon: "warning",
                                buttonsStyling: !1,
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            });
                        });
                    } else {
                        Swal.fire({
                            title: "Download Attachment",
                            text: "Attachment not found",
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
    }
});

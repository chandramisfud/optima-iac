'use strict';

let validator, method, promoId, promoRefId, isClose, isCancel, isCancelLocked;
let dt_mechanism;
let swalTitle = "Promo Creation - Cancel Request";

let targetAttribute = document.querySelector(".card_attribute");
let blockUIAttribute = new KTBlockUI(targetAttribute, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetAttachment = document.querySelector(".card_attachment");
let blockUIAttachment = new KTBlockUI(targetAttachment, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $('form').submit(false);

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
    });

    dt_mechanism = $('#dt_mechanism').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "30vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                width: 50,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                title: 'Mechanism',
                data: 'mechanism',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Notes',
                data: 'notes',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'productId',
                data: 'productId',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'product',
                data: 'product',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'brandId',
                data: 'brandId',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Sub Brand',
                data: 'brand',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'mechanismId',
                data: 'mechanismId',
                visible: false,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    blockUI.block();
    blockUIAttribute.block();
    blockUIAttachment.block();
    Promise.all([getData(promoId)]).then(async () => {
        $('#txt_info_method').text('Promo ID ' + promoRefId);
        await getCancelReason();
        blockUI.release();
        blockUIAttribute.release();
        blockUIAttachment.release();

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
            blockUIAttribute.block();
            blockUIAttachment.block();
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
                    blockUIAttribute.release();
                    blockUIAttachment.release();
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
    blockUI.release();
});

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#attachment' + row);
        let url = file_host + "/assets/media/promo/" + promoId + "/row" + row + "/" + attachment.val();
        blockUIAttachment.block();
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
                        blockUIAttachment.release();
                    })
                        .catch(e => {
                            blockUIAttachment.release();
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
                    blockUIAttachment.release();
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
    }
});

const getCancelReason = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/list/cancel-reason",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
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
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

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

                    for (let i = 0; i < values.channels.length; i++) {
                        if (values.channels[i].flag) $('#channel').val(values.channels[i].longDesc);
                    }
                    for (let i = 0; i < values.subChannels.length; i++) {
                        if (values.subChannels[i].flag) $('#subChannel').val(values.subChannels[i].longDesc);
                    }
                    for (let i = 0; i < values.accounts.length; i++) {
                        if (values.accounts[i].flag) $('#account').val(values.accounts[i].longDesc);
                    }
                    for (let i = 0; i < values.subAccounts.length; i++) {
                        if (values.subAccounts[i].flag) $('#subAccount').val(values.subAccounts[i].longDesc);
                    }

                    isCancel = values.promoHeader.isCancel;
                    isClose = values.promoHeader.isClose;
                    isCancelLocked = values.promoHeader.isCancelLocked;

                    $('#period').val(new Date(values.promoHeader.startPromo).getFullYear());
                    $('#promoPlanRefId').val(values.promoHeader.promoPlanRefId);
                    $('#allocationRefId').val(values.promoHeader.allocationRefId);
                    $('#allocationDesc').val(values.promoHeader.allocationDesc);
                    $('#tsCoding').val(values.promoHeader.tsCoding);
                    $('#subCategoryDesc').val(values.promoHeader.subCategoryDesc);
                    $('#entity').val(values.promoHeader.principalName);
                    $('#distributor').val(values.promoHeader.distributorName);
                    $('#budgetAmount').val(formatMoney(values.promoHeader.budgetAmount, 0));
                    $('#remainingBudget').val(formatMoney(values.promoHeader.remainingBudget, 0));

                    $('#startPromo').val(formatDateOptima(values.promoHeader.startPromo));
                    $('#endPromo').val(formatDateOptima(values.promoHeader.endPromo));
                    $('#activityName').val(values.promoHeader.activityDesc);

                    $('#activityDesc').val(values.promoHeader.activityLongDesc);
                    $('#subActivityDesc').val(values.promoHeader.subActivityLongDesc);
                    $('#initiatorNotes').val(values.promoHeader.initiator_notes);
                    $('#baselineSales').val(formatMoney(values.promoHeader.normalSales, 0));
                    $('#incrementSales').val(formatMoney(values.promoHeader.incrSales, 0));
                    $('#investment').val(formatMoney(values.promoHeader.investment, 0));
                    $('#totalSales').val(formatMoney(values.promoHeader.normalSales + values.promoHeader.incrSales, 0));
                    $('#totalInvestment').val(formatMoney(values.promoHeader.investment, 0));
                    $('#roi').val(formatMoney(values.promoHeader.roi, 2));
                    $('#costRatio').val(formatMoney(values.promoHeader.costRatio, 2));

                    //Attribute Region
                    let longDescRegion = '';
                    for (let i = 0; i < values.regions.length; i++) {
                        if (values.regions[i].flag) {
                            longDescRegion += '<span className="fw-bold text-sm-left">' + values.regions[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        }
                    }
                    $('#card_list_region').html(longDescRegion);

                    //Attribute Brand
                    let longDescBrand = '';
                    for (let i = 0; i < values.brands.length; i++) {
                        if (values.brands[i].flag) {
                            longDescBrand += '<span className="fw-bold text-sm-left">' + values.brands[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        }
                    }
                    $('#card_list_brand').html(longDescBrand);

                    //Attribute SKU
                    let longDescSKU = '';
                    for (let i = 0; i < values.skus.length; i++) {
                        if (values.skus[i].flag) {
                            longDescSKU += '<span className="fw-bold text-sm-left">' + values.skus[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        }
                    }
                    $('#card_list_sku').html(longDescSKU);

                    // Attribuate Mechanism
                    let nourut = 0;
                    dt_mechanism.clear().draw();
                    for (let i = 0; i < values.mechanisms.length; i++) {
                        nourut += 1;
                        let dMechanism = [];
                        dMechanism["no"] = nourut;
                        dMechanism["mechanism"] = values.mechanisms[i].mechanism;
                        dMechanism["notes"] = values.mechanisms[i].notes;
                        dMechanism["productId"] = values.mechanisms[i].productId;
                        dMechanism["product"] = values.mechanisms[i].product;
                        dMechanism["brandId"] = values.mechanisms[i].brandId;
                        dMechanism["brand"] = values.mechanisms[i].brand;
                        dMechanism["mechanismId"] = values.mechanisms[i].mechanismId;

                        dt_mechanism.row.add(dMechanism).draw();
                    }

                    if (values.attachments) {
                        values.attachments.forEach((item, index, arr) => {
                            if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                let elAttachmentInfo = $('#attachment' + parseInt(item.docLink.replace('row', '')));
                                elAttachmentInfo.val(item.fileName);
                                elAttachmentInfo.addClass('form-control-solid-bg');
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
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

'use strict';
let validator, method, promoId, isClose, isCancel, isCancelLocked, promoRefId;
let dt_mechanism;
let swalTitle = "Promo Reconciliation - Cancel Request";
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
                    message: "Reason must be enter"
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
    // $('form').submit(false);
    blockUI.block();
    initMechanism();
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
                    window.location.href = '/promo/recon';
                });
            }
        });
    }
});

$('#cancelReason').on('change', async function () {
    validator.addField('reason', {
        validators: {
            notEmpty: {
                message: "Reason must be enter"
            },
        }
    });

    if ($(this).val() === "Others") {
        $('#reason').removeClass('d-none');
        $('#reason').val('');
        validator.revalidateField('reason');
    } else {
        $('#reason').val($(this).val());
        $('#reason').addClass('d-none');
        validator.removeField('reason');
    }
    validator.revalidateField('cancelReason');
});

$('#btn_cancel_request').on('click', function () {
    let e = document.querySelector("#btn_cancel_request");
    blockUI.block();
    validator.validate().then(async function (status) {
        if (status === "Valid") {
            let formData = new FormData($('#form_promo_cancel_request')[0]);
            formData.append('promoId', promoId);

            let url = "/promo/recon/save-cancel-request";
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
                            window.location.href = '/promo/recon';
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

        }
    });
    blockUI.release();
});

const initMechanism = function () {
    dt_mechanism = $('#dt_mechanism').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                width: 50,
                orderable: false,
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
}

const getData = (promoId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/recon/data/id",
            type: "GET",
            data: {promoId: promoId},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;

                    promoRefId = values.promoHeader.refId;
                    isCancel = values.promoHeader.isCancel;
                    isClose = values.promoHeader.isClose;
                    isCancelLocked = values.promoHeader.isClose;

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

                    $('#period').val(new Date(values.promoHeader.startPromo).getFullYear());
                    $('#promoPlanRefId').val(values.promoHeader.promoPlanRefId);
                    $('#allocationRefId').val(values.promoHeader.allocationRefId);
                    $('#allocationDesc').val(values.promoHeader.allocationDesc);
                    $('#tsCoding').val(values.promoHeader.tsCoding);
                    $('#subCategoryDesc').val(values.promoHeader.subCategoryLongDesc);
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
                        let fileSource = "";
                        values.attachments.forEach((item, index, arr) => {
                            if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).text(item.fileName).attr('title', item.fileName);
                                $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).addClass('form-control-solid-bg');
                                $('#attachment' + parseInt(item.docLink.replace('row', ''))).attr('disabled', true);
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
            url         : "/promo/recon/list/cancel-reason",
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
                return reject(jqXHR.responseText);
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
        let url = file_host + "/assets/media/promo/" + promoId + "/row" + row + "/" + attachment.text();
        if (attachment.text() !== "") {
            fetch(url)
                .then((resp) => {
                    if (resp.ok) {
                        resp.blob().then(blob => {
                            const url_blob = window.URL.createObjectURL(blob);
                            const a = document.createElement('a');
                            a.style.display = 'none';
                            a.href = url_blob;
                            a.download = attachment.text();
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
                                    customClass: {confirmButton: "btn btn-optima"},
                                    closeOnConfirm: false,
                                    showLoaderOnConfirm: false,
                                    closeOnClickOutside: false,
                                    closeOnEsc: false,
                                    allowOutsideClick: false,
                                });
                            });
                    } else {
                        Swal.fire({
                            title: "Download Attachment",
                            text: "Attachment not found",
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
                    }
                });
        }
    }
});

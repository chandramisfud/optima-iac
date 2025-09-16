'use strict';

var validator, method, promoId, createOn, channelId, subChannelId, accountId, subAccountId, newMechanismMethod;
var statusApprovalCode, isCancel, isClose, tsCoding, promoRefId, promoPlanningId, allocationId, categoryId,
    subCategoryId, activityId, subActivityId, entityId, distributorId, appCutOffMechanism, appCutOffHierarchy;
var dt_mechanism;

var swalTitle = "Promo Display";

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

var targetModal = document.querySelector(".modal-content");
var blockUIModal = new KTBlockUI(targetModal, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $('form').submit(false);

    dt_mechanism = $('#dt_mechanism').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "20vh",
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
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    promoId = url_str.searchParams.get("id");
    blockUI.block();
    disableButtonSave();
    if (method === 'view') {
        Promise.all([]).then(async () => {
            await getData(promoId);
            blockUI.release();
        });
    }
});

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/display/data/id",
            type: "GET",
            data: {promoId: id},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;

                    $('#txt_info_method').text('View ' + (values.promoHeader.refId ?? ""));
                    statusApprovalCode = values.promoHeader.statusApprovalCode;
                    isCancel = values.promoHeader.isCancel;
                    isClose = values.promoHeader.isClose;
                    tsCoding = values.promoHeader.tsCoding;
                    promoRefId = values.promoHeader.refId;
                    promoPlanningId = values.promoHeader.promoPlanId;
                    allocationId = values.promoHeader.allocationId;
                    categoryId = values.promoHeader.categoryId;
                    subCategoryId = values.promoHeader.subCategoryId;
                    activityId = values.promoHeader.activityId;
                    subActivityId = values.promoHeader.subActivityId;
                    entityId = values.promoHeader.principalId;
                    distributorId = values.promoHeader.distributorId;
                    createOn = values.promoHeader.createOn;

                    // header
                    $('#period').val(new Date(values.promoHeader.startPromo).getFullYear());
                    $('#promoPlanRefId').val(values.promoHeader.promoPlanRefId);
                    $('#allocationRefId').val(values.promoHeader.allocationRefId);
                    $('#allocationDesc').val(values.promoHeader.allocationDesc);

                    $('#startPromo').val(formatDateOptima(values.promoHeader.startPromo));
                    $('#endPromo').val(formatDateOptima(values.promoHeader.endPromo));

                    $('#activity').val(values.promoHeader.activityLongDesc);
                    $('#subActivityDesc').val(values.promoHeader.subActivityLongDesc);
                    $('#activityDesc').val(values.promoHeader.activityDesc);
                    $('#tsCoding').val(values.promoHeader.tsCoding);
                    $('#subCategoryDesc').val(values.promoHeader.subCategoryLongDesc);
                    $('#entity').val(values.promoHeader.principalName);
                    $('#distributor').val(values.promoHeader.distributorName);
                    $('#budgetAmount').val(formatMoney(values.promoHeader.budgetAmount,0) ?? 0);
                    $('#remainingBudget').val(formatMoney(values.promoHeader.remainingBudget,0) ?? 0);
                    $('#initiatorNotes').val(values.promoHeader.initiator_notes);
                    $('#investmentType').val(values.promoHeader.investmentTypeDesc);

                    $('#baselineSales').val(formatMoney(values.promoHeader.normalSales, 0) ?? 0);
                    $('#incrementSales').val(formatMoney(values.promoHeader.incrSales, 0) ?? 0);
                    $('#investment').val(formatMoney(values.promoHeader.investment, 0) ?? 0);
                    $('#investmentBfrClose').val(formatMoney(values.promoHeader.investmentBfrClose, 0) ?? 0);
                    $('#investmentClosedBalance').val(formatMoney(values.promoHeader.investmentClosedBalance, 0) ?? 0);
                    $('#totalSales').val(formatMoney(values.promoHeader.normalSales + values.promoHeader.incrSales, 0) ?? 0);
                    $('#totalInvestment').val(formatMoney(values.promoHeader.investment, 0) ?? 0);
                    $('#roi').val(formatMoney(values.promoHeader.roi, 2) ?? 0);
                    $('#costRatio').val(formatMoney(values.promoHeader.costRatio, 2) ?? 0);
                    $('#notes_message').val(values.promoHeader.approvalNotes);

                    for (let i = 0; i < values.channels.length; i++) {
                        if (values.channels[i].flag) {
                            channelId = values.channels[i].id;
                            $('#channel').val(values.channels[i].longDesc);
                        }
                    }
                    for (let i = 0; i < values.subChannels.length; i++) {
                        if (values.subChannels[i].flag) {
                            subChannelId = values.subChannels[i].id;
                            $('#subChannel').val(values.subChannels[i].longDesc);
                        }
                    }
                    for (let i = 0; i < values.accounts.length; i++) {
                        if (values.accounts[i].flag) {
                            accountId = values.accounts[i].id;
                            $('#account').val(values.accounts[i].longDesc);
                        }
                    }
                    for (let i = 0; i < values.subAccounts.length; i++) {
                        if (values.subAccounts[i].flag) {
                            subAccountId = values.subAccounts[i].id;
                            $('#subAccount').val(values.subAccounts[i].longDesc);
                        }
                    }

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
                                $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).val(item.fileName);
                                $('#btn_download' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                                $('#btn_view' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                            }
                        });
                    }

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

$('.btn_view').on('click', function() {
    let row = $(this).val();
    let attachment = $('#review_file_label_' + row);
    let file_name = attachment.val();
    let title = $('#txt_info_method').text();
    let preview_path =  "/promo/display/preview-attachment?i="+promoId+"&r="+row+"&fileName="+encodeURIComponent(file_name)+"&t="+title;
    window.open(preview_path, "_blank");
});

$('#btn_export_pdf').on('click', function() {
    window.open( "/promo/display/export-pdf?id=" + promoId , "_blank");
});

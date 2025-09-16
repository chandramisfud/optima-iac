'use strict';

let swalTitle = "Promo Cancel Request Reconciliation";
let url_str = new URL(window.location.href);
let method = url_str.searchParams.get("m");
let promoId = url_str.searchParams.get("id");
let categoryShortDescEnc = url_str.searchParams.get("c");
let approvalCycle = url_str.searchParams.get("cycle");

let categoryId, groupBrandId, subActivityTypeId, subActivityId, distributorId, channelId;

let validator, promoRefId, isClose, createdBy;

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

//<editor-fold desc="Loading Animation">
let targetHeader = document.querySelector(".card_header");
let blockUIHeader = new KTBlockUI(targetHeader, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetBudget = document.querySelector(".card_budget");
let blockUIBudget = new KTBlockUI(targetBudget, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetAttachment = document.querySelector(".card_attachment");
let blockUIAttachment = new KTBlockUI(targetAttachment, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetCalculator = document.querySelector(".card_promo_calculator");
let blockUICalculator = new KTBlockUI(targetCalculator, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});
//</editor-fold>

//<editor-fold desc="Document on load">
$(document).ready(function () {
    $('form').submit(false);

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#remainingBudget, #totalCost, #baseline, #baselineBefore, #totalSales, #totalSalesBefore, #cost, #costBefore");

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
        suffix: ' %'
    }).mask("#uplift, #upliftBefore, #salesContribution, #salesContributionBefore, #storesCoverage, #storesCoverageBefore, #redemptionRate, #redemptionRateBefore, #roi, #roiBefore, #cr, #crBefore");

    blockUIHeader.block();
    blockUIBudget.block();
    blockUIAttachment.block();
    blockUICalculator.block();
    disableButtonSave();


    Promise.all([]).then(async () => {
        await getData(promoId).then(async function () {
            blockUIHeader.release();
            blockUIBudget.release();
            blockUIAttachment.release();
            blockUICalculator.release();
            enableButtonSave();
        });
    });
});
//</editor-fold>

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
                            a.download = $('#review_file_label_' + row).val();
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
                                    customClass: {confirmButton: "btn btn-optima"},
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
                            customClass: {confirmButton: "btn btn-optima"},
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
                customClass: {confirmButton: "btn btn-optima"},
                allowOutsideClick: false,
            });
        }
    }
});

$('.btn_view').on('click', function () {
    let row = $(this).val();
    let attachment = $('#review_file_label_' + row);
    let file_name = attachment.val();
    let title = $('#txt_info_method').text();
    let preview_path =  "/promo/cancel-request/preview-attachment?i="+promoId+"&r="+row+"&fileName="+encodeURIComponent(file_name)+"&t="+title;
    let w = window.innerWidth;
    let h = window.innerHeight;
    window.open(preview_path, "promoWindow", "location=1,status=1,scrollbars=1,width=" + w + ",height=" + h);
});

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    let formData = new FormData($('#form_promo_cancel')[0]);
    formData.append('promoId', promoId);
    formData.append('notes', $('#notes').val());
    formData.append('promoRefId', promoRefId);
    formData.append('createdBy', createdBy);

    let url = '/promo/cancel-request/approve';
    blockUIHeader.block();
    blockUIBudget.block();
    blockUIAttachment.block();
    blockUICalculator.block();
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;

    $.get('/refresh-csrf').done(function(data) {
        let elMeta = $('meta[name="csrf-token"]');
        elMeta.attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': elMeta.attr('content')
            }
        });
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
            },
            success: function(result) {
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
                        window.location.href = '/promo/cancel-request';
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
                blockUIHeader.release();
                blockUIBudget.release();
                blockUIAttachment.release();
                blockUICalculator.release();
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown)
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
    });
});

$('#btn_reject').on('click', function () {
    let e = document.querySelector("#btn_reject");
    let formData = new FormData($('#form_promo_cancel')[0]);
    formData.append('promoId', promoId);
    formData.append('notes', $('#notes').val());
    formData.append('promoRefId', promoRefId);
    formData.append('createdBy', createdBy);

    let url = '/promo/cancel-request/reject';
    blockUIHeader.block();
    blockUIBudget.block();
    blockUIAttachment.block();
    blockUICalculator.block();
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;

    $.get('/refresh-csrf').done(function(data) {
        let elMeta = $('meta[name="csrf-token"]');
        elMeta.attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': elMeta.attr('content')
            }
        });
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

            },
            success: function(result) {
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
                        window.location.href = '/promo/cancel-request';
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
                blockUIHeader.release();
                blockUIBudget.release();
                blockUIAttachment.release();
                blockUICalculator.release();
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown)
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
    });
});

const getData = (pId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/promo/cancel-request/data-revamp/id",
            type: "GET",
            data: {id: pId},
            dataType: "JSON",
            success: async function (result) {
                if (!result['error']) {
                    let data = result['data'];
                    let promo = data['promo'];

                    categoryId = (promo['categoryId'] ?? 0);
                    groupBrandId = (promo['groupBrandId'] ?? 0);
                    subActivityTypeId = (promo['subActivityTypeId'] ?? 0);
                    subActivityId = (promo['subActivityId'] ?? 0);
                    distributorId = (promo['distributorId'] ?? 0);
                    channelId = (promo['channelId'] ?? 0);
                    promoRefId = (promo['refId'] ?? 0);
                    isClose = (promo['isClose'] ?? 0);
                    createdBy = promo['createBy'];
                    $('#txt_info_method').text('Promo ID ' + promo['refId']);

                    $('#period').val(promo['period']);
                    $('#entityLongDesc').val(promo['entityLongDesc']);
                    $('#groupBrandLongDesc').val(promo['groupBrandLongDesc']);
                    $('#distributorLongDesc').val(promo['distributorLongDesc']);
                    $('#subActivityTypeDesc').val(promo['subActivityType']);
                    $('#subActivityLongDesc').val(promo['subActivityLongDesc']);
                    $('#channelDesc').val(promo['channelDesc']);
                    $('#startPromo').val(formatDateOptima(promo['startPromo']));
                    $('#endPromo').val(formatDateOptima(promo['endPromo']));
                    $('#initiatorNotes').val(promo['initiator_notes']);

                    $('#notes').val(promo['cancelReason']);

                    $('#budgetSourceName').val(promo['budgetName']);
                    $('#remainingBudget').val((promo['remainingBudget'] ? formatMoney(promo['remainingBudget'],0) : 0));
                    $('#totalCost').val(formatMoney(promo['cost'],0));

                    if (data['mechanism'].length > 0) {
                        $('#mechanism').val(data['mechanism'][0]['mechanism']);
                    }

                    if (data['attachment']) {
                        data['attachment'].forEach((item) => {
                            if (item['docLink'] === 'row' + parseInt(item['docLink'].replace('row', ''))) {
                                $('#review_file_label_' + parseInt(item['docLink'].replace('row', ''))).val(item.fileName);
                                $('#btn_download' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                                $('#btn_view' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                            }
                        });
                    }

                    $('#baseline').val('0');
                    $('#uplift').val(promo['upLift']);
                    $('#totalSales').val(promo['totalSales']);
                    $('#salesContribution').val(promo['salesContribution']);
                    $('#storesCoverage').val(promo['storesCoverage']);
                    $('#redemptionRate').val(promo['redemptionRate']);
                    $('#cr').val(promo['cr']);
                    $('#cost').val(promo['cost']);

                    roiFormula();
                }
            },
            complete: function () {
                return resolve();
            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return 0;
            }
        });
    }).catch((e) => {
        console.log(e);
        return 0;
    });
}

const roiFormula = () => {
    let elUplift = $('#uplift');
    let elTotalSales = $('#totalSales');
    let elRoi = $('#roi');
    let elCost = $('#cost');

    let uplift = parseFloat((elUplift.val() === '' ? '0' : elUplift.val()).replace(/,/g, ''));
    let totalSales = parseFloat((elTotalSales.val() === '' ? '0' : elTotalSales.val()).replace(/,/g, ''));
    let cost = parseFloat((elCost.val() === '' ? '0' : elCost.val()).replace(/,/g, ''));
    //uplift
    uplift = uplift+100;

    let incrSales = (Math.round(totalSales) * (uplift / 100));
    let roi = Math.round(((incrSales - cost) / cost) * 100);
    if (!isFinite(roi)) {
        elRoi.val(0);
    } else {
        elRoi.val(roi);
    }
}

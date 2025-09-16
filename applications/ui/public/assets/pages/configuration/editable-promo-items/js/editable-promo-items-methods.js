'use strict';

let swalTitle = "Editable Promo Reconciliation Items";
let dialerObject, dialerObjectDC, categoryId, categoryIdDC;

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    let dialerElementDC = document.querySelector("#dialer_period_dc");
    dialerObjectDC = new KTDialer(dialerElementDC, {
        step: 1,
    });

    blockUI.block();
    Promise.all([getDataConfig(), getDataConfigDC()]).then(function () {
        blockUI.release();
    })
});

$('#btn_export_excel').on('click', function () {
    let filter_period = ($('#filter_period').val()) ?? "";
    let url = "/configuration/editable-promo-items/export-xls-history?period=" + filter_period + '&categoryShortDesc=RC';
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_excel_dc').on('click', function () {
    let filter_period = ($('#filter_period_dc').val()) ?? "";
    let url = "/configuration/editable-promo-items/export-xls-history?period=" + filter_period + '&categoryShortDesc=DC';
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");
    let formData = new FormData($('#form_config')[0]);
    formData.append('categoryId', categoryId);
    let url = "/configuration/editable-promo-items/submit";
    blockUI.block();
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
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
                    getDataConfig().then(function () {
                        blockUI.release();
                    });
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
                }).then(function () {
                    blockUI.release();
                });
            }
        },
        complete: function() {
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.message);
            blockUI.release();
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

$('#btn_save_dc').on('click', function () {
    let e = document.querySelector("#btn_save_dc");
    let formData = new FormData($('#form_config_dc')[0]);
    formData.append('categoryId', categoryIdDC);
    let url = "/configuration/editable-promo-items/submit-dc";
    blockUI.block();
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
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
                    getDataConfigDC().then(function () {
                        blockUI.release();
                    });
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
                }).then(function () {
                    blockUI.release();
                });
            }
        },
        complete: function() {
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.message);
            blockUI.release();
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

const getDataConfig = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/editable-promo-items/get-data",
            type        : "GET",
            data        : {categoryDesc: 'RC'},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result.error) {
                    let promoConfig = result.data.promoConfig;
                    let enableConfig = result.data.enableConfig;

                    categoryId = promoConfig.categoryId;

                    //Promo Config
                    $('#budgetYear').prop('checked', (!(promoConfig.budgetYear)));
                    $('#promoPlanning').prop('checked', (!(promoConfig.promoPlanning)));
                    $('#budgetSource').prop('checked', (!(promoConfig.budgetSource)));
                    $('#subCategory').prop('checked', (!(promoConfig.subCategory)));
                    $('#activity').prop('checked', (!(promoConfig.activity)));
                    $('#subActivity').prop('checked', (!(promoConfig.subActivity)));
                    $('#startPromo').prop('checked', (!(promoConfig.startPromo)));
                    $('#endPromo').prop('checked', (!(promoConfig.endPromo)));
                    $('#activityName').prop('checked', (!(promoConfig.activityName)));
                    $('#initiatorNotes').prop('checked', (!(promoConfig.initiatorNotes)));
                    $('#incrSales').prop('checked', (!(promoConfig.incrSales)));
                    $('#investment').prop('checked', (!(promoConfig.investment)));
                    $('#roi').prop('checked', (!(promoConfig.ROI)));
                    $('#cr').prop('checked', (!(promoConfig.CR)));
                    $('#channel').prop('checked', (!(promoConfig.channel)));
                    $('#subChannel').prop('checked', (!(promoConfig.subChannel)));
                    $('#account').prop('checked', (!(promoConfig.account)));
                    $('#subAccount').prop('checked', (!(promoConfig.subAccount)));
                    $('#region').prop('checked', (!(promoConfig.region)));
                    $('#groupBrand').prop('checked', (!(promoConfig.groupBrand)));
                    $('#sku').prop('checked', (!(promoConfig.SKU)));
                    $('#mechanism').prop('checked',  (!(promoConfig.mechanism)));
                    $('#attachment').prop('checked',  (!(promoConfig.attachment)));

                    //enabledConfig
                    $('#budgetYear').attr('disabled', enableConfig.disabledBudgetYear);
                    $('#promoPlanning').attr('disabled', enableConfig.disabledPromoPlanning);
                    $('#budgetSource').attr('disabled', enableConfig.disabledBudgetSource);
                    $('#subCategory').attr('disabled', enableConfig.disabledSubCategory);
                    $('#activity').attr('disabled', enableConfig.disabledActivity);
                    $('#subActivity').attr('disabled', enableConfig.disabledSubActivity);
                    $('#startPromo').attr('disabled', enableConfig.disabledStartPromo);
                    $('#endPromo').attr('disabled', enableConfig.disabledEndPromo);
                    $('#activityName').attr('disabled', enableConfig.disabledActivityName);
                    $('#initiatorNotes').attr('disabled', enableConfig.disabledInitiatorNotes);
                    $('#incrSales').attr('disabled', enableConfig.disabledIncrSales);
                    $('#investment').attr('disabled', enableConfig.disabledInvestment);
                    $('#roi').attr('disabled', enableConfig.disabledROI);
                    $('#cr').attr('disabled', enableConfig.disabledCR);
                    $('#channel').attr('disabled', enableConfig.disabledChannel);
                    $('#subChannel').attr('disabled', enableConfig.disabledSubChannel);
                    $('#account').attr('disabled', enableConfig.disabledAccount);
                    $('#subAccount').attr('disabled', enableConfig.disabledSubAccount);
                    $('#region').attr('disabled', enableConfig.disabledRegion);
                    $('#groupBrand').attr('disabled', enableConfig.disabledGroupBrand);
                    $('#sku').attr('disabled', enableConfig.disabledSKU);
                    $('#mechanism').attr('disabled', enableConfig.disabledMechanism);
                    $('#attachment').attr('disabled', enableConfig.disabledAttachment);
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

const getDataConfigDC = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/editable-promo-items/get-data",
            type        : "GET",
            data        : {categoryDesc: 'DC'},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result.error) {
                    let promoConfig = result.data.promoConfig;
                    let enableConfig = result.data.enableConfig;

                    categoryIdDC = promoConfig.categoryId;

                    $('#budgetYearDC').prop('checked', (!(promoConfig.budgetYear)));
                    $('#entityDC').prop('checked', (!(promoConfig.entity)));
                    $('#groupBrandDC').prop('checked', (!(promoConfig.groupBrand)));
                    $('#distributorDC').prop('checked', (!(promoConfig.distributor)));
                    $('#subActivityTypeDC').prop('checked', (!(promoConfig.subActivityType)));
                    $('#subActivityDC').prop('checked', (!(promoConfig.subActivity)));
                    $('#channelDC').prop('checked', (!(promoConfig.channel)));
                    $('#budgetSourceDC').prop('checked', (!(promoConfig.budgetSource)));
                    $('#startPromoDC').prop('checked', (!(promoConfig.startPromo)));
                    $('#endPromoDC').prop('checked', (!(promoConfig.endPromo)));
                    $('#mechanismDC').prop('checked', (!(promoConfig.mechanism)));
                    $('#investmentDC').prop('checked', (!(promoConfig.investment)));
                    $('#initiatorNotesDC').prop('checked', (!(promoConfig.initiatorNotes)));
                    $('#attachmentDC').prop('checked', (!(promoConfig.attachment)));

                    $('#budgetYearDC').attr('disabled', enableConfig.disabledBudgetYear);
                    $('#entityDC').attr('disabled', enableConfig.disabledEntity);
                    $('#groupBrandDC').attr('disabled', enableConfig.disabledGroupBrand);
                    $('#distributorDC').attr('disabled', enableConfig.disabledDistributor);
                    $('#subActivityTypeDC').attr('disabled', enableConfig.disabledSubActivityType);
                    $('#subActivityDC').attr('disabled', enableConfig.disabledSubActivity);
                    $('#channelDC').attr('disabled', enableConfig.disabledChannel);
                    $('#budgetSourceDC').attr('disabled', enableConfig.disabledBudgetSource);
                    $('#startPromoDC').attr('disabled', enableConfig.disabledStartPromo);
                    $('#endPromoDC').attr('disabled', enableConfig.disabledEndPromo);
                    $('#mechanismDC').attr('disabled', enableConfig.disabledMechanism);
                    $('#investmentDC').attr('disabled', enableConfig.disabledInvestment);
                    $('#initiatorNotesDC').attr('disabled', enableConfig.disabledInitiatorNotes);
                    $('#attachmentDC').attr('disabled', enableConfig.disabledAttachment);
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

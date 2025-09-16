'use strict';

let swalTitle = "Major Changes";
let dialerObject, dialerObjectDC;

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
    let url = "/configuration/major-changes/export-xls-history?period=" + filter_period;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_excel_dc').on('click', function () {
    let filter_period = ($('#filter_period_dc').val()) ?? "";
    let url = "/configuration/major-changes/export-xls-history-dc?period=" + filter_period;
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
    let url = "/configuration/major-changes/submit";
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
    let url = "/configuration/major-changes/submit-dc";
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
            url         : "/configuration/major-changes/get-data",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result.error) {
                    let arr_val = result.data;
                    if (arr_val.length > 0) {
                        let values = arr_val[0];

                        $('#budgetSources').prop('checked', (!!(values.budgetSources)));
                        $('#activity').prop('checked', (!!(values.activity)));
                        $('#subActivity').prop('checked', (!!(values.subActivity)));
                        $('#startPromo').prop('checked', (!!(values.startPromo)));
                        $('#endPromo').prop('checked', (!!(values.endPromo)));
                        $('#activityDesc').prop('checked', (!!(values.activityDesc)));
                        $('#initiatorNotes').prop('checked', (!!(values.initiatorNotes)));
                        $('#incrSales').prop('checked', (!!(values.incrSales)));
                        $('#investment').prop('checked', (!!(values.investment)));
                        $('#roi').prop('checked', (!!(values.roi)));
                        $('#cr').prop('checked', (!!(values.cr)));
                        $('#channel').prop('checked', (!!(values.channel)));
                        $('#subChannel').prop('checked', (!!(values.subChannel)));
                        $('#account').prop('checked', (!!(values.account)));
                        $('#subAccount').prop('checked', (!!(values.subAccount)));
                        $('#region').prop('checked', (!!(values.region)));
                        $('#brand').prop('checked', (!!(values.brand)));
                        $('#sku').prop('checked', (!!(values.sku)));
                        $('#mechanism').prop('checked',  (!!(values.mechanism)));
                        $('#attachment').prop('checked',  (!!(values.attachment)));
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
            url         : "/configuration/major-changes/get-data-dc",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result.error) {
                    let arr_val = result.data;
                    if (arr_val.length > 0) {
                        let values = arr_val[0];
                        $('#groupBrandDC').prop('checked', (!!(values.groupBrand)));
                        $('#distributorDC').prop('checked', (!!(values.distributor)));
                        $('#subCategoryDC').prop('checked', (!!(values.subCategory)));
                        $('#subActivityDC').prop('checked', (!!(values.subActivity)));
                        $('#channelDC').prop('checked', (!!(values.channel)));
                        $('#budgetSourcesDC').prop('checked', (!!(values.budgetSources)));
                        $('#startPromoDC').prop('checked', (!!(values.startPromo)));
                        $('#endPromoDC').prop('checked', (!!(values.endPromo)));
                        $('#mechanismDC').prop('checked', (!!(values.mechanism)));
                        $('#investmentDC').prop('checked', (!!(values.investment)));
                        $('#initiatorNotesDC').prop('checked', (!!(values.initiatorNotes)));
                        $('#attachmentDC').prop('checked', (!!(values.attachment)));
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

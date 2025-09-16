'use strict';
let toggleElement = null;
let toggle = KTToggle.getInstance(toggleElement);

const url_str = new URL(window.location.href);
const i = url_str.searchParams.get("i");

const elTotMatrix = $('#tot_matrix');
const elProcessMatrix = $('#process_matrix');

$(document).ready(function () {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toastr-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "500",
        "hideDuration": "100",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    getListMatrix().then(async function (resMatrix) {
        let cntMatrix = resMatrix.length;
        if (cntMatrix > 0) {
            elTotMatrix.text(cntMatrix);
            let elProgressBar = $('#progress_bar');
            let elInfoSendingProgress = $('#info_sending_progress');
            for (let i=0; i<resMatrix.length; i++) {
                elProcessMatrix.text(i+1);
                elProgressBar.css('width', '0%').attr('aria-valuenow', '0');
                // List Promo
                elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Getting promo list</span>`);
                let promoList = await getListPromo(resMatrix[i]['MatrixId']);

                if (promoList.length === 0) {
                    toastr.warning(`Matrix promo approval  number ${i+1} has not promo`);
                    elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Matrix has not promo</span>`);
                } else {
                    if (promoList.length > 0) {
                        let perc = 0;
                        for (let j=0; j<promoList.length; j++) {
                            // send email
                            if (promoList[j]['cycle'] === 1) {
                                elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Sending email: [APPROVAL NOTIF] Promo requires approval (${promoList[j]['refId']})</span>`);
                            } else {
                                elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Sending email: [APPROVAL NOTIF] Promo Reconciliation requires approval (${promoList[j]['refId']})</span>`);
                            }
                            let reSendEmail = await sendEmail(promoList[j]['promoid'], promoList[j]['refId'], promoList[j]['userid'], promoList[j]['username'], promoList[j]['email'], promoList[j]['cycle']);

                            if (!reSendEmail.error) {
                                if (promoList[j]['cycle'] === 1) {
                                    toastr.success(`[APPROVAL NOTIF] Promo requires approval (${promoList[j]['refId']}) has been sent`);
                                } else {
                                    toastr.success(`[APPROVAL NOTIF] Promo Reconciliation requires approval (${promoList[j]['refId']}) has been sent`);
                                }
                            } else {
                                if (promoList[j]['cycle'] === 1) {
                                    toastr.error(`[APPROVAL NOTIF] Promo requires approval (${promoList[j]['refId']}) failed to send`);
                                } else {
                                    toastr.error(`[APPROVAL NOTIF] Promo Reconciliation requires approval (${promoList[j]['refId']}) failed to send`);
                                }
                            }

                            perc = (((j+1) / promoList.length) * 100).toFixed(0);
                            $('#text_progress').text(perc.toString() + '%');
                            elProgressBar.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());
                        }
                    }
                }

                // finished process
                if (i+1 === resMatrix.length) {
                    $('#matrix_loading').removeClass('spinner-border').addClass('fa fa-check process_success').text('');
                    $('#text_progress').text('100%');
                    elProgressBar.css('width', '100%').attr('aria-valuenow', '100');
                    elInfoSendingProgress.html(`<span class="text-gray-800 info_send_email_finish">Matrix promo approval has been processed</span>`);
                }
            }
        }
     })
});

const getListMatrix = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/tools/upload-matrix-approval/process/list-matrix",
            type: "GET",
            data: {processId: i},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    return resolve(result['data']);
                } else {
                    return resolve([]);
                }
            },
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                return reject([]);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListPromo = (matrixId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/tools/upload-matrix-approval/process/list-promo",
            type: "GET",
            data: {matrixId: matrixId},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    return resolve(result['data']);
                } else {
                    return resolve([]);
                }
            },
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                return reject([]);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const sendEmail = (promoId, refId, profileApprover, nameApprover, emailApprover, cycle) => {
    return new Promise((resolve) => {
        let formData = new FormData();
        formData.append('promoId', promoId);
        formData.append('refId', refId);
        formData.append('profileApprover', profileApprover);
        formData.append('nameApprover', nameApprover);
        formData.append('emailApprover', emailApprover);
        formData.append('cycle', cycle);
        $.get('/refresh-csrf').done(function(data) {
            let elMeta = $('meta[name="csrf-token"]');
            elMeta.attr('content', data)
            $.ajaxSetup({
                headers: {
                    'X-CSRF-TOKEN': elMeta.attr('content')
                }
            });
            $.ajax({
                url: '/tools/upload-matrix-approval/process/send-email',
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                success: function (result) {
                    if (result.error) {
                        return resolve(result);
                    } else {
                        return resolve(result);
                    }
                },
                complete: function () {

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                    return resolve(false);
                }
            });
        });
    }).catch((e) => {
        console.log(e);
    });
}

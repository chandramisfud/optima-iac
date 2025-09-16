'use strict';
let toggleElement = null;
let toggle = KTToggle.getInstance(toggleElement);

const url_str = new URL(window.location.href);
const matrixId = url_str.searchParams.get("i");

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

    getListPromo(matrixId).then(async function (resPromo) {
        let cntPromo = resPromo.length;
        let elProgressBar = $('#progress_bar');
        let elInfoSendingProgress = $('#info_sending_progress');

        if (cntPromo > 0) {
            let perc = 0;

            for (let i=0; i<resPromo.length; i++) {
                // send email
                if (resPromo[i]['cycle'] === 1) {
                    elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Sending email: [APPROVAL NOTIF] Promo requires approval (${resPromo[i]['refId']})</span>`);
                } else {
                    elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Sending email: [APPROVAL NOTIF] Promo Reconciliation requires approval (${resPromo[i]['refId']})</span>`);
                }
                let reSendEmail = await sendEmail(resPromo[i]['promoid'], resPromo[i]['refId'], resPromo[i]['userid'], resPromo[i]['username'], resPromo[i]['email'], resPromo[i]['cycle']);

                if (!reSendEmail.error) {
                    if (resPromo[i]['cycle'] === 1) {
                        toastr.success(`[APPROVAL NOTIF] Promo requires approval (${resPromo[i]['refId']}) has been sent`);
                    } else {
                        toastr.success(`[APPROVAL NOTIF] Promo Reconciliation requires approval (${resPromo[i]['refId']}) has been sent`);
                    }
                } else {
                    if (resPromo[i]['cycle'] === 1) {
                        toastr.error(`[APPROVAL NOTIF] Promo requires approval (${resPromo[i]['refId']}) failed to send`);
                    } else {
                        toastr.error(`[APPROVAL NOTIF] Promo Reconciliation requires approval (${resPromo[i]['refId']}) failed to send`);
                    }
                }

                perc = (((i+1) / resPromo.length) * 100).toFixed(0);
                $('#text_progress').text(perc.toString() + '%');
                elProgressBar.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());

                if (i+1 === resPromo.length) {
                    $('#matrix_loading').removeClass('spinner-border').addClass('fa fa-check process_success').text('');
                    $('#text_progress').text('100%');
                    elProgressBar.css('width', '100%').attr('aria-valuenow', '100');
                    elInfoSendingProgress.html(`<span class="text-gray-800 info_send_email_finish">Matrix promo approval has been processed</span>`);
                }
            }

        } else {
            toastr.warning(`Matrix promo approval has not promo`);
            elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Matrix has not promo</span>`);

            //Finished
            $('#matrix_loading').removeClass('spinner-border').addClass('fa fa-check process_success').text('');
            $('#text_progress').text('100%');
            elProgressBar.css('width', '100%').attr('aria-valuenow', '100');
            elInfoSendingProgress.html(`<span class="text-gray-800 info_send_email_finish">Matrix promo approval has been processed</span>`);
        }
    });
});

const getListPromo = (matrixId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/matrix/promoapproval/process/list-promo",
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
        $.get('/refresh-csrf').done(function (data) {
            let elMeta = $('meta[name="csrf-token"]');
            elMeta.attr('content', data)
            $.ajaxSetup({
                headers: {
                    'X-CSRF-TOKEN': elMeta.attr('content')
                }
            });
            $.ajax({
                url: '/master/matrix/promoapproval/process/send-email',
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

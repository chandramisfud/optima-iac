'use strict';
let toggleElement = null;
let toggle = KTToggle.getInstance(toggleElement);

const url_str = new URL(window.location.href);
const batchId = url_str.searchParams.get("i");

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

    getListPromo(batchId).then(async function (resPromo) {
        let cntPromo = resPromo.length;
        let elProgressBar = $('#progress_bar');
        let elTxtProgress = $('#text_progress');
        let elInfoSendingProgress = $('#info_sending_progress');

        if (cntPromo > 0) {
            let perc = 0;

            for (let i=0; i<resPromo.length; i++) {
                // send email
                elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Sending email: [APPROVAL NOTIF] Promo requires approval (${resPromo[i]['RefId']})</span>`);
                let reSendEmail = await sendEmail(resPromo[i]['Id'], resPromo[i]['RefId'], resPromo[i]['userIdApprover'], resPromo[i]['userNameApprover'], resPromo[i]['emailApprover']);

                if (!reSendEmail.error) {
                    toastr.success(`[APPROVAL NOTIF] Promo requires approval (${resPromo[i]['RefId']}) has been sent`);
                } else {
                    toastr.error(`[APPROVAL NOTIF] Promo requires approval (${resPromo[i]['RefId']}) failed to send`);
                }

                perc = (((i+1) / resPromo.length) * 100).toFixed(0);
                elTxtProgress.text(perc.toString() + '%');
                elProgressBar.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());

                if (i+1 === resPromo.length) {
                    $('#matrix_loading').removeClass('spinner-border').addClass('fa fa-check process_success').text('');
                    elTxtProgress.text('100%');
                    elProgressBar.css('width', '100%').attr('aria-valuenow', '100');
                    elInfoSendingProgress.html(`<span class="text-gray-800 info_send_email_finish">Budget deployment has been processed</span>`);
                }
            }

        } else {
            toastr.warning(`Budget deployment has not promo`);
            elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Budget has not promo</span>`);

            //Finished
            $('#matrix_loading').removeClass('spinner-border').addClass('fa fa-check process_success').text('');
            elTxtProgress.text('100%');
            elProgressBar.css('width', '100%').attr('aria-valuenow', '100');
            elInfoSendingProgress.html(`<span class="text-gray-800 info_send_email_finish">Budget deployment has been processed</span>`);
        }
    });
});

const getListPromo = (batchId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/budget/deployment/process/list-promo",
            type: "GET",
            data: {batchId: batchId},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    return resolve(result['data']['promoApproval']);
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
                url: '/budget/deployment/process/send-email',
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

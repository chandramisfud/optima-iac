'use strict';

var dt_list_upload;
var swalTitle = "Budget TT Consol Upload RC";
heightContainer = 430;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_list_upload = $('#dt_list_upload').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        processing: true,
        serverSide: false,
        paging: true,
        ordering: false,
        searching: false,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Description',
                data: 'doc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Status',
                data: 'status',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });
});

$('#btn_upload').on('click', function () {
    if ( document.getElementById("file").files.length > 0 ){
        dt_list_upload.clear().draw();
        let span = document.getElementById("upload_result");
        span.textContent = "";
        let e = document.querySelector("#btn_upload");
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        blockUI.block();
        let formData = new FormData($('#form_upload')[0]);
        $.get('/refresh-csrf').done(function(data) {
            $('meta[name="csrf-token"]').attr('content', data);
            $.ajaxSetup({
                headers: {
                    'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                }
            });
            $.ajax({
                url             : '/budget/tt-console/upload-xls-rc',
                data            : formData,
                type            : 'POST',
                async           : true,
                dataType        : 'JSON',
                cache           : false,
                contentType     : false,
                processData     : false,
                success: async function (result) {
                    if (!result.error) {
                        let errMessage = result['data']['errMessage'];
                        let uploadStat = result['data']['uploadStat'];
                        let promoList = result['data']['promoList'];
                        if (promoList.length > 0) {
                            let elProgress = $('#d_progress');
                            let elInfoSendingProgress = $('#info_sending_progress');
                            let perc = 0;
                            elProgress.removeClass('d-none');
                            for (let i = 1; i <= promoList.length; i++) {

                                let userApprover = promoList[i-1]['approver'];
                                let nameApprover = promoList[i-1]['userNameApprover'];
                                let promoId = promoList[i-1]['promoId'];
                                let emailApprover = promoList[i-1]['emailApprover'];

                                let subject;
                                if(promoList[i-1]['reconciled']){
                                    subject = `[APPROVAL NOTIF] Promo Reconciliation requires approval (${promoList[i-1]['promoRefId']})`;
                                    elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Sending email: [APPROVAL NOTIF] Promo Reconciliation requires approval (${promoList[i-1]['promoRefId']})</span>`);
                                    await sendEmailRecon(userApprover, nameApprover, promoId, emailApprover, subject);
                                } else {
                                    subject = `[APPROVAL NOTIF] Promo requires approval (${promoList[i-1]['promoRefId']})`;
                                    elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Sending email: [APPROVAL NOTIF] Promo requires approval (${promoList[i-1]['promoRefId']})</span>`);
                                    await sendEmail(userApprover, nameApprover, promoId, emailApprover, subject);
                                }

                                perc = ((i / promoList.length) * 100).toFixed(0);
                                $('#text_progress').text(perc.toString() + '%');
                                let progress_import = $('#progress_bar');
                                progress_import.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());
                                if (i === promoList.length) {
                                    progress_import.css('width', '0%').attr('aria-valuenow', '0');
                                    elProgress.addClass('d-none');
                                }
                            }
                        }

                        if (errMessage.length > 0) {
                            dt_list_upload.rows.add(errMessage).draw();
                        }

                        span = document.getElementById("upload_result");
                        let total_upload = formatMoney(uploadStat[0]['totUpload'],0);
                        let total_failed = formatMoney(uploadStat[0]['totFailed'],0);
                        let total_success = formatMoney(uploadStat[0]['totSuccess'],0);
                        span.textContent = "Total upload : " + total_upload + " ; failed : " + total_failed + " ; success : " + total_success;

                        Swal.fire({
                            title: 'Files Uploaded!',
                            icon: "success",
                            html: 'All failed Budget TT Consol RC Upload items, are shown on the list',
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: { confirmButton: "btn btn-primary" }
                        });

                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                        blockUI.release();
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: { confirmButton: "btn btn-primary" }
                        });

                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                        blockUI.release();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR.responseJSON.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-primary" }
                    });

                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    blockUI.release();
                }
            });
        });
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please attach file",
            icon: "warning",
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

const sendEmail = (userApprover, nameApprover, promoId, emailApprover, subject) => {
    return new Promise((resolve, reject) => {
        let formData = new FormData();

        formData.append('userApprover', userApprover);
        formData.append('nameApprover', nameApprover);
        formData.append('promoId', promoId);
        formData.append('emailApprover', emailApprover);
        formData.append('subject', subject);

        $.ajax({
            url             : "/budget/tt-console/send-email",
            data            : formData,
            type            : 'POST',
            async           : true,
            dataType        : 'JSON',
            cache           : false,
            contentType     : false,
            processData     : false,
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const sendEmailRecon = (userApprover, nameApprover, promoId, emailApprover, subject) => {
    return new Promise((resolve, reject) => {
        let formData = new FormData();

        formData.append('userApprover', userApprover);
        formData.append('nameApprover', nameApprover);
        formData.append('promoId', promoId);
        formData.append('emailApprover', emailApprover);
        formData.append('subject', subject);

        $.ajax({
            url             : "/budget/tt-console/recon/send-email",
            data            : formData,
            type            : 'POST',
            async           : true,
            dataType        : 'JSON',
            cache           : false,
            contentType     : false,
            processData     : false,
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

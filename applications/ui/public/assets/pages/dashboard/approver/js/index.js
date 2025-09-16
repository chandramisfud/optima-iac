'use strict';

// filter approval request
const targetApprovalRequest = document.querySelector(".card_approval_request");
const blockUIApprovalRequest = new KTBlockUI(targetApprovalRequest, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// filter approval request response time
const targetApprovalResponseTime = document.querySelector(".card_approval_request_response_time");
const blockUIApprovalResponseTime = new KTBlockUI(targetApprovalResponseTime, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// filter promo approval
const targetPromoApproval = document.querySelector(".card_promo_approval");
const blockUIPromoApproval = new KTBlockUI(targetPromoApproval, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// filter approver league
const targetApproverLeague = document.querySelector(".card_approver_league");
const blockUIApproverLeague = new KTBlockUI(targetApproverLeague, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    let elPeriodStart = $('#filter_period_start');
    let elPeriodEnd = $('#filter_period_end');

    elPeriodStart.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        defaultDate: new Date(new Date().getFullYear() + '-01-01')
    });
    elPeriodStart.next().css('background-color', '#fff !important');

    elPeriodEnd.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        defaultDate: new Date(new Date().getFullYear() + '-12-31')
    });
    elPeriodEnd.next().css('background-color', '#fff !important');

    blockUIApprovalRequest.block();
    blockUIApprovalResponseTime.block();
    blockUIPromoApproval.block();
    blockUIApproverLeague.block();

    getDataHeaderDashboard(elPeriodStart.val(), elPeriodEnd.val()).then(function () {
        blockUIApprovalRequest.release();
        blockUIApprovalResponseTime.release();
        blockUIPromoApproval.release();
        blockUIApproverLeague.release();
    });
});

$('#filter_period_start').on('change', function () {
    let el_start = $('#filter_period_start');
    let el_end = $('#filter_period_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_end.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(startDate)
        });
        el_end.next().css('background-color', '#fff !important');
    }
});

$('#filter_period_end').on('change', function () {
    let el_start = $('#filter_period_start');
    let el_end = $('#filter_period_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_start.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(endDate)
        });
        el_start.next().css('background-color', '#fff !important');
    }
});

$('#btn_view').on('click', async function () {
    let btn = document.getElementById('btn_view');
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    blockUIApprovalRequest.block();
    blockUIApprovalResponseTime.block();
    blockUIPromoApproval.block();
    blockUIApproverLeague.block();

    let elPeriodStart = $('#filter_period_start');
    let elPeriodEnd = $('#filter_period_end');

    getDataHeaderDashboard(elPeriodStart.val(), elPeriodEnd.val()).then(function () {
        blockUIApprovalRequest.release();
        blockUIApprovalResponseTime.release();
        blockUIPromoApproval.release();
        blockUIApproverLeague.release();
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });

});

const getDataHeaderDashboard = (periodStart, periodEnd) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dashboard/approver/data",
            type: "GET",
            dataType: 'json',
            data: {periodStart:periodStart, periodEnd:periodEnd},
            async: true,
            success: function (result) {
                let res = result.data;
                if (res.kpiApproverResults1.length > 0) {
                    let approval_request = (res.kpiApproverResults1[0].approval_request ?? 0);
                    let avg_approval_response_time = (res.kpiApproverResults1[0].avg_approval_response_time ?? 0);
                    let promo_approval_pct = (res.kpiApproverResults1[0].promo_approval_pct ?? 0);
                    let promo_approval_sla_max_3days = (res.kpiApproverResults1[0].promo_approval_sla_max_3days ?? 0);
                    let promo_approval_sla_max_3days_bobot = (res.kpiApproverResults1[0].promo_approval_sla_max_3days_bobot ?? 0);
                    let promo_approval_sla_max_3days_subscore = (res.kpiApproverResults1[0].promo_approval_sla_max_3days_subscore ?? 0);

                    $('#txt_approval_request').html(formatMoney(approval_request, 0));
                    $('#txt_approval_request_response_time').html(formatMoney(avg_approval_response_time, 2) + ' <span class="fs-2 text-gray-600 fw-normal">Days</span>');
                    $('#txt_promo_approval').html(formatMoney(promo_approval_pct, 2) + '<span class="text-gray-600 fw-normal ms-2">%</span>');
                    $('#txt_sub_score').html(formatMoney(promo_approval_sla_max_3days_subscore, 2) + '/' +formatMoney(promo_approval_sla_max_3days_bobot, 0));

                    if (promo_approval_sla_max_3days > 0) {
                        $('#progress_bar').css('width', promo_approval_sla_max_3days+'%');
                        $('#text_progress').text(promo_approval_sla_max_3days+'%')
                    } else {
                        let elProgressBar = $('#progress_bar');
                        elProgressBar.removeClass('bg-58dd91');
                        elProgressBar.css('width', '100%').css('background-color', '#f5f8fa');
                        $('#text_progress').text(promo_approval_sla_max_3days+'%')
                    }
                }

                // approver league
                let html_top = '';
                let html_bottom = '';
                if (res.kpiApproverResults2.length > 0) {
                    for (let i=0;i<5;i++) {
                        html_top += '<div class="row px-3 py-2">\n' +
                            '                                            <div class="col-lg-6 col-6">\n' +
                            '                                                <span class="fw-normal">'+ res.kpiApproverResults2[i].userapprover_top +'</span>\n' +
                            '                                            </div>\n' +
                            '                                            <div class="col-lg-2 col-6 text-end">\n' +
                            '                                                <span class="fw-normal">'+ formatMoney(res.kpiApproverResults2[i].score_top, 2) +'%</span>\n' +
                            '                                            </div>\n' +
                            '                                            <div class="col-lg-2 col-6 text-end">\n' +
                            '                                                <span class="fw-normal">'+ formatMoney(res.kpiApproverResults2[i].num_of_optima_top, 2) +'</span>\n' +
                            '                                            </div>\n' +
                            '                                            <div class="col-lg-2 col-6 text-end">\n' +
                            '                                                <span class="fw-normal">'+ formatMoney(res.kpiApproverResults2[i].avg_days_top, 2) +'</span>\n' +
                            '                                            </div>\n' +
                            '                                        </div>';

                        html_bottom += '<div class="row px-3 py-2">\n' +
                            '                                            <div class="col-lg-6 col-6">\n' +
                            '                                                <span class="fw-normal">'+ res.kpiApproverResults2[i].userapprover_bottom +'</span>\n' +
                            '                                            </div>\n' +
                            '                                            <div class="col-lg-2 col-6 text-end">\n' +
                            '                                                <span class="fw-normal">'+ formatMoney(res.kpiApproverResults2[i].score_bottom, 2) +'%</span>\n' +
                            '                                            </div>\n' +
                            '                                            <div class="col-lg-2 col-6 text-end">\n' +
                            '                                                <span class="fw-normal">'+ formatMoney(res.kpiApproverResults2[i].num_of_optima_bottom, 2) +'</span>\n' +
                            '                                            </div>\n' +
                            '                                            <div class="col-lg-2 col-6 text-end">\n' +
                            '                                                <span class="fw-normal">'+ formatMoney(res.kpiApproverResults2[i].avg_days_bottom, 2) +'</span>\n' +
                            '                                            </div>\n' +
                            '                                        </div>';
                    }
                } else {
                    html_top += '<div class="row px-3 py-2">\n' +
                        '                                            <div class="col-lg-6 col-6">\n' +
                        '                                                <span class="fw-normal">-</span>\n' +
                        '                                            </div>\n' +
                        '                                            <div class="col-lg-2 col-6 text-end">\n' +
                        '                                                <span class="fw-normal">0.00%</span>\n' +
                        '                                            </div>\n' +
                        '                                            <div class="col-lg-2 col-6 text-end">\n' +
                        '                                                <span class="fw-normal">0.00</span>\n' +
                        '                                            </div>\n' +
                        '                                            <div class="col-lg-2 col-6 text-end">\n' +
                        '                                                <span class="fw-normal">0.00</span>\n' +
                        '                                            </div>\n' +
                        '                                        </div>';

                    html_bottom += '<div class="row px-3 py-2">\n' +
                        '                                            <div class="col-lg-6 col-6">\n' +
                        '                                                <span class="fw-normal">-</span>\n' +
                        '                                            </div>\n' +
                        '                                            <div class="col-lg-2 col-6 text-end">\n' +
                        '                                                <span class="fw-normal">0.00%</span>\n' +
                        '                                            </div>\n' +
                        '                                            <div class="col-lg-2 col-6 text-end">\n' +
                        '                                                <span class="fw-normal">0.00</span>\n' +
                        '                                            </div>\n' +
                        '                                            <div class="col-lg-2 col-6 text-end">\n' +
                        '                                                <span class="fw-normal">0.00</span>\n' +
                        '                                            </div>\n' +
                        '                                        </div>';
                }
                $('#list_top5').html(html_top);
                $('#list_bottom5').html(html_bottom);

                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(null);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

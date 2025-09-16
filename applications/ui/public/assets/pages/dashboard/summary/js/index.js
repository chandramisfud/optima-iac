'use strict';

// row 1 promo plan card
const targetRow1PromoPlan = document.querySelector(".card_row1_promo_plan");
const blockUIRow1PromoPlan = new KTBlockUI(targetRow1PromoPlan, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// row 1 overall score card
const targetRow1OverallScore = document.querySelector(".card_row1_overall_score");
const blockUIRow1OverallScore = new KTBlockUI(targetRow1OverallScore, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// row 1 sub score card
const targetRow1SubScore = document.querySelector(".card_row1_sub_score");
const blockUIRow1SubScore = new KTBlockUI(targetRow1SubScore, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// row 1 approval request card
const targetRow1ApprovalRequest = document.querySelector(".card_row1_approval_request");
const blockUIRow1ApprovalRequest = new KTBlockUI(targetRow1ApprovalRequest, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// row 1 approval request response time card
const targetRow1ApprovalRequestResponseTime = document.querySelector(".card_row1_approval_request_response_time");
const blockUIRow1ApprovalRequestResponseTime = new KTBlockUI(targetRow1ApprovalRequestResponseTime, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// row 2 promo plan card
const targetRow2PromoPlan = document.querySelector(".card_row2_promo_plan");
const blockUIRow2PromoPlan = new KTBlockUI(targetRow2PromoPlan, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// row 2 accuracy promo card
const targetRow2AccuracyPromo = document.querySelector(".card_row2_accuracy_promo");
const blockUIRow2AccuracyPromo = new KTBlockUI(targetRow2AccuracyPromo, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// row 2 accuracy skp card
const targetRow2AccuracySKP = document.querySelector(".card_row2_accuracy_skp");
const blockUIRow2AccuracySKP = new KTBlockUI(targetRow2AccuracySKP, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// row 2 recon monitoring card
const targetRow2ReconMonitoring = document.querySelector(".card_row2_recon_monitoring");
const blockUIRow2ReconMonitoring = new KTBlockUI(targetRow2ReconMonitoring, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// row 2 promo apporval sla card
const targetRow2PromoSLAApproval = document.querySelector(".card_row2_promo_approval_sla");
const blockUIRow2PromoSLAApproval = new KTBlockUI(targetRow2PromoSLAApproval, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// creator summary card
const targetCreatorSummary = document.querySelector(".card_creator_summary");
const blockUICreatorSummary = new KTBlockUI(targetCreatorSummary, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// creator standing card
const targetCreatorStanding = document.querySelector(".card_creator_standing");
const blockUICreatorStanding = new KTBlockUI(targetCreatorStanding, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// approval standing card
const targetApproverStanding = document.querySelector(".card_approver_standing");
const blockUIApproverStanding = new KTBlockUI(targetApproverStanding, {
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
    DonutAccuracy(0,0,0);
    DonutSKP(0,0,0);

    let elPeriodStart = $('#filter_period_start');
    let elPeriodEnd = $('#filter_period_end');
    let elDatePromoPlan = $('#filter_date_promo_plan');

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
    });
    elPeriodEnd.next().css('background-color', '#fff !important');

    elDatePromoPlan.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        defaultDate: new Date(new Date().getFullYear() + '-01-01')
    });
    elDatePromoPlan.next().css('background-color', '#fff !important');

    let isMonitoring = ($('#checkbox_promo_plan').attr("checked") ? 1 : 0);
    loadData(elPeriodStart.val(), elPeriodEnd.val(), isMonitoring, elDatePromoPlan.val());
});

$('#filter_period_start').on('change', function () {
    let startDate = new Date($(this).val()).getTime();
    let elDatePromo = $('#filter_date_promo_plan');
    elDatePromo.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        defaultDate: new Date(startDate)
    });
    elDatePromo.next().css('background-color', '#fff !important');
});

$('#btn_accuracy_promo_plan_hidden').on('click', function () {
    let el = $('#accuracy_promo_plan_hidden');
    let el_skp = $('#accuracy_skp_hidden');
    el_skp.addClass('d-none');
    if (el.hasClass('d-none')) {
        el.removeClass('d-none');
    } else {
        el.addClass('d-none');
    }
});

$('#btn_accuracy_skp_hidden').on('click', function () {
    let el = $('#accuracy_skp_hidden');
    let el_promo_plan = $('#accuracy_promo_plan_hidden');
    el_promo_plan.addClass('d-none');
    if (el.hasClass('d-none')) {
        el.removeClass('d-none');
    } else {
        el.addClass('d-none');
    }
});

$('#btn_view').on('click', async function () {
    let elPeriodStart = $('#filter_period_start');
    let elPeriodEnd = $('#filter_period_end');
    let elDatePromoPlan = $('#filter_date_promo_plan');
    let isMonitoring = ($('#checkbox_promo_plan').attr("checked") ? 1 : 0);

    await loadData(elPeriodStart.val(), elPeriodEnd.val(), isMonitoring, elDatePromoPlan.val())
});

$('#btn_download').on('click', function () {
    if ($('#export_list').hasClass('d-none')) {
        $('#export_list').removeClass('d-none');
    } else {
        $('#export_list').addClass('d-none');
    }
});

$('#btn_download_xls').on('click', function () {
    let elPeriodStart = $('#filter_period_start');
    let elPeriodEnd = $('#filter_period_end');
    let url = '/dashboard/summary/export-xls?create_from=' + elPeriodStart.val() + '&create_to=' + elPeriodEnd.val();
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_download_pdf').on('click', function () {
    html2canvas(document.getElementById('print'),{
        onrendered: function(canvas) {
            var img = canvas.toDataURL("image/png");
            var doc = new jsPDF();
            doc.addImage(img, 'JPEG', 5,5,200,297);
            doc.save('Export.pdf')
        }
    });
});

$('#btn_download_xls_dtl').on('click', function () {
    let elPeriodStart = $('#filter_period_start');
    let elPeriodEnd = $('#filter_period_end');
    let elDatePromoPlan = $('#filter_date_promo_plan');
    let isMonitoring = ($('#checkbox_promo_plan').attr("checked") ? 1 : 0);
    let url = '/dashboard/summary/export-xls-detail?create_from=' + elPeriodStart.val() + '&create_to=' + elPeriodEnd.val() + '&date_monitoring=' + elDatePromoPlan.val() + '&period_monitoring=' + isMonitoring;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const loadData = async (create_start, create_end, is_promo_monitoring, date_monitoring) => {
    let btn = document.getElementById('btn_view');
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    blockUIRow1PromoPlan.block();
    blockUIRow1OverallScore.block();
    blockUIRow1SubScore.block();
    blockUIRow2PromoPlan.block();
    blockUIRow2AccuracyPromo.block();
    blockUIRow2AccuracySKP.block();
    blockUIRow2ReconMonitoring.block();
    getDataSummary(create_start, create_end, is_promo_monitoring, date_monitoring).then(function () {
        blockUIRow1PromoPlan.release();
        blockUIRow1OverallScore.release();
        blockUIRow1SubScore.release();
        blockUIRow2PromoPlan.release();
        blockUIRow2AccuracyPromo.release();
        blockUIRow2AccuracySKP.release();
        blockUIRow2ReconMonitoring.release();
    });

    blockUICreatorSummary.block();
    getDataLeagues(create_start, create_end).then(function () {
        blockUICreatorSummary.release();
    });

    blockUICreatorStanding.block();
    getDataCreatorStanding(create_start, create_end).then(function () {
        blockUICreatorStanding.release();
    });

    blockUIApproverStanding.block();
    blockUIRow1ApprovalRequest.block();
    blockUIRow1ApprovalRequestResponseTime.block();
    blockUIRow2PromoSLAApproval.block();
    getDataApproverStanding(create_start, create_end).then(function () {
        blockUIApproverStanding.release();
        blockUIRow1ApprovalRequest.release();
        blockUIRow1ApprovalRequestResponseTime.release();
        blockUIRow2PromoSLAApproval.release();
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
}

const getDataSummary = (createFrom, createTo, periodMonitoring, dateMonitoring) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dashboard/summary/data",
            type: "GET",
            dataType: 'json',
            data: {
                create_from:createFrom,
                create_to:createTo,
                period_monitoring:periodMonitoring,
                date_monitoring:dateMonitoring
            },
            async: true,
            success: async function (result) {
                let res = result.data;

                if (res.length > 0) {
                    let val = res[0];

                    // dashboard row 1 promo
                    $('#txtPromoPlanEvaluated').text(val.promoPlanEvaluated);
                    $('#txtOverallScorePct').html(formatMoney((val.overall_Score_pct), 2) + '<span class="text-gray-600 fw-normal ms-2">%</span>');
                    $('#txtProgressOverallScorePct').text(formatMoney((val.overall_Score_pct), 2) + '%');
                    $('#progress_overall_score').css('width', val.overall_Score_pct + '%');
                    $('#txtSubScorePromoPlan').html(formatMoney(val.scoreboardPromoPlan,2) + ' <span class="text-gray-900">/ '+val.subscorePromoPlan_bobot+'</span>');
                    $('#txtSubScoreAccuracyPromo').html(formatMoney(val.scoreboardAccuracy_OptimaInput_vs_PromoPlan,2) + ' <span class="text-gray-900">/ '+val.subscoreAccuracy_OptimaInput_vs_PromoPlan_bobot+'</span>');
                    $('#txtSubScoreAccuracySKP').html(formatMoney(val.scoreboardAccuracy_SKP_vs_Optima,2) + ' <span class="text-gray-900">/ '+val.subscoreAccuracy_SKP_vs_Optima_bobot+'</span>');
                    $('#txtSubScoreReconMonitoring').html(formatMoney(val.scoreboardReconMonitoring,2) + ' <span class="text-gray-900">/ '+val.subscoreReconMonitoring_bobot+'</span>');

                    // dashboard row 1 approval



                    // dashboard row 2 Promo
                    if (val.promoPlanSubmittedBfrQuarterStart90_Score < 60) {
                        $('#txtPromoPlanSubmittedBfrQuarterStart90_Score').text(formatMoney(val.promoPlanSubmittedBfrQuarterStart90_Score, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                    } else if (val.promoPlanSubmittedBfrQuarterStart90_Score >= 60 && val.promoPlanSubmittedBfrQuarterStart90_Score < 80) {
                        $('#txtPromoPlanSubmittedBfrQuarterStart90_Score').text(formatMoney(val.promoPlanSubmittedBfrQuarterStart90_Score, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                    } else if (val.promoPlanSubmittedBfrQuarterStart90_Score >=80) {
                        $('#txtPromoPlanSubmittedBfrQuarterStart90_Score').text(formatMoney(val.promoPlanSubmittedBfrQuarterStart90_Score, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                    }

                    if (val.scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct < 60) {
                        $('#txtScoreboardAccuracy_OptimaInput_vs_PromoPlan_pct').text(formatMoney(val.scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                    } else if (val.scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >= 60 && val.scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct < 80) {
                        $('#txtScoreboardAccuracy_OptimaInput_vs_PromoPlan_pct').text(formatMoney(val.scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                    } else if (val.scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >=80) {
                        $('#txtScoreboardAccuracy_OptimaInput_vs_PromoPlan_pct').text(formatMoney(val.scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                    }

                    if (val.scoreboardAccuracy_SKP_vs_Optima_pct < 60) {
                        $('#txtScoreboardAccuracy_SKP_vs_Optima_pct').text(formatMoney(val.scoreboardAccuracy_SKP_vs_Optima_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                    } else if (val.scoreboardAccuracy_SKP_vs_Optima_pct >= 60 && val.scoreboardAccuracy_SKP_vs_Optima_pct < 80) {
                        $('#txtScoreboardAccuracy_SKP_vs_Optima_pct').text(formatMoney(val.scoreboardAccuracy_SKP_vs_Optima_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                    } else if (val.scoreboardAccuracy_SKP_vs_Optima_pct >=80) {
                        $('#txtScoreboardAccuracy_SKP_vs_Optima_pct').text(formatMoney(val.scoreboardAccuracy_SKP_vs_Optima_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                    }

                    if (val.recon_pct < 60) {
                        $('#txtRecon_pct').text(formatMoney(val.recon_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                    } else if (val.recon_pct >= 60 && val.recon_pct < 80) {
                        $('#txtRecon_pct').text(formatMoney(val.recon_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                    } else if (val.recon_pct >=80) {
                        $('#txtRecon_pct').text(formatMoney(val.recon_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                    }

                    // Dashboard hidden Promo
                    $('#txtOptimaPeriodMatch_Score').html(val.optimaPeriodMatch_Score.toFixed(2) + '<span class="fw-normal">/ '+val.optimaPeriodMatch_bobot+'</span></span>');
                    $('#txtOptimaDescAndMechanismMatch_Score').html(val.optimaDescAndMechanismMatch_Score.toFixed(2) + '<span class="fw-normal">/ '+val.optimaDescAndMechanismMatch_bobot+'</span></span>');
                    $('#txtOptimaAmountMatch_Score').html(val.optimaAmountMatch_Score.toFixed(2) + '<span class="fw-normal">/ '+val.optimaAmountMatch_bobot+'</span></span>');
                    $('#txtOptimaCreationBrfActivityStart60_Score').html(val.optimaCreationBrfActivityStart60_Score.toFixed(2) + '<span class="fw-normal">/ '+val.optimaCreationBrfActivityStart60_bobot+'</span></span>');

                    if (val.optimaPeriodMatch_pct <60) {
                        $('#txtOptimaPeriodMatch_pct').text(formatMoney(val.optimaPeriodMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                        $('#symOptimaPeriodMatch_pct').removeClass('bg-00d23b').removeClass('bg-ffb822').addClass('bg-ff1616');
                    } else if (val.optimaPeriodMatch_pct >=60 && val.optimaPeriodMatch_pct < 80) {
                        $('#txtOptimaPeriodMatch_pct').text(formatMoney(val.optimaPeriodMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                        $('#symOptimaPeriodMatch_pct').removeClass('bg-00d23b').removeClass('bg-ff1616').addClass('bg-ffb822');
                    } else if (val.optimaPeriodMatch_pct >= 80) {
                        $('#txtOptimaPeriodMatch_pct').text(formatMoney(val.optimaPeriodMatch_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                        $('#symOptimaPeriodMatch_pct').removeClass('bg-ffb822').removeClass('bg-ff1616').addClass('bg-00d23b');
                    }

                    if (val.optimaDescAndMechanismMatch_pct <60) {
                        $('#txtOptimaDescAndMechanismMatch_pct').text(formatMoney(val.optimaDescAndMechanismMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                        $('#symOptimaDescAndMechanismMatch_pct').removeClass('bg-00d23b').removeClass('bg-ffb822').addClass('bg-ff1616');
                    } else if (val.optimaDescAndMechanismMatch_pct >=60 && val.optimaDescAndMechanismMatch_pct < 80) {
                        $('#txtOptimaDescAndMechanismMatch_pct').text(formatMoney(val.optimaDescAndMechanismMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                        $('#symOptimaDescAndMechanismMatch_pct').removeClass('bg-00d23b').removeClass('bg-ff1616').addClass('bg-ffb822');
                    } else if (val.optimaDescAndMechanismMatch_pct >= 80) {
                        $('#txtOptimaDescAndMechanismMatch_pct').text(formatMoney(val.optimaDescAndMechanismMatch_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                        $('#symOptimaDescAndMechanismMatch_pct').removeClass('bg-ffb822').removeClass('bg-ff1616').addClass('bg-00d23b');
                    }

                    if (val.optimaAmountMatch_pct <60) {
                        $('#txtOptimaAmountMatch_pct').text(formatMoney(val.optimaAmountMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                        $('#symOptimaAmountMatch_pct').removeClass('bg-00d23b').removeClass('bg-ffb822').addClass('bg-ff1616');
                    } else if (val.optimaAmountMatch_pct >=60 && val.optimaAmountMatch_pct < 80) {
                        $('#txtOptimaAmountMatch_pct').text(formatMoney(val.optimaAmountMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                        $('#symOptimaAmountMatch_pct').removeClass('bg-00d23b').removeClass('bg-ff1616').addClass('bg-ffb822');
                    } else if (val.optimaAmountMatch_pct >= 80) {
                        $('#txtOptimaAmountMatch_pct').text(formatMoney(val.optimaAmountMatch_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                        $('#symOptimaAmountMatch_pct').removeClass('bg-ffb822').removeClass('bg-ff1616').addClass('bg-00d23b');
                    }

                    if (val.optimaCreationBrfActivityStart60_pct <60) {
                        $('#txtOptimaCreationBrfActivityStart60_pct').text(formatMoney(val.optimaCreationBrfActivityStart60_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                        $('#symOptimaCreationBrfActivityStart60_pct').removeClass('bg-00d23b').removeClass('bg-ffb822').addClass('bg-ff1616');
                    } else if (val.optimaCreationBrfActivityStart60_pct >=60 && val.optimaCreationBrfActivityStart60_pct < 80) {
                        $('#txtOptimaCreationBrfActivityStart60_pct').text(formatMoney(val.optimaCreationBrfActivityStart60_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                        $('#symOptimaCreationBrfActivityStart60_pct').removeClass('bg-00d23b').removeClass('bg-ff1616').addClass('bg-ffb822');
                    } else if (val.optimaCreationBrfActivityStart60_pct >= 80) {
                        $('#txtOptimaCreationBrfActivityStart60_pct').text(formatMoney(val.optimaCreationBrfActivityStart60_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                        $('#symOptimaCreationBrfActivityStart60_pct').removeClass('bg-ffb822').removeClass('bg-ff1616').addClass('bg-00d23b');
                    }

                    $('#chartAccuracy').remove();
                    DonutAccuracy(val.scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct.toFixed(2), val.scoreboardAccuracy_OptimaInput_vs_PromoPlan.toFixed(2), val.subscoreAccuracy_OptimaInput_vs_PromoPlan_bobot);

                    // Dashboard hidden SKP
                    $('#txtSKPPeriodMatch_Score').html(val.skpPeriodMatch_Score.toFixed(2) + '<span class="fw-normal">/ '+val.skpPeriodMatch_bobot+'</span></span>');
                    $('#txtSKPDescAndMechanismMatch_Score').html(val.skpDescAndMechanismMatch_Score.toFixed(2) + '<span class="fw-normal">/ '+val.skpDescAndMechanismMatch_bobot+'</span></span>');
                    $('#txtSKPAmountMatch_Score').html(val.skpAmountMatch_Score.toFixed(2) + '<span class="fw-normal">/ '+val.skpAmountMatch_bobot+'</span></span>');
                    $('#txtSKPCreationBrfActivityStart60_Score').html(val.skpDraftH60_Score.toFixed(2) + '<span class="fw-normal">/ '+val.skpDraftH60_bobot+'</span></span>');

                    if (val.skpPeriodMatch_pct <60) {
                        $('#txtSKPPeriodMatch_pct').text(formatMoney(val.skpPeriodMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                        $('#symSKPPeriodMatch_pct').removeClass('bg-00d23b').removeClass('bg-ffb822').addClass('bg-ff1616');
                    } else if (val.skpPeriodMatch_pct >=60 && val.skpPeriodMatch_pct < 80) {
                        $('#txtSKPPeriodMatch_pct').text(formatMoney(val.skpPeriodMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                        $('#symSKPPeriodMatch_pct').removeClass('bg-00d23b').removeClass('bg-ff1616').addClass('bg-ffb822');
                    } else if (val.skpPeriodMatch_pct >= 80) {
                        $('#txtSKPPeriodMatch_pct').text(formatMoney(val.skpPeriodMatch_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                        $('#symSKPPeriodMatch_pct').removeClass('bg-ffb822').removeClass('bg-ff1616').addClass('bg-00d23b');
                    }

                    if (val.skpDescAndMechanismMatch_pct <60) {
                        $('#txtSKPDescAndMechanismMatch_pct').text(formatMoney(val.skpDescAndMechanismMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                        $('#symSKPDescAndMechanismMatch_pct').removeClass('bg-00d23b').removeClass('bg-ffb822').addClass('bg-ff1616');
                    } else if (val.skpDescAndMechanismMatch_pct >=60 && val.skpDescAndMechanismMatch_pct < 80) {
                        $('#txtSKPDescAndMechanismMatch_pct').text(formatMoney(val.skpDescAndMechanismMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                        $('#symSKPDescAndMechanismMatch_pct').removeClass('bg-00d23b').removeClass('bg-ff1616').addClass('bg-ffb822');
                    } else if (val.skpDescAndMechanismMatch_pct >= 80) {
                        $('#txtSKPDescAndMechanismMatch_pct').text(formatMoney(val.skpDescAndMechanismMatch_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                        $('#symSKPDescAndMechanismMatch_pct').removeClass('bg-ffb822').removeClass('bg-ff1616').addClass('bg-00d23b');
                    }

                    if (val.skpAmountMatch_pct <60) {
                        $('#txtSKPAmountMatch_pct').text(formatMoney(val.skpAmountMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                        $('#symSKPAmountMatch_pct').removeClass('bg-00d23b').removeClass('bg-ffb822').addClass('bg-ff1616');
                    } else if (val.skpAmountMatch_pct >=60 && val.skpAmountMatch_pct < 80) {
                        $('#txtOptimaAmountMatch_pct').text(formatMoney(val.skpAmountMatch_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                        $('#symOptimaAmountMatch_pct').removeClass('bg-00d23b').removeClass('bg-ff1616').addClass('bg-ffb822');
                    } else if (val.skpAmountMatch_pct >= 80) {
                        $('#txtOptimaAmountMatch_pct').text(formatMoney(val.skpAmountMatch_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                        $('#symOptimaAmountMatch_pct').removeClass('bg-ffb822').removeClass('bg-ff1616').addClass('bg-00d23b');
                    }

                    if (val.skpDraftH60_pct <60) {
                        $('#txtSKPCreationBrfActivityStart60_pct').text(formatMoney(val.skpDraftH60_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ffb822').addClass('text-ff1616');
                        $('#symSKPCreationBrfActivityStart60_pct').removeClass('bg-00d23b').removeClass('bg-ffb822').addClass('bg-ff1616');
                    } else if (val.skpDraftH60_pct >=60 && val.skpDraftH60_pct < 80) {
                        $('#txtSKPCreationBrfActivityStart60_pct').text(formatMoney(val.skpDraftH60_pct, 2) + '%').removeClass('text-00d23b').removeClass('text-ff1616').addClass('text-ffb822');
                        $('#symSKPCreationBrfActivityStart60_pct').removeClass('bg-00d23b').removeClass('bg-ff1616').addClass('bg-ffb822');
                    } else if (val.skpDraftH60_pct >= 80) {
                        $('#txtSKPCreationBrfActivityStart60_pct').text(formatMoney(val.skpDraftH60_pct, 2) + '%').removeClass('text-ffb822').removeClass('text-ff1616').addClass('text-00d23b');
                        $('#symSKPCreationBrfActivityStart60_pct').removeClass('bg-ffb822').removeClass('bg-ff1616').addClass('bg-00d23b');
                    }

                    $('#chartSKP').remove();
                    DonutSKP(val.scoreboardAccuracy_SKP_vs_Optima_pct.toFixed(2), val.scoreboardAccuracy_SKP_vs_Optima.toFixed(2), val.subscoreAccuracy_SKP_vs_Optima_bobot);
                }

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

const getDataLeagues = (createFrom, createTo) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dashboard/summary/data/creator-leagues-summary",
            type: "GET",
            dataType: 'json',
            data: {
                create_from:createFrom,
                create_to:createTo,
            },
            async: true,
            success: async function (result) {
                let res = result.data;

                if (res.overallScores) {
                    let val = res.overallScores;

                    for (let i=0; i<val.length; i++) {
                        if (val[i].channelDesc === "LKA") {
                            $('#text_progress_promo_plan_lka').text(formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            $('#text_progress_promo_plan_lka_mobile').text(formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            $('#text_progress_accuracy_promo_lka').text(formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            $('#text_progress_accuracy_promo_lka_mobile').text(formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            $('#text_progress_accuracy_skp_lka').text(formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            $('#text_progress_accuracy_skp_lka_mobile').text(formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            $('#text_progress_recon_lka').text(formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');
                            $('#text_progress_recon_lka_mobile').text(formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');

                            $('#text_progress_total_lka').text(formatMoney((val[i].overall_Score_pct), 2) + '%');
                            $('#text_progress_total_lka_mobile').text(formatMoney((val[i].overall_Score_pct), 2) + '%');

                            let progressPromoPlanLKA = $('#progress_bar_promo_plan_lka');
                            let progressPromoPlanLKAMobile = $('#progress_bar_promo_plan_lka_mobile');
                            let progressAccuracyPromoLKA = $('#progress_bar_accuracy_promo_lka');
                            let progressAccuracyPromoLKAMobile = $('#progress_bar_accuracy_promo_lka_mobile');
                            let progressAccuracySkpLKA = $('#progress_bar_accuracy_skp_lka');
                            let progressAccuracySkpLKAMobile = $('#progress_bar_accuracy_skp_lka_mobile');
                            let progressReconLKA = $('#progress_bar_recon_lka');
                            let progressReconLKAMobile = $('#progress_bar_recon_lka_mobile');

                            progressPromoPlanLKA.css('width', formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            progressPromoPlanLKAMobile.css('width', formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');

                            progressAccuracyPromoLKA.css('width', formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            progressAccuracyPromoLKAMobile.css('width', formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');

                            progressAccuracySkpLKA.css('width', formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            progressAccuracySkpLKAMobile.css('width', formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');

                            progressReconLKA.css('width', formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');
                            progressReconLKAMobile.css('width', formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');

                            if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct === 0) {
                                progressPromoPlanLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressPromoPlanLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct < 60) {
                                progressPromoPlanLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressPromoPlanLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct >=60 && val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct <80) {
                                progressPromoPlanLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressPromoPlanLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct >= 80) {
                                progressPromoPlanLKA.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressPromoPlanLKAMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct === 0) {
                                progressAccuracyPromoLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressAccuracyPromoLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct < 60) {
                                progressAccuracyPromoLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressAccuracyPromoLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >=60 && val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct <80) {
                                progressAccuracyPromoLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressAccuracyPromoLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >= 80) {
                                progressAccuracyPromoLKA.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressAccuracyPromoLKAMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct === 0) {
                                progressAccuracySkpLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressAccuracySkpLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct < 60) {
                                progressAccuracySkpLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressAccuracySkpLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct >=60 && val[i].scoreboardAccuracy_SKP_vs_Optima_pct <80) {
                                progressAccuracySkpLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressAccuracySkpLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct >= 80) {
                                progressAccuracySkpLKA.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressAccuracySkpLKAMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardReconMonitoring_pct === 0) {
                                progressReconLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressReconLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardReconMonitoring_pct < 60) {
                                progressReconLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressReconLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardReconMonitoring_pct >=60 && val[i].scoreboardReconMonitoring_pct <80) {
                                progressReconLKA.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressReconLKAMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardReconMonitoring_pct >= 80) {
                                progressReconLKA.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressReconLKAMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }
                        }
                        if (val[i].channelDesc === "MMKA,HYSU,WHOLESALER KA") {
                            $('#text_progress_promo_plan_mm').text(formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            $('#text_progress_promo_plan_mm_mobile').text(formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            $('#text_progress_accuracy_promo_mm').text(formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            $('#text_progress_accuracy_promo_mm_mobile').text(formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            $('#text_progress_accuracy_skp_mm').text(formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            $('#text_progress_accuracy_skp_mm_mobile').text(formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            $('#text_progress_recon_mm').text(formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');
                            $('#text_progress_recon_mm_mobile').text(formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');

                            $('#text_progress_total_mm').text(formatMoney((val[i].overall_Score_pct), 2) + '%');
                            $('#text_progress_total_mm_mobile').text(formatMoney((val[i].overall_Score_pct), 2) + '%');

                            let progressPromoPlanMM = $('#progress_bar_promo_plan_mm');
                            let progressPromoPlanMMMobile = $('#progress_bar_promo_plan_mm_mobile');
                            let progressAccuracyPromoMM = $('#progress_bar_accuracy_promo_mm');
                            let progressAccuracyPromoMMMobile = $('#progress_bar_accuracy_promo_mm_mobile');
                            let progressAccuracySkpMM = $('#progress_bar_accuracy_skp_mm');
                            let progressAccuracySkpMMMobile = $('#progress_bar_accuracy_skp_mm_mobile');
                            let progressReconMM = $('#progress_bar_recon_mm');
                            let progressReconMMMobile = $('#progress_bar_recon_mm_mobile');

                            progressPromoPlanMM.css('width', formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            progressPromoPlanMMMobile.css('width', formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            progressAccuracyPromoMM.css('width', formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            progressAccuracyPromoMMMobile.css('width', formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            progressAccuracySkpMM.css('width', formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            progressAccuracySkpMMMobile.css('width', formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            progressReconMM.css('width', formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');
                            progressReconMMMobile.css('width', formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');

                            if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct === 0) {
                                progressPromoPlanMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressPromoPlanMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct < 60) {
                                progressPromoPlanMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressPromoPlanMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct >=60 && val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct <80) {
                                progressPromoPlanMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressPromoPlanMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct >= 80) {
                                progressPromoPlanMM.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressPromoPlanMMMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct === 0) {
                                progressAccuracyPromoMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressAccuracyPromoMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct < 60) {
                                progressAccuracyPromoMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressAccuracyPromoMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >=60 && val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct <80) {
                                progressAccuracyPromoMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressAccuracyPromoMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >= 80) {
                                progressAccuracyPromoMM.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressAccuracyPromoMMMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct === 0) {
                                progressAccuracySkpMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressAccuracySkpMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct < 60) {
                                progressAccuracySkpMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressAccuracySkpMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct >=60 && val[i].scoreboardAccuracy_SKP_vs_Optima_pct <80) {
                                progressAccuracySkpMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressAccuracySkpMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct >= 80) {
                                progressAccuracySkpMM.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressAccuracySkpMMMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardReconMonitoring_pct === 0) {
                                progressReconMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressReconMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardReconMonitoring_pct < 60) {
                                progressReconMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressReconMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardReconMonitoring_pct >=60 && val[i].scoreboardReconMonitoring_pct <80) {
                                progressReconMM.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressReconMMMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardReconMonitoring_pct >= 80) {
                                progressReconMM.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressReconMMMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }
                        }
                        if (val[i].channelDesc === "E- COMMERCE,MOM & BABY SHOP,PHARMA") {
                            $('#text_progress_promo_plan_gt').text(formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            $('#text_progress_promo_plan_gt_mobile').text(formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            $('#text_progress_accuracy_promo_gt').text(formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            $('#text_progress_accuracy_promo_gt_mobile').text(formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            $('#text_progress_accuracy_skp_gt').text(formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            $('#text_progress_accuracy_skp_gt_mobile').text(formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            $('#text_progress_recon_gt').text(formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');
                            $('#text_progress_recon_gt_mobile').text(formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');

                            $('#text_progress_total_gt').text(formatMoney((val[i].overall_Score_pct), 2) + '%');
                            $('#text_progress_total_gt_mobile').text(formatMoney((val[i].overall_Score_pct), 2) + '%');

                            let progressPromoPlanGT = $('#progress_bar_promo_plan_gt');
                            let progressPromoPlanGTMobile = $('#progress_bar_promo_plan_gt_mobile');
                            let progressAccuracyPromoGT = $('#progress_bar_accuracy_promo_gt');
                            let progressAccuracyPromoGTMobile = $('#progress_bar_accuracy_promo_gt_mobile');
                            let progressAccuracySkpGT = $('#progress_bar_accuracy_skp_gt');
                            let progressAccuracySkpGTMobile = $('#progress_bar_accuracy_skp_gt_mobile');
                            let progressReconGT = $('#progress_bar_recon_gt');
                            let progressReconGTMobile = $('#progress_bar_recon_gt_mobile');

                            progressPromoPlanGT.css('width', formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            progressPromoPlanGTMobile.css('width', formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            progressAccuracyPromoGT.css('width', formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            progressAccuracyPromoGTMobile.css('width', formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            progressAccuracySkpGT.css('width', formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            progressAccuracySkpGTMobile.css('width', formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            progressReconGT.css('width', formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');
                            progressReconGTMobile.css('width', formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');

                            if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct === 0) {
                                progressPromoPlanGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressPromoPlanGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct < 60) {
                                progressPromoPlanGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressPromoPlanGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct >=60 && val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct <80) {
                                progressPromoPlanGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressPromoPlanGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct >= 80) {
                                progressPromoPlanGT.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressPromoPlanGTMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct === 0) {
                                progressAccuracyPromoGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressAccuracyPromoGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct < 60) {
                                progressAccuracyPromoGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressAccuracyPromoGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >=60 && val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct <80) {
                                progressAccuracyPromoGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressAccuracyPromoGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >= 80) {
                                progressAccuracyPromoGT.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressAccuracyPromoGTMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct === 0) {
                                progressAccuracySkpGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressAccuracySkpGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct < 60) {
                                progressAccuracySkpGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressAccuracySkpGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct >=60 && val[i].scoreboardAccuracy_SKP_vs_Optima_pct <80) {
                                progressAccuracySkpGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressAccuracySkpGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct >= 80) {
                                progressAccuracySkpGT.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressAccuracySkpGTMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardReconMonitoring_pct === 0) {
                                progressReconGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressReconGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardReconMonitoring_pct < 60) {
                                progressReconGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressReconGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardReconMonitoring_pct >=60 && val[i].scoreboardReconMonitoring_pct <80) {
                                progressReconGT.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressReconGTMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardReconMonitoring_pct >= 80) {
                                progressReconGT.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressReconGTMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }
                        }
                        if (val[i].channelDesc === "MEDICAL") {
                            $('#text_progress_promo_plan_medical').text(formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            $('#text_progress_promo_plan_medical_mobile').text(formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            $('#text_progress_accuracy_promo_medical').text(formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            $('#text_progress_accuracy_promo_medical_mobile').text(formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            $('#text_progress_accuracy_skp_medical').text(formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            $('#text_progress_accuracy_skp_medical_mobile').text(formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            $('#text_progress_recon_medical').text(formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');
                            $('#text_progress_recon_medical_mobile').text(formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');

                            $('#text_progress_total_medical').text(formatMoney((val[i].overall_Score_pct), 2) + '%');
                            $('#text_progress_total_medical_mobile').text(formatMoney((val[i].overall_Score_pct), 2) + '%');

                            let progressPromoPlanMedical = $('#progress_bar_promo_plan_medical');
                            let progressPromoPlanMedicalMobile = $('#progress_bar_promo_plan_medical_mobile');
                            let progressAccuracyPromoMedical = $('#progress_bar_accuracy_promo_medical');
                            let progressAccuracyPromoMedicalMobile = $('#progress_bar_accuracy_promo_medical_mobile');
                            let progressAccuracySkpMedical = $('#progress_bar_accuracy_skp_medical');
                            let progressAccuracySkpMedicalMobile = $('#progress_bar_accuracy_skp_medical_mobile');
                            let progressReconMedical = $('#progress_bar_recon_medical');
                            let progressReconMedicalMobile = $('#progress_bar_recon_medical_mobile');

                            progressPromoPlanMedical.css('width', formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            progressPromoPlanMedicalMobile.css('width', formatMoney((val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct), 2) + '%');
                            progressAccuracyPromoMedical.css('width', formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            progressAccuracyPromoMedicalMobile.css('width', formatMoney((val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct), 2) + '%');
                            progressAccuracySkpMedical.css('width', formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            progressAccuracySkpMedicalMobile.css('width', formatMoney((val[i].scoreboardAccuracy_SKP_vs_Optima_pct), 2) + '%');
                            progressReconMedical.css('width', formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');
                            progressReconMedicalMobile.css('width', formatMoney((val[i].scoreboardReconMonitoring_pct), 2) + '%');

                            if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct === 0) {
                                progressPromoPlanMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressPromoPlanMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct < 60) {
                                progressPromoPlanMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressPromoPlanMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct >=60 && val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct <80) {
                                progressPromoPlanMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressPromoPlanMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].promoPlanSubmittedBfrQuarterStart90_Score_pct >= 80) {
                                progressPromoPlanMedical.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressPromoPlanMedicalMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct === 0) {
                                progressAccuracyPromoMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressAccuracyPromoMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct < 60) {
                                progressAccuracyPromoMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressAccuracyPromoMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >=60 && val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct <80) {
                                progressAccuracyPromoMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressAccuracyPromoMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardAccuracy_OptimaInput_vs_PromoPlan_pct >= 80) {
                                progressAccuracyPromoMedical.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressAccuracyPromoMedicalMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct === 0) {
                                progressAccuracySkpMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressAccuracySkpMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct < 60) {
                                progressAccuracySkpMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressAccuracySkpMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct >=60 && val[i].scoreboardAccuracy_SKP_vs_Optima_pct <80) {
                                progressAccuracySkpMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressAccuracySkpMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardAccuracy_SKP_vs_Optima_pct >= 80) {
                                progressAccuracySkpMedical.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressAccuracySkpMedicalMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }

                            if (val[i].scoreboardReconMonitoring_pct === 0) {
                                progressReconMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                                progressReconMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-secondary');
                            } else if (val[i].scoreboardReconMonitoring_pct < 60) {
                                progressReconMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                                progressReconMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-ffb822').removeClass('bg-secondary').addClass('bg-ff5a5a');
                            } else if (val[i].scoreboardReconMonitoring_pct >=60 && val[i].scoreboardReconMonitoring_pct <80) {
                                progressReconMedical.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                                progressReconMedicalMobile.removeClass('bg-secondary').removeClass('bg-58dd91').removeClass('bg-secondary').removeClass('bg-ff5a5a').addClass('bg-ffb822');
                            } else if (val[i].scoreboardReconMonitoring_pct >= 80) {
                                progressReconMedical.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                                progressReconMedicalMobile.removeClass('bg-secondary').removeClass('bg-secondary').removeClass('bg-ffb822').removeClass('bg-ff5a5a').addClass('bg-58dd91');
                            }
                        }
                    }
                }
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

const getDataCreatorStanding = (createFrom, createTo) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dashboard/summary/data/creator-leagues-standing",
            type: "GET",
            dataType: 'json',
            data: {
                create_from:createFrom,
                create_to:createTo,
            },
            async: true,
            success: async function (result) {
                let res = result.data;

                if (res.result1s.length > 0) {
                    let result_top_bottom = res.result1s;
                    let html_list_top3_creator_league = "";
                    let html_list_bottom3_creator_league = "";
                    for (let i=0; i<3; i++) {
                        html_list_top3_creator_league += '' +
                            '<div class="row mt-3">\n' +
                            '    <div class="col-lg-9 col-9 px-5">\n' +
                            '        <span class="fs-6">'+ result_top_bottom[i].top_Creator +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-3 col-3 px-5 text-end">\n' +
                            '        <span class="fs-6">'+ formatMoney(result_top_bottom[i].topTotalScore, 2) +'%</span>\n' +
                            '    </div>\n' +
                            '</div>';

                        html_list_bottom3_creator_league += '' +
                            '<div class="row mt-3">\n' +
                            '    <div class="col-lg-9 col-9 px-5">\n' +
                            '        <span class="fs-6">'+ result_top_bottom[i].bottom_Creator +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-3 col-3 px-5 text-end">\n' +
                            '        <span class="fs-6">'+ formatMoney(result_top_bottom[i].botTotalScore, 2) +'%</span>\n' +
                            '    </div>\n' +
                            '</div>';
                    }

                    $('#list_top3_creator_league').html(html_list_top3_creator_league);
                    $('#list_bottom3_creator_league').html(html_list_bottom3_creator_league);
                }

                if (res.result2s.length > 0) {
                    let result_lka = res.result2s;
                    let html_list_lka_league = '' +
                        '<div class="row mt-3">\n' +
                        '    <div class="col-lg-7 col-9 px-5">\n' +
                        '        <span class="fs-6 fw-bolder">Name</span>\n' +
                        '    </div>\n' +
                        '    <div class="col-lg-5 col-3 px-5 text-end">\n' +
                        '        <span class="fs-6 fw-bolder">Score</span>\n' +
                        '    </div>\n' +
                        '</div>';

                    for (let i=0; i<result_lka.length; i++) {
                        html_list_lka_league += '' +
                            '<div class="row mt-3">\n' +
                            '    <div class="col-lg-7 col-9 px-5">\n' +
                            '        <span class="fs-6">'+ result_lka[i].top_creator +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-5 col-3 px-5 text-end">\n' +
                            '        <span class="fs-6">'+ formatMoney(result_lka[i].total_score, 2) +'%</span>\n' +
                            '    </div>\n' +
                            '</div>'
                    }

                    $('#list_header_lka').html(html_list_lka_league);
                }

                if (res.result3s.length > 0) {
                    let result_mm = res.result3s;
                    let html_list_mm_league = '' +
                        '<div class="row mt-3">\n' +
                        '    <div class="col-lg-7 col-9 px-5">\n' +
                        '        <span class="fs-6 fw-bolder">Name</span>\n' +
                        '    </div>\n' +
                        '    <div class="col-lg-5 col-3 px-5 text-end">\n' +
                        '        <span class="fs-6 fw-bolder">Score</span>\n' +
                        '    </div>\n' +
                        '</div>';

                    for (let i=0; i<result_mm.length; i++) {
                        html_list_mm_league += '' +
                            '<div class="row mt-3">\n' +
                            '    <div class="col-lg-7 col-9 px-5">\n' +
                            '        <span class="fs-6">'+ result_mm[i].top_creator +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-5 col-3 px-5 text-end">\n' +
                            '        <span class="fs-6">'+ formatMoney(result_mm[i].total_score, 2) +'%</span>\n' +
                            '    </div>\n' +
                            '</div>'
                    }

                    $('#list_header_mm').html(html_list_mm_league);
                }

                if (res.result4s.length > 0) {
                    let result_gt = res.result4s;
                    let html_list_gt_league = '' +
                        '<div class="row mt-3">\n' +
                        '    <div class="col-lg-7 col-9 px-5">\n' +
                        '        <span class="fs-6 fw-bolder">Name</span>\n' +
                        '    </div>\n' +
                        '    <div class="col-lg-5 col-3 px-5 text-end">\n' +
                        '        <span class="fs-6 fw-bolder">Score</span>\n' +
                        '    </div>\n' +
                        '</div>';

                    for (let i=0; i<result_gt.length; i++) {
                        html_list_gt_league += '' +
                            '<div class="row mt-3">\n' +
                            '    <div class="col-lg-7 col-9 px-5">\n' +
                            '        <span class="fs-6">'+ result_gt[i].top_creator +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-5 col-3 px-5 text-end">\n' +
                            '        <span class="fs-6">'+ formatMoney(result_gt[i].total_score, 2) +'%</span>\n' +
                            '    </div>\n' +
                            '</div>'
                    }

                    $('#list_header_gt').html(html_list_gt_league);
                }

                if (res.result5s.length > 0) {
                    let result_medical = res.result5s;
                    let html_list_medical_league = '' +
                        '<div class="row mt-3">\n' +
                        '    <div class="col-lg-7 col-9 px-5">\n' +
                        '        <span class="fs-6 fw-bolder">Name</span>\n' +
                        '    </div>\n' +
                        '    <div class="col-lg-5 col-3 px-5 text-end">\n' +
                        '        <span class="fs-6 fw-bolder">Score</span>\n' +
                        '    </div>\n' +
                        '</div>';

                    for (let i=0; i<result_medical.length; i++) {
                        html_list_medical_league += '' +
                            '<div class="row mt-3">\n' +
                            '    <div class="col-lg-7 col-9 px-5">\n' +
                            '        <span class="fs-6">'+ result_medical[i].top_creator +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-5 col-3 px-5 text-end">\n' +
                            '        <span class="fs-6">'+ formatMoney(result_medical[i].total_score, 2) +'%</span>\n' +
                            '    </div>\n' +
                            '</div>'
                    }

                    $('#list_header_medical').html(html_list_medical_league);
                }
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

const getDataApproverStanding = (createFrom, createTo) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dashboard/summary/data/approver-leagues-standing",
            type: "GET",
            dataType: 'json',
            data: {
                create_from:createFrom,
                create_to:createTo,
            },
            async: true,
            success: async function (result) {
                let res = result.data;

                if (res.kpiApproverResults1.length > 0) {
                    let val = res.kpiApproverResults1[0];
                    $('#txt_approval_request').text(val.approval_request);
                    $('#txt_approval_request_response_time').html(formatMoney(val.avg_approval_response_time, 2) + '<span class="fs-4 text-gray-500 fw-normal ms-2">Days</span>');

                    $('#txt_promo_approval_sla').html(formatMoney((val.promo_approval_pct), 2)+'<span class="text-gray-600 fw-normal ms-2">%</span>');
                    $('#txt_progress_promo_approval_sla').text(formatMoney((val.promo_approval_pct), 2) + '%');
                    $('#progress_promo_approval_sla').css('width', val.promo_approval_pct + '%');
                    $('#txt_promo_approval_sla_sub_score').html(formatMoney(val.promo_approval_sla_max_3days_subscore, 2) + '/' + val.promo_approval_sla_max_3days_bobot);
                }

                if (res.kpiApproverResults2.length > 0) {
                    let val = res.kpiApproverResults2;
                    let html_list_top5_approver_league = "";
                    let html_list_bottom5_approver_league = "";
                    for (let i=0; i<5; i++) {
                        html_list_top5_approver_league += '' +
                            '<div class="row px-3 pt-3">\n' +
                            '    <div class="col-lg-6 col-6">\n' +
                            '        <span class="fw-normal">'+ val[i].userapprover_top +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-2 col-6 text-end">\n' +
                            '        <span class="fw-normal">'+ formatMoney(val[i].score_top, 2) +'%</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-2 col-6 text-end">\n' +
                            '        <span class="fw-normal">'+ formatMoney(val[i].num_of_optima_top, 1) +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-2 col-6 text-end">\n' +
                            '        <span class="fw-normal">'+ formatMoney(val[i].avg_days_top, 1) +'</span>\n' +
                            '    </div>\n' +
                            '</div>';

                        html_list_bottom5_approver_league += '' +
                            '<div class="row px-3 pt-3">\n' +
                            '    <div class="col-lg-6 col-6">\n' +
                            '        <span class="fw-normal">'+ val[i].userapprover_bottom +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-2 col-6 text-end">\n' +
                            '        <span class="fw-normal">'+ formatMoney(val[i].score_bottom, 2) +'%</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-2 col-6 text-end">\n' +
                            '        <span class="fw-normal">'+ formatMoney(val[i].num_of_optima_bottom, 1) +'</span>\n' +
                            '    </div>\n' +
                            '    <div class="col-lg-2 col-6 text-end">\n' +
                            '        <span class="fw-normal">'+ formatMoney(val[i].avg_days_bottom, 1) +'</span>\n' +
                            '    </div>\n' +
                            '</div>';
                    }

                    $('#top5_approver').html(html_list_top5_approver_league);
                    $('#bottom5_approver').html(html_list_bottom5_approver_league);
                }
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

const DonutAccuracy = (value, value1, value2) =>{
    $('#containerChartAccuracy').html('<div id="chartAccuracy" class="mx-auto h-200px w-200px"></div>');

    // Create chart instance
    let charta = am4core.create("chartAccuracy", am4charts.PieChart);


    let sisa1 = 100 - value;
    // Add data
    charta.data = [
        { "sector": "Empty", "size": sisa1 },
        { "sector": "Overall Score", "size": value},
    ];

    // Add label
    charta.innerRadius = 60;

    let labela = charta.seriesContainer.createChild(am4core.Label);
    labela.horizontalCenter = "middle";
    labela.verticalCenter = "middle";
    labela.fontSize = 20;
    labela.html = "<center><p><b>"+value+"%</b><br>Sub Score <br>"+ value1 +"/"+ value2 +"</p></center>";


    // Add and configure Series
    let pieSeriesa = charta.series.push(new am4charts.PieSeries());
    pieSeriesa.dataFields.value = "size";
    pieSeriesa.dataFields.category = "sector";
    pieSeriesa.labels.template.disabled = true;
    pieSeriesa.ticks.template.disabled = true;


    let colorSeta = new am4core.ColorSet();
    colorSeta.list = ["#ff1616", "#ffb822"].map(function(color) {
        return new am4core.color(color);
    });
    pieSeriesa.colors = colorSeta;
}

const DonutSKP = (value, value1, value2) =>{
    $('#containerChartSKP').html('<div id="chartSKP" class="mx-auto h-200px w-200px"></div>');

    // Create chart instance
    let chart = am4core.create("chartSKP", am4charts.PieChart);


    let sisa = 100 - value;
    // Add data
    chart.data = [
        { "sector": "Empty", "size": sisa },
        { "sector": "Overall Score", "size": value},
    ];

    chart.innerRadius = 60;

    // Add label
    let label = chart.seriesContainer.createChild(am4core.Label);
    label.horizontalCenter = "middle";
    label.verticalCenter = "middle";
    label.fontSize = 20;
    label.html = "<center><p><b>"+value+"%</b><br>Sub Score <br>"+ value1 +"/"+ value2 +"</p></center>";


    // Add and configure Series
    let pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "size";
    pieSeries.dataFields.category = "sector";
    pieSeries.labels.template.disabled = true;
    pieSeries.ticks.template.disabled = true;


    let colorSet = new am4core.ColorSet();
    colorSet.list = ["#ff1616", "#ffb822"].map(function(color) {
        return new am4core.color(color);
    });
    pieSeries.colors = colorSet;
}

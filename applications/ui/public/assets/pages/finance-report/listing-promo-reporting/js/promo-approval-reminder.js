'use strict';

var dt_promo_approval_reminder, dt_profile, dt_profile_config, dt_send_email, dt_send_email_config;
var swalTitle = "Promo Approval Reminder";
var heightContainer = 315;
var dataFilter, url_datatable;
var validatorFilterMonth;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#filter_activity_start, #filter_activity_end').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    let dialerObject = new KTDialer(dialerElement, {
        min: 2015,
        step: 1,
    });

    $('#filter_period').val(new Date().getFullYear());
    dialerObject.setValue(new Date().getFullYear());
    getGapNotif($('#filter_year').val(), $('#filter_month_start').val(), $('#filter_month_end').val());

    const v_fm_end = function () {
        return {
            validate: function () {
                let data = $('#filter_month_start').find(':selected')[0].innerHTML;
                let strMonthStart = data;
                if (parseInt($('#filter_month_start').val()) > parseInt($('#filter_month_end').val())) {
                    return {
                        valid: false,
                        message: 'Please enter month greater then equal ' + strMonthStart
                    }
                } else {
                    return {
                        valid: true
                    }
                }
            }
        }
    }

    FormValidation.validators.v_fm_end = v_fm_end;
    validatorFilterMonth = FormValidation.formValidation(document.getElementById('form_promo_approval'), {
        fields: {
            filter_month_end: {
                validators: {
                    v_fm_end: {

                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

    dt_promo_approval_reminder = $('#dt_promo_approval_reminder').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        // order: [[1, 'asc']],
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        ordering: false,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'channelhead',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if(full.channel.includes("Total")) {
                        return '';
                    }else{
                        if(data.includes("Total")) {
                            return "<b>"+ data + "</b>";
                        }else{
                            return data;
                        }
                    }
                }
            },
            {
                targets: 1,
                data: 'channel',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if(data.includes("Total")) {
                        return "<b>"+ data + "</b>";
                    }else{
                        return data
                    }
                }
            },
            {
                targets: 2,
                data: 'status2',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                data: 'kamfcmcem',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                data: 'sb_group',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                data: 'w11',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
            {
                targets: 6,
                data: 'inv11',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
            {
                targets: 7,
                data: 'w12',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
            {
                targets: 8,
                data: 'inv12',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
            {
                targets: 9,
                data: 'w21',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
            {
                targets: 10,
                data: 'inv21',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
            {
                targets: 11,
                data: 'w22',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
            {
                targets: 12,
                data: 'inv22',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
            {
                targets: 13,
                data: 'wtot',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
            {
                targets: 14,
                data: 'invTot',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if(full.channelhead.includes("Total") || full.channel.includes("Total")) {
                        return "<b>" +  formatMoney(data,0) + "</b>";
                    }else{
                        return  formatMoney(data,0);
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });


    let month1 = $('#filter_month_start').select2('data');
    let strMonth1 = ( (month1[0].id !== "") ? month1[0].text : '' );
    let month2 = $('#filter_month_end').select2('data');
    let strMonth2 = ( (month2[0].id !== "") ? month2[0].text : '' );
    if (strMonth1===strMonth2) {
        dt_promo_approval_reminder.columns( [9,10,11,12] ).visible( false );
    }else{
        dt_promo_approval_reminder.columns( [9,10,11,12] ).visible( true );
    }
    $("#month1").text(strMonth1);
    $("#month2").text(strMonth2);

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
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
    };
});

$('#filter_month_start').on('change', function () {
    $('#filter_month_end').val($(this).val()).trigger('change');
});

$('#filter_month_end').on('change', async function () {
    if (parseInt($('#filter_month_start').val()) > parseInt($(this).val())) {
        validatorFilterMonth.revalidateField('filter_month_end').then(function () {
            $('#filter_month_end').val($('#filter_month_start').val()).trigger('change.select2');
        });
    } else {
        validatorFilterMonth.revalidateField('filter_month_end');
    }
});

$('#btn_send_email').on('click', function() {
    dt_profile.clear().draw();
    $('#filter_groupuser').val('').trigger('change');
    $('#dt_profile_search').val('');
    $('#dt_send_email_search').val('');

    $('#filter_groupuser_grouping').empty();
    getDataRegular();
    dt_send_email.clear().draw();
    dt_send_email.rows.add(defaultRegular).draw();
    $('#modal-send-email-report').modal('show');
});

$('#btn_config').on('click', function() {
    dt_profile_config.clear().draw();
    $('#filter_groupuser_config').val('').trigger('change');
    $('#dt_profile_config_search').val('');
    $('#dt_send_email_config_search').val('');

    $('#filter_groupuser_grouping_config').empty();
    getDataConfiguration();
    dt_send_email_config.clear().draw();
    dt_send_email_config.rows.add(defaultConfiguration).draw();
    $('#modal-send-email-auto-config').modal('show');
});

$('#dt_promo_approval_reminder_view').on('click', function (){
    validatorFilterMonth.validate().then(function (status) {
        if (status === "Valid") {
            let btn = document.getElementById('dt_promo_approval_reminder_view');
            let month1 = $('#filter_month_start').select2('data');
            let strMonth1 = ( (month1[0].id !== "") ? month1[0].text : '' );
            let month2 = $('#filter_month_end').select2('data');
            let strMonth2 = ( (month2[0].id !== "") ? month2[0].text : '' );
            if (strMonth1===strMonth2) {
                dt_promo_approval_reminder.columns( [9,10,11,12] ).visible( false );
            }else{
                dt_promo_approval_reminder.columns( [9,10,11,12] ).visible( true );
            }
            $("#month1").text(strMonth1);
            $("#month2").text(strMonth2);
            let filter_year = ($('#filter_year').val()) ?? "";
            let filter_month_start = ($('#filter_month_start').val()) ?? 0;
            let filter_month_end = ($('#filter_month_end').val()) ?? 0;
            let url = "/fin-rpt/listing-promo-reporting/list/promo-approval-reminder?year=" + filter_year + "&month_start=" + filter_month_start + "&month_end=" + filter_month_end;
            btn.setAttribute("data-kt-indicator", "on");
            btn.disabled = !0;
            dt_promo_approval_reminder.ajax.url(url).load(function () {
                btn.setAttribute("data-kt-indicator", "off");
                btn.disabled = !1;
            }).on('xhr.dt', function ( e, settings, json, xhr ) {
                btn.setAttribute("data-kt-indicator", "off");
                btn.disabled = !1;
            });
        }
    });
});

const getGapNotif = (filter_year, filter_month_start, filter_month_end) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/listing-promo-reporting/list/promo-approval-reminder",
            type        : "GET",
            dataType    : 'json',
            data        : {year: filter_year, month_start: filter_month_start, month_end: filter_month_end},
            async       : true,
            success: function(result) {
                if(result.gap){
                    $("#gapnotif_promo_count").html(formatMoney(result.gap.promoCount,0));
                    $("#gapnotif_promo_investment").html(formatMoney(result.gap.promoInvestment,0));
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

$('#btn_export_excel_promo_approval_reminder').on('click', function() {
    let filter_year = ($('#filter_year').val()) ?? "";
    let filter_month_start = ($('#filter_month_start').val()) ?? 0;
    let filter_month_end = ($('#filter_month_end').val()) ?? 0;
    let strMonthStart = $('#filter_month_start').find(':selected')[0].innerHTML;
    let strMonthEnd = $('#filter_month_end').find(':selected')[0].innerHTML;
    let url = "/fin-rpt/listing-promo-reporting/export-xls/promo-approval-reminder?year=" + filter_year + "&month_start=" + filter_month_start + "&month_end=" + filter_month_end + "&strMonthStart=" + strMonthStart + "&strMonthEnd=" + strMonthEnd;

    window.location.href = url;
});

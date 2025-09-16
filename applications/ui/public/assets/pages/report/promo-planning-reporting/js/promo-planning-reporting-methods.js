'use strict';

let dt_promo_planning_reporting;
let swalTitle = "Promo Planning Reporting";
heightContainer = 315;
let dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    let filter_activity_start = $('#filter_activity_start');
    let filter_activity_end = $('#filter_activity_end');

    if (localStorage.getItem('promoPlanningReportingState')) {
        dataFilter = JSON.parse(localStorage.getItem('promoPlanningReportingState'));

        url_datatable = '/rpt/promo-planning-reporting/list/paginate/filter?period=' + dataFilter.period +
            "&startFrom=" + dataFilter.activityStart + "&startTo=" + dataFilter.activityEnd +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId + "&channelId=" + dataFilter.channelId;
    } else {
        filter_activity_start.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(new Date().getFullYear() + '-01-01')
        }).setDate(new Date(new Date().getFullYear()+ "-01-01"));
        filter_activity_start.next().css('background-color', '#fff !important');
        filter_activity_end.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(new Date().getFullYear() + '-12-31')
        });
        filter_activity_end.next().css('background-color', '#fff !important');
        url_datatable = '/rpt/promo-planning-reporting/list/paginate/filter?period=' + $('#filter_period').val() +
            "&startFrom=" + filter_activity_start.val()  + "&startTo=" + filter_activity_end.val() +
            "&entityId=" + $('#filter_entity').val() + "&distributorId=" + $('#filter_distributor').val() + "&channelId=" + $('#filter_channel').val();
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListEntity(), getListChannel()]).then(async function () {
        if (dataFilter) {
            $('#filter_period').val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));
            filter_activity_start.flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(dataFilter.activityStart)
            }).setDate(new Date(dataFilter.activityStart));
            filter_activity_start.next().css('background-color', '#fff !important');

            filter_activity_end.flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(dataFilter.activityEnd)
            }).setDate(new Date(dataFilter.activityEnd));
            filter_activity_end.next().css('background-color', '#fff !important');

            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
            $('#filter_channel').val(dataFilter.channelId).trigger('change');
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_promo_planning_reporting = $('#dt_promo_planning_reporting').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[0, 'asc']],
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'promoPlanRefId',
                width: 170,
                title: 'Planning ID',
                className: 'align-middle',
            },
            {
                targets: 1,
                title: 'Initiator',
                data: 'initiator',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'TSCode',
                data: 'tsCode',
                width: 120,
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Promo ID',
                data: 'promoNumber',
                width: 170,
                className: 'align-middle',
            },
            {
                targets: 4,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 250,
                className: 'align-middle',
            },
            {
                targets: 5,
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Start Promo',
                data: 'startPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDateOptima(data);
                }
            },
            {
                targets: 7,
                title: 'End Promo',
                data: 'endPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDateOptima(data);
                }
            },
            {
                targets: 8,
                title: 'Investment',
                data: 'investment',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 9,
                title: 'Cost Ratio',
                data: 'costRatio',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 2,".", ",") + " %";
                }
            },
            {
                targets: 10,
                title: 'ROI',
                data: 'roi',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney((data * 100), 2,".", ",") + " %";
                }
            },
            {
                targets: 11,
                title: 'Last Status',
                data: 'lastStatus',
                width: 150,
                className: 'align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            showLoaderOnConfirm: false,
            allowOutsideClick: false,

        });
    };

});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#filter_activity_start').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
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

$('#filter_activity_end').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
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

$('#filter_period').on('change', function() {
    let period = this.value;
    var startDate = formatDate(new Date(period,0,1));
    var endDate = formatDate(new Date(period, 11, 31));
    let filter_activity_start = $('#filter_activity_start');
    let filter_activity_end = $('#filter_activity_end');
    filter_activity_start.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        defaultDate: startDate
    });
    filter_activity_start.next().css('background-color', '#fff !important');
    filter_activity_end.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        defaultDate: endDate
    });
    filter_activity_end.next().css('background-color', '#fff !important');
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_promo_planning_reporting_search').on('keyup', function () {
    dt_promo_planning_reporting.search(this.value).draw();
});

$('#dt_promo_planning_reporting_view').on('click', function (){
    let btn = document.getElementById('dt_promo_planning_reporting_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_channel = ($('#filter_channel').val()) ?? "";
    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let url = "/rpt/promo-planning-reporting/list/paginate/filter?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_planning_reporting.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            activityStart: $('#filter_activity_start').val(),
            activityEnd: $('#filter_activity_end').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? ""),
            channelId: ($('#filter_channel').val() ?? "")
        };

        localStorage.setItem('promoPlanningReportingState', JSON.stringify(data_filter));
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let text_entity = "";
    let data_entity = $('#filter_entity').select2('data');
    (data_entity[0].id !== "") ? text_entity = data_entity[0].longDesc : text_entity ='ALL';
    let text_distributor = "";
    let data_distributor = $('#filter_distributor').select2('data');
    if (data_distributor.length > 0) {
        if (data_distributor[0].id !== "") {
            text_distributor = data_distributor[0].text
        } else {
            text_distributor ='ALL'
        }
    } else {
        text_distributor ='ALL'
    }
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_channel = ($('#filter_channel').val()) ?? "";
    let url = "/rpt/promo-planning-reporting/export-xls?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel + "&entity=" + text_entity + "&distributor=" + text_distributor;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/rpt/promo-planning-reporting/list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].shortDesc + " - " + result.data[j].longDesc,
                        longDesc: result.data[j].longDesc
                    });
                }
                $('#filter_entity').select2({
                    placeholder: "Select an Entity",
                    width: '100%',
                    data: data
                });
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

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/rpt/promo-planning-reporting/list/distributor/entity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {entityId: entityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_distributor').select2({
                    placeholder: "Select a Distributor",
                    width: '100%',
                    data: data
                });
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

const getListChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/rpt/promo-planning-reporting/list/channel",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_channel').select2({
                    placeholder: "Select a Channel",
                    width: '100%',
                    data: data
                });
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

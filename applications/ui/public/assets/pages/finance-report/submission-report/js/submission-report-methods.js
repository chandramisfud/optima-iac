'use strict';

let dt_submission, dt_profile, dt_send_email;
let swalTitle = "Promo Submission report";
heightContainer = 315;
let dataFilter, url_datatable;
let dialerObject;

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

    if (localStorage.getItem('FinanceSubmissionReportingState')) {
        dataFilter = JSON.parse(localStorage.getItem('FinanceSubmissionReportingState'));

        url_datatable = '/fin-rpt/submission/list/paginate/filter?period=' + dataFilter.period +
            "&startFrom=" + dataFilter.activityStart + "&startTo=" + dataFilter.activityEnd +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId +
            "&channelId=" + dataFilter.channelId;
    } else {
        url_datatable = '/fin-rpt/submission/list/paginate/filter?period=' + $('#filter_period').val() +
            "&startFrom=" + $('#filter_activity_start').val()  + "&startTo=" + $('#filter_activity_end').val() +
            "&entityId=" + $('#filter_entity').val() + "&distributorId=" + $('#filter_distributor').val() +
            "&channelId=" + $('#filter_channel').val();
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListEntity(), getListChannel(), getListUserGroup() ]).then(async function () {
        if (dataFilter) {
            $('#filter_period').val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));
            $('#filter_activity_start').val(dataFilter.activityStart);
            $('#filter_activity_end').val(dataFilter.activityEnd);
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            await $('#filter_channel').val(dataFilter.channelId).trigger('change.select2');
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_submission = $('#dt_submission').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        // order: [[1, 'asc']],
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        ordering: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                width: 20,
                className: 'dt-control',
                orderable: false,
                data: null,
                defaultContent: '',
            },
            {
                targets: 1,
                data: 'promoNumber',
                width: 170,
                title: 'Promo ID',
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'Initiator',
                data: 'initiator',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 150,
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
                title: 'Promo Start',
                data: 'startPromo',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 6,
                title: 'Promo End',
                data: 'endPromo',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 7,
                title: 'Investment',
                data: 'investment',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 8,
                title: 'Creation Date',
                data: 'createOn',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 9,
                title: 'Status',
                data: 'lastStatus',
                width: 300,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 10,
                title: 'Status Date',
                data: 'lastStatusDate',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 11,
                title: 'DN Claim',
                data: 'dnClaim',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
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
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,

        });
    };

    $('#dt_submission tbody').on('click', 'td.dt-control', function () {
        let tr = $(this).closest('tr');
        let row = dt_submission.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        } else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });
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
        el_end.val(el_start.val());
    }
});

$('#filter_activity_end').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_start.val(el_end.val());
    }
});

$('#filter_period').on('change', function() {
    let period = this.value;
    var startDate = formatDate(new Date(period,0,1));
    var endDate = formatDate(new Date(period, 11, 31));
    $('#filter_activity_start').val(startDate);
    $('#filter_activity_end').val(endDate);

});

$('#dt_submission_search').on('keyup', function () {
    dt_submission.search(this.value).draw();
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_submission_view').on('click', function (){
    let btn = document.getElementById('dt_submission');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_channel = ($('#filter_channel').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let url = "/fin-rpt/submission/list/paginate/filter?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_submission.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            activityStart: $('#filter_activity_start').val(),
            activityEnd: $('#filter_activity_end').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            channelId: ($('#filter_channel').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? "")
        };

        localStorage.setItem('FinanceSubmissionReportingState', JSON.stringify(data_filter));
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
    (data_distributor[0].id !== "") ? text_distributor = data_distributor[0].text : text_distributor ='ALL';
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_channel = ($('#filter_channel').val()) ?? "";
    let url = "/fin-rpt/submission/export-xls?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel + "&entity=" + text_entity + "&distributor=" + text_distributor;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_summary').on('click', function() {
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    if(filter_entity=="" || filter_entity==null) { filter_entity = "0"; }

    dt_summary_late_creation.clear().draw()
    dt_summary_late_creation.ajax.url("/fin-rpt/submission/get-data/late-promo?period=" + filter_period + '&entity=' + filter_entity + '&distributor=' + filter_distributor).load();

    $('#modal-summary-late-creation').modal('show');
});

$('#btn_send_email').on('click', function() {
    dt_profile.clear().draw();
    $('#filter_groupuser').val('').trigger('change');
    $('#dt_profile_search').val('');
    $('#dt_send_email_search').val('');
    $('#modal-send-email-report').modal('show');
});

$('#btn_exception').on('click', function() {
    $('#modal-exception').modal('show');
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/submission/list/entity",
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
            url         : "/fin-rpt/submission/list/distributor/entity-id",
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
            url         : "/fin-rpt/submission/list/channel",
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
                    placeholder: "Select an Channel",
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

const format = (d) => {
    return '<table class="table table-sm table-condensed" style="margin-left:20px;">' +
        '<thead><tr>' +
        '<td>Entity</td><td>Distributor</td><td>Channel</td><td>Account</td><td>Sub Account</td>     <td>Sub Category</td>      <td>Activity</td>          <td>Sub Activity</td>' +
        '<td>Initiator</td>' +
        '</tr></thead>' +
        '<tr>' +
        '<td>' + d.entity + '</td><td>' + d.distributor + '</td><td>' + d.channelDesc + '</td><td>' + d.accountDesc + '</td><td>' + d.subAccountDesc + '</td><td>' + d.subCategory + '</td><td>' + d.activity + '</td><td>' + d.subActivity + '</td>' +
        '<td>' + d.initiator + '</td>' +
        '</tr>' +
        '<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>' +
        '</table>' +

        '<table class="table table-sm table-condensed" style="margin-left:20px;">' +
        '<thead><tr><td>Region</td></tr></thead>' +
        '<tr>' +
        '<td>' + d.regionDesc + '</td>' +
        '</tr>' +
        '<tr><td></td></tr>' +
        '</table>' +

        '<table class="table table-sm table-condensed" style="margin-left:20px;">' +
        '<thead><tr><td>SKU</td></tr></thead>' +
        '<tr>' +
        '<td>' + d.skuDesc + '</td>' +
        '</tr>' +
        '<tr><td></td></tr>' +
        '</table>' +

        '<table class="table table-sm table-condensed" style="margin-left:20px;">' +
        '<thead><tr>' +
        '<td>Mechanism</td><td>Status</td><td>Status Date</td><td>Notes</td><td class="text-end">Target</td><td class="text-end">DN Claim</td><td class="text-end">DN Paid</td>' +
        '</tr></thead>' +
        '<tr>' +
        '<td>' + d.mechanisme1 + '</td><td>' + d.lastStatus + '</td><td>' + formatDate(d.lastStatusDate) + '</td><td>' + d.approvalNotes + '</td>' +
        '<td class="text-end">' + formatMoney(d.target, 0) + '</td><td class="text-end">' + formatMoney(d.dnClaim, 0) + '</td><td class="text-end">' + formatMoney(d.dnPaid,0) + '</td>' +
        '</tr>' +
        // '<tr><td></td><td></td><td></td></tr>' +
        '</table>';
}

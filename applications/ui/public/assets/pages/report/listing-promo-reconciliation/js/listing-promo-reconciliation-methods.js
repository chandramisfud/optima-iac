'use strict';

let dt_listing_promo_reconciliation;
let swalTitle = "Listing Promo Reconciliation Reporting";
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

    if (localStorage.getItem('listingPromoReconciliationReportingState')) {
        dataFilter = JSON.parse(localStorage.getItem('listingPromoReconciliationReportingState'));

        url_datatable = '/rpt/listing-promo-recon-reporting/list/paginate/filter?period=' + dataFilter.period +
            "&startFrom=" + dataFilter.activityStart + "&startTo=" + dataFilter.activityEnd +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId;
    } else {
        url_datatable = '/rpt/listing-promo-recon-reporting/list/paginate/filter?period=' + $('#filter_period').val() +
            "&startFrom=" + $('#filter_activity_start').val()  + "&startTo=" + $('#filter_activity_end').val() +
            "&entityId=" + $('#filter_entity').val() + "&distributorId=" + $('#filter_distributor').val();
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListEntity()]).then(async function () {
        if (dataFilter) {
            $('#filter_period').val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));
            $('#filter_activity_start').val(dataFilter.activityStart);
            $('#filter_activity_end').val(dataFilter.activityEnd);
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_listing_promo_reconciliation = $('#dt_listing_promo_reconciliation').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
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
                width: 20,
                className: 'dt-control align-middle',
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
                title: 'Pre-Investment',
                data: 'investment',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 6,
                title: 'Post-Investment',
                data: 'investment_after',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 7,
                title: 'Pre-Cost Ratio',
                data: 'costRatio',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 2,".", ",") + " %";
                }
            },
            {
                targets: 8,
                title: 'Post-Cost Ratio',
                data: 'costRatio_after',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 2,".", ",") + " %";
                }
            },
            {
                targets: 9,
                title: 'Pre-ROI',
                data: 'roi',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 2,".", ",") + " %";
                }
            },
            {
                targets: 10,
                title: 'Post-ROI',
                data: 'roi_after',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 2,".", ",") + " %";
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

    $('#dt_listing_promo_reconciliation tbody').on('click', 'td.dt-control', function () {
        let tr = $(this).closest('tr');
        let row = dt_listing_promo_reconciliation.row(tr);

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

$('#dt_listing_promo_reconciliation_search').on('keyup', function () {
    dt_listing_promo_reconciliation.search(this.value).draw();
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_listing_promo_reconciliation_view').on('click', function (){
    let btn = document.getElementById('dt_listing_promo_reconciliation_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let url = "/rpt/listing-promo-recon-reporting/list/paginate/filter?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_listing_promo_reconciliation.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            activityStart: $('#filter_activity_start').val(),
            activityEnd: $('#filter_activity_end').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? "")
        };

        localStorage.setItem('listingPromoReconciliationReportingState', JSON.stringify(data_filter));
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let text_entity = "";
    let data = $('#filter_entity').select2('data');
    (data[0].id !== "") ? text_entity = data[0].longDesc : text_entity ='ALL';
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/rpt/listing-promo-recon-reporting/export-xls?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&entity=" + text_entity;
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
            url         : "/rpt/listing-promo-recon-reporting/list/entity",
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
            url         : "/rpt/listing-promo-recon-reporting/list/distributor/entity-id",
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
        '<td>Mechanism</td><td>Status</td><td>Notes</td>' +
        '</tr></thead>' +
        '<tr>' +
        '<td>' + d.mechanisme1_after + '</td><td>' + d.lastStatus + '</td><td>' + d.approvalNotes + '</td>' +
        '</tr>' +
        '<tr><td></td><td></td><td></td></tr>' +
        '</table>' +

        '<table class="table table-sm table-condensed" style="margin-left:20px;">' +
        '<thead><tr>' +
        '<td class="text-end">Target</td><td class="text-end">DN Claim</td><td class="text-end">DN Paid</td>' +
        '</tr></thead>' +
        '<tr>' +
        '<td class="text-end">' + formatMoney(d.target, 0) + '</td><td class="text-end">' + formatMoney(d.dnClaim, 0) + '</td><td class="text-end">' + formatMoney(d.dnPaid,0) + '</td>' +
        '</tr>' +
        '</table>';
}

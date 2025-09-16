'use strict';

var dt_dn_dtl_reporting;
var swalTitle = "Debit Note Detail";
var heightContainer = 280;
var dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    if (localStorage.getItem('FinanceDNDetailReportingReportState')) {
        dataFilter = JSON.parse(localStorage.getItem('FinanceDNDetailReportingReportState'));

        url_datatable = '/fin-rpt/dn-detail-reporting/list/paginate/filter?period=' + dataFilter.period + "&categoryId=" + (dataFilter.categoryId ?? '0') +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId + "&subAccountId=" + dataFilter.subAccountId;
    } else {
        url_datatable = '/fin-rpt/dn-detail-reporting/list/paginate/filter?period=' + $('#filter_period').val()  + "&categoryId=" + ($('#filter_category').val() ?? '0') +
            "&entityId=" + $('#filter_entity').val() + "&distributorId=" + $('#filter_distributor').val() + "&subAccountId=" + $('#filter_subaccount').val();
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListCategory(), getListEntity(), getListSubAccount()]).then(async function () {
        if (dataFilter) {
            $('#filter_period').val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));
            await $('#filter_category').val(dataFilter.categoryId).trigger('change.select2');
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
            $('#filter_subaccount').val(dataFilter.subAccountId).trigger('change');
            await getGapNotif(dataFilter.period);
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
            await getGapNotif( $('#filter_period').val());
        }
    });

    dt_dn_dtl_reporting = $('#dt_dn_dtl_reporting').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        fixedColumns: {
            left: 3,
        },
        processing: true,
        serverSide: true,
        paging: true,
        ordering: true,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'refId',
                width: 300,
                title: 'DN Number',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'DN Description',
                data: 'activityDesc',
                width: 500,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'DPP',
                data: 'dpp',
                width: 200,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 3,
                title: 'Category',
                data: 'dnCategory',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 300,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Over Budget Status',
                data: 'overBudgetStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Initiator',
                data: 'initiator',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Last Status',
                data: 'lastStatus',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Batch Name',
                data: 'batchname',
                orderable: false,
                width: 120,
                className: 'text-nowrap align-middle',
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

});

$('#dt_dn_dtl_reporting_search').on('keyup', function () {
    dt_dn_dtl_reporting.search(this.value).draw();
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_dn_dtl_reporting_view').on('click', function (){
    let btn = document.getElementById('dt_dn_dtl_reporting_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_category = ($('#filter_category').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";

    $('#gapnotif_dnclaim').html('<span class="indicator-progress text-optima d-inline" id="gapnotif_dnclaim">\n' +
        '                     <span class="spinner-border spinner-border-sm align-middle ms-2"></span>\n' +
        '                     Loading...\n' +
        '                </span>');
    getGapNotif(filter_period).then(function () {});

    let url = "/fin-rpt/dn-detail-reporting/list/paginate/filter?period=" + filter_period + "&categoryId=" + filter_category + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&subAccountId=" + filter_subaccount;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_dn_dtl_reporting.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            categoryId: ($('#filter_category').val() ?? ""),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? ""),
            subAccountId: ($('#filter_subaccount').val() ?? ""),
        };

        localStorage.setItem('FinanceDNDetailReportingReportState', JSON.stringify(data_filter));
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
    let filter_category = ($('#filter_category').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";
    let url = "/fin-rpt/dn-detail-reporting/export-csv?period=" + filter_period + "&categoryId=" + filter_category + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&subAccountId=" + filter_subaccount + "&entity=" + text_entity;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('.gapnotif_dnclaim').on('click', function() {
    let periode = $('#filter_period').val();
    let url = "/fin-rpt/dn-detail-reporting/export-xls/gap?periode=" + periode;
    window.open(url, '_blank');
});

const getListCategory = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/dn-detail-reporting/list/category",
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
                $('#filter_category').select2({
                    placeholder: "Select a Category",
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

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/dn-detail-reporting/list/entity",
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
            url         : "/fin-rpt/dn-detail-reporting/list/distributor/entity-id",
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

const getListSubAccount = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/dn-detail-reporting/list/sub-account",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc,
                    });
                }
                $('#filter_subaccount').select2({
                    placeholder: "Select a Sub Account",
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

const getGapNotif = (periode) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/dn-detail-reporting/get-data/gap",
            type        : "GET",
            dataType    : 'json',
            data        : {periode: periode},
            async       : true,
            success: function(result) {
                let dataAccrual = result.data['accrual'][0];
                let dataPromo = result.data['promo'][0];
                let dataDebitNote = result.data['debitnote'][0];

                let gapDNClaim_accrual_promo = dataAccrual['dnClaim'] - dataPromo['dnClaim'];
                let gapDNClaim_accrual_dn = dataAccrual['dnClaim'] - dataDebitNote['dnClaim'];
                let gapDNClaim_promo_dn = dataPromo['dnClaim'] - dataDebitNote['dnClaim'];

                let totalGap = '0';
                if (gapDNClaim_accrual_promo !== 0){
                    totalGap = formatMoney(gapDNClaim_accrual_promo,0);
                } else if (gapDNClaim_accrual_dn !== 0) {
                    totalGap = formatMoney(gapDNClaim_accrual_dn,0);
                } else if (gapDNClaim_promo_dn !== 0){
                    totalGap = formatMoney(gapDNClaim_promo_dn,0);
                }
                $("#gapnotif_dnclaim").html(totalGap);
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                $("#gapnotif_dnclaim").html('0');
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

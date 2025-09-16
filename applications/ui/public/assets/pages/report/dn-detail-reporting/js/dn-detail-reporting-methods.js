'use strict';

let dt_dn_dtl_reporting;
let swalTitle = "DN Detail Reporting";
heightContainer = 280;
var dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    if (localStorage.getItem('DNDetailReportingReportState')) {
        dataFilter = JSON.parse(localStorage.getItem('DNDetailReportingReportState'));

        url_datatable = '/rpt/dn-detail-reporting/list/paginate/filter?period=' + dataFilter.period +
             "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId + "&subAccountId=" + dataFilter.subAccountId;
    } else {
        url_datatable = '/rpt/dn-detail-reporting/list/paginate/filter?period=' + $('#filter_period').val()  +
            "&entityId=" + $('#filter_entity').val() + "&distributorId=" + $('#filter_distributor').val() + "&subAccountId=" + $('#filter_subaccount').val();
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    let dialerObject = new KTDialer(dialerElement, {
        min: 2015,
        step: 1,
    });

    Promise.all([getListCategory(), getListEntity(), getListSubAccount()]).then(async function () {
        if (dataFilter) {
            $('#filter_period').val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
            $('#filter_subaccount').val(dataFilter.subAccountId).trigger('change');
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
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
                data: 'refId',
                width: 150,
                title: 'DN Number',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Activity',
                data: 'activityDesc',
                width: 300,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Account',
                data: 'account',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Sub Account',
                data: 'subAccount',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Selling Point',
                data: 'sellingpoint',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Profit Center',
                data: 'profitCenter',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Fee Description',
                data: 'feeDesc',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Fee (%)',
                data: 'feePct',
                width: 100,
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
                targets: 9,
                title: 'Fee Amount',
                data: 'feeAmount',
                width: 100,
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
                targets: 10,
                title: 'DPP',
                data: 'dpp',
                width: 100,
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
                targets: 11,
                title: 'PPN Amount',
                data: 'ppnAmt',
                width: 100,
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
                targets: 12,
                title: 'PPH (%)',
                data: 'pphPct',
                width: 100,
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
                targets: 13,
                title: 'PPH Amount',
                data: 'pphAmt',
                width: 100,
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
                targets: 14,
                title: 'Total Claim',
                data: 'totalClaim',
                width: 100,
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
                targets: 15,
                title: 'Total Paid',
                data: 'paymentDate',
                width: 100,
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
                targets: 16,
                title: 'Payment Date',
                data: 'totalPaid',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 17,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 18,
                title: 'Surat Pengantar Cabang',
                data: 'suratPengantarCabang',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 19,
                title: 'Surat Pengantar HO',
                data: 'suratPengantarHO',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 20,
                title: 'Invoice No',
                data: 'invoiceNo',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 21,
                title: 'Create By',
                data: 'createBy',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 22,
                title: 'Create On',
                data: 'createOn',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return  new Date(data).toLocaleString();
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 23,
                title: 'Received By Danone By',
                data: 'received_by_danone_by',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 24,
                title: 'Received By Danone On',
                data: 'receivedByDanoneOn',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return  new Date(data).toLocaleString();
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 25,
                title: 'Validate By Finance By',
                data: 'validate_by_finance_by',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 26,
                title: 'Validate By Finance By (User Name)',
                data: 'validate_by_finance_by_username',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 27,
                title: 'Validate By Finance On',
                data: 'validate_by_finance_on',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return  new Date(data).toLocaleString();
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 28,
                title: 'Validate By Sales By',
                data: 'validate_by_sales_by',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 29,
                title: 'Validate By Sales On',
                data: 'validate_by_sales_on',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return  new Date(data).toLocaleString();
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 30,
                title: 'Invoice Notif By',
                data: 'invoiceNotifBy',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 31,
                title: 'Invoice Notif On',
                data: 'invoiceNotifOn',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return  new Date(data).toLocaleString();
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 32,
                title: 'Invoice By',
                data: 'invoice_by',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 33,
                title: 'Invoice On',
                data: 'invoice_on',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return  new Date(data).toLocaleString();
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 34,
                title: 'Confirm Paid By',
                data: 'confirm_paid_by',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 35,
                title: 'Confirm Paid On',
                data: 'confirm_paid_on',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return  new Date(data).toLocaleString();
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 36,
                title: 'Aging',
                data: 'aging',
                width: 50,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 37,
                title: 'Last Update',
                data: 'lastUpdate',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return  new Date(data).toLocaleString();
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 38,
                title: 'Over Budget Status',
                data: 'overBudgetStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 39,
                title: 'Category',
                data: 'dnCategory',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 40,
                title: 'Internal Doc. No',
                data: 'intDocNo',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 41,
                title: 'Memorial Doc. No',
                data: 'memDocNo',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 42,
                title: 'Tax Level',
                data: 'taxLevel',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 43,
                title: 'Rejection Remarks by Finance',
                data: 'notes',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 44,
                title: 'Initiator',
                data: 'initiator',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 45,
                title: 'Sales Validation Status',
                data: 'salesValidationStatus',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 46,
                title: 'FP Number',
                data: 'fpNumber',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 47,
                title: 'FP Number',
                data: 'fpDate',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDateOptima(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 48,
                title: 'Remark by Sales',
                data: 'statusSalesNotes',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 49,
                title: 'VAT Expired',
                data: 'vatExpired',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data === 1) {
                        return "Y";
                    } else {
                        return "N";
                    }
                }
            },
            {
                targets: 50,
                title: 'Batch Name',
                data: 'batchname',
                width: 100,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    // $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
    //     console.log(e)
    //     let strmessage = e.jqXHR.responseJSON.message
    //     if(strmessage==="") strmessage = "Please contact your vendor"
    //     Swal.fire({
    //         text: strmessage,
    //         icon: "warning",
    //         buttonsStyling: !1,
    //         confirmButtonText: "OK",
    //         customClass: {confirmButton: "btn btn-optima"},
    //         allowOutsideClick: false,
    //
    //     });
    // };

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
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";
    let url = "/rpt/dn-detail-reporting/list/paginate/filter?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&subAccountId=" + filter_subaccount;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_dn_dtl_reporting.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? ""),
            subAccountId: ($('#filter_subaccount').val() ?? ""),
        };

        localStorage.setItem('DNDetailReportingReportState', JSON.stringify(data_filter));
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
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";
    let url = "/rpt/dn-detail-reporting/export-xls?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&subAccountId=" + filter_subaccount + "&entity=" + text_entity;
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
            url         : "/rpt/dn-detail-reporting/list/entity",
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
            url         : "/rpt/dn-detail-reporting/list/distributor/entity-id",
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
            url         : "/rpt/dn-detail-reporting/list/sub-account",
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

const getListCategory = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/rpt/listing-promo-reporting/list/category",
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

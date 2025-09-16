'use strict';

var dt_investment_report;
var swalTitle = "Investment Report";
heightContainer = 280;
var dataFilter, url_datatable, dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    if (localStorage.getItem('investmentReportState')) {
        dataFilter = JSON.parse(localStorage.getItem('investmentReportState'));

        url_datatable = '/rpt/investment/list/paginate/filter?period=' + dataFilter.period +
            "&budgetAllocationId=" + dataFilter.budgetAllocationId + "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId;
    } else {
        url_datatable = '/rpt/investment/list/paginate/filter?period=' + $('#filter_period').val() + "&budgetAllocationId=" + $('#filter_budget_allocation_id').val()  +
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
            $('#filter_budget_allocation_id').val(dataFilter.budgetAllocationId);
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_investment_report = $('#dt_investment_report').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[3, 'asc']],
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
                data: 'entity',
                width: 200,
                title: 'Entity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Distributor',
                data: 'distributor',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Category',
                data: 'categoryDesc',
                width: 100,
                orderable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Budget Name',
                data: 'budgetAllocationName',
                width: 300,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Sub Activity Type',
                data: 'activityType',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Is Last Layer',
                data: 'isLastLayer',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data === 1) {
                        return "true";
                    } else {
                        return "false";
                    }
                }
            },
            {
                targets: 6,
                title: 'Channel',
                data: 'channel',
                width: 250,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Budget Deployed',
                data: 'budgetDeployed',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 8,
                title: 'Promo Created',
                data: 'promoCreated',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 9,
                title: 'DN Claim',
                data: 'dnClaimed',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 10,
                title: 'DN Paid',
                data: 'dnPaid',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 11,
                title: 'Budget Deployed vs Promo Created',
                data: 'gapBudgetDeployedvsPromoCreated',
                width: 200,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 12,
                title: 'Promo Created vs DN Claim',
                data: 'gapPromoCreatedvsDNClaimed',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 13,
                title: 'DN Claim vs DN Paid',
                data: 'gapDNClaimedvsDNPaid',
                width: 150,
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
});

$('#dt_investment_report_search').on('keyup', function () {
    dt_investment_report.search(this.value).draw();
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#btn_search_budget_allocation').on('click', function () {
    $('#dt_list_budget_allocation_search').val('');
    let year = $('#filter_period').val();
    let entityId = $('#filter_entity').val();
    let distributorId = $('#filter_distributor').val();
    let budgetParentId = $('#filter_budget_allocation_id').val();
    dt_budget_allocation_list.ajax.url("/rpt/investment/list/budget-allocation?year=" + year + '&entityId=' + entityId +
        '&distributorId=' + distributorId + '&budgetParentId=' + budgetParentId + '&channelId=').load();
    $('#modal_list_budget_allocation').modal('show');
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_investment_report_view').on('click', function (){
    let btn = document.getElementById('dt_investment_report_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_budget_allocation_id = ($('#filter_budget_allocation_id').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/rpt/investment/list/paginate/filter?period=" + filter_period + "&budgetAllocationId=" + filter_budget_allocation_id
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_investment_report.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            budgetAllocationId: $('#filter_budget_allocation_id').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? "")
        };

        localStorage.setItem('investmentReportState', JSON.stringify(data_filter));
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
    let filter_budget_allocation_id = ($('#filter_budget_allocation_id').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/rpt/investment/export-xls?period=" + filter_period + "&budgetAllocationId=" + filter_budget_allocation_id
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
            url         : "/rpt/investment/list/entity",
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
            url         : "/rpt/investment/list/distributor/entity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {entityId: entityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].shortDesc + " - " + result.data[j].longDesc
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


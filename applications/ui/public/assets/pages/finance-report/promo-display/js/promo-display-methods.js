'use strict';

let dt_promo_display;
let swalTitle = "Promo Display";
heightContainer = 280;
let dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    if (localStorage.getItem('FinancePromoDisplayReportState')) {
        dataFilter = JSON.parse(localStorage.getItem('FinancePromoDisplayReportState'));

        url_datatable = '/fin-rpt/promo-display/list/paginate/filter?period=' + dataFilter.period +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId;
    } else {
        url_datatable = '/fin-rpt/promo-display/list/paginate/filter?period=' + $('#filter_period').val()  +
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
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_promo_display = $('#dt_promo_display').DataTable({
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
        ordering: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: '',
                targets: 0,
                data: 'promoId',
                width: 20,
                orderable: false,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full) {
                    let startYear = new Date(full['startPromo']).getFullYear();
                    return `<div class="me-0">
                                <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>
                                </a>
                                <div class="menu menu-sub menu-sub-dropdown w-175px w-md-175px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">
                                    <a class="dropdown-item text-start approve-record" href="/fin-rpt/promo-display/form?method=view&id=${full['promoId']}&c=${full['categoryShortDesc']}&sy=${startYear}&recon=${full['reconciled'] ? '1' : '0'}"><i class="fa fa-edit fs-6"></i> View Data</a>
                                </div>
                        </div>`;
                }
            },
            {
                targets: 1,
                title: 'Promo ID',
                data: 'refId',
                width: 170,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 300,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Start Promo',
                data: 'startPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data) {
                    return formatDate(data);
                }
            },
            {
                targets: 5,
                title: 'End Promo',
                data: 'endPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data) {
                    return formatDate(data);
                }
            },
            {
                targets: 6,
                title: 'Investment',
                data: 'investment',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 7,
                title: 'Creation Date',
                data: 'createOn',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data) {
                    return formatDate(data);
                }
            },
            {
                targets: 8,
                title: 'Status',
                data: 'lastStatus',
                width: 450,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: 'Status Date',
                data: 'lastStatusDate',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 10,
                title: 'DN Claim',
                data: 'dnclaim',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data, 0,".", ",");
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
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

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#dt_promo_display_search').on('keyup', function () {
    dt_promo_display.search(this.value).draw();
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    let elFilterDistributor = $('#filter_distributor');
    elFilterDistributor.empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    elFilterDistributor.val('').trigger('change');
});

$('#dt_promo_display_view').on('click', function (){
    let btn = document.getElementById('dt_promo_display_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/fin-rpt/promo-display/list/paginate/filter?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_display.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? "")
        };

        localStorage.setItem('FinancePromoDisplayReportState', JSON.stringify(data_filter));
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let text_entity;
    let elFilterEntity = $('#filter_entity');
    let elFilterPeriod = $('#filter_period');
    let elFilterDistributor = $('#filter_distributor');

    let data = elFilterEntity.select2('data');
    (data[0].id !== "") ? text_entity = data[0].longDesc : text_entity ='ALL';
    let filter_period = (elFilterPeriod.val()) ?? "";
    let filter_entity = (elFilterEntity.val()) ?? "";
    let filter_distributor = (elFilterDistributor.val()) ?? "";
    let url = "/fin-rpt/promo-display/export-xls?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&entity=" + text_entity;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/promo-display/list/entity",
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

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/promo-display/list/distributor/entity-id",
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

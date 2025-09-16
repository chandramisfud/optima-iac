'use strict';

let dt_dn_display;
let swalTitle = "DN Display";
heightContainer = 280;
let dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    if (localStorage.getItem('DNDisplayReportState')) {
        dataFilter = JSON.parse(localStorage.getItem('DNDisplayReportState'));

        url_datatable = '/rpt/dn-display/list/paginate/filter?period=' + dataFilter.period +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId;
    } else {
        url_datatable = '/rpt/dn-display/list/paginate/filter?period=' + $('#filter_period').val()  +
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

    dt_dn_display = $('#dt_dn_display').DataTable({
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
                data: 'id',
                width: 20,
                orderable: false,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return '<div class="me-0">' +
                                '<a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">' +
                                    '<i class="la la-cog fs-3 text-optima text-hover-optima"></i>' +
                                '</a>' +
                                '<div class="menu menu-sub menu-sub-dropdown w-180px w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">' +
                                    '<a class="dropdown-item text-start info-record" href="/rpt/dn-display/form?&id=' + data + '"><i class="fa fa-file-alt fs-6 me-2"></i> DN Display</a>' +
                                    '<a class="dropdown-item text-start print-record" href="/rpt/dn-display/print-pdf?&id=' + data + '" target="_blank"><i class="fa fa-print fs-6"></i> Print DN</a>' +
                                '</div>' +
                            '</div>';
                }
            },
            {
                targets: 1,
                title: 'DN Number',
                data: 'refId',
                width: 170,
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 170,
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Activity',
                data: 'activityDesc',
                width: 300,
                className: 'align-middle',
            },
            {
                targets: 4,
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
                targets: 5,
                title: 'VAT',
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
                targets: 6,
                title: 'Last Status',
                data: 'lastStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Sales Validation Status',
                data: 'salesValidationStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Remark by Sales',
                data: 'remarkSales',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: 'Rejection Remarks by Finance',
                data: 'notes',
                width: 180,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 10,
                title: 'Validated by Finance On',
                data: 'validate_by_finance_on',
                width: 150,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDateTimeMDYSlash(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 11,
                title: 'Validated by Sales On',
                data: 'validate_by_sales_on',
                width: 150,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDateTimeMDYSlash(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 12,
                title: 'Payment Date',
                data: 'confirm_paid_on',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDateTimeMDYSlash(data);
                    } else {
                        return '';
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
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

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#dt_dn_display_search').on('keyup', function () {
    dt_dn_display.search(this.value).draw();
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_dn_display_view').on('click', function (){
    let btn = document.getElementById('dt_dn_display_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/rpt/dn-display/list/paginate/filter?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_dn_display.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? "")
        };

        localStorage.setItem('DNDisplayReportState', JSON.stringify(data_filter));
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/rpt/dn-display/list/entity",
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
            url         : "/rpt/dn-display/list/distributor/entity-id",
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

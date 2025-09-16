'use strict';

let dt_accrual;
let swalTitle = "Accrual Report";
heightContainer = 280;
let dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#filter_closing_date').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    if (localStorage.getItem('accrualReportState')) {
        dataFilter = JSON.parse(localStorage.getItem('accrualReportState'));

        url_datatable = '/rpt/accrual/list/paginate/filter?period=' + dataFilter.period + "&entityId=" + dataFilter.entityId + "&closingDate=" + dataFilter.closingDate;
    } else {
        url_datatable = '/rpt/accrual/list/paginate/filter?period=' + $('#filter_period').val() + "&entityId=" + $('#filter_entity').val() + "&closingDate=" + $('#filter_closing_date').val();
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
            await $('#filter_entity').val(dataFilter.entityId).trigger('change');
            $('#filter_closing_date').val(dataFilter.closingDate);
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_accrual = $('#dt_accrual').DataTable({
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
                data: 'promoNumber',
                width: 170,
                title: 'Promo ID',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Account',
                data: 'accountDesc',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Mechanism 1',
                data: 'accountDesc',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 250,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Start Promo',
                data: 'startPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 5,
                title: 'End Promo',
                data: 'endPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDateOptima(data);
                }
            },
            {
                targets: 6,
                title: 'Investment',
                data: 'investment',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 7,
                title: 'Accrual YTD',
                data: 'accrueYTD',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 8,
                title: 'Last Update',
                data: 'lastUpdate',
                width: 350,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 9,
                title: 'Last Status',
                data: 'lastStatus',
                width: 350,
                className: 'text-nowrap align-middle'
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

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#dt_accrual_search').on('keyup', function () {
    dt_accrual.search(this.value).draw();
});

$('#dt_accrual_view').on('click', function (){
    let btn = document.getElementById('dt_accrual_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_closing_date = ($('#filter_closing_date').val()) ?? "";
    let url = "/rpt/accrual/list/paginate/filter?period=" + filter_period + "&entityId=" + filter_entity + "&closingDate=" + filter_closing_date;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_accrual.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            closingDate: ($('#filter_closing_date').val() ?? "")
        };

        localStorage.setItem('accrualReportState', JSON.stringify(data_filter));
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
    let filter_closing_date = ($('#filter_closing_date').val()) ?? "";
    let url = "/rpt/accrual/export-xls?period=" + filter_period + "&entityId=" + filter_entity + "&closingDate=" + filter_closing_date + "&entity=" + text_entity;
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
            url         : "/rpt/accrual/list/entity",
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

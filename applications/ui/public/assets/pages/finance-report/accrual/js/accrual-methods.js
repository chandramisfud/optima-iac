'use strict';

let dt_fin_accrual, dt_report_header;
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
        altInput: "true",
        allowInput: "true"
    });

    if (localStorage.getItem('financeAccrualReportState')) {
        dataFilter = JSON.parse(localStorage.getItem('financeAccrualReportState'));
        url_datatable = '/fin-rpt/accrual/list/paginate/filter?period=' + dataFilter.period + "&entityId=" + dataFilter.entityId + "&closingDate=" + dataFilter.closingDate;
    } else {
        url_datatable = '/fin-rpt/accrual/list/paginate/filter?period=' + $('#filter_period').val() + "&entityId=" + $('#filter_entity').val() + "&closingDate=" + $('#filter_closing_date').val();
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
            $('#filter_closing_date').flatpickr({
                altFormat: "Y-m-d",
                dateFormat: "Y-m-d",
                disableMobile: "true",
                altInput: "true",
                allowInput: "true"
            }).setDate(dataFilter.closingDate);
            await getGapNotif(dataFilter.period);
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
            await getGapNotif( $('#filter_period').val());
        }
    });

    dt_fin_accrual = $('#dt_fin_accrual').DataTable({
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
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'promoNumber',
                width: 300,
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
                width: 300,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Start Promo',
                data: 'startPromo',
                width: 200,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 5,
                title: 'End Promo',
                data: 'endPromo',
                width: 200,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDateOptima(data);
                }
            },
            {
                targets: 6,
                title: 'Investment',
                data: 'investment',
                width: 300,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 7,
                title: 'Accrual YTD',
                data: 'accrueYTD',
                width: 300,
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

$('#dt_fin_accrual_search').on('keyup', function () {
    dt_fin_accrual.search(this.value).draw();
});

$('#dt_fin_accrual_view').on('click', function (){
    let btn = document.getElementById('dt_fin_accrual_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_closing_date = ($('#filter_closing_date').val()) ?? "";

    $('#gapnotif_dnclaim').html('<span class="indicator-progress text-optima d-inline" id="gapnotif_dnclaim">\n' +
        '                     <span class="spinner-border spinner-border-sm align-middle ms-2"></span>\n' +
        '                     Loading...\n' +
        '                </span>');
    getGapNotif(filter_period).then(function () {});

    let url = "/fin-rpt/accrual/list/paginate/filter?period=" + filter_period + "&entityId=" + filter_entity + "&closingDate=" + filter_closing_date;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_fin_accrual.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            period: $('#filter_period').val(),
            entityId: ($('#filter_entity').val() ?? ""),
            closingDate: ($('#filter_closing_date').val() ?? "")
        };

        localStorage.setItem('financeAccrualReportState', JSON.stringify(data_filter));
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
    if (filter_closing_date == '') {
        Swal.fire({
            title: swalTitle,
            text: 'Enter Closing Date',
            icon: "warning",
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    } else {

        let url = "/fin-rpt/accrual/export-xls?period=" + filter_period + "&entityId=" + filter_entity + "&closingDate=" + filter_closing_date + "&entity=" + text_entity;

        let a = document.createElement("a");
        a.href = url;
        let evt = document.createEvent("MouseEvents");
        //the tenth parameter of initMouseEvent sets ctrl key
        evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
            true, false, false, false, 0, null);
        a.dispatchEvent(evt);
    }
});

$('#btn_excel_list').on('click', function() {
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_closing_date = ($('#filter_closing_date').val()) ?? "";
    if(filter_entity == "" || filter_entity == null) { filter_entity = "0"; }

    dt_report_header.clear().draw()
    dt_report_header.ajax.url("/fin-rpt/accrual/list/report-header?periode=" + filter_period + '&entity=' + filter_entity + '&closingdt=' + filter_closing_date).load();
    $('#modal-report-header-list').modal('show');
});

$('.gapnotif_dnclaim').on('click', function() {
    let periode = $('#filter_period').val();
    let url = "/fin-rpt/accrual/export-xls/gap?periode=" + periode;

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
            url         : "/fin-rpt/accrual/list/entity",
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

const getGapNotif = (periode) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/accrual/get-data/gap",
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
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

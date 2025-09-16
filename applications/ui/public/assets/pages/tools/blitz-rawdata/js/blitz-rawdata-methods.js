'use strict';

var dt_promo_creation_info, dt_baseline_calculation, dt_result_raw, dt_sales_calculation;
var promostart;
var swalTitle = "BLITZ Raw Data";

$(document).ready(async function () {

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

    dt_promo_creation_info = $('#dt_promo_creation_info').DataTable({
        dom:
            "<'row'<'col-sm-12'Rtr>>",
        processing: true,
        serverSide: false,
        paging: false,
        ordering: false,
        searching: true,
        scrollCollapse: true,
        scrollX: true,
        scrollY: 280,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Item',
                data: 'f1',
                width: 150,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    if (full.f1 == 'Promo ID') {
                        $('#promorefid').val(full.f3);
                        $('#lbl_promorefid').text("Promo ID");
                    }
                    if (full.f1 == 'Promo Planning ID') {
                        $('#promorefid').val(full.f3);
                        $('#lbl_promorefid').text("Promo Planning ID");
                    }
                    if (full.f1 == 'Period') {
                        $('#period').val(full.f2);
                    }
                    if (full.f1 == 'Planning Creation Date') {
                        $('#planningcreationdate').val(full.f2);
                    }
                    if (full.f1 == 'Start Promo') {
                        promostart = full.f2;
                    }
                    if (full.f1 == 'End Promo') {
                        $('#promoperiod').val(promostart + ' s/d ' + full.f2);
                    }

                    return data;
                }
            },
            {
                targets: 1,
                title: 'Value',
                data: 'f2',
                width: 150,
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'Description',
                data: 'f3',
                width: 150,
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Code Mapping',
                data: 'f4',
                width: 150,
                className: 'align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_baseline_calculation = $('#dt_baseline_calculation').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        processing: true,
        serverSide: false,
        paging: false,
        ordering: false,
        searching: true,
        scrollCollapse: true,
        scrollX: false,
        scrollY: 125,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: '',
                data: 'bs',
                className: 'align-middle',
            },
            {
                targets: 1,
                title: 'Old',
                data: 'bsold',
                className: 'align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0);
                }
            },
            {
                targets: 2,
                title: 'New',
                data: 'bsnew',
                className: 'align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0);
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_result_raw = $('#dt_result_raw').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        processing: true,
        serverSide: false,
        paging: false,
        ordering: false,
        searching: true,
        scrollCollapse: true,
        scrollX: false,
        scrollY: 62,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Baseline Sales',
                data: 'baseline_sales',
                className: 'align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0);
                }
            },
            {
                targets: 1,
                title: 'Actual Sales',
                data: 'actual_sales',
                className: 'align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0);
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_sales_calculation = $('#dt_sales_calculation').DataTable({
        dom:
            "<'row'<'col-sm-12'Rtr>>",
        processing: true,
        serverSide: false,
        paging: false,
        ordering: false,
        searching: true,
        scrollCollapse: true,
        scrollX: false,
        scrollY: 175,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Year Month',
                data: 'ym',
                width: 150,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '-'
                    } else {
                        return data;
                    }
                }
            },
            {
                targets: 1,
                title: 'Sales',
                data: 'sales',
                width: 150,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    return data.toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
                }
            },
            {
                targets: 2,
                title: '% Anomaly',
                data: 'anomaly_pct',
                width: 150,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    return (data * 100).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 }) + '%';
                }
            },
            {
                targets: 3,
                title: 'Old Baseline',
                data: 'bsold',
                width: 150,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    return data.toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
                }
            },
            {
                targets: 4,
                title: '% Abnormal',
                data: 'abnormal_pct',
                width: 150,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    return (data * 100).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 }) + '%';
                }
            },
            {
                targets: 5,
                title: 'Abnormal Sales',
                data: 'sales_abnormal',
                width: 150,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    return data.toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
                }
            },
            {
                targets: 6,
                title: 'New Sales',
                data: 'sales_new',
                width: 150,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    return data.toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
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

    await rawData(null,'');
});

$('#refid').on('keyup', async function (e) {
    if (e.key === 'Enter' || e.keyCode === 13) {
        if($('#refid').val() === '' || $('#refid').val() == null) {
            await rawData(null,'');
        } else {
            let isPromo = $('#filter_promo').val();
            let refId = document.getElementById('refid').value;

            if (refId == null) refId = "";
            await rawData(refId, isPromo);
        }
    }
});

const rawData = (refId, isPromo) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : '/tools/blitz-rawdata/get-data/raw',
            type        : 'GET',
            data        : {refid: refId, promoplan: isPromo},
            async       : true,
            dataType    : 'JSON',
            beforeSend: function() {
                blockUI.block();
            },
            success: function (result) {
                if (!result.error) {
                    dt_promo_creation_info.clear().draw();
                    dt_baseline_calculation.clear().draw();
                    dt_result_raw.clear().draw();
                    dt_sales_calculation.clear().draw();

                    $('#promorefid').val('');
                    $('#planningcreationdate').val('');
                    $('#period').val('');
                    $('#promoperiod').val('');

                    dt_promo_creation_info.rows.add(result.data[0].promoPlanBaseline).draw();
                    dt_baseline_calculation.rows.add(result.data[0].baselineRawBSCalculation).draw();
                    dt_result_raw.rows.add(result.data[0].baselineRawResult).draw();
                    dt_sales_calculation.rows.add(result.data[0].baselineCalculation).draw();

                    $('#txt_type_promo').text((isPromo === "1" ? "Promo Planning ID" : "Promo ID"))
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete:function(){
                blockUI.release();
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    });
}

$('#btn_export_actual_sales').on('click', function (e) {
    let isPromo = $('#filter_promo').val();
    let refId = document.getElementById('refid').value;

    if (refId == null) refId = "";
    let url = "/tools/blitz-rawdata/export-xls/actual-sales?refid=" + refId + '&promoplan=' + isPromo;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_baseline').on('click', function (e) {
    let isPromo = $('#filter_promo').val();
    let refId = document.getElementById('refid').value;

    if (refId == null) refId = "";
    let url = "/tools/blitz-rawdata/export-xls/baseline?refid=" + refId + '&promoplan=' + isPromo;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

'use strict';
var dt_summary_late_creation;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_summary_late_creation = $('#dt_summary_late_creation').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        processing: true,
        serverSide: false,
        searching: true,
        paging: false,
        scrollCollapse: true,
        scrollX: true,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'channelDesc',
                width: 80,
                title: 'Channel',
                className: 'text-nowrap align-middle text-start',
            },
            {
                targets: 1,
                title: 'Total OPTIMA Created',
                data: 'totOptimaCreated',
                width: 100,
                className: 'text-nowrap align-middle text-end',
            },
            {
                targets: 2,
                title: '# OPTIMA On Time',
                data: 'onTime',
                width: 100,
                className: 'text-nowrap align-middle text-end',
            },
            {
                targets: 3,
                title: '% On Time Submission',
                data: 'onTimePCT',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,1) + '%';
                }
            },
            {
                targets: 4,
                title: '# OPTIMA Late',
                data: 'late',
                width: 100,
                className: 'text-nowrap align-middle text-end',
            },
            {
                targets: 5,
                title: '% Late Submission',
                data: 'latePCT',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,1) + '%';
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
        footerCallback: function ( row, data, start, end, display ){

            let api = this.api();
            let numRows = api.rows().count();
            let intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[, Rs]|(\.\d{2})/g, "") * 1 :
                    typeof i === 'number' ?
                        i : 0;
            },
            totOptimaCreated = api.column(1).data().reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);

            $(api.column(1).footer()).html(formatMoney(totOptimaCreated, 0));

            let optimaOnTime = api.column(2).data().reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);

            $(api.column(2).footer()).html(formatMoney(optimaOnTime, 0));

            let onTimeSubmission;
            (optimaOnTime|| totOptimaCreated) ? onTimeSubmission = (100 * (optimaOnTime / totOptimaCreated)) : onTimeSubmission = 0;
            $(api.column(3).footer()).html(formatMoney(onTimeSubmission, 1).toString() + '%');

            let optimaLate = api.column(4).data().reduce(function (a, b) {
                return intVal(a) + intVal(b);
            }, 0);

            $(api.column(4).footer()).html(optimaLate);

            let lateSubmission;
            (optimaOnTime|| totOptimaCreated) ? lateSubmission = (100 * (optimaLate / totOptimaCreated)) : lateSubmission = 0;
            $(api.column(5).footer()).html(formatMoney(lateSubmission, 1).toString() + '%');
        }
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

    $('#dt_summary_late_creation_search').on('keyup', function () {
        dt_summary_late_creation.search(this.value).draw();
    });

});

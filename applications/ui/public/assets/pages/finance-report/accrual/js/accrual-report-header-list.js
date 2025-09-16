'use strict';

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_report_header = $('#dt_report_header').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        scrollY: "30vh",
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 300,
                title: 'Download',
                className: 'text-nowrap align-middle text-center',
                render: function (data, type, full, meta) {
                    return '<button class="btn btn-sm btn-outline-optima text-hover-white btn-icon btn_download"><i class="bi bi-download"></i></button>'
                }
            },
            {
                targets: 1,
                title: 'User',
                data: 'userId',
                width: 300,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Periode',
                data: 'periode',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Entity',
                data: 'entity',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Closing Date',
                data: 'closingDt',
                width: 300,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 5,
                title: 'Generate On',
                data: 'createOn',
                width: 300,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDateTime(data);
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

    $("#dt_report_header").on('click', '.btn_download', function () {
        let tr = this.closest("tr");
        let trdata = dt_report_header.row(tr).data();
        let url = '/fin-rpt/accrual/download/report-header/by-id?id=' + trdata.id + '&entity=' + $("#filter_entity").val() +'&period=' + $("#filter_period").val() + '&userid=' + trdata.userId + '&closingdt=' + trdata.closingDt;

        let a = document.createElement("a");
        a.href = url;
        let evt = document.createEvent("MouseEvents");
        //the tenth parameter of initMouseEvent sets ctrl key
        evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
            true, false, false, false, 0, null);
        a.dispatchEvent(evt);
    });

});

'use strict';

var swalTitle = "Promo Planning Approval";
var dt_promo_planning_approval;
heightContainer = 430;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    new KTDialer(dialerElement, {
        min: 1900,
        step: 1,
    });

    dt_promo_planning_approval = $('#dt_promo_planning_approval').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        processing: true,
        serverSide: false,
        paging: true,
        ordering: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[0, 'asc']],
        columnDefs: [
            {
                targets: 0,
                title: 'Ref ID',
                data: 'planningrefid',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'TSCode',
                data: 'tscode',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Investment',
                data: 'investment',
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
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
        footerCallback: function (row, data, start, end, display) {
            let api = this.api();
            let intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // Total over all pages
            let total = api
                .column(2, { filter: 'applied' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            $('.dt-footer').html("<div class='mb-2'>Total Investment : <span id='tot'>" + formatMoney(total, 0) + "</span></div>");
        },
    });

});

$('#btn_download_template').on('click', function () {
    let period = $('#filter_period').val();
    let url = "/promo/planning-approval/download-template?period=" + period;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_upload').on('click', function () {
    let e = document.querySelector("#btn_upload");
    if(document.getElementById("file").files.length > 0 ){
        let formData = new FormData($('#form_approval')[0]);
        let url = '/promo/planning-approval/approve';
        $.ajax({
            url: url,
            data: formData,
            type: 'POST',
            async: true,
            dataType: 'JSON',
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
            },
            success: function (result, status, xhr, $form) {
                if (!result.error) {
                    Swal.fire({
                        title: swalTitle,
                        text: 'Files Uploaded!',
                        icon: "success",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        $('#form_approval')[0].reset();
                        let data = result.data;
                        $('#card_result_approved').removeClass('d-none');
                        dt_promo_planning_approval.clear().draw();
                        dt_promo_planning_approval.rows.add(data.planning).draw();
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete: function () {
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown)
                Swal.fire({
                    title: swalTitle,
                    text: "File Upload Failed",
                    icon: "error",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        });
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please attach file",
            icon: "warning",
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

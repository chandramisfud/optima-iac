'use strict';

var dt_drive;
var swalTitle = "Application Drive";
heightContainer = 290;

$(document).ready(function () {

    dt_drive = $('#dt_drive').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[0, 'asc']],
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                title: 'ID',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 1,
                data: 'row',
                title: 'Row',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                data: 'fileName',
                title: 'File Name',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                data: 'dateModified',
                title: 'Date Modified',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                data: 'size',
                title: 'Size',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full, meta) {
                    let byte = parseFloat(data);
                    let kb = (byte / 1000);
                    return formatMoney(kb, 2,".", ",") + ' KB';
                }
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });
});

$('#dt_drive_search').on('keyup', function () {
    dt_drive.search(this.value).draw();
});

$('#btn_view').on('click', function () {
    let btn = document.getElementById('btn_view');
    let type = $('#attachment_type').val();
    if (type !== "") {
        btn.setAttribute("data-kt-indicator", "on");
        btn.disabled = !0;
        dt_drive.ajax.url('/app-drive/list?type=' + type).load(function () {
            btn.setAttribute("data-kt-indicator", "off");
            btn.disabled = !1;
        });
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please select an attachment type",
            icon: "warning",
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

$('#btn_export_excel').on('click', function() {
    let type = $('#attachment_type').val();
    if (type !== "") {
        let url = '/app-drive/export-excel?type=' + type;
        let a = document.createElement("a");
        a.href = url;
        let evt = document.createEvent("MouseEvents");
        evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
            true, false, false, false, 0, null);
        a.dispatchEvent(evt);
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please select an attachment type",
            icon: "warning",
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

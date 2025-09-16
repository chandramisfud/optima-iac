'use strict';

var dt_log;
var swalTitle = "Application Log";
heightContainer = 305;

$(document).ready(function () {

    $('#filter_date').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    dt_log = $('#dt_log').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'desc']],
        ajax: {
            url: "/app-log/list",
            type: 'get',
        },
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
                data: 'nameFile',
                width: 10,
                orderable:false,
                title: '',
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return '<button class="btn btn-icon btn-sm btn-optima btn-clean btn_download" title="Download"><i class="fa fa-download text-optima fs-6"></i></button>';
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 1,
                title: 'Files',
                data: 'nameFile',
                width: 250,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return data.replace("optima-", "");
                    } else {
                        return "";
                    }
                }
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $("#dt_log").on('click', '.btn_download', function () {
        let tr = this.closest("tr");
        let trdata = dt_log.row(tr).data();
        window.open('/app-log/download?f='+trdata.nameFile);
    });
});

$('#btn_clear_date').on('click', function () {
    $('#filter_date').flatpickr().clear();
});

$('#dt_log_view').on('click', function (){
    let btn = document.getElementById('dt_log_view');
    let filter_date = ($('#filter_date').val()) ?? "";
    let url = "/app-log/list?date=" + filter_date;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_log.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

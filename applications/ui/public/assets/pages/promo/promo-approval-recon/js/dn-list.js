'use strict';
let dt_list_dn;

$(document).ready(function () {
    $('form').submit(false);

    dt_list_dn = $('#dt_list_dn').DataTable({
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
        order: [[0, 'asc']],
        columnDefs: [
            {
                title: 'DN Number',
                targets: 0,
                data: 'refId',
                className: 'text-nowrap align-middle',
                render: function (data, type, full) {
                    return '<a href="/promo/approval-recon/dn?id='+ full['id'] +'" class="text-optima fw-bold fw-bold cursor-pointer" title="DN Information">'+data+'</a>'
                }
            },
            {
                targets: 1,
                title: 'Status',
                data: 'lastStatus',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'DPP',
                data: 'dpp',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 3,
                title: 'PPN',
                data: 'ppnAmt',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 4,
                title: 'PPH',
                data: 'pphAmt',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 5,
                title: 'VAT Expired',
                data: 'vATExpired',
                className: 'text-nowrap align-middle text-center',
                render: function (data) {
                    if (data) {
                        return "Y";
                    } else {
                        return "N";
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });
});

$('#dt_list_dn_search').on('keyup', function() {
    dt_list_dn.search(this.value, false, false).draw();
});

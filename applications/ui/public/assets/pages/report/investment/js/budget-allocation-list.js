'use strict';

var swalTitleModal = "Investment Report";
var dt_budget_allocation_list;


$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_budget_allocation_list = $('#dt_budget_allocation_list').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        scrollY: "50vh",
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                title: 'ID',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Period',
                data: 'periode',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Ref ID',
                data: 'refId',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Type',
                data: 'budgetType',
                width: 50,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Description',
                data: 'longDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Amount',
                data: 'budgetAmount',
                width: 200,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0)
                }
            },

        ],
        initComplete: function (settings, json) {
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
            tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl)
            });
        },
        drawCallback: function (settings, json) {
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
            tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl)
            });
            KTMenu.createInstances();
        },
    });

    $('#dt_budget_allocation_list').on( 'dblclick', 'tr', function () {
        let data = dt_budget_allocation_list.row( this ).data();
        $('#filter_budget_allocation_id').val(data.id);
        $('#modal_list_budget_allocation').modal('hide');
        dt_budget_allocation_list.clear().draw();
    });

});

$('#dt_budget_allocation_list_search').on('keyup', function () {
    dt_budget_allocation_list.search(this.value).draw();
});

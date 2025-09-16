'use strict';

let dt_source_budget_list;
let elDtSourceBudget = $('#dt_source_budget_list');

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_source_budget_list = elDtSourceBudget.DataTable({
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
        scrollY: "40vh",
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Allocation ID',
                data: 'id',
                width: 100,
                visible: false,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 1,
                title: 'Ref ID',
                data: 'refId',
                width: 100,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 2,
                title: 'Sub Category',
                data: 'subCategory',
                width: 150,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 3,
                title: 'Budget Type',
                data: 'budgetType',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 4,
                title: 'Description',
                data: 'longDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 5,
                title: 'Budget Amount',
                data: 'budgetAmount',
                className: 'text-nowrap align-middle text-end cursor-pointer',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0)
                }
            },
            {
                targets: 6,
                title: 'Distributor',
                data: 'distributor',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 7,
                title: 'Remaining Budget',
                data: 'remainingBudget',
                className: 'text-nowrap align-middle text-end cursor-pointer',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0)
                }
            },

        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    elDtSourceBudget.on( 'dblclick', 'tr', async function () {
        let data = dt_source_budget_list.row( this ).data();
        allocationId = data.id;
        $('#allocationRefId').val(data.refId);
        $('#allocationDesc').val(data.longDesc);
        $('#budgetDeployed').val(formatMoney(data.budgetAmount, 0));
        $('#budgetRemaining').val(formatMoney(data.remainingBudget, 0));

        $('#modal_source_budget_list').modal('hide');
        validator.revalidateField('allocationRefId');
    });
});

$('#dt_source_budget_list_search').on('keyup', function () {
    dt_source_budget_list.search(this.value, false, false).draw();
});

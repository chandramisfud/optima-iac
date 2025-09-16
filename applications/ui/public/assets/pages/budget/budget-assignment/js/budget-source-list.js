'use strict';

var dt_budget_source_list;

var targetModal = document.querySelector(".modal-content");
var blockUIModal = new KTBlockUI(targetModal, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListEntity();

    dt_budget_source_list = $('#dt_budget_source_list').DataTable({
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
                title: 'Period',
                data: 'periode',
                width: 100,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 1,
                title: 'Ref ID',
                data: 'refId',
                width: 150,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 2,
                title: 'Description',
                data: 'longDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 3,
                title: 'Owner Name',
                data: 'ownerId',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 4,
                title: 'Amount',
                data: 'budgetAmount',
                width: 200,
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

    $('#dt_budget_source_list').on( 'dblclick', 'tr', async function () {
        let data = dt_budget_source_list.row( this ).data();
        blockUIModal.block();
        let getData = await getDataBudgetSource(data.id);
        blockUIModal.release();
        if (getData) {
            $('#modal_list_budget_source').modal('hide');
        } else {
            Swal.fire({
                title: "Budget Source",
                text: "Failed get data detail",
                icon: "error",
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });

});

$('#dt_budget_source_list_search').on('keyup', function () {
    dt_budget_source_list.search(this.value).draw();
});

$('#dt_budget_source_list_view').on('click', function (){
    let btn = document.getElementById('dt_budget_source_list_view');
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/budget/assignment/list/source?period=" + $('#period').val() + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_budget_source_list.clear().draw();
    dt_budget_source_list.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#filter_entity').on('change', async function () {
    blockUIModal.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUIModal.release();
    $('#filter_distributor').val('').trigger('change');
});

const getListEntity = () => {
    $.ajax({
        url         : "/budget/assignment/list/entity",
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
        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(jqXHR.responseText);
        }
    });
}

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/assignment/list/distributor/entity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {entityId: entityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_distributor').select2({
                    placeholder: "Select a Distributor",
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

const getDataBudgetSource = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/assignment/get-data/source/id",
            type        : "GET",
            dataType    : 'json',
            data        : {id: id},
            async       : true,
            success: function(result) {
                if (!result.error) {
                    let budgetAllocation = result.data.budgetAllocation;
                    let userAccess = (result.data.userAccess ?? []);
                    let budgetAssignment = result.data.budgetAssignment;
                    let data = [];
                    for (let j = 0, len = userAccess.length; j < len; ++j){
                        if (userAccess[j].flag) {
                            data.push({
                                id: userAccess[j].id,
                                text: userAccess[j].id
                            });
                        }
                    }
                    $('#profileId').select2({
                        placeholder: "Select a User",
                        width: '100%',
                        data: data
                    });

                    budgetId = budgetAssignment.id;
                    allocationId = budgetAllocation.id;
                    frownId = budgetAllocation.fromOwnerId;
                    $('#longDesc').val(budgetAllocation.longDesc);
                    $('#fromOwnerName').val(budgetAllocation.fromOwnerName);
                    $('#entityLongDesc').val(budgetAllocation.entity);
                    $('#budgetAssigned').val(budgetAllocation.budgetAmount);
                    $('#budgetRemaining').val('0');

                    return resolve(true);
                } else {
                    return reject(false);
                }
            },
            complete: function() {
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(false);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

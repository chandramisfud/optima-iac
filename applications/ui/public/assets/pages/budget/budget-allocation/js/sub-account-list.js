'use strict';

var dt_subaccount;
var dt_account;
var target_subaccount = document.querySelector("#subaccount");
var blockUISubAccount = new KTBlockUI(target_subaccount, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_subaccount = $('#dt_subaccount').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-12'i>>",
        order: [[1, 'asc']],
        processing: true,
        serverSide: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "40vh",
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 25,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center',
                checkboxes: {
                    'selectRow': false
                },
                createdCell:  function (td, cellData, rowData, row, col){
                    if (load_data) {
                        if(rowData['flag']){
                            this.api().cell(td).checkboxes.select();
                            $(td.firstElementChild).trigger('click');
                        }
                    }
                }
            },
            {
                targets: 1,
                title: 'Description',
                data: 'longDesc',
                className: 'text-nowrap align-middle',
            },

        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

});

$('#dt_subaccount_search').on('keyup', function() {
    dt_subaccount.search(this.value).draw();
});

const getListSubAccount = (accountId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/sub-account",
            type        : "GET",
            dataType    : 'json',
            data        : {accountId: accountId},
            async       : true,
            success: function(result) {
                if (!result.error) {
                    resolve(result.data);
                } else {
                    resolve([]);
                }
            },
            complete: function() {

            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log();
                return reject([]);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getArrayListSubAccount = (accountId) => {
    return new Promise(async (resolve, reject) => {
        let data_sub_accounts = [];
        for (let i=0; i<list_sub_accounts.length; i++) {
            if (accountId === list_sub_accounts[i].accountId) {
                data_sub_accounts.push(list_sub_accounts[i]);
            }
        }
        resolve(data_sub_accounts);
    }).catch((e) => {
        console.log(e);
    });
}

const removeTickRowsSubAccount = () => {
    let el_rows_sub_accounts = $('#dt_subaccount_wrapper .dt-checkboxes');
    for (let i=0; i<el_rows_sub_accounts.length; i++) {
        if (el_rows_sub_accounts[i].checked) {
            $(el_rows_sub_accounts[i]).trigger('click');
        }
    }
}

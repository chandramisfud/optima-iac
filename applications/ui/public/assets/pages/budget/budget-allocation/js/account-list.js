'use strict';

var dt_account;
var target_account = document.querySelector("#account");
var blockUIAccount = new KTBlockUI(target_account, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_account = $('#dt_account').DataTable({
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
                    'selectRow': false,
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

    $('#dt_account_wrapper').on('change', 'thead th #dt-checkbox-header', async function () {
        if (this.checked) {
            // get data sub account by account id
            let data_account = dt_account.column(0).checkboxes.selected();
            let list_sub_account = [];
            if (data_account.length > 0) {
                for (let i=0; i<data_account.length; i++) {
                    let list = await getArrayListSubAccount(data_account[i]);
                    if (list.length > 0) {
                        list_sub_account.push(...list);
                    }
                }

                // fill data sub account
                dt_subaccount.clear().draw();
                let el_header_account = $('#dt_account_wrapper #dt-checkbox-header');
                if (el_header_account[0].checked) {
                    dt_subaccount.rows.add(list_sub_account).draw();
                }

                // enable checkbox header sub account
                $('#dt_subaccount_wrapper #dt-checkbox-header').prop('disabled', false);

                // tick header sub account
                if (list_sub_account.length > 0) {
                    let el_header_sub_accounts = $('#dt_subaccount_wrapper #dt-checkbox-header');
                    el_header_sub_accounts[0].checked = false;
                    if (!el_header_sub_accounts[0].checked) {
                        el_header_sub_accounts.trigger('click');
                    }
                }

                // disable checkbox header sub account
                $('#dt_subaccount_wrapper #dt-checkbox-header').prop('disabled', true);

                // disable checkbox rows sub account
                let el_rows_sub_accounts = $('#dt_subaccount_wrapper .dt-checkboxes');
                for (let i=0; i<el_rows_sub_accounts.length; i++) {
                    $(el_rows_sub_accounts[i]).prop('disabled', true);
                }
            }
        } else {
            // enable checkbox header sub account
            $('#dt_subaccount_wrapper #dt-checkbox-header').prop('disabled', false);

            // remove checked header sub account
            let el_header_sub_account = $('#dt_subaccount_wrapper #dt-checkbox-header');
            if (el_header_sub_account[0].checked) {
                el_header_sub_account.trigger('click');
            }

            // remove rows sub account
            dt_subaccount.clear().draw();
        }
    });

    $('#dt_account').on('change', 'tbody td .dt-checkboxes', async function () {
        let row_data = dt_account.row(this.closest('tr')).data();
        if (this.checked) {
            // un-tick rows sub account
            removeTickRowsSubAccount();

            // fill data sub account by account id
            let data_sub_account = await getArrayListSubAccount(row_data.id);
            dt_subaccount.rows.add(data_sub_account).draw();

            // tick rows sub account
            if (data_sub_account.length > 0) {
                let el_rows_sub_accounts = $('#dt_subaccount_wrapper .dt-checkboxes');
                for (let i=0; i<el_rows_sub_accounts.length; i++) {
                    if (!el_rows_sub_accounts[i].checked) {
                        $(el_rows_sub_accounts[i]).trigger('click');
                    } else if (el_rows_sub_accounts[i].checked) {
                        $(el_rows_sub_accounts[i]).trigger('click');
                        $(el_rows_sub_accounts[i]).trigger('click');
                    }

                    // disable checkbox rows sub account
                    $(el_rows_sub_accounts[i]).prop('disabled', true);
                }

                // disable checkbox header sub account
                $('#dt_subaccount_wrapper #dt-checkbox-header').prop('disabled', true);
            }
        } else {
            // un-tick rows sub account
            removeTickRowsSubAccount();

            dt_subaccount.rows( function ( idx, data, node ) {
                return row_data.id == data.accountId;
            }).remove().draw();
        }
    });

});

$('#dt_account_search').on('keyup', function() {
    dt_account.search(this.value).draw();
});

const getListAccount = (subChannelId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/account",
            type        : "GET",
            dataType    : 'json',
            data        : {subChannelId: subChannelId},
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

const getArrayListAccount = (subChannelId) => {
    return new Promise(async (resolve, reject) => {
        let data_accounts = [];
        for (let i=0; i<list_accounts.length; i++) {
            if (subChannelId === list_accounts[i].subChannelId) {
                data_accounts.push(list_accounts[i]);
            }
        }
        resolve(data_accounts);
    }).catch((e) => {
        console.log(e);
    });
}

const removeTickRowsAccount = () => {
    let el_rows_accounts = $('#dt_account_wrapper .dt-checkboxes');
    for (let i=0; i<el_rows_accounts.length; i++) {
        if (el_rows_accounts[i].checked) {
            $(el_rows_accounts[i]).trigger('click');
        }
    }
}

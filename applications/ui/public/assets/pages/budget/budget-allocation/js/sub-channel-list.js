'use strict';

var dt_subchannel;
var target_subchannel = document.querySelector("#subchannel");
var blockUISubChannel = new KTBlockUI(target_subchannel, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_subchannel = $('#dt_subchannel').DataTable({
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

    $('#dt_subchannel_wrapper').on('change', 'thead th #dt-checkbox-header', async function () {
        if (this.checked) {
            let data_subchannels = dt_subchannel.column(0).checkboxes.selected();
            let list_account = [];
            if (data_subchannels.length > 0) {
                // get data account by sub channel id
                for (let i=0; i<data_subchannels.length; i++) {
                    let list = await getArrayListAccount(data_subchannels[i]);
                    if (list.length > 0) {
                        list_account.push(...list);
                    }
                }

                // un-tick rows account
                removeTickRowsAccount();

                // un-tick rows sub account
                removeTickRowsSubAccount();

                // fill set data account
                dt_account.clear().draw();
                let el_header_sub_channels = $('#dt_subchannel_wrapper #dt-checkbox-header');
                if (el_header_sub_channels[0].checked) {
                    dt_account.rows.add(list_account).draw();
                }

                // un-tick header account
                if (list_account.length > 0) {
                    let el_header_account = $('#dt_account_wrapper #dt-checkbox-header');
                    if (el_header_account[0].checked) {
                        el_header_account.trigger('click');
                    }
                }
            }
        } else {
            // enable checkbox header sub account
            $('#dt_subaccount_wrapper #dt-checkbox-header').prop('disabled', false);

            // un-tick rows sub account
            removeTickRowsSubAccount();

            // un-tick rows account
            removeTickRowsAccount();

            // remove checked header account
            let el_header_account = $('#dt_account_wrapper #dt-checkbox-header');
            if (el_header_account[0].checked) {
                el_header_account.trigger('click');
            }

            // clear data account and sub account
            dt_account.clear().draw();
            dt_subaccount.clear().draw();
        }
    });

    $('#dt_subchannel').on('change', 'tbody td .dt-checkboxes', async function () {
        let row_data = dt_subchannel.row(this.closest('tr')).data();
        if (this.checked) {
            // un-tick rows account
            removeTickRowsAccount();

            // un-tick rows sub account
            removeTickRowsSubAccount();

            // fill data account by sub channel id
            let data_account = await getArrayListAccount(row_data.id);
            dt_account.rows.add(data_account).draw();

            // remove rows sub account
            dt_subaccount.rows().remove().draw();
        } else {
            // un-tick rows sub account
            removeTickRowsSubAccount();

            // un-tick rows account
            removeTickRowsAccount();

            dt_account.rows( function ( idx, data, node ) {
                return row_data.id === data.subChannelId;
            }).remove().draw();

            // remove rows sub account
            dt_subaccount.rows().remove().draw();
        }
    });

});

$('#dt_subchannel_search').on('keyup', function() {
    dt_subchannel.search(this.value).draw();
});

const getListSubChannel = (channelId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/sub-channel",
            type        : "GET",
            dataType    : 'json',
            data        : {channelId: channelId},
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

const getArrayListSubChannel = (channelId) => {
    return new Promise(async (resolve, reject) => {
        let data_sub_channels = [];
        for (let i=0; i<list_sub_channels.length; i++) {
            if (channelId === list_sub_channels[i].channelId) {
                data_sub_channels.push(list_sub_channels[i]);
            }
        }
        resolve(data_sub_channels);
    }).catch((e) => {
        console.log(e);
    });
}

const removeTickRowsSubChannel = () => {
    let el_rows_sub_channels = $('#dt_subchannel_wrapper .dt-checkboxes');
    for (let i=0; i<el_rows_sub_channels.length; i++) {
        if (el_rows_sub_channels[i].checked) {
            $(el_rows_sub_channels[i]).trigger('click');
        }
    }
}

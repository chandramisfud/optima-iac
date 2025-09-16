'use strict';

var dt_channel;
var target_channel = document.querySelector("#channel");
var blockUIChannel = new KTBlockUI(target_channel, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_channel = $('#dt_channel').DataTable({
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

    $('#dt_channel_wrapper').on('change', 'thead th #dt-checkbox-header', async function () {
        if (this.checked) {
            let data_channels = dt_channel.column(0).checkboxes.selected();
            let list_sub_channels = [];
            if (data_channels.length > 0) {
                // get data sub channel by channel id
                for (let i=0; i<data_channels.length; i++) {
                    let list = await getArrayListSubChannel(data_channels[i]);
                    if (list.length > 0) {
                        list_sub_channels.push(...list);
                    }
                }

                // un-tick rows sub account
                removeTickRowsSubAccount();

                // un-tick rows account
                removeTickRowsAccount();

                // un-tick rows sub channel
                removeTickRowsSubChannel();

                // un-tick header sub channel
                let el_header_sub_channels = $('#dt_subchannel_wrapper #dt-checkbox-header');
                if (el_header_sub_channels[0].checked) {
                    el_header_sub_channels.trigger('click');
                }

                // fill data sub channel
                dt_subchannel.clear().draw();
                let el_header_channels = $('#dt_channel_wrapper #dt-checkbox-header');
                if (el_header_channels[0].checked) {
                    dt_subchannel.rows.add(list_sub_channels).draw();
                }

                // tick header sub channel
                if (list_sub_channels.length > 0) {
                    let el_header_sub_channels = $('#dt_subchannel_wrapper #dt-checkbox-header');
                    el_header_sub_channels[0].checked = false;
                    if (!el_header_sub_channels[0].checked) {
                        el_header_sub_channels.trigger('click');
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

            // un-tick header sub channel
            let el_header_sub_channels = $('#dt_subchannel_wrapper #dt-checkbox-header');
            if (el_header_sub_channels[0].checked) {
                el_header_sub_channels.trigger('click');
            }

            // clear data sub channel, account, sub account
            dt_subchannel.clear().draw();
            dt_account.clear().draw();
            dt_subaccount.clear().draw();
        }
    });

    $('#dt_channel').on('change', 'tbody td .dt-checkboxes', async function () {
        let row_data = dt_channel.row(this.closest('tr')).data();

        if (this.checked) {
            // un-tick rows sub account
            removeTickRowsSubAccount();

            // un-tick rows account
            removeTickRowsAccount();

            // fill data sub channel by channel id
            let data_sub_channel = await getArrayListSubChannel(row_data.id);
            dt_subchannel.rows.add(data_sub_channel).draw();

            // tick rows sub channel
            if (data_sub_channel.length > 0) {
                let el_rows_sub_channels = $('#dt_subchannel_wrapper .dt-checkboxes');
                for (let i=0; i<el_rows_sub_channels.length; i++) {
                    if (!el_rows_sub_channels[i].checked) {
                        $(el_rows_sub_channels[i]).trigger('click');
                    } else if (el_rows_sub_channels[i].checked) {
                        $(el_rows_sub_channels[i]).trigger('click');
                        $(el_rows_sub_channels[i]).trigger('click');
                    }
                }
            }

            // remove rows sub account
            dt_subaccount.rows().remove().draw();
        } else {
            // un-tick all sub account
            removeTickRowsSubAccount();

            // un-tick all account
            removeTickRowsAccount();

            let removed_sub_channel_id = []
            // remove sub channel by channel id removed
            dt_subchannel.rows( function ( idx, data, node ) {
                if (row_data.id === data.channelId) removed_sub_channel_id.push(data.id);
                return row_data.id === data.channelId;
            }).remove().draw();

            // remove account by sub channel id removed
            for (let i=0; i<removed_sub_channel_id.length; i++) {
                let subChannelId = removed_sub_channel_id[i];
                dt_account.rows( function ( idx, data, node ) {
                    return subChannelId === data.subChannelId;
                }).remove().draw();
            }

            // remove all sub account
            dt_subaccount.rows().remove().draw();
        }
    });

});

$('#dt_channel_search').on('keyup', function() {
    dt_channel.search(this.value).draw();
});

const getListChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/channel",
            type        : "GET",
            dataType    : 'json',
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

const removeTickRowsChannel = () => {
    let el_rows_channels = $('#dt_channel_wrapper .dt-checkboxes');
    for (let i=0; i<el_rows_channels.length; i++) {
        if (el_rows_channels[i].checked) {
            $(el_rows_channels[i]).trigger('click');
        }
    }
}

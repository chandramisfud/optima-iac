'use strict';

var dt_user;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_user = $('#dt_user').DataTable({
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
                    if(rowData['flag']){
                        this.api().cell(td).checkboxes.select();
                        $(td.firstElementChild).trigger('click');
                    }
                }
            },
            {
                targets: 1,
                title: 'User ID',
                data: 'id',
                className: 'text-nowrap align-middle',
            },

        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

});

$('#dt_user_search').on('keyup', function() {
    dt_user.search(this.value).draw();
});

const getListUser = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/user",
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

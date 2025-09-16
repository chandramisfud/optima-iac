'use strict';

var dt_sku;
var target_sku = document.querySelector("#sku");
var blockUISKU = new KTBlockUI(target_sku, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_sku = $('#dt_sku').DataTable({
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

$('#dt_sku_search').on('keyup', function() {
    dt_sku.search(this.value).draw();
});

const getListSKU = (brandId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/sku",
            type        : "GET",
            dataType    : 'json',
            data        : {brandId: brandId},
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

const getArrayListSKU = (brandId) => {
    return new Promise(async (resolve, reject) => {
        let data_skus = [];
        for (let i=0; i<list_skus.length; i++) {
            if (brandId === list_skus[i].brandId) {
                data_skus.push(list_skus[i]);
            }
        }
        resolve(data_skus);
    }).catch((e) => {
        console.log(e);
    });
}

const removeTickRowsSKU = () => {
    let el_rows_skus = $('#dt_sku_wrapper .dt-checkboxes');
    for (let i=0; i<el_rows_skus.length; i++) {
        if (el_rows_skus[i].checked) {
            $(el_rows_skus[i]).trigger('click');
        }
    }
}

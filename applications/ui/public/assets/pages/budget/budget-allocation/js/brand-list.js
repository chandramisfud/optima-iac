'use strict';

var dt_brand;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_brand = $('#dt_brand').DataTable({
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

    $('#dt_brand_wrapper').on('change', 'thead th #dt-checkbox-header', async function () {
        if (this.checked) {
            let data_brand = dt_brand.column(0).checkboxes.selected();
            let list_sku = [];
            if (data_brand.length > 0) {
                //get data sku by brand id
                for (let i=0; i<data_brand.length; i++) {
                    let list = await getArrayListSKU(data_brand[i]);
                    if (list.length > 0) {
                        list_sku.push(...list);
                    }
                }

                // un-tick rows sku
                removeTickRowsSKU();

                // un-tick header sku
                let el_header_skus = $('#dt_sku_wrapper #dt-checkbox-header');
                if (el_header_skus[0].checked) {
                    el_header_skus.trigger('click');
                }

                // fill data sku
                dt_sku.clear().draw();
                let el_header_brands = $('#dt_brand_wrapper #dt-checkbox-header');
                if (el_header_brands[0].checked) {
                    dt_sku.rows.add(list_sku).draw();
                }

                // tick header sku
                if (list_sku.length > 0) {
                    let el_header_sku = $('#dt_sku_wrapper #dt-checkbox-header');
                    el_header_sku[0].checked = false;
                    if (!el_header_sku[0].checked) {
                        el_header_sku.trigger('click');
                    }
                }
            }
        } else {
            // remove checked header sku
            let el_header_sku = $('#dt_sku_wrapper #dt-checkbox-header');
            if (el_header_sku[0].checked) {
                el_header_sku.trigger('click');
            }

            // clear data sku
            dt_sku.clear().draw();
        }
    });

    $('#dt_brand').on('change', 'tbody td .dt-checkboxes', async function () {
        let row_data = dt_brand.row(this.closest('tr')).data();

        if (this.checked) {
            // fill data sku by brand id
            let data_sku = await getArrayListSKU(row_data.id);
            dt_sku.rows.add(data_sku).draw();

            // tick rows sku
            if (data_sku.length > 0) {
                let el_rows_skus = $('#dt_sku_wrapper .dt-checkboxes');
                for (let i=0; i<el_rows_skus.length; i++) {
                    if (!el_rows_skus[i].checked) {
                        $(el_rows_skus[i]).trigger('click');
                    } else if (el_rows_skus[i].checked) {
                        $(el_rows_skus[i]).trigger('click');
                        $(el_rows_skus[i]).trigger('click');
                    }
                }
            }
        } else {
            // remove sku by brand id removed
            dt_sku.rows( function ( idx, data, node ) {
                return row_data.id === data.brandId;
            }).remove().draw();
        }
    });

});

$('#dt_brand_search').on('keyup', function() {
    dt_brand.search(this.value).draw();
});

const getListBrand = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/brand",
            type        : "GET",
            dataType    : 'json',
            data        : {entityId: entityId},
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

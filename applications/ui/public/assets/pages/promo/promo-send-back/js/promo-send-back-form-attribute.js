'use strict';

let swalTitleAttribute = "Promo Send Back - Attribute";
let mechanism_row;
let dt_region, dt_brand, dt_sku, dt_mechanism_source, dt_mechanism_result;

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {

    dt_region = $('#dt_region').DataTable({
        dom:
            "<'row'<'dt_region_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: '35vh',
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                data: 'id',
                width: 10,
                searchable: false,
                className: 'text-nowrap dt-body-start',
                checkboxes: {
                    'selectRow': false,
                },
                createdCell: function (td, cellData, rowData) {
                    if (arr_region.includes(rowData.id)) {
                        this.api().cell(td).checkboxes.select();
                    }else{
                        this.api().cell(td).checkboxes.deselect();
                    }
                }
            },
            {
                targets: 1,
                title: 'Region',
                data: 'longDesc',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    dt_brand = $('#dt_brand').DataTable({
        dom:
            "<'row'<'dt_brand_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: '35vh',
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                data: 'id',
                width: 10,
                searchable: false,
                className: 'text-nowrap dt-body-start',
                checkboxes: {
                    'selectRow': false,
                    'selectCallback': async function () {
                        await thickBrand();
                    }
                },
                createdCell:  function (td, cellData, rowData){
                    if(arr_brand.includes(rowData.id)){
                        this.api().cell(td).checkboxes.select();
                    }else{
                        this.api().cell(td).checkboxes.deselect();
                    }
                }
            },
            {
                targets: 1,
                title: 'Sub Brand',
                data: 'longDesc',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_sku = $('#dt_sku').DataTable({
        dom:
            "<'row'<'dt_sku_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: '35vh',
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                data: 'id',
                width: 10,
                searchable: false,
                className: 'text-nowrap dt-body-start',
                checkboxes: {
                    'selectRow': false,
                    'selectCallback': function () {
                        thickSKU();
                    }
                },
                createdCell:  function (td, cellData, rowData){
                    if(arr_sku.includes(rowData.id) || arr_sku_temp.includes(rowData.id)){
                        this.api().cell(td).checkboxes.select();
                    }else{
                        this.api().cell(td).checkboxes.deselect();
                    }
                }
            },
            {
                targets: 1,
                title: 'SKU',
                data: 'longDesc',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_mechanism_source = $('#dt_mechanism_source').DataTable({
        dom:
            "<'row'<'dt_mechanism_source_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: '25vh',
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Double Click to Select Mechanism',
                data: 'mechanism',
                width: 900,
                selected: true
            },
            {
                targets: 1,
                title: 'productId',
                data: 'productId',
                visible: false,
                searchable: false,
            },
            {
                targets: 2,
                title: 'product',
                data: 'product',
                visible: false,
                searchable: false,
            },
            {
                targets: 3,
                title: 'brandid',
                data: 'brandId',
                visible: false,
                searchable: false,
            },
            {
                targets: 4,
                title: 'brand',
                data: 'brand',
                visible: false,
                searchable: false,
            },
            {
                targets: 5,
                title: 'mechanismid',
                data: 'id',
                visible: false,
                searchable: false,
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_mechanism_result = $('#dt_mechanism_result').DataTable({
        dom:
            "<'row'<'dt_mechanism_result_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: '25vh',
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'SKU',
                data: 'productId',
                width: 900,
                visible: false,
                searchable: false,
            },
            {
                targets: 1,
                title: 'SKU',
                data: 'product',
                width: 300,
                className: "text-nowrap",
            },
            {
                targets: 2,
                title: 'Mechanism',
                data: 'mechanism',
                width: 1000,
                className: "text-nowrap",
            },
            {
                targets: 3,
                title: 'Notes',
                data: 'notes',
                width: 400,
                className: "text-nowrap",
            },
            {
                targets: 4,
                title: 'brandid',
                data: 'brandId',
                visible: false,
                searchable: false,
            },
            {
                targets: 5,
                title: 'brand',
                data: 'brand',
                visible: false,
                searchable: false,
            },
            {
                targets: 6,
                title: 'mechanismid',
                data: 'id',
                visible: false,
                searchable: false,
            },
            {
                targets: 7,
                title: '',
                data: 'null',
                orderable: false,
                searchable: false,
                render: function () {
                    return '<button class="btn btn-clear btn-icon-h float-right btn-edit-mechanism-notes" title="Edit Notes"><i class="la la-edit btn-outline-primary" ></i></button>'
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });
});

$('#dt_region_search').on('keyup', function () {
    dt_region.search(this.value, false, false).draw();
});

$('#dt_brand_search').on('keyup', function () {
    dt_brand.search(this.value, false, false).draw();
});

$('#dt_sku_search').on('keyup', function () {
    dt_sku.search(this.value, false, false).draw();
});

$('#dt_mechanism_source_search').on('keyup', function () {
    dt_mechanism_source.search(this.value, false, false).draw();
});

$('#dt_mechanism_result_search').on('keyup', function () {
    dt_mechanism_result.search(this.value, false, false).draw();
});

$('#dt_mechanism_source').on('dblclick', 'tbody tr', function () {
    let data = dt_mechanism_source.row(this).data();
    let idx = dt_mechanism_result
        .columns(2)
        .data()
        .eq(0)
        .indexOf(data.mechanism);

    if (idx === -1) {
        let dbResult = []

        dbResult["productId"] = data.productId
        dbResult["product"] = data.product
        dbResult["mechanism"] = data.mechanism
        dbResult["notes"] = ''
        dbResult["brandId"] = data.brandId
        dbResult["brand"] = data.brand
        dbResult["id"] = data.id

        dt_mechanism_result.row.add(dbResult).draw();
    }
});

$('#dt_mechanism_result').on('dblclick', 'tbody tr', function () {
    let tr = this.closest("tr");
    let trIndex = dt_mechanism_result.row(tr).index();
    dt_mechanism_result.row(trIndex).remove().draw();
});

$("#dt_mechanism_result").on('click', '.btn-edit-mechanism-notes', function () {
    mechanism_row = dt_mechanism_result.row($(this).parents('tr')).index();

    let row_data = dt_mechanism_result.row($(this).parents('tr')).data();

    $('#mechanism_notes').val(row_data.notes);
    $('#modal_mechanism_notes').modal('show');
});

$('#btn_updatenotes').on('click', function () {
    let notes = $('#mechanism_notes').val();

    dt_mechanism_result.cell({ row: mechanism_row, column: 3 }).data(notes);
    $("#modal_mechanism_notes").modal("hide");
});

$('#btn_attribute_submit').on('click', async function () {
    blockUIModal.block();
    let e = document.querySelector("#btn_attribute_submit");
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;

    let err_message = "Data not valid";
    let validateAttribute = 0;
    let dataRegion = await setDataRegion();
    let dataBrand = await setDataBrand();
    let dataSKU = await setDataSKU();

    if (dataRegion.length<=0) {
        err_message = "Please tick region at least one";
        validateAttribute = 1;
    }

    if (newMechanismMethod) {
        if (dt_mechanism_result.rows().count() === 0) {
            err_message = "Please select a mechanism at least one";
            validateAttribute = 7;
        }
    } else {
        if(!$('#mechanisme1').val()){
            err_message = "Please fill in a mechanism";
            validateAttribute = 5;
        }
        if (dataBrand.length<=0) {
            err_message = "Please tick brand at least one";
            validateAttribute = 6;
        }

        if (dataSKU.length<=0) {
            err_message = "Please tick SKU at least one";
            validateAttribute = 7;
        }
    }

    if (validateAttribute === 0) {
        let longDescRegion = '';
        if (dataRegion) {
            if (dataRegion.length > 0) {
                dataRegion.forEach(function (el) {
                    longDescRegion += '<span class="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                });
                $('#card_list_region').html(longDescRegion);
            }
        }

        if (newMechanismMethod ) {
            writeMechanismNew();
        } else {
            writeMechanismManual();
            let longDescBrand = '';
            if (dataBrand) {
                if (dataBrand.length > 0) {
                    dataBrand.forEach(function (el) {
                        longDescBrand += '<span class="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                    });
                    $('#card_list_brand').html(longDescBrand);
                }
            }

            let longDescSKU = '';
            if (dataSKU) {
                if (dataSKU.length > 0) {
                    dataSKU.forEach(function (el) {
                        longDescSKU += '<span class="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                    });
                    $('#card_list_sku').html(longDescSKU);
                }
            }
        }

        $('#modal_attribute').modal('hide');
    }else{
        Swal.fire({
            title: swalTitleAttribute,
            text: err_message,
            icon: "warning",
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"}
        }).then(function () {
        });
    }
    e.setAttribute("data-kt-indicator", "off");
    e.disabled = !1;

    blockUIModal.release();
});

const getRegion = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/send-back/list/region",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                dt_region.clear().draw();
                dt_region.rows.add(result.data).draw()
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const setDataRegion = async function () {
    let dataRowsChecked = [];
    let dataNodes = dt_region.rows().nodes();
    let rowData = dt_region.rows().data();
    let isChecked;
    arr_region = [];
    $.each(dataNodes, function (index) {
        isChecked = $(this).find('input').prop('checked');
        if (isChecked) {
            arr_region.push(rowData[index].id);
            dataRowsChecked.push({
                id: rowData[index].id,
                longDesc: rowData[index].longDesc
            });
        }
    });
    return dataRowsChecked;
}

const getBrand = (entityId) => {
    return new Promise((resolve, reject) => {
        if(entityId) {
            $.ajax({
                url: "/promo/send-back/list/brand",
                type: "GET",
                data: {entityId: entityId},
                dataType: 'json',
                async: true,
                success: function (result) {
                    dt_brand.clear().draw();
                    dt_brand.rows.add(result.data).draw()
                },
                complete: function () {
                    return resolve();
                },
                error: function (jqXHR, textStatus, errorThrown)
                {
                    console.log(jqXHR.responseText);
                    return reject(errorThrown);
                }
            });
        }else{
            return resolve();
        }
    }).catch((e) => {
        console.log(e);
    });
}

const setDataBrand = function () {
    let dataRowsChecked = [];
    if ($.fn.dataTable.isDataTable('#dt_brand')) {
        let dataNodes = dt_brand.rows().nodes();
        let rowData = dt_brand.rows().data();
        let isChecked;
        arr_brand = [];
        $.each(dataNodes, function (index) {
            isChecked = $(this).find('input').prop('checked');
            if (isChecked) {
                arr_brand.push(rowData[index].id);
                dataRowsChecked.push({
                    id: rowData[index].id,
                    longDesc: rowData[index].longDesc
                });
            }
        });
    }
    return dataRowsChecked;
}

const thickBrand = async function  () {
    let rows_selected = dt_brand.column(0).checkboxes.selected();
    dt_sku.clear().draw();
    $.each(rows_selected, async function (index, value) {
        await getSKU(value);
    });
}

const getSKU = (brandId) => {
    return new Promise((resolve, reject) => {
        if (!blockUIModal.blocked) blockUIModal.block();
        $.ajax({
            url         : "/promo/send-back/list/sku",
            type        : "GET",
            data        : {brandId: brandId},
            dataType    : 'json',
            async       : true,
            beforeSend: function() {

            },
            success: function(result) {
                for (let i = 0; i < result.data.length; i++) {
                    let idx = dt_sku
                        .columns(0)
                        .data()
                        .eq(0)
                        .indexOf(result.data[i].id);

                    if (idx === -1) {
                        let dbSKU = []
                        dbSKU["id"] = result.data[i].id
                        dbSKU["longDesc"] = result.data[i].longDesc

                        dt_sku.row.add(dbSKU).draw()
                    }
                }
            },
            complete: function() {
                blockUIModal.release();
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const setDataSKU = function () {
    let dataRowsChecked = [];
    let dataNodes = dt_sku.rows().nodes();
    let rowData = dt_sku.rows().data();
    let isChecked;
    arr_sku = [];
    $.each(dataNodes, function (index) {
        isChecked = $(this).find('input').prop('checked');
        if (isChecked) {
            arr_sku.push(rowData[index].id);
            dataRowsChecked.push({
                id: rowData[index].id,
                longDesc: rowData[index].longDesc
            });
        }
    });
    return dataRowsChecked;
}

const thickSKU = function  () {
    let rows_selected = dt_sku.column(0).checkboxes.selected();
    $.each(rows_selected, function (index, value) {
        arr_sku_temp.push(value);
    });
}

const newPeriodMechanism = () => {
    let periodMechanism = new Date();
    let cutOffMechanism = convertStringToDate(appCutOffMechanism);

    return periodMechanism >= cutOffMechanism;
}

const getMechanismMethod = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/send-back/cutoff",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                appCutOffMechanism = result.app_cutoff_mechanism;
                appCutOffHierarchy = result.app_cutoff_hierarchy;
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getMechanism = () => {
    return new Promise((resolve, reject) => {
        let subCategoryId = ($('#subCategoryId').val() ?? "0");
        let activityId = ($('#activityId').val() ?? "0");
        let subActivityId = ($('#subActivityId').val() ?? "0");
        let channelId = ($('#channelId').val() ?? "0");
        let startDate = formatDate($('#startPromo').val());
        let endDate = formatDate($('#endPromo').val());

        $.ajax({
            url         : "/promo/send-back/list/mechanism",
            type        : "GET",
            data        : {
                entityId: entityId,
                subCategoryId: subCategoryId,
                activityId: activityId,
                subActivityId: subActivityId,
                skuId: 0,
                channelId: channelId,
                startDate: startDate,
                endDate: endDate
            },
            dataType    : 'json',
            async       : true,
            success: function (result) {
                if(newPeriodMechanism() && result.data.length>0){
                    dt_mechanism_source.clear().draw();
                    dt_mechanism_source.rows.add(result.data).draw()

                    newMechanismMethod = true
                } else {
                    newMechanismMethod = false
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const readMechanismManual = () => { //set mechanisme manual dari form main ke modal
    let dataNodes = dt_mechanism.rows().nodes();
    let rowData = dt_mechanism.rows().data();
    let no = 0;
    $.each(dataNodes, function (index) {
        no += 1;
        switch (no) {
            case 1:
                $('#mechanisme1').val(rowData[index].mechanism);
                break;
            case 2:
                $('#mechanisme2').val(rowData[index].mechanism);
                break;
            case 3:
                $('#mechanisme3').val(rowData[index].mechanism);
                break;
            case 4:
                $('#mechanisme4').val(rowData[index].mechanism);
                break;
            default:
        }
    });
}

const writeMechanismManual = () => {
    //set mechanism manual dari modal ke table mechanism di form main

    dt_mechanism.clear().draw();

    arr_mechanism = []
    let dMechanism = []
    let mechanism1 = $('#mechanisme1').val()
    if (mechanism1 !== '') {
        dMechanism = []
        dMechanism["no"] = 1
        dMechanism["mechanism"] = mechanism1
        dMechanism["notes"] = ''
        dMechanism["productId"] = 0
        dMechanism["product"] = ''
        dMechanism["brandId"] = 0
        dMechanism["brand"] = ''
        dMechanism["mechanismId"] = 1

        dt_mechanism.row.add(dMechanism).draw();

        let mechanism = {}
        mechanism.id = 1
        mechanism.mechanism = mechanism1
        mechanism.notes = ''
        mechanism.productId = 0
        mechanism.product = ''
        mechanism.brandId = 0
        mechanism.brand = ''

        arr_mechanism.push(mechanism);

    }

    let mechanism2 = $('#mechanisme2').val()
    if (mechanism2 !== '') {
        dMechanism = []
        dMechanism["no"] = 2
        dMechanism["mechanism"] = mechanism2
        dMechanism["notes"] = ''
        dMechanism["productId"] = 0
        dMechanism["product"] = ''
        dMechanism["brandId"] = 0
        dMechanism["brand"] = ''
        dMechanism["mechanismId"] = 2

        dt_mechanism.row.add(dMechanism).draw();

        let mechanism = {}
        mechanism.id = 2
        mechanism.mechanism = mechanism2
        mechanism.notes = ''
        mechanism.productId = 0
        mechanism.product = ''
        mechanism.brandId = 0
        mechanism.brand = ''

        arr_mechanism.push(mechanism);


    }

    let mechanism3 = $('#mechanisme3').val()
    if (mechanism3 !== '') {
        dMechanism = []
        dMechanism["no"] = 3
        dMechanism["mechanism"] = mechanism3
        dMechanism["notes"] = ''
        dMechanism["productId"] = 0
        dMechanism["product"] = ''
        dMechanism["brandId"] = 0
        dMechanism["brand"] = ''
        dMechanism["mechanismId"] = 3

        dt_mechanism.row.add(dMechanism).draw();

        let mechanism = {}
        mechanism.id = 3
        mechanism.mechanism = mechanism3
        mechanism.notes = ''
        mechanism.productId = 0
        mechanism.product = ''
        mechanism.brandId = 0
        mechanism.brand = ''

        arr_mechanism.push(mechanism);

    }

    let mechanism4 = $('#mechanisme4').val()
    if (mechanism4 !== '') {
        dMechanism = []
        dMechanism["no"] = 4
        dMechanism["mechanism"] = mechanism4
        dMechanism["notes"] = ''
        dMechanism["productId"] = 0
        dMechanism["product"] = ''
        dMechanism["brandId"] = 0
        dMechanism["brand"] = ''
        dMechanism["mechanismId"] = 4

        dt_mechanism.row.add(dMechanism).draw();

        let mechanism = {}
        mechanism.id = 4
        mechanism.mechanism = mechanism4
        mechanism.notes = ''
        mechanism.productId = 0
        mechanism.product = ''
        mechanism.brandId = 0
        mechanism.brand = ''

        arr_mechanism.push(mechanism);

    }
}

const readMechanismMechanismNew = function() {
    //set mechanism dari form main ke modal
    let dataNodes = dt_mechanism.rows().nodes();
    let rowData = dt_mechanism.rows().data();
    let dbResult = [];
    dt_mechanism_result.clear().draw();
    $.each(dataNodes, function (index) {
        dbResult = [];
        dbResult["productId"] = rowData[index].productId
        dbResult["product"] = rowData[index].product
        dbResult["mechanism"] = rowData[index].mechanism
        dbResult["notes"] = rowData[index].notes
        dbResult["brandId"] = rowData[index].brandId
        dbResult["brand"] = rowData[index].brand
        dbResult["id"] = rowData[index].mechanismId
        dt_mechanism_result.row.add(dbResult).draw();
    });
}

const writeMechanismNew = function() {
    //set mechanism dari modal ke form main
    let dataNodes = dt_mechanism_result.rows().nodes();
    let rowData = dt_mechanism_result.rows().data();

    let cnt = 0;
    let longDescBrand ='';
    let longDescSKU = '';
    dt_mechanism.clear().draw();
    arr_mechanism = [];
    arr_brand = [];
    arr_sku = [];
    $.each(dataNodes, function (index) {
        if (arr_brand.indexOf(rowData[index].brandId) === -1) {
            longDescBrand += '<span class="fw-bold text-sm-left">' + rowData[index].brand + '</span><div class="separator border-2 border-secondary my-2"></div>';
            arr_brand.push(rowData[index].brandId)
        }
        if (arr_sku.indexOf(rowData[index].productId) === -1) {
            longDescSKU += '<span class="fw-bold text-sm-left">' + rowData[index].product + '</span><div class="separator border-2 border-secondary my-2"></div>';
            arr_sku.push(rowData[index].productId)
        }

        // Mechanism Table
        cnt += 1;
        let dMechanism = [];
        dMechanism["no"] = cnt;
        dMechanism["mechanism"] = rowData[index].mechanism;
        dMechanism["notes"] = rowData[index].notes;
        dMechanism["productId"] = rowData[index].productId;
        dMechanism["product"] = rowData[index].product;
        dMechanism["brandId"] = rowData[index].brandId;
        dMechanism["brand"] = rowData[index].brand;
        dMechanism["mechanismId"] = rowData[index].id;

        let mechanism = {
            id : rowData[index].id,
            mechanism : rowData[index].mechanism,
            notes : rowData[index].notes,
            productId : rowData[index].productId,
            product : rowData[index].product,
            brandId : rowData[index].brandId,
            brand : rowData[index].brand,
        };

        arr_mechanism.push(mechanism);

        dt_mechanism.row.add(dMechanism).draw();
    });
    $('#card_list_brand').html(longDescBrand);
    $('#card_list_sku').html(longDescSKU);
}

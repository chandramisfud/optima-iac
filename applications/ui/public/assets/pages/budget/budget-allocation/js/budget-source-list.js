'use strict';

var dt_budget_source_list;
var budget_type = $('#filter_budget_type').val();

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
                data: 'desc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 3,
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
        validator.revalidateField('period');
        validator.revalidateField('budget_source');
        validator.revalidateField('budgetName');
        validator.revalidateField('categoryId');
        validator.resetField('subCategoryId');
        validator.revalidateField('budgetAmount');
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
    let period = ($('#period').val() ?? "");
    let budgetType = ($('#filter_budget_type').val() ?? "");
    let entityId = ($('#filter_entity').val() ?? "");
    let distributorId = ($('#filter_distributor').val() ?? "");
    let url = "/budget/allocation/list/source?period=" + period + "&entityId="+entityId + "&distributorId="+distributorId+"&budgetType="+budgetType;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_budget_source_list.clear().draw();
    dt_budget_source_list.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
        budget_type = budgetType;
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
        url         : "/budget/allocation/list/entity",
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
            url         : "/budget/allocation/list/distributor/entity-id",
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
            url         : "/budget/allocation/get-data/source/id",
            type        : "GET",
            dataType    : 'json',
            data        : {id: id, budgetType: budget_type},
            async       : true,
            success: async function(result) {
                if (!result.error) {
                    if (budget_type === "BTR") {
                        let data = result.data;

                        distributorId = data.budgetAllocation.distributorId;
                        ownerId = data.budgetAssignment.ownId;
                        fromOwnerId = data.budgetAllocation.fromOwnerId;
                        budgetMasterId = data.budgetAllocation.budgetSourceId;
                        budgetAllocationId = data.budgetAllocation.id;
                        budgetSourceId = data.budgetAssignment.id;
                        $('#budgetAssigned').val(data.budgetAssignment.budgetAssigned);
                        $('#entityLongDesc').val(data.budgetAllocation.entity);
                        $('#budgetRemaining').val(data.budgetAssignment.remaining);
                        $("#budget_source").val(data.budgetAllocation.refId);
                        $('#budgetName').val(data.budgetAssignment.desc);
                        $('#budgetMaster').val(data.budgetAllocation.mLongDesc);
                        $('#budgetAmount').val(data.budgetAssignment.remaining);

                        // empty dropdown sub category
                        $('#subCategoryId').empty();

                        // set data category
                        let select_category = [{
                            id: data.budgetAllocation.categoryId,
                            text: data.budgetAllocation.categoryDesc
                        }];
                        let elCategory = $('#categoryId');
                        elCategory.select2({
                            placeholder: "Select a Category",
                            width: '100%',
                            data: select_category
                        });
                        elCategory.val(data.budgetAllocation.categoryId).trigger('change');

                        // set data sub category
                        let select_sub_category = [{
                            id: data.budgetDetail[0].subcategory,
                            text: data.budgetDetail[0].subcategorydesc
                        }];
                        let elSubCategory = $('#subCategoryId');
                        elSubCategory.select2({
                            placeholder: "Select a Sub Category",
                            width: '100%',
                            data: select_sub_category
                        });
                        elSubCategory.val(data.budgetDetail[0].subcategory).trigger('change');

                        // set data activity
                        let select_activity = [{
                            id: data.budgetDetail[0].activity,
                            text: data.budgetDetail[0].activitydesc
                        }];
                        let elActivity = $('#activityId');
                        elActivity.select2({
                            placeholder: "Select an Activity",
                            width: '100%',
                            data: select_activity
                        });
                        // elActivity.val(data.budgetDetail[0].activity).trigger('change');

                        // set data sub activity
                        let select_sub_activity = [{
                            id: data.budgetDetail[0].subactivity,
                            text: data.budgetDetail[0].subactivitydesc
                        }];
                        let elSubActivity = $('#subActivityId');
                        elSubActivity.select2({
                            placeholder: "Select a Sub Activity",
                            width: '100%',
                            data: select_sub_activity
                        });
                        // elSubActivity.val(data.budgetDetail[0].subactivity).trigger('change');

                        // set data region
                        dt_region.clear().draw();
                        dt_region.rows.add(data.regions).draw();
                        if (data.regions.length > 0) {
                            $('#dt_region_wrapper #dt-checkbox-header').trigger('click');
                        }

                        // set data channel
                        list_channels = data.channels

                        // set data sub channel
                        list_sub_channels = data.subChannels;

                        // set data account
                        list_accounts = data.accounts;

                        // set data sub account
                        list_sub_accounts = data.subAccounts;

                        // fill data channel
                        dt_channel.clear().draw();
                        dt_channel.rows.add(list_channels).draw();
                        if (list_channels.length > 0) {
                            // tick all channels using tick header channel
                            let el_header_channels = $('#dt_channel_wrapper #dt-checkbox-header');
                            el_header_channels[0].checked = false;
                            if (!el_header_channels[0].checked) {
                                el_header_channels.trigger('click');
                            }
                        }

                        // set data user
                        dt_user.clear().draw();
                        dt_user.rows.add(data.userAccess).draw();

                        // set data sku
                        dt_sku.clear().draw();
                        list_skus = data.product;

                        // set data brand
                        dt_brand.clear().draw();
                        list_brands = data.brand;

                        // fill data brand
                        dt_brand.rows.add(data.brand).draw();
                        if (list_brands.length > 0) {
                            let el_header_brands = $('#dt_brand_wrapper #dt-checkbox-header');
                            if (!el_header_brands[0].checked) {
                                el_header_brands.trigger('click');
                            } else {
                                el_header_brands.trigger('click');
                                el_header_brands.trigger('click');
                            }
                        }
                    }
                    if (budget_type === "BAL") {
                        let data = result.data;
                        distributorId = data.distributorId;
                        ownerId = data.ownerId;
                        budgetMasterId = data.id;
                        budgetAllocationId = data.id;
                        budgetSourceId = data.id;
                        $('#budgetAssigned').val(data.budgetAmount);
                        $('#entityLongDesc').val(data.principalLongDesc);
                        $('#budgetRemaining').val(data.budgetAmount);
                        $("#budget_source").val(data.refId);
                        $('#budgetName').val(data.longDesc);
                        $('#budgetMaster').val(data.longDesc);
                        $('#budgetAmount').val(data.budgetAmount);

                        // empty dropdown sub category
                        $('#subCategoryId').empty();

                        // set data category
                        let select_category = [{
                            id: data.categoryId,
                            text: data.categoryLongDesc
                        }];
                        let elCategory = $('#categoryId');
                        elCategory.select2({
                            placeholder: "Select a Category",
                            width: '100%',
                            data: select_category
                        });
                        elCategory.val(data.categoryId).trigger('change');

                        // set data sub category
                        await getListSubCategory(data.categoryId);
                        $('#subCategoryId').val('').trigger('change');

                        // set data region
                        dt_region.clear().draw();
                        let list_regions = await getListRegion();
                        dt_region.rows.add(list_regions).draw();
                        if (list_regions.length > 0) {
                            let el_header_regions = $('#dt_region_wrapper #dt-checkbox-header');
                            if (!el_header_regions[0].checked) {
                                el_header_regions.trigger('click');
                            }
                        }

                        // get data channel
                        list_channels = await getListChannel();

                        // get data sub channel
                        list_sub_channels = await getListSubChannel(0);

                        // get data accounts
                        list_accounts = await getListAccount(0);

                        // get data sub accounts
                        list_sub_accounts = await getListSubAccount(0);

                        // set data channel
                        dt_channel.clear().draw();
                        dt_channel.rows.add(list_channels).draw();
                        if (list_channels.length > 0) {
                            // tick all channels using tick header channel
                            let el_header_channels = $('#dt_channel_wrapper #dt-checkbox-header');
                            el_header_channels[0].checked = false;
                            if (!el_header_channels[0].checked) {
                                el_header_channels.trigger('click');
                            }
                        }

                        // set data user
                        dt_user.clear().draw();
                        let list_users = await getListUser();
                        dt_user.rows.add(list_users).draw();

                        // get data brand
                        dt_brand.clear().draw();
                        list_brands = await getListBrand(data.principalId);

                        //get data sku
                        list_skus = await getListSKU(0);

                        // set data brand
                        dt_brand.clear().draw();
                        dt_brand.rows.add(list_brands).draw();
                        if (list_brands.length > 0) {
                            let el_header_brands = $('#dt_brand_wrapper #dt-checkbox-header');
                            if (!el_header_brands[0].checked) {
                                el_header_brands.trigger('click');
                            }
                        }
                    }
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

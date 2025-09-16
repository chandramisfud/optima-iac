'use strict';

var validator, method, id;
var distributorId, ownerId, fromOwnerId, budgetMasterId, budgetAllocationId, budgetSourceId;
var swalTitle = "Budget Allocation";
var list_channels = [], list_sub_channels = [], list_accounts = [], list_sub_accounts = [], list_brands = [], list_skus = [];
var load_data = true;

var categoryId, subCategoryId, activityId, subActivityId;

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    let dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });
    dialerObject.setValue(new Date().getFullYear());

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#budgetAssigned, #budgetRemaining, #budgetAmount");

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    id = url_str.searchParams.get("id");

    if (method === 'update') {
        $('#btn_search_budget_source').remove();
        $('#dialer_period').html('<input type="text" class="form-control form-control-sm form-control-solid-bg" name="period" id="period" autocomplete="off" readonly/>');
        $('#fieldCategory').html('<input class="form-control form-control-sm form-control-solid-bg" type="text" name="categoryName" id="categoryName" autocomplete="off" readonly/>');
        $('#fieldSubCategory').html('<input class="form-control form-control-sm form-control-solid-bg" type="text" name="subCategoryName" id="subCategoryName" autocomplete="off" readonly/>');
        $('#fieldActivity').html('<input class="form-control form-control-sm form-control-solid-bg" type="text" name="activityName" id="activityName" autocomplete="off" readonly/>');
        $('#fieldSubActivity').html('<input class="form-control form-control-sm form-control-solid-bg" type="text" name="subActivityName" id="subActivityName" autocomplete="off" readonly/>');
        $('#budgetName').addClass('form-control-solid-bg').prop('readonly', true);
        blockUI.block();
        disableButtonSave();
        Promise.all([ getData(id) ]).then(async () => {
            $('#txt_info_method').text('Edit');
            enableButtonSave();
            blockUI.release();
        });
    }

    validator = FormValidation.formValidation(document.getElementById('form_budget_allocation'), {
        fields: {
            period: {
                validators: {
                    notEmpty: {
                        message: "This field is required"
                    },
                    greaterThan: {
                        min: 1900,
                        message: "Budget year is not valid"
                    }
                }
            },
            budget_source: {
                validators: {
                    notEmpty: {
                        message: "Select a budget source"
                    },
                }
            },
            budgetName: {
                validators: {
                    notEmpty: {
                        message: "This field is required"
                    },
                }
            },
            categoryId: {
                validators: {
                    notEmpty: {
                        message: "This field is required"
                    },
                }
            },
            subCategoryId: {
                validators: {
                    notEmpty: {
                        message: "This field is required"
                    },
                }
            },
            budgetAmount: {
                validators: {
                    notEmpty: {
                        message: "This field is required"
                    },
                    greaterThan: {
                        min: 1,
                        message: "Budget amount must be greater than 0"
                    }
                },
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });
});

$('#btn_search_budget_source').on('click', function () {
    $('#dt_budget_source_list_search').val('');
    let period = $('#period').val();
    let budgetType = $('#filter_budget_type').val();
    let entityId = $('#filter_entity').val();
    let distributorId = $('#filter_distributor').val();
    dt_budget_source_list.clear().draw();
    let url = "/budget/allocation/list/source?period=" + period + "&entityId="+entityId + "&distributorId="+distributorId+"&budgetType="+budgetType;
    dt_budget_source_list.ajax.url(url).load();
    $('#modal_list_budget_source').modal('show');
});

$('#subCategoryId').on('change', async function () {
    if (!blockUI.blocked) blockUI.block();
    $('#activityId').empty();
    if ($(this).val() !== "") await getListActivity($(this).val());
    if (blockUI.blocked) blockUI.release();
    $('#activityId').val('').trigger('change');
});

$('#activityId').on('change', async function () {
    if (!blockUI.blocked) blockUI.block();
    $('#subActivityId').empty();
    if ($(this).val() !== "") await getListSubActivity($(this).val());
    if (blockUI.blocked) blockUI.release();
    $('#subActivityId').val('').trigger('change');
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let checked_regions = dt_region.column(0).checkboxes.selected();
            if (checked_regions.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please tick Region at least one",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    let tabTriggerEl = document.querySelector('#tabRegion');
                    let tab = new bootstrap.Tab(tabTriggerEl);
                    tab.show();
                });
            }
            let checked_channels = dt_channel.column(0).checkboxes.selected();
            if (checked_channels.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please tick Channel at least one",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    let tabTriggerEl = document.querySelector('#tabChannel');
                    let tab = new bootstrap.Tab(tabTriggerEl);
                    tab.show();
                });
            }
            let checked_sub_channels = dt_subchannel.column(0).checkboxes.selected();
            if (checked_sub_channels.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please tick Sub Channel at least one",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    let tabTriggerEl = document.querySelector('#tabSubChannel');
                    let tab = new bootstrap.Tab(tabTriggerEl);
                    tab.show();
                });
            }
            let checked_accounts = dt_account.column(0).checkboxes.selected();
            if (checked_accounts.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please tick Account at least one",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    let tabTriggerEl = document.querySelector('#tabAccount');
                    let tab = new bootstrap.Tab(tabTriggerEl);
                    tab.show();
                });
            }
            let checked_sub_accounts = dt_subaccount.column(0).checkboxes.selected();
            if (checked_sub_accounts.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please tick Sub Account at least one",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    let tabTriggerEl = document.querySelector('#tabSubAccount');
                    let tab = new bootstrap.Tab(tabTriggerEl);
                    tab.show();
                });
            }
            let checked_users = dt_user.column(0).checkboxes.selected();
            if (checked_users.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please tick User at least one",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    let tabTriggerEl = document.querySelector('#tabUser');
                    let tab = new bootstrap.Tab(tabTriggerEl);
                    tab.show();
                });
            }
            let checked_brands = dt_brand.column(0).checkboxes.selected();
            if (checked_brands.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please tick Brand at least one",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    let tabTriggerEl = document.querySelector('#tabBrand');
                    let tab = new bootstrap.Tab(tabTriggerEl);
                    tab.show();
                });
            }
            let checked_skus = dt_sku.column(0).checkboxes.selected();
            if (checked_skus.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please tick SKU at least one",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    let tabTriggerEl = document.querySelector('#tabSku');
                    let tab = new bootstrap.Tab(tabTriggerEl);
                    tab.show();
                });
            }
            let dataRowsRegion = [];
            $.each(checked_regions, function (index, value) {
                dataRowsRegion.push({
                    id: value
                });
            });
            let dataRowsChannel = [];
            $.each(checked_channels, function (index, value) {
                dataRowsChannel.push({
                    id: value
                });
            });
            let dataRowsSubChannel = [];
            $.each(checked_sub_channels, function (index, value) {
                dataRowsSubChannel.push({
                    id: value
                });
            });
            let dataRowsAccount = [];
            $.each(checked_accounts, function (index, value) {
                dataRowsAccount.push({
                    id: value
                });
            });
            let dataRowsSubAccount = [];
            $.each(checked_sub_accounts, function (index, value) {
                dataRowsSubAccount.push({
                    id: value
                });
            });
            let dataRowsUser = [];
            $.each(checked_users, function (index, value) {
                dataRowsUser.push({
                    id: value
                });
            });
            let dataRowsBrand = [];
            $.each(checked_brands, function (index, value) {
                dataRowsBrand.push({
                    id: value
                });
            });
            let dataRowsSKU = [];
            $.each(checked_skus, function (index, value) {
                dataRowsSKU.push({
                    id: value
                });
            });
            let formData = new FormData($('#form_budget_allocation')[0]);
            formData.append('budgetType', budget_type);
            formData.append('distributorId', distributorId);
            formData.append('ownerId', ownerId);
            formData.append('budgetMasterId', budgetMasterId);
            formData.append('budgetAllocationId', budgetAllocationId);
            formData.append('budgetSourceId', budgetSourceId);
            formData.append('regions', JSON.stringify(dataRowsRegion));
            formData.append('channels', JSON.stringify(dataRowsChannel));
            formData.append('subChannels', JSON.stringify(dataRowsSubChannel));
            formData.append('accounts', JSON.stringify(dataRowsAccount));
            formData.append('subAccounts', JSON.stringify(dataRowsSubAccount));
            formData.append('userAccess', JSON.stringify(dataRowsUser));
            formData.append('brands', JSON.stringify(dataRowsBrand));
            formData.append('skus', JSON.stringify(dataRowsSKU));
            let url = "/budget/allocation/save";
            if (method === "update") {
                url = "/budget/allocation/update";
                formData.append('id', id);
                formData.append('categoryId', categoryId);
                formData.append('subCategoryId', subCategoryId);
                formData.append('activityId', activityId);
                formData.append('subActivityId', subActivityId);
            }
            $.ajax({
                url         : url,
                data        : formData,
                type        : 'POST',
                async       : true,
                dataType    : 'JSON',
                cache       : false,
                contentType : false,
                processData : false,
                beforeSend: function() {
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                },
                success: function(result, status, xhr, $form) {
                    if (!result.error) {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.location.href = '/budget/allocation';
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "error",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                },
                complete: function() {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.message)
                    Swal.fire({
                        title: swalTitle,
                        text: "Failed to save data, an error occurred in the process",
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        } else {
            let tabTriggerEl = document.querySelector('#tabDetail');
            let tab = new bootstrap.Tab(tabTriggerEl);
            tab.show();
        }
    });
});

const getListSubCategory = (categoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/sub-category",
            type        : "GET",
            dataType    : 'json',
            data        : {categoryId: categoryId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#subCategoryId').select2({
                    placeholder: "Select a Sub Category",
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

const getListActivity = (subCategoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/activity",
            type        : "GET",
            dataType    : 'json',
            data        : {subCategoryId: subCategoryId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#activityId').select2({
                    placeholder: "Select an Activity",
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

const getListSubActivity = (activityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/list/sub-activity",
            type        : "GET",
            dataType    : 'json',
            data        : {activityId: activityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#subActivityId').select2({
                    placeholder: "Select a Sub Activity",
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

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/allocation/get-data/id",
            type        : "GET",
            dataType    : 'json',
            data        : {id: id},
            async       : true,
            success: async function(result) {
                if (!result.error) {
                    let data = result.data;
                    if (data.budgetAllocation.budgetType === "BAL") {
                        budgetSourceId = data.budgetAllocation.budgetType;
                        distributorId = data.budgetAllocation.distributorId;
                        ownerId = data.budgetAllocation.ownerId;
                        budgetMasterId = data.budgetAllocation.budgetMasterId;
                        budgetAllocationId = data.budgetAllocation.id;
                        budgetSourceId = data.budgetAllocation.budgetSourceId;
                        $('#period').val(data.budgetAllocation.periode);
                        $('#budgetAssigned').val(data.budgetAllocation.budgetAmount);
                        $('#entityLongDesc').val(data.budgetAllocation.entity);
                        $('#budgetRemaining').val(data.budgetAllocation.remainingBudget);
                        $("#budget_source").val(data.budgetAllocation.mRefId);
                        $('#budgetName').val(data.budgetAllocation.longDesc);
                        $('#budgetMaster').val(data.budgetAllocation.mLongDesc);

                        // fill data detail
                        categoryId = data.budgetAllocation.categoryId;
                        subCategoryId = data.budgetDetail[0].subcategory;
                        activityId = data.budgetDetail[0].activity;
                        subActivityId = data.budgetDetail[0].subactivity;
                        $('#categoryName').val(data.budgetAllocation.categoryDesc);
                        $('#subCategoryName').val(data.budgetDetail[0].subcategorydesc);
                        $('#activityName').val(data.budgetDetail[0].activitydesc);
                        $('#subActivityName').val(data.budgetDetail[0].subactivitydesc);
                        $('#budgetAmount').val(data.budgetDetail[0].budgetAmount);

                        // set data region
                        dt_region.clear().draw();
                        let list_regions = data.regions;
                        dt_region.rows.add(list_regions).draw();

                        // set data sub account
                        dt_subaccount.clear().draw();
                        list_sub_accounts = data.subAccounts;

                        // set data account
                        dt_account.clear().draw();
                        list_accounts = data.accounts;

                        // set data sub channel
                        dt_subchannel.clear().draw();
                        list_sub_channels = data.subChannels;

                        // set data channel
                        dt_channel.clear().draw();
                        list_channels = data.channels;
                        dt_channel.rows.add(list_channels).draw();

                        // fill data sub account
                        let sub_accounts = [];
                        for (let i=0; i<list_sub_accounts.length; i++) {
                            if (list_sub_accounts[i].flag) sub_accounts.push(list_sub_accounts[i])
                        }
                        dt_subaccount.rows.add(sub_accounts).draw();

                        // disable checkbox header sub account
                        $('#dt_subaccount_wrapper #dt-checkbox-header').prop('disabled', true);

                        // disable checkbox rows sub account
                        let el_rows_sub_accounts = $('#dt_subaccount_wrapper .dt-checkboxes');
                        for (let i=0; i<el_rows_sub_accounts.length; i++) {
                            $(el_rows_sub_accounts[i]).prop('disabled', true);
                        }

                        // fill data account
                        let accounts = [];
                        for (let i=0; i<list_accounts.length; i++) {
                            if (list_accounts[i].flag) accounts.push(list_accounts[i])
                        }
                        dt_account.rows.add(accounts).draw();

                        // fill data sub channel
                        let sub_channels = [];
                        for (let i=0; i<list_sub_channels.length; i++) {
                            if (list_sub_channels[i].flag) sub_channels.push(list_sub_channels[i])
                        }
                        dt_subchannel.rows.add(sub_channels).draw();

                        // set data user
                        dt_user.clear().draw();
                        let list_user = data.userAccess;
                        dt_user.rows.add(list_user).draw();

                        // set data brand
                        dt_brand.clear().draw();
                        let list_brand = data.brand;
                        dt_brand.rows.add(list_brand).draw();

                        // set data sku
                        dt_sku.clear().draw();
                        list_skus = data.product;
                        let data_skus = [];
                        for (let i=0; i<list_skus.length; i++) {
                            if (list_skus[i].flag) data_skus.push(list_skus[i])
                        }
                        dt_sku.rows.add(data_skus).draw();
                    } else if (data.budgetAllocation.budgetType === "BTR") {
                        budgetSourceId = data.budgetAllocation.budgetType;
                        distributorId = data.budgetAllocation.distributorId;
                        ownerId = data.budgetAllocation.ownerId;
                        budgetMasterId = data.budgetAllocation.budgetMasterId;
                        budgetAllocationId = data.budgetAllocation.id;
                        budgetSourceId = data.budgetAllocation.budgetSourceId;
                        $('#period').val(data.budgetAllocation.periode);
                        $('#budgetAssigned').val(data.budgetAllocation.budgetAmount);
                        $('#entityLongDesc').val(data.budgetAllocation.entity);
                        $('#budgetRemaining').val(data.budgetAllocation.remainingBudget);
                        $("#budget_source").val(data.budgetAllocation.mRefId);
                        $('#budgetName').val(data.budgetAllocation.longDesc);
                        $('#budgetMaster').val(data.budgetAllocation.mLongDesc);

                        // fill data detail
                        categoryId = data.budgetAllocation.categoryId;
                        subCategoryId = data.budgetDetail[0].subcategory;
                        activityId = data.budgetDetail[0].activity;
                        subActivityId = data.budgetDetail[0].subactivity;
                        $('#categoryName').val(data.budgetAllocation.categoryDesc);
                        $('#subCategoryName').val(data.budgetDetail[0].subcategorydesc);
                        $('#activityName').val(data.budgetDetail[0].activitydesc);
                        $('#subActivityName').val(data.budgetDetail[0].subactivitydesc);
                        $('#budgetAmount').val(data.budgetDetail[0].budgetAmount);

                        // set data region
                        dt_region.clear().draw();
                        let list_regions = data.regions;
                        dt_region.rows.add(list_regions).draw();

                        // set data sub account
                        dt_subaccount.clear().draw();
                        list_sub_accounts = data.subAccounts;

                        // set data account
                        dt_account.clear().draw();
                        list_accounts = data.accounts;

                        // set data sub channel
                        dt_subchannel.clear().draw();
                        list_sub_channels = data.subChannels;

                        // set data channel
                        dt_channel.clear().draw();
                        list_channels = data.channels;
                        dt_channel.rows.add(list_channels).draw();

                        // fill data sub account
                        let sub_accounts = [];
                        for (let i=0; i<list_sub_accounts.length; i++) {
                            if (list_sub_accounts[i].flag) sub_accounts.push(list_sub_accounts[i])
                        }
                        dt_subaccount.rows.add(sub_accounts).draw();

                        // disable checkbox header sub account
                        $('#dt_subaccount_wrapper #dt-checkbox-header').prop('disabled', true);

                        // disable checkbox rows sub account
                        let el_rows_sub_accounts = $('#dt_subaccount_wrapper .dt-checkboxes');
                        for (let i=0; i<el_rows_sub_accounts.length; i++) {
                            $(el_rows_sub_accounts[i]).prop('disabled', true);
                        }

                        // fill data account
                        let accounts = [];
                        for (let i=0; i<list_accounts.length; i++) {
                            if (list_accounts[i].flag) accounts.push(list_accounts[i])
                        }
                        dt_account.rows.add(accounts).draw();

                        // fill data sub channel
                        let sub_channels = [];
                        for (let i=0; i<list_sub_channels.length; i++) {
                            if (list_sub_channels[i].flag) sub_channels.push(list_sub_channels[i])
                        }
                        dt_subchannel.rows.add(sub_channels).draw();

                        // set data user
                        dt_user.clear().draw();
                        let list_user = data.userAccess;
                        dt_user.rows.add(list_user).draw();

                        // set data brand
                        dt_brand.clear().draw();
                        let list_brand = data.brand;
                        dt_brand.rows.add(list_brand).draw();

                        // set data sku
                        dt_sku.clear().draw();
                        list_skus = data.product;
                        let data_skus = [];
                        for (let i=0; i<list_skus.length; i++) {
                            if (list_skus[i].flag) data_skus.push(list_skus[i])
                        }
                        dt_sku.rows.add(data_skus).draw();
                    }

                    load_data = false;
                }
                resolve();
            },
            complete: function() {

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

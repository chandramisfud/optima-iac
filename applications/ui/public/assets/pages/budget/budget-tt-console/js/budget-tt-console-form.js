'use strict';

let validator, method, dialerObject, budgetTTConsoleId, distributorShortDesc='', budgetName= '', periodText='2025', brandText= '', subAccountText='', activityText='';
let channelId, subChannelId, accountId, subAccountId, distributorId, groupBrandId, subActivityTypeId, categoryId, subCategoryId, activityId, subActivityId;
let channel, subChannel, account, subAccount, distributor, groupBrand, subActivityType, subCategory, activity, subActivity;
let elPeriod = $('#period');
let swalTitle = "Budget TT Consol";

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    budgetTTConsoleId = url_str.searchParams.get("i");
    categoryId = url_str.searchParams.get("ic");

    blockUI.block();
    disableButtonSave();
    if (method === 'update') {
        $('#dialer_period').html('<input type="text" class="form-control form-control-sm form-control-solid-bg" name="period" id="period" autocomplete="off" readonly/>');
        $('#channel').attr('readonly', true);
        $('#sub_channel').attr('readonly', true);
        $('#account').attr('readonly', true);
        $('#sub_account').attr('readonly', true);
        $('#distributor').attr('readonly', true);
        $('#groupbrand').attr('readonly', true);
        $('#sub_activity_type').attr('readonly', true);
        $('#sub_category').attr('readonly', true);
        $('#activity').attr('readonly', true);
        $('#sub_activity').attr('readonly', true);
        Promise.all([ getListChannel(), getListDistributor(), getListGroupBrand() ]).then(async () => {
            await getData(budgetTTConsoleId);

            await getListSubChannel(channelId);
            $('#sub_channel').val(subChannelId).trigger('change.select2');

            await getListAccount(subChannelId);
            $('#account').val(accountId).trigger('change.select2');

            await getListSubAccount(accountId);
            $('#sub_account').val(subAccountId).trigger('change.select2');

            await getListSubActivityType(categoryId);
            $('#sub_activity_type').val(subActivityTypeId).trigger('change.select2');

            await getListSubCategory(categoryId, subActivityTypeId);
            $('#sub_category').val(subCategoryId).trigger('change.select2');

            await getListActivity(categoryId, subCategoryId, subActivityTypeId);
            let elActivity = $('#activity');
            elActivity.val(activityId).trigger('change.select2');

            activityText = elActivity.select2('data')[0].text;

            await getListSubActivity(categoryId, activityId, subActivityTypeId);
            $('#sub_activity').val(subActivityId).trigger('change.select2');

            budgetName = periodText + ' ' + brandText + ' - ' + distributorShortDesc + ' ' + subAccountText + ' ' + activityText;
            $('#budget_name').val(budgetName);

            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ getListSubActivityType(categoryId), getListChannel(), getListAccount(), getListDistributor(), getListGroupBrand() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#ttPercent");

    const form = document.getElementById('form_budget_tt_console');

    validator = FormValidation.formValidation(form, {
        fields: {
            channel: {
                validators: {
                    notEmpty: {
                        message: "Channel must be enter"
                    },
                }
            },
            sub_channel: {
                validators: {
                    notEmpty: {
                        message: "Sub Channel must be enter"
                    },
                }
            },
            account: {
                validators: {
                    notEmpty: {
                        message: "Account must be enter"
                    },
                }
            },
            sub_account: {
                validators: {
                    notEmpty: {
                        message: "Sub Account must be enter"
                    },
                }
            },
            distributor: {
                validators: {
                    notEmpty: {
                        message: "Distributor must be enter"
                    },
                }
            },
            groupbrand: {
                validators: {
                    notEmpty: {
                        message: "Brand must be enter"
                    },
                }
            },
            sub_activity_type: {
                validators: {
                    notEmpty: {
                        message: "Sub Activity Type must be enter"
                    },
                }
            },
            activity: {
                validators: {
                    notEmpty: {
                        message: "Activity must be enter"
                    },
                }
            },
            sub_category: {
                validators: {
                    notEmpty: {
                        message: "Subcategory must be enter"
                    },
                }
            },
            ttPercent: {
                validators: {
                    notEmpty: {
                        message: "TT % must be enter"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })

    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        min: 2025,
        step: 1,
    });
});

elPeriod.on('blur', function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2025) {
        $(this).val("2025");
        dialerObject.setValue(2025);
    }
});

elPeriod.on('change', function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2025) {
        periodText = $(this).val('2025');
    } else {
        periodText = $(this).val();
    }
    budgetName = periodText + ' ' + brandText + ' - ' + distributorShortDesc + ' ' + subAccountText + ' ' + activityText + ' - ';
    $('#budget_name').val(budgetName);
});

$('#channel').on('change', async function () {
    let elSubChannel = $('#sub_channel');
    elSubChannel.empty();
    if ($(this).val() !== null) await getListSubChannel($(this).val());
    elSubChannel.val('').trigger('change');
});

$('#sub_channel').on('change', async function () {
    let elAccount = $('#account');
    elAccount.empty();
    if ($(this).val() !== null) await getListAccount($(this).val());
    elAccount.val('').trigger('change');
});

$('#account').on('change', async function () {
    let elSubAccount = $('#sub_account');
    elSubAccount.empty();
    if ($(this).val() !== null) await getListSubAccount($(this).val());
    elSubAccount.val('').trigger('change');
});

$('#sub_account').on('change', async function () {
    if ($(this).val() !== null) {
        subAccountText = $(this).select2('data')[0].text;

        budgetName = periodText + ' ' + brandText + ' - ' + distributorShortDesc + ' - ' + subAccountText + ' ' + activityText;
        $('#budget_name').val(budgetName);
    }
});

$('#distributor').on('change', async function () {
    if ($(this).val() !== null) {
        distributorShortDesc = $(this).select2('data')[0].shortDesc;

        budgetName = periodText + ' ' + brandText + ' - ' + distributorShortDesc + ' - ' + subAccountText + ' ' + activityText;
        $('#budget_name').val(budgetName);
    }
});

$('#groupbrand').on('change', async function () {
    if ($(this).val() !== null) {
        brandText = $(this).select2('data')[0].text;

        budgetName = periodText + ' ' + brandText + ' - ' + distributorShortDesc + ' - ' + subAccountText + ' ' + activityText ;
        $('#budget_name').val(budgetName);
    }
});

$('#sub_activity_type').on('change', async function () {
    let elSubCategory = $('#sub_category');
    elSubCategory.empty();
    $('#activity').empty();
    $('#sub_activity').empty();
    if ($(this).val() !== null) {
        await getListSubCategory(categoryId, $(this).val());
        elSubCategory.val('').trigger('change');

        activityText = '';
        budgetName = periodText + ' ' + brandText + ' - ' + distributorShortDesc + ' - ' + subAccountText + ' ' + activityText;
        $('#budget_name').val(budgetName);
    }
});

$('#sub_category').on('change', async function () {
    let elActivity = $('#activity');
    elActivity.empty();

    if ($(this).val() !== null) {
        await getListActivity(categoryId, $(this).val(), $('#sub_activity_type').val());
        elActivity.val('').trigger('change');

        activityText = '';
        budgetName = periodText + ' ' + brandText + ' - ' + distributorShortDesc + ' - ' + subAccountText + ' ' + activityText;
        $('#budget_name').val(budgetName);
    }
});

$('#activity').on('change', async function () {
    $('#sub_activity').empty();
    if ($(this).val() !== null) {
        activityText = $(this).select2('data')[0].text;
        Promise.all([ getListSubActivity(categoryId, $(this).val(), $('#sub_activity_type').val()) ]).then(async () => {
            $('#sub_activity').val('').trigger('change');
            budgetName = periodText + ' ' + brandText + ' - ' + distributorShortDesc + ' - ' + subAccountText + ' ' + activityText;
            $('#budget_name').val(budgetName);
        });
    }
});

$('#btn_save_exit').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save_action");
            save(true, e);
        }
    });
});

$('#btn_save_add').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save_action");
            save(false, e);
        }
    });
})

$('#btn_save').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save");
            save(false, e);
        }
    });
});

const save = (exit, e) => {
    let formData = new FormData($('#form_budget_tt_console')[0]);
    let url = '/budget/tt-console/save';
    if (method === "update") {
        formData.append('id', budgetTTConsoleId);
        url = '/budget/tt-console/update';
    }
    formData.append('category', categoryId);
    formData.append('distributorShortDesc', distributorShortDesc);

    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    blockUI.block();
    $.get('/refresh-csrf').done(function(data) {
        let elMeta = $('meta[name="csrf-token"]');
        elMeta.attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': elMeta.attr('content')
            }
        });
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
            },
            success: async function(result) {
                if (!result.error) {
                    if (method === 'update') {
                        let promoList = result['data'];
                        if (promoList.length > 0) {
                            let elProgress = $('#d_progress');
                            let elInfoSendingProgress = $('#info_sending_progress');
                            let perc = 0;
                            elProgress.removeClass('d-none');
                            for (let i = 1; i <= promoList.length; i++) {
                                let userApprover = promoList[i-1]['approver'];
                                let nameApprover = promoList[i-1]['userNameApprover'];
                                let promoId = promoList[i-1]['promoId'];
                                let emailApprover = promoList[i-1]['emailApprover'];

                                let subject;
                                if(promoList[i-1]['reconciled']){
                                    subject = `[APPROVAL NOTIF] Promo Reconciliation requires approval (${promoList[i-1]['promoRefId']})`;
                                    elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Sending email: [APPROVAL NOTIF] Promo Reconciliation requires approval (${promoList[i-1]['promoRefId']})</span>`);
                                    await sendEmailRecon(userApprover, nameApprover, promoId, emailApprover, subject);
                                } else {
                                    subject = `[APPROVAL NOTIF] Promo requires approval (${promoList[i-1]['promoRefId']})`;
                                    elInfoSendingProgress.html(`<span class="text-gray-800 info_sending_email">Sending email: [APPROVAL NOTIF] Promo requires approval (${promoList[i-1]['promoRefId']})</span>`);
                                    await sendEmail(userApprover, nameApprover, promoId, emailApprover, subject);
                                }

                                perc = ((i / promoList.length) * 100).toFixed(0);
                                $('#text_progress').text(perc.toString() + '%');
                                let progress_import = $('#progress_bar');
                                progress_import.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());
                                if (i === promoList.length) {
                                    Swal.fire({
                                        title: swalTitle,
                                        text: result.message,
                                        icon: "success",
                                        confirmButtonText: "OK",
                                        customClass: {confirmButton: "btn btn-optima"}
                                    }).then(function () {
                                        progress_import.css('width', '0%').attr('aria-valuenow', '0');
                                        elProgress.addClass('d-none');
                                        e.setAttribute("data-kt-indicator", "off");
                                        e.disabled = !1;
                                        blockUI.release();
                                        window.location.href = '/budget/tt-console';
                                    });
                                }
                            }
                        } else {
                            Swal.fire({
                                title: swalTitle,
                                text: result.message,
                                icon: "success",
                                confirmButtonText: "OK",
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(function () {
                                window.location.href = '/budget/tt-console';
                                e.setAttribute("data-kt-indicator", "off");
                                e.disabled = !1;
                                blockUI.release();
                            });
                        }
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "success",
                            confirmButtonText: "OK",
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            if (exit) {
                                window.location.href = '/budget/tt-console';
                            } else {
                                formReset();
                            }
                            e.setAttribute("data-kt-indicator", "off");
                            e.disabled = !1;
                            blockUI.release();
                        });
                    }
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                        blockUI.release();
                    });
                }
            },
            error: function (jqXHR) {
                console.log(jqXHR.message)
                Swal.fire({
                    title: swalTitle,
                    text: "Failed to save data, an error occurred in the process",
                    icon: "error",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    blockUI.release();
                });
            }
        });
    });
}

const getData = (budgetTTConsoleId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/budget/tt-console/data/id",
            type: "GET",
            data: {id:budgetTTConsoleId},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    if (result.data.length > 0){
                        $('#txt_info_method').text('Edit')
                        let values = result['data'][0];

                        channelId = values['channelId'];
                        channel = values['channel'];
                        subChannelId = values['subChannelId'];
                        subChannel = values['subChannel'];
                        accountId = values['accountId'];
                        account = values['account'];
                        subAccountId = values['subAccountId'];
                        subAccount = values['subAccount'];
                        distributorId = values['distributorId'];
                        distributor = values['distributor'];
                        groupBrandId = values['groupBrandId'];
                        groupBrand = values['groupBrand'];
                        subActivityTypeId = values['subActivityTypeId'];
                        subActivityType =  values['subActivityType'];
                        categoryId = values['categoryId'];
                        subCategoryId = values['subCategoryId'];
                        subCategory = values['subCategory'];
                        activityId = values['activityId'];
                        activity = values['activity'];
                        subActivityId = values['subActivityId'];
                        subActivity = values['subActivity'];

                        budgetName = values['budgetName'];
                        periodText = values['period'];
                        brandText = values['groupBrand'];
                        distributorShortDesc = values['distributorShortDesc'];
                        subAccountText = values['subAccount'];
                        activityText = values['activity'];

                        $('#period').val(values['period']);

                        $('#category').val(values['categoryId']).trigger('change.select2');
                        $('#channel').val(values['channelId']).trigger('change.select2');

                        $('#distributor').val(values['distributorId']).trigger('change.select2');
                        $('#groupbrand').val(values['groupBrandId']).trigger('change.select2');

                        $('#ttPercent').val(values['tt']);
                        $('#budget_name').val(budgetName);
                    }
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete:function(){
                return resolve();
            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/channel",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (channelId !== result.data[j].id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: channelId,
                            text: channel
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#channel').select2({
                    placeholder: "Select a Channel",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListSubChannel = (channelId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/sub-channel/channel-id",
            type        : "GET",
            dataType    : 'json',
            data        : {channelId: channelId},
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (subChannelId !== result.data[j].id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: subChannelId,
                            text: subChannel
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#sub_channel').select2({
                    placeholder: "Select a Sub Channel",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListAccount = (subChannelId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/account/sub-channel-id",
            type        : "GET",
            dataType    : 'json',
            data        : {subChannelId: subChannelId},
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (accountId !== result.data[j].id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: accountId,
                            text: account
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#account').select2({
                    placeholder: "Select an Account",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListSubAccount = (accountId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/sub-account/account-id",
            type        : "GET",
            dataType    : 'json',
            data        : {accountId: accountId},
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (subAccountId !== result.data[j].id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: subAccountId,
                            text: subAccount
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#sub_account').select2({
                    placeholder: "Select a Sub Account",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListDistributor = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/distributor",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (distributorId !== result.data[j].id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: distributorId,
                            text: distributor,
                            shortDesc: distributorShortDesc
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc,
                        shortDesc: result.data[j].shortDesc
                    });
                }
                $('#distributor').select2({
                    placeholder: "Select a Distributor",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListGroupBrand = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/groupBrand",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (groupBrandId !== result.data[j].Id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: groupBrandId,
                            text: groupBrand
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j]['Id'],
                        text: result.data[j]['LongDesc']
                    });
                }
                $('#groupbrand').select2({
                    placeholder: "Select a Brand",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListSubActivityType = (categoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/sub-activity-type/category-id",
            type        : "GET",
            dataType    : 'json',
            data        : {categoryId: categoryId},
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (subActivityTypeId !== result.data[j].id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: subActivityTypeId,
                            text: subActivityType
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#sub_activity_type').select2({
                    placeholder: "Select a Sub Activity Type",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListActivity = (categoryId, subCategoryId, subActivityTypeId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/activity/category-id",
            type        : "GET",
            dataType    : 'json',
            data        : {categoryId: categoryId, subCategoryId: subCategoryId, subActivityTypeId: subActivityTypeId},
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (activityId !== result.data[j].id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: activityId,
                            text: activity
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#activity').select2({
                    placeholder: "Select an Activity",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListSubActivity = (categoryId, activityId, subActivityTypeId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/sub-activity/category-id-activity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {categoryId: categoryId, activityId: activityId, subActivityTypeId: subActivityTypeId},
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (subActivityId !== result.data[j].id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: subActivityId,
                            text: subActivity
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#sub_activity').select2({
                    placeholder: "Select a Sub Activity",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const getListSubCategory = (categoryId, subActivityTypeId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/tt-console/list/sub-category/category-id",
            type        : "GET",
            dataType    : 'json',
            data        : {categoryId: categoryId, subActivityTypeId: subActivityTypeId},
            async       : true,
            success: function(result) {
                let data = [];
                if (method === 'update') {
                    let exist = true;
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (subCategoryId !== result.data[j].id) {
                            exist = false
                        }
                    }
                    if (!exist) {
                        data = [{
                            id: subCategoryId,
                            text: subCategory
                        }];
                    }
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#sub_category').select2({
                    placeholder: "Select a Sub Category",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
}

const sendEmail = (userApprover, nameApprover, promoId, emailApprover, subject) => {
    return new Promise((resolve, reject) => {
        let formData = new FormData();

        formData.append('userApprover', userApprover);
        formData.append('nameApprover', nameApprover);
        formData.append('promoId', promoId);
        formData.append('emailApprover', emailApprover);
        formData.append('subject', subject);

        $.ajax({
            url             : "/budget/tt-console/send-email",
            data            : formData,
            type            : 'POST',
            async           : true,
            dataType        : 'JSON',
            cache           : false,
            contentType     : false,
            processData     : false,
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const sendEmailRecon = (userApprover, nameApprover, promoId, emailApprover, subject) => {
    return new Promise((resolve, reject) => {
        let formData = new FormData();

        formData.append('userApprover', userApprover);
        formData.append('nameApprover', nameApprover);
        formData.append('promoId', promoId);
        formData.append('emailApprover', emailApprover);
        formData.append('subject', subject);

        $.ajax({
            url             : "/budget/tt-console/recon/send-email",
            data            : formData,
            type            : 'POST',
            async           : true,
            dataType        : 'JSON',
            cache           : false,
            contentType     : false,
            processData     : false,
            complete: function() {
                return resolve();
            },
            error: function (jqXHR)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const formReset = () => {
    $('#ttPercent').val('');
}

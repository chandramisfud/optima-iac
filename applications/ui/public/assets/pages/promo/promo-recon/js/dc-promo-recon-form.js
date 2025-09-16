'use strict';

let method, validator, categoryShortDescEnc;
let promoId, promoRefId, promoPlanningId, allocationId;
let statusApprovalCode, isCancel, isClose, tsCoding, createOn, isCancelLocked;
let entityId, distributorId, categoryId, categoryShortDesc, subCategoryId, activityId, subActivityId, groupBrandId;
let listChannel = [], listSubChannel = [], listAccount = [], listSubAccount = [], listBrand = [], arr_mechanism = [], listSKU = [], listRegion = [];
let uuid, strActivityName, entityShortDesc;
let startDatePromo, endDatePromo;
let strChannel;
let promoConfigItem, configStartPromo, configEndPromo;
let subCategoryDesc, subActivityLongDesc, activityLongDesc;

let swalTitle = "Promo Reconciliation";

let targetModal = document.querySelector(".modal-content");
let blockUIModal = new KTBlockUI(targetModal, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

const form = document.getElementById('form_promo');

const crossYear = function () {
    return {
        validate: function (input) {
            const yearPeriod = $('#period').val()
            const value = input.value;
            const inputYear = new Date(value).getFullYear().toString();

            if (yearPeriod === inputYear) {
                return {
                    valid: true
                }
            } else {
                return {
                    valid: false,
                    message: 'Cross year is not allowed'
                }
            }
        }
    };
};

const investmentMin = function () {
    return {
        validate: function (input) {
            const value = parseFloat(input.value.replace(/,/g, ''));
            if (value >= 3000) {
                return {
                    valid: true,
                };
            } else {
                return {
                    valid: false
                }
            }
        },
    };
};

FormValidation.validators.crossYear = crossYear;
FormValidation.validators.investmentMin = investmentMin;

validator = FormValidation.formValidation(form, {
    fields: {
        startPromo: {
            validators: {
                // check cross Year
                crossYear: {

                },
                notEmpty: {
                    message: "Please fill start promo"
                },
            }
        },
        investmentRecon: {
            validators: {
                // check investment
                investmentMin: {
                    message: 'Please fill in minimum investment value of 3,000'
                },
            }
        },
        subActivityId: {
            validators: {
                notEmpty: {
                    message: "Please select a sub activity"
                },
            }
        },
        channelId: {
            validators: {
                notEmpty: {
                    message: "Please select a channel"
                },
            }
        },
        mechanism1: {
            validators: {
                notEmpty: {
                    message: "Please fill in mechanism"
                },
                stringLength: {
                    max: 255,
                    message: 'Mechanism must be less than 255 characters',
                }
            }
        },
    },
    plugins: {
        trigger: new FormValidation.plugins.Trigger,
        bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
    }
});

$(document).ready(function () {
    $('form').submit(false);

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#investment");

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#investmentRecon");

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    promoId = url_str.searchParams.get("promoId");
    categoryShortDescEnc = url_str.searchParams.get("c");

    $('#btn_search_budget').hide();
    blockUI.block();
    disableButtonSave();
    if (method === "update") {
        getCategoryId(categoryShortDescEnc).then(function () {
            Promise.all([getEntity(), getSubCategory(), getChannel(), getRegion()]).then(async () => {
                await getData(promoId);
                await $('#entityId').val(entityId).trigger('change.select2');
                $("#entityId").attr("readonly", "readonly");

                await getGroupBrand(entityId);
                $('#groupBrandId').val(groupBrandId).trigger('change.select2');
                $("#groupBrandId").attr("readonly", "readonly");

                await getDistributor(entityId);
                await $('#distributorId').val(distributorId).trigger('change.select2');
                $("#distributorId").attr("readonly", "readonly");

                await getSubCategory(categoryId);
                $('#subCategoryId').val(subCategoryId).trigger('change.select2');
                $("#subCategoryId").attr("readonly", "readonly");

                await getSubActivity(subCategoryId);
                $('#subActivityId').val(subActivityId).trigger('change.select2');

                enableButtonSave();
                ActivityDescFormula();
                editableConfig(promoConfigItem[0]);

                blockUI.release();
            });
        });
    } else {

    }
});

$('#btn_search_budget').on('click', async function () {
    $('#dt_source_budget_list_search').val('');
    dt_source_budget_list.clear().draw();
    blockUI.block();

    let param = {
        period: $('#period').val(),
        entityId: $('#entityId').val(),
        distributorId: $('#distributorId').val(),
        subCategoryId: subCategoryId,
        activityId: activityId,
        subActivityId: $('#subActivityId').val(),
        region: [0],
        channel: [0],
        subChannel: [0],
        account: [0],
        subAccount: [0],
        brand: [$('#groupBrandId').val()],
        sku: [0],
    }

    $.ajax({
        url: "/promo/recon/list/source-budget",
        type: "GET",
        data: param,
        dataType: 'json',
        async: true,
        beforeSend: function () {
            if (!blockUI.blocked)
                blockUI.block();
        },
        success: function (result) {
            dt_source_budget_list.rows.add(result.data).draw();
        },
        complete: function () {
            blockUI.release();
            $('#modal_source_budget_list').modal('show');
        },
        error: function (jqXHR) {
            console.log(jqXHR);
        }
    });

});

$('#startPromo').on('change', async function () {
    let el_start = $('#startPromo');
    let el_end = $('#endPromo');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    let startYear = new Date(el_start.val()).getFullYear();
    let endYear = new Date(el_end.val()).getFullYear();
    if (startDate > endDate || startYear !== endYear) {
        el_end.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(startDate)
        });
        el_end.next().addClass('form-control-solid-bg')
        // el_end.next().css('background-color', '#fff !important');
    }
    ActivityDescFormula();
    validator.revalidateField('startPromo');
});

$('#endPromo').on('change', function () {
    let el_start = $('#startPromo');
    let el_end = $('#endPromo');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    let startYear = new Date(el_start.val()).getFullYear();
    let endYear = new Date(el_end.val()).getFullYear();
    if (startDate > endDate || startYear !== endYear) {
        el_start.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(endDate)
        });
        // el_start.next().css('background-color', '#fff !important');
    }
    ActivityDescFormula();
    validator.revalidateField('startPromo')
});

$('#subActivityId').on('change', async function () {
    if (method !== "update") {
        clearBudgetSource();
        let data = $(this).select2('data');
        activityId = (data[0].activityId ?? "0" );
    }
    ActivityDescFormula();
    validator.revalidateField('subActivityId');
});

$('#channelId').on('change', async function () {
    if (!blockUI.blocked) blockUI.block();
    let arrChannel = $(this).val();
    if (arrChannel.length === 0 || arrChannel[0] === "") {
        $(this).val([]).trigger('change.select2');
        $('#channelId option').prop('disabled', false);
        listChannel = [];
        listSubChannel = [];
        listAccount = [];
        listSubAccount = [];
    } else {
        listChannel = [];
        let listChannelParam = [];
        for (let i = 0; i < arrChannel.length; i++) {
            listChannel.push(parseInt(arrChannel[i]));
            listChannelParam.push(parseInt(arrChannel[i]));
        }
        await getSubChannel(listChannelParam);
        await getAccount();
        await getSubAccount();

        let textChannel = $(this).select2('data');
        if (textChannel[0].text.toLowerCase() === "all channel") {
            $('#channelId option').each(function () {
                if (this.text.toLowerCase() !== "all channel") {
                    $(this).prop('disabled', true);
                }
            });
        } else {
            $('#channelId option').each(function () {
                if (this.text.toLowerCase() === "all channel") {
                    $(this).prop('disabled', true);
                }
            });
        }
    }
    blockUI.release();
    validator.revalidateField('channelId');
});

$('.input_file').on('change', function () {
    let row = $(this).attr('data-row');
    let elLabel = $('#review_file_label_' + row);
    let oldNameFile = elLabel.text();
    if (this.files.length > 0) {
        let fileName = this.files[0].name;
        elLabel.text(fileName).attr('title', fileName);
        if (this.files[0].size > 10000000) {
            Swal.fire({
                title: swalTitle,
                icon: "warning",
                text: 'Maximum file size 10Mb',
                showConfirmButton: true,
                confirmButtonText: 'Confirm',
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
            elLabel.text(oldNameFile);
        } else if (checkNameFile(this.value)) {
            Swal.fire({
                title: swalTitle,
                icon: "warning",
                text: 'File name has special characters /\:*<>?|#%" \n. These are not allowed\n',
                showConfirmButton: true,
                confirmButtonText: 'Confirm',
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
            elLabel.text(oldNameFile);
        } else {
            upload_file($(this), row);
        }
    } else {
        let elAttachment = $('#attachment' + row);
        elAttachment.val('');
        elAttachment.removeClass('visible').addClass('invisible');
        elAttachment.attr('disabled', false);

        if (oldNameFile !== "") {
            elLabel.text(oldNameFile);
        } else {
            if (row === '1') {
                elLabel.text('For SKP Draft');
            } else if (row === '2') {
                elLabel.text('For SKP Fully Approved');
            } else {
                elLabel.text('');
            }

            elLabel.removeClass('form-control-solid-bg');

            $('#btn_delete' + row).attr('disabled', true);
            let elInfo = $('#info' + row);
            elInfo.removeClass('visible').addClass('invisible');
        }
    }
});

$('.btn_delete').on('click', function () {
    let row = this.value;
    let fileName = $('#review_file_label_' + row).text();
    let formData = new FormData();
    formData.append('fileName', fileName);
    formData.append('row', 'row'+row);
    if (method === 'update'){
        formData.append('promoId', promoId);
        formData.append('mode', 'edit');
    } else {
        formData.append('uuid', uuid);
        formData.append('promoId', promoId);
    }

    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this file",
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it',
        cancelButtonText: 'No, cancel',
        reverseButtons: true,
        allowOutsideClick: false,
        allowEscapeKey: false,
    }).then(function (result) {
        if (result.value) {
            $.ajax({
                url: "/promo/recon/attachment-delete",
                type: "POST",
                dataType: "JSON",
                data: formData,
                cache: false,
                processData: false,
                contentType: false,
                async: true,
                beforeSend: function () {
                    blockUI.block();
                },
                success: function (result) {
                    if (!result.error) {
                        $('#btn_delete' + row).attr('disabled', true);
                        $('#btn_download' + row).attr('disabled', true);
                        let elAttachment = $('#attachment' + row);
                        let elLabel = $('#review_file_label_' + row);
                        elAttachment.val('');

                        if (row === '1') {
                            elLabel.text('For SKP Draft');
                        } else if (row === '2') {
                            elLabel.text('For SKP Fully Approved');
                        } else {
                            elLabel.text('');
                        }
                        elLabel.removeClass('form-control-solid-bg');

                        elAttachment.removeClass('visible').addClass('invisible');
                        elAttachment.attr('disabled', false);

                        let elInfo = $('#info' + row);
                        elInfo.removeClass('visible').addClass('invisible');
                    } else {
                        Swal.fire({
                            title: 'File Delete',
                            text: result.message,
                            icon: 'warning',
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                },
                complete: function () {
                    blockUI.release();
                    validator.revalidateField('attachment1');
                },
                error: function (jqXHR) {
                    console.log(jqXHR)
                    blockUI.release();
                }
            });
        }
    });
});

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#review_file_label_' + row);
        let id = ((!promoId || promoId === "0") ? uuid : promoId);
        let url = file_host + "/assets/media/promo/" + id + "/row" + row + "/" + attachment.text();
        if (attachment.text() !== "") {
            fetch(url)
                .then((resp) => {
                    if (resp.ok) {
                        resp.blob().then(blob => {
                            const url_blob = window.URL.createObjectURL(blob);
                            const a = document.createElement('a');
                            a.style.display = 'none';
                            a.href = url_blob;
                            a.download = attachment.text();
                            document.body.appendChild(a);
                            a.click();
                            window.URL.revokeObjectURL(url_blob);
                        })
                            .catch(e => {
                                console.log(e);
                                Swal.fire({
                                    title: "Download Attachment",
                                    text: "Download attachment failed",
                                    icon: "warning",
                                    buttonsStyling: !1,
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"}
                                });
                            });
                    } else {
                        Swal.fire({
                            title: "Download Attachment",
                            text: "Attachment not found",
                            icon: "warning",
                            buttonsStyling: !1,
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                });
        }
    }
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");

    validator.validate().then(async function (status) {
        if (status === "Valid") {
            blockUI.block();
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            //enter reason if edit
            let modifyReason = '';
            if (method === "update") {
                Swal.fire({
                    title: 'reason modify',
                    input: 'text',
                    inputAttributes: {
                        autocapitalize: 'off'
                    },
                    showCancelButton: true,
                    confirmButtonText: 'Submit',
                    showLoaderOnConfirm: true,
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima", cancelButton: "btn btn-optima"}
                }).then(async function (result) {
                    if (result.isConfirmed) {
                        modifyReason = result.value;
                        await submitSave(modifyReason);
                    } else {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                        blockUI.release();
                    }
                });
            } else {
                await submitSave("");
            }
        }
    });
});

const submitSave = async function (modifyReason) {
    let e = document.querySelector("#btn_save");

    let elChannel = $("#channelId");
    if (elChannel.length > 0) {
        let channel = elChannel.val();
        let listChannelInput = [];
        for (let i = 0; i < channel.length; i++) {
            listChannelInput.push(channel[i]);
        }

        listChannel = listChannelInput;
    }

    let mechanism = {}
    mechanism.id = 1
    mechanism.mechanism = $('#mechanism1').val();
    mechanism.notes = ''
    mechanism.productId = 0
    mechanism.product = ''
    mechanism.brandId = 0
    mechanism.brand = ''

    arr_mechanism.push(mechanism);

    let entity = $('#entityId').select2('data');
    entityShortDesc = (entity[0].text ?? "0" );

    let formData = new FormData($('#form_promo')[0]);
    formData.append('promoId', promoId);
    formData.append('categoryId', categoryId);
    formData.append('subCategoryId', subCategoryId);
    formData.append('activityId', activityId);
    formData.append('activityDesc', strActivityName);
    formData.append('entityShortDesc', entityShortDesc);
    formData.append('categoryShortDesc', categoryShortDesc);
    formData.append('baselineSalesRecon', '0');
    formData.append('incrementSalesRecon', '0');
    formData.append('roiRecon', '0');
    formData.append('costRatioRecon', '0');
    formData.append('tsCode', '');
    formData.append('dataChannel', JSON.stringify(listChannel));
    formData.append('dataSubChannel', JSON.stringify(listSubChannel));
    formData.append('dataAccount', JSON.stringify(listAccount));
    formData.append('dataSubAccount', JSON.stringify(listSubAccount));
    formData.append('dataRegion', JSON.stringify(listRegion));
    formData.append('dataBrand', JSON.stringify(listBrand));
    formData.append('dataSKU', JSON.stringify(listSKU));
    formData.append('dataMechanism', JSON.stringify(arr_mechanism));
    formData.append('modifReason', modifyReason);
    formData.append('promoPlanId', 0);
    formData.append('allocationId', allocationId);
    formData.append('fileName1', $('#review_file_label_1').text());
    formData.append('fileName2', $('#review_file_label_2').text());
    formData.append('fileName3', $('#review_file_label_3').text());
    formData.append('fileName4', $('#review_file_label_4').text());
    formData.append('fileName5', $('#review_file_label_5').text());
    formData.append('fileName6', $('#review_file_label_6').text());
    formData.append('fileName7', $('#review_file_label_7').text());
    formData.delete('attachment1');
    formData.delete('attachment2');
    formData.delete('attachment3');
    formData.delete('attachment4');

    let url = "/promo/recon/update";
    $.ajax({
        url: url,
        data: formData,
        type: 'POST',
        async: true,
        dataType: 'JSON',
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function () {

        },
        success: function (result, status, xhr, $form) {
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
                    window.location.href = '/promo/recon';
                });
            } else {
                Swal.fire({
                    title: swalTitle,
                    html: result.message,
                    icon: "warning",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        },
        complete: function () {
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
            blockUI.release();
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
}

const getCategoryId = (categoryShortDescEnc) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon/category/category-desc",
            type        : "GET",
            data        : {categoryShortDesc: categoryShortDescEnc},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result.error) {
                    categoryId = result.data.Id;
                    categoryShortDesc = result.data.shortDesc;
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

const getEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon/list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc,
                        shortDesc: result.data[j].shortDesc,
                    });
                }
                $('#entityId').select2({
                    placeholder: "Select an Entity",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, errorThrown)
            {
                console.log(jqXHR);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getGroupBrand = (entityId) => {
    return new Promise((resolve, reject) => {
        if(entityId) {
            $.ajax({
                url: "/promo/recon/list/group-brand",
                type: "GET",
                data: {entityId: entityId},
                dataType: 'json',
                async: true,
                success: function (result) {
                    let data = [];
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        data.push({
                            id: result.data[j].id,
                            text: result.data[j].longDesc
                        });
                    }
                    $('#groupBrandId').select2({
                        placeholder: "Select a Brand",
                        width: '100%',
                        data: data
                    });
                },
                complete: function () {
                    return resolve();
                },
                error: function (jqXHR, errorThrown)
                {
                    console.log(jqXHR);
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

const getDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon/list/distributor",
            type        : "GET",
            data        : {entityId: entityId},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#distributorId').select2({
                    placeholder: "Select a Distributor",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, errorThrown)
            {
                console.log(jqXHR);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getSubCategory = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon/list/sub-category/category-id",
            type        : "GET",
            data        : {categoryId: categoryId},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                let exist = false;
                if (result.data.length > 0) {
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (result.data[j].id === subCategoryId) {
                            exist = true;
                        }
                        data.push({
                            id: result.data[j].id,
                            text: result.data[j].longDesc
                        });
                    }
                }
                if (!exist) data.push({id: subCategoryId, text: subCategoryDesc});
                $('#subCategoryId').select2({
                    placeholder: "Select a Sub Activity Type",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, errorThrown)
            {
                console.log(jqXHR);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getSubActivity = (subCategoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon/list/sub-activity/sub-category-id",
            type        : "GET",
            data        : {subCategoryId: subCategoryId},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                let exist = false;
                if (result.data.length > 0) {
                    for (let j = 0, len = result.data.length; j < len; ++j){
                        if (result.data[j].subActivityId === subActivityId) {
                            exist = true;
                        }
                        data.push({
                            id: result.data[j].subActivityId,
                            text: result.data[j].subActivityLongDesc,
                            activityId: result.data[j].activityId,
                            activityLongDesc: result.data[j].activityLongDesc,
                        });
                    }
                }
                if (!exist) data.push({id: subActivityId, text: subActivityLongDesc, activityId: activityId, activityLongDesc: activityLongDesc});
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
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon/list/channel-master",
            type        : "GET",
            data        : {promoId: promoId},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                listChannel = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    listChannel.push(result.data[j].id);
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#channelId').select2({
                    placeholder: "Select a Channel",
                    width: '100%',
                    data: data
                }).on('select2:unselecting', function() {
                    $(this).data('unselecting', true);
                }).on('select2:opening', function(e) {
                    if ($(this).data('unselecting')) {
                        $(this).removeData('unselecting');
                        e.preventDefault();
                    }
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, errorThrown)
            {
                console.log(jqXHR);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getSubChannel = (listChannelParam) => {
    return new Promise((resolve, reject) => {
        let url;
        url = "/promo/recon/list/sub-channel/channels";
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoId: promoId, dataChannel: JSON.stringify(listChannelParam)},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                listSubChannel = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    listSubChannel.push(result.data[j].id);
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, errorThrown)
            {
                console.log(jqXHR);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getAccount = () => {
    return new Promise((resolve, reject) => {
        let url;
        url = "/promo/recon/list/account/sub-channels";
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoId: promoId, dataSubChannel: JSON.stringify(listSubChannel)},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                listAccount = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    listAccount.push(result.data[j].id);
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, errorThrown)
            {
                console.log(jqXHR);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getSubAccount = () => {
    return new Promise((resolve, reject) => {
        let url;
        url = "/promo/recon/list/sub-account/accounts";
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoId: promoId, dataAccount: JSON.stringify(listAccount)},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                listSubAccount = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    listSubAccount.push(result.data[j].id);
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

const getRegion = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon/list/region",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                for (let i = 0; i < result.data.length; i++) {
                    listRegion.push(result.data[i].id)
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

const getBrand = (groupBrandId) => {
    return new Promise((resolve, reject) => {
        if(groupBrandId) {
            $.ajax({
                url: "/promo/recon/list/brand/group-brand-id",
                type: "GET",
                data: {groupBrandId: groupBrandId},
                dataType: 'json',
                async: true,
                success: function (result) {
                    listBrand = [];
                    for (let i = 0; i < result.data.length; i++) {
                        listBrand.push(result.data[i].id)
                    }
                },
                complete: function () {
                    return resolve();
                },
                error: function (jqXHR, errorThrown)
                {
                    console.log(jqXHR);
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

const getSKU = (brandId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon/list/sku",
            type        : "GET",
            data        : {brandId: brandId},
            dataType    : 'json',
            async       : true,
            beforeSend: function() {

            },
            success: function(result) {
                listSKU = [];
                for (let i = 0; i < result.data.length; i++) {
                    listSKU.push(result.data[i].id)
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, errorThrown)
            {
                console.log(jqXHR);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/recon/data/id",
            type: "GET",
            data: {promoId: id},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    if(result.errCode === 200){
                        let values = result.data;
                        promoConfigItem = values.promoConfigItem;

                        $('#txt_info_method').text('Edit ' + (values.promoHeader.refId ?? ""));

                        statusApprovalCode = values.promoHeader.statusApprovalCode;
                        isCancel = values.promoHeader.isCancel;
                        isClose = values.promoHeader.isClose;
                        tsCoding = values.promoHeader.tsCoding;
                        promoRefId = values.promoHeader.refId;
                        promoPlanningId = values.promoHeader.promoPlanId;
                        allocationId = values.promoHeader.allocationId;
                        categoryId = values.promoHeader.categoryId;
                        subCategoryId = values.promoHeader.subCategoryId;
                        subCategoryDesc = values.promoHeader.subCategoryDesc;
                        activityId = values.promoHeader.activityId;
                        activityLongDesc = values.promoHeader.activityLongDesc;
                        subActivityId = values.promoHeader.subActivityId;
                        subActivityLongDesc = values.promoHeader.subActivityLongDesc;
                        entityId = values.promoHeader.principalId;
                        distributorId = values.promoHeader.distributorId;
                        createOn = values.promoHeader.createOn;
                        isCancelLocked = values.promoHeader.isCancelLocked;
                        strActivityName = values.promoHeader.activityDesc;
                        startDatePromo = formatDate(values['promoHeader']['startPromo']);
                        endDatePromo = formatDate(values['promoHeader']['endPromo']);

                        if (values.channels) {
                            listChannel = [];
                            let arrStrChannel = [];
                            for (let i=0; i < values.channels.length; i++) {
                                if (values.channels[i].flag) {
                                    listChannel.push(values.channels[i].id);
                                    arrStrChannel.push(values.channels[i].longDesc);
                                }
                            }
                            strChannel = arrStrChannel.reduce((text, value, i, array) => text + (i < array.length - 1 ? ', ' : ', ') + value);

                            let dtStart = new Date(values.promoHeader.startPromo);
                            let dtEnd = new Date(values.promoHeader.endPromo);
                            let dtNow = new Date();

                            if (Math.floor((dtNow - dtEnd) / (24 * 3600 * 1000)) >= 0) {
                                validator.removeField('channelId');
                                $('#channelId').remove();
                                $('#dynamicElChannel').html('<input type="text" class="form-control form-control-sm" value="'+ strChannel +'" readonly />');
                            } else if (Math.floor((dtNow - dtStart) / (24 * 3600 * 1000)) >= 0 && Math.floor((dtNow - dtEnd) / (24 * 3600 * 1000)) < 0) {
                                validator.removeField('channelId');
                                $('#channelId').remove();
                                $('#dynamicElChannel').html('<input type="text" class="form-control form-control-sm" value="'+ strChannel +'" readonly />');
                            } else {
                                let elChannel = $('#channelId');
                                elChannel.val(listChannel).trigger('change.select2');
                                let textChannel = elChannel.select2('data');
                                if (textChannel[0].text.toLowerCase() === "all channel") {
                                    $('#channelId option').each(function () {
                                        if (this.text.toLowerCase() !== "all channel") {
                                            $(this).prop('disabled', true);
                                        }
                                    });
                                } else {
                                    $('#channelId option').each(function () {
                                        if (this.text.toLowerCase() === "all channel") {
                                            $(this).prop('disabled', true);
                                        }
                                    });
                                }
                            }
                        }

                        if (values.subChannels) {
                            listSubChannel = [];
                            for (let i=0; i < values.subChannels.length; i++) {
                                if (values.subChannels[i].flag) listSubChannel.push(values.subChannels[i].id);
                            }
                        }

                        if (values.accounts) {
                            listAccount = [];
                            for (let i=0; i < values.accounts.length; i++) {
                                if (values.accounts[i].flag) listAccount.push(values.accounts[i].id);
                            }
                        }

                        if (values.subAccounts) {
                            listSubAccount = [];
                            for (let i=0; i < values.subAccounts.length; i++) {
                                if (values.subAccounts[i].flag) listSubAccount.push(values.subAccounts[i].id);
                            }
                        }

                        if (values.groupBrand) {
                            if (values.groupBrand.length > 0) {
                                for (let i = 0; i < values.groupBrand.length; i++) {
                                    if (values.groupBrand[i].flag) groupBrandId = values.groupBrand[i].id;
                                }
                            }
                        }

                        if (values.brands) {
                            listBrand = [];
                            for (let i=0; i < values.brands.length; i++) {
                                if (values.brands[i].flag) listBrand.push(values.brands[i].id);
                            }
                        }

                        if (values.skus) {
                            listSKU = [];
                            for (let i=0; i < values.skus.length; i++) {
                                if (values.skus[i].flag) listSKU.push(values.skus[i].id);
                            }
                        }

                        $('#period').val(new Date(values.promoHeader.startPromo).getFullYear());
                        $('#allocationRefId').val(values.promoHeader.allocationRefId);

                        $('#startPromo').val(formatDate(values.promoHeader.startPromo)).flatpickr({
                            altFormat: "d-m-Y",
                            altInput: true,
                            allowInput: false,
                            dateFormat: "Y-m-d",
                            disableMobile: "true",
                        });
                        $('#endPromo').val(formatDate(values.promoHeader.endPromo)).flatpickr({
                            altFormat: "d-m-Y",
                            altInput: true,
                            allowInput: false,
                            dateFormat: "Y-m-d",
                            disableMobile: "true",
                        });
                        $('#startPromo').next().css('background-color', '#fff !important');
                        $('#endPromo').next().css('background-color', '#fff !important');

                        $('#initiatorNotes').val(values.promoHeader.initiator_notes);
                        $('#investment').val(formatMoney(values.promoHeader.investment, 0));
                        $('#investmentRecon').val(formatMoney(values.promoHeader.investment, 0));
                        $('#actualDNClaimed').val(formatMoney(values.promoHeader.invoiced, 0));

                        if (values.mechanisms.length > 0) {
                            for (let i = 0; i < values.mechanisms.length; i++) {
                                $('#mechanism1').val(values.mechanisms[i].mechanism);
                            }
                        }

                        $('#allocationDesc').val(values.promoHeader.allocationDesc);
                        $('#budgetDeployed').val(formatMoney(values.promoHeader.budgetAmount, 0));
                        $('#budgetRemaining').val(formatMoney(values.promoHeader.remainingBudget, 0));

                        if (values.attachments) {
                            let fileSource = "";
                            values.attachments.forEach((item, index, arr) => {
                                if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                    let elLabel = $('#review_file_label_' + parseInt(item.docLink.replace('row', '')));
                                    elLabel.text(item.fileName).attr('title', item.fileName);
                                    elLabel.addClass('form-control-solid-bg');
                                    $('#attachment' + parseInt(item.docLink.replace('row', ''))).attr('disabled', true);
                                    $('#btn_download' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                                    $('#btn_delete' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                                    fileSource = file_host + "/assets/media/promo/" + promoId + "/" + item.docLink + "/" + item.fileName;
                                    const fileInput = document.querySelector('#attachment' + parseInt(item.docLink.replace('row', '')));
                                    fileInput.dataset.file = fileSource;
                                }
                            });
                        }
                    } else {
                        if (result.errCode === 404) {
                            Swal.fire({
                                title: swalTitle,
                                text: result.message,
                                icon: "warning",
                                buttonsStyling: !1,
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(function () {
                                window.location.href = '/promo/recon';
                            });
                        }
                    }
                }
            },
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const checkNameFile = (value) => {
    if (value) {
        var startIndex = (value.indexOf('\\') >= 0 ? value.lastIndexOf('\\') : value.lastIndexOf('/'));
        var filename = value.substring(startIndex);
        if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
            filename = filename.substring(1);
        }
    }
    let format = /[/\:*"<>?|#%]/;
    return format.test(filename);
}

const upload_file =  (el, row) => {
    let formData = new FormData();
    let file = document.getElementById('attachment'+row).files[0];
    if (method === 'update') {
        formData.append('promoId', promoId);
        formData.append('mode', 'edit');
    } else {
        formData.append('promoId', '0');
    }

    formData.append('file', file);
    formData.append('row', 'row'+row);
    formData.append('docLink', el.val());

    $.ajax({
        url: "/promo/recon/attachment-upload",
        type: "POST",
        dataType: "JSON",
        data: formData,
        cache: false,
        processData: false,
        contentType: false,
        async: true,
        beforeSend: function () {
            blockUI.block();
        },
        success: function (result) {
            let swalType = '';
            let elInfo = $('#info' + row);
            if (!result.error) {
                swalType = 'success';
                $('#btn_delete' + row).attr('disabled', false);
                $('#btn_download' + row).attr('disabled', false);
                elInfo.removeClass('invisible').addClass('visible');
                elInfo.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
            } else {
                swalType = 'error';
                elInfo.removeClass('invisible').addClass('visible');
                elInfo.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                $('#attachment'+row).val('');
                if (row === '1') {
                    $('#review_file_label_'+row).text('For SKP Draft');
                } else if (row === '2') {
                    $('#review_file_label_'+row).text('For SKP Fully Approved');
                } else {
                    $('#review_file_label_'+row).text('');
                }
                $('#btn_delete' + row).attr('disabled', true);
            }
            Swal.fire({
                title: 'File Upload',
                text: result.message,
                icon: swalType,
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        },
        complete: function () {
            blockUI.release();
            validator.revalidateField('attachment1');
        },
        error: function (jqXHR) {
            console.log(jqXHR);
            let elInfo = $('#info' + row);
            $('#attachment'+row).val('');
            if (row === '1') {
                $('#review_file_label_'+row).text('For SKP Draft');
            } else if (row === '2') {
                $('#review_file_label_'+row).text('For SKP Fully Approved');
            } else {
                $('#review_file_label_'+row).text('');
            }
            $('#btn_delete' + row).attr('disabled', true);
            elInfo.removeClass('invisible').addClass('visible');
            elInfo.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
        }
    });
}

const ActivityDescFormula = () => {
    let strMonth = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Des']

    let startPromoYear = new Date($('#startPromo').val()).getFullYear();
    let startPromoMonth = new Date($('#startPromo').val()).getMonth();

    let strStartPromoMonth = strMonth[startPromoMonth];

    let periode_desc = [strStartPromoMonth, startPromoYear.toString()].join(' ');
    let subactivity_desc = '';
    if ($("#subActivityId").val()) {
        subactivity_desc = $("#subActivityId").select2('data')[0].text;
        strActivityName = subactivity_desc + ' ' + periode_desc;
    }
}

const editableConfig = (values) => {
    $('#subActivityId').attr('readonly', values.subActivity);
    $('#channelId').attr('readonly', values.channel);
    $('#mechanism1').attr('readonly', values.mechanism);
    $('#investmentRecon').attr('readonly', values.investment);
    $('#initiatorNotes').attr('readonly', values.initiatorNotes);

    // Budget Source
    if (values.budgetSource == true) {
        $('#btn_search_budget').hide();
    } else {
        $('#btn_search_budget').show();
    }

    if (values.startPromo == true) {
        $('#startPromo')
        .flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: false,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            clickOpens: false,
        });
        $('#startPromo').attr('readonly', true);
        $('#startPromo').addClass('form-control-solid-bg');
    }

    if (values.endPromo == true) {
        $('#endPromo')
        .flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: false,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            clickOpens: false,
        });
        $('#endPromo').attr('readonly', true);
        $('#endPromo').addClass('form-control-solid-bg');
    }

    //Config
    configStartPromo = values.startPromo;
    configEndPromo = values.endPromo;

    //attachment
    if (values.attachment == true) {
        $('.input_file').attr('disabled', true);
        $('.review_file_label').addClass('form-control-solid-bg');
        $('.btn_delete').attr('disabled', true);
    }
}

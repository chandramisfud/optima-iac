'use strict';

let swalTitle = "Promo Send Back Reconciliation";
let dt_mechanism, validator, method, newMechanismMethod, uuid, categoryShortDesc, categoryShortDescEnc, createOn, url_str, promoId;
let errCode, statusApprovalCode, isCancel, isClose, isCancelLocked, promoRefId, promoPlanningId, allocationId, tsCoding, allowedit;
let categoryId, subCategoryId, entityId, entityShortDesc, distributorId, activityLongDesc, subActivityLongDesc, channelId, subChannelId, accountId, subAccountId, activityId, subActivityId;
let startDatePromo, endDatePromo;
let investmentTypeId, minROI, maxROI, minCR, maxCR;
let arr_region = [], arr_brand = [], arr_sku = [], arr_sku_temp = [], arr_mechanism = [];
let appCutOffMechanism, appCutOffHierarchy;
let promoConfigItem;
let listSubCategory = [];
let configRegion, configBrand, configSku, configMechanism, configStartPromo, configEndPromo;

FormValidation.validators.crossYear = function () {
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

FormValidation.validators.investmentMin = function () {
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

FormValidation.validators.actualSales = function () {
    return {
        validate: async function (input) {
            const baseLine = parseFloat($('#baselineSalesRecon').val().replace(/,/g, ''));
            const incrSales = parseFloat($('#incrementSalesRecon').val().replace(/,/g, ''));
            const actualSales = await getBaseline();
            if ((baseLine + incrSales) <= actualSales) {
                return {
                    valid: true
                };
            } else {
                return {
                    valid: false,
                    message: 'Total sales cannot exceed actual sales from Blitz: ' + formatMoney(actualSales, 0)
                };
            }
        },
    };
};

FormValidation.validators.roiMinMax = function () {
    return {
        validate: function (input) {
            const value = parseFloat(input.value.replace(/,/g, ''));
            if ((value < minROI && minROI !== 0) || (value > maxROI && maxROI !== 0)) {
                return {
                    valid: false
                }
            } else {
                return {
                    valid: true,
                };
            }
        },
    };
};

FormValidation.validators.crMinMax = function () {
    return {
        validate: function (input) {
            const value = parseFloat(input.value.replace(/,/g, ''));
            if ((value < minCR && minCR !== 0) || (value > maxCR && maxCR !== 0)) {
                return {
                    valid: false,
                };
            } else {
                return {
                    valid: true
                }
            }
        },
    };
};

validator = FormValidation.formValidation(document.getElementById('form_promo'), {
    fields: {
        subCategoryId: {
            validators: {
                notEmpty: {
                    message: "Please select a sub category"
                },
            }
        },
        activityId: {
            validators: {
                notEmpty: {
                    message: "Please select an activity"
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
        allocationRefId: {
            validators: {
                notEmpty: {
                    message: "Please select budget source"
                },
            }
        },
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
        activityDesc: {
            validators: {
                notEmpty: {
                    message: "Please fill in activity name"
                },
                stringLength: {
                    max: 255,
                    message: 'Activity Name must be less than 255 characters',
                }

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
        // incrementSalesRecon: {
        //     validators: {
        //         actualSales: {
        //         },
        //     }
        // },
        channelId: {
            validators: {
                notEmpty: {
                    message: "Please select a channel"
                },
            }
        },
        subChannelId: {
            validators: {
                notEmpty: {
                    message: "Please select a sub channel"
                },
            }
        },
        accountId: {
            validators: {
                notEmpty: {
                    message: "Please select an account"
                },
            }
        },
        subAccountId: {
            validators: {
                notEmpty: {
                    message: "Please select a sub account"
                },
            }
        },
    },
    plugins: {
        trigger: new FormValidation.plugins.Trigger,
        bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
    }
});

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

var targetModal = document.querySelector(".modal-content");
var blockUIModal = new KTBlockUI(targetModal, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetRecon = document.querySelector(".card_recon");
let blockUIRecon = new KTBlockUI(targetRecon, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetAttribute = document.querySelector(".card_attribute");
let blockUIAttribute = new KTBlockUI(targetAttribute, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetAttachment = document.querySelector(".card_attachment");
let blockUIAttachment = new KTBlockUI(targetAttachment, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $('form').submit(false);

    url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    promoId = url_str.searchParams.get("id");
    categoryShortDescEnc = url_str.searchParams.get("c");
    uuid = generateUUID(5);

    $('#startPromo, #endPromo')
        .flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
        });

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#investmentRecon");

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#baselineSalesRecon, #incrementSalesRecon, #totalSalesRecon, #totalInvestmentRecon, #roiRcon, #costRatioRcon");

    dt_mechanism = $('#dt_mechanism').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "20vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                width: 50,
                orderable: false,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                title: 'Mechanism',
                data: 'mechanism',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Notes',
                data: 'notes',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $('#btn_search_budget').hide();
    blockUI.block();
    blockUIAttribute.block();
    blockUIRecon.block();
    blockUIAttachment.block();
    disableButtonSave();
    getCategoryId(categoryShortDescEnc).then(function () {
        Promise.all([ getCutOff(), getChannel() ]).then(async () => {

            await getSubCategory(categoryId);

            await getData(promoId).then( async function () {
                await $('#channelId').val(channelId).trigger('change.select2');

                await getSubChannel(channelId);
                await $('#subChannelId').val(subChannelId).trigger('change.select2');

                await getAccount(subChannelId);
                await $('#accountId').val(accountId).trigger('change.select2');

                await getSubAccount(accountId)
                $('#subAccountId').val(subAccountId).trigger('change.select2');

                await getActivity(subCategoryId);
                await $('#activityId').val(activityId).trigger('change.select2');

                await getSubActivity(activityId);
                await $('#subActivityId').val(subActivityId).trigger('change.select2');

                if (method === "update") {
                    if (isClose || isCancel || statusApprovalCode !== 'TP2' || isCancelLocked) {
                        disableButtonSave();
                        let txtReason = ''
                        if (statusApprovalCode !== 'TP2') txtReason = ', because this promo is not reconciliation';
                        if (isCancelLocked) txtReason = ', because this promo has cancelled request';
                        if (isCancel) txtReason = ', because this promo is cancel';
                        if (isClose) txtReason = ', because this promo is close';
                        Swal.fire({
                            title: swalTitle,
                            text: 'Promo ' + promoRefId + ' can not edit' + txtReason,
                            icon: "warning",
                            buttonsStyling: !1,
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.location.href = '/promo/send-back';
                        });
                    }
                }

                enableButtonSave();
                editableConfig(promoConfigItem[0]);
                blockUI.release();
                blockUIAttribute.release();
                blockUIRecon.release();
                blockUIAttachment.release();
            });
        });
    });

    $('.modal-header').on('mousedown', function (e) {
        $('.modal-dialog').draggable("disable");
    }).on('mouseover', function () {
        $('.modal-header').css('cursor', 'default');
        $('.modal-title').css('cursor', 'default');
    });
});

$('#btn_search_budget').on('click', function () {
    $('#dt_source_budget_list_search').val('');
    dt_source_budget_list.clear().draw();
    blockUI.block();
    let arr_channel = [$('#channelId').val()];
    let arr_subChannel = [$('#subChannelId').val()];
    let arr_account = [$('#accountId').val()];
    let arr_subAccount = [$('#subAccountId').val()];
    let data = {
        period: $('#period').val(),
        entityId: entityId,
        distributorId: distributorId,
        subCategoryId: subCategoryId,
        activityId: $('#activityId').val(),
        subActivityId: $('#subActivityId').val(),
        region: arr_region,
        channel: arr_channel,
        subChannel: arr_subChannel,
        account: arr_account,
        subAccount: arr_subAccount,
        brand: [arr_brand[0] ?? 0],
        sku: [arr_sku[0] ?? 0],
    }

    $.ajax({
        url: "/promo/recon-send-back/list/source-budget",
        type: "GET",
        data: data,
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
            $('#modal_source_budget_list').modal('show');
            blockUI.release();
        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(jqXHR.responseText);
            return reject(errorThrown);
        }
    });

});

$('#sticky_notes').on('click', function () {
    let elModal = $('#modal_notes');
    if (elModal.hasClass('show')) {
        elModal.modal('hide');
    } else {
        elModal.modal('show');
    }
});

$('#channelId').on('change', async function () {
    blockUIAttribute.block();
    let elSubChannel = $('#subChannelId');
    let elAccount = $('#accountId');
    let elSubAccountId = $('#subAccountId');
    elSubChannel.empty();
    elAccount.empty();
    elSubAccountId.empty();
    if ($(this).val()) await getSubChannel($(this).val());
    clearAttribute();
    blockUIAttribute.release();
    elSubChannel.val('').trigger('change.select2');
    elAccount.val('').trigger('change.select2');
    elSubAccountId.val('').trigger('change.select2');
    validator.revalidateField('channelId');
    validator.revalidateField('subChannelId');
    validator.revalidateField('accountId');
    validator.revalidateField('subAccountId');
});

$('#subChannelId').on('change', async function () {
    blockUIAttribute.block();
    let elAccount = $('#accountId');
    let elSubAccountId = $('#subAccountId');
    elAccount.empty();
    elSubAccountId.empty();
    if ($(this).val() !== ""  && $(this).val() !== null) await getAccount($(this).val());
    blockUIAttribute.release();
    elAccount.val('').trigger('change.select2');
    elSubAccountId.val('').trigger('change.select2');
    validator.revalidateField('subChannelId');
    validator.revalidateField('accountId');
    validator.revalidateField('subAccountId');
});

$('#accountId').on('change', async function () {
    blockUIAttribute.block();
    let elSubAccountId = $('#subAccountId');
    elSubAccountId.empty();
    if ($(this).val() !== ""  && $(this).val() !== null) await getSubAccount($(this).val());
    blockUIAttribute.release();
    elSubAccountId.val('').trigger('change.select2');
    validator.revalidateField('accountId');
    validator.revalidateField('subAccountId');
});

$('#subAccountId').on('change', async function () {
    validator.revalidateField('subAccountId')
});

$('#subCategoryId').on('change', async function () {
    blockUI.block();
    let elActivity = $('#activityId');
    let elSubActivity = $('#subActivityId');
    elActivity.empty();
    elSubActivity.empty();
    if ($(this).val()) await getActivity($(this).val());
    clearAttribute();
    clearBudgetSource();
    blockUI.release();
    elActivity.val('').trigger('change.select2');
    elSubActivity.val('').trigger('change.select2');
    validator.revalidateField('subCategoryId');
    validator.revalidateField('activityId');
    validator.revalidateField('subActivityId');
    subCategoryId = $(this).val();
});

$('#activityId').on('change', async function () {
    blockUI.block();
    let elSubActivity = $('#subActivityId');
    elSubActivity.empty();
    if ($(this).val()) await getSubActivity($(this).val());
    clearAttribute();
    activityId = $('#activityId').val();
    blockUI.release();
    elSubActivity.val('').trigger('change.select2');
    validator.revalidateField('activityId');
    validator.revalidateField('subActivityId');
});

$('#subActivityId').on('change', async function () {
    blockUI.block();
    $('#investmentType').val('');
    if ($(this).val()) {
        await getInvestmentType($(this).val());
        await getConfigRoiCR($(this).val());
        let data = $('#subActivityId').select2('data');
        allowedit = ((data[0].allowEdit) ?? 1);
    }
    ActivityDescFormula();
    clearAttribute();
    subActivityId = $('#subActivityId').val();
    blockUI.release();

    validator.addField('roi', {
        validators: {
            // check min max ROI
            roiMinMax: {
                message: 'Min. ROI ' + minROI + '% and Max. ROI ' + maxROI + '%'
            },
        }
    });
    validator.addField('costRatio', {
        validators: {
            // check min max CR
            crMinMax: {
                message: 'Min. CR ' + minCR + '% and Max. CR ' + maxCR + '%'
            },
        }
    });
    validator.revalidateField('subActivityId');
    validator.revalidateField('roi');
    validator.revalidateField('costRatio');
});

$('#incrementSalesRecon').on('blur', async function() {
    validator.revalidateField('incrementSalesRecon');
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
    let form_data = new FormData();
    form_data.append('fileName', fileName);
    form_data.append('row', 'row'+row);
    if (method === 'update'){
        form_data.append('promoId', promoId);
        form_data.append('mode', 'edit');
    } else {
        form_data.append('promoId', promoId);
        form_data.append('uuid', uuid);
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
            if (fileName !== 'Choose File') {
                blockUIAttachment.block();
                $.ajax({
                    url: "/promo/recon-send-back/attachment-delete",
                    type: "POST",
                    dataType: "JSON",
                    data: form_data,
                    cache: false,
                    processData: false,
                    contentType: false,
                    async: true,
                    beforeSend: function () {

                    },
                    success: function (result) {
                        if (!result.error) {
                            $('#btn_delete' + row).attr('disabled', true);
                            $('#btn_download' + row).attr('disabled', true);
                            let elAttachment = $('#attachment' + row);
                            let elLabelAttachment = $('#review_file_label_' + row);
                            elAttachment.val('');
                            if (row === '1') {
                                elLabelAttachment.text('For SKP Draft');
                            } else if (row === '2') {
                                elLabelAttachment.text('For SKP Fully Approved');
                            } else {
                                elLabelAttachment.text('');
                            }
                            elLabelAttachment.removeClass('form-control-solid-bg');

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
                        blockUIAttachment.release();
                        validator.revalidateField('attachment1');
                    },
                    error: function (jqXHR) {
                        console.log(jqXHR)
                        blockUIAttachment.release();
                    }
                });
            }
        }
    });
});

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let id = ((!promoId || promoId===0) ? uuid : promoId);
        let attachment = $('#review_file_label_' + row);
        let url = file_host + "/assets/media/promo/" + id + "/row" + row + "/" + attachment.text();
        blockUIAttachment.block();
        if (attachment.text() !== "") {
            fetch(url).then((resp) => {
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
                        blockUIAttachment.release();
                    })
                        .catch(e => {
                            blockUIAttachment.release();
                            console.log(e);
                            Swal.fire({
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
                    blockUIAttachment.release();
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
        el_end.next().addClass('form-control-solid-bg');
    }
    ActivityDescFormula();
    clearAttribute();
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
    }
    ActivityDescFormula();
    clearAttribute();
    validator.revalidateField('startPromo');
});

$('#incrementSalesRecon, #investmentRecon').on('keyup', function () {
    calcPromo();
});

$('#btn_attribute').on('click', async function () {
    let e = document.querySelector("#btn_attribute");
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    await getMechanism();
    if (newMechanismMethod) {
        $("#attribute_region").removeClass('col-md-4').addClass('col-md-12');
        $('#attribute_brand').addClass('d-none');
        $('#attribute_sku').addClass('d-none');
        $('#newMechanism').removeClass('d-none');
        $('#manualMechanism').addClass('d-none');
        getRegion().then(async function () {
            await readMechanismMechanismNew();

            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
            $('#modal_attribute').modal('show');
        });

        let dtStart = new Date($('#startPromo').val()).getTime();
        let dtEnd = new Date($('#endPromo').val()).getTime();
        let dtNow = new Date().getTime();

        // on going
        if (dtStart <= dtNow && dtEnd >= dtNow) {
            if(configMechanism == true){
                $('#mechanism_source').addClass('d-none');
                $('#mechanism_result').removeClass('col-lg-8').addClass('col-lg-12');
                $('#mechanism_result').removeClass('col-md-8').addClass('col-md-12');
                $('.btn-edit-mechanism-notes').addClass('d-none');
            }
        }
    } else {
        $("#attribute_region").removeClass('col-md-12').addClass('col-md-4');
        $('#attribute_brand').removeClass('d-none');
        $('#attribute_sku').removeClass('d-none');
        $('#newMechanism').addClass('d-none');
        $('#manualMechanism').removeClass('d-none');
        getRegion().then(async function () {
            await getBrand(entityId);
            arr_sku_temp = [];
            dt_sku.clear().draw();
            await readMechanismManual();

            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
            $('#modal_attribute').modal('show');
        });

        if(configMechanism == true){
            $('#mechanisme1').attr('readonly', true);
            $('#mechanisme2').attr('readonly', true);
            $('#mechanisme3').attr('readonly', true);
            $('#mechanisme4').attr('readonly', true);
        }
    }
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");

    validator.validate().then(async function (status) {
        if (status === "Valid") {
            blockUI.block();
            blockUIAttribute.block();
            blockUIRecon.block();
            blockUIAttachment.block();
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;

            if (dt_mechanism.rows().count() === 0) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please enter mechanism",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    blockUI.release();
                    blockUIAttribute.release();
                    blockUIRecon.release();
                    blockUIAttachment.release();
                });
            }

            if (arr_region.length === 0) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please enter region",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    blockUI.release();
                    blockUIAttribute.release();
                    blockUIRecon.release();
                    blockUIAttachment.release();
                });
            }

            if (arr_brand.length === 0) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please enter brand",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    blockUI.release();
                    blockUIAttribute.release();
                    blockUIRecon.release();
                    blockUIAttachment.release();
                });
            }

            if (arr_sku.length === 0) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please enter sku",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    blockUI.release();
                    blockUIAttribute.release();
                    blockUIRecon.release();
                    blockUIAttachment.release();
                });
            }

            //enter reason if edit
            let modifReason = '';
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
                        modifReason = result.value;
                        await submitSave(modifReason);
                    } else {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                        blockUI.release();
                        blockUIAttribute.release();
                        blockUIRecon.release();
                        blockUIAttachment.release();
                    }
                });
            } else {
                await submitSave("");
            }
        }
    });
});

const submitSave = async function (modifReason) {
    let e = document.querySelector("#btn_save");

    let listChannel = [];
    listChannel.push($("#channelId").val());

    let listSubChannel = [];
    listSubChannel.push($("#subChannelId").val());

    let listAccount = [];
    listAccount.push($("#accountId").val());

    let listSubAccount = [];
    listSubAccount.push($("#subAccountId").val());

    let formData = new FormData($('#form_promo')[0]);
    formData.append('promoId', promoId);
    formData.append('categoryId', categoryId);
    formData.append('subCategoryId', subCategoryId);
    formData.append('activityId', activityId);
    formData.append('subActivityId', subActivityId);
    formData.append('entityShortDesc', entityShortDesc);
    formData.append('categoryShortDesc', categoryShortDesc);
    formData.append('statusApproval', statusApprovalCode);
    formData.append('tsCode', tsCoding);
    formData.append('dataChannel', JSON.stringify(listChannel));
    formData.append('dataSubChannel', JSON.stringify(listSubChannel));
    formData.append('dataAccount', JSON.stringify(listAccount));
    formData.append('dataSubAccount', JSON.stringify(listSubAccount));
    formData.append('dataRegion', JSON.stringify(arr_region));
    formData.append('dataBrand', JSON.stringify(arr_brand));
    formData.append('dataSKU', JSON.stringify(arr_sku));
    formData.append('dataMechanism', JSON.stringify(arr_mechanism));
    formData.append('modifReason', modifReason);
    formData.append('promoPlanId', promoPlanningId);
    formData.append('allocationId', allocationId);
    formData.append('fileName1', $('#review_file_label_1').text());
    formData.append('fileName2', $('#review_file_label_2').text());
    formData.append('fileName3', $('#review_file_label_3').text());
    formData.append('fileName4', $('#review_file_label_4').text());
    formData.append('fileName5', $('#review_file_label_5').text());
    formData.append('fileName6', $('#review_file_label_6').text());
    formData.append('fileName7', $('#review_file_label_7').text());
    formData.append('promoId', promoId);

    let url = "/promo/recon-send-back/update";
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
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
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
                    window.location.href = '/promo/recon-send-back';
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
            blockUIAttribute.release();
            blockUIRecon.release();
            blockUIAttachment.release();
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

const checkNameFile = (value) => {
    if (value) {
        let startIndex = (value.indexOf('\\') >= 0 ? value.lastIndexOf('\\') : value.lastIndexOf('/'));
        let filename = value.substring(startIndex);
        if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
            filename = filename.substring(1);
        }
        let format = /[/:*"<>?|#%]/;
        return format.test(filename);
    }
}

const getBaseline = () => {
    let arr_channel = [];
    let arr_subChannel = [];
    let arr_account = [];
    let arr_subAccount = [];
    arr_channel.push($('#channelId').val());
    arr_subChannel.push($('#subChannelId').val());
    arr_account.push($('#accountId').val());
    arr_subAccount.push($('#subAccountId').val())
    let data = {
        promoId: promoId,
        period: $('#period').val(),
        Entity: entityId,
        distributorId: distributorId,
        subCategoryId: subCategoryId,
        activityId: activityId,
        subActivityId: subActivityId,
        region: arr_region,
        channel: arr_channel,
        subChannel: arr_subChannel,
        account: arr_account,
        subAccount: arr_subAccount,
        brand: arr_brand,
        sku: arr_sku,
        startPromo: $('#startPromo').val(),
        endPromo: $('#endPromo').val(),
    }
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/recon-send-back/baseline",
            type: "GET",
            data: data,
            dataType: 'json',
            async: true,
            beforeSend: function () {
            },
            success: function (result) {
                let actualSales = 0;
                if (!result.error) {
                    actualSales = result.data.actual_sales;
                }
                resolve(actualSales);
            },
            complete: function () {
                blockUI.release();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return resolve(0);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const upload_file =  (el, row) => {
    blockUIAttachment.block();
    let form_data = new FormData();
    let file = document.getElementById('attachment'+row).files[0];
    if (method === 'update'){
        form_data.append('promoId', promoId);
        form_data.append('mode', 'edit');
    } else {
        form_data.append('promoId', '0');
    }

    form_data.append('uuid', uuid);
    form_data.append('file', file);
    form_data.append('row', 'row'+row);
    form_data.append('docLink', el.val());

    $.ajax({
        url: "/promo/recon-send-back/attachment-upload",
        type: "POST",
        dataType: "JSON",
        data: form_data,
        cache: false,
        processData: false,
        contentType: false,
        async: true,
        beforeSend: function () {
        },
        success: function (result) {
            let swalType;
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
            blockUIAttachment.release();
            validator.revalidateField('attachment1');
        },
        error: function (jqXHR) {
            console.log(jqXHR)
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

                        errCode = 200;
                        $('#txt_info_method').text('Edit ' + (values.promoHeader.refId ?? ""));
                        statusApprovalCode = values.promoHeader.statusApprovalCode;
                        isCancel = values.promoHeader.isCancel;
                        isClose = values.promoHeader.isClose;
                        isCancelLocked = values.promoHeader.isClose;
                        tsCoding = values.promoHeader.tsCoding;
                        promoRefId = values.promoHeader.refId;
                        promoPlanningId = values.promoHeader.promoPlanId;
                        allocationId = values.promoHeader.allocationId;
                        categoryId = values.promoHeader.categoryId;
                        subCategoryId = values.promoHeader.subCategoryId;
                        activityId = values.promoHeader.activityId;
                        subActivityId = values.promoHeader.subActivityId;
                        entityId = values.promoHeader.principalId;
                        distributorId = values.promoHeader.distributorId;
                        activityLongDesc = values.promoHeader.activityLongDesc;
                        subActivityLongDesc = values.promoHeader.subActivityLongDesc;
                        entityShortDesc = values.promoHeader.principalShortDesc;
                        allowedit = values.promoHeader.allowedit;
                        createOn = new Date(values.promoHeader.createOn);
                        startDatePromo = formatDate(values['promoHeader']['startPromo']);
                        endDatePromo = formatDate(values['promoHeader']['endPromo']);

                        // header
                        if (values.promoHeader.reconciled) $('#finalRecon').prop('checked', true);
                        $('#finalRecon').prop('disabled', true);
                        $('#period').val(new Date(values.promoHeader.startPromo).getFullYear());
                        $('#promoPlanRefId').val(values.promoHeader.promoPlanRefId);
                        $('#allocationRefId').val(values.promoHeader.allocationRefId);
                        $('#allocationDesc').val(values.promoHeader.allocationDesc);

                        $('#startPromo').val(formatDate(values.promoHeader.startPromo)).flatpickr({
                            altFormat: "d-m-Y",
                            altInput: true,
                            allowInput: true,
                            dateFormat: "Y-m-d",
                            disableMobile: "true",
                        });
                        $('#endPromo').val(formatDate(values.promoHeader.endPromo)).flatpickr({
                            altFormat: "d-m-Y",
                            altInput: true,
                            allowInput: true,
                            dateFormat: "Y-m-d",
                            disableMobile: "true",
                        });

                        for (let i=0; i<listSubCategory.length; i++) {
                            if (listSubCategory[i].id !== values.promoHeader.subCategoryId) {
                                $('#subCategoryId').select2({
                                    data: [{
                                        'id' : values.promoHeader.subCategoryId,
                                        'text' : values.promoHeader.subCategoryLongDesc
                                    }]
                                });
                            }
                        }

                        $('#activityDesc').val(values.promoHeader.activityDesc);
                        $('#tsCoding').val(values.promoHeader.tsCoding);
                        $('#subCategoryId').val(values.promoHeader.subCategoryId).trigger('change.select2');
                        $('#entityDesc').val(values.promoHeader.principalName);
                        $('#distributorDesc').val(values.promoHeader.distributorName);
                        $('#budgetDeployed').val(formatMoney(values.promoHeader.budgetAmount,0) ?? 0);
                        $('#budgetRemaining').val(formatMoney(values.promoHeader.remainingBudget,0) ?? 0);
                        $('#initiatorNotes').val(values.promoHeader.initiator_notes);
                        $('#investmentType').val(values.promoHeader.investmentTypeDesc);

                        $('#baselineSales').val(formatMoney(values.promoHeader.prevNormalSales, 0) ?? 0);
                        $('#incrementSales').val(formatMoney(values.promoHeader.prevIncrSales, 0) ?? 0);
                        $('#investment').val(formatMoney(values.promoHeader.prevInvestment, 0) ?? 0);
                        $('#totalInvestment').val(formatMoney(values.promoHeader.prevInvestment, 0) ?? 0)
                        $('#roi').val(formatMoney(values.promoHeader.prevRoi, 2) ?? 0);
                        $('#costRatio').val(formatMoney(values.promoHeader.prevCostRatio, 2) ?? 0);

                        $('#baselineSalesRecon').val(formatMoney(values.promoHeader.normalSales, 0) ?? 0);
                        $('#incrementSalesRecon').val(formatMoney(values.promoHeader.incrSales, 0) ?? 0);
                        $('#investmentRecon').val(formatMoney(values.promoHeader.investment, 0) ?? 0);
                        $('#actualDNClaimed').val(formatMoney(values.promoHeader.invoiced, 0) ?? 0);
                        $('#notes_message').val(values.promoHeader.approvalNotes);
                        calcPromo();

                        //Detail Attribute
                        if(values.channels) {
                            for (let i = 0; i < values.channels.length; i++) {
                                if (values.channels[i].flag)  {
                                    channelId = values.channels[i].id;
                                }
                            }
                        }

                        if(values.subChannels) {
                            for (let i = 0; i < values.subChannels.length; i++) {
                                if (values.subChannels[i].flag)  {
                                    subChannelId = values.subChannels[i].id;
                                }
                            }
                        }

                        if(values.accounts) {
                            for (let i = 0; i < values.accounts.length; i++) {
                                if (values.accounts[i].flag)  {
                                    accountId = values.accounts[i].id;
                                }
                            }
                        }

                        if(values.subAccounts) {
                            for (let i = 0; i < values.subAccounts.length; i++) {
                                if (values.subAccounts[i].flag)  {
                                    subAccountId = values.subAccounts[i].id;
                                }
                            }
                        }

                        //Attribute Region
                        let longDescRegion = '';
                        for (let i = 0; i < values.regions.length; i++) {
                            if (values.regions[i].flag) {
                                arr_region.push(values.regions[i].id);
                                longDescRegion += '<span className="fw-bold text-sm-left">' + values.regions[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                            }
                        }
                        $('#card_list_region').html(longDescRegion);

                        //Attribute Brand
                        let longDescBrand = '';
                        for (let i = 0; i < values.brands.length; i++) {
                            if (values.brands[i].flag) {
                                arr_brand.push(values.brands[i].id);
                                longDescBrand += '<span className="fw-bold text-sm-left">' + values.brands[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                            }
                        }
                        $('#card_list_brand').html(longDescBrand);

                        // Attribuate Mechanism
                        let nourut = 0;
                        dt_mechanism.clear().draw();
                        arr_mechanism = [];
                        for (let i = 0; i < values.mechanisms.length; i++) {
                            nourut += 1;
                            let dMechanism = [];
                            dMechanism["no"] = nourut;
                            dMechanism["mechanism"] = values.mechanisms[i].mechanism;
                            dMechanism["notes"] = values.mechanisms[i].notes;
                            dMechanism["productId"] = values.mechanisms[i].productId;
                            dMechanism["product"] = values.mechanisms[i].product;
                            dMechanism["brandId"] = values.mechanisms[i].brandId;
                            dMechanism["brand"] = values.mechanisms[i].brand;
                            dMechanism["mechanismId"] = values.mechanisms[i].mechanismId;
                            let mechanism = {};
                            mechanism.id = values.mechanisms[i].mechanismId;
                            mechanism.mechanism = values.mechanisms[i].mechanism;
                            mechanism.notes = values.mechanisms[i].notes;
                            mechanism.productId = values.mechanisms[i].productId;
                            mechanism.product = values.mechanisms[i].product;
                            mechanism.brandId = values.mechanisms[i].brandId;
                            mechanism.brand = values.mechanisms[i].brand;

                            arr_mechanism.push(mechanism);

                            dt_mechanism.row.add(dMechanism).draw();
                        }

                        //Attribute SKU
                        let longDescSKU = '';
                        for (let i = 0; i < values.skus.length; i++) {
                            if (values.skus[i].flag) {
                                arr_sku.push(values.skus[i].id);
                                longDescSKU += '<span className="fw-bold text-sm-left">' + values.skus[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                            }
                        }
                        $('#card_list_sku').html(longDescSKU);


                        if (values.attachments) {
                            let fileSource = "";
                            values.attachments.forEach((item, index, arr) => {
                                if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                    $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).text(item.fileName).attr('title', item.fileName);
                                    $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).addClass('form-control-solid-bg');
                                    $('#attachment' + parseInt(item.docLink.replace('row', ''))).attr('disabled', true);
                                    $('#btn_download' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                                    $('#btn_delete' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                                }
                            });
                        }
                    } else if (result.errCode === 404) {
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
                            window.location.href = '/promo/recon-send-back';
                        });
                    }
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
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

const getSubCategory = (categoryId) => {
    return new Promise((resolve, reject) => {
        let url = "/promo/recon-send-back/list/sub-category";
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {categoryId: categoryId},
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
                listSubCategory = data;
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
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getChannel = () => {
    return new Promise((resolve, reject) => {
        let url = "/promo/recon-send-back/list/edit/channel";
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoId: promoId},
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
                $('#channelId').select2({
                    placeholder: "Select a Channel",
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

const getSubChannel = (channelId) => {
    return new Promise((resolve, reject) => {
        let url = "/promo/recon-send-back/list/edit/sub-channel";
        let listChannel = [];
        listChannel.push(channelId);
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoId: promoId, channelId: channelId, dataChannel : JSON.stringify(listChannel)},
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
                $('#subChannelId').select2({
                    placeholder: "Select a Sub Channel",
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

const getAccount = (subChannelId) => {
    return new Promise((resolve, reject) => {
        let url = "/promo/recon-send-back/list/edit/account";
        let listSubChannel = [];
        listSubChannel.push(subChannelId);
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoId: promoId, subChannelId: subChannelId, dataSubChannel: JSON.stringify(listSubChannel)},
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
                $('#accountId').select2({
                    placeholder: "Select an Account",
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

const getSubAccount = (accountId) => {
    return new Promise((resolve, reject) => {
        let url = "/promo/recon-send-back/list/edit/sub-account";
        let listAccount = [];
        listAccount.push(accountId);
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoId: promoId, accountId: accountId, dataAccount: JSON.stringify(listAccount)},
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
                $('#subAccountId').select2({
                    placeholder: "Select a Sub Account",
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

const getCategoryId = (categoryShortDescEnc) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon-send-back/category/category-desc",
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

const getActivity = (paramSubCategoryId) => {
    return new Promise((resolve, reject) => {
        let isDeleted = ((createOn < convertStringToDate(appCutOffHierarchy)) ? "all" : "0");
        $.ajax({
            url         : "/promo/recon-send-back/list/activity",
            type        : "GET",
            data        : {subCategoryId: paramSubCategoryId, isDeleted: isDeleted},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                if (subCategoryId.toString() === paramSubCategoryId.toString()) {
                    data = [{
                        id: activityId,
                        text: activityLongDesc
                    }];
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    if (result.data[j].id !== id) {
                        data.push({
                            id: result.data[j].id,
                            text: result.data[j].longDesc,
                        });
                    }
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
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getSubActivity = (paramActivityId) => {
    return new Promise((resolve, reject) => {
        let isDeleted = ((createOn < convertStringToDate(appCutOffHierarchy)) ? "all" : "0");
        $.ajax({
            url         : "/promo/recon-send-back/list/sub-activity",
            type        : "GET",
            data        : {activityId: paramActivityId, isDeleted: isDeleted},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                if (activityId.toString() === paramActivityId.toString()) {
                    data = [{
                        id: subActivityId,
                        text: subActivityLongDesc,
                        allowEdit: allowedit
                    }];
                }
                for (let j = 0, len = result.data.length; j < len; ++j){
                    if (result.data[j].id !== subActivityId) {
                        data.push({
                            id: result.data[j].id,
                            text: result.data[j].subActivity,
                            allowEdit: result.data[j].allowEdit
                        });
                    }
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
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getInvestmentType = (subActivityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon-send-back/investment-type",
            type        : "GET",
            data        : {subActivityId: subActivityId},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let elInvestmentType = $('#investmentType');
                investmentTypeId = 0;
                elInvestmentType.val('');
                if(result.data.length>0) {
                    investmentTypeId = result.data[0].investmentTypeId;
                    elInvestmentType.val(result.data[0].investmentTypeRefId + " - " + result.data[0].investmentTypeDesc);
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

const getConfigRoiCR = (subActivityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon-send-back/config-roi-cr",
            type        : "GET",
            data        : {subActivityId: subActivityId},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                minROI = 0;
                maxROI = 0;
                minCR = 0;
                maxCR = 0;
                if (!result.error) {
                    minROI = result.data.minimumROI;
                    maxROI = result.data.maksimumROI;
                    minCR = result.data.minimumCostRatio;
                    maxCR = result.data.maksimumCostRatio;
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

const getCutOff = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/recon-send-back/cut-off",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                appCutOffMechanism = result.app_cutoff_mechanism;
                appCutOffHierarchy = result.app_cutoff_hierarchy;
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
    }).catch((e) => {
        console.log(e);
    });
}

const clearBudgetSource = () => {
    allocationId = null;
    $('#allocationRefId').val('');
    $('#allocationDesc').val('');
    $('#budgetDeployed').val('0');
    $('#budgetRemaining').val('0');
}

const clearAttribute = () => {
    // if(newPeriodMechanism()){
    //     dt_mechanism.clear().draw();
    //     arr_brand = []
    //     $('#card_list_brand').html('');
    //     arr_sku = []
    //     $('#card_list_sku').html('');
    //     $('#mechanisme1').val('');
    // }
}

const convertStringToDate = (strDate) => {
    let year = strDate.substr(0,4);
    let month = strDate.substr(4,2);
    let date = strDate.substr(6,2);

    return new Date(year + '-' + month + '-' + date);
}

const calcPromo = () => {
    let costRatio =  parseFloat($('#costRatio').val().replace(/,/g, ''));
    let normalSales = parseFloat($('#baselineSales').val().replace(/,/g, ''));
    let incrementSales = parseFloat($('#incrementSales').val().replace(/,/g, ''));

    let totalSales = normalSales + incrementSales;

    let normalSalesRecon = parseFloat($('#baselineSalesRecon').val().replace(/,/g, ''));
    let incrementSalesRecon = parseFloat($('#incrementSalesRecon').val().replace(/,/g, ''));
    let investmentRecon = parseFloat($('#investmentRecon').val().replace(/,/g, ''));
    let totalSalesRecon = normalSalesRecon + incrementSalesRecon;
    let totalInvestmentRecon = investmentRecon;

    let crRecon = 0;
    let roiRecon = 0;
    if (totalSalesRecon !== 0) crRecon = (totalInvestmentRecon / totalSalesRecon) * 100;
    if (investmentRecon !== 0) roiRecon = ((incrementSalesRecon - investmentRecon) / investmentRecon) * 100;

    let estimatedInvestment = ((costRatio / 100) * totalSalesRecon);

    $('#totalSales').val(formatMoney(totalSales, 0));
    $('#totalSalesRecon').val(formatMoney(totalSalesRecon, 0));
    $('#estimatedInvestment').val(formatMoney(estimatedInvestment, 0));
    $('#roiRecon').val(formatMoney(roiRecon, 2));
    $('#costRatioRecon').val(formatMoney(crRecon, 2));
}

const ActivityDescFormula = () => {
    let strMonth = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Des'];

    let elStartPromo = $('#startPromo');

    let elRowShowDesc = $('#row_show_desc');
    let elSubActivityId = $('#subActivityId');

    let startPromoYear = new Date(elStartPromo.val()).getFullYear();
    let startPromoMonth = new Date(elStartPromo.val()).getMonth();

    let strStartPromoMonth = strMonth[startPromoMonth];

    let periode_desc = [strStartPromoMonth, startPromoYear.toString()].join(' ');
    let subActivityDesc = '';

    if (!elRowShowDesc.hasClass('d-none')) elRowShowDesc.addClass('d-none');

    if (elSubActivityId.val()) {
        subActivityDesc = elSubActivityId.select2('data')[0].text;

        let strActivityName = subActivityDesc + ' ' + periode_desc;
        $('#activityDesc').val(strActivityName)
        $('#show_desc').html(strActivityName)
        document.getElementById('activityDesc').focus();
        document.getElementById('activityDesc').select();

        if (elRowShowDesc.hasClass('d-none')) elRowShowDesc.removeClass('d-none');
    }
}

const editableConfig = (values) => {
    $('#subCategoryId').attr('readonly', values.subCategory);
    $('#activityId').attr('readonly', values.activity);
    $('#subActivityId').attr('readonly', values.subActivity);
    $('#activityDesc').attr('readonly', values.activityName);
    $('#initiatorNotes').attr('readonly', values.initiatorNotes);
    $('#incrementSalesRecon').attr('readonly', values.incrSales);
    $('#investmentRecon').attr('readonly', values.investment);
    $('#channelId').attr('readonly', values.channel);
    $('#subChannelId').attr('readonly', values.subChannel);
    $('#accountId').attr('readonly', values.account);
    $('#subAccountId').attr('readonly', values.subAccount);

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
    configRegion = values.region;
    configBrand = values.brand;
    configSku = values.SKU;
    configMechanism = values.mechanism;
    configStartPromo = values.startPromo;
    configEndPromo = values.endPromo;

    // button attribute
    if (configRegion === false || configBrand === false || configSku === false || configMechanism === false) {
        $('#btn_attribute').removeClass('d-none');
    } else {
        $('#btn_attribute').addClass('d-none');
    }

    //attachment
    if (values.attachment == true) {
        $('.input_file').attr('disabled', true);
        $('.review_file_label').addClass('form-control-solid-bg');
        $('.btn_delete').attr('disabled', true);
    }
}

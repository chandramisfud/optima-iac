'use strict';

let validator, method, newMechanismMethod;
let tsCoding, promoPlanningRefId, promoPlanningId, allocationId;
let uuid, categoryShortDescEnc, promoId, promoRefId, createOn, investmentTypeId, appCutOffMechanism, appCutOffHierarchy, statusApprovalCode, isCancel, isClose, isCancelLocked;
let entityId, entityShortDesc, distributorId, categoryId, categoryShortDesc, subCategoryId, activityId, subActivityId, channelId, subChannelId, accountId, subAccountId;
let minROI, maxROI, minCR, maxCR;
let arr_region = [], arr_brand = [], arr_sku = [], arr_sku_temp = [], arr_mechanism = [];
let dt_mechanism, dialerObject;

let swalTitle = "Promo Creation";

let targetAttribute = document.querySelector(".card_attribute");
let blockUIAttribute = new KTBlockUI(targetAttribute, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetModal = document.querySelector(".modal-content");
let blockUIModal = new KTBlockUI(targetModal, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetAttachment = document.querySelector(".card_attachment");
let blockUIAttachment = new KTBlockUI(targetAttachment, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let url_str = new URL(window.location.href);
method = url_str.searchParams.get("method");
promoId = url_str.searchParams.get("promoId");
categoryShortDescEnc = url_str.searchParams.get("c");
uuid = generateUUID(5);

const form = document.getElementById('form_promo');

const crossYear = function () {
    return {
        validate: function (input) {
            const yearPeriod = $('#period').val()
            const value = input.value;
            const inputYear = new Date(value).getFullYear().toString();

            let dtStart = new Date($('#startPromo').val()).getTime();
            let dtNow = new Date().getTime();

            if (method === 'update') {
                if ($('#startPromo').is('[readonly]')) {
                    return {
                        valid: true,
                    };
                } else {
                    if (yearPeriod === inputYear) {
                        if (Math.floor((dtNow - dtStart) / (24 * 3600 * 1000)) <= 0) {
                            return {
                                valid: true,
                            };
                        } else {
                            return {
                                valid: false
                            }
                        }
                    } else {
                        return {
                            valid: false
                        }
                    }
                }
            } else {
                if (yearPeriod === inputYear) {
                    return {
                        valid: true,
                    };
                } else {
                    return {
                        valid: false
                    }
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

const roiMinMax = function () {
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

const crMinMax = function () {
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

FormValidation.validators.crossYear = crossYear;
FormValidation.validators.investmentMin = investmentMin;
FormValidation.validators.roiMinMax = roiMinMax;
FormValidation.validators.crMinMax = crMinMax;

validator = FormValidation.formValidation(form, {
    fields: {
        promoPlanningRefId: {
            validators: {
                notEmpty: {
                    message: "Please select a promo planning"
                },
            }
        },
        allocationRefId: {
            validators: {
                notEmpty: {
                    message: "Please select a budget source"
                },
            }
        },
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
        startPromo: {
            validators: {
                // check cross Year
                crossYear: {
                    message: 'Cross year is not allowed, start date must be older than end date'
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
        investment: {
            validators: {
                // check investment
                investmentMin: {
                    message: 'Please fill in minimum investment value of 3,000'
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

$(document).ready(function () {
    $('form').submit(false);

    KTDialer.createInstances();

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
    }).mask("#baselineSales, #incrementSales, #totalSales, #totalInvestment, #roi, #costRatio");

    dt_mechanism = $('#dt_mechanism').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "30vh",
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
            {
                targets: 3,
                title: 'productId',
                data: 'productId',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'product',
                data: 'product',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'brandId',
                data: 'brandId',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'brand',
                data: 'brand',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'mechanismId',
                data: 'mechanismId',
                visible: false,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    blockUI.block();
    blockUIAttribute.block();
    blockUIAttachment.block();
    disableButtonSave();
    if (method === 'update') {
        $('#period').attr('readonly', true);
        $('#dialer_period button').remove();
        getCategoryId(categoryShortDescEnc).then(function () {
            Promise.all([getCutOff(), getChannel()]).then(async () => {
                await getData(promoId);

                $('#txt_info_method').text('Edit ' + promoRefId);
                calcPromo();

                await getSubCategory();
                await $('#subCategoryId').val(subCategoryId).trigger('change.select2');

                await getActivity(subCategoryId);
                await $('#activityId').val(activityId).trigger('change.select2');

                await getSubActivity(activityId);
                $('#subActivityId').val(subActivityId).trigger('change.select2');

                await getInvestmentType(subActivityId);

                $('#channelId').val(channelId).trigger('change.select2');

                await getSubChannel(channelId);
                await $('#subChannelId').val(subChannelId).trigger('change.select2');

                await getAccount(subChannelId);
                await $('#accountId').val(accountId).trigger('change.select2');

                await getSubAccount(accountId);
                $('#subAccountId').val(subAccountId).trigger('change.select2');

                if (isClose || isCancel || statusApprovalCode === 'TP2' || isCancelLocked) {
                    disableButtonSave();
                    let txtReason = ''
                    if (statusApprovalCode === 'TP2') txtReason = ', because this promo is reconciliation';
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
                        window.location.href = '/promo/creation';
                    });
                }

                disabledActivityPeriod();
                enableButtonSave();
                blockUI.release();
                blockUIAttribute.release();
                blockUIAttachment.release();
            });
        });
    } else if (method === "createPromo") {
        createOn = new Date();
        let dialerElement = document.querySelector("#dialer_period");
        dialerObject = new KTDialer(dialerElement, {
            step: 1,
        });

        getCategoryId(categoryShortDescEnc).then(function () {
            Promise.all([getCutOff(), getChannel()]).then(async () => {
                await getSubCategory();
                promoPlanningId = url_str.searchParams.get("promoPlanId");
                await getDataPlanningSource(promoPlanningId);
                enableButtonSave();
                blockUI.release();
                blockUIAttribute.release();
                blockUIAttachment.release();
            });
        });
    } else {
        createOn = new Date();
        let dialerElement = document.querySelector("#dialer_period");
        dialerObject = new KTDialer(dialerElement, {
            step: 1,
        });

        $('#startPromo, #endPromo').flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
        });
        getCategoryId(categoryShortDescEnc).then(function () {
            Promise.all([getCutOff(), getChannel()]).then( async () => {
                await getSubCategory();
                if (method === 'createPromo') {
                    promoPlanningId = url_str.searchParams.get("promoPlanId");
                    await getDataPlanningSource(promoPlanningId);
                }
                enableButtonSave();
                blockUI.release();
                blockUIAttribute.release();
                blockUIAttachment.release();
            });
        });
    }
});

$('#period').on('blur', function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

$('#period').on('change', function () {
    let period = this.value;
    let startDate = formatDate(new Date(period, 0, 1));
    let endDate = formatDate(new Date(period, 11, 31));
    let startPromo = $('#startPromo');
    let endPromo = $('#endPromo');

    startPromo.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        defaultDate: startDate
    });

    endPromo.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        defaultDate: endDate
    });
    validator.revalidateField('startPromo')
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
    }
    ActivityDescFormula();
    clearAttribute();
    await backDate();
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

$('#btn_search_promo_planning').on('click', function () {
    $('#dt_budget_source_list_search').val('');
    dt_source_planning_list.clear().draw();
    let period = $('#period').val();
    let url = "/promo/creation/list/source-planning?period=" + period;
    dt_source_planning_list.ajax.url(url).load();
    $('#modal_source_planning_list').modal('show');
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
        subCategoryId: $('#subCategoryId').val(),
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
        url: "/promo/creation/list/source-budget",
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

$('#normalSales, #investment, #incrementSales').on('keyup', async function () {
    await calcPromo();
    await validator.revalidateField('roi');
    await validator.revalidateField('costRatio');
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
    }
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
});

$('#activityId').on('change', async function () {
    blockUI.block();
    let elSubActivity = $('#subActivityId');
    elSubActivity.empty();
    if ($(this).val()) await getSubActivity($(this).val());
    clearAttribute();
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
    }
    ActivityDescFormula();
    clearAttribute();
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
                    url: "/promo/creation/attachment-delete",
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

$('#btn_save').on('click', function () {
    blockUI.block();
    blockUIAttribute.block();
    blockUIAttachment.block();
    let e = document.querySelector("#btn_save");
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;

    validator.validate().then(async function (status) {
        if (status === "Valid") {
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
                    blockUIAttachment.release();
                });
            }

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
                    if (result.value) {
                        modifyReason = result.value;
                        await saveData(modifyReason);
                    } else {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                        blockUI.release();
                        blockUIAttribute.release();
                        blockUIAttachment.release();
                    }
                });
            } else {
                await saveData("");
            }
        } else {
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
            blockUI.release();
            blockUIAttribute.release();
            blockUIAttachment.release();
        }
    });
});

const getCategoryId = (categoryShortDescEnc) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/category/category-desc",
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

const getSubCategory = () => {
    return new Promise((resolve, reject) => {
        let isDeleted = ((createOn < convertStringToDate(appCutOffHierarchy)) ? "all" : "0");
        let url = "/promo/creation/list/sub-category";
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {categoryId: categoryId, isDeleted: isDeleted},
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

const getActivity = (subCategoryId) => {
    return new Promise((resolve, reject) => {
        let isDeleted = ((createOn < convertStringToDate(appCutOffHierarchy)) ? "all" : "0");
        $.ajax({
            url         : "/promo/creation/list/activity",
            type        : "GET",
            data        : {subCategoryId: subCategoryId, isDeleted: isDeleted},
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

const getSubActivity = (activityId) => {
    return new Promise((resolve, reject) => {
        let isDeleted = ((createOn < convertStringToDate(appCutOffHierarchy)) ? "all" : "0");
        $.ajax({
            url         : "/promo/creation/list/sub-activity",
            type        : "GET",
            data        : {activityId: activityId, isDeleted: isDeleted},
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
        let url = ((method === 'update') ? "/promo/creation/list/edit/channel" : "/promo/creation/list/channel");
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
        let url = ((method === "update") ? "/promo/creation/list/edit/sub-channel" : "/promo/creation/list/sub-channel");
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
        let url = ((method === "update") ? "/promo/creation/list/edit/account" : "/promo/creation/list/account");
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
        let url = ((method === "update") ? "/promo/creation/list/edit/sub-account" : "/promo/creation/list/sub-account");
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

const getInvestmentType = (subActivityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/investment-type",
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

const calcPromo = () => {
    let normalSales = parseFloat($('#baselineSales').val().replace(/,/g, ''));
    let incrementSales = parseFloat($('#incrementSales').val().replace(/,/g, ''));
    let investment = parseFloat($('#investment').val().replace(/,/g, ''));
    let totalSales = normalSales + incrementSales;
    let totalInvestment = investment;

    let cr = 0;
    let roi = 0;
    if (totalSales !== 0) cr = (totalInvestment / totalSales) * 100;
    if (investment !== 0) roi = ((incrementSales - investment) / investment) * 100;

    $('#totalSales').val(formatMoney(totalSales, 0));
    $('#totalInvestment').val(formatMoney(totalInvestment, 0));
    $('#roi').val(formatMoney(roi, 2));
    $('#costRatio').val(formatMoney(cr, 2));
}

const getConfigRoiCR = (subActivityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/config-roi-cr",
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
            url: "/promo/creation/cutoff",
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

const clearAttribute = () => {
    if(newPeriodMechanism()){
        dt_mechanism.clear().draw();
        arr_brand = []
        $('#card_list_brand').html('');
        arr_sku = []
        $('#card_list_sku').html('');
        $('#mechanisme1').val('');
    }
}

const clearBudgetSource = () => {
    allocationId = null;
    $('#allocationRefId').val('');
    $('#allocationDesc').val('');
    $('#budgetDeployed').val('0');
    $('#budgetRemaining').val('0');
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

const getLatePromoDays = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/creation/late-promo-days",
            type: "GET",
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    return resolve(result.data.days);
                } else {
                    return resolve(0);
                }
            },
            complete: function () {
            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return 0;
            }
        });
    }).catch((e) => {
        console.log(e);
        return 0;
    });
}

const getPromoExist = () => {
    return new Promise((resolve, reject) => {
        let exist = false;
        if (method === 'update') {
            resolve(exist);
        } else {
            let data = {
                period: $('#period').val(),
                activityDesc: $('#activityDesc').val(),
                channelId: $('#channelId').val(),
                subAccountId: $('#subAccountId').val(),
                startPromo: $('#startPromo').val(),
                endPromo: $('#endPromo').val()
            }

            $.ajax({
                url: "/promo/creation/exist",
                type: "GET",
                data: data,
                dataType: 'json',
                async: true,
                success: async function (result) {
                    if (!result.error) {
                        let d1 = formatDateOptima(result.data.startPromo);
                        let d2 = formatDateOptima(result.data.endPromo)
                        await swal.fire({
                            title: 'Promo ID with similar data already exists',
                            text: '',
                            html:
                                "<div class='row'> \
                                    <div class='col-sm-5 text-start'>Promo Number</div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + result.data.refId + "</div> \
                                    <div class='col-sm-5 text-start'>Activity Period </div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + d1 + ' to ' + d2 + "</div> \
                                    <div class='col-sm-5 text-start'>Channel </div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + result.data.channel + "</div> \
                                    <div class='col-sm-5 text-start'>Sub Account </div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + result.data.subAccount + "</div> \
                                    <div class='col-sm-5 text-start'>Activity Name </div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + result.data.activityDesc + "</div>\
                                    <div class='col-sm-5 text-start mb-5'></div><div class='col-sm-1 mb-5'></div><div class='col-sm-6 text-start mb-5'></div>\
                                </div>",

                            type: 'warning',
                            showCancelButton: true,
                            confirmButtonText: 'SAVE',
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima", cancelButton: "btn btn-optima"}
                        }).then(function (result) {
                            if (result.isConfirmed) {
                                return resolve(false);
                            } else {
                                return resolve(true);
                            }
                        });
                    } else {
                        return resolve(false);
                    }

                },
                complete: function () {

                },
                error: function (jqXHR, textStatus, errorThrown)
                {
                    console.log(jqXHR.responseText);
                    return reject(errorThrown);
                }
            });
        }
    }).catch((e) => {
        console.log(e);
    });
}

const backDate = async function () {
    let elStartPromo = $('#startPromo');
    if (method !== "update") {
        let latePromoDays = await getLatePromoDays();


        let dtStart = new Date(elStartPromo.val()).getTime();
        let dtNow = new Date().getTime();
        let diffDays = Math.ceil((dtStart - dtNow) / (1000 * 60 * 60 * 24));
        if (diffDays < latePromoDays) {
            elStartPromo.next().css('background-color', '#f7f8fa !important');
            $('#endPromo').next().css('background-color', '#f7f8fa !important');
        } else {
            elStartPromo.next().css('background-color', '#fff !important');
            $('#endPromo').next().css('background-color', '#fff !important');
        }
    } else {
        elStartPromo.next().css('background-color', '#fff !important');
        $('#endPromo').next().css('background-color', '#fff !important');
    }
}

const saveData = (modifyReason) => {
    let e = document.querySelector("#btn_save");
    //cek Promo Exist
    getPromoExist().then( async (value) => {
        if (!value) {
            if (method !== "update") {
                let latePromoDays = await getLatePromoDays();

                let elStartPromo = $('#startPromo');
                let dtStart = new Date(elStartPromo.val()).getTime();
                let dtNow = new Date().getTime();
                let diffDays = Math.ceil((dtStart - dtNow) / (1000 * 60 * 60 * 24));
                validator.addField('initiatorNotes', {
                    validators: {
                        notEmpty: {
                            message: "Please enter a notes"
                        },
                    }
                });

                if (diffDays < latePromoDays) {
                    swal.fire({
                        title: 'Your Activity Period is Backdate',
                        input: 'text',
                        inputAttributes: {
                            autocapitalize: 'off'
                        },
                        type: 'warning',
                        html:
                            '<span class="text-danger">' + formatDateOptima(elStartPromo.val() )+ '</span>' +
                            ' to ' + formatDateOptima($('#endPromo').val()) +
                            '<br><b>please fill in the late submission reason<br>in the following box</b>'
                        ,
                        showCancelButton: true,
                        confirmButtonText: 'Save',
                        showLoaderOnConfirm: true,
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        reverseButtons: true,
                        customClass: {confirmButton: "btn btn-optima", cancelButton: "btn btn-optima"},
                        preConfirm: (value) => {
                            if (value === "") {
                                Swal.showValidationMessage(
                                    `Please fill in the late submission reason`
                                )
                            }
                        }
                    }).then(function (result) {
                        if (result.isConfirmed) {
                            $('#initiatorNotes').val(result.value)
                            $('#startPromo').addClass('form-control-solid-bg')
                            $('#endPromo').addClass('form-control-solid-bg')
                            $('#startPromo, #endPromo').flatpickr({
                                altFormat: "d-m-Y",
                                altInput: true,
                                allowInput: true,
                                dateFormat: "Y-m-d",
                                disableMobile: "true",
                            });

                            validator.revalidateField('initiatorNotes');
                            submitSave(modifyReason);
                        } else if (result.dismiss === 'cancel') {

                            $('#startPromo').addClass('form-control-solid-bg')
                            $('#endPromo').addClass('form-control-solid-bg')
                            $('#startPromo, #endPromo').flatpickr({
                                altFormat: "d-m-Y",
                                altInput: true,
                                allowInput: true,
                                dateFormat: "Y-m-d",
                                disableMobile: "true",
                            });

                            e.setAttribute("data-kt-indicator", "off");
                            e.disabled = !1;
                            blockUI.release();
                            blockUIAttribute.release();
                            blockUIAttachment.release();
                        }
                    });
                } else {
                    elStartPromo.removeClass('form-control-solid-bg')
                    $('#endPromo').removeClass('form-control-solid-bg')
                    $('#startPromo, #endPromo').flatpickr({
                        altFormat: "d-m-Y",
                        altInput: true,
                        allowInput: true,
                        dateFormat: "Y-m-d",
                        disableMobile: "true",
                    });

                    validator.removeField('initiatorNotes');
                    await submitSave(modifyReason);
                }
            } else {
                await submitSave(modifyReason);
            }
        } else {
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
            blockUI.release();
            blockUIAttribute.release();
            blockUIAttachment.release();
        }
    });
}

const submitSave = async function (modifyReason) {
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
    formData.append('uuid', uuid);
    formData.append('promoId', promoId);
    formData.append('promoPlanId', promoPlanningId);
    formData.append('allocationId', allocationId);
    formData.append('entityShortDesc', entityShortDesc);
    formData.append('categoryId', categoryId);
    formData.append('categoryShortDesc', categoryShortDesc);
    formData.append('dataChannel', JSON.stringify(listChannel));
    formData.append('dataSubChannel', JSON.stringify(listSubChannel));
    formData.append('dataAccount', JSON.stringify(listAccount));
    formData.append('dataSubAccount', JSON.stringify(listSubAccount));
    formData.append('dataRegion', JSON.stringify(arr_region));
    formData.append('dataBrand', JSON.stringify(arr_brand));
    formData.append('dataSKU', JSON.stringify(arr_sku));
    formData.append('dataMechanism', JSON.stringify(arr_mechanism));
    formData.append('modifReason', modifyReason);
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
    formData.delete('attachment5');
    formData.delete('attachment6');
    formData.delete('attachment7');

    let url = "/promo/creation/save";
    if (method === "update") {
        url = "/promo/creation/update";
        formData.append('promoId', promoId);
    }

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
        success: function (result) {
            if (!result.error) {
                if (method !== "update") {
                    if (result.attachment2 !== 'No File Upload attachment2') {
                        let elInfo2 = $('#info2');
                        if (result.attachment2 === 'Upload Successfully') {
                            elInfo2.removeClass('invisible').addClass('visible');
                            elInfo2.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo2.removeClass('invisible').addClass('visible');
                            elInfo2.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                    if (result.attachment3 !== 'No File Upload attachment3') {
                        let elInfo3 = $('#info3');
                        if (result.attachment3 === 'Upload Successfully') {
                            elInfo3.removeClass('invisible').addClass('visible');
                            elInfo3.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo3.removeClass('invisible').addClass('visible');
                            elInfo3.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                    if (result.attachment4 !== 'No File Upload attachment4') {
                        let elInfo4 = $('#info4');
                        if (result.attachment4 === 'Upload Successfully') {
                            elInfo4.removeClass('invisible').addClass('visible');
                            elInfo4.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo4.removeClass('invisible').addClass('visible');
                            elInfo4.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                    if (result.attachment5 !== 'No File Upload attachment5') {
                        let elInfo5 = $('#info5');
                        if (result.attachment5 === 'Upload Successfully') {
                            elInfo5.removeClass('invisible').addClass('visible');
                            elInfo5.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo5.removeClass('invisible').addClass('visible');
                            elInfo5.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                    if (result.attachment6 !== 'No File Upload attachment6') {
                        let elInfo6 = $('#info6');
                        if (result.attachment6 === 'Upload Successfully') {
                            elInfo6.removeClass('invisible').addClass('visible');
                            elInfo6.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo6.removeClass('invisible').addClass('visible');
                            elInfo6.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                    if (result.attachment7 !== 'No File Upload attachment7') {
                        let elInfo7 = $('#info7');
                        if (result.attachment7 === 'Upload Successfully') {
                            elInfo7.removeClass('invisible').addClass('visible');
                            elInfo7.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo7.removeClass('invisible').addClass('visible');
                            elInfo7.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                }
                Swal.fire({
                    title: swalTitle,
                    text: result.message,
                    icon: "success",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    window.location.href = '/promo/creation';
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
            blockUIAttachment.release();
        },
        error: function (jqXHR) {
            console.log(jqXHR)
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
        url: "/promo/creation/attachment-upload",
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

const getData = (promoId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/creation/data/id",
            type: "GET",
            data: {id: promoId},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    if(result.errCode === 200){
                        let values = result.data;

                        statusApprovalCode = values.promoHeader.statusApprovalCode;
                        entityShortDesc = values.promoHeader.principalShortDesc;
                        isCancel = values.promoHeader.isCancel;
                        isClose = values.promoHeader.isClose;
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
                        isCancelLocked = values.promoHeader.isCancelLocked;
                        createOn = new Date(values.promoHeader.createOn);

                        for (let i = 0; i < values.channels.length; i++) {
                            if (values.channels[i].flag) channelId = values.channels[i].id;
                        }
                        for (let i = 0; i < values.subChannels.length; i++) {
                            if (values.subChannels[i].flag) subChannelId = values.subChannels[i].id;
                        }
                        for (let i = 0; i < values.accounts.length; i++) {
                            if (values.accounts[i].flag) accountId = values.accounts[i].id;
                        }
                        for (let i = 0; i < values.subAccounts.length; i++) {
                            if (values.subAccounts[i].flag) subAccountId = values.subAccounts[i].id;
                        }

                        $('#period').val(new Date(values.promoHeader.startPromo).getFullYear());
                        $('#promoPlanningRefId').val(values.promoHeader.promoPlanRefId);
                        $('#allocationRefId').val(values.promoHeader.allocationRefId);
                        $('#allocationDesc').val(values.promoHeader.allocationDesc);
                        $('#tsCode').val(values.promoHeader.tsCoding);
                        $('#entity').val(values.promoHeader.principalName);
                        $('#distributor').val(values.promoHeader.distributorName);
                        $('#budgetDeployed').val(formatMoney(values.promoHeader.budgetAmount, 0));
                        $('#budgetRemaining').val(formatMoney(values.promoHeader.remainingBudget, 0));
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

                        $('#activityDesc').val(values.promoHeader.activityDesc);
                        $('#initiatorNotes').val(values.promoHeader.initiator_notes);
                        $('#baselineSales').val(formatMoney(values.promoHeader.normalSales, 0));
                        $('#incrementSales').val(formatMoney(values.promoHeader.incrSales, 0));
                        $('#investment').val(formatMoney(values.promoHeader.investment, 0));
                        $('#totalInvestment').val(formatMoney(values.promoHeader.investment, 0));
                        $('#roi').val(formatMoney(values.promoHeader.roi, 2));
                        $('#costRatio').val(formatMoney(values.promoHeader.costRatio, 2));

                        //Attribute Region
                        let longDescRegion = '';
                        for (let i = 0; i < values.regions.length; i++) {
                            if (values.regions[i].flag) {
                                arr_region.push(values.regions[i].id);
                                longDescRegion += '<span class="fw-bold text-sm-left">' + values.regions[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                            }
                        }
                        $('#card_list_region').html(longDescRegion);

                        //Attribute Brand
                        let longDescBrand = '';
                        for (let i = 0; i < values.brands.length; i++) {
                            if (values.brands[i].flag) {
                                arr_brand.push(values.brands[i].id);
                                longDescBrand += '<span class="fw-bold text-sm-left">' + values.brands[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                            }
                        }
                        $('#card_list_brand').html(longDescBrand);

                        //Attribute SKU
                        let longDescSKU = '';
                        for (let i = 0; i < values.skus.length; i++) {
                            if (values.skus[i].flag) {
                                arr_sku.push(values.skus[i].id);
                                longDescSKU += '<span class="fw-bold text-sm-left">' + values.skus[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                            }
                        }
                        $('#card_list_sku').html(longDescSKU);

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
                                window.location.href = '/promo/creation';
                            });
                        }
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
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const convertStringToDate = (strDate) => {
    let year = strDate.substr(0,4);
    let month = strDate.substr(4,2);
    let date = strDate.substr(6,2);

    return new Date(year + '-' + month + '-' + date);
}

const disabledActivityPeriod = function () {
    let dtStart = new Date($('#startPromo').val()).getTime();
    let dtNow = new Date().getTime();

    if(Math.floor((dtNow-dtStart)/(24*3600*1000)) >= 0 ){
        $('#startPromo, #endPromo')
            .flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: false,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                clickOpens: false,
            });
        $('#startPromo').attr('readonly', true);
        $('#endPromo').attr('readonly', true);
        $('#startPromo').addClass('form-control-solid-bg');
        $('#endPromo').addClass('form-control-solid-bg');
    } else {
        $('#startPromo').attr('readonly', false);
        $('#endPromo').attr('readonly', false);
        $('#startPromo').removeClass('form-control-solid-bg')
        $('#endPromo').removeClass('form-control-solid-bg')
        $('#startPromo, #endPromo')
            .flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
            });
    }
}

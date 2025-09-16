'use strict';

let validator, method, createOn, investmentTypeId, appCutOffMechanism, appCutOffHierarchy, newMechanismMethod;
let promoPlanningId, promoPlanningRefId, tsCoding, categoryId, subCategoryId, activityId, subActivityId, entityId, distributorId;
let minROI, maxROI, minCR, maxCR;
let channelId, subChannelId, accountId, subAccountId;
let dt_mechanism;
let swalTitle = "Promo Planning";
let dialerObject;

let targetAttribute = document.querySelector(".card_attribute");
let blockUIAttribute = new KTBlockUI(targetAttribute, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetModal = document.querySelector(".modal-content");
let blockUIModal = new KTBlockUI(targetModal, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetSKU = document.querySelector("#attribute_sku");
let blockUISKU = new KTBlockUI(targetSKU, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

const form = document.getElementById('form_promo_planning');
const crossYear = function () {
    return {
        validate: function (input) {
            const yearPeriod = $('#period').val()
            const value = input.value;
            const inputYear = new Date(value).getFullYear().toString();
            if (yearPeriod === inputYear) {
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
                    message: "Please fill in a activity name"
                },
                stringLength: {
                    max: 255,
                    message: 'Activity Name must be less than 255 characters',
                }

            }
        },
        entityId: {
            validators: {
                notEmpty: {
                    message: "Please select an entity"
                },
            }
        },
        distributorId: {
            validators: {
                notEmpty: {
                    message: "Please select a distributor"
                },
            }
        },
        investment: {
            validators: {
                // check investment
                investmentMin: {
                    message: 'Please fill in a minimum investment value of 3,000'
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
})

$(document).ready(function () {
    $('form').submit(false);

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    let elStartEndPromo = $('#startPromo, #endPromo');

    elStartEndPromo.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });
    elStartEndPromo.next().css('background-color', '#fff !important');

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
        scrollY: '35vh',
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
                title: 'Sub brand',
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

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    promoPlanningId = url_str.searchParams.get("promoPlanId");

    blockUI.block();
    blockUIAttribute.block();
    disableButtonSave();
    if (method === 'update' || method === "duplicate") {
        Promise.all([getEntity(), getCutOff(), getChannel()]).then(async () => {
            await getData(promoPlanningId);

            await getSubCategory();
            await $('#subCategoryId').val(subCategoryId).trigger('change.select2');

            await $('#entityId').val(entityId).trigger('change.select2');

            await getDistributor(entityId);
            $('#distributorId').val(distributorId).trigger('change.select2');

            await getActivity(subCategoryId);
            await $('#activityId').val(activityId).trigger('change.select2');

            await getSubActivity(activityId);
            $('#subActivityId').val(subActivityId).trigger('change.select2');

            await getInvestmentType(subActivityId);

            $('#channelId').val(channelId).trigger('change.select2');

            await getSubChannel(channelId)
            await $('#subChannelId').val(subChannelId).trigger('change.select2');

            await getAccount(subChannelId);
            await $('#accountId').val(accountId).trigger('change.select2');

            await getSubAccount(accountId)
            $('#subAccountId').val(subAccountId).trigger('change.select2');

            if (method === 'update') {
                $('#txt_info_method').text('Edit ' + promoPlanningRefId);
            } else {
                $('#txt_info_method').text('Duplicate ' + promoPlanningRefId);
            }
            enableButtonSave();
            blockUI.release();
            blockUIAttribute.release();

            if (method === "update") {
                if (tsCoding) {
                    Swal.fire({
                        title: swalTitle,
                        text: 'Promo Planning ' + promoPlanningRefId + ' can not edit',
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        window.location.href = '/promo/planning';
                    });
                }
            }
        });
    } else {
        createOn = new Date();
        Promise.all([getEntity(), getCutOff(), getChannel()]).then(async () => {
            await getSubCategory();
            enableButtonSave();
            blockUI.release();
            blockUIAttribute.release();
        });
    }
});

$('#subCategoryId').on('change', async function () {
    let elActivity = $('#activityId');
    let elSubActivityId = $('#subActivityId');
    blockUI.block();
    elActivity.empty();
    elSubActivityId.empty();
    if ($(this).val()) await getActivity($(this).val());
    clearAttribute();
    blockUI.release();
    elActivity.val('').trigger('change.select2');
    elSubActivityId.val('').trigger('change.select2');
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

$('#entityId').on('change', async function () {
    blockUI.block();
    let elDistributor = $('#distributorId');
    elDistributor.empty();
    if ($(this).val()) {
        await getDistributor($(this).val());
        elDistributor.val('').trigger('change.select2');
        await getBrand($(this).val());
    }
    elDistributor.val('').trigger('change.select2');
    clearAttribute();
    validator.revalidateField('entityId');
    blockUI.release();
});

$('#distributorId').on('change', async function () {
    validator.revalidateField('distributorId')
});

$('#channelId').on('change', async function () {
    blockUIAttribute.block();
    let elSubChannel = $('#subChannelId');
    let elAccount = $('#accountId');
    let elSubAccount = $('#subAccountId');
    elSubChannel.empty();
    elAccount.empty();
    elSubAccount.empty();
    if ($(this).val()) await getSubChannel($(this).val());
    clearAttribute();
    blockUIAttribute.release();
    elSubChannel.val('').trigger('change.select2');
    elAccount.val('').trigger('change.select2');
    elSubAccount.val('').trigger('change.select2');
    validator.revalidateField('channelId');
    validator.revalidateField('subChannelId');
    validator.revalidateField('accountId');
    validator.revalidateField('subAccountId');
});

$('#subChannelId').on('change', async function () {
    blockUIAttribute.block();
    let elAccount = $('#accountId');
    let elSubAccount = $('#subAccountId');
    elAccount.empty();
    elSubAccount.empty();
    if ($(this).val()) await getAccount($(this).val());
    blockUIAttribute.release();
    elAccount.val('').trigger('change.select2');
    elSubAccount.val('').trigger('change.select2');
    validator.revalidateField('subChannelId');
    validator.revalidateField('accountId');
    validator.revalidateField('subAccountId');
});

$('#accountId').on('change', async function () {
    blockUIAttribute.block();
    let elSubAccount = $('#subAccountId');
    elSubAccount.empty();
    if ($(this).val()) await getSubAccount($(this).val());
    blockUIAttribute.release();
    elSubAccount.val('').trigger('change.select2');
    validator.revalidateField('accountId');
    validator.revalidateField('subAccountId');
});

$('#subAccountId').on('change', async function () {
    validator.revalidateField('subAccountId')
});

$('#period').on('blur', function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

$('#startPromo').on('change', function () {
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
        el_end.next().css('background-color', '#fff !important');
    }
    ActivityDescFormula();
    clearAttribute();
    validator.revalidateField('startPromo')
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
        el_start.next().css('background-color', '#fff !important');
    }
    ActivityDescFormula();
    clearAttribute();
    validator.revalidateField('startPromo')
});

$('#period').on('change', function () {
    let period = this.value;
    var startDate = formatDate(new Date(period, 0, 1));
    var endDate = formatDate(new Date(period, 11, 31));
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
    startPromo.next().css('background-color', '#fff !important');
    endPromo.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        defaultDate: endDate
    });
    endPromo.next().css('background-color', '#fff !important');
    validator.revalidateField('startPromo')
});

$('#btn_get_baseline_sales').on('click', function () {
    getBaseline();
});

$('#normalSales, #investment, #incrementSales').on('keyup', async function () {
    await calcPromoPlanning();
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
        await getRegion().then(function () {
            readMechanismMechanismNew();

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
        await getRegion().then(async function () {
            await getBrand($('#entityId').val());
            arr_sku_temp = [];
            dt_sku.clear().draw();
            readMechanismManual();

            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
            $('#modal_attribute').modal('show');
        });
    }
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");

    validator.validate().then(async function (status) {
        if (status === "Valid") {
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            blockUI.block();
            blockUIAttribute.block();
            if (dt_mechanism.rows().count() === 0) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please fill in a mechanism",
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
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima", cancelButton: "btn btn-optima"}
                }).then(async function (result) {
                    if (result.isConfirmed) {
                        modifReason = result.value;
                        await saveData(modifReason);
                    } else {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;

                        blockUI.release();
                        blockUIAttribute.release();
                    }
                });
            } else {
                await saveData("");
            }
        }
    });
});

const saveData = function (modifReason) {
    //cek Promo Exist
    let e = document.querySelector("#btn_save");
    getPromoPlanningExist().then(async (values) => {
        if (!values) {
            submitSave(modifReason);
        } else {
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;

            blockUI.release();
            blockUIAttribute.release();
        }
    })
}

const submitSave = function (modifReason) {
    let e = document.querySelector("#btn_save");
    let formData = new FormData($('#form_promo_planning')[0]);
    formData.append('dataRegion', JSON.stringify(arr_region));
    formData.append('dataBrand', JSON.stringify(arr_brand));
    formData.append('dataSKU', JSON.stringify(arr_sku));
    formData.append('dataMechanism', JSON.stringify(arr_mechanism));
    formData.append('modifReason', modifReason);

    let url = "/promo/planning/save";
    if (method === "update") {
        url = "/promo/planning/update";
        formData.append('promoPlanningId', promoPlanningId);
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
                    window.location.href = '/promo/planning';
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

const getData = (promoPlanningId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/planning/data/id",
            type: "GET",
            data: {id: promoPlanningId},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;

                    tsCoding = values.promoPlanningHeader.tsCoding;
                    promoPlanningRefId = values.promoPlanningHeader.refId;
                    categoryId = values.promoPlanningHeader.categoryId;
                    subCategoryId = values.promoPlanningHeader.subCategoryId;
                    activityId = values.promoPlanningHeader.activityId;
                    subActivityId = values.promoPlanningHeader.subActivityId;
                    entityId = values.promoPlanningHeader.entityId;
                    distributorId = values.promoPlanningHeader.distributorId;

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

                    $('#period').val(new Date(values.promoPlanningHeader.startPromo).getFullYear());
                    $('#activityDesc').val(values.promoPlanningHeader.activityDesc);
                    $('#initiatorNotes').val(values.promoPlanningHeader.initiator_notes);
                    $('#baselineSales').val(formatMoney(values.promoPlanningHeader.normalSales, 0));
                    $('#incrementSales').val(formatMoney(values.promoPlanningHeader.incrSales, 0));
                    $('#investment').val(formatMoney(values.promoPlanningHeader.investment, 0));
                    $('#totalSales').val((parseFloat(values.promoPlanningHeader.incrSales) + parseFloat(values.promoPlanningHeader.normalSales)).toString());
                    $('#totalInvestment').val(values.promoPlanningHeader.investment);
                    $('#roi').val(formatMoney(values.promoPlanningHeader.roi, 2));
                    $('#costRatio').val(formatMoney(values.promoPlanningHeader.costRatio, 2));
                    $('#startPromo').val(formatDate(values.promoPlanningHeader.startPromo)).flatpickr({
                        altFormat: "d-m-Y",
                        altInput: true,
                        allowInput: true,
                        dateFormat: "Y-m-d",
                        disableMobile: "true",
                    });
                    $('#endPromo').val(formatDate(values.promoPlanningHeader.endPromo)).flatpickr({
                        altFormat: "d-m-Y",
                        altInput: true,
                        allowInput: true,
                        dateFormat: "Y-m-d",
                        disableMobile: "true",
                    });

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

const getCutOff = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/planning/cut-off",
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
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(e);
    });
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
        promoId: promoPlanningId,
        period: $('#period').val(),
        Entity: $('#entityId').val(),
        distributorId: $('#distributorId').val(),
        subCategoryId: $('#subCategoryId').val(),
        activityId: $('#activityId').val(),
        subActivityId: $('#subActivityId').val(),
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
            url: "/promo/planning/baseline",
            type: "GET",
            data: data,
            dataType: 'json',
            async: true,
            beforeSend: function () {
                if (!blockUI.blocked)
                    blockUI.block();
            },
            success: function (result) {
                let normalSales = 0;
                if (!result.error) {
                    normalSales = result.data.baseline_sales
                }
                $('#baselineSales').val(normalSales);
            },
            complete: function () {
                blockUI.release();
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

const getPromoPlanningExist = () => {
    return new Promise((resolve, reject) => {
        if (method === 'update') {
            resolve(false);
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
                url: "/promo/planning/exist",
                type: "GET",
                data: data,
                dataType: 'json',
                async: true,
                success: async function (result) {
                    if (!result.error) {
                        let d1 = formatDateOptima(result.data.startPromo);
                        let d2 = formatDateOptima(result.data.endPromo)
                        await swal.fire({
                            title: 'Promo Planning with similar data already exists',
                            text: '',
                            html:
                                "<div class='row'> \
                                    <div class='col-sm-5 text-start'>Promo Planning</div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + result.data.refId + "</div> \
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
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.responseText);
                    return resolve(false);
                }
            });
        }
    }).catch((e) => {
        console.log(e);
    });
}

const ActivityDescFormula = () => {
    let strMonth = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Des']

    let elStartPromo = $('#startPromo');

    let startPromoYear = new Date(elStartPromo.val()).getFullYear();
    let startPromoMonth = new Date(elStartPromo.val()).getMonth();

    let strStartPromoMonth = strMonth[startPromoMonth];

    let periode_desc = [strStartPromoMonth, startPromoYear.toString()].join(' ');
    let subActivityDesc = '';
    let elSubActivity = $("#subActivityId");
    let elRowShowDesc = $('#row_show_desc');
    if (!elRowShowDesc.hasClass('d-none')) elRowShowDesc.addClass('d-none');
    if (elSubActivity.val()) {
        subActivityDesc = elSubActivity.select2('data')[0].text;

        let strActivityName = subActivityDesc + ' ' + periode_desc;
        $('#activityDesc').val(strActivityName);
        $('#show_desc').html(strActivityName);
        document.getElementById('activityDesc').focus();
        document.getElementById('activityDesc').select();
        if (elRowShowDesc.hasClass('d-none')) elRowShowDesc.removeClass('d-none');
    }
}

const getEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/planning/list/entity",
            type        : "GET",
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
                $('#entityId').select2({
                    placeholder: "Select an Entity",
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

const getDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/planning/list/distributor",
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
        let url = "/promo/planning/list/sub-category";
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
            url         : "/promo/planning/list/activity",
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
            url         : "/promo/planning/list/sub-activity",
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

const getInvestmentType = (subActivityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/planning/investment-type",
            type        : "GET",
            data        : {subActivityId: subActivityId},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                investmentTypeId = 0;
                let elInvestmentType = $('#investmentType');
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
            url         : "/promo/planning/config-roi-cr",
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

const getChannel = () => {
    let url = "/promo/planning/list/channel";
    if (method === 'update') url = "/promo/planning/list/edit/channel";
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoPlanningId: promoPlanningId},
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
    let url = "/promo/planning/list/sub-channel";
    if (method === 'update') url = "/promo/planning/list/edit/sub-channel";
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoPlanningId: promoPlanningId, channelId: channelId},
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
    let url = "/promo/planning/list/account";
    if (method === 'update') url = "/promo/planning/list/edit/account";
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoPlanningId: promoPlanningId, subChannelId: subChannelId},
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
    let url = "/promo/planning/list/edit/sub-account";
    if (method === 'update') url = "/promo/planning/list/edit/sub-account";
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : url,
            type        : "GET",
            data        : {promoPlanningId: promoPlanningId, accountId: accountId},
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

const calcPromoPlanning = () => {
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

const convertStringToDate = (strDate) => {
    let year = strDate.substr(0,4);
    let month = strDate.substr(4,2);
    let date = strDate.substr(6,2);

    return new Date(year + '-' + month + '-' + date);
}

const clearAttribute = function () {
    if(newPeriodMechanism()){
        dt_mechanism.clear().draw();
        arr_brand = []
        $('#card_list_brand').html('');
        arr_sku = []
        $('#card_list_sku').html('');
        $('#mechanisme1').val('');
    }
}

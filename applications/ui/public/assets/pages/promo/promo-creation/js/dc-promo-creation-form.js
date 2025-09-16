'use strict';

let validator, method, dialerObject;
let uuid, categoryShortDescEnc, categoryId, categoryShortDesc, promoId, activityId, strActivityName, allocationId;
let entityId, entityShortDesc, distributorId, groupBrandId, subCategoryId, subActivityId, statusApprovalCode, isCancel, isClose, tsCoding, isCancelLocked;
let listChannel = [], listSubChannel = [], listAccount = [], listSubAccount = [], listBrand = [], listSKU = [], listRegion = [];
let subCategoryDesc, subActivityLongDesc, activityLongDesc;
let swalTitle = "Promo Creation";

let url_str = new URL(window.location.href);
method = url_str.searchParams.get("method");
promoId = url_str.searchParams.get("promoId");
categoryShortDescEnc = url_str.searchParams.get("c");

uuid = generateUUID(5);

FormValidation.validators.crossYear = function () {
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

validator = FormValidation.formValidation(document.getElementById('form_promo'), {
    fields: {
        entityId: {
            validators: {
                notEmpty: {
                    message: "Please select an entity"
                },
            }
        },
        groupBrandId: {
            validators: {
                notEmpty: {
                    message: "Please select a brand"
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
        subCategoryId: {
            validators: {
                notEmpty: {
                    message: "Please select a sub activity type"
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
                crossYear: {
                    message: 'Cross year is not allowed, start date must be older than end date'
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
        investment: {
            validators: {
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

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#investment");

    disableButtonSave();
    blockUI.block();
    if (method === "update") {
        $('#period').attr('readonly', true);
        $('#dialer_period button').remove();
        getCategoryId(categoryShortDescEnc).then(function () {
            Promise.all([getEntity(), getSubCategory(), getChannel(), getRegion()]).then(async () => {
                await getData(promoId);
                await $('#entityId').val(entityId).trigger('change.select2');
                $("#entityId").attr("readonly", "readonly");

                await getGroupBrand(entityId);
                $('#groupBrandId').val(groupBrandId).trigger('change.select2');

                await getDistributor(entityId);
                await $('#distributorId').val(distributorId).trigger('change.select2');

                await getSubCategory(categoryId);
                $('#subCategoryId').val(subCategoryId).trigger('change.select2');

                await getSubActivity(subCategoryId);
                $('#subActivityId').val(subActivityId).trigger('change.select2');

                ActivityDescFormula();
                enableButtonSave();
                disabledActivityPeriod();
                blockUI.release();
            });
        });
    } else {
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
            Promise.all([getEntity(), getSubCategory(), getChannel(), getRegion()]).then(async () => {
                enableButtonSave();
                blockUI.release();
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

    clearBudgetSource();
    validator.revalidateField('startPromo')
});

$('#entityId').on('change', async function () {
    blockUI.block();
    let elDistributor = $('#distributorId');
    let elGroupBrand = $('#groupBrandId');

    elDistributor.empty();
    elGroupBrand.empty();

    if ($(this).val()) {
        let dataEntity = $(this).select2('data');
        entityShortDesc = ((dataEntity[0].shortDesc) ? dataEntity[0].shortDesc : '');
        await getDistributor($(this).val());
        elDistributor.val('').trigger('change.select2');
        await getGroupBrand($(this).val());
        elGroupBrand.val('').trigger('change.select2');
    }
    validator.revalidateField('entityId');
    if (method !== "update") clearBudgetSource();
    blockUI.release();
});

$('#groupBrandId').on('change', async function () {
    if (!blockUI.blocked) blockUI.block();
    if ($(this).val()) {
        await getBrand($(this).val());
        listSKU = [];
        for (let i = 0; i < listBrand.length; i++) {
            await getSKU(listBrand[i]);
        }
    }
    clearBudgetSource();
    validator.revalidateField('groupBrandId');
    blockUI.release();
});

$('#distributorId').on('change', async function () {
    clearBudgetSource();
    validator.revalidateField('distributorId');
});

$('#subCategoryId').on('change', async function () {
    blockUI.block();
    let elSubActivity = $('#subActivityId');

    elSubActivity.empty();

    if ($(this).val()) {
        await getSubActivity($(this).val());
        elSubActivity.val('').trigger('change.select2');
    }
    validator.revalidateField('subCategoryId');
    clearBudgetSource();
    blockUI.release();
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

$('#btn_search_budget').on('click', async function () {
    let validEntity = false;
    let validGroupBrand = false;
    let validDistributor = false;
    let validSubCategory = false;

    await validator.validateField('entityId').then(function (status) {
        if (status === "Valid") {
            validEntity = true;
        }
    });
    await validator.validateField('groupBrandId').then(function (status) {
        if (status === "Valid") {
            validGroupBrand = true;
        }
    });
    await validator.validateField('distributorId').then(function (status) {
        if (status === "Valid") {
            validDistributor = true;
        }
    });
    await validator.validateField('subCategoryId').then(function (status) {
        if (status === "Valid") {
            validSubCategory = true;
        }
    });

    if(validEntity && validGroupBrand && validDistributor && validSubCategory) {
        $('#dt_source_budget_list_search').val('');
        dt_source_budget_list.clear().draw();
        blockUI.block();

        let param = {
            period: $('#period').val(),
            entityId: $('#entityId').val(),
            distributorId: $('#distributorId').val(),
            subCategoryId: $('#subCategoryId').val(),
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
            url: "/promo/creation/list/source-budget",
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
    validator.revalidateField('startPromo');
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
            if (fileName !== 'Choose File') {
                $.ajax({
                    url: "/promo/creation/attachment-delete",
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

            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            blockUI.block();

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
                    }
                });
            } else {
                await saveData("");
            }
        }
    });
});

const saveData = async (modifyReason) => {
    let e = document.querySelector("#btn_save");
    await getPromoExist().then(async function (values) {
        if (!values) {
            if (method !== "update") {
                let latePromoDays = await getLatePromoDays();

                let elStartPromo = $('#startPromo');
                let dtStart = new Date(elStartPromo.val()).getTime();
                let dtNow = new Date().getTime();
                let diffDays = Math.ceil((dtStart - dtNow) / (1000 * 60 * 60 * 24));
                validator.addField('initiatorNotes', {
                    validators: {
                        notEmpty: {
                            message: "Please fill in a notes"
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
                    }).then(async function (result) {
                        if (result.isConfirmed) {
                            $('#initiatorNotes').val(result.value)
                            elStartPromo.addClass('form-control-solid-bg')
                            $('#endPromo').addClass('form-control-solid-bg')
                            $('#startPromo, #endPromo').flatpickr({
                                altFormat: "d-m-Y",
                                altInput: true,
                                allowInput: true,
                                dateFormat: "Y-m-d",
                                disableMobile: "true",
                            });

                            validator.revalidateField('initiatorNotes');
                            await submitSave(modifyReason);
                        } else {
                            elStartPromo.addClass('form-control-solid-bg')
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
                        }
                    });
                } else {
                    elStartPromo.removeClass('form-control-solid-bg')
                    $('#endPromo').removeClass('form-control-solid-bg')
                    $('#startPromo, #endPromo')
                        .flatpickr({
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
        }
    });
}

const submitSave = async function (modifyReason) {
    let e = document.querySelector("#btn_save");
    let channel = $("#channelId").val();
    let listChannel = [];
    for (let i = 0; i < channel.length; i++) {
        listChannel.push(channel[i]);
    }

    let arr_mechanism = [];
    let mechanism = {
        id : 1,
        mechanism : $('#mechanism1').val(),
        notes : "",
        productId : 0,
        product : "",
        brandId : 0,
        brand : ""
    };

    arr_mechanism.push(mechanism);

    let formData = new FormData($('#form_promo')[0]);
    formData.append('uuid', uuid);
    formData.append('promoId', promoId);
    formData.append('promoPlanId', '0');
    formData.append('allocationId', allocationId);
    formData.append('entityShortDesc', entityShortDesc);
    formData.append('categoryId', categoryId);
    formData.append('categoryShortDesc', categoryShortDesc);
    formData.append('subCategoryId', $('#subCategoryId').val());
    formData.append('activityId', activityId);
    formData.append('activityDesc', strActivityName);
    formData.append('baselineSales', '0');
    formData.append('incrementSales', '0');
    formData.append('roi', '0');
    formData.append('costRatio', '0');
    formData.append('dataChannel', JSON.stringify(listChannel));
    formData.append('dataSubChannel', JSON.stringify(listSubChannel));
    formData.append('dataAccount', JSON.stringify(listAccount));
    formData.append('dataSubAccount', JSON.stringify(listSubAccount));
    formData.append('dataRegion', JSON.stringify(listRegion));
    formData.append('dataBrand', JSON.stringify(listBrand));
    formData.append('dataSKU', JSON.stringify(listSKU));
    formData.append('dataMechanism', JSON.stringify(arr_mechanism));
    formData.append('modifReason', modifyReason);
    formData.append('tsCode', '');
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
        },
        error: function (jqXHR) {
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

const getPromoExist = () => {
    return new Promise((resolve, reject) => {
        let exist = false;
        if (method === 'update') {
            resolve(exist);
        } else {
            let data = {
                period: $('#period').val(),
                distributorId: $('#distributorId').val(),
                subActivityTypeId: $('#subCategoryId').val(),
                subActivityId: $('#subActivityId').val(),
                startPromo: $('#startPromo').val(),
                endPromo: $('#endPromo').val()
            }

            $.ajax({
                url: "/promo/creation/exist/dc",
                type: "GET",
                data: data,
                dataType: 'json',
                async: true,
                success: async function (result) {
                    if (!result.error) {
                        let d1 = formatDateOptima(result.data.StartPromo);
                        let d2 = formatDateOptima(result.data.EndPromo)
                        await swal.fire({
                            title: 'Promo ID with similar data already exists',
                            text: '',
                            html:
                                "<div class='row'> \
                                    <div class='col-sm-5 text-start'>Promo Number</div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + result.data.RefId + "</div> \
                                    <div class='col-sm-5 text-start'>Distributor </div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + result.data.distributor + "</div> \
                                    <div class='col-sm-5 text-start'>Sub Activity Type </div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + result.data.subActivcityType + "</div> \
                                    <div class='col-sm-5 text-start'>Sub Activity </div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + result.data.subActivity + "</div> \
                                    <div class='col-sm-5 text-start'>Activity Period </div><div class='col-sm-1'>:</div><div class='col-sm-6 text-start'>" + d1 + ' to ' + d2 + "</div> \
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
                    console.log(jqXHR);
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
            elStartPromo.addClass('form-control-solid-bg')
            $('#endPromo').addClass('form-control-solid-bg')
            $('#startPromo, #endPromo').flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
            });
            return true;
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
            return false;
        }
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
        return false;
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
                return reject(0);
            }
        });
    }).catch((e) => {
        console.log(e);
        return reject(0);
    });
}

const clearBudgetSource = () => {
    allocationId = null;
    $('#allocationRefId').val('')
    $('#allocationDesc').val('')
    $('#budgetDeployed').val('')
    $('#budgetRemaining').val('')
}

const getEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/list/entity",
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
                url: "/promo/creation/list/group-brand",
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
            url         : "/promo/creation/list/distributor",
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
        $.ajax({
            url         : "/promo/creation/list/sub-category/category-id",
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
            url         : "/promo/creation/list/sub-activity/sub-category-id",
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
            url         : "/promo/creation/list/channel-master",
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
        url = "/promo/creation/list/sub-channel/channels";
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
        url = "/promo/creation/list/account/sub-channels";
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
        url = "/promo/creation/list/sub-account/accounts";
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
            url         : "/promo/creation/list/region",
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
                url: "/promo/creation/list/brand/group-brand-id",
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
            url         : "/promo/creation/list/sku",
            type        : "GET",
            data        : {brandId: brandId},
            dataType    : 'json',
            async       : true,
            beforeSend: function() {

            },
            success: function(result) {
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
    let formData = new FormData();
    let file = document.getElementById('attachment'+row).files[0];
    if (method === 'update') {
        formData.append('promoId', promoId);
        formData.append('mode', 'edit');
    } else {
        formData.append('promoId', '0');
    }

    formData.append('uuid', uuid);
    formData.append('file', file);
    formData.append('row', 'row'+row);
    formData.append('docLink', el.val());

    $.ajax({
        url: "/promo/creation/attachment-upload",
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

const getData = (promoId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/creation/data/id",
            type: "GET",
            data: {id: promoId},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    if(result.errCode === 200){
                        let values = result.data;

                        let promoHeader = values.promoHeader;

                        statusApprovalCode = promoHeader.statusApprovalCode;
                        isCancel = promoHeader.isCancel;
                        isClose = promoHeader.isClose;
                        tsCoding = promoHeader.tsCoding;
                        isCancelLocked = promoHeader.isCancelLocked;

                        $('#txt_info_method').text('Edit ' + promoHeader.refId);


                        $('#period').val(new Date(promoHeader.startPromo).getFullYear());
                        entityId = promoHeader.principalId;
                        entityShortDesc = promoHeader.principalShortDesc;

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

                        distributorId = promoHeader.distributorId;
                        subCategoryId = promoHeader.subCategoryId;
                        subCategoryDesc = promoHeader.subCategoryDesc;
                        activityId = promoHeader.activityId;
                        activityLongDesc = promoHeader.activityLongDesc;
                        subActivityId = promoHeader.subActivityId;
                        subActivityLongDesc = promoHeader.subActivityLongDesc;
                        strActivityName = promoHeader.activityDesc;

                        if (values.channels) {
                            listChannel = [];
                            for (let i=0; i < values.channels.length; i++) {
                                if (values.channels[i].flag) listChannel.push(values.channels[i].id);
                            }
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

                        allocationId = promoHeader.allocationId;
                        $('#allocationRefId').val(promoHeader.allocationRefId);
                        $('#startPromo').val(formatDate(promoHeader.startPromo)).flatpickr({
                            altFormat: "d-m-Y",
                            altInput: true,
                            allowInput: true,
                            dateFormat: "Y-m-d",
                            disableMobile: "true",
                        });
                        $('#endPromo').val(formatDate(promoHeader.endPromo)).flatpickr({
                            altFormat: "d-m-Y",
                            altInput: true,
                            allowInput: true,
                            dateFormat: "Y-m-d",
                            disableMobile: "true",
                        });
                        if (values.mechanisms.length > 0) {
                            for (let i = 0; i < values.mechanisms.length; i++) {
                                $('#mechanism1').val(values.mechanisms[i].mechanism);
                            }
                        }
                        $('#investment').val(formatMoney(promoHeader.investment, 0));
                        $('#initiatorNotes').val(promoHeader.initiator_notes);

                        $('#allocationDesc').val(promoHeader.allocationDesc);
                        $('#budgetDeployed').val(formatMoney(promoHeader.budgetAmount, 0));
                        $('#budgetRemaining').val(formatMoney(promoHeader.remainingBudget, 0));

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

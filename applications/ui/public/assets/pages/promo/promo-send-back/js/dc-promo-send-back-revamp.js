'use strict';

let swalTitle = "Promo Send Back";
let url_str = new URL(window.location.href);
let method = url_str.searchParams.get("method");
let promoId = url_str.searchParams.get("id");
let categoryShortDescEnc = url_str.searchParams.get("c");

let dialerObject;
let categoryId;
let entityList = [], brandList = [], distributorList = [], subActivityTypeList = [], subActivityList = [], channelList = [], configCalculator = [];

let elPeriod = $('#period');
let elSubActivity = $('#subActivityId'), elChannel = $('#channelId');

let baseline = 0, totalSales = 0, cr = 0, cost = 0, remainingBudget = 0, dataMatrixCalculator;
let strActivityName, statusApprovalCode;
let fvPromo;
const formPromo = document.getElementById('form_promo');

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

//<editor-fold desc="Form Validation">
const crossYear = function () {
    return {
        validate: function () {
            const valueStartPromo = $('#startPromo').val();
            const valueEndPromo = $('#endPromo').val();
            const startPromoYear = new Date(valueStartPromo).getFullYear().toString();
            const endPromoYear = new Date(valueEndPromo).getFullYear().toString();

            if (startPromoYear === endPromoYear) {
                if ((new Date(valueStartPromo)) > (new Date(valueEndPromo))) {
                    return {
                        message: 'Start date is greater than end date, is not allowed',
                        valid: false,
                    };
                } else {
                    return {
                        valid: true,
                    };
                }
            } else {
                return {
                    message: 'Cross year is not allowed',
                    valid: false
                }
            }
        }
    };
};

FormValidation.validators.crossYear = crossYear;

fvPromo = FormValidation.formValidation(formPromo, {
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
        subActivityTypeId: {
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
        channelId: {
            validators: {
                notEmpty: {
                    message: "Please select a channel"
                },
            }
        },
        startPromo: {
            validators: {
                // check cross Year
                crossYear: {

                },
            }
        },
        mechanism: {
            validators: {
                notEmpty: {
                    message: "Please fill in mechanism"
                },
            }
        },
        initiatorNotes: {
            validators: {
                stringLength: {
                    max: 255,
                    message: 'Please enter no more than 255 characters.'
                },
            }
        },
    },
    plugins: {
        trigger: new FormValidation.plugins.Trigger,
        bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
    }
});
//</editor-fold>

//<editor-fold desc="Loading Animation">
let targetHeader = document.querySelector(".card_header");
let blockUIHeader = new KTBlockUI(targetHeader, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetBudget = document.querySelector(".card_budget");
let blockUIBudget = new KTBlockUI(targetBudget, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetAttachment = document.querySelector(".card_attachment");
let blockUIAttachment = new KTBlockUI(targetAttachment, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetPromoCalculator = document.querySelector(".card_promo_calculator");
let blockUIPromoCalculator = new KTBlockUI(targetPromoCalculator, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});
//</editor-fold>

//<editor-fold desc="Document on load">
$(document).ready(function () {
    $('form').submit(false);

    KTDialer.createInstances();

    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
        min: 2025
    });

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#remainingBudget, #totalCost, #baseline, #totalSales, #cost");

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
        jitMasking: true,
        suffix: ' %'
    }).mask("#uplift, #salesContribution, #storesCoverage, #redemptionRate, #roi, #cr");

    $('#startPromo, #endPromo').flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        minDate: "2025-01",
        onClose: function() {
            $('#startPromo, #endPromo').triggerHandler('change');
        }
    });

    blockUIHeader.block();
    blockUIBudget.block();
    blockUIAttachment.block();
    blockUIPromoCalculator.block();
    disableButtonSave();
    getListAttribute().then(async function () {
        categoryId = await getCategoryId();
        // entity
        let entityDropdown = [];
        for (let i = 0; i < entityList.length; i++) {
            entityDropdown.push({
                id: entityList[i]['entityId'],
                text: entityList[i]['entityDesc']
            });
        }
        $('#entityId').select2({
            placeholder: "Select an Entity",
            width: '100%',
            data: entityDropdown
        }).on('change', function () {
            // Revalidate the color field when an option is chosen
            fvPromo.revalidateField('entityId');
        });

        // distributor
        let distributorDropdown = [];
        for (let i = 0; i < distributorList.length; i++) {
            distributorDropdown.push({
                id: distributorList[i]['distributorId'],
                text: distributorList[i]['distributorDesc']
            });
        }
        $('#distributorId').select2({
            placeholder: "Select a Distributor",
            width: '100%',
            data: distributorDropdown
        }).on('change', function () {
            // Revalidate the color field when an option is chosen
            fvPromo.revalidateField('distributorId');
        });

        // sub activity type
        let subActivityTypeDropdown = [];
        for (let i = 0; i < subActivityTypeList.length; i++) {
            if (subActivityTypeList[i]['CategoryDesc'] === "Distributor Cost") {
                subActivityTypeDropdown.push({
                    id: subActivityTypeList[i]['SubActivityTypeId'],
                    text: subActivityTypeList[i]['SubActivityTypeDesc']
                });
            }
        }
        $('#subActivityTypeId').select2({
            placeholder: "Select a Sub Category",
            width: '100%',
            data: subActivityTypeDropdown
        }).on('change', function () {
            // Revalidate the color field when an option is chosen
            fvPromo.revalidateField('subActivityTypeId');
        });

        // channel
        let channelDropdown = [];
        for (let i = 0; i < channelList.length; i++) {
            channelDropdown.push({
                id: channelList[i]['ChannelId'],
                text: channelList[i]['ChannelDesc']
            });
        }
        $('#channelId').select2({
            placeholder: "Select a Channel",
            width: '100%',
            data: channelDropdown
        }).on('change', function () {
            // Revalidate the color field when an option is chosen
            fvPromo.revalidateField('channelId');
        });

        await resetCalculator();


        await getData(promoId);
        await getBudget();
        ActivityDescFormula();

        blockUIHeader.release();
        blockUIBudget.release();
        blockUIAttachment.release();
        blockUIPromoCalculator.release();
        enableButtonSave();
    });
});
//</editor-fold>

//<editor-fold desc="Period Event">
elPeriod.on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

elPeriod.on('change', async function () {
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
        minDate: "2025-01",
        defaultDate: startDate,
        onClose: function() {
            startPromo.trigger('change');
        }
    });

    endPromo.flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
        minDate: "2025-01",
        defaultDate: endDate,
        onClose: function() {
            startPromo.trigger('change');
        }
    });
    if (blockUIHeader.isBlocked()) blockUIHeader.release();
    if (blockUIPromoCalculator.isBlocked()) blockUIPromoCalculator.release();
    await costFormula();
    await getBudget();
    fvPromo.revalidateField('startPromo');
});
//</editor-fold>

//<editor-fold desc="Activity Period Event">
$('#startPromo').on('change', async function () {
    let elStart = $('#startPromo');
    let elEnd = $('#endPromo');
    let startDate = new Date(elStart.val()).getTime();
    let endDate = new Date(elEnd.val()).getTime();
    let startYear = new Date(elStart.val()).getFullYear();
    let endYear = new Date(elEnd.val()).getFullYear();
    fvPromo.revalidateField('startPromo');
    if (startDate > endDate || startYear !== endYear) {
        elEnd.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            minDate: "2025-01",
            defaultDate: new Date(endDate),
            onClose: function() {
                elStart.trigger('change');
            }
        });
    }
    if (blockUIHeader.isBlocked()) blockUIHeader.release();
    if (blockUIPromoCalculator.isBlocked()) blockUIPromoCalculator.release();
    await costFormula();
    elPeriod.val(new Date(elStart.val()).getFullYear()).on('change');
    ActivityDescFormula();
    await backDate();
});

$('#endPromo').on('change', async function () {
    let elStart = $('#startPromo');
    let elEnd = $('#endPromo');
    let startDate = new Date(elStart.val()).getTime();
    let endDate = new Date(elEnd.val()).getTime();
    let startYear = new Date(elStart.val()).getFullYear();
    let endYear = new Date(elEnd.val()).getFullYear();
    if (startDate > endDate || startYear !== endYear) {
        elStart.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            minDate: "2025-01",
            defaultDate: new Date(startDate),
            onClose: function() {
                elStart.trigger('change');
            }
        });
    }
    if (blockUIHeader.isBlocked()) blockUIHeader.release();
    if (blockUIPromoCalculator.isBlocked()) blockUIPromoCalculator.release();
    await costFormula();
    elPeriod.val(new Date(elEnd.val()).getFullYear()).on('change');
    fvPromo.revalidateField('startPromo');
});
//</editor-fold>

//<editor-fold desc="Entity Matrix Event">
$('#entityId').on('change', async function () {
    await loadDropdownGroupBrand($(this).val(), '');
    fvPromo.revalidateField('entityId');
});

$('#groupBrandId').on('change', async function () {
    if ($(this).val()) {
        await getBudget();
        await costFormula();
        await roiFormula();
        fvPromo.revalidateField('groupBrandId');
    }
});

$('#distributorId').on('change', async function () {
    if ($(this).val()) {
        await getBudget();
        await costFormula();
        await roiFormula();
        fvPromo.revalidateField('distributorId');
    }
});
//</editor-fold>

//<editor-fold desc="Sub Activity Type Matrix Event">
$('#subActivityTypeId').on('change', async function () {
    await resetCalculator();
    await loadDropdownSubActivity($(this).val(), '');
    await getBudget();
    fvPromo.revalidateField('subActivityTypeId');
});

elSubActivity.on('change', async function () {
    await resetCalculator();
    if ($(this).val()) {
        ActivityDescFormula();
        fvPromo.revalidateField('subActivityId');
    }
    if ($(this).val() && elChannel.val()) {
        let subActivitySelected = elSubActivity.select2('data');
        let dataCalculator = null;
        //<editor-fold desc="Selected Config Calculator">
        let channelId = parseInt(elChannel.val());
        for (let i=0; i<configCalculator.length; i++) {
            if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
                dataCalculator = configCalculator[i];
            }
        }
        //</editor-fold>

        if (!dataCalculator) {
            return Swal.fire({
                title: swalTitle,
                icon: "warning",
                text: 'Promo calculator configuration not found',
                showConfirmButton: true,
                confirmButtonText: 'Confirm',
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
        await setConfigCalculator(dataCalculator);

        await getBudget();
        await costFormula();
    }
});
//</editor-fold>

//<editor-fold desc="Channel Event">
elChannel.on('change', async function () {
    await resetCalculator();
    if ($(this).val() && elSubActivity.val()) {
        let subActivitySelected = elSubActivity.select2('data');
        let dataCalculator = null;
        //<editor-fold desc="Selected Config Calculator">
        let channelId = parseInt(elChannel.val());
        for (let i=0; i<configCalculator.length; i++) {
            if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
                dataCalculator = configCalculator[i];
            }
        }
        //</editor-fold>

        if (!dataCalculator) {
            return Swal.fire({
                title: swalTitle,
                icon: "warning",
                text: 'Promo calculator configuration not found',
                showConfirmButton: true,
                confirmButtonText: 'Confirm',
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
        await setConfigCalculator(dataCalculator);

        await getBudget();
        await costFormula();
    }
});
//</editor-fold>

//<editor-fold desc="Attachment">
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
            elLabel.text('');

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
    form_data.append('promoId', promoId);
    form_data.append('mode', 'edit');

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
                            elLabelAttachment.text('');
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
        let id = promoId;
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
//</editor-fold>

//<editor-fold desc="Trigger Cost Formula">
$('#baseline').on('keyup', async function (e) {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['baseline'] === 1) {
        let code = e.keyCode || e.which;
        if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
            await costFormula(true);
            roiFormula();
        }
    }
}).on('blur', async function () {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['baseline'] === 1) {
        if (!$(this).val()) $(this).val(0);
        await costFormula(true);
        roiFormula();
    }
});

$('#uplift').on('keyup', async function (e) {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['uplift'] === 1) {
        let code = e.keyCode || e.which;
        if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
            await totalSalesFormula();
            await costFormula(true);
            roiFormula();
        }
    }
}).on('blur', async function () {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['uplift'] === 1) {
        if (!$(this).val()) $(this).val(0);
        await totalSalesFormula();
        await costFormula(true);
        roiFormula();
    }
});

$('#totalSales').on('keyup', async function (e) {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['totalSales'] === 1) {
        let code = e.keyCode || e.which;
        if ($(this).val()) {
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormula(true);
                roiFormula();
                await crFormula();
            }
        }
    }
}).on('blur', async function () {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['totalSales'] === 1) {
        if (!$(this).val()) $(this).val(0);
        await costFormula(true);
        roiFormula();
        await crFormula();
    }
});

$('#salesContribution').on('keyup', async function (e) {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['salesContribution'] === 1) {
        let code = e.keyCode || e.which;
        if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
            await costFormula(true);
            roiFormula();
        }
    }
}).on('blur', async function () {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['salesContribution'] === 1) {
        if (!$(this).val()) $(this).val(0);
        await costFormula(true);
        roiFormula();
    }
});

$('#storesCoverage').on('keyup', async function (e) {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['storesCoverage'] === 1) {
        let code = e.keyCode || e.which;
        if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
            await costFormula(true);
            roiFormula();
        }
    }
}).on('blur', async function () {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['storesCoverage'] === 1) {
        if (!$(this).val()) $(this).val(0);
        await costFormula(true);
        roiFormula();
    }
});

$('#redemptionRate').on('keyup', async function (e) {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['redemptionRate'] === 1) {
        let code = e.keyCode || e.which;
        if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
            await costFormula(true);
            roiFormula();
        }
    }
}).on('blur', async function () {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['redemptionRate'] === 1) {
        if (!$(this).val()) $(this).val(0);
        await costFormula(true);
        roiFormula();
    }
});

$('#cr').on('keyup', async function (e) {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['cr'] === 1) {
        let code = e.keyCode || e.which;
        if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
            await costFormula(true);
            roiFormula();
        }
    }
}).on('blur', async function () {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['cr'] === 1) {
        if (!$(this).val()) $(this).val(0);
        await costFormula(true);
        roiFormula();
    }
});

$('#cost').on('keyup', async function (e) {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['cost'] === 1) {
        let code = e.keyCode || e.which;
        if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
            await crFormula();
            roiFormula();
        }
    }
}).on('blur', async function () {
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }

    if (dataCalculator['cost'] === 1) {
        if (!$(this).val()) $(this).val(0);
        await crFormula();
        roiFormula();
    }
});
//</editor-fold>

$('#sticky_notes').on('click', function () {
    let elModal = $('#modal_notes');
    if (elModal.hasClass('show')) {
        elModal.modal('hide');
    } else {
        elModal.modal('show');
    }
});

$('#btn_save').on('click', async function () {
    let e = document.querySelector("#btn_save");
    fvPromo.validate().then(async function (status) {
        if (status === "Valid") {
            //enter reason if edit
            let modifyReason = '';
            let elCost = $('#cost');
            if (elCost.val() === '0') {
                return Swal.fire({
                    title: 'Cost cannot be zero',
                    icon: "warning",
                    showConfirmButton: true,
                    confirmButtonText: 'Confirm',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
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
        }
    });
});

const upliftFormula = async () => {
    let elBaseline = $('#baseline');
    let elTotalSales = $('#totalSales');
    let elUplift = $('#uplift');

    let baseline = parseFloat((elBaseline.val() === '' ? '0' : elBaseline.val()).replace(/,/g, ''));
    let totalSalesValue = parseFloat((elTotalSales.val() === '' ? '0' : elTotalSales.val()).replace(/,/g, ''));
    if (baseline === 0) baseline = 1;
    let uplift = ((((totalSalesValue - baseline) / totalSalesValue) * 100) + 100);
    if (!isFinite(uplift)) {
        elUplift.val(0);
    } else {
        elUplift.val(+(uplift).toFixed(2));
    }
}

const redemptionRateFormula = async () => {
    let elBaseline = $('#baseline');
    let elUplift = $('#uplift');
    let elSalesContribution = $('#salesContribution');
    let elStoresCoverage = $('#storesCoverage');
    let elCr = $('#cr');
    let elCost = $('#cost');
    let elRedemptionRate = $('#redemptionRate');

    let baseline = parseFloat((elBaseline.val() === '' ? '0' : elBaseline.val()).replace(/,/g, ''));
    let uplift = parseFloat((elUplift.val() === '' ? '0' : elUplift.val()).replace(/,/g, ''));
    let salesContribution = parseFloat((elSalesContribution.val() === '' ? '0' : elSalesContribution.val()).replace(/,/g, ''));
    let storesCoverage = parseFloat((elStoresCoverage.val() === '' ? '0' : elStoresCoverage.val()).replace(/,/g, ''));
    let cr = parseFloat((elCr.val() === '' ? '0' : elCr.val()).replace(/,/g, ''));
    let cost = parseFloat((elCost.val() === '' ? '0' : elCost.val()).replace(/,/g, ''));

    if (baseline === 0) baseline = 1;
    let baselineXAllPerc = parseFloat((baseline * (uplift  / 100).toFixed(2) * (salesContribution / 100).toFixed(2) * (storesCoverage / 100).toFixed(2) * (cr / 100).toFixed(2)).toFixed(2));
    let redemptionRate = parseFloat((cost / baselineXAllPerc).toFixed(2)) * 100;
    if (!isFinite(redemptionRate)) {
        elRedemptionRate.val(0);
    } else {
        elRedemptionRate.val(+(redemptionRate).toFixed(2));
    }
}

const crFormula = async () => {
    let elTotalSales = $('#totalSales');
    let elCr = $('#cr');
    let elCost = $('#cost');

    let totalSales = parseFloat((elTotalSales.val() === '' ? '0' : elTotalSales.val()).replace(/,/g, ''));
    let cost = parseFloat((elCost.val() === '' ? '0' : elCost.val()).replace(/,/g, ''));

    let cr = (cost / totalSales) * 100;
    if (!isFinite(cr)) {
        elCr.val((0).toFixed(0));
    } else {
        elCr.val(cr.toFixed(2));
    }
}

const roiFormula = () => {
    let elUplift = $('#uplift');
    let elTotalSales = $('#totalSales');
    let elRoi = $('#roi');
    let elCost = $('#cost');

    let uplift = parseFloat((elUplift.val() === '' ? '0' : elUplift.val()).replace(/,/g, ''));
    let totalSales = parseFloat((elTotalSales.val() === '' ? '0' : elTotalSales.val()).replace(/,/g, ''));
    let cost = parseFloat((elCost.val() === '' ? '0' : elCost.val()).replace(/,/g, ''));
    //uplift
    uplift = uplift+100;

    let incrSales = (Math.round(totalSales) * (uplift / 100));
    let roi = Math.round(((incrSales - cost) / cost) * 100);
    if (!isFinite(roi)) {
        elRoi.val(0);
    } else {
        elRoi.val(roi);
    }
}

const setConfigCalculator = (data) => {
    let elBaseline = $('#baseline');
    switch (data['baseline']) {
        case 0:
            elBaseline.val('').attr('readonly', true).addClass('form-control-solid-bg');
            break;
        case 1:
            elBaseline.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
            break;
        case 2:
            elBaseline.val('').attr('readonly', true).css('background-color', '#8f99e7 !important');
            break;
    }

    let elUplift = $('#uplift');
    switch (data['uplift']) {
        case 0:
            elUplift.val('').attr('readonly', true).addClass('form-control-solid-bg');
            break;
        case 1:
            elUplift.val('100').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
            break;
        case 2:
            elUplift.val('').attr('readonly', true).css('background-color', '#8f99e7 !important');
            break;
    }

    let elTotalSales = $('#totalSales');
    switch (data['totalSales']) {
        case 0:
            elTotalSales.val('').attr('readonly', true).addClass('form-control-solid-bg');
            break;
        case 1:
            elTotalSales.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
            break;
        case 2:
            elTotalSales.val('').attr('readonly', true).css('background-color', '#8f99e7 !important');
            break;
    }

    let elSalesContribution = $('#salesContribution');
    switch (data['salesContribution']) {
        case 0:
            elSalesContribution.val('').attr('readonly', true).addClass('form-control-solid-bg');
            break;
        case 1:
            elSalesContribution.val('100').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
            break;
        case 2:
            elSalesContribution.val('').attr('readonly', true).css('background-color', '#8f99e7 !important');
            break;
    }

    let elStoresCoverage = $('#storesCoverage');
    switch (data['storesCoverage']) {
        case 0:
            elStoresCoverage.val('').attr('readonly', true).addClass('form-control-solid-bg');
            break;
        case 1:
            elStoresCoverage.val('100').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
            break;
        case 2:
            elStoresCoverage.val('').attr('readonly', true).css('background-color', '#8f99e7 !important');
            break;
    }

    let elRedemptionRate = $('#redemptionRate');
    switch (data['redemptionRate']) {
        case 0:
            elRedemptionRate.val('').attr('readonly', true).addClass('form-control-solid-bg');
            break;
        case 1:
            elRedemptionRate.val('100').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
            break;
        case 2:
            elRedemptionRate.val('').attr('readonly', true).css('background-color', '#8f99e7 !important');
            break;
    }

    let elCR = $('#cr');
    switch (data['cr']) {
        case 0:
            elCR.val('').attr('readonly', true).addClass('form-control-solid-bg');
            break;
        case 1:
            elCR.val('100').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
            break;
        case 2:
            elCR.val('').attr('readonly', true).css('background-color', '#8f99e7 !important');
            break;
    }

    let elCost = $('#cost');
    switch (data['cost']) {
        case 0:
            elCost.val('').attr('readonly', true).addClass('form-control-solid-bg');
            break;
        case 1:
            elCost.attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
            break;
        case 2:
            elCost.val('').attr('readonly', true).css('background-color', '#8f99e7 !important');
            break;
    }

    let elROI = $('#roi');
    elROI.val('')
}

const costFormula = async (keyInput = false, skuId = 0) => {
    let elCost = $('#cost');

    //<editor-fold desc="Set Config Calculator">
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }
    //</editor-fold>
    if (dataCalculator) {
        blockUIHeader.block();
        blockUIPromoCalculator.block();
        if (dataCalculator['cost'] === 2) {
            let cost = await calculatorFormula(keyInput, skuId);
            elCost.val(cost);
        }
        blockUIHeader.release();
        blockUIPromoCalculator.release();
    }
}

const calculatorFormula = async (keyInput = false, skuId = 0) => {
    let elBaseline = $('#baseline');
    let elUplift = $('#uplift');
    let elTotalSales = $('#totalSales');
    let elSalesContribution = $('#salesContribution');
    let elStoresCoverage = $('#storesCoverage');
    let elRedemptionRate = $('#redemptionRate');
    let elCr = $('#cr');

    let baselineValue = 1;
    let upliftValue = 1;
    let totalSalesValue = 1;
    let salesContributionValue = 1;
    let storesCoverageValue = 1;
    let redemptionRateValue = 1;
    let crValue = 1;

    //<editor-fold desc="Set Config Calculator">
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    let mainActivityDesc = subActivitySelected[0]['mainActivityDesc'];
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }
    //</editor-fold>

    //<editor-fold desc="Baseline">
    if (dataCalculator['baseline'] === 0) {
        elBaseline.val("");
    } else if (dataCalculator['baseline'] === 1) {
        elBaseline.val(0);
    } else if (dataCalculator['baseline'] === 2) {
        if (!keyInput) {
            let paramArrSKU = [];
            if (!autoMechanism) {
                let skuSelectedList = dt_sku_selected.rows().data();
                for (let i = 0; i < skuSelectedList.length; i++) {
                    paramArrSKU.push(skuSelectedList[i]['skuId']);
                }
            } else {
                paramArrSKU = [skuId];
            }
            await getBaseline(paramArrSKU).then(function () {
                elBaseline.val(baseline);
                baselineValue = baseline;
            });
        } else {
            baselineValue = (parseFloat(elBaseline.val().replace(/,/g, '')));
        }
    }
    //</editor-fold>

    //<editor-fold desc="Uplift">
    if (dataCalculator['uplift'] === 0) {
        elUplift.val("");
        upliftValue = upliftValue * 100;
    } else if (dataCalculator['uplift'] === 1) {
        upliftValue = (parseFloat(elUplift.val().replace(/,/g, '')) + 100);
    } else if (dataCalculator['uplift'] === 2) {
        if (!keyInput) {
            await upliftFormula();
            upliftValue = (parseFloat(elUplift.val().replace(/,/g, '')));
        } else {
            upliftValue = (parseFloat(elUplift.val().replace(/,/g, '')));
        }
    }
    //</editor-fold>

    //<editor-fold desc="Total Sales">
    if (dataCalculator['totalSales'] === 0) {
        elTotalSales.val("");
    } else if (dataCalculator['totalSales'] === 2) {
        if (!keyInput) {
            await getTotalSales().then(function () {
                elTotalSales.val(totalSales);
                totalSalesValue = totalSales;
            });
        } else {
            totalSalesValue = (parseFloat(elTotalSales.val().replace(/,/g, '')));
        }
    }
    //</editor-fold>

    //<editor-fold desc="Sales Contribution">
    if (dataCalculator['salesContribution'] === 0) {
        elSalesContribution.val("");
        salesContributionValue = salesContributionValue * 100;
    } else if (dataCalculator['salesContribution'] === 1) {
        salesContributionValue = parseFloat(elSalesContribution.val().replace(/,/g, ''));
    } else if (dataCalculator['salesContribution'] === 2) {
        salesContributionValue = salesContributionValue * 100;
        elSalesContribution.val("");
    }
    //</editor-fold>

    //<editor-fold desc="Stores Coverage">
    if (dataCalculator['storesCoverage'] === 0) {
        elStoresCoverage.val("");
        storesCoverageValue = storesCoverageValue * 100;
    } else if (dataCalculator['storesCoverage'] === 1) {
        storesCoverageValue = parseFloat(elStoresCoverage.val().replace(/,/g, ''));
    } else if (dataCalculator['storesCoverage'] === 2) {
        elStoresCoverage.val("");
        storesCoverageValue = storesCoverageValue * 100;
    }
    //</editor-fold>

    //<editor-fold desc="Redemption Rate">
    if (dataCalculator['redemptionRate'] === 0) {
        elRedemptionRate.val("");
        redemptionRateValue = redemptionRateValue * 100;
    } else if (dataCalculator['redemptionRate'] === 1) {
        redemptionRateValue = parseFloat(elRedemptionRate.val().replace(/,/g, ''));
    } else if (dataCalculator['redemptionRate'] === 2) {
        if (!keyInput) {
            await redemptionRateFormula();
            redemptionRateValue = parseFloat(elRedemptionRate.val().replace(/,/g, ''));
        } else {
            redemptionRateValue = parseFloat(elRedemptionRate.val().replace(/,/g, ''));
        }
    }
    //</editor-fold>

    //<editor-fold desc="CR">
    if (dataCalculator['cr'] === 0) {
        elCr.val("");
        crValue = crValue * 100;
    } else if (dataCalculator['cr'] === 1) {
        crValue = parseFloat(elCr.val().replace(/,/g, ''));
    } else if (dataCalculator['cr'] === 2) {
        if (!keyInput) {
            if (mainActivityDesc === "Non Running Rate - Fix Value") {
                await crFormula();
                crValue = parseFloat(elCr.val().replace(/,/g, ''));
            } else {
                await getCR().then(function () {
                    elCr.val(cr);
                    crValue = cr;
                });
            }
        } else {
            crValue = cr;
        }
    }
    //</editor-fold>

    let costValue = Math.round(baselineValue * ((upliftValue) / 100) * totalSalesValue * (salesContributionValue / 100) * (storesCoverageValue / 100) * (redemptionRateValue / 100) * (crValue / 100));
    if (!isFinite(costValue)) {
        return 0;
    } else {
        return costValue;
    }
}

const recalculateCostRunningRate = async (data) => {
    let baselineValue = 1;
    let upliftValue = 1;
    let totalSalesValue = 1;
    let salesContributionValue = 1;
    let storesCoverageValue = 1;
    let redemptionRateValue = 1;
    let crValue = 1;

    //<editor-fold desc="Set Config Calculator">
    let dataCalculator;
    let subActivitySelected = elSubActivity.select2('data');
    let channelId = parseInt(elChannel.val());
    for (let i=0; i<configCalculator.length; i++) {
        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
            dataCalculator = configCalculator[i];
        }
    }
    //</editor-fold>

    //<editor-fold desc="Baseline">
    if (dataCalculator['baseline'] === 1) {
        baselineValue = data['baseline'];
    }
    //</editor-fold>

    //<editor-fold desc="Uplift">
    if (dataCalculator['uplift'] === 0) {
        upliftValue = upliftValue * 100;
    } else if (dataCalculator['uplift'] === 1) {
        upliftValue = (data['uplift'] + 100);
    }
    //</editor-fold>

    //<editor-fold desc="Total Sales">
    if (dataCalculator['totalSales'] === 0) {
        totalSalesValue = 1;
    } else if (dataCalculator['totalSales'] === 2) {
        await getTotalSales().then(function () {
            totalSalesValue = totalSales;
        });
    }
    //</editor-fold>

    //<editor-fold desc="Sales Contribution">
    if (dataCalculator['salesContribution'] === 0) {
        salesContributionValue = salesContributionValue * 100;
    } else if (dataCalculator['salesContribution'] === 1) {
        salesContributionValue = data['salesContribution'];
    } else if (dataCalculator['salesContribution'] === 2) {
        salesContributionValue = salesContributionValue * 100;
    }
    //</editor-fold>

    //<editor-fold desc="Stores Coverage">
    if (dataCalculator['storesCoverage'] === 0) {
        storesCoverageValue = storesCoverageValue * 100;
    } else if (dataCalculator['storesCoverage'] === 1) {
        storesCoverageValue = data['storesCoverage'];
    } else if (dataCalculator['storesCoverage'] === 2) {
        storesCoverageValue = storesCoverageValue * 100;
    }
    //</editor-fold>

    //<editor-fold desc="Redemption Rate">
    if (dataCalculator['redemptionRate'] === 0) {
        redemptionRateValue = redemptionRateValue * 100;
    } else if (dataCalculator['redemptionRate'] === 1) {
        redemptionRateValue = data['redemptionRate'];
    }
    //</editor-fold>

    //<editor-fold desc="CR">
    if (dataCalculator['cr'] === 0) {
        crValue = crValue * 100;
    } else if (dataCalculator['cr'] === 1) {
        crValue = data['cr'];
    } else if (dataCalculator['cr'] === 2) {
        await getCR().then(function () {
            crValue = cr;
        });
    }
    //</editor-fold>

    let costValue = Math.round(baselineValue * ((upliftValue) / 100) * totalSalesValue * (salesContributionValue / 100) * (storesCoverageValue / 100) * (redemptionRateValue / 100) * (crValue / 100));
    if (!isFinite(costValue)) {
        return 0;
    } else {
        return costValue;
    }
}

const getCategoryId = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/category/category-desc",
            type        : "GET",
            data        : {c: categoryShortDescEnc},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    return resolve(result['data']['Id']);
                }
            },
            complete: function() {

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

const getTotalSales = () => {
    return new Promise((resolve, reject) => {
        let pPeriod = $('#period').val();
        let pGroupBrandId = $('#groupBrandId').val();
        let pDistributorId = $('#distributorId').val();
        let pStartPromo = $('#startPromo').val();
        let pEndPromo = $('#endPromo').val();
        $.ajax({
            url         : "/promo/creation/total-sales-dc",
            type        : "GET",
            dataType    : 'json',
            data        : {
                'period'        : pPeriod,
                'groupBrandId'  : pGroupBrandId,
                'distributorId' : pDistributorId,
                'startPromo'    : pStartPromo,
                'endPromo'      : pEndPromo
            },
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    if (result['data'].length > 0) {
                        let data = result['data'][0];
                        totalSales = data['ssvalue'];
                    } else {
                        totalSales = 0;
                    }
                } else {
                    totalSales = 0;
                }
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
    });
}

const getCR = () => {
    return new Promise((resolve, reject) => {
        let pPeriod = $('#period').val();
        let pGroupBrandId = $('#groupBrandId').val();
        let pSubActivityId = $('#subActivityId').val();
        let pDistributor = $('#distributorId').val();
        let pChannelId = $('#channelId').val();
        $.ajax({
            url         : "/promo/creation/cr",
            type        : "GET",
            dataType    : 'json',
            data        : {
                'period' : pPeriod,
                'groupBrandId' : pGroupBrandId,
                'subActivityId' : pSubActivityId,
                'distributorId' : pDistributor,
                'subAccountId' : pChannelId
            },
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    if (result['data'].length > 0) {
                        let data = result['data'][0];
                        cr = data['tt'];
                    } else {
                        cr = 0;
                    }
                } else {
                    cr = 0;
                }
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
    });
}

const getBudget = () => {
    return new Promise((resolve, reject) => {
        let pCategoryId = categoryId;
        let pPeriod = $('#period').val();
        let pGroupBrandId = $('#groupBrandId').val();
        let pSubActivityTypeId = $('#subActivityTypeId').val();
        let pSubActivityId = $('#subActivityId').val();
        let pDistributorId = $('#distributorId').val();
        let pChannelId = $('#channelId').val();
        if (pPeriod && pGroupBrandId && pCategoryId && pSubActivityTypeId && pDistributorId && pChannelId) {
            let pSubCategoryId = 0;
            let pActivityId = 0;
            let pSubChannelId = 0;
            let pAccountId = 0;
            let pSubAccountId = 0;
            let btn = document.querySelector("#btn_save");
            btn.disabled = !0;
            if (blockUIBudget.isBlocked()) blockUIBudget.release();
            blockUIBudget.block();
            $.ajax({
                url         : "/promo/creation/budget",
                type        : "GET",
                dataType    : 'json',
                data        : {
                    'period' : pPeriod,
                    'groupBrandId' : pGroupBrandId,
                    'categoryId' : pCategoryId,
                    'subCategoryId' : pSubCategoryId,
                    'activityId' : pActivityId,
                    'subActivityId' : pSubActivityId,
                    'subActivityTypeId' : pSubActivityTypeId,
                    'distributorId' : pDistributorId,
                    'channelId' : pChannelId,
                    'subChannelId' : pSubChannelId,
                    'accountId' : pAccountId,
                    'subAccountId' : pSubAccountId,
                },
                async       : true,
                success: function(result) {
                    if (!result['error']) {
                        let data = result['data'];
                        if (data.length > 0) {
                            $('#budgetSourceName').val(data[0]['budgetname']);
                            if (statusApprovalCode === 'TP0') {
                                $('#remainingBudget').val('');
                            } else {
                                $('#remainingBudget').val(data[0]['RemainingBudget']);
                            }
                            remainingBudget = data[0]['RemainingBudget'];
                        } else {
                            $('#budgetSourceName').val('');
                            $('#remainingBudget').val('');
                            remainingBudget = 0;
                        }
                    } else {
                        $('#budgetSourceName').val('');
                        $('#remainingBudget').val('');
                        remainingBudget = 0;
                        Swal.fire({
                            title: "TT Console in Contractual Budget Not Found",
                            icon: "warning",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            confirmButtonText: "OK",
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                },
                complete: function() {
                    btn.disabled = !1;
                    blockUIBudget.release();
                    return resolve();
                },
                error: function (jqXHR)
                {
                    console.log(jqXHR.responseText);
                    return reject(jqXHR.responseText);
                }
            });
        } else {
            return resolve();
        }
    }).catch((e) => {
        console.log(e);
    });
}

const getListAttribute = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/promo-attribute",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    let data = result['data'];
                    entityList = data['entity'];
                    brandList = data['grpBrand'];
                    distributorList = data['distibutor'];
                    subActivityTypeList = [...new Map(data['subActivity'].map(item => [item['SubActivityTypeId'], item])).values()];
                    subActivityList = data['subActivity'];
                    channelList = data['channel'];
                    configCalculator = data['configCalculator'];
                }
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
                $('#review_file_label_'+row).text('');
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
        },
        error: function (jqXHR) {
            console.log(jqXHR)
            let elInfo = $('#info' + row);
            $('#attachment'+row).val('');
            $('#review_file_label_'+row).text('');
            $('#btn_delete' + row).attr('disabled', true);
            elInfo.removeClass('invisible').addClass('visible');
            elInfo.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
        }
    });
}

const getLatePromoDays = () => {
    return new Promise((resolve) => {
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
                onClose: function() {
                    elStartPromo.trigger('change');
                }
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
                onClose: function() {
                    elStartPromo.trigger('change');
                }
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
            onClose: function() {
                elStartPromo.trigger('change');
            }
        });
        return false;
    }
}

const ActivityDescFormula = () => {
    let elStartPromo = $('#startPromo');
    let strMonth = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Des']

    let startPromoYear = new Date(elStartPromo.val()).getFullYear();
    let startPromoMonth = new Date(elStartPromo.val()).getMonth();

    let strStartPromoMonth = strMonth[startPromoMonth];

    let periode_desc = [strStartPromoMonth, startPromoYear.toString()].join(' ');
    let subActivityDesc = '';
    if (elSubActivity.val()) {
        subActivityDesc = elSubActivity.select2('data')[0].text;
        strActivityName = subActivityDesc + ' ' + periode_desc;
    }
}

const loadDropdownGroupBrand = async (entityIdSelected, groupBrandIdSelected) => {
    let elBrand = $('#groupBrandId');
    let entityId = parseInt(entityIdSelected);
    elBrand.empty();
    let brandDropdown = [{id:'', text:''}];
    for (let i = 0; i < brandList.length; i++) {
        if (brandList[i]['entityId'] === entityId) {
            brandDropdown.push({
                id: brandList[i]['groupBrandId'],
                text: brandList[i]['groupBrandDesc']
            });
        }
    }
    elBrand.select2({
        placeholder: "Select a Brand",
        width: '100%',
        data: brandDropdown
    }).on('change', function () {
        // Revalidate the color field when an option is chosen
        fvPromo.revalidateField('groupBrandId');
    });
    elBrand.val(groupBrandIdSelected).trigger('change.select2');
}

const loadDropdownSubActivity = async (subActivityTypeIdSelected, subActivityIdSelected) => {
    let subActivityTypeId = parseInt(subActivityTypeIdSelected);
    elSubActivity.empty();
    let subActivityDropdown = [{id:'', text:''}];
    for (let i = 0; i < subActivityList.length; i++) {
        if (subActivityList[i]['SubActivityTypeId'] === subActivityTypeId) {
            subActivityDropdown.push({
                id: subActivityList[i]['SubActivityId'],
                text: subActivityList[i]['SubActivityDesc'],
                subActivityTypeId: subActivityList[i]['SubActivityTypeId'],
                subActivityTypeDesc: subActivityList[i]['SubActivityTypeDesc'],
                mainActivityId: subActivityList[i]['mainActivityId'],
                mainActivityDesc: subActivityList[i]['mainActivityDesc'],
            });
        }
    }
    elSubActivity.select2({
        placeholder: "Select a Sub Activity",
        width: '100%',
        data: subActivityDropdown
    }).on('change', function () {
        // Revalidate the color field when an option is chosen
        fvPromo.revalidateField('subActivityId');
    });
    elSubActivity.val(subActivityIdSelected).trigger('change.select2');
}

const resetCalculator = async () => {
    let elBaseline = $('#baseline');
    let elUplift = $('#uplift');
    let elTotalSales = $('#totalSales');
    let elSalesContribution = $('#salesContribution');
    let elStoresCoverage = $('#storesCoverage');
    let elRedemptionRate = $('#redemptionRate');
    let elCr = $('#cr');
    let elRoi = $('#roi');
    let elCost = $('#cost');

    elBaseline.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
    elUplift.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
    elTotalSales.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
    elSalesContribution.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
    elStoresCoverage.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
    elRedemptionRate.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
    elCr.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
    elRoi.val('');
    elCost.val('').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
}

const getData = (pId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/promo/send-back/dc/id",
            type: "GET",
            data: {id: pId},
            dataType: "JSON",
            success: async function (result) {
                if (!result['error']) {
                    let data = result['data'];
                    let promo = data['promo'];
                    if (method === "update") {
                        $('#txt_info_method').text('Edit ' + promo['refId']);
                    }
                    categoryId = promo['categoryId'];
                    $('#entityId').val(promo['entityId'] ?? '').trigger('change.select2');
                    await loadDropdownGroupBrand(promo['entityId'].toString(), promo['groupBrandId'].toString());
                    $('#distributorId').val(promo['distributorId'] ?? '').trigger('change.select2');

                    $('#subActivityTypeId').val(promo['subActivityTypeId'] ?? '').trigger('change.select2');
                    await loadDropdownSubActivity(promo['subActivityTypeId'].toString(), promo['subActivityId'].toString());

                    $('#channelId').val(promo['channelId'] ?? '').trigger('change.select2');

                    $('#startPromo').flatpickr({
                        altFormat: "d-m-Y",
                        altInput: true,
                        allowInput: true,
                        dateFormat: "Y-m-d",
                        disableMobile: "true",
                        minDate: "2025-01",
                        onClose: function() {
                            $('#startPromo').trigger('change');
                        }
                    }).setDate(promo['startPromo']);
                    $('#endPromo').flatpickr({
                        altFormat: "d-m-Y",
                        altInput: true,
                        allowInput: true,
                        dateFormat: "Y-m-d",
                        disableMobile: "true",
                        minDate: "2025-01",
                        onClose: function() {
                            $('#startPromo').trigger('change');
                        }
                    }).setDate(promo['endPromo']);

                    $('#mechanism').val(data['mechanism'][0]['mechanism'] ?? '');
                    $('#initiatorNotes').val(promo['initiator_notes']);

                    if (data['attachment']) {
                        let fileSource = "";
                        data['attachment'].forEach((item) => {
                            if (item['docLink'] === 'row' + parseInt(item['docLink'].replace('row', ''))) {
                                let elLabel = $('#review_file_label_' + parseInt(item['docLink'].replace('row', '')));
                                elLabel.text(item['fileName']).attr('title', item['fileName']);
                                elLabel.addClass('form-control-solid-bg');
                                $('#attachment' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', true);
                                $('#btn_download' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                                $('#btn_delete' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                                fileSource = file_host + "/assets/media/promo/" + item['promoId'] + "/" + item['docLink'] + "/" + item['fileName'];
                                const fileInput = document.querySelector('#attachment' + parseInt(item['docLink'].replace('row', '')));
                                fileInput.dataset.file = fileSource;
                            }
                        });
                    }

                    statusApprovalCode = promo['statusApprovalCode'];

                    let subActivitySelected = elSubActivity.select2('data');
                    let dataCalculator = null;
                    //<editor-fold desc="Selected Config Calculator">
                    let channelId = parseInt(elChannel.val());
                    for (let i=0; i<configCalculator.length; i++) {
                        if (subActivitySelected[0]['mainActivityId'] === configCalculator[i]['mainActivityId'] && channelId === configCalculator[i]['channelId']) {
                            dataCalculator = configCalculator[i];
                        }
                    }
                    //</editor-fold>

                    if (!dataCalculator) {
                        return Swal.fire({
                            title: swalTitle,
                            icon: "warning",
                            text: 'Promo calculator configuration not found',
                            showConfirmButton: true,
                            confirmButtonText: 'Confirm',
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                    dataMatrixCalculator = dataCalculator;

                    setConfigCalculator(dataCalculator);

                    $('#baseline').val('');
                    $('#uplift').val((promo['upLift'] === 0 ? '' : promo['upLift']));
                    $('#totalSales').val((promo['totalSales'] === 0 ? '' : promo['totalSales']));
                    $('#salesContribution').val((promo['salesContribution'] === 0 ? '' : promo['salesContribution']));
                    $('#storesCoverage').val((promo['storesCoverage'] === 0 ? '' : promo['storesCoverage']));
                    $('#redemptionRate').val((promo['redemptionRate'] === 0 ? '' : promo['redemptionRate']));
                    $('#cr').val((promo['cr'] === 0 ? '' : promo['cr']));
                    $('#cost').val((promo['cost'] === 0 ? '' : promo['cost']));

                    roiFormula();
                }
            },
            complete: function () {
                return resolve();
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

const saveData = async (modifyReason) => {
    let e = document.querySelector("#btn_save");

    blockUIHeader.block();
    blockUIBudget.block();
    blockUIAttachment.block();
    blockUIPromoCalculator.block();
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    await submitSave(modifyReason);
}

const submitSave = async function (modifyReason) {
    let e = document.querySelector("#btn_save");

    let regionData = [];

    let skuData = [];

    let elBaseline = $('#baseline');
    let elUplift = $('#uplift');
    let elTotalSales = $('#totalSales');
    let elSalesContribution = $('#salesContribution');
    let elStoresCoverage = $('#storesCoverage');
    let elRedemptionRate = $('#redemptionRate');
    let elCr = $('#cr');
    let elCost = $('#cost');

    let subActivitySelected = elSubActivity.select2('data');
    let mainActivityDesc = subActivitySelected[0]['mainActivityDesc'];

    let baseline = (elBaseline.val() === "" ? 0 : parseFloat(elBaseline.val().replace(/,/g, '')));
    let upLift = (elUplift.val() === "" ? 0 : parseFloat(elUplift.val().replace(' %', '').replace(/,/g, '')));
    let totalSalesSave = (elTotalSales.val() === "" ? 0 : parseFloat(elTotalSales.val().replace(/,/g, '').replace(/,/g, '')));
    let salesContribution = (elSalesContribution.val() === "" ? 0 : parseFloat(elSalesContribution.val().replace(' %', '').replace(/,/g, '')));
    let storesCoverage = (elStoresCoverage.val() === "" ? 0 : parseFloat(elStoresCoverage.val().replace(' %', '').replace(/,/g, '')));
    let redemptionRate = (elRedemptionRate.val() === "" ? 0 : parseFloat(elRedemptionRate.val().replace(' %', '').replace(/,/g, '')));
    let crSave = (elCr.val() === "" ? 0 : parseFloat(elCr.val().replace(' %', '').replace(/,/g, '')));
    let costSave = (elCost.val() === "" ? 0 : parseFloat(elCost.val().replace(/,/g, '')));

    let dataCalculatorSave = [{
        baseline: baseline,
        uplift: upLift,
        totalSales: totalSalesSave,
        salesContribution: salesContribution,
        storesCoverage: storesCoverage,
        redemptionRate: redemptionRate,
        cr: crSave,
        cost: costSave,
    }];

    if (mainActivityDesc === "Running Rate") {
        await getTotalSales();
        await getCR();
        totalSalesSave = totalSales;
        crSave = cr;
        salesContribution = (elSalesContribution.val() === "" ? 0 : parseFloat(elSalesContribution.val().replace(' %', '').replace(/,/g, '')));
        costSave = await recalculateCostRunningRate(dataCalculatorSave[0]);
    }

    let mechanismData = [{
        id: 0,
        mechanism: $('#mechanism').val(),
        notes: '',
        productId: 0,
        baseline: baseline,
        uplift: upLift,
        totalSales: totalSalesSave,
        salesContribution: salesContribution,
        storesCoverage: storesCoverage,
        redemptionRate: redemptionRate,
        cr: crSave,
        cost: costSave,
    }];

    let formData = new FormData($('#form_promo')[0]);
    formData.append('promoId', promoId);
    formData.append('categoryId', categoryId);
    formData.append('baseline', baseline.toString());
    formData.append('upLift', upLift.toString());
    formData.append('totalSales', totalSalesSave.toString());
    formData.append('salesContribution', salesContribution.toString());
    formData.append('storesCoverage', storesCoverage.toString());
    formData.append('redemptionRate', redemptionRate.toString());
    formData.append('cr', crSave.toString());
    formData.append('cost', costSave.toString());
    formData.append('region', JSON.stringify(regionData));
    formData.append('sku', JSON.stringify(skuData));
    formData.append('mechanism', JSON.stringify(mechanismData));
    formData.append('modifReason', modifyReason);
    formData.append('activityDesc', strActivityName);
    formData.append('fileName1', $('#review_file_label_1').text());
    formData.append('fileName2', $('#review_file_label_2').text());
    formData.append('fileName3', $('#review_file_label_3').text());
    formData.append('fileName4', $('#review_file_label_4').text());
    formData.append('notes_message', $('#notes_message').val());
    formData.append('promoId', promoId);

    let url = "/promo/send-back/revamp/update-dc";

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
                    if (result['attachment2'] !== 'No File Upload attachment2') {
                        let elInfo2 = $('#info2');
                        if (result['attachment2'] === 'Upload Successfully') {
                            elInfo2.removeClass('invisible').addClass('visible');
                            elInfo2.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo2.removeClass('invisible').addClass('visible');
                            elInfo2.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                    if (result['attachment3'] !== 'No File Upload attachment3') {
                        let elInfo3 = $('#info3');
                        if (result['attachment3'] === 'Upload Successfully') {
                            elInfo3.removeClass('invisible').addClass('visible');
                            elInfo3.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo3.removeClass('invisible').addClass('visible');
                            elInfo3.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                    if (result['attachment4'] !== 'No File Upload attachment4') {
                        let elInfo4 = $('#info4');
                        if (result['attachment4'] === 'Upload Successfully') {
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
                    window.location.href = '/promo/send-back';
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
            blockUIHeader.release();
            blockUIBudget.release();
            blockUIAttachment.release();
            blockUIPromoCalculator.release();
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

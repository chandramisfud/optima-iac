'use strict';

let swalTitle = "Promo Creation";
let url_str = new URL(window.location.href);
let method = url_str.searchParams.get("method");
let promoId = url_str.searchParams.get("promoId");
let categoryShortDescEnc = url_str.searchParams.get("c");

let dialerObject;
let dt_mechanism, dt_sku, dt_sku_selected;
let categoryId;
let entityList = [], brandList = [], subCategoryList = [], activityList = [], subActivityList = [], distributorList = [], channelList = [], subChannelList = [], accountList = [], subAccountList = [], configCalculator = [];
let regionList = [], skuList = [];

let elPeriod = $('#period');
let elSubActivity = $('#subActivityId'), elChannel = $('#channelId');
let elDtSKU = $('#dt_sku'), elDtSKUSelected = $('#dt_sku_selected');

let elDtMechanismSource = $('#dt_mechanism_source'), elDtMechanismInput = $('#dt_mechanism_input');
let autoMechanism = false, arrSourceMechanism = [], dt_mechanism_source, dt_mechanism_input;
let skuIdAutoMechanism = null, methodDetailMechanism = 'add', trIndexAutoMechanism;
let indexManualMechanism = 1, arrManualMechanismInput = [1];

let baseline = 0, totalSales = 0, cr = 0, cost = 0, remainingBudget = 0, dataMatrixCalculator;
let statusApprovalCode = 'TP0';
let mainActivityBefore;
let initiatorNotes = "";

let fvPromo;
const formPromo = document.getElementById('form_promo');
let uuid = generateUUID(5);

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

//<editor-fold desc="Form Validation Header">
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
                // check cross Year & start date > end date
                crossYear: {

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
        distributorId: {
            validators: {
                notEmpty: {
                    message: "Please select a distributor"
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
//</editor-fold>

//<editor-fold desc="Loading Animation">
let targetHeader = document.querySelector(".card_header");
let blockUIHeader = new KTBlockUI(targetHeader, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetMechanism = document.querySelector(".card_mechanism");
let blockUIMechanism = new KTBlockUI(targetMechanism, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetBudget = document.querySelector(".card_budget");
let blockUIBudget = new KTBlockUI(targetBudget, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetRegion = document.querySelector(".card_region");
let blockUIRegion = new KTBlockUI(targetRegion, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetSKU = document.querySelector(".card_sku");
let blockUISKU = new KTBlockUI(targetSKU, {
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

    dt_mechanism = $('#dt_mechanism').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: false,
        scrollY: "40vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                width: 10,
                data: 'no',
                orderable: false,
                className: 'text-nowrap text-center align-top',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
            {
                title: 'Mechanism',
                targets: 1,
                data: 'mechanism',
                className: 'align-top',
            },
            {
                title: 'Notes',
                targets: 2,
                data: 'notes',
                className: 'align-top',
            },
            {
                title: 'Cost',
                width: 100,
                targets: 3,
                data: 'cost',
                className: 'text-nowrap align-top text-end pe-1',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
        footerCallback: function (row, data) {
            let total = 0;
            if (data.length > 0) {
                if (autoMechanism) {
                    for (let i=0; i<data.length; i++) {
                        total = total + data[i]['cost'];
                    }
                } else {
                    total = data[0]['cost'];
                }
            }
            $('#totalCost').val(total);
        }
    });

    dt_sku = elDtSKU.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "27vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Double click to select SKU',
                data: 'skuDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $('#dt_sku_search').on('keyup', function () {
        dt_sku.search(this.value, false, false).draw();
    });

    elDtSKU.on('dblclick', 'tr', function () {
        if (!autoMechanism) {
            let dataSKU = dt_sku.row( this ).data();
            let dataSKUSelected = dt_sku_selected.rows().data().toArray();
            if (dt_sku_selected.rows().data().length >= 1) {
                let filter = {
                    skuId: dataSKU['skuId']
                };
                dataSKUSelected = dataSKUSelected.filter(function (item) {
                    for (let key in filter) {
                        if (item[key] === undefined || item[key] !== filter[key]) {
                            return false;
                        }
                    }
                    return true;
                });
                if (dataSKUSelected.length >= 1) {
                    return Swal.fire({
                        title: "Select SKU",
                        text: "SKU already exist",
                        icon: "warning",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                } else {
                    let dataSelected = [];
                    dataSelected['skuId'] = dataSKU['skuId'];
                    dataSelected['skuDesc'] = dataSKU['skuDesc'];
                    dt_sku_selected.row.add(dataSelected).draw();
                }
            } else {
                let dataSelected = [];
                dataSelected['skuId'] = dataSKU['skuId'];
                dataSelected['skuDesc'] = dataSKU['skuDesc'];
                dt_sku_selected.row.add(dataSelected).draw();
            }
        }
    });

    dt_sku_selected = elDtSKUSelected.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "27vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Selected SKU',
                data: 'skuDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $('#dt_sku_selected_search').on('keyup', function () {
        dt_sku_selected.search(this.value, false, false).draw();
    });

    elDtSKUSelected.on( 'dblclick', 'tr', function () {
        if (!autoMechanism) {
            let tr = this.closest("tr");
            let trIndex = dt_sku_selected.row(tr).index();
            dt_sku_selected.row(trIndex).remove().draw();
        }
    });

    //<editor-fold desc="Auto Mechanism Input">
    dt_mechanism_source = elDtMechanismSource.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "15vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Double click to select mechanism',
                data: 'mechanism',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $('#dt_mechanism_source_search').on('keyup', function () {
        dt_mechanism_source.search(this.value, false, false).draw();
    });

    elDtMechanismSource.on('dblclick', 'tr', async function () {
        let dataMechanism = dt_mechanism_source.row( this ).data();
        let data = dt_mechanism_input.rows().data().toArray();
        if (dt_mechanism_input.rows().data().length >= 1) {
            let filter = {
                mechanism: dataMechanism['mechanism']
            };
            data = data.filter(function (item) {
                for (let key in filter) {
                    if (item[key] === undefined || item[key] !== filter[key]) {
                        return false;
                    }
                }
                return true;
            });
            if (data.length >= 1) {
                return Swal.fire({
                    title: 'Form Mechanism',
                    text: "Mechanism is already exist",
                    icon: "warning",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-primary"}
                });
            }

            $('#baseline').val('0');
            $('#uplift').val('100');
            $('#totalSales').val('0');
            $('#salesContribution').val('100');
            $('#storesCoverage').val('100');
            $('#redemptionRate').val('100');
            $('#cr').val('100');
            $('#roi').val('0');
            $('#cost').val('0');
            skuIdAutoMechanism = dataMechanism['productId'];
            $('#skuDesc').val(dataMechanism['product']);
            $('#mechanism').val(dataMechanism['mechanism']);
            $('#notes').val('');
        } else {
            $('#baseline').val('0');
            $('#uplift').val('100');
            $('#totalSales').val('0');
            $('#salesContribution').val('100');
            $('#storesCoverage').val('100');
            $('#redemptionRate').val('100');
            $('#cr').val('100');
            $('#roi').val('0');
            $('#cost').val('0');
            skuIdAutoMechanism = dataMechanism['productId'];
            $('#skuDesc').val(dataMechanism['product']);
            $('#mechanism').val(dataMechanism['mechanism']);
            $('#notes').val('');
        }
        blockUIPromoCalculator.block();
        await costFormula(false, dataMechanism['productId']);
        blockUIPromoCalculator.release();
        methodDetailMechanism = 'add';

        $('#btn_mechanism_save').attr('disabled', true);
    });

    dt_mechanism_input = elDtMechanismInput.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: false,
        scrollY: "20vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: '',
                width: 10,
                targets: 0,
                orderable: false,
                className: 'text-nowrap text-center align-top',
                render: function () {
                    return '<button class="btn btn-icon btn-sm btn-optima btn-clean h-20px btn_edit_mechanism_auto" title="edit"><i class="fa fa-edit fs-6"></i></button>' +
                        '<button class="btn btn-icon btn-sm btn-optima btn-clean h-20px btn_delete_mechanism_auto" title="remove" ><i class="fa fa-trash fs-6"/></i></button>';
                }
            },
            {
                title: 'No',
                width: 10,
                targets: 1,
                data: 'no',
                orderable: false,
                className: 'text-nowrap text-center align-top',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
            {
                title: 'SKU',
                targets: 2,
                data: 'skuDesc',
                className: 'text-nowrap align-top',
            },
            {
                title: 'Mechanism',
                targets: 3,
                data: 'mechanism',
                className: 'align-top',
            },
            {
                title: 'Notes',
                targets: 4,
                data: 'notes',
                className: 'align-top',
            },
            {
                title: 'Cost',
                targets: 5,
                width: 150,
                data: 'cost',
                className: 'text-nowrap align-top text-end pe-1',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    elDtMechanismInput.on('click', '.btn_edit_mechanism_auto', async function () {
        methodDetailMechanism = 'edit';
        let tr = this.closest("tr");
        let trData = dt_mechanism_input.row(tr).data();
        trIndexAutoMechanism = dt_mechanism_input.row(tr).index();

        let elSkuDesc = $('#skuDesc');
        let elMechanism = $('#mechanism');
        let elNotes = $('#notes');
        let elBaseline = $('#baseline');
        let elUplift = $('#uplift');
        let elTotalSales = $('#totalSales');
        let elSalesContribution = $('#salesContribution');
        let elStoresCoverage = $('#storesCoverage');
        let elRedemptionRate = $('#redemptionRate');
        let elCr = $('#cr');
        let elCost = $('#cost');

        elSkuDesc.val(trData['skuDesc']);
        elMechanism.val(trData['mechanism']);
        elNotes.val(trData['notes']);
        elBaseline.val(trData['baseline']);
        elUplift.val(trData['uplift']);
        elTotalSales.val(trData['totalSales']);
        elSalesContribution.val(trData['salesContribution']);
        elStoresCoverage.val(trData['storesCoverage']);
        elRedemptionRate.val(trData['redemptionRate']);
        elCr.val(trData['cr']);
        elCost.val(trData['cost']);
        roiFormula();

        methodDetailMechanism = 'edit';

        $('#btn_mechanism_save').attr('disabled', true);
    });

    elDtMechanismInput.on('click', '.btn_delete_mechanism_auto', async function () {
        let tr = this.closest("tr");
        let trIndex = dt_mechanism_input.row(tr).index();
        let dataExistingMechanismInput = dt_mechanism_input.rows().data();
        dt_mechanism_input.row(trIndex).remove().draw();
        for (let i=0; i<dataExistingMechanismInput.length; i++) {
            dt_mechanism_input.row(i).every(function () {
                let data = this.data();
                data['no'] = i+1;

                this.invalidate();
            });
        }
        dt_mechanism_input.draw();
    });
    //</editor-fold>

    blockUIHeader.block();
    blockUIMechanism.block();
    blockUIBudget.block();
    blockUIRegion.block();
    blockUISKU.block();
    blockUIAttachment.block();
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

        // sub category
        let subCategoryDropdown = [];
        for (let i = 0; i < subCategoryList.length; i++) {
            if (subCategoryList[i]['categoryid'] === categoryId) {
                subCategoryDropdown.push({
                    id: subCategoryList[i]['subcategoryid'],
                    text: subCategoryList[i]['subcategorydesc']
                });
            }
        }
        $('#subCategoryId').select2({
            placeholder: "Select a Sub Category",
            width: '100%',
            data: subCategoryDropdown
        }).on('change', function () {
            // Revalidate the color field when an option is chosen
            fvPromo.revalidateField('subCategoryId');
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

        // channel
        let channelDropdown = [];
        for (let i = 0; i < channelList.length; i++) {
            channelDropdown.push({
                id: channelList[i]['ChannelId'],
                text: channelList[i]['ChannelDesc']
            });
        }
        elChannel.select2({
            placeholder: "Select a Channel",
            width: '100%',
            data: channelDropdown
        }).on('change', function () {
            // Revalidate the color field when an option is chosen
            fvPromo.revalidateField('channelId');
        });

        // region
        let regionDropdown = [];
        for (let i = 0; i < regionList.length; i++) {
            regionDropdown.push({
                id: regionList[i]['regionId'],
                text: regionList[i]['regionDesc']
            });
        }
        $('#regionId').select2({
            placeholder: "Select Region",
            width: '100%',
            data: regionDropdown
        });

        if (method === "update") {
            await getData(promoId);
            await getBudget();
        }
        if (method === "duplicate") {
            await getData(promoId);
            await getBudget();
        }

        blockUIHeader.release();
        blockUIMechanism.release();
        blockUIBudget.release();
        blockUIRegion.release();
        blockUISKU.release();
        blockUIAttachment.release();
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

    if ($(this).val()) await getMechanism();
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
            //Khusus untuk mengecek validate jika backdate endpromo yang diubah pertama
            startPromo.trigger('change');
        }
    });
    startPromo.trigger('change');
    ActivityDescFormula();

    if ($(this).val()) await getMechanism();
    await getBudget();
    fvPromo.revalidateField('startPromo');
});
//</editor-fold>

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
    elPeriod.val(new Date(elStart.val()).getFullYear()).on('change');
    ActivityDescFormula();
    if ($(this).val()) await getMechanism(true);
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
    ActivityDescFormula();
    elPeriod.val(new Date(elEnd.val()).getFullYear()).on('change');
    if ($(this).val()) await getMechanism(true);
    fvPromo.revalidateField('startPromo');
});
//</editor-fold>

//<editor-fold desc="SKU Matrix Event">
$('#entityId').on('change', async function () {
    await loadDropdownGroupBrand($(this).val(), '');
    if ($(this).val()) await getMechanism();
});

$('#groupBrandId').on('change', async function () {
    await loadListSKU($(this).val());
    await getBudget();
    if ($(this).val()) await getMechanism();
});

$('#distributorId').on('change', async function () {
    await getBudget();
    await getMechanism();
});
//</editor-fold>

//<editor-fold desc="Activity Matrix Event">
$('#subCategoryId').on('change', async function () {
    await loadDropdownActivity($(this).val(), '');
    await getBudget();
});

$('#activityId').on('change', async function () {
    await loadDropdownSubActivity($(this).val(), '');
    if ($(this).val()) await getMechanism();
    await getBudget();
});

elSubActivity.on('change', async function () {
    ActivityDescFormula();
    if ($(this).val()) await getMechanism();
    await getBudget();
});
//</editor-fold>

//<editor-fold desc="Account Matrix Event">
elChannel.on('change', async function () {
    await loadDropdownSubChannel($(this).val(), '');
    if ($(this).val()) await getMechanism();
    await getBudget();
});

$('#subChannelId').on('change', async function () {
    await loadDropdownAccount($(this).val(), '');
    await getBudget();
});

$('#accountId').on('change', async function () {
    await loadDropdownSubAccount($(this).val(), '');
    await getBudget();
});

$('#subAccountId').on('change', async function () {
    await getBudget();
    await getMechanism();
});
//</editor-fold>

$('#btn_select_all_sku').on('click', function () {
    if (!autoMechanism) {
        let dataSKu = dt_sku.rows().data();
        dt_sku_selected.clear().draw();
        dt_sku_selected.rows.add(dataSKu).draw();
    }
});

$('#btn_deselect_all_sku').on('click', function () {
    if (!autoMechanism) {
        dt_sku_selected.clear().draw();
    }
});

//<editor-fold desc="Region Event">
$('#regionId').on('change', function () {
    let textRegion = $(this).select2('data');
    if (textRegion.length > 0) {
        if (textRegion[0].text.toLowerCase() === "region - all") {
            $('#regionId option').each(function () {
                if (this.text.toLowerCase() !== "region - all") {
                    $(this).prop('disabled', true);
                } else {
                    $(this).prop('disabled', false);
                }
            });
        } else {
            $('#regionId option').each(function () {
                if (this.text.toLowerCase() === "region - all") {
                    $(this).prop('disabled', true);
                } else {
                    $(this).prop('disabled', false);
                }
            });
        }
    } else {
        $('#regionId option').each(function () {
            $(this).prop('disabled', false);
        });
    }
}).on('select2:clear', function () {
    $('#regionId').val([]).trigger('change');
});
//</editor-fold>

//<editor-fold desc="Mechanism Method">
$('#btn_mechanism_edit').on('click', function () {
    let elRegion = $('#regionId');
    $('#btn_mechanism_save').attr('disabled', false);
    fvPromo.validate().then(async function (status) {
        if (status === "Valid") {
            let regionVal = elRegion.val();
            if (regionVal.length < 1) {
                return $('#regionInvalid').removeClass('d-none');
            } else {
                $('#regionInvalid').addClass('d-none');
            }

            let skuSelected = dt_sku_selected.rows().data();
            if (skuSelected.length < 1 && !autoMechanism) {
                $('#skuInvalid').removeClass('d-none');
                return Swal.fire({
                    title: swalTitle,
                    icon: "warning",
                    text: 'Please select a sku',
                    showConfirmButton: true,
                    confirmButtonText: 'Confirm',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            } else {
                $('#skuInvalid').addClass('d-none');
            }

            let dataCalculator = null;

            let brandSelected = $('#groupBrandId').select2('data');
            let brandText = (brandSelected[0]['text'] ?? '-');
            let subCategorySelected = $('#subCategoryId').select2('data');
            let subCategoryText = (subCategorySelected[0]['text'] ?? '-');
            let subActivitySelected = elSubActivity.select2('data');
            let subActivityText = (subActivitySelected[0]['text'] ?? '-');
            let activityDescText = ($('#activityDesc').val() ?? '-');
            let activityPeriodText = (formatDateOptima($('#startPromo').val()) + ' to ' + formatDateOptima($('#endPromo').val()) ?? '-');
            let distributorSelected = $('#distributorId').select2('data');
            let distributorText = (distributorSelected[0]['text'] ?? '-');
            let channelSelected = elChannel.select2('data');
            let channelText = (channelSelected[0]['text'] ?? '-');
            let accountSelected = $('#accountId').select2('data');
            let accountText = (accountSelected[0]['text'] ?? '-');
            let subAccountSelected = $('#subAccountId').select2('data');
            let subAccountText = (subAccountSelected[0]['text'] ?? '-');
            let selectedSKU = dt_sku_selected.rows().data();
            let arrSKU = [];
            for (let i = 0; i < selectedSKU.length; i++) {
                arrSKU.push(selectedSKU[i]['skuDesc']);
            }
            let skuText = (arrSKU.join(', ') ?? '-');


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
            setConfigCalculator(dataCalculator);

            let elPrimaryForm = $('#primaryForm');
            let elMechanismForm = $('#MechanismMatrixForm');
            if (elMechanismForm.hasClass('d-none')) {
                elMechanismForm.removeClass('d-none');
                $('#btn_save').attr('disabled', true);
                elPrimaryForm.addClass('d-none');
                let elAutoMechanismForm = $('#autoMechanismFormSection');
                let elAutoMechanism = $('#autoMechanismSourceSection');
                let elAutoMechanismInput = $('#autoMechanismInputSection');
                let elAutoMechanismList = $('#autoMechanismListSection');
                let elManualMechanism = $('#manualMechanismInputSection');
                dataMatrixCalculator = dataCalculator;

                if (autoMechanism) {
                    //<editor-fold desc="Set Details Info">
                    $('#mainActivityText').text(`Promo ID Details - ${subActivitySelected[0]['mainActivityDesc'] ?? '-'}`);
                    $('#txtInfoGroupBrand').text(brandText);
                    $('#txtInfoSubCategory').text(subCategoryText);
                    $('#txtInfoSubActivity').text(subActivityText);
                    $('#txtInfoActivityDesc').text(activityDescText);
                    $('#txtInfoActivityPeriod').text(activityPeriodText);
                    $('#txtInfoDistributor').text(distributorText);
                    $('#txtInfoChannel').text(channelText);
                    $('#txtInfoAccount').text(accountText);
                    $('#txtInfoSubAccount').text(subAccountText);
                    $('#txtInfoSKU').text(skuText);
                    //</editor-fold>

                    dt_mechanism_input.clear().draw();
                    skuIdAutoMechanism = null;
                    $('#skuDesc').val('');
                    $('#mechanism').val('');
                    $('#notes').val('');
                    dt_mechanism_source.clear().draw();

                    elAutoMechanismForm.removeClass('d-none');
                    elAutoMechanism.removeClass('d-none');
                    elAutoMechanismInput.removeClass('d-none');
                    elAutoMechanismList.removeClass('d-none');
                    elManualMechanism.addClass('d-none');
                    $('#btn_calculator_save').removeClass('d-none');

                    dt_mechanism_source.rows.add(arrSourceMechanism).draw();
                    let dataMechanism = dt_mechanism.rows().data();
                    if (dataMechanism.length > 0) {
                        for (let i=0; i<dataMechanism.length; i++) {
                            if (dataMechanism[i]['skuId'] === 0) {
                                dataMechanism.splice(i, 1);
                            }
                        }
                        for (let i=0; i<dataMechanism.length; i++) {
                            dataMechanism[i]['no'] = i+1;
                        }
                    }
                    dt_mechanism_input.rows.add(dataMechanism).draw();
                } else {
                    //<editor-fold desc="Set Details Info">
                    $('#mainActivityTextInput').text(`Promo ID Details - ${subActivitySelected[0]['mainActivityDesc'] ?? '-'}`);
                    $('#txtInfoGroupBrandInput').text(brandText);
                    $('#txtInfoSubCategoryInput').text(subCategoryText);
                    $('#txtInfoSubActivityInput').text(subActivityText);
                    $('#txtInfoActivityDescInput').text(activityDescText);
                    $('#txtInfoActivityPeriodInput').text(activityPeriodText);
                    $('#txtInfoDistributorInput').text(distributorText);
                    $('#txtInfoChannelInput').text(channelText);
                    $('#txtInfoAccountInput').text(accountText);
                    $('#txtInfoSubAccountInput').text(subAccountText);
                    $('#txtInfoSKUInput').text(skuText);
                    //</editor-fold>

                    elAutoMechanismForm.addClass('d-none');
                    elAutoMechanism.addClass('d-none');
                    elAutoMechanismInput.addClass('d-none');
                    elAutoMechanismList.addClass('d-none');
                    elManualMechanism.removeClass('d-none');
                    $('#btn_calculator_save').addClass('d-none');

                    let elManualMechanismList = $('#manualMechanismList');
                    elManualMechanismList.html('');

                    let dataMechanism = dt_mechanism.rows().data();
                    //<editor-fold desc="Mechanism Manual Input Is Exist Or No">
                    if (dataMechanism.length > 0) {
                        let arrMechanism = [];
                        indexManualMechanism = 1;
                        for (let i=0; i<dataMechanism.length; i++) {
                            arrMechanism.push(i+1);
                            elManualMechanismList.append(`
                                <div class="row mb-3" id="mechanismRow${i+1}">
                                    <div class="col-12">
                                        <div class="d-flex justify-content-between">
                                            <label class="text-nowrap my-auto me-10" for="manual_mechanism_${i+1}" id="lbl_manual_mechanism_${i+1}">Mechanism ${i+1}</label>
                                            <input type="text" class="form-control form-control-sm" name="manual_mechanism_${i+1}" id="manual_mechanism_${i+1}" value="${dataMechanism[i]['mechanism']}" autocomplete="off"/>
                                            <button class="btn btn-sm btn-outline-optima ms-2 ${(i===0 && dataMechanism.length === 1 ? 'invisible' : '' )}" id="btn_manual_mechanism_delete_${i+1}" title="Delete" value="${i+1}">
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-outline-optima ms-2 ${(i+1 !== dataMechanism.length ? 'invisible' : '')}" id="btn_manual_mechanism_add_${i+1}" title="Add Row">
                                                <span class="fa fa-plus"></span>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            `);

                            $(`#btn_manual_mechanism_add_${i+1}`).on('click', async function () {
                                addManualMechanism($(this));
                            });

                            $(`#btn_manual_mechanism_delete_${i+1}`).on('click', async function () {
                                deleteManualMechanism($(this).val());
                            });
                            indexManualMechanism++;
                        }

                        $('#baseline').val((dataMechanism[0]['baseline'] === 0 ? '' : dataMechanism[0]['baseline']));
                        $('#uplift').val((dataMechanism[0]['uplift'] === 0 ? '' : dataMechanism[0]['uplift']));
                        $('#totalSales').val((dataMechanism[0]['totalSales'] === 0 ? '' : dataMechanism[0]['totalSales']));
                        $('#salesContribution').val((dataMechanism[0]['salesContribution'] === 0 ? '' : dataMechanism[0]['salesContribution']));
                        $('#storesCoverage').val((dataMechanism[0]['storesCoverage'] === 0 ? '' : dataMechanism[0]['storesCoverage']));
                        $('#redemptionRate').val((dataMechanism[0]['redemptionRate'] === 0 ? '' : dataMechanism[0]['redemptionRate']));
                        $('#cr').val((dataMechanism[0]['cr'] === 0 ? '' : dataMechanism[0]['cr']));
                        $('#roi').val((dataMechanism[0]['roi'] === 0 ? '' : dataMechanism[0]['roi']));
                        $('#cost').val((dataMechanism[0]['cost'] === 0 ? '' : dataMechanism[0]['cost']));

                        blockUIPromoCalculator.block();
                        await costFormula();
                        roiFormula();
                        blockUIPromoCalculator.release();

                        arrManualMechanismInput = arrMechanism;
                    } else {
                        arrManualMechanismInput = [1];
                        indexManualMechanism = 1;
                        elManualMechanismList.append(`
                                <div class="row mb-3" id="mechanismRow1">
                                    <div class="col-12">
                                        <div class="d-flex justify-content-between">
                                            <label class="text-nowrap my-auto me-10" for="manual_mechanism_1" id="lbl_manual_mechanism_1">Mechanism 1</label>
                                            <input type="text" class="form-control form-control-sm" name="manual_mechanism_1" id="manual_mechanism_1" value="" autocomplete="off"/>
                                            <button class="btn btn-sm btn-outline-optima ms-2 invisible" id="btn_manual_mechanism_delete_1" title="Delete" value="1">
                                                <span class="fa fa-trash-alt"> </span>
                                            </button>
                                            <button class="btn btn-sm btn-outline-optima ms-2" id="btn_manual_mechanism_add_1" title="Add Row">
                                                <span class="fa fa-plus"></span>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            `);

                        $(`#btn_manual_mechanism_add_1`).on('click', async function () {
                            addManualMechanism($(this));
                        });

                        $(`#btn_manual_mechanism_delete_1`).on('click', async function () {
                            deleteManualMechanism($(this).val());
                        });

                        blockUIPromoCalculator.block();
                        await costFormula();
                        roiFormula();
                        blockUIPromoCalculator.release();
                    }
                    //</editor-fold>
                }
            } else {
                elPrimaryForm.removeClass('d-none');
                $('#btn_save').attr('disabled', false);
                elMechanismForm.addClass('d-none');
            }
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        }
    });
});

$('#btn_mechanism_save').on('click', async function () {
    if (autoMechanism) {
        //<editor-fold desc="Save Mechanism From Master">
        let dataMechanism = dt_mechanism_input.rows().data();
        if (dataMechanism.length < 1) {
            return Swal.fire({
                title: 'Form Mechanism',
                icon: "warning",
                text: 'Please fill in a mechanism',
                showConfirmButton: true,
                confirmButtonText: 'Confirm',
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }

        dt_mechanism.clear().draw();
        dt_sku_selected.clear().draw();

        let skuList = [];
        for (let i=0; i<dataMechanism.length; i++) {
            skuList.push({
                skuId: dataMechanism[i]['skuId'],
                skuDesc: dataMechanism[i]['skuDesc'],
            });
        }

        skuList = [...new Map(skuList.map(item => [item['skuId'], item])).values()];

        dt_sku_selected.rows.add(skuList).draw();
        dt_mechanism.rows.add(dataMechanism).draw();
        //</editor-fold>
    } else {
        //<editor-fold desc="Save Mechanism Input Free">
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
        let mechanismValid = true;
        for (let i=0; i<arrManualMechanismInput.length; i++) {
            let elMechanism = $(`#manual_mechanism_${arrManualMechanismInput[i]}`);
            if (elMechanism.val() === "") {
                mechanismValid = false;
            }
        }
        if (!mechanismValid) {
            return Swal.fire({
                title: 'Form Mechanism',
                icon: "warning",
                text: 'Please fill in a mechanism',
                showConfirmButton: true,
                confirmButtonText: 'Confirm',
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }

        dt_mechanism.clear().draw();
        let elBaseline = $('#baseline');
        let elUplift = $('#uplift');
        let elTotalSales = $('#totalSales');
        let elSalesContribution = $('#salesContribution');
        let elStoresCoverage = $('#storesCoverage');
        let elRedemptionRate = $('#redemptionRate');
        let elCr = $('#cr');
        let elRoi = $('#roi');

        let dataMechanismInput = [];

        for (let i=0; i<arrManualMechanismInput.length; i++) {
            let elMechanism = $(`#manual_mechanism_${arrManualMechanismInput[i]}`);
            dataMechanismInput.push({
                no: i+1,
                mechanism: elMechanism.val(),
                notes: '',
                baseline: (elBaseline.val() ? parseFloat(elBaseline.val().replace(/,/g, '')) : ''),
                uplift: (elUplift.val() ? parseFloat(elUplift.val().replace(/,/g, '')) : ''),
                totalSales: (elTotalSales.val() ? parseFloat(elTotalSales.val().replace(/,/g, '')) : ''),
                salesContribution: (elSalesContribution.val() ? parseFloat(elSalesContribution.val().replace(/,/g, '')) : ''),
                storesCoverage: (elStoresCoverage.val() ? parseFloat(elStoresCoverage.val().replace(/,/g, '')) : ''),
                redemptionRate: (elRedemptionRate.val() ? parseFloat(elRedemptionRate.val().replace(/,/g, '')) : ''),
                cr: (elCr.val() ? parseFloat(elCr.val().replace(/,/g, '')) : ''),
                roi: (elRoi.val() ? parseFloat(elRoi.val().replace(/,/g, '')) : ''),
                cost: (elCost.val() ? parseFloat(elCost.val().replace(/,/g, '')) : '')
            });
        }

        dt_mechanism.rows.add(dataMechanismInput).draw();
        //</editor-fold>
    }

    let elPrimaryForm = $('#primaryForm');
    let elMechanismForm = $('#MechanismMatrixForm');
    if (elMechanismForm.hasClass('d-none')) {
        elMechanismForm.removeClass('d-none');
        $('#btn_save').attr('disabled', true);
        elPrimaryForm.addClass('d-none');
    } else {
        elPrimaryForm.removeClass('d-none');
        $('#btn_save').attr('disabled', false);
        elMechanismForm.addClass('d-none');
    }
    $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
});

$('#btn_mechanism_cancel').on('click', function () {
    let elPrimaryForm = $('#primaryForm');
    let elMechanismForm = $('#MechanismMatrixForm');
    if (elMechanismForm.hasClass('d-none')) {
        elMechanismForm.removeClass('d-none');
        $('#btn_save').attr('disabled', true);
        elPrimaryForm.addClass('d-none');
    } else {
        elPrimaryForm.removeClass('d-none');
        $('#btn_save').attr('disabled', false);
        elMechanismForm.addClass('d-none');
    }
});

$('#btn_calculator_save').on('click', function () {
    let elSkuDesc = $('#skuDesc');
    let elMechanism = $('#mechanism');
    let elNotes = $('#notes');
    let elBaseline = $('#baseline');
    let elUplift = $('#uplift');
    let elTotalSales = $('#totalSales');
    let elSalesContribution = $('#salesContribution');
    let elStoresCoverage = $('#storesCoverage');
    let elRedemptionRate = $('#redemptionRate');
    let elCr = $('#cr');
    let elRoi = $('#roi');
    let elCost = $('#cost');

    if (elMechanism.val() === "") {
        return Swal.fire({
            title: 'Form Mechanism',
            text: "Please choose mechanism",
            icon: "warning",
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-primary"}
        });
    }

    if (elCost.val() === "0" || elCost.val() === "") {
        return Swal.fire({
            title: 'Cost cannot be zero',
            icon: "warning",
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-primary"}
        });
    }

    if (methodDetailMechanism === 'edit') {
        dt_mechanism_input.row(trIndexAutoMechanism).every(function () {
            let detail = this.data();
            detail['notes'] = elNotes.val();
            detail['baseline'] = (elBaseline.val() ? parseFloat(elBaseline.val().replace(/,/g, '')) : '');
            detail['uplift'] = (elUplift.val() ? parseFloat(elUplift.val().replace(/,/g, '')) : '');
            detail['totalSales'] = (elTotalSales.val() ? parseFloat(elTotalSales.val().replace(/,/g, '')) : '');
            detail['salesContribution'] = (elSalesContribution.val() ? parseFloat(elSalesContribution.val().replace(/,/g, '')) : '');
            detail['storesCoverage'] = (elStoresCoverage.val() ? parseFloat(elStoresCoverage.val().replace(/,/g, '')) : '');
            detail['redemptionRate'] = (elRedemptionRate.val() ? parseFloat(elRedemptionRate.val().replace(/,/g, '')) : '');
            detail['cr'] = (elCr.val() ? parseFloat(elCr.val().replace(/,/g, '')) : '');
            detail['roi'] = (elRoi.val() ? parseFloat(elRoi.val().replace(/,/g, '')) : '');
            detail['cost'] = (elCost.val() ? parseFloat(elCost.val().replace(/,/g, '')) : '');

            this.invalidate();
        });
        dt_mechanism_input.draw();
    } else {
        let dataExistingMechanismInput = dt_mechanism_input.rows().data();
        let no = dataExistingMechanismInput.length + 1;
        let dataMechanismInput = [{
            no: no,
            skuId: skuIdAutoMechanism,
            skuDesc: elSkuDesc.val(),
            mechanism: elMechanism.val(),
            notes: elNotes.val(),
            baseline: (elBaseline.val() ? parseFloat(elBaseline.val().replace(/,/g, '')) : ''),
            uplift: (elUplift.val() ? parseFloat(elUplift.val().replace(/,/g, '')) : ''),
            totalSales: (elTotalSales.val() ? parseFloat(elTotalSales.val().replace(/,/g, '')) : ''),
            salesContribution: (elSalesContribution.val() ? parseFloat(elSalesContribution.val().replace(/,/g, '')) : ''),
            storesCoverage: (elStoresCoverage.val() ? parseFloat(elStoresCoverage.val().replace(/,/g, '')) : ''),
            redemptionRate: (elRedemptionRate.val() ? parseFloat(elRedemptionRate.val().replace(/,/g, '')) : ''),
            cr: (elCr.val() ? parseFloat(elCr.val().replace(/,/g, '')) : ''),
            roi: (elRoi.val() ? parseFloat(elRoi.val().replace(/,/g, '')) : ''),
            cost: (elCost.val() ? parseFloat(elCost.val().replace(/,/g, '')) : ''),
        }];
        dt_mechanism_input.rows.add(dataMechanismInput).draw();
    }

    skuIdAutoMechanism = null;
    elSkuDesc.val('');
    elMechanism.val('');
    elNotes.val('');
    elBaseline.val('');
    elUplift.val('');
    elTotalSales.val('');
    elSalesContribution.val('');
    elStoresCoverage.val('');
    elRedemptionRate.val('');
    elCr.val('');
    elCost.val('');
    elRoi.val('');
    elRoi.val('');

    $('#btn_mechanism_save').attr('disabled', false);
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

//<editor-fold desc="Add/Delete Manual Mechanism Event Init">
$('#btn_manual_mechanism_add_1').on('click', async function () {
    addManualMechanism($(this));
    $('#btn_manual_mechanism_delete_1').removeClass('invisible');
});

$(`#btn_manual_mechanism_delete_1`).on('click', async function () {
    deleteManualMechanism($(this).val());
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
        let id = (promoId ?? uuid);
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

$('#btn_save').on('click', async function () {
    fvPromo.validate().then(async function (status) {
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
                });
            }

            let elRegion = $('#regionId');
            let regionVal = elRegion.val();
            if (regionVal.length < 1) {
                return $('#regionInvalid').removeClass('d-none');
            } else {
                $('#regionInvalid').addClass('d-none');
            }

            let selectedSKU = dt_sku_selected.rows().count();
            if (selectedSKU.length === 0) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please enter sku",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
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
        }
    });
});

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
                    subCategoryList = data['subCategory'];
                    activityList = data['activity'];
                    subActivityList = data['subActivity'];
                    distributorList = data['distibutor'];
                    channelList = data['channel'];
                    subChannelList = data['subChannel'];
                    accountList = data['account'];
                    subAccountList = data['subAccount'];
                    regionList = data['region'];
                    skuList = data['sku'];
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

const ActivityDescFormula = () => {
    let strMonth = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Des'];

    let elStartPromo = $('#startPromo');

    let startPromoYear = new Date(elStartPromo.val()).getFullYear();
    let startPromoMonth = new Date(elStartPromo.val()).getMonth();

    let strStartPromoMonth = strMonth[startPromoMonth];

    let periode_desc = [strStartPromoMonth, startPromoYear.toString()].join(' ');
    let subActivityDesc = '';

    if (elSubActivity.val()) {
        subActivityDesc = elSubActivity.select2('data')[0].text;

        let strActivityName = subActivityDesc + ' ' + periode_desc;
        $('#activityDesc').val(strActivityName)
        document.getElementById('activityDesc').focus();
        document.getElementById('activityDesc').select();
    }
}

const getMechanism = (changedActivityPeriod = false) => {
    return new Promise((resolve, reject) => {
        let pEntity = $('#entityId').val();
        let pActivity = $('#activityId').val();
        let pSubActivity = elSubActivity.val();
        let pChannel = elChannel.val();
        let pStart = $('#startPromo').val();
        let pEnd = $('#endPromo').val();
        let pGroupBrandId = $('#groupBrandId').val();
        if (pEntity && pActivity && pSubActivity && pChannel && pStart && pEnd && pGroupBrandId) {
            $.ajax({
                url         : "/promo/creation/mechanism",
                type        : "GET",
                dataType    : 'json',
                data        : {
                    'entityId' : pEntity,
                    'activityId' : pActivity,
                    'subActivityId' : pSubActivity,
                    'channelId' : pChannel,
                    'startDate' : pStart,
                    'endDate' : pEnd,
                    'brandId' : pGroupBrandId
                },
                async       : true,
                success: function(result) {
                    if (!result['error']) {
                        let data = result['data'];
                        autoMechanism = data['mechanismeAvailable'];
                        if (!autoMechanism) {
                            dt_mechanism.column(3).visible(false);
                        } else {
                            dt_mechanism.column(3).visible(true);
                        }
                        arrSourceMechanism = data['mechanism'];

                        if (method === "update" || method === "duplicate") {
                            let subActivitySelected = elSubActivity.select2('data');
                            let mainActivityId = subActivitySelected[0]['mainActivityId'];
                            if (mainActivityBefore !== mainActivityId || subActivitySelected[0]['mainActivityDesc'] === 'Running Rate') {
                                dt_sku_selected.clear().draw();
                                dt_mechanism.clear().draw();
                            }

                            if (subActivitySelected[0]['mainActivityDesc'] === 'Non Running Rate - Cosmo') {
                                if (!changedActivityPeriod) {
                                    dt_sku_selected.clear().draw();
                                    dt_mechanism.clear().draw();
                                }
                            }
                        } else {
                            dt_sku_selected.clear().draw();
                            dt_mechanism.clear().draw();
                        }
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
        }
    }).catch((e) => {
        console.log(e);
    });
}

const getBaseline = (pArrSku) => {
    return new Promise((resolve, reject) => {
        let pPromoId = promoId;
        let pPeriod = $('#period').val();
        let pGroupBrandId = $('#groupBrandId').val();
        let pSubCategoryId = $('#subCategoryId').val();
        let pSubActivityId = elSubActivity.val();
        let pDistributorId = $('#distributorId').val();
        let pChannelId = elChannel.val();
        let pSubChannelId = $('#subChannelId').val();
        let pAccountId = $('#accountId').val();
        let pSubAccountId = $('#subAccountId').val();
        let pStart = $('#startPromo').val();
        let pEnd = $('#endPromo').val();
        let arrRegion = $('#regionId').val();
        let pRegion = JSON.stringify(arrRegion);
        let pSku = JSON.stringify(pArrSku);
        $.ajax({
            url         : "/promo/creation/baseline",
            type        : "GET",
            dataType    : 'json',
            data        : {
                'promoId' : pPromoId,
                'period' : pPeriod,
                'groupBrandId' : pGroupBrandId,
                'subCategoryId' : pSubCategoryId,
                'subActivityId' : pSubActivityId,
                'startDate' : pStart,
                'endDate' : pEnd,
                'distributorId' : pDistributorId,
                'channelId' : pChannelId,
                'subChannelId' : pSubChannelId,
                'accountId' : pAccountId,
                'subAccountId' : pSubAccountId,
                'regions' : pRegion,
                'skus' : pSku
            },
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    let data = result['data'][0];
                    baseline = data['baseline_sales'];
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

const getTotalSales = () => {
    return new Promise((resolve, reject) => {
        let pPeriod = $('#period').val();
        let pGroupBrandId = $('#groupBrandId').val();
        let pChannelId = elChannel.val();
        let pSubChannelId = $('#subChannelId').val();
        let pAccountId = $('#accountId').val();
        let pSubAccountId = $('#subAccountId').val();
        let pStart = $('#startPromo').val();
        let pEnd = $('#endPromo').val();
        $.ajax({
            url         : "/promo/creation/total-sales",
            type        : "GET",
            dataType    : 'json',
            data        : {
                'period' : pPeriod,
                'groupBrandId' : pGroupBrandId,
                'startDate' : pStart,
                'endDate' : pEnd,
                'channelId' : pChannelId,
                'subChannelId' : pSubChannelId,
                'accountId' : pAccountId,
                'subAccountId' : pSubAccountId,
            },
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    if (result['data'].length > 0) {
                        let data = result['data'][0];
                        totalSales = data['ssvalue'];
                    }
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
        let pSubActivityId = elSubActivity.val();
        let pDistributor = $('#distributorId').val();
        let pSubAccountId = $('#subAccountId').val();
        $.ajax({
            url         : "/promo/creation/cr",
            type        : "GET",
            dataType    : 'json',
            data        : {
                'period' : pPeriod,
                'groupBrandId' : pGroupBrandId,
                'subActivityId' : pSubActivityId,
                'distributorId' : pDistributor,
                'subAccountId' : pSubAccountId
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
        let pPeriod = $('#period').val();
        let pGroupBrandId = $('#groupBrandId').val();
        let pCategoryId = categoryId;
        let pSubCategoryId = $('#subCategoryId').val();
        let pActivityId = $('#activityId').val();
        let subActivitySelected = elSubActivity.select2('data');
        let pDistributorId = $('#distributorId').val();
        let pChannelId = elChannel.val();
        let pSubChannelId = $('#subChannelId').val();
        let pAccountId = $('#accountId').val();
        let pSubAccountId = $('#subAccountId').val();
        if (pPeriod && pGroupBrandId && pCategoryId && pSubCategoryId && pActivityId && subActivitySelected.length > 0 &&
            pDistributorId && pChannelId && pSubChannelId && pAccountId && pSubAccountId) {

            let pSubActivityId = subActivitySelected[0]['id'];
            let pSubActivityTypeId = subActivitySelected[0]['subActivityTypeId'];
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

const totalSalesFormula = async () => {
    let elBaseline = $('#baseline');
    let elUplift = $('#uplift');
    let elTotalSales = $('#totalSales');

    let baseline = parseFloat((elBaseline.val() === '' ? '0' : elBaseline.val()).replace(/,/g, ''));
    let uplift = parseFloat((elUplift.val() === '' ? '0' : elUplift.val()).replace(/,/g, ''));

    let totalSales = baseline * ((uplift+100) / 100);
    elTotalSales.val(Math.round(totalSales));
}

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
    if (dataCalculator['cost'] === 2) {
        let cost = await calculatorFormula(keyInput, skuId);
        elCost.val(cost);
    }
}

const roiFormula = () => {
    let elBaseline = $('#baseline');
    let elUplift = $('#uplift');
    let elRoi = $('#roi');
    let elCost = $('#cost');

    let uplift = parseFloat((elUplift.val() === '' ? '0' : elUplift.val()).replace(/,/g, ''));
    let baseline = parseFloat((elBaseline.val() === '' ? '0' : elBaseline.val()).replace(/,/g, ''));
    let cost = parseFloat((elCost.val() === '' ? '0' : elCost.val()).replace(/,/g, ''));
    if (baseline === 0) baseline = 1;

    let incrSales = (baseline * (uplift / 100));
    let roi = Math.round(((incrSales - cost) / cost) * 100);
    if (!isFinite(roi)) {
        elRoi.val(0);
    } else {
        elRoi.val(roi);
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
            if (mainActivityDesc === "Non Running Rate - Cosmo") {
                await totalSalesFormula();
            } else {
                await getTotalSales().then(function () {
                    elTotalSales.val(totalSales);
                    totalSalesValue = totalSales;
                });
            }
        } else {
            if (mainActivityDesc !== "Non Running Rate - Cosmo") {
                totalSalesValue = (parseFloat(elTotalSales.val().replace(/,/g, '')));
            }
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
            if (mainActivityDesc === "Non Running Rate - Cosmo") {
                crValue = parseFloat(elCr.val().replace(/,/g, ''));
            } else {
                crValue = cr;
            }
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

const getPromoExist = () => {
    return new Promise((resolve, reject) => {
        let exist = false;
        if (method === 'update') {
            resolve(exist);
        } else {
            let data = {
                period: $('#period').val(),
                activityDesc: $('#activityDesc').val(),
                channelId: elChannel.val(),
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

const loadDropdownGroupBrand = async (entityIdSelected, groupBrandIdSelected) => {
    let elBrand = $('#groupBrandId');
    let entityId = parseInt(entityIdSelected);
    elBrand.empty();
    dt_sku.clear().draw();
    dt_sku_selected.clear().draw();
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

const loadListSKU = async (groupBrandIdSelected) => {
    let groupBrandId = parseInt(groupBrandIdSelected);
    let skuRows = [];
    dt_sku.clear().draw();
    dt_sku_selected.clear().draw();
    for (let i = 0; i < skuList.length; i++) {
        if (skuList[i]['groupBrandId'] === groupBrandId) {
            skuRows.push(skuList[i]);
        }
    }
    dt_sku.rows.add(skuRows).draw();
}

const loadDropdownActivity = async (subCategoryIdSelected, activityIdSelected) => {
    let elActivity = $('#activityId');
    let subCategoryId = parseInt(subCategoryIdSelected);
    elActivity.empty();
    elSubActivity.empty();
    let activityDropdown = [{id:'', text:''}];
    for (let i = 0; i < activityList.length; i++) {
        if (activityList[i]['SubCategoryId'] === subCategoryId) {
            activityDropdown.push({
                id: activityList[i]['ActivityId'],
                text: activityList[i]['ActivityDesc']
            });
        }
    }
    elActivity.select2({
        placeholder: "Select an Activity",
        width: '100%',
        data: activityDropdown
    }).on('change', function () {
        // Revalidate the color field when an option is chosen
        fvPromo.revalidateField('activityId');
    });
    elActivity.val(activityIdSelected).trigger('change.select2');
}

const loadDropdownSubActivity = async (activityIdSelected, subActivityIdSelected) => {
    let activityId = parseInt(activityIdSelected);
    elSubActivity.empty();
    let subActivityDropdown = [{id:'', text:''}];
    for (let i = 0; i < subActivityList.length; i++) {
        if (subActivityList[i]['ActivityId'] === activityId) {
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

const loadDropdownSubChannel = async (channelIdSelected, subChannelIdSelected) => {
    let elSubChannel = $('#subChannelId');
    let channelId = parseInt(channelIdSelected);
    elSubChannel.empty();
    $('#accountId').empty();
    $('#subAccountId').empty();
    let subActivityDropdown = [{id:'', text:''}];
    for (let i = 0; i < subChannelList.length; i++) {
        if (subChannelList[i]['ChannelId'] === channelId) {
            subActivityDropdown.push({
                id: subChannelList[i]['SubChannelId'],
                text: subChannelList[i]['SubChannelDesc']
            });
        }
    }
    elSubChannel.select2({
        placeholder: "Select a Sub Channel",
        width: '100%',
        data: subActivityDropdown
    }).on('change', function () {
        // Revalidate the color field when an option is chosen
        fvPromo.revalidateField('subChannelId');
    });
    elSubChannel.val(subChannelIdSelected).trigger('change.select2');
}

const loadDropdownAccount = async (subChannelIdSelected, accountIdSelected) => {
    let elAccount = $('#accountId');
    let subChannelId = parseInt(subChannelIdSelected);
    elAccount.empty();
    $('#subAccountId').empty();
    let accountDropdown = [{id:'', text:''}];
    for (let i = 0; i < accountList.length; i++) {
        if (accountList[i]['SubChannelId'] === subChannelId) {
            accountDropdown.push({
                id: accountList[i]['AccountId'],
                text: accountList[i]['AccountDesc']
            });
        }
    }
    elAccount.select2({
        placeholder: "Select an Account",
        width: '100%',
        data: accountDropdown
    }).on('change', function () {
        // Revalidate the color field when an option is chosen
        fvPromo.revalidateField('accountId');
    });
    elAccount.val(accountIdSelected).trigger('change.select2');
}

const loadDropdownSubAccount = async (accountIdSelected, subAccountIdSelected) => {
    let elSubAccount = $('#subAccountId');
    let accountId = parseInt(accountIdSelected);
    elSubAccount.empty();
    let subAccountDropdown = [{id:'', text:''}];
    for (let i = 0; i < subAccountList.length; i++) {
        if (subAccountList[i]['AccountId'] === accountId) {
            subAccountDropdown.push({
                id: subAccountList[i]['SubAccountId'],
                text: subAccountList[i]['SubAccountDesc']
            });
        }
    }
    elSubAccount.select2({
        placeholder: "Select a Sub Account",
        width: '100%',
        data: subAccountDropdown
    }).on('change', function () {
        // Revalidate the color field when an option is chosen
        fvPromo.revalidateField('subAccountId');
    });
    elSubAccount.val(subAccountIdSelected).trigger('change.select2');
}

const getData = (pId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/promo/creation/id",
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
                    $('#entityId').val(promo['entityId'] ?? '').trigger('change.select2');
                    $('#groupBrandId').val(promo['groupBrandId'] ?? '').trigger('change.select2');
                    await loadDropdownGroupBrand(promo['entityId'].toString(), promo['groupBrandId'].toString());
                    await loadListSKU(promo['groupBrandId'].toString());

                    categoryId = promo['categoryId'];
                    $('#subCategoryId').val(promo['subCategoryId'] ?? '').trigger('change.select2');
                    await loadDropdownActivity(promo['subCategoryId'].toString(), promo['activityId'].toString());
                    await loadDropdownSubActivity(promo['activityId'].toString(), promo['subActivityId'].toString());

                    for (let i=0; i<subActivityList.length; i++) {
                        if (promo['subActivityId'] === subActivityList[i]['SubActivityId']) {
                            mainActivityBefore = subActivityList[i]['mainActivityId'];
                        }
                    }

                    $('#activityDesc').val(promo['activityDesc'] ?? '');
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

                    $('#distributorId').val(promo['distributorId'] ?? '').trigger('change.select2');

                    initiatorNotes = promo['initiator_notes'];

                    elChannel.val(promo['channelId'] ?? '').trigger('change.select2');
                    await loadDropdownSubChannel(promo['channelId'].toString(), promo['subChannelId'].toString());
                    await loadDropdownAccount(promo['subChannelId'].toString(), promo['accountId'].toString());
                    await loadDropdownSubAccount(promo['accountId'].toString(), promo['subAccountId'].toString());

                    // set data region
                    let elRegion = $('#regionId');
                    let arrSetRegion = [];
                    let exist=true;
                    let regionDropdown = [];
                    for (let i = 0; i < regionList.length; i++) {
                        regionDropdown.push({
                            id: regionList[i]['regionId'],
                            text: regionList[i]['regionDesc']
                        });
                    }
                    for (let i=0; i<data['region'].length; i++) {
                        for (let j=0; j<regionList.length; j++) {
                            if (regionList[j]['regionId'] !== data['region'][i]['regionId']) {
                                exist = false;
                            }
                        }
                        if (!exist) {
                            regionDropdown.push({id: data['region'][i]['regionId'], text: data['region'][i]['regionDesc']});
                        }

                        arrSetRegion.push(data['region'][i]['regionId']);
                    }
                    elRegion.select2({
                        placeholder: "Select Region",
                        width: '100%',
                        data: regionDropdown
                    });
                    elRegion.val(arrSetRegion ).trigger('change');

                    await getMechanism();

                    dt_sku_selected.rows.add(data['sku']).draw();

                    if (data['mechanism'].length > 0) {
                        for (let i=0; i<data['mechanism'].length; i++) {
                            data['mechanism'][i]['no'] = i+1;
                        }
                    }
                    dt_mechanism.rows.add(data['mechanism']).draw();

                    if (method !== "duplicate") {
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
                    }

                    if (method === "update") statusApprovalCode = promo['statusApprovalCode'];
                    await getBudget();
                    cost = promo['cost'];
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
    blockUIMechanism.block();
    blockUIBudget.block();
    blockUIRegion.block();
    blockUISKU.block();
    blockUIAttachment.block();
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    //cek Promo Exist
    let isPromoExist = await getPromoExist();
    if (!isPromoExist) {
        if (method !== "update") {
            let latePromoDays = await getLatePromoDays();

            let elStartPromo = $('#startPromo');
            let dtStart = new Date(elStartPromo.val()).getTime();
            let dtNow = new Date().getTime();
            let diffDays = Math.ceil((dtStart - dtNow) / (1000 * 60 * 60 * 24));

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
                        $('#startPromo').addClass('form-control-solid-bg')
                        $('#endPromo').addClass('form-control-solid-bg')
                        $('#startPromo, #endPromo').flatpickr({
                            altFormat: "d-m-Y",
                            altInput: true,
                            allowInput: true,
                            dateFormat: "Y-m-d",
                            disableMobile: "true",
                            minDate: "2025-01"
                        });

                        submitSave(modifyReason, result.value);
                    } else if (result.dismiss === 'cancel') {

                        $('#startPromo').addClass('form-control-solid-bg')
                        $('#endPromo').addClass('form-control-solid-bg')
                        $('#startPromo, #endPromo').flatpickr({
                            altFormat: "d-m-Y",
                            altInput: true,
                            allowInput: true,
                            dateFormat: "Y-m-d",
                            disableMobile: "true",
                            minDate: "2025-01"
                        });

                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                        blockUIHeader.release();
                        blockUIMechanism.release();
                        blockUIBudget.release();
                        blockUIRegion.release();
                        blockUISKU.release();
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
                    minDate: "2025-01"
                });

                await submitSave(modifyReason, "");
            }
        } else {
            await submitSave(modifyReason, initiatorNotes);
        }
    } else {
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
        blockUIHeader.release();
        blockUIMechanism.release();
        blockUIBudget.release();
        blockUIRegion.release();
        blockUISKU.release();
        blockUIAttachment.release();
    }
}

const submitSave = async function (modifyReason, initiatorNotes) {
    let e = document.querySelector("#btn_save");

    let subActivitySelected = elSubActivity.select2('data');
    let subActivityTypeId = subActivitySelected[0]['subActivityTypeId'];
    let mainActivityDesc = subActivitySelected[0]['mainActivityDesc'];

    let elRegion = $('#regionId');
    let regionVal = elRegion.val();
    let regionData = [];
    for (let i=0; i<regionVal.length; i++) {
        regionData.push({
            id: regionVal[i]
        });
    }

    let selectedSKU = dt_sku_selected.rows().data();
    let skuData = [];
    for (let i=0; i<selectedSKU.length; i++) {
        skuData.push({
            id: selectedSKU[i]['skuId']
        });
    }

    let baseline = 0;
    let upLift = 0;
    let totalSalesSave = 0;
    let salesContribution = 0;
    let storesCoverage = 0;
    let redemptionRate = 0;
    let crSave = 0;
    let costSave = 0;
    let mechanismList = dt_mechanism.rows().data();

    let mechanismData = [];
    if (autoMechanism) {
        for(let i=0; i<mechanismList.length; i++) {
            baseline = baseline + (mechanismList[i]['baseline'] === "" ? 0 : mechanismList[i]['baseline']);
            upLift = upLift + (mechanismList[i]['uplift'] === "" ? 0 : mechanismList[i]['uplift']);
            totalSalesSave = totalSalesSave + (mechanismList[i]['totalSales'] === "" ? 0 : mechanismList[i]['totalSales']);
            salesContribution = salesContribution + (mechanismList[i]['salesContribution'] === "" ? 0 : mechanismList[i]['salesContribution']);
            storesCoverage = storesCoverage + (mechanismList[i]['storesCoverage'] === "" ? 0 : mechanismList[i]['storesCoverage']);
            redemptionRate = redemptionRate + (mechanismList[i]['redemptionRate'] === "" ? 0 : mechanismList[i]['redemptionRate']);
            crSave = crSave + (mechanismList[i]['cr'] === "" ? 0 : mechanismList[i]['cr']);
            costSave = costSave + (mechanismList[i]['cost'] === "" ? 0 : mechanismList[i]['cost']);
        }
    } else {
        if (mainActivityDesc === "Running Rate") {
            await getTotalSales();
            await getCR();
            totalSalesSave = totalSales;
            crSave = cr;
            salesContribution = (mechanismList[0]['salesContribution'] === "" ? 0 : mechanismList[0]['salesContribution']);
            costSave = await recalculateCostRunningRate(mechanismList[0]);
        } else {
            baseline = (mechanismList[0]['baseline'] === "" ? 0 : mechanismList[0]['baseline']);
            upLift = (mechanismList[0]['uplift'] === "" ? 0 : mechanismList[0]['uplift']);
            totalSalesSave = (mechanismList[0]['totalSales'] === "" ? 0 : mechanismList[0]['totalSales']);
            salesContribution = (mechanismList[0]['salesContribution'] === "" ? 0 : mechanismList[0]['salesContribution']);
            storesCoverage = (mechanismList[0]['storesCoverage'] === "" ? 0 : mechanismList[0]['storesCoverage']);
            redemptionRate = (mechanismList[0]['redemptionRate'] === "" ? 0 : mechanismList[0]['redemptionRate']);
            crSave = (mechanismList[0]['cr'] === "" ? 0 : mechanismList[0]['cr']);
            costSave = (mechanismList[0]['cost'] === "" ? 0 : mechanismList[0]['cost']);
        }
    }

    for(let i=0; i<mechanismList.length; i++) {
        mechanismData.push({
            id: (mechanismList[i]['id'] ?? 0),
            mechanism: mechanismList[i]['mechanism'],
            notes: mechanismList[i]['notes'],
            productId: (mechanismList[i]['skuId'] ?? 0),
            baseline: (mechanismList[i]['baseline'] === "" ? 0 : mechanismList[i]['baseline']),
            uplift: (mechanismList[i]['uplift'] === "" ? 0 : mechanismList[i]['uplift']),
            totalSales: (mainActivityDesc === "Running Rate" ? totalSales : (mechanismList[i]['totalSales'] === "" ? 0 : mechanismList[i]['totalSales'])),
            salesContribution: (mechanismList[i]['salesContribution'] === "" ? 0 : mechanismList[i]['salesContribution']),
            storesCoverage: (mechanismList[i]['storesCoverage'] === "" ? 0 : mechanismList[i]['storesCoverage']),
            redemptionRate: (mechanismList[i]['redemptionRate'] === "" ? 0 : mechanismList[i]['redemptionRate']),
            cr: (mainActivityDesc === "Running Rate" ? cr : (mechanismList[i]['cr'] === "" ? 0 : mechanismList[i]['cr'])),
            cost: (mainActivityDesc === "Running Rate" ? costSave : (mechanismList[i]['cost'] === "" ? 0 : mechanismList[i]['cost'])),
        });
    }

    let formData = new FormData($('#form_promo')[0]);
    formData.append('uuid', uuid);
    formData.append('promoId', promoId);
    formData.append('categoryId', categoryId);
    formData.append('subActivityTypeId', subActivityTypeId);
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
    formData.append('fileName1', $('#review_file_label_1').text());
    formData.append('fileName2', $('#review_file_label_2').text());
    formData.append('fileName3', $('#review_file_label_3').text());
    formData.append('fileName4', $('#review_file_label_4').text());
    formData.append('fileName5', $('#review_file_label_5').text());
    formData.append('fileName6', $('#review_file_label_6').text());
    formData.append('fileName7', $('#review_file_label_7').text());
    formData.append('initiatorNotes', initiatorNotes);

    let url = "/promo/creation/revamp/save";
    if (method === "update") {
        url = "/promo/creation/revamp/update";
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
                    if (result['attachment5'] !== 'No File Upload attachment5') {
                        let elInfo5 = $('#info5');
                        if (result['attachment5'] === 'Upload Successfully') {
                            elInfo5.removeClass('invisible').addClass('visible');
                            elInfo5.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo5.removeClass('invisible').addClass('visible');
                            elInfo5.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                    if (result['attachment6'] !== 'No File Upload attachment6') {
                        let elInfo6 = $('#info6');
                        if (result['attachment6'] === 'Upload Successfully') {
                            elInfo6.removeClass('invisible').addClass('visible');
                            elInfo6.children().removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
                        } else {
                            elInfo6.removeClass('invisible').addClass('visible');
                            elInfo6.children().removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
                        }
                    }
                    if (result['attachment7'] !== 'No File Upload attachment7') {
                        let elInfo7 = $('#info7');
                        if (result['attachment7'] === 'Upload Successfully') {
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
            blockUIHeader.release();
            blockUIMechanism.release();
            blockUIBudget.release();
            blockUIRegion.release();
            blockUISKU.release();
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

//<editor-fold desc="Function Add/Delete Manual Mechanism">
const addManualMechanism = (elementDeleteBefore) => {
    indexManualMechanism++;
    arrManualMechanismInput.push(indexManualMechanism);
    elementDeleteBefore.addClass('invisible');
    let elFirstManualMechanism =  $(`#btn_manual_mechanism_delete_${arrManualMechanismInput[0]}`);
    if (elFirstManualMechanism.hasClass('invisible')) elFirstManualMechanism.removeClass('invisible');
    $('#manualMechanismList').append(`
        <div class="row mb-3" id="mechanismRow${indexManualMechanism}">
            <div class="col-12">
                <div class="d-flex justify-content-between">
                    <label class="text-nowrap my-auto me-10" for="manual_mechanism_${indexManualMechanism}" id="lbl_manual_mechanism_${indexManualMechanism}">Mechanism ${arrManualMechanismInput.length}</label>
                    <input type="text" class="form-control form-control-sm" name="manual_mechanism_${indexManualMechanism}" id="manual_mechanism_${indexManualMechanism}" autocomplete="off"/>
                    <button class="btn btn-sm btn-outline-optima ms-2" id="btn_manual_mechanism_delete_${indexManualMechanism}" title="Delete" value="${indexManualMechanism}">
                        <span class="fa fa-trash-alt"> </span>
                    </button>
                    <button class="btn btn-sm btn-outline-optima ms-2" id="btn_manual_mechanism_add_${indexManualMechanism}" title="Add Row">
                        <span class="fa fa-plus"></span>
                    </button>
                </div>
            </div>
        </div>
    `);

    $(`#btn_manual_mechanism_add_${indexManualMechanism}`).on('click', async function () {
        addManualMechanism($(this));
    });

    $(`#btn_manual_mechanism_delete_${indexManualMechanism}`).on('click', async function () {
        deleteManualMechanism($(this).val());
    });
}

const deleteManualMechanism = (index) => {
    $(`#mechanismRow${index}`).remove();
    const indexArrManualMechanismInput = arrManualMechanismInput.indexOf(parseInt(index));

    if (indexArrManualMechanismInput > -1) {
        arrManualMechanismInput.splice(indexArrManualMechanismInput, 1);
    }

    for (let i=0; i<arrManualMechanismInput.length; i++) {
        $(`#lbl_manual_mechanism_${arrManualMechanismInput[i]}`).text(`Mechanism ${i+1}`);
        if (arrManualMechanismInput.length === 1) {
            let elLastAddManualMechanism = $(`#btn_manual_mechanism_add_${arrManualMechanismInput[i]}`);
            if (elLastAddManualMechanism.hasClass('invisible')) {
                elLastAddManualMechanism.removeClass('invisible');
            }
            let elLastDeleteManualMechanism = $(`#btn_manual_mechanism_delete_${arrManualMechanismInput[i]}`);
            if (!elLastDeleteManualMechanism.hasClass('invisible')) {
                elLastDeleteManualMechanism.addClass('invisible');
            }
        } else {
            if (arrManualMechanismInput.length === i+1) {
                let elLastAddManualMechanism = $(`#btn_manual_mechanism_add_${arrManualMechanismInput[i]}`);
                if (elLastAddManualMechanism.hasClass('invisible')) {
                    elLastAddManualMechanism.removeClass('invisible');
                }
            }
        }
    }
}
//</editor-fold>

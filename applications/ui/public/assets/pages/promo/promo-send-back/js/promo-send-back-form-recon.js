'use strict';

let swalTitle = "Promo Send Back Reconciliation";
let url_str = new URL(window.location.href);
let method = url_str.searchParams.get("method");
let promoId = url_str.searchParams.get("id");
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
let autoMechanism = false, arrSourceMechanism = [], dt_mechanism_source, dt_mechanism_input, dt_mechanism_input_before;
let skuIdAutoMechanism = null, methodDetailMechanism = 'add', trIndexAutoMechanism;
let indexManualMechanism = 1, arrManualMechanismInput = [1];
let promoReconConfigItem, disabledSKU;
let baseline = 0, actualSales = 0, totalSales = 0, cr = 0, lastCR = 0, cost = 0, remainingBudget = 0, dataMatrixCalculator, mechanismCurrent, mechanismBefore;
let isCreation = 0;
let statusApprovalCode = 'TP2';
let mainActivityBefore;
let initiatorNotes = "";

let fvPromo;
const formPromo = document.getElementById('form_promo');

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
                // check cross Year
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
    }).mask("#remainingBudget, #totalCost, #baseline, #baselineBefore, #totalSales, #totalSalesBefore, #cost, #costBefore");

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
        jitMasking: true,
        suffix: ' %'
    }).mask("#uplift, #upliftBefore, #salesContribution, #salesContributionBefore, #storesCoverage, #storesCoverageBefore, #redemptionRate, #redemptionRateBefore, #roi, #roiBefore, #cr, #crBefore");

    $('#startPromo, #endPromo').flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
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
            await setConfigCalculatorCreation(dataMatrixCalculator);

            skuIdAutoMechanism = dataMechanism['productId'];
            $('#skuDesc').val(dataMechanism['product']);
            $('#mechanism').val(dataMechanism['mechanism']);
            $('#notes').val('');
        } else {
            await setConfigCalculatorCreation(dataMatrixCalculator);
            skuIdAutoMechanism = dataMechanism['productId'];
            $('#skuDesc').val(dataMechanism['product']);
            $('#mechanism').val(dataMechanism['mechanism']);
            $('#notes').val('');
        }
        isCreation = 1;
        blockUIPromoCalculator.block();
        await costFormulaCreation(false, dataMechanism['productId']);
        roiFormula();
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

        if (trData['statusRecon']) {
            await setConfigCalculator(dataMatrixCalculator);

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
            await loadDefaultCalculator(trData['skuId']);

            isCreation = 0;
            blockUIPromoCalculator.block();
            await costFormula(false, trData['skuId']);
            roiFormula();
            blockUIPromoCalculator.release();
        } else {
            await setConfigCalculatorCreation(dataMatrixCalculator);

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

            isCreation = 1;
            blockUIPromoCalculator.block();
            await costFormulaCreation(false, trData['skuId']);
            roiFormula();
            blockUIPromoCalculator.release();
        }

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

    dt_mechanism_input_before = $('#dt_mechanism_input_before').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "34vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: '',
                targets: 0,
                data: 'no',
                width: 10,
                orderable: false,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                width: 150,
                data: 'mechanism',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                width: 120,
                data: 'notes',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 4,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 5,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 6,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 7,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 8,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 9,
                width: 150,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
        createdRow: function (row, data, dataIndex) {
            $('td:eq(1)', row).attr('colspan', '5');
            $('td:eq(2)', row).attr('colspan', '4');
            $('.deleted_cell', row).remove();
            dt_mechanism_input_before.rows().every(function (rowIdx, tableLoop, rowLoop) {
                if (dataIndex === rowIdx) {
                    let incrSales = (Math.round(data['baseline']) * (data['uplift'] / 100));
                    let roi = Math.round(((incrSales - data['cost']) / data['cost']) * 100);
                    if (!isFinite(roi)) {
                        roi= 0;
                    }

                    this.child(
                        $(`
                        <tr>
                            <td style="width: 0"></td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['baseline'] ?? 0), 0)}</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['uplift'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['totalSales'] ?? 0), 0)}</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['salesContribution'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['storesCoverage'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['redemptionRate'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['cr'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((roi ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['cost'] ?? 0), 0)}</td>
                        </tr>
                    `)
                    ).show();
                }
            });
        }
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

        await getData(promoId);
        await getBudget();

        editableConfig(promoReconConfigItem);

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
            defaultDate: new Date(endDate),
            onClose: function() {
                elStart.trigger('change');
            }
        });
    }
    elPeriod.val(new Date(elStart.val()).getFullYear()).on('change');
    ActivityDescFormula();

    if ($(this).val()) await getMechanism(true);
    mechanismCurrent = [];
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
            defaultDate: new Date(startDate),
            onClose: function() {
                elStart.trigger('change');
            }
        });
    }
    ActivityDescFormula();

    elPeriod.val(new Date(elEnd.val()).getFullYear()).on('change');

    if ($(this).val()) await getMechanism(true);
    mechanismCurrent = [];
    fvPromo.revalidateField('startPromo');
});
//</editor-fold>

//<editor-fold desc="SKU Matrix Event">
$('#entityId').on('change', async function () {
    await loadDropdownGroupBrand($(this).val(), '');
    if ($(this).val()) await getMechanism();
    mechanismCurrent = [];
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
    mechanismCurrent = [];
    await getBudget();
});

elSubActivity.on('change', async function () {
    ActivityDescFormula();
    if ($(this).val()) await getMechanism();
    mechanismCurrent = [];
    await getBudget();
});
//</editor-fold>

//<editor-fold desc="Account Matrix Event">
elChannel.on('change', async function () {
    await loadDropdownSubChannel($(this).val(), '');
    if ($(this).val()) await getMechanism();
    mechanismCurrent = [];
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
                let elAutoMechanismBeforeList = $('#autoMechanismBeforeListSection');
                let elManualMechanism = $('#manualMechanismInputSection');
                let elPromoCalculatorBefore = $('#promoCalculatorBefore');
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

                    dt_mechanism_input_before.clear().draw();
                    dt_mechanism_input_before.rows.add(mechanismBefore).draw();

                    elAutoMechanismForm.removeClass('d-none');
                    elAutoMechanism.removeClass('d-none');
                    elAutoMechanismInput.removeClass('d-none');
                    elAutoMechanismList.removeClass('d-none');
                    elManualMechanism.addClass('d-none');
                    elAutoMechanismBeforeList.removeClass('d-none');
                    elPromoCalculatorBefore.addClass('d-none');
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
                    elAutoMechanismBeforeList.addClass('d-none');
                    elManualMechanism.removeClass('d-none');
                    elPromoCalculatorBefore.removeClass('d-none');
                    $('#btn_calculator_save').addClass('d-none');

                    let elManualMechanismList = $('#manualMechanismList');
                    elManualMechanismList.html('');

                    let dataMechanism = mechanismCurrent;
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
                        roiFormulaBefore();
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
                        roiFormulaBefore();
                        blockUIPromoCalculator.release();
                    }
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

        mechanismCurrent = dataMechanismInput;
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
            statusRecon: 0
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

    if (isCreation === 0) {
        if (dataCalculator['baselineRecon'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormula(true);
                roiFormula();
            }
        }
    } else {
        if (dataCalculator['baseline'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormulaCreation(true);
                roiFormula();
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

    if (isCreation === 0) {
        if (dataCalculator['baselineRecon'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormula(true);
            roiFormula();
        }
    } else {
        if (dataCalculator['baseline'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormulaCreation(true);
            roiFormula();
        }
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

    if (isCreation === 0) {
        if (dataCalculator['upliftRecon'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormula(true);
                roiFormula();
            }
        }
    } else {
        if (dataCalculator['uplift'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await totalSalesFormula();
                await costFormulaCreation(true);
                roiFormula();
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

    if (isCreation === 0) {
        if (dataCalculator['upliftRecon'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormula(true);
            roiFormula();
        }
    } else {
        if (dataCalculator['uplift'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await totalSalesFormula();
            await costFormulaCreation(true);
            roiFormula();
        }
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

    if (isCreation === 0) {
        if (dataCalculator['totalSalesRecon'] === 1) {
            let code = e.keyCode || e.which;
            if ($(this).val()) {
                if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                    await costFormula(true);
                    roiFormula();
                    await crFormula();
                }
            }
        }
    } else {
        if (dataCalculator['totalSales'] === 1) {
            let code = e.keyCode || e.which;
            if ($(this).val()) {
                if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                    await costFormulaCreation(true);
                    roiFormula();
                    await crFormula();
                }
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

    if (isCreation === 0) {
        if (dataCalculator['totalSalesRecon'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormula(true);
            roiFormula();
            await crFormula();
        }
    } else {
        if (dataCalculator['totalSales'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormulaCreation(true);
            roiFormula();
            await crFormula();
        }
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

    if (isCreation === 0) {
        if (dataCalculator['salesContributionRecon'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormula(true);
                roiFormula();
            }
        }
    } else {
        if (dataCalculator['salesContribution'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormulaCreation(true);
                roiFormula();
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

    if (isCreation === 0) {
        if (dataCalculator['salesContributionRecon'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormula(true);
            roiFormula();
        }
    } else {
        if (dataCalculator['salesContribution'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormulaCreation(true);
            roiFormula();
        }
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

    if (isCreation === 0) {
        if (dataCalculator['storesCoverageRecon'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormula(true);
                roiFormula();
            }
        }
    } else {
        if (dataCalculator['storesCoverage'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormulaCreation(true);
                roiFormula();
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

    if (isCreation === 0) {
        if (dataCalculator['storesCoverageRecon'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormula(true);
            roiFormula();
        }
    } else {
        if (dataCalculator['storesCoverage'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormulaCreation(true);
            roiFormula();
        }
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

    if (isCreation === 0) {
        if (dataCalculator['redemptionRateRecon'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormula(true);
                roiFormula();
            }
        }
    } else {
        if (dataCalculator['redemptionRate'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormulaCreation(true);
                roiFormula();
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

    if (isCreation === 0) {
        if (dataCalculator['redemptionRateRecon'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormula(true);
            roiFormula();
        }
    } else {
        if (dataCalculator['redemptionRate'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormulaCreation(true);
            roiFormula();
        }
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

    if (isCreation === 0) {
        if (dataCalculator['crRecon'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormula(true);
                roiFormula();
            }
        }
    } else {
        if (dataCalculator['cr'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                await costFormulaCreation(true);
                roiFormula();
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

    if (isCreation === 0) {
        if (dataCalculator['crRecon'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormula(true);
            roiFormula();
        }
    } else {
        if (dataCalculator['cr'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await costFormulaCreation(true);
            roiFormula();
        }
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

    if (isCreation === 0) {
        if (dataCalculator['costRecon'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                roiFormula();
                if (subActivitySelected[0]['mainActivityDesc'] === 'Non Running Rate - Fix Value') {
                    await crFormula();
                }
            }
        }
    } else {
        if (dataCalculator['cost'] === 1) {
            let code = e.keyCode || e.which;
            if ((code > 47 && code < 58) || code === 8 || code === 46 || code === 189 || e.ctrlKey || code === 86) {
                roiFormula();
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

    if (isCreation === 0) {
        if (dataCalculator['costRecon'] === 1) {
            if (!$(this).val()) $(this).val(0);
            roiFormula();
            if (subActivitySelected[0]['mainActivityDesc'] === 'Non Running Rate - Fix Value') {
                await crFormula();
            }
        }
    } else {
        if (dataCalculator['cost'] === 1) {
            if (!$(this).val()) $(this).val(0);
            await crFormula();
            roiFormula();
        }
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
                    url: "/promo/send-back/attachment-delete",
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

$('#sticky_notes').on('click', function () {
    let elModal = $('#modal_notes');
    if (elModal.hasClass('show')) {
        elModal.modal('hide');
    } else {
        elModal.modal('show');
    }
});

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

const getListAttribute = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/send-back/promo-attribute",
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
            url         : "/promo/send-back/category/category-desc",
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
                url         : "/promo/send-back/mechanism",
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

                        if (method === "update") {
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
            url         : "/promo/send-back/baseline",
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
                    actualSales = data['actual_sales'];
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
            url         : "/promo/send-back/total-sales",
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
            url         : "/promo/send-back/cr",
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
                url         : "/promo/send-back/budget",
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

const setConfigCalculatorCreation = (data) => {
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

const setConfigCalculator = (data) => {
    let elBaseline = $('#baseline');
    switch (data['baselineRecon']) {
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
    switch (data['upLiftRecon']) {
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
    switch (data['totalSalesRecon']) {
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
    switch (data['salesContributionRecon']) {
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
    switch (data['storesCoverageRecon']) {
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
    switch (data['redemptionRateRecon']) {
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
    switch (data['crRecon']) {
        case 0:
            if (mechanismCurrent.length === 0) {
                elCR.val(lastCR).attr('readonly', true).addClass('form-control-solid-bg');
            } else {
                elCR.val('').attr('readonly', true).addClass('form-control-solid-bg');
            }
            break;
        case 1:
            elCR.val('100').attr('readonly', false).removeClass('form-control-solid-bg').css('background-color', '#fff');
            break;
        case 2:
            elCR.val('').attr('readonly', true).css('background-color', '#8f99e7 !important');
            break;
    }

    let elCost = $('#cost');
    switch (data['costRecon']) {
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

const roiFormula = () => {
    let elBaseline = $('#baseline');
    let elUplift = $('#uplift');
    let elRoi = $('#roi');
    let elCost = $('#cost');

    let uplift = parseFloat((elUplift.val() === '' ? '0' : elUplift.val()).replace(/,/g, ''));
    let baseline = parseFloat((elBaseline.val() === '' ? '0' : elBaseline.val()).replace(/,/g, ''));
    let cost = parseFloat((elCost.val() === '' ? '0' : elCost.val()).replace(/,/g, ''));

    if (baseline === 0) baseline = 1;
    let incrSales = (Math.round(baseline) * (uplift / 100));
    let roi = Math.round(((incrSales - cost) / cost) * 100);
    if (!isFinite(roi)) {
        elRoi.val(0);
    } else {
        elRoi.val(roi);
    }
}

const roiFormulaBefore = () => {
    let elBaseline = $('#baselineBefore');
    let elUplift = $('#upliftBefore');
    let elRoi = $('#roiBefore');
    let elCost = $('#costBefore');

    let uplift = parseFloat((elUplift.val() === '' ? '0' : elUplift.val()).replace(/,/g, ''));
    let baseline = parseFloat((elBaseline.val() === '' ? '0' : elBaseline.val()).replace(/,/g, ''));
    let cost = parseFloat((elCost.val() === '' ? '0' : elCost.val()).replace(/,/g, ''));

    if (baseline === 0) baseline = 1;
    let incrSales = (Math.round(baseline) * (uplift / 100));
    let roi = Math.round(((incrSales - cost) / cost) * 100);
    if (!isFinite(roi)) {
        elRoi.val(0);
    } else {
        elRoi.val(roi);
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
    if (subActivitySelected[0]['mainActivityDesc'] === "Non Running Rate - Cosmo") {
        await calculatorFormula(keyInput, skuId);
    } else {
        if (dataCalculator['costRecon'] === 2) {
            let cost = await calculatorFormula(keyInput, skuId);
            elCost.val(cost);
        }
    }
}

const costFormulaCreation = async (keyInput = false, skuId = 0) => {
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
        let cost = await calculatorFormulaCreation(keyInput, skuId);
        elCost.val(cost);
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
    if (dataCalculator['baselineRecon'] === 1) {
        elBaseline.val(0);
    } else if (dataCalculator['baselineRecon'] === 2) {
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

    //<editor-fold desc="Total Sales">
    if (dataCalculator['totalSalesRecon'] === 2) {
        if (!keyInput) {
            if (mainActivityDesc === "Non Running Rate - Cosmo") {
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
                    elTotalSales.val(actualSales);
                    totalSalesValue = actualSales;
                });
            } else {
                await getTotalSales().then(function () {
                    elTotalSales.val(totalSales);
                    totalSalesValue = totalSales;
                });
            }
        } else {
            totalSalesValue = (parseFloat(elTotalSales.val().replace(/,/g, '')));
        }
    }
    //</editor-fold>

    //<editor-fold desc="Uplift">
    if (dataCalculator['upLiftRecon'] === 0) {
        upliftValue = upliftValue * 100;
    } else if (dataCalculator['upLiftRecon'] === 1) {
        upliftValue = (parseFloat(elUplift.val().replace(/,/g, '')) + 100);
    } else if (dataCalculator['upLiftRecon'] === 2) {
        if (!keyInput) {
            await upliftFormula();
            upliftValue = (parseFloat(elUplift.val().replace(/,/g, '')));
        } else {
            upliftValue = (parseFloat(elUplift.val().replace(/,/g, '')));
        }
    }
    //</editor-fold>

    //<editor-fold desc="Sales Contribution">
    if (dataCalculator['salesContributionRecon'] === 0) {
        salesContributionValue = salesContributionValue * 100;
    } else if (dataCalculator['salesContributionRecon'] === 1) {
        salesContributionValue = parseFloat(elSalesContribution.val().replace(/,/g, ''));
    } else if (dataCalculator['salesContributionRecon'] === 2) {
        salesContributionValue = salesContributionValue * 100;
        elSalesContribution.val("");
    }
    //</editor-fold>

    //<editor-fold desc="Stores Coverage">
    if (dataCalculator['storesCoverageRecon'] === 0) {
        storesCoverageValue = storesCoverageValue * 100;
    } else if (dataCalculator['storesCoverageRecon'] === 1) {
        storesCoverageValue = parseFloat(elStoresCoverage.val().replace(/,/g, ''));
    } else if (dataCalculator['storesCoverageRecon'] === 2) {
        elStoresCoverage.val("");
        storesCoverageValue = storesCoverageValue * 100;
    }
    //</editor-fold>

    //<editor-fold desc="Redemption Rate">
    if (dataCalculator['redemptionRateRecon'] === 0) {
        redemptionRateValue = redemptionRateValue * 100;
    } else if (dataCalculator['redemptionRateRecon'] === 1) {
        redemptionRateValue = parseFloat(elRedemptionRate.val().replace(/,/g, ''));
    } else if (dataCalculator['redemptionRateRecon'] === 2) {
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
        crValue = parseFloat(elCr.val().replace(/,/g, ''));
    } else if (dataCalculator['crRecon'] === 1) {
        crValue = parseFloat(elCr.val().replace(/,/g, ''));
    } else if (dataCalculator['crRecon'] === 2) {
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

    if (dataCalculator['costRecon'] === 2) {
        let costValue = Math.round(baselineValue * ((upliftValue) / 100) * totalSalesValue * (salesContributionValue / 100) * (storesCoverageValue / 100) * (redemptionRateValue / 100) * (crValue / 100));
        if (!isFinite(costValue)) {
            return 0;
        } else {
            return costValue;
        }
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

const calculatorFormulaCreation = async (keyInput = false, skuId = 0) => {
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
            baselineValue = baseline;
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
                totalSalesValue = totalSales;
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
    form_data.append('promoId', promoId);
    form_data.append('mode', 'edit');

    form_data.append('file', file);
    form_data.append('row', 'row'+row);
    form_data.append('docLink', el.val());

    $.ajax({
        url: "/promo/send-back/attachment-upload",
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
            url: "/promo/send-back/recon/id",
            type: "GET",
            data: {id: pId},
            dataType: "JSON",
            success: async function (result) {
                if (!result['error']) {
                    let data = result['data'];
                    let promo = data['promo'];

                    promoReconConfigItem = data['editableConfig'];

                    $('#txt_info_method').text('Edit ' + promo['refId']);

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

                    $('#actualDnClaim').val(promo['actualDnClaim'] ?? "0");
                    $('#actualDnClaimFreeText').val(promo['actualDnClaimFreeText'] ?? "0");

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

                    mechanismCurrent = data['mechanism'];

                    lastCR = data['mechanism'][0]['cr'] ?? 0;

                    mechanismBefore = data['prevMechanism'];

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
                    await getBudget();
                    cost = promo['cost'];

                    if (!autoMechanism) {
                        $('#baselineBefore').val((data['prevMechanism'][0]['baseline'] === 0 ? '' : data['prevMechanism'][0]['baseline']));
                        $('#upliftBefore').val((data['prevMechanism'][0]['uplift'] === 0 ? '' : data['prevMechanism'][0]['uplift']));
                        $('#totalSalesBefore').val((data['prevMechanism'][0]['totalSales'] === 0 ? '' : data['prevMechanism'][0]['totalSales']));
                        $('#salesContributionBefore').val((data['prevMechanism'][0]['salesContribution'] === 0 ? '' : data['prevMechanism'][0]['salesContribution']));
                        $('#storesCoverageBefore').val((data['prevMechanism'][0]['storesCoverage'] === 0 ? '' : data['prevMechanism'][0]['storesCoverage']));
                        $('#redemptionRateBefore').val((data['prevMechanism'][0]['redemptionRate']  === 0 ? '' : data['prevMechanism'][0]['redemptionRate']));
                        $('#crBefore').val((data['prevMechanism'][0]['cr'] === 0 ? '' : data['prevMechanism'][0]['cr']));
                        $('#costBefore').val((data['prevMechanism'][0]['cost'] === 0 ? '' : data['prevMechanism'][0]['cost']));
                    }
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
    await submitSave(modifyReason);
}

const submitSave = async function (modifyReason) {
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

    let mechanismData = [];
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
    formData.append('notes_message', $('#notes_message').val());
    formData.append('initiatorNotes', initiatorNotes);

    let url = "/promo/send-back/revamp/update-recon";
    formData.append('promoId', promoId);

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

const editableConfig = (values) => {
    if (values) {
        $('#entityId').attr('readonly', values['entity']);
        $('#groupBrandId').attr('readonly', values['groupBrand']);
        $('#subCategoryId').attr('readonly', values['subCategory']);
        $('#activityId').attr('readonly',  values['activity']);
        $('#activityDesc').attr('readonly', values['activityName']);

        $('#distributorId').attr('readonly', values['distributor']);
        $('#channelId').attr('readonly', values['channel']);
        $('#subChannelId').attr('readonly', values['subChannel']);
        $('#accountId').attr('readonly', values['account']);
        $('#subAccountId').attr('readonly', values['subAccount']);

        let elStartPromo = $('#startPromo');
        let elEndPromo = $('#endPromo');
        if (values['startPromo']) {
            elStartPromo.flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: false,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                clickOpens: false,
            });
            elStartPromo.attr('readonly', true);
        }

        if (values['endPromo']) {
            elEndPromo.flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: false,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                clickOpens: false,
            });
            elEndPromo.attr('readonly', true);
        }

        if (values['mechanism']) {
            $('#btn_mechanism_edit').addClass('d-none');
        }

        $("#regionId").attr("readonly", values['region']);


        if (values['SKU']) {
            $('#btn_select_all_sku').addClass('d-none');
            $('#btn_deselect_all_sku').addClass('d-none');
            disabledSKU = values['SKU'];
        }

        //attachment
        if (values['attachment']) {
            $('.input_file').attr('disabled', true);
            $('.review_file_label').addClass('form-control-solid-bg');
            $('.btn_delete').attr('disabled', true);
        }
    }
}

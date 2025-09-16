'use strict';

let swalTitle = "Promo Calculator Configuration"
const elDtSubActivityCoverage = $('#dt_sub_activity_coverage');
const elDtSubActivitySelected = $('#dt_sub_activity_selected');
let dt_channel_source, dt_channel_selected;
const elDtChannelSource = $('#dt_channel_source');
const elDtChannelSelected = $('#dt_channel_selected');
let dt_sub_activity_coverage, dt_sub_activity_selected;
let dt_channel_calculator_configuration;
const elDtChannelCalculatorConfiguration = $('#dt_channel_calculator_configuration');
let url_str = new URL(window.location.href);
let method = url_str.searchParams.get("m");
let mainActivityId = url_str.searchParams.get("i");
let channelId = url_str.searchParams.get("c");
let categoryList = [], subCategoryList = [], activityList = [], subActivityList = [];
let channelList = [];
let validator;

let targetMainActivity = document.querySelector(".card_main_activity");
let blockUIMainActivity = new KTBlockUI(targetMainActivity, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetConfiguration = document.querySelector(".card_configuration");
let blockUIConfiguration = new KTBlockUI(targetConfiguration, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetSubActivityCoverage = document.querySelector(".card_sub_activity_coverage");
let blockUISubActivityCoverage = new KTBlockUI(targetSubActivityCoverage, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetChannelCalculatorConfiguration = document.querySelector(".card_channel_calculator_configuration");
let blockUIChannelCalculatorConfiguration = new KTBlockUI(targetChannelCalculatorConfiguration, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
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

    validator = FormValidation.formValidation(document.getElementById('formConfiguration'), {
        fields: {
            mainActivity: {
                validators: {
                    notEmpty: {
                        message: "Please fill in main activity"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })

    blockUIMainActivity.block();
    blockUIConfiguration.block();
    blockUISubActivityCoverage.block();
    blockUIChannelCalculatorConfiguration.block();
    getCoverage().then(async function () {
        // category
        let categoryDropdown = [];
        for (let i = 0; i < categoryList.length; i++) {
            categoryDropdown.push({
                id: categoryList[i]['categoryid'],
                text: categoryList[i]['categorydesc']
            });
        }
        $('#dt_sub_activity_coverage_category').select2({
            placeholder: "Select a Category",
            width: '100%',
            data: categoryDropdown
        });
        $('#dt_sub_activity_selected_category').select2({
            placeholder: "Select a Category",
            width: '100%',
            data: categoryDropdown
        });
        dt_sub_activity_coverage.rows.add(subActivityList).draw();

        //Channel
        await getChannel().then(async function () {
            dt_channel_source.rows.add(channelList).draw();
        });

        if (method === "update") {
            await getData(mainActivityId, channelId);
        } else {
            defaultCalculatorConfig();
        }

        blockUIMainActivity.release();
        blockUIConfiguration.release();
        blockUISubActivityCoverage.release();
        blockUIChannelCalculatorConfiguration.release();

    });

    dt_sub_activity_coverage = elDtSubActivityCoverage.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "28vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Double click to select Sub Activity',
                data: 'subActivity',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 1,
                title: 'Category',
                data: 'category',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 2,
                title: 'Sub Category',
                data: 'subCategory',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 3,
                title: 'Activity',
                data: 'activity',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 4,
                title: 'Main Activity',
                data: 'mainActivity',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    elDtSubActivityCoverage.on('dblclick', 'tr', function () {
        let dataSubActivity = dt_sub_activity_coverage.row( this ).data();
        let dataSubActivitySelected = dt_sub_activity_selected.rows().data().toArray();
        if (dataSubActivity['available'] === 0) {
            return Swal.fire({
                title: "Select Sub Activity",
                text: `Sub Activity already used in Main Activity ${dataSubActivity['mainActivity']}`,
                icon: "warning",
                allowOutsideClick: false,
                allowEscapeKey: false,
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
        if (dt_sub_activity_selected.rows().data().length >= 1) {
            let filter = {
                subActivityId: dataSubActivity['subActivityId']
            };
            dataSubActivitySelected = dataSubActivitySelected.filter(function (item) {
                for (let key in filter) {
                    if (item[key] === undefined || item[key] !== filter[key]) {
                        return false;
                    }
                }
                return true;
            });
            if (dataSubActivitySelected.length >= 1) {
                return Swal.fire({
                    title: "Select Sub Activity",
                    text: "Sub Activity already added",
                    icon: "warning",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            } else {
                let dataSelected = [];
                dataSelected['category'] = dataSubActivity['category'];
                dataSelected['subCategory'] = dataSubActivity['subCategory'];
                dataSelected['activity'] = dataSubActivity['activity'];
                dataSelected['subActivityId'] = dataSubActivity['subActivityId'];
                dataSelected['subActivity'] = dataSubActivity['subActivity'];
                dt_sub_activity_selected.row.add(dataSelected).draw();

                Swal.fire({
                    title: 'Sub Activity added',
                    icon: "success",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-primary"}
                });
            }
        } else {
            let dataSelected = [];
            dataSelected['category'] = dataSubActivity['category'];
            dataSelected['subCategory'] = dataSubActivity['subCategory'];
            dataSelected['activity'] = dataSubActivity['activity'];
            dataSelected['subActivityId'] = dataSubActivity['subActivityId'];
            dataSelected['subActivity'] = dataSubActivity['subActivity'];
            dt_sub_activity_selected.row.add(dataSelected).draw();

            Swal.fire({
                title: 'Sub Activity added',
                icon: "success",
                allowOutsideClick: false,
                allowEscapeKey: false,
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-primary"}
            });
        }
    })

    $('#dt_sub_activity_coverage_search').on('keyup', function () {
        dt_sub_activity_coverage.column(0).search(this.value, false, false).draw();
    });

    dt_sub_activity_selected = elDtSubActivitySelected.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "28vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Selected Sub Activity',
                data: 'subActivity',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 1,
                title: 'Category',
                data: 'category',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 2,
                title: 'Sub Category',
                data: 'subCategory',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 3,
                title: 'Activity',
                data: 'activity',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    elDtSubActivitySelected.on( 'dblclick', 'tr', function () {
        let tr = this.closest("tr");
        let trIndex = dt_sub_activity_selected.row(tr).index();
        let dataSubActivity = dt_sub_activity_selected.row(tr).data();
        dt_sub_activity_selected.row(trIndex).remove().draw();
        let dataSubActivityCoverage = dt_sub_activity_coverage.rows().data();
        for (let i=0; i<dataSubActivityCoverage.length; i++) {
            if (dataSubActivityCoverage[i]['subActivityId'] === dataSubActivity['subActivityId']) {
                dataSubActivityCoverage[i]['mainActivity'] = '';
                dataSubActivityCoverage[i]['available'] = 1;
            }
        }
        dt_sub_activity_coverage.clear().draw();
        dt_sub_activity_coverage.rows.add(dataSubActivityCoverage).draw();

        Swal.fire({
            title: 'Sub Activity removed',
            icon: "success",
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-primary"}
        });
    });

    $('#dt_sub_activity_selected_search').on('keyup', function () {
        dt_sub_activity_selected.column(0).search(this.value, false, false).draw();
    });

    dt_channel_source = elDtChannelSource.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "28vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'channelId',
                visible: false,
            },
            {
                targets: 1,
                title: 'Double click to select Channel',
                data: 'channelLongDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    elDtChannelSource.on('dblclick', 'tr', function () {
        let dataChannel = dt_channel_source.row( this ).data();
        let dataChannelSelected = dt_channel_selected.rows().data().toArray();

        if (dt_channel_selected.rows().data().length >= 1) {
            let filter = {
                channelId: dataChannel['channelId']
            };
            dataChannelSelected = dataChannelSelected.filter(function (item) {
                for (let key in filter) {
                    if (item[key] === undefined || item[key] !== filter[key]) {
                        return false;
                    }
                }
                return true;
            });
            if (dataChannelSelected.length >= 1) {
                return Swal.fire({
                    title: "Select Channel",
                    text: "Channel already added",
                    icon: "warning",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            } else {
                let dataSelected = [];
                dataSelected['channelId'] = dataChannel['channelId'];
                dataSelected['channelLongDesc'] = dataChannel['channelLongDesc'];
                dt_channel_selected.row.add(dataSelected).draw();

                Swal.fire({
                    title: 'Channel added',
                    icon: "success",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-primary"}
                });
            }
        } else {
            let dataSelected = [];
            dataSelected['channelId'] = dataChannel['channelId'];
            dataSelected['channelLongDesc'] = dataChannel['channelLongDesc'];
            dt_channel_selected.row.add(dataSelected).draw();

            Swal.fire({
                title: 'Channel added',
                icon: "success",
                allowOutsideClick: false,
                allowEscapeKey: false,
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-primary"}
            });
        }
    })

    $('#dt_channel_source_search').on('keyup', function () {
        dt_channel_source.column(1).search(this.value, false, false).draw();
    });

    dt_channel_selected = elDtChannelSelected.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "28vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'channelId',
                visible: false,
            },
            {
                targets: 1,
                title: 'Selected Channel',
                data: 'channelLongDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    elDtChannelSelected.on( 'dblclick', 'tr', function () {
        let tr = this.closest("tr");
        let trIndex = dt_channel_selected.row(tr).index();
        let dataChannelSelected = dt_channel_selected.rows().data().toArray();

        if (dataChannelSelected.length >= 1) {
            dt_channel_selected.row(trIndex).remove().draw();

            Swal.fire({
                title: 'Channel removed',
                icon: "success",
                allowOutsideClick: false,
                allowEscapeKey: false,
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-primary"}
            });
        }
    });

    $('#dt_channel_selected_search').on('keyup', function () {
        dt_channel_selected.column(1).search(this.value, false, false).draw();
    });

    //Channel Configuration
    dt_channel_calculator_configuration = elDtChannelCalculatorConfiguration.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: true,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "28vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[0, 'asc']],
        columnDefs: [
            {
                targets: 0,
                data: 'channelId',
                visible: false,
            },
            {
                targets: 1,
                title: 'Channel',
                data: 'channelLongDesc',
                className: 'text-nowrap align-middle cursor-pointer',
                orderable: false,
            },
            {
                targets: 2,
                title: 'Baseline',
                data: 'baseline',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 3,
                title: 'Uplift',
                data: 'uplift',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 4,
                title: 'Total Sales',
                data: 'totalSales',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 5,
                title: 'Sales Contribution',
                data: 'salesContribution',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 6,
                title: 'Store Coverage',
                data: 'storesCoverage',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 7,
                title: 'Redemption Rate',
                data: 'redemptionRate',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 8,
                title: 'CR',
                data: 'cr',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 9,
                title: 'Cost',
                data: 'cost',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 10,
                title: 'Baseline Recon',
                data: 'baselineRecon',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 11,
                title: 'Uplift Recon',
                data: 'upliftRecon',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 12,
                title: 'Total Sales Recon',
                data: 'totalSalesRecon',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 13,
                title: 'Sales Contribution Recon',
                data: 'salesContributionRecon',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 14,
                title: 'Store Coverage Recon',
                data: 'storesCoverageRecon',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 15,
                title: 'Redemption Rate Recon',
                data: 'redemptionRateRecon',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 16,
                title: 'CR Recon',
                data: 'crRecon',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
            {
                targets: 17,
                title: 'Cost Recon',
                data: 'costRecon',
                className: 'text-nowrap align-middle cursor-pointer text-center',
                orderable: false,
                render: function (data) {
                    if (data === 0) {
                        return 'Disabled';
                    } else if (data === 1) {
                        return 'Enabled';
                    } else {
                        return 'Auto';
                    }
                }
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });
});

//<editor-fold desc="Coverage Hierarchy">
$('#dt_sub_activity_coverage_category').on('change', async function () {
    let elSubCategory = $('#dt_sub_activity_coverage_sub_category');
    let elActivity = $('#dt_sub_activity_coverage_activity');

    let categoryId = parseInt($(this).val());
    let dataSelected = $(this).select2('data');
    let categoryText = dataSelected[0].text;

    elSubCategory.empty();
    elActivity.empty();
    elSubCategory.val('').trigger('change');
    elActivity.val('').trigger('change');
    let subCategoryDropdown = [{id:'', text:''}];
    for (let i = 0; i < subCategoryList.length; i++) {
        if (subCategoryList[i]['categoryid'] === categoryId) {
            subCategoryDropdown.push({
                id: subCategoryList[i]['subcategoryid'],
                text: subCategoryList[i]['subcategorydesc']
            });
        }
    }
    elSubCategory.select2({
        placeholder: "Select a Sub Category",
        width: '100%',
        data: subCategoryDropdown
    });
    elSubCategory.val('').on('change.select2');
    dt_sub_activity_coverage.column(1).search(categoryText, false, false).draw();
});

$('#dt_sub_activity_coverage_sub_category').on('change', async function () {
    let elActivity = $('#dt_sub_activity_coverage_activity');

    let subCategoryId = parseInt($(this).val());
    let dataSelected = $(this).select2('data');
    let subCategoryText = '';
    if (dataSelected.length > 0) subCategoryText = dataSelected[0].text;

    elActivity.empty();
    elActivity.val('').trigger('change');
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
    });
    elActivity.val('').on('change.select2');
    dt_sub_activity_coverage.column(2).search(subCategoryText, false, false).draw();
});

$('#dt_sub_activity_coverage_activity').on('change', async function () {
    let dataSelected = $(this).select2('data');
    let activityText = '';
    if (dataSelected.length > 0) activityText = dataSelected[0].text;
    dt_sub_activity_coverage.column(3).search(activityText, false, false).draw();
});
//</editor-fold>

//<editor-fold desc="Selected Hierarchy">
$('#dt_sub_activity_selected_category').on('change', async function () {
    let elSubCategory = $('#dt_sub_activity_selected_sub_category');
    let elActivity = $('#dt_sub_activity_selected_activity');

    let categoryId = parseInt($(this).val());
    let dataSelected = $(this).select2('data');
    let categoryText = dataSelected[0].text;

    elSubCategory.empty();
    elActivity.empty();
    elSubCategory.val('').trigger('change');
    let subCategoryDropdown = [{id:'', text:''}];
    for (let i = 0; i < subCategoryList.length; i++) {
        if (subCategoryList[i]['categoryid'] === categoryId) {
            subCategoryDropdown.push({
                id: subCategoryList[i]['subcategoryid'],
                text: subCategoryList[i]['subcategorydesc']
            });
        }
    }
    elSubCategory.select2({
        placeholder: "Select a Sub Category",
        width: '100%',
        data: subCategoryDropdown
    });
    elSubCategory.val('').on('change.select2');
    dt_sub_activity_selected.column(1).search(categoryText, false, false).draw();
});

$('#dt_sub_activity_selected_sub_category').on('change', async function () {
    let elActivity = $('#dt_sub_activity_selected_activity');

    let subCategoryId = parseInt($(this).val());
    let dataSelected = $(this).select2('data');
    let subCategoryText = '';
    if (dataSelected.length > 0) subCategoryText = dataSelected[0].text;

    elActivity.empty();
    elActivity.val('').trigger('change');
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
    });
    elActivity.val('').on('change.select2');
    dt_sub_activity_selected.column(2).search(subCategoryText, false, false).draw();
});

$('#dt_sub_activity_selected_activity').on('change', async function () {
    let dataSelected = $(this).select2('data');
    let activityText = '';
    if (dataSelected.length > 0) activityText = dataSelected[0].text;
    dt_sub_activity_selected.column(3).search(activityText, false, false).draw();
});
//</editor-fold>

$('#btn_update_configuration').on('click', function () {
    let dataChannelSelected = dt_channel_selected.rows().data().toArray();
    let dataConfig = dt_channel_calculator_configuration.rows().data().toArray();

    let dataCalculatorConfig = [];
    if (dataChannelSelected.length >= 1) {
        for (let i = 0; i < dataChannelSelected.length; i++) {
            if (dataConfig.length >= 1) {
                dt_channel_calculator_configuration.rows().every(function () {
                    let data = this.data();
                    if (data !== undefined) {
                        if (data.channelId === dataChannelSelected[i]['channelId']) {
                            this.remove();
                        }
                    }
                });
            }
            dataCalculatorConfig.push({
                channelId: dataChannelSelected[i]['channelId'],
                channelLongDesc: dataChannelSelected[i]['channelLongDesc'],
                baseline: parseInt($('#baseline').val()),
                uplift: parseInt($('#uplift').val()),
                totalSales: parseInt($('#totalSales').val()),
                salesContribution: parseInt($('#salesContribution').val()),
                storesCoverage: parseInt($('#storesCoverage').val()),
                redemptionRate: parseInt($('#redemptionRate').val()),
                cr: parseInt($('#cr').val()),
                cost: parseInt($('#cost').val()),
                baselineRecon: parseInt($('#baselineRecon').val()),
                upliftRecon: parseInt($('#upliftRecon').val()),
                totalSalesRecon: parseInt($('#totalSalesRecon').val()),
                salesContributionRecon: parseInt($('#salesContributionRecon').val()),
                storesCoverageRecon: parseInt($('#storesCoverageRecon').val()),
                redemptionRateRecon: parseInt($('#redemptionRateRecon').val()),
                crRecon: parseInt($('#crRecon').val()),
                costRecon: parseInt($('#costRecon').val())
            });
        }

        dt_channel_calculator_configuration.rows.add(dataCalculatorConfig).draw();
        resetCalculator();
    } else {
        return Swal.fire({
            title: "Update Configuration",
            text: "Please select channel",
            icon: "warning",
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let trSubActivity = dt_sub_activity_selected.rows().data();
            let trChannelCalculatorConfig = dt_channel_calculator_configuration.rows().data();
            if (trSubActivity.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: 'Please select sub activity',
                    icon: "error",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
            if (trChannelCalculatorConfig.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: 'Please select channel calculator',
                    icon: "error",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }

            let subActivity = []
            for (let i = 0; i < trSubActivity.length; i++) {
                subActivity.push(trSubActivity[i]['subActivityId']);
            }

            let calculatorConfiguration = []
            for (let i = 0; i < trChannelCalculatorConfig.length; i++) {
                calculatorConfiguration.push({
                    mainActivity: $('#mainActivity').val(),
                    channelId: trChannelCalculatorConfig[i]['channelId'],
                    baseline: parseInt(trChannelCalculatorConfig[i]['baseline']),
                    uplift: parseInt(trChannelCalculatorConfig[i]['uplift'],),
                    totalSales: parseInt(trChannelCalculatorConfig[i]['totalSales']),
                    salesContribution: parseInt(trChannelCalculatorConfig[i]['salesContribution']),
                    storesCoverage: parseInt(trChannelCalculatorConfig[i]['storesCoverage']),
                    redemptionRate: parseInt(trChannelCalculatorConfig[i]['redemptionRate']),
                    cr: parseInt(trChannelCalculatorConfig[i]['cr'],),
                    cost: parseInt(trChannelCalculatorConfig[i]['cost'],),
                    baselineRecon: parseInt(trChannelCalculatorConfig[i]['baselineRecon']),
                    upliftRecon: parseInt(trChannelCalculatorConfig[i]['upliftRecon']),
                    totalSalesRecon: parseInt(trChannelCalculatorConfig[i]['totalSalesRecon']),
                    salesContributionRecon: parseInt(trChannelCalculatorConfig[i]['salesContributionRecon']),
                    storesCoverageRecon: parseInt(trChannelCalculatorConfig[i]['storesCoverageRecon']),
                    redemptionRateRecon: parseInt(trChannelCalculatorConfig[i]['redemptionRateRecon']),
                    crRecon: parseInt(trChannelCalculatorConfig[i]['crRecon']),
                    costRecon: parseInt(trChannelCalculatorConfig[i]['costRecon'])
                });
            }

            let url = '/configuration/promo-calculator/save';
            if (method === "update") {
                url = '/configuration/promo-calculator/update';
                calculatorConfiguration.forEach((obj, index) => {
                    obj.mainActivityId = mainActivityId;
                });
            }

            let formData = new FormData($('#formConfiguration')[0]);
            formData.append('subActivity', JSON.stringify(subActivity));
            formData.append('calculatorConfiguration', JSON.stringify(calculatorConfiguration));

            $.get('/refresh-csrf').done(function(data) {
                let elMeta = $('meta[name="csrf-token"]');
                elMeta.attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': elMeta.attr('content')
                    }
                });
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
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
                    success: function(result) {
                        if (!result.error) {
                            Swal.fire({
                                title: swalTitle,
                                text: result.message,
                                icon: "success",
                                confirmButtonText: "OK",
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(function () {
                                window.location.href = '/configuration/promo-calculator';
                            });
                        } else {
                            Swal.fire({
                                title: swalTitle,
                                text: result.message,
                                icon: "error",
                                confirmButtonText: "OK",
                                customClass: {confirmButton: "btn btn-optima"}
                            });
                        }
                    },
                    complete: function() {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown)
                        Swal.fire({
                            title: swalTitle,
                            text: "Failed to save data, an error occurred in the process",
                            icon: "error",
                            confirmButtonText: "OK",
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                });
            });
        }
    });
});

const getData = (p_main_activity_id, p_channel_id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/promo-calculator/data",
            type        : "GET",
            data        : {mainActivityId: p_main_activity_id, channelId: p_channel_id},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    let data = result['data'];

                    let dataChannelSelected = [];
                    dataChannelSelected.push({
                        "channelId": data['channelId'],
                        "channelLongDesc": data['channelLongDesc']
                    });

                    $('#mainActivity').val(data['mainActivityDesc']);
                    $('#baseline').val(data['baseline']).trigger('change');
                    $('#uplift').val(data['uplift']).trigger('change');
                    $('#totalSales').val(data['totalSales']).trigger('change');
                    $('#salesContribution').val(data['salesContribution']).trigger('change');
                    $('#storesCoverage').val(data['storesCoverage']).trigger('change');
                    $('#redemptionRate').val(data['redemptionRate']).trigger('change');
                    $('#cr').val(data['cr']).trigger('change');
                    $('#cost').val(data['cost']).trigger('change');
                    $('#baselineRecon').val(data['baselineRecon'] ?? '0').trigger('change');
                    $('#upliftRecon').val(data['upliftRecon'] ?? '0').trigger('change');
                    $('#totalSalesRecon').val(data['totalSalesRecon'] ?? '0').trigger('change');
                    $('#salesContributionRecon').val(data['salesContributionRecon'] ?? '0').trigger('change');
                    $('#storesCoverageRecon').val(data['storesCoverageRecon'] ?? '0').trigger('change');
                    $('#redemptionRateRecon').val(data['redemptionRateRecon'] ?? '0').trigger('change');
                    $('#crRecon').val(data['crRecon'] ?? '0').trigger('change');
                    $('#costRecon').val(data['costRecon'] ?? '0').trigger('change');

                    dt_sub_activity_selected.rows.add(data['subActivityList']).draw();
                    dt_channel_selected.rows.add(dataChannelSelected).draw();
                    dt_channel_calculator_configuration.rows.add(data['channelList']).draw();
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

const getCoverage = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/promo-calculator/coverage",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    if (result['data']) {
                        let data = result['data'];

                        categoryList = data['category'];
                        subCategoryList = data['subCategory'];
                        activityList = data['activity'];
                        subActivityList = data['subActivity'];
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

const getChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/promo-calculator/channel",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    if (result['data']) {
                        channelList = result['data'];
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

const resetCalculator = () => {
    //Reset Table Selected Channel
    dt_channel_selected.rows().remove().draw();

    //Reset Dropdown Channel Calculator
    $('#baseline').val(0).trigger('change');
    $('#uplift').val(0).trigger('change');
    $('#totalSales').val(0).trigger('change');
    $('#salesContribution').val(0).trigger('change');
    $('#storesCoverage').val(0).trigger('change');
    $('#redemptionRate').val(0).trigger('change');
    $('#cr').val(0).trigger('change');
    $('#cost').val(0).trigger('change');

    $('#baselineRecon').val(0).trigger('change');
    $('#upliftRecon').val(0).trigger('change');
    $('#totalSalesRecon').val(0).trigger('change');
    $('#salesContributionRecon').val(0).trigger('change');
    $('#storesCoverageRecon').val(0).trigger('change');
    $('#redemptionRateRecon').val(0).trigger('change');
    $('#crRecon').val(0).trigger('change');
    $('#costRecon').val(0).trigger('change');
}

const defaultCalculatorConfig = () => {
    let defaultCalculatorConfig = channelList;
    defaultCalculatorConfig.forEach((obj, index) => {
        obj.mainActivityId = mainActivityId;
        obj.baseline = 0;
        obj.uplift = 0;
        obj.totalSales = 0;
        obj.salesContribution = 0;
        obj.storesCoverage = 0;
        obj.redemptionRate = 0;
        obj.cr = 0;
        obj.cost = 0;
        obj.baselineRecon = 0;
        obj.upliftRecon = 0;
        obj.totalSalesRecon = 0;
        obj.salesContributionRecon = 0;
        obj.storesCoverageRecon = 0;
        obj.redemptionRateRecon = 0;
        obj.crRecon = 0;
        obj.costRecon = 0;
    });
    dt_channel_calculator_configuration.rows.add(defaultCalculatorConfig).draw();
}

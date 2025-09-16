'use strict';

let swalTitle = "Generate Promo ID";
let dialerObject;
let distributorList = [], brandList = [], categoryList = [], subAccountList = [], channelList = [], subActivityRcList = [], subActivityDcList = [];
let elDistributor = $('#distributorId');
let elBrand = $('#groupBrandId');
let elCategory = $('#categoryId');
let elSubAccount = $('#subAccountId');
let elChannel = $('#channelId');
let elDtSubActivity = $('#dt_sub_activity');
let elDtSubActivitySelected = $('#dt_sub_activity_selected');
let elDtGeneratedResult = $('#dt_generated_result');
let dt_sub_activity, dt_sub_activity_selected, dt_generated_result;
let validator;

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

let targetHeader = document.querySelector(".card_header");
let blockUIHeader = new KTBlockUI(targetHeader, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetSubActivity = document.querySelector(".card_sub_activity");
let blockUISubActivity = new KTBlockUI(targetSubActivity, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetResult = document.querySelector(".card_result");
let blockUIResult = new KTBlockUI(targetResult, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

//<editor-fold desc="Document on load">
$(document).ready(function () {
    $('form').submit(false);

    validator = FormValidation.formValidation(document.getElementById('form_promo'), {
        fields: {
            period: {
                validators: {
                    notEmpty: {
                        message: "Please fill in period"
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
            groupBrandId: {
                validators: {
                    notEmpty: {
                        message: "Please select a brand"
                    },
                }
            },
            categoryId: {
                validators: {
                    notEmpty: {
                        message: "Please select a category"
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
            channelId: {
                validators: {
                    notEmpty: {
                        message: "Please select a channel"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
        min: 2025
    });

    dt_sub_activity = elDtSubActivity.DataTable({
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
                title: 'Sub Activity',
                data: 'subActivityDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 1,
                title: 'Activity',
                data: 'activityDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 2,
                title: 'Sub Category',
                data: 'subCategoryDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $('#dt_sub_activity_search').on('keyup', function () {
        dt_sub_activity.search(this.value, false, false).draw();
    });

    elDtSubActivity.on('dblclick', 'tr', function () {
        let dataSubActivity = dt_sub_activity.row( this ).data();
        let dataSubActivitySelected = dt_sub_activity_selected.rows().data().toArray();
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
                    text: "Sub Activity already exist",
                    icon: "warning",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            } else {
                dt_sub_activity_selected.row.add(dataSubActivity).draw();
            }
        } else {
            dt_sub_activity_selected.row.add(dataSubActivity).draw();
        }
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
        scrollY: "27vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Sub Activity',
                data: 'subActivityDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 1,
                title: 'Activity',
                data: 'activityDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 2,
                title: 'Sub Category',
                data: 'subCategoryDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $('#dt_sub_activity_selected_search').on('keyup', function () {
        dt_sub_activity_selected.search(this.value, false, false).draw();
    });

    elDtSubActivitySelected.on( 'dblclick', 'tr', function () {
        let tr = this.closest("tr");
        let trIndex = dt_sub_activity_selected.row(tr).index();
        dt_sub_activity_selected.row(trIndex).remove().draw();
    });

    dt_generated_result = elDtGeneratedResult.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-5'i><'col-sm-7'p>>",
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
                title: 'Period',
                data: 'period',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Distributor',
                data: 'distributorDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Brand',
                data: 'brandDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Category',
                data: 'categoryDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Sub Category',
                data: 'subCategoryDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Activity',
                data: 'activityDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Sub Activity',
                data: 'subActivityDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Channel',
                data: 'channelDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Sub Account',
                data: 'subAccountDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: 'Result',
                data: 'result',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $('#dt_generated_result_search').on('keyup', function () {
        dt_generated_result.search(this.value, false, false).draw();
    });

    blockUIHeader.block();
    blockUISubActivity.block();
    blockUIResult.block();
    getListAttribute().then(function () {
        //<editor-fold desc="Category">
        let categoryDropdown = [];
        for (let i = 0; i < categoryList.length; i++) {
            categoryDropdown.push({
                id: categoryList[i]['categoryId'],
                text: categoryList[i]['categoryDesc']
            });
        }
        $('#categoryId').select2({
            placeholder: "Select a Category",
            width: '100%',
            data: categoryDropdown
        }).on('change', function () {
            // Revalidate the color field when an option is chosen
            validator.revalidateField('categoryId');
        });
        //</editor-fold>

        //<editor-fold desc="Distributor">
        let distributorDropdown = [];
        for (let i = 0; i < distributorList.length; i++) {
            distributorDropdown.push({
                id: distributorList[i]['distributorId'],
                text: distributorList[i]['distributorDesc']
            });
        }
        elDistributor.select2({
            placeholder: "Select a Distributor",
            width: '100%',
            data: distributorDropdown
        }).on('change', function () {
            // Revalidate the color field when an option is chosen
            validator.revalidateField('distributorId');
        });
        //</editor-fold>

        //<editor-fold desc="Brand">
        let brandDropdown = [];
        for (let i = 0; i < brandList.length; i++) {
            brandDropdown.push({
                id: brandList[i]['groupBrandId'],
                text: brandList[i]['groupBrandDesc']
            });
        }
        elBrand.select2({
            placeholder: "Select a Brand",
            width: '100%',
            data: brandDropdown
        }).on('change', function () {
            // Revalidate the color field when an option is chosen
            validator.revalidateField('groupBrandId');
        });
        //</editor-fold>

        blockUIHeader.release();
        blockUISubActivity.release();
        blockUIResult.release();
    });
});
//</editor-fold>

elCategory.on('change', function () {
    let category = $('#categoryId').select2('data');
    if (category.length > 0) {
        if (category[0]['text'] === "Retailer Cost") {
            validator.disableValidator('channelId');
            validator.enableValidator('subAccountId');
            $('#fieldSubAccount').removeClass('d-none');
            $('#fieldChannel').addClass('d-none');

            //<editor-fold desc="Sub Account">
            elSubAccount.empty();
            let subAccountDropdown = [{id: "", text:""}];
            for (let i = 0; i < subAccountList.length; i++) {
                subAccountDropdown.push({
                    id: subAccountList[i]['subAccountId'],
                    text: subAccountList[i]['subAccountDesc']
                });
            }
            elSubAccount.select2({
                placeholder: "Select a Sub Account",
                width: '100%',
                data: subAccountDropdown
            }).on('change', function () {
                // Revalidate the color field when an option is chosen
                validator.revalidateField('subAccountId');
            });
            //</editor-fold>

            dt_sub_activity.clear().draw();
            dt_sub_activity_selected.clear().draw();

            dt_sub_activity.rows.add(subActivityRcList).draw();
        } else {
            validator.disableValidator('subAccountId');
            validator.enableValidator('channelId');
            $('#fieldSubAccount').addClass('d-none');
            $('#fieldChannel').removeClass('d-none');

            //<editor-fold desc="Channel">
            elChannel.empty();
            let channelDropdown = [{id: "", text:""}];
            for (let i = 0; i < channelList.length; i++) {
                channelDropdown.push({
                    id: channelList[i]['channelId'],
                    text: channelList[i]['channelDesc']
                });
            }
            elChannel.select2({
                placeholder: "Select a Channel",
                width: '100%',
                data: channelDropdown
            }).on('change', function () {
                // Revalidate the color field when an option is chosen
                validator.revalidateField('channelId');
            });
            //</editor-fold>

            dt_sub_activity.clear().draw();
            dt_sub_activity_selected.clear().draw();

            dt_sub_activity.rows.add(subActivityDcList).draw();
        }
    }
});

$('#btn_generate').on('click', function () {
    let elProgress = $('#loadingProcess');

    validator.validate().then(async function (status) {
        if (status === "Valid") {
            let subActivitySelected = dt_sub_activity_selected.rows().data();

            if (subActivitySelected.length < 1) {
                return Swal.fire({
                    title: swalTitle,
                    text: "Please select one or more sub activity",
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }

            //<editor-fold desc="Generate Combination Value">
            let dataSubActivity = [];
            let channel = [];
            let subAccount = [];
            let category = $('#categoryId').select2('data');

            for (let i=0; i<subActivitySelected.length; i++) {
                dataSubActivity.push({
                    subActivityId : subActivitySelected[i]['subActivityId'],
                    subActivityDesc : subActivitySelected[i]['subActivityDesc'],
                    activityDesc : subActivitySelected[i]['activityDesc'],
                    subCategoryDesc : subActivitySelected[i]['subCategoryDesc'],
                });
            }

            let brand = elBrand.select2('data');
            let distributor = elDistributor.select2('data');

            let data = [];
            if (category.length > 0) {
                if (category[0]['text'] === "Retailer Cost") {
                    subAccount = elSubAccount.select2('data');

                    dt_generated_result.column(7).visible(false);
                    dt_generated_result.column(8).visible(true);

                    for (let i=0; i<dataSubActivity.length; i++) {
                        for (let j=0; j<subAccount.length; j++) {
                            for (let k=0; k<brand.length; k++) {
                                for (let l=0; l<distributor.length; l++) {
                                    data.push({
                                        period: $('#period').val(),
                                        subActivity: dataSubActivity[i]['subActivityId'],
                                        subActivityDesc: dataSubActivity[i]['subActivityDesc'],
                                        activityDesc: dataSubActivity[i]['activityDesc'],
                                        subCategoryDesc: dataSubActivity[i]['subCategoryDesc'],
                                        category:  elCategory.val(),
                                        categoryDesc: category[0]['text'],
                                        channel: 0,
                                        channelDesc: "",
                                        subAccount: subAccount[j]['id'],
                                        subAccountDesc: subAccount[j]['text'],
                                        brand: brand[k]['id'],
                                        brandDesc: brand[k]['text'],
                                        distributor: distributor[l]['id'],
                                        distributorDesc: distributor[l]['text']
                                    });
                                }
                            }
                        }
                    }
                } else {
                    channel = elChannel.select2('data');

                    dt_generated_result.column(7).visible(true);
                    dt_generated_result.column(8).visible(false);

                    for (let i=0; i<dataSubActivity.length; i++) {
                        for (let j=0; j<channel.length; j++) {
                            for (let k=0; k<brand.length; k++) {
                                for (let l=0; l<distributor.length; l++) {
                                    data.push({
                                        period: $('#period').val(),
                                        subActivity: dataSubActivity[i]['subActivityId'],
                                        subActivityDesc: dataSubActivity[i]['subActivityDesc'],
                                        activityDesc: dataSubActivity[i]['activityDesc'],
                                        subCategoryDesc: dataSubActivity[i]['subCategoryDesc'],
                                        category:  elCategory.val(),
                                        categoryDesc: category[0]['text'],
                                        channel: channel[j]['id'],
                                        channelDesc: channel[j]['text'],
                                        subAccount: 0,
                                        subAccountDesc: "",
                                        brand: brand[k]['id'],
                                        brandDesc: brand[k]['text'],
                                        distributor: distributor[l]['id'],
                                        distributorDesc: distributor[l]['text']
                                    });
                                }
                            }
                        }
                    }
                }
            }
            //</editor-fold>

            dt_generated_result.clear().draw();
            let e = document.querySelector("#btn_generate");
            blockUIHeader.block();
            blockUISubActivity.block();
            blockUIResult.block();
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            let messageError = "";
            elProgress.removeClass('d-none');
            let perc = 0;
            let error = false;
            let failed = false;
            for (let i=1; i <= data.length; i++) {
                let dataRow = data[i-1];
                let res_method = await process(dataRow);
                if (res_method.error) {
                    error = true;
                    messageError = res_method.message;
                    break;
                }
                if (!res_method.error) {
                    perc = ((i / data.length) * 100).toFixed(0);
                    $('#text_progress').text(perc.toString() + '%');
                    let progress_import = $('#progress_bar');
                    progress_import.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());
                    dt_generated_result.rows.add([{
                        period : dataRow['period'],
                        distributorDesc : dataRow['distributorDesc'],
                        brandDesc : dataRow['brandDesc'],
                        categoryDesc : dataRow['categoryDesc'],
                        subCategoryDesc : dataRow['subCategoryDesc'],
                        activityDesc : dataRow['activityDesc'],
                        subActivityDesc : dataRow['subActivityDesc'],
                        subAccountDesc : dataRow['subAccountDesc'],
                        channelDesc : dataRow['channelDesc'],
                        result : res_method['message']
                    }]).draw();
                    if (!res_method['message'].includes(`/AL${dataRow['period'].substr(-2)}/`)) failed = true;
                    if (i === data.length) {
                        let iconSwal = "error";
                        let textSwal = "Generate Promo ID Failed";
                        if (!failed) {
                            iconSwal = "success";
                            textSwal = "Generate Promo ID Complete";
                        }
                        Swal.fire({
                            title: swalTitle,
                            text: textSwal,
                            icon: iconSwal,
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            progress_import.css('width', '0%').attr('aria-valuenow', '0');
                            blockUIHeader.release();
                            blockUISubActivity.release();
                            blockUIResult.release();
                            e.setAttribute("data-kt-indicator", "off");
                            e.disabled = !1;
                            elProgress.addClass('d-none');
                        });
                    }
                }
            }
            if (error) {
                let progress_import = $('#progress_bar');
                progress_import.css('width', '0%').attr('aria-valuenow', '0');
                blockUIHeader.release();
                blockUISubActivity.release();
                blockUIResult.release();
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
                elProgress.addClass('d-none');
                Swal.fire({
                    title: swalTitle,
                    text: messageError,
                    icon: "warning",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: { confirmButton: "btn btn-optima" }
                });
            }
        }
    });
});

const process = (data) =>  {
    return new Promise(function (resolve) {
        let formData = new FormData();
        formData.append('period', data['period']);
        formData.append('category', data['category']);
        formData.append('distributor', data['distributor']);
        formData.append('brand', data['brand']);
        formData.append('channel', data['channel']);
        formData.append('subAccount', data['subAccount']);
        formData.append('subActivity', data['subActivity']);
        let url = "/promo/creation/generate-promo";
        $.ajax({
            type        : 'POST',
            url         : url,
            data        : formData,
            dataType    : "JSON",
            async       : true,
            cache       : false,
            contentType : false,
            processData : false,
            success: function (result) {
                return resolve(result);
            },
            complete: function() {

            },
            error: function (jqXHR) {
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR['responseJSON'].message,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-primary" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                return resolve(false);
            }
        });
    });
}


const getListAttribute = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/filter-generate-promo",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    distributorList = result['data']['distributor'];
                    brandList = result['data']['brand'];
                    categoryList = result['data']['category'];
                    subAccountList = result['data']['subAccount'];
                    channelList = result['data']['channel'];

                    for (let i=0; i<result['data']['subActivity'].length; i++) {
                        if (result['data']['subActivity'][i]['categoryDesc'] === "Retailer Cost") {
                            subActivityRcList.push(result['data']['subActivity'][i]);
                        } else {
                            subActivityDcList.push(result['data']['subActivity'][i]);
                        }
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

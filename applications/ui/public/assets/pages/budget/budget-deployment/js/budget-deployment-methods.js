'use strict';

let dialerObject;
let swalTitle = 'Budget Deployment';
let elSelectChannel = $('#filter_channel');
let elSelectGroupBrand = $('#filter_group_brand');
let elSelectSubActivityType = $('#filter_sub_activity_type');
let elDtBudgetDeploymentDetail = $('#dt_budget_deployment_detail');
let dt_budget_deployment_detail;
let sumAmount = 0, arrChecked = [];
heightContainer = 300;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        min: 2025,
        step: 1,
    });

    dt_budget_deployment_detail = elDtBudgetDeploymentDetail.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-6'i><'col-sm-6'p>>",
        processing: true,
        serverSide: false,
        paging: false,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[1, 'ASC']],
        columnDefs: [
            {
                targets: 0,
                data: 'Id',
                width: 25,
                searchable: false,
                orderable: false,
                className: 'text-nowrap dt-body-start text-center',
                checkboxes: {
                    'selectRow': false
                },
            },
            {
                targets: 1,
                title: 'Promo ID',
                data: 'promoId',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Brand',
                data: 'groupBrand',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Channel',
                data: 'channel',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Sub Channel',
                data: 'subChannel',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Account',
                data: 'account',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Sub Account',
                data: 'subAccount',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Activity',
                data: 'activity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Sub Activity',
                data: 'subActivity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: 'Activity Name',
                data: 'activityName',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 10,
                title: 'Promo Start',
                data: 'promoStart',
                className: 'text-nowrap align-middle',
                render: function (data) {
                    if (data) {
                        return formatDateOptima(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 11,
                title: 'Promo End',
                data: 'promoEnd',
                className: 'text-nowrap align-middle',
                render: function (data) {
                    if (data) {
                        return formatDateOptima(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 12,
                title: 'Cost',
                data: 'investment',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return formatMoney(0, 0);
                    }
                }
            },
        ],
        initComplete: function () {
            $('#dt_budget_deployment_detail_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_budget_deployment_detail.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        arrChecked.push({
                            id: data[i].id,
                            investment: parseFloat(data[i].investment)
                        });
                    }
                } else {
                    arrChecked = [];
                }
                sumAmount = 0;
                if (arrChecked.length > 0) {
                    for (let i = 0; i < arrChecked.length; i++) {
                        sumAmount += parseFloat(arrChecked[i].investment);
                    }
                }
                $('#tot').html(formatMoney(sumAmount.toString(), 0));
                $('#count').html(arrChecked.length);
            });
        },
        drawCallback: function () {

        },
    });

    $('.dt-footer').html("<div>Total Selected : <span id='count'>0</span>&nbsp&nbsp&nbsp Total Amount : <span id='tot'>0</span></div>");

    getList($('#filter_period').val(), elSelectChannel.val(), elSelectGroupBrand.val(), elSelectSubActivityType.val()).then(function (result) {
        dt_budget_deployment_detail.rows.add(result['budgetDeploymentDetail']).draw();
    });
    getListFilter();


    elDtBudgetDeploymentDetail.on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_budget_deployment_detail.row(this.closest('tr')).data();
        let investment = rows.investment;
        if (this.checked) {
            arrChecked.push({
                id: rows.id,
                investment: parseFloat(rows.investment)
            });
            sumAmount += parseFloat(investment);
        } else {
            sumAmount -= parseFloat(investment);
            let index = arrChecked.findIndex(p => p.id === rows.id);
            if (index > -1) {
                arrChecked.splice(index, 1);
            }
        }
        $('#tot').html(formatMoney(sumAmount.toString(), 0));
        $('#count').html(arrChecked.length);
    });
});

$('#dt_budget_deployment_detail_search').on('keyup', function() {
    dt_budget_deployment_detail.search(this.value).draw();
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2025) {
        $(this).val("2025");
        dialerObject.setValue(2025);
    }
});

$('#btn_view').on('click', async function () {
    let btn = document.getElementById('btn_view');
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify(elSelectChannel.val());
    let filter_group_brand = JSON.stringify(elSelectGroupBrand.val());
    let filter_sub_activity_type = JSON.stringify(elSelectSubActivityType.val());

    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_budget_deployment_detail.clear().draw();
    let data = await getList(filter_period, filter_channel, filter_group_brand, filter_sub_activity_type);
    if (data) {
        dt_budget_deployment_detail.rows.add(data['budgetDeploymentDetail']).draw();
    }
    btn.setAttribute("data-kt-indicator", "off");
    btn.disabled = !1;
});

$('#btn_export_excel').on('click', function () {
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify(elSelectChannel.val());
    let filter_group_brand = JSON.stringify(elSelectGroupBrand.val());
    let filter_sub_activity_type = JSON.stringify(elSelectSubActivityType.val());
    window.open(`/budget/deployment/download-excel?period=${filter_period}&channelId=${filter_channel}&groupBrand=${filter_group_brand}&subActivityType=${filter_sub_activity_type}`, '_blank');
});

$('#btn_deploy').on('click', async function () {
    let checkedDetails = dt_budget_deployment_detail.column(0).checkboxes.selected();
    let dataRows = [];
    if (checkedDetails.length === 0) {
        return Swal.fire({
            title: swalTitle,
            text: "Please select one or more items",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }

    Swal.fire({
        title: swalTitle,
        text: `${checkedDetails.length} selected Promo ID will be deployed. Confirm?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#AAAAAA',
        allowOutsideClick: false,
        allowEscapeKey: false,
        cancelButtonText: 'No, cancel',
        confirmButtonText: 'Yes',
        reverseButtons: true
    }).then((result) => {
        if(result.isConfirmed){
            $.each(checkedDetails, function (index, value) {
                dataRows.push(value);
            });
            let btn = document.getElementById('btn_deploy');
            let formData = new FormData();
            formData.append('promoId', JSON.stringify(dataRows));
            let url = '/budget/deployment/deploy';
            btn.setAttribute("data-kt-indicator", "on");
            btn.disabled = !0;
            $.ajax({
                url: url,
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                // enctype: "multipart/form-data",
                beforeSend: function () {
                },
                success: function (result) {
                    if (!result.error) {
                        Swal.fire({
                            title: swalTitle,
                            html: 'Deploy Success! <br/> Send email will be processing on new tab',
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            if (result['batchId']) {
                                dt_budget_deployment_detail.clear().draw();
                                $('#tot').html(0);
                                $('#count').html(0);

                                let handle = window.open('/budget/deployment/process?i=' + result['batchId']);
                                handle.blur();
                                window.focus();
                            }
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "OK",
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                },
                complete: function () {
                    btn.setAttribute("data-kt-indicator", "off");
                    btn.disabled = !1;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown)
                    Swal.fire({
                        title: swalTitle,
                        text: "Files Upload Failed",
                        icon: "error",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        }
    });
});

const getList = (pPeriod, pChannel, pGroupBrand, pSubActivityType) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/budget/deployment/list",
            type: "GET",
            dataType: 'json',
            data: {
                period: pPeriod,
                channelId: pChannel,
                groupBrand: pGroupBrand,
                subActivityType: pSubActivityType
            },
            async: true,
            success: function (result) {
                if (!result['error']) {
                    return resolve(result['data']);
                } else {
                    return resolve(null);
                }
            },
            complete: function () {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                return reject(null);
            }
        });
    });
}

const getListFilter = () => {
    $.ajax({
        url         : "/budget/deployment/list/filter",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            if (!result['error']) {
                let data = result['data'];
                let channelList = data['channel'];
                let groupBrandList = data['grpBrand'];
                let subActivityType = data['subactivityType'];

                //<editor-fold desc="Dropdown Channel">
                let dataChannelList = [];
                for (let i = 0, len = channelList.length; i < len; ++i) {
                    dataChannelList.push({
                        id: channelList[i]['id'],
                        text: channelList[i]['longDesc']
                    });
                }

                $.fn.select2.amd.require([
                    'select2/selection/single',
                    'select2/selection/placeholder',
                    'select2/selection/allowClear',
                    'select2/dropdown',
                    'select2/dropdown/search',
                    'select2/dropdown/attachBody',
                    'select2/utils'
                ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                    let SelectionAdapter = Utils.Decorate(
                        SingleSelection,
                        Placeholder
                    );

                    SelectionAdapter = Utils.Decorate(
                        SelectionAdapter,
                        AllowClear
                    );

                    let DropdownAdapter = Utils.Decorate(
                        Utils.Decorate(
                            Dropdown,
                            DropdownSearch
                        ),
                        AttachBody
                    );
                    elSelectChannel.select2({
                        placeholder: 'Select a Channel',
                        selectionAdapter: SelectionAdapter,
                        dropdownAdapter: DropdownAdapter,
                        allowClear: true,
                        templateResult: function (data) {
                            if (!data.id) {
                                return data.text;
                            }

                            let $res = $('<div></div>');

                            $res.text(data.text);
                            $res.addClass('wrap');

                            return $res;
                        },
                        templateSelection: function (data) {
                            if (!data.id) {
                                return data.text;
                            }
                            let selected = (elSelectChannel.val() || []).length;
                            let total = $('option', elSelectChannel).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: dataChannelList
                    });

                    $('[aria-controls="select2-filter_channel-container"]').addClass('form-select form-select-sm');
                });
                //</editor-fold>

                //<editor-fold desc="Dropdown Group Brand">
                let dataGroupBrandList = [];
                for (let i = 0, len = groupBrandList.length; i < len; ++i) {
                    dataGroupBrandList.push({
                        id: groupBrandList[i]['groupBrandId'],
                        text: groupBrandList[i]['groupBrand']
                    });
                }
                $.fn.select2.amd.require([
                    'select2/selection/single',
                    'select2/selection/placeholder',
                    'select2/selection/allowClear',
                    'select2/dropdown',
                    'select2/dropdown/search',
                    'select2/dropdown/attachBody',
                    'select2/utils'
                ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                    let SelectionAdapter = Utils.Decorate(
                        SingleSelection,
                        Placeholder
                    );

                    SelectionAdapter = Utils.Decorate(
                        SelectionAdapter,
                        AllowClear
                    );

                    let DropdownAdapter = Utils.Decorate(
                        Utils.Decorate(
                            Dropdown,
                            DropdownSearch
                        ),
                        AttachBody
                    );
                    elSelectGroupBrand.select2({
                        placeholder: 'Select a Brand',
                        selectionAdapter: SelectionAdapter,
                        dropdownAdapter: DropdownAdapter,
                        allowClear: true,
                        templateResult: function (data) {
                            if (!data.id) {
                                return data.text;
                            }

                            let $res = $('<div></div>');

                            $res.text(data.text);
                            $res.addClass('wrap');

                            return $res;
                        },
                        templateSelection: function (data) {
                            if (!data.id) {
                                return data.text;
                            }
                            let selected = (elSelectGroupBrand.val() || []).length;
                            let total = $('option', elSelectGroupBrand).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: dataGroupBrandList
                    });

                    $('[aria-controls="select2-filter_group_brand-container"]').addClass('form-select form-select-sm');
                });
                //</editor-fold>

                //<editor-fold desc="Dropdown Sub Activity Type">
                let dataSubActivityTypeList = [];
                for (let i = 0, len = subActivityType.length; i < len; ++i) {
                    dataSubActivityTypeList.push({
                        id: subActivityType[i]['id'],
                        text: subActivityType[i]['subActivityType']
                    });
                }
                $.fn.select2.amd.require([
                    'select2/selection/single',
                    'select2/selection/placeholder',
                    'select2/selection/allowClear',
                    'select2/dropdown',
                    'select2/dropdown/search',
                    'select2/dropdown/attachBody',
                    'select2/utils'
                ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                    let SelectionAdapter = Utils.Decorate(
                        SingleSelection,
                        Placeholder
                    );

                    SelectionAdapter = Utils.Decorate(
                        SelectionAdapter,
                        AllowClear
                    );

                    let DropdownAdapter = Utils.Decorate(
                        Utils.Decorate(
                            Dropdown,
                            DropdownSearch
                        ),
                        AttachBody
                    );
                    elSelectSubActivityType.select2({
                        placeholder: 'Select a Sub Activity Type',
                        selectionAdapter: SelectionAdapter,
                        dropdownAdapter: DropdownAdapter,
                        allowClear: true,
                        templateResult: function (data) {
                            if (!data.id) {
                                return data.text;
                            }

                            let $res = $('<div></div>');

                            $res.text(data.text);
                            $res.addClass('wrap');

                            return $res;
                        },
                        templateSelection: function (data) {
                            if (!data.id) {
                                return data.text;
                            }
                            let selected = (elSelectSubActivityType.val() || []).length;
                            let total = $('option', elSelectSubActivityType).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: dataSubActivityTypeList
                    });

                    $('[aria-controls="select2-filter_sub_activity_type-container"]').addClass('form-select form-select-sm');
                });
                //</editor-fold>
            }
        },
        complete: function() {

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(errorThrown);
        }
    });
}

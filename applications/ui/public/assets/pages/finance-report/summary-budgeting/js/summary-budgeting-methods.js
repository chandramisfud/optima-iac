'use strict';

let dt_summary_budgeting;
let swalTitle = "Summary Budgeting";
heightContainer = 325;
let dataFilter, url_datatable;
let dialerObject;
let elFilterPeriod = $('#filter_period');
let elFilterCategory = $('#filter_category');
let elFilterGroupBrand = $('#filter_group_brand');
let elFilterChannel = $('#filter_channel');

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    if (localStorage.getItem('financeSummaryBudgetingState')) {
        dataFilter = JSON.parse(localStorage.getItem('financeSummaryBudgetingState'));

        url_datatable = `/fin-rpt/summary-budget/list/paginate/filter?period=${dataFilter.period}&categoryId=${(dataFilter.category ?? "")}&groupBrand=${(dataFilter.groupBrand ?? [])}&channel=${(dataFilter.channel ?? [])}`;
    } else {
        url_datatable = '/fin-rpt/summary-budget/list/paginate/filter?period=' + elFilterPeriod.val();
    }

    getListFilter().then(function (result) {
        if (!result['error']) {
            let category = result['data']['category'];
            let groupBrandList = result['data']['grpBrand'];
            let channelList = result['data']['channel'];

            if (dataFilter) {
                elFilterPeriod.val(dataFilter.period);
                dialerObject.setValue(parseInt(dataFilter.period));
            } else {
                elFilterPeriod.val(new Date().getFullYear());
                dialerObject.setValue(new Date().getFullYear());
            }

            //<editor-fold desc="Dropdown Category">
            let categoryDropdown = [{id:'', text:''}];
            for (let i = 0; i < category.length; i++) {
                categoryDropdown.push({
                    id: category[i]['id'],
                    text: category[i]['longDesc']
                });
            }
            $('#filter_category').select2({
                placeholder: "Select a Category",
                width: '100%',
                data: categoryDropdown
            });
            if (dataFilter) elFilterCategory.val(dataFilter.category).trigger('change.select2');
            //</editor-fold>

            //<editor-fold desc="Dropdown Group Brand">
            let dataGroupBrandList = [];
            if (dataFilter) {
                let dataFilterGroupBrand = (dataFilter.hasOwnProperty('groupBrand') ? dataFilter.groupBrand : []);
                for (let i = 0, len = groupBrandList.length; i < len; ++i) {
                    if (dataFilterGroupBrand.length > 0) {
                        let selected = false;
                        for (let j=0; j<dataFilterGroupBrand.length; j++) {
                            if (groupBrandList[i]['groupBrandId'] === parseInt(dataFilterGroupBrand[j])) {
                                selected = true;
                            }
                        }
                        dataGroupBrandList.push({
                            id: groupBrandList[i]['groupBrandId'],
                            text: groupBrandList[i]['groupBrandDesc'],
                            selected: selected
                        });
                    } else {
                        dataGroupBrandList.push({
                            id: groupBrandList[i]['groupBrandId'],
                            text: groupBrandList[i]['groupBrandDesc']
                        });
                    }
                }
            } else {
                for (let i = 0, len = groupBrandList.length; i < len; ++i) {
                    dataGroupBrandList.push({
                        id: groupBrandList[i]['groupBrandId'],
                        text: groupBrandList[i]['groupBrandDesc']
                    });
                }
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
                elFilterGroupBrand.select2({
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
                        let selected = (elFilterGroupBrand.val() || []).length;
                        let total = $('option', elFilterGroupBrand).length;
                        return "Selected " + selected + " of " + total;
                    },
                    data: dataGroupBrandList
                });

                $('[aria-controls="select2-filter_group_brand-container"]').addClass('form-select form-select-sm');
            });
            //</editor-fold>

            //<editor-fold desc="Dropdown Channel">
            let dataChannelList = [];
            if (dataFilter) {
                let dataFilterChannel = (dataFilter.hasOwnProperty('channel') ? dataFilter.channel : []);
                for (let i = 0, len = channelList.length; i < len; ++i) {
                    if (dataFilterChannel.length > 0) {
                        let selected = false;
                        for (let j=0; j<dataFilterChannel.length; j++) {
                            if (channelList[i]['channelId'] === parseInt(dataFilterChannel[j])) {
                                selected = true;
                            }
                        }
                        dataChannelList.push({
                            id: channelList[i]['channelId'],
                            text: channelList[i]['channelDesc'],
                            selected: selected
                        });
                    } else {
                        dataChannelList.push({
                            id: channelList[i]['channelId'],
                            text: channelList[i]['channelDesc']
                        });
                    }
                }
            } else {
                for (let i = 0, len = channelList.length; i < len; ++i) {
                    dataChannelList.push({
                        id: channelList[i]['channelId'],
                        text: channelList[i]['channelDesc']
                    });
                }
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
                elFilterChannel.select2({
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
                        let selected = (elFilterChannel.val() || []).length;
                        let total = $('option', elFilterChannel).length;
                        return "Selected " + selected + " of " + total;
                    },
                    data: dataChannelList
                });

                $('[aria-controls="select2-filter_channel-container"]').addClass('form-select form-select-sm');
            });
            //</editor-fold>
        }
    });

    dt_summary_budgeting = $('#dt_summary_budgeting').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        saveState: true,
        processing: true,
        serverSide: true,
        paging: true,
        ordering: false,
        searching: true,
        scrollX: true,
        autoWidth: false,
        fixedColumns: {
            left: 5,
        },
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Brand',
                data: 'brand',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 1,
                title: 'Channel',
                data: 'channel',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'Sub Channel',
                data: 'subchannel',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'Account',
                data: 'account',
                className: 'align-middle text-nowrap'
            },
            {
                targets: 4,
                title: 'Sub Account',
                data: 'subaccount',
                className: 'align-middle text-nowrap'
            },
            {
                targets: 5,
                title: 'SS Volume (tons)',
                data: 'ssVolumeTons',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 6,
                title: 'PS Volume (tons)',
                data: 'psVolumeTons',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 7,
                title: 'SS',
                data: 'ss',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 8,
                title: 'PS',
                data: 'ps',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 9,
                title: 'KPI',
                data: 'kpi',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 10,
                title: 'KPI %',
                data: 'kpiPct',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 11,
                title: 'RGA',
                data: 'rga',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 12,
                title: 'RGA %',
                data: 'rgaPct',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 13,
                title: 'Transport',
                data: 'transport',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 14,
                title: 'Transport %',
                data: 'transportPct',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 15,
                title: 'Other Cost',
                data: 'otherCost',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 16,
                title: 'Other Cost (%)',
                data: 'otherCostPct',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 17,
                title: 'Total Distributor Cost',
                data: 'totalDistributorCost',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 18,
                title: 'Total Distributor Cost (%)',
                data: 'totalDistributorCostPct',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 19,
                title: 'TT',
                data: 'tt',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 20,
                title: '% TT to SS',
                data: 'pctTtToSs',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 21,
                title: '% TT to PS',
                data: 'pctTtToPs',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 22,
                title: 'Adhoc',
                data: 'adhoc',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 23,
                title: '% Adhoc to SS',
                data: 'pctAdhocToSs',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 24,
                title: '% Adhoc to PS',
                data: 'pctAdhocToPs',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 25,
                title: 'TT + Adhoc',
                data: 'ttPlusAdhoc',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 26,
                title: '% TT + Adhoc to SS',
                data: 'pctTtPlusAdhocTotSs',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 27,
                title: '% TT + Adhoc to PS',
                data: 'pctTtPlusAdhocTotPs',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 28,
                title: 'Total Trade Spend',
                data: 'totalTradeSpend',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 29,
                title: '% to PS',
                data: 'tradeSpendPctToPs',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 30,
                title: 'Warchest',
                data: 'warChest',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 31,
                title: '% to PS',
                data: 'warChestPctToPs',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
            {
                targets: 32,
                title: 'Total TS',
                data: 'totalTS',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 33,
                title: '% to PS',
                data: 'pctToPs',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    return formatMoney(data, 2);
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $('#dt_summary_budgeting_search').on('keyup', function () {
        dt_summary_budgeting.search(this.value, false, false).draw();
    });

});

$('#dt_summary_budgeting_view').on('click', function (){
    let btn = document.getElementById('dt_summary_budgeting_view');
    let filter_period = (elFilterPeriod.val()) ?? "";
    let filter_category = (elFilterCategory.val()) ?? "";
    let filter_group_brand = JSON.stringify(elFilterGroupBrand.val());
    let filter_channel = JSON.stringify(elFilterChannel.val());

    let url = `/fin-rpt/summary-budget/list/paginate/filter?period=${filter_period}&categoryId=${filter_category}&groupBrand=${filter_group_brand}&channel=${filter_channel}`;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_summary_budgeting.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
        let data_filter = {
            period: filter_period,
            category: filter_category,
            groupBrand: JSON.parse(filter_group_brand),
            channel: JSON.parse(filter_channel)
        };

        localStorage.setItem('financeSummaryBudgetingState', JSON.stringify(data_filter));
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let filter_period = (elFilterPeriod.val()) ?? "";
    let filter_category = (elFilterCategory.val()) ?? "";
    let filter_group_brand = JSON.stringify(elFilterGroupBrand.val());
    let filter_channel = JSON.stringify(elFilterChannel.val());

    let url = `/fin-rpt/summary-budget/export-xls?period=${filter_period}&categoryId=${filter_category}&groupBrand=${filter_group_brand}&channel=${filter_channel}`;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const getListFilter = () => {
    return new Promise((resolve) => {
        $.ajax({
            url         : "/fin-rpt/summary-budget/list/filter",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                return resolve(result);
            },
            complete: function() {
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return resolve({error: true});
            }
        });
    }).catch((e) => {
        console.log(e);
        return 0;
    });
}

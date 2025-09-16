'use strict';

let dt_channel_summary;
let swalTitle = "Channel Summary";
heightContainer = 285;
let dataFilter, url_datatable;
let dialerObject;
let elFilterPeriod = $('#filter_period');
let elFilterCategory = $('#filter_category');
let elFilterGroupBrand = $('#filter_group_brand');
let elFilterChannel = $('#filter_channel');
let elFilterSubActivityType = $('#filter_subactivitytype');
let categoryDropdown = [];
$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    url_datatable = `/fin-rpt/tt-control/list/paginate/filter?period=${elFilterPeriod.val()}`;
    if (localStorage.getItem('financeChannelSummaryState')) {
        dataFilter = JSON.parse(localStorage.getItem('financeChannelSummaryState'));
        url_datatable = `/fin-rpt/tt-control/list/paginate/filter?period=${dataFilter.period}&categoryId=${JSON.stringify(dataFilter.category) ?? []}`;
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    getListFilter().then(function () {
        if (dataFilter) {
            elFilterPeriod.val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));
            elFilterCategory.val(dataFilter.category).trigger('change');
            dataFilter.category.forEach(id => {
                const found = categoryDropdown.find(item => item.id == id);
                if (found) {
                    const newOption = new Option(found.text, found.id, true, true);
                    elFilterCategory.append(newOption);
                }
            });
        } else {
            elFilterPeriod.val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_channel_summary = $('#dt_channel_summary').DataTable({
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
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Brand',
                data: 'brand',
                className: 'align-middle text-nowrap',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        return `<span class="fw-bolder">${data}</span>`;
                    } else {
                        return data;
                    }
                }
            },
            {
                targets: 1,
                title: 'Channel',
                data: 'channelDesc',
                className: 'align-middle text-nowrap',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        return `<span class="fw-bolder">${data}</span>`;
                    } else {
                        return data;
                    }
                }
            },
            {
                targets: 2,
                title: 'Sub Channel',
                data: 'subChannelDesc',
                className: 'align-middle text-nowrap',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        return `<span class="fw-bolder">${data}</span>`;
                    } else {
                        if (full['subTotalActivity']) {
                            return `<span class="fw-bolder">${data}</span>`;
                        } else {
                            return data;
                        }
                    }
                }
            },
            {
                targets: 3,
                title: 'Account',
                data: 'accountDesc',
                className: 'align-middle text-nowrap',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        return `<span class="fw-bolder">${data}</span>`;
                    } else {
                        if (full['subTotalActivity']) {
                            return `<span class="fw-bolder">${data}</span>`;
                        } else {
                            return data;
                        }
                    }
                }
            },
            {
                targets: 4,
                title: 'Sub Account',
                data: 'subAccountDesc',
                className: 'align-middle text-nowrap',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        return `<span class="fw-bolder">${data}</span>`;
                    } else {
                        if (full['subTotalActivity']) {
                            return `<span class="fw-bolder">${data}</span>`;
                        } else {
                            return data;
                        }
                    }
                }
            },
            {
                targets: 5,
                title: 'FY SS',
                data: 'fySs',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        if (data) {
                            return `<span class="fw-bolder">${formatMoney(data, 0)}</span>`;
                        } else {
                            return `<span class="fw-bolder">0</span>`;
                        }
                    } else {
                        if (data) {
                            return formatMoney(data, 0);
                        } else {
                            return "0";
                        }
                    }
                }
            },
            {
                targets: 6,
                title: 'Activity',
                data: 'activityDesc',
                className: 'align-middle text-nowrap',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        return `<span class="fw-bolder">${data}</span>`;
                    } else {
                        if (full['subTotalActivity']) {
                            return `<span class="fw-bolder">${data}</span>`;
                        } else {
                            return data;
                        }
                    }
                }
            },
            {
                targets: 7,
                title: 'Sub Activity',
                data: 'subActivityDesc',
                className: 'align-middle text-nowrap',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        return `<span class="fw-bolder">${data}</span>`;
                    } else {
                        if (full['subTotalActivity']) {
                            return `<span class="fw-bolder">${data}</span>`;
                        } else {
                            return data;
                        }
                    }
                }
            },
            {
                targets: 8,
                title: 'TT Rate',
                data: 'ttRate',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        if (data) {
                            return `<span class="fw-bolder">${formatMoney(data, 2)}</span>`;
                        } else {
                            return `<span class="fw-bolder">0</span>`;
                        }
                    } else {
                        if (data) {
                            return formatMoney(data, 2);
                        } else {
                            return "0";
                        }
                    }
                }
            },
            {
                targets: 9,
                title: 'FY Budget',
                data: 'fyTtBudget',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        if (data) {
                            return `<span class="fw-bolder">${formatMoney(data, 0)}</span>`;
                        } else {
                            return `<span class="fw-bolder">0</span>`;
                        }
                    } else {
                        if (data) {
                            return formatMoney(data, 0);
                        } else {
                            return "0";
                        }
                    }
                }
            },
            {
                targets: 10,
                title: 'FY Cost Submitted',
                data: 'fyTtCostSubmitted',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        if (data) {
                            return `<span class="fw-bolder">${formatMoney(data, 0)}</span>`;
                        } else {
                            return `<span class="fw-bolder">0</span>`;
                        }
                    } else {
                        if (data) {
                            return formatMoney(data, 0);
                        } else {
                            return "0";
                        }
                    }
                }
            },
            {
                targets: 11,
                title: 'Returned Balance From Closure',
                data: 'returnedBalanceFromClosure',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        if (data) {
                            return `<span class="fw-bolder">${formatMoney(data, 0)}</span>`;
                        } else {
                            return `<span class="fw-bolder">0</span>`;
                        }
                    } else {
                        if (data) {
                            return formatMoney(data, 0);
                        } else {
                            return "0";
                        }
                    }
                }
            },
            {
                targets: 12,
                title: 'Remaining Budget Include Closure',
                data: 'remainingBudgetIncludeClosure',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full) {
                    if (full['isSubTotal']) {
                        if (data) {
                            return `<span class="fw-bolder">${formatMoney(data, 0)}</span>`;
                        } else {
                            return `<span class="fw-bolder">0</span>`;
                        }
                    } else {
                        if (data) {
                            return formatMoney(data, 0);
                        } else {
                            return "0";
                        }
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
        createdRow: function (row, data) {
            if (data['isSubTotal']) {
                if (data['tBrand']) {
                    $('td', row).css('background-color', 'rgb(70, 159, 247) !important');
                } else if (data['tChannel']) {
                    $('td', row).css('background-color', 'rgb(89, 166, 243) !important');
                } else if (data['tSubAccount']) {
                    $('td', row).css('background-color', 'rgb(125, 186, 245) !important');
                } else if (data['tActivity']) {
                    $('td', row).css('background-color', 'rgb(154, 200, 245) !important');
                }
            }
        }
    });

    $('#dt_channel_summary_search').on('keyup', function () {
        dt_channel_summary.search(this.value, false, false).draw();
    });
});

$('#dt_channel_summary_view').on('click', function (){
    let btn = document.getElementById('dt_channel_summary_view');
    let filter_period = (elFilterPeriod.val()) ?? "";
    let filter_category = JSON.stringify(elFilterCategory.val());
    let filter_group_brand = JSON.stringify(elFilterGroupBrand.val());
    let filter_channel = JSON.stringify(elFilterChannel.val());
    let filter_subactivity_type = JSON.stringify(elFilterSubActivityType.val());

    let url = `/fin-rpt/tt-control/list/paginate/filter?period=${filter_period}&categoryId=${filter_category}&groupBrandId=${filter_group_brand}&channelId=${filter_channel}&subActivityTypeId=${filter_subactivity_type}`;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_channel_summary.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
        let data_filter = {
            period: filter_period,
            category: elFilterCategory.val()
        };

        localStorage.setItem('financeChannelSummaryState', JSON.stringify(data_filter));
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let filter_period = (elFilterPeriod.val()) ?? "";
    let filter_category = JSON.stringify(elFilterCategory.val());
    let filter_group_brand = JSON.stringify(elFilterGroupBrand.val());
    let filter_channel = JSON.stringify(elFilterChannel.val());
    let filter_subactivity_type = JSON.stringify(elFilterSubActivityType.val());

    let selectedCategory = $('#filter_category').select2('data').map(item => item.text);
    let categoryText = selectedCategory.join(', ');

    let selectedSubActivityType = $('#filter_subactivitytype').select2('data').map(item => item.text);
    let subActivityTypeText = selectedSubActivityType.join(', ');

    let url = `/fin-rpt/tt-control/export-xls?period=${filter_period}&categoryId=${filter_category}&groupBrandId=${filter_group_brand}&channelId=${filter_channel}&subActivityTypeId=${filter_subactivity_type}&categoryText=${categoryText}&subActivityTypeText=${subActivityTypeText}`;

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
            url         : "/fin-rpt/tt-control/list/filter",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: async function(result) {
                if (!result['error']) {
                    let category = result['data']['category'];
                    let groupBrand = result['data']['grpBrand'];
                    let channel = result['data']['channel'];
                    let subActivityType = result['data']['subActivityType'];

                    //<editor-fold desc="Dropdown Category">
                    for (let i = 0; i < category.length; i++) {
                        categoryDropdown.push({
                            id: category[i]['id'],
                            text: category[i]['longDesc']
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
                        elFilterCategory.select2({
                            placeholder: 'Select Category',
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
                                let selected = (elFilterCategory.val() || []).length;
                                let total = $('option', elFilterCategory).length;
                                return "Selected " + selected + " of " + total;
                            },
                            data: categoryDropdown
                        });
                        $('[aria-controls="select2-filter_category-container"]').addClass('form-select form-select-sm');
                        // elFilterCategory.val(categoryDefault).trigger('change.select2');
                    });
                    //</editor-fold>

                    //<editor-fold desc="Dropdown Group Brand">
                    let groupBrandDropdown = [];
                    for (let i = 0; i < groupBrand.length; i++) {
                        groupBrandDropdown.push({
                            id: groupBrand[i]['groupBrandId'],
                            text: groupBrand[i]['groupBrandDesc']
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
                        elFilterGroupBrand.select2({
                            placeholder: 'Select Brands',
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
                            data: groupBrandDropdown
                        });

                        $('[aria-controls="select2-filter_group_brand-container"]').addClass('form-select form-select-sm');
                    });
                    //</editor-fold>

                    //<editor-fold desc="Dropdown Channel">
                    let channelDropdown = [];
                    for (let i = 0; i < channel.length; i++) {
                        channelDropdown.push({
                            id: channel[i]['channelId'],
                            text: channel[i]['channelDesc']
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
                        elFilterChannel.select2({
                            placeholder: 'Select Channels',
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
                            data: channelDropdown
                        });

                        $('[aria-controls="select2-filter_channel-container"]').addClass('form-select form-select-sm');
                    });
                    //</editor-fold>

                    //<editor-fold desc="Dropdown Sub Activity Type">
                    let subActivityTypeDropdown = [];
                    for (let i = 0; i < subActivityType.length; i++) {
                        subActivityTypeDropdown.push({
                            id: subActivityType[i]['subActivityTypeId'],
                            text: subActivityType[i]['subActivityTypeDesc']
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
                        elFilterSubActivityType.select2({
                            placeholder: 'Select Sub Activity Type',
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
                                let selected = (elFilterSubActivityType.val() || []).length;
                                let total = $('option', elFilterSubActivityType).length;
                                return "Selected " + selected + " of " + total;
                            },
                            data: subActivityTypeDropdown
                        });

                        $('[aria-controls="select2-filter_subactivitytype-container"]').addClass('form-select form-select-sm');
                    });
                    //</editor-fold>
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
        return 0;
    });
}

'use strict';

let validator, dt_budget_tt_console, dialerObject, arrChecked = [], sumAmount = 0, checkFlag = [], filterCount = 0;
let swalTitle = "Budget [TT Consol]";
let elDtBudgetTTConsole = $('#dt_budget_tt_console');
heightContainer = 280;
let elPeriod = $('#filter_period');
let elSubChannel = $('#filter_subchannel');
let elAccount = $('#filter_account');
let elSubAccount = $('#filter_subaccount');
let elSubCategory = $('#filter_subcategory');
let elSubActivityType = $('#filter_subactivity_type');
let elSubActivity = $('#filter_subactivity');
let elActivity = $('#filter_activity');
let elCategory = $('#filter_category');
let elChannel = $('#filter_channel');
let elDistributor = $('#filter_distributor');
let elGroupBrand = $('#filter_groupbrand');
let dataFilter, url_datatable;

$(document).ready(async function () {
    $('#form-upload').hide();
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

    url_datatable = '/budget/tt-console/list/paginate/filter?period=' + elPeriod.val();
    if (localStorage.getItem('ttConsoleState')) {
        dataFilter = JSON.parse(localStorage.getItem('ttConsoleState'));

        url_datatable = `/budget/tt-console/list/paginate/filter?period=${dataFilter.period ?? 2025}
            &category=${JSON.stringify(dataFilter.category) ?? []}
            &subActivityType=${JSON.stringify(dataFilter.subActivityType) ?? []}
            &subCategory=${JSON.stringify(dataFilter.subCategory) ?? []}
            &activity=${JSON.stringify(dataFilter.activity) ?? []}
            &subActivity=${JSON.stringify(dataFilter.subActivity) ?? []}
            &channel=${JSON.stringify(dataFilter.channel) ?? []}
            &subChannel=${JSON.stringify(dataFilter.subChannel) ?? []}
            &account=${JSON.stringify(dataFilter.account) ?? []}
            &subAccount=${JSON.stringify(dataFilter.subAccount) ?? []}
            &distributor=${JSON.stringify(dataFilter.distributor) ?? []}
            &groupBrand=${JSON.stringify(dataFilter.groupBrand) ?? []}
            `;

        filterCount = 0;
        if (dataFilter.period !== ''){ filterCount ++; }
        if (dataFilter.category) {if (dataFilter.category.length > 0) { filterCount ++; }}
        if (dataFilter.subActivityType) {if (dataFilter.subActivityType.length > 0) { filterCount ++; }}
        if (dataFilter.subCategory) {if (dataFilter.subCategory.length > 0) { filterCount ++; }}
        if (dataFilter.activity) {if (dataFilter.activity.length > 0) { filterCount ++; }}
        if (dataFilter.subActivity) {if (dataFilter.subActivity.length > 0) { filterCount ++; }}
        if (dataFilter.channel) {if (dataFilter.channel.length > 0) { filterCount ++; }}
        if (dataFilter.subChannel) {if (dataFilter.subChannel.length > 0) { filterCount ++; }}
        if (dataFilter.account) {if (dataFilter.account.length > 0) { filterCount ++; }}
        if (dataFilter.subAccount) {if (dataFilter.subAccount.length > 0) { filterCount ++; }}
        if (dataFilter.distributor) {if (dataFilter.distributor.length > 0) { filterCount ++; }}
        if (dataFilter.groupBrand) {if (dataFilter.groupBrand.length > 0) { filterCount ++; }}

        $('#filter_count_badge').html(filterCount);
    }

    if (dataFilter) {
        elPeriod.val(dataFilter.period);
        dialerObject.setValue(parseInt(dataFilter.period));
    } else {
        elPeriod.val('2025');
        dialerObject.setValue(2025);
    }

    dt_budget_tt_console = elDtBudgetTTConsole.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[0, 'ASC']],
        columnDefs: [
            {
                targets: 0,
                data: 'Id',
                width: 10,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function () {
                    return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-125px w-md-125px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start edit-record" href="#"><i class="fa fa-edit fs-6"></i> Edit Data</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'Period',
                data: 'period',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Category',
                data: 'category',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Sub Category',
                data: 'subCategory',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Channel',
                data: 'channel',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Sub Channel',
                data: 'subChannel',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Account',
                data: 'account',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Sub Account',
                data: 'subAccount',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Distributor',
                data: 'distributor',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: 'Distributor Short Desc',
                data: 'distributorShortDesc',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 10,
                title: 'Brand',
                data: 'groupbrand',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 11,
                title: 'Sub Activity Type',
                data: 'subActivityType',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 12,
                title: 'Sub Activity',
                data: 'subActivity',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 13,
                title: 'Activity',
                data: 'activity',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 14,
                title: 'TT % (in %)',
                data: 'tt',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data, 2) + '%';
                }
            },
            {
                targets: 15,
                title: 'Budget Name',
                data: 'budgetName',
                width: 100,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function () {

        },
        drawCallback: function () {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        console.log(e.jqXHR);
        let strMessage = e.jqXHR['responseJSON'].message
        if (strMessage === "") strMessage = "Please contact your vendor"
        Swal.fire({
            text: strMessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,

        });
    };

    //<editor-fold desc="Dropdown The start where it will be loaded first">
    await getListCategory();
    await getListChannel();
    getListGroupBrand();
    getListDistributor();
    if (dataFilter) {
        // await loadDefaultUrlDatatable();
        await loadDropdownFilter();
    }

    //</editor-fold>
    //<editor-fold desc="Initial Dropdown Sub Activity Type">
    $("#dt_budget_tt_console").on('click', '.edit-record', function () {
        let tr = this.closest("tr");
        let trData = dt_budget_tt_console.row(tr).data();

        let url = '/budget/tt-console/form?method=update' + '&ic=' + trData.categoryId + '&i=' + trData.Id + '&c=' + trData['categoryEnc'];
        checkFormAccess('update_rec', '', url, '')
    });
});

elPeriod.on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2025) {
        $(this).val("2025");
        dialerObject.setValue(2025);
    }
});

elCategory.on('change', async function () {
    let values = $(this).val();

    //Delete hierarch state
    await updateLocalStorage('category');
    await updateLocalStorage('subCategory');
    await updateLocalStorage('subActivityType');
    await updateLocalStorage('activity');
    await updateLocalStorage('subActivity');
    elSubCategory.empty();
    elSubActivityType.empty();
    elActivity.empty();
    if (values.length > 0) {
        await getListSubActivityType(JSON.stringify($(this).val())).then(async function () {
            await elSubActivityType.val('').trigger('change');
        });
    }
});

elSubActivityType.on('change', async function () {
    let values = $(this).val();

    //Delete hierarch state
    await updateLocalStorage('subCategory');
    elSubCategory.empty();
    if (values.length > 0) await getListSubCategory(JSON.stringify(elCategory.val()), JSON.stringify($(this).val()));
    elSubCategory.val('').trigger('change');
});

elSubCategory.on('change', async function () {
    let values = $(this).val();

    //Delete hierarch state
    await updateLocalStorage('activity');
    elActivity.empty();
    if (values.length > 0) await getListActivity(JSON.stringify(elCategory.val()), JSON.stringify(elSubActivityType.val()), JSON.stringify($(this).val()));
    elActivity.val('').trigger('change');
});

elActivity.on('change', async function () {
    let values = $(this).val();

    //Delete hierarch state
    await updateLocalStorage('subActivity');
    elSubActivity.empty();
    if (values.length > 0) await getListSubActivity(JSON.stringify(elCategory.val()), JSON.stringify(elSubActivityType.val()), JSON.stringify($(this).val()));
    elSubActivity.val('').trigger('change');
});

elChannel.on('change', async function () {
    let values = $(this).val();

    //Delete hierarch state
    await updateLocalStorage('channel');
    await updateLocalStorage('subChannel');
    elSubChannel.empty();
    if (values.length > 0) await getListSubChannel(JSON.stringify($(this).val()));
    elSubChannel.val('').trigger('change');
});

elSubChannel.on('change', async function () {
    let values = $(this).val();

    //Delete hierarch state
    await updateLocalStorage('account');
    elAccount.empty();
    if (values.length > 0) await getListAccount(JSON.stringify($(this).val()));
    elAccount.val('').trigger('change');
});

elAccount.on('change', async function () {
    let values = $(this).val();

    //Delete hierarch state
    await updateLocalStorage('subAccount');
    elSubAccount.empty();
    if (values.length > 0) await getListSubAccount(JSON.stringify($(this).val()));
    elSubAccount.val('').trigger('change');
});

$('.record-create').on('click', function () {
    let c = $(this).attr('data-category')
    let ic = $(this).attr('data-category-id')
    checkFormAccess('create_rec', '', '/budget/tt-console/form?ic=' + ic + '&c=' + c, '')
});

$('#dt_budget_tt_console_search').on('keyup', function () {
    dt_budget_tt_console.search(this.value).draw();
});

$('#btn_filter').on('click', function () {
    let btn = document.getElementById('btn_filter');
    let btn_reset = document.getElementById('btn_reset_filter');
    let filter_period = elPeriod.val();
    let filter_category = JSON.stringify(elCategory.val());
    let filter_subcategory = JSON.stringify(elSubCategory.val());
    let filter_channel = JSON.stringify(elChannel.val());
    let filter_subchannel = JSON.stringify(elSubChannel.val());
    let filter_account = JSON.stringify(elAccount.val());
    let filter_subaccount = JSON.stringify(elSubAccount.val());
    let filter_distributor = JSON.stringify(elDistributor.val());
    let filter_groupbrand = JSON.stringify(elGroupBrand.val());
    let filter_subactivity_type = JSON.stringify(elSubActivityType.val());
    let filter_subactivity = JSON.stringify(elSubActivity.val());
    let filter_activity = JSON.stringify(elActivity.val());

    let url = "/budget/tt-console/list/paginate/filter?period=" + filter_period + "&category=" + filter_category +
        "&subActivityType=" + filter_subactivity_type +
        "&subCategory=" + filter_subcategory +
        "&activity=" + filter_activity +
        "&subActivity=" + filter_subactivity +
        "&channel=" + filter_channel +
        "&subChannel=" + filter_subchannel +
        "&account=" + filter_account +
        "&subAccount=" + filter_subaccount +
        "&distributor=" + filter_distributor +
        "&groupBrand=" + filter_groupbrand;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    btn_reset.disabled = !0;

    //Count Filtered
    filterCount = 0;
    if (elPeriod.val() !== ''){ filterCount ++; }
    if (elCategory.val().length !== 0){ filterCount ++; }
    if (elSubCategory.val().length !== 0){ filterCount ++; }
    if (elChannel.val().length !== 0){ filterCount ++; }
    if (elSubChannel.val().length !== 0){ filterCount ++; }
    if (elAccount.val().length !== 0){ filterCount ++; }
    if (elSubAccount.val().length !== 0){ filterCount ++; }
    if (elDistributor.val().length !== 0){ filterCount ++; }
    if (elGroupBrand.val().length !== 0){ filterCount ++; }
    if (elSubActivityType.val().length !== 0){ filterCount ++; }
    if (elSubActivity.val().length !== 0){ filterCount ++; }
    if (elActivity.val().length !== 0){ filterCount ++; }

    $('#filter_count_badge').html(filterCount);

    dt_budget_tt_console.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn_reset.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
        btn_reset.disabled = !1;

        let data_filter = {
            period: filter_period,
            distributor: JSON.parse(filter_distributor),
            groupBrand: JSON.parse(filter_groupbrand),
            category: JSON.parse(filter_category),
            subCategory: JSON.parse(filter_subcategory),
            subActivityType: JSON.parse(filter_subactivity_type),
            activity: JSON.parse(filter_activity),
            subActivity: JSON.parse(filter_subactivity),
            channel: JSON.parse(filter_channel),
            subChannel: JSON.parse(filter_subchannel),
            account: JSON.parse(filter_account),
            subAccount: JSON.parse(filter_subaccount)
        };

        localStorage.setItem('ttConsoleState', JSON.stringify(data_filter));
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn_reset.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
        btn_reset.disabled = !1;
    });
    $('#filter_close').trigger('click');
});

const getListCategory = () => {
    return new Promise(async (resolve) => {
        $.ajax({
            url: "/budget/tt-console/list/category",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                let data = [];
                let category = result.data;
                if (dataFilter) {
                    let dataFilterCategory = (dataFilter.hasOwnProperty('category') ? dataFilter.category : []);
                    for (let i = 0, len = category.length; i < len; ++i) {
                        if (dataFilterCategory.length > 0) {
                            let selected = false;
                            for (let j = 0; j < dataFilterCategory.length; j++) {
                                if (category[i]['id'] === parseInt(dataFilterCategory[j])) {
                                    selected = true;
                                }
                            }
                            data.push({
                                id: category[i]['id'],
                                text: category[i]['longDesc'],
                                selected: selected
                            });
                        } else {
                            data.push({
                                id: category[i]['id'],
                                text: category[i]['longDesc']
                            });
                        }
                    }
                } else {
                    for (let i = 0, len = category.length; i < len; ++i) {
                        data.push({
                            id: category[i]['id'],
                            text: category[i]['longDesc']
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
                    elCategory.select2({
                        placeholder: 'Select a Category',
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
                            let selected = (elCategory.val() || []).length;
                            let total = $('option', elCategory).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_category-container"]').addClass('form-select form-select-sm');
                });
            },
            complete: function () {
                resolve();
            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
            }
        });
    });
}

const getListSubActivityType = (categoryId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/budget/tt-console/list/sub-activity-type/category-id",
            type: "GET",
            dataType: 'json',
            data: {categoryId: categoryId},
            async: true,
            success: function (result) {
                let data = [];
                let subActivityType = result.data;
                if (dataFilter) {
                    let dataFilterSubActivityType = (dataFilter.hasOwnProperty('subActivityType') ? dataFilter.subActivityType : []);
                    for (let i = 0, len = subActivityType.length; i < len; ++i) {
                        if (dataFilterSubActivityType.length > 0) {
                            let selected = false;
                            for (let j = 0; j < dataFilterSubActivityType.length; j++) {
                                if (subActivityType[i]['id'] === parseInt(dataFilterSubActivityType[j])) {
                                    selected = true;
                                }
                            }
                            data.push({
                                id: subActivityType[i]['id'],
                                text: subActivityType[i]['longDesc'],
                                selected: selected
                            });
                        } else {
                            data.push({
                                id: subActivityType[i]['id'],
                                text: subActivityType[i]['longDesc']
                            });
                        }
                    }
                } else {
                    for (let i = 0, len = subActivityType.length; i < len; ++i) {
                        data.push({
                            id: subActivityType[i]['id'],
                            text: subActivityType[i]['longDesc']
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
                    elSubActivityType.select2({
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
                            let selected = (elSubActivityType.val() || []).length;
                            let total = $('option', elSubActivityType).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_subactivity_type-container"]').addClass('form-select form-select-sm');
                });

                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return resolve();
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListSubCategory = (categoryId, subActivityTypeId) => {
    $.ajax({
        url: "/budget/tt-console/list/sub-category/category-id",
        type: "GET",
        dataType: 'json',
        data: {categoryId: categoryId, subActivityTypeId: subActivityTypeId},
        async: true,
        success: function (result) {
            let data = [];
            let subCategory = result.data;
            if (dataFilter) {
                let dataFilterSubCategory = (dataFilter.hasOwnProperty('subCategory') ? dataFilter.subCategory : []);
                for (let i = 0, len = subCategory.length; i < len; ++i) {
                    if (subCategory.length > 0) {
                        let selected = false;
                        for (let j = 0; j < dataFilterSubCategory.length; j++) {
                            if (subCategory[i]['id'] === parseInt(dataFilterSubCategory[j])) {
                                selected = true;
                            }
                        }
                        data.push({
                            id: subCategory[i]['id'],
                            text: subCategory[i]['longDesc'],
                            selected: selected
                        });
                    } else {
                        data.push({
                            id: subCategory[i]['id'],
                            text: subCategory[i]['longDesc']
                        });
                    }
                }
            } else {
                for (let i = 0, len = subCategory.length; i < len; ++i) {
                    data.push({
                        id: subCategory[i]['id'],
                        text: subCategory[i]['longDesc']
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
                elSubCategory.select2({
                    placeholder: 'Select a Sub Category',
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
                        let selected = (elSubCategory.val() || []).length;
                        let total = $('option', elSubCategory).length;
                        return "Selected " + selected + " of " + total;
                    },
                    data: data
                });

                $('[aria-controls="select2-filter_subcategory-container"]').addClass('form-select form-select-sm');
            });
        },
        complete: function () {

        },
        error: function (jqXHR) {
            console.log(jqXHR.responseText);
        }
    });
}

const getListActivity = (categoryId, subActivityTypeId, subCategoryId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/budget/tt-console/list/activity/category-id",
            type: "GET",
            dataType: 'json',
            data: {categoryId: categoryId, subActivityTypeId: subActivityTypeId, subCategoryId: subCategoryId},
            async: true,
            success: function (result) {
                let data = [];
                let activity = result.data;
                if (dataFilter) {
                    let dataFilterActivity = (dataFilter.hasOwnProperty('activity') ? dataFilter.activity : []);
                    for (let i = 0, len = activity.length; i < len; ++i) {
                        if (activity.length > 0) {
                            let selected = false;
                            for (let j = 0; j < dataFilterActivity.length; j++) {
                                if (activity[i]['id'] === parseInt(dataFilterActivity[j])) {
                                    selected = true;
                                }
                            }
                            data.push({
                                id: activity[i]['id'],
                                text: activity[i]['longDesc'],
                                selected: selected
                            });
                        } else {
                            data.push({
                                id: activity[i]['id'],
                                text: activity[i]['longDesc']
                            });
                        }
                    }
                } else {
                    for (let i = 0, len = activity.length; i < len; ++i) {
                        data.push({
                            id: activity[i]['id'],
                            text: activity[i]['longDesc']
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
                    elActivity.select2({
                        placeholder: 'Select an Activity',
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
                            let selected = (elActivity.val() || []).length;
                            let total = $('option', elActivity).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_activity-container"]').addClass('form-select form-select-sm');
                });

                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return resolve();
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListSubActivity = (categoryId, subActivityTypeId, activityId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/budget/tt-console/list/sub-activity/category-id-activity-id",
            type: "GET",
            dataType: 'json',
            data: {categoryId: categoryId, subActivityTypeId: subActivityTypeId, activityId: activityId},
            async: true,
            success: function (result) {
                let data = [];
                let subActivity = result.data;
                if (dataFilter) {
                    let dataFilterSubActivity = (dataFilter.hasOwnProperty('subActivity') ? dataFilter.subActivity : []);
                    for (let i = 0, len = subActivity.length; i < len; ++i) {
                        if (subActivity.length > 0) {
                            let selected = false;
                            for (let j = 0; j < dataFilterSubActivity.length; j++) {
                                if (subActivity[i]['id'] === parseInt(dataFilterSubActivity[j])) {
                                    selected = true;
                                }
                            }
                            data.push({
                                id: subActivity[i]['id'],
                                text: subActivity[i]['longDesc'],
                                selected: selected
                            });
                        } else {
                            data.push({
                                id: subActivity[i]['id'],
                                text: subActivity[i]['longDesc']
                            });
                        }
                    }
                } else {
                    for (let i = 0, len = subActivity.length; i < len; ++i) {
                        data.push({
                            id: subActivity[i]['id'],
                            text: subActivity[i]['longDesc']
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
                    elSubActivity.select2({
                        placeholder: 'Select a Sub Activity',
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
                            let selected = (elSubActivity.val() || []).length;
                            let total = $('option', elSubActivity).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_subactivity-container"]').addClass('form-select form-select-sm');
                });

                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return resolve();
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListChannel = () => {
    return new Promise(async (resolve) => {
        $.ajax({
            url: "/budget/ss-input/conversion-rate/list/channel",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                let data = [];
                let channel = result.data;
                if (dataFilter) {
                    let dataFilterChannel = (dataFilter.hasOwnProperty('channel') ? dataFilter.channel : []);

                    for (let i = 0, len = channel.length; i < len; ++i) {
                        if (dataFilterChannel.length > 0) {
                            let selected = false;
                            for (let j = 0; j < dataFilterChannel.length; j++) {
                                if (channel[i]['id'] === parseInt(dataFilterChannel[j])) {
                                    selected = true;
                                }
                            }
                            data.push({
                                id: channel[i]['id'],
                                text: channel[i]['longDesc'],
                                selected: selected
                            });
                        } else {
                            data.push({
                                id: channel[i]['id'],
                                text: channel[i]['longDesc']
                            });
                        }
                    }
                } else {
                    for (let i = 0, len = channel.length; i < len; ++i) {
                        data.push({
                            id: channel[i]['id'],
                            text: channel[i]['longDesc']
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
                    elChannel.select2({
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
                            let selected = (elChannel.val() || []).length;
                            let total = $('option', elChannel).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_channel-container"]').addClass('form-select form-select-sm');
                });
            },
            complete: function () {
                resolve();
            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
            }
        });
    });
}

const getListSubChannel = (channelId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/budget/tt-console/list/sub-channel/channel-id",
            type: "GET",
            dataType: 'json',
            data: {channelId: channelId},
            async: true,
            success: function (result) {
                let data = [];
                let subChannel = result.data;
                if (dataFilter) {
                    let dataFilterSubChannel = (dataFilter.hasOwnProperty('subChannel') ? dataFilter.subChannel : []);
                    for (let i = 0, len = subChannel.length; i < len; ++i) {
                        if (dataFilterSubChannel.length > 0) {
                            let selected = false;
                            for (let j = 0; j < dataFilterSubChannel.length; j++) {
                                if (subChannel[i]['id'] === parseInt(dataFilterSubChannel[j])) {
                                    selected = true;
                                }
                            }
                            data.push({
                                id: subChannel[i]['id'],
                                text: subChannel[i]['longDesc'],
                                selected: selected
                            });
                        } else {
                            data.push({
                                id: subChannel[i]['id'],
                                text: subChannel[i]['longDesc']
                            });
                        }
                    }
                } else {
                    for (let i = 0, len = subChannel.length; i < len; ++i) {
                        data.push({
                            id: subChannel[i]['id'],
                            text: subChannel[i]['longDesc']
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
                    elSubChannel.select2({
                        placeholder: 'Select a Sub Channel',
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
                            let selected = (elSubChannel.val() || []).length;
                            let total = $('option', elSubChannel).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_subchannel-container"]').addClass('form-select form-select-sm');
                });


                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return resolve();
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListAccount = (subChannelId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/budget/tt-console/list/account/sub-channel-id",
            type: "GET",
            dataType: 'json',
            data: {subChannelId: subChannelId},
            async: true,
            success: function (result) {
                let data = [];
                let account = result.data;
                if (dataFilter) {
                    let dataFilterAccount = (dataFilter.hasOwnProperty('account') ? dataFilter.account : []);
                    for (let i = 0, len = account.length; i < len; ++i) {
                        if (dataFilterAccount.length > 0) {
                            let selected = false;
                            for (let j = 0; j < dataFilterAccount.length; j++) {
                                if (account[i]['id'] === parseInt(dataFilterAccount[j])) {
                                    selected = true;
                                }
                            }
                            data.push({
                                id: account[i]['id'],
                                text: account[i]['longDesc'],
                                selected: selected
                            });
                        } else {
                            data.push({
                                id: account[i]['id'],
                                text: account[i]['longDesc']
                            });
                        }
                    }
                } else {
                    for (let i = 0, len = account.length; i < len; ++i) {
                        data.push({
                            id: account[i]['id'],
                            text: account[i]['longDesc']
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
                    elAccount.select2({
                        placeholder: 'Select an Account',
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
                            let selected = (elAccount.val() || []).length;
                            let total = $('option', elAccount).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_account-container"]').addClass('form-select form-select-sm');
                });

                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return resolve();
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListSubAccount = (accountId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/budget/tt-console/list/sub-account/account-id",
            type: "GET",
            dataType: 'json',
            data: {accountId: accountId},
            async: true,
            success: function (result) {
                let data = [];
                let subAccount = result.data;
                if (dataFilter) {
                    let dataFilterSubAccount = (dataFilter.hasOwnProperty('subAccount') ? dataFilter.subAccount : []);
                    for (let i = 0, len = subAccount.length; i < len; ++i) {
                        if (dataFilterSubAccount.length > 0) {
                            let selected = false;
                            for (let j = 0; j < dataFilterSubAccount.length; j++) {
                                if (subAccount[i]['id'] === parseInt(dataFilterSubAccount[j])) {
                                    selected = true;
                                }
                            }
                            data.push({
                                id: subAccount[i]['id'],
                                text: subAccount[i]['longDesc'],
                                selected: selected
                            });
                        } else {
                            data.push({
                                id: subAccount[i]['id'],
                                text: subAccount[i]['longDesc']
                            });
                        }
                    }
                } else {
                    for (let i = 0, len = subAccount.length; i < len; ++i) {
                        data.push({
                            id: subAccount[i]['id'],
                            text: subAccount[i]['longDesc']
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
                    elSubAccount.select2({
                        placeholder: 'Select a Sub Account',
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
                            let selected = (elSubAccount.val() || []).length;
                            let total = $('option', elSubAccount).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_subaccount-container"]').addClass('form-select form-select-sm');
                });

                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return resolve();
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListDistributor = () => {
    $.ajax({
        url: "/budget/tt-console/list/distributor",
        type: "GET",
        dataType: 'json',
        async: true,
        success: function (result) {
            let data = [];
            let distributor = result.data;
            if (dataFilter) {
                let dataFilterDistributor = (dataFilter.hasOwnProperty('distributor') ? dataFilter.distributor : []);
                for (let i = 0, len = distributor.length; i < len; ++i) {
                    if (distributor.length > 0) {
                        let selected = false;
                        for (let j = 0; j < dataFilterDistributor.length; j++) {
                            if (distributor[i]['id'] === parseInt(dataFilterDistributor[j])) {
                                selected = true;
                            }
                        }
                        data.push({
                            id: distributor[i]['id'],
                            text: distributor[i]['longDesc'],
                            selected: selected
                        });
                    } else {
                        data.push({
                            id: distributor[i]['id'],
                            text: distributor[i]['longDesc']
                        });
                    }
                }
            } else {
                for (let i = 0, len = distributor.length; i < len; ++i) {
                    data.push({
                        id: distributor[i]['id'],
                        text: distributor[i]['longDesc']
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
                elDistributor.select2({
                    placeholder: 'Select a Distributor',
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
                        let selected = (elDistributor.val() || []).length;
                        let total = $('option', elDistributor).length;
                        return "Selected " + selected + " of " + total;
                    },
                    data: data
                });

                $('[aria-controls="select2-filter_distributor-container"]').addClass('form-select form-select-sm');
            });
        },
        complete: function () {

        },
        error: function (jqXHR) {
            console.log(jqXHR.responseText);
        }
    });
}

const getListGroupBrand = () => {
    $.ajax({
        url: "/budget/tt-console/list/groupBrand",
        type: "GET",
        dataType: 'json',
        async: true,
        success: function (result) {
            let data = [];
            let groupBrand = result.data;

            if (dataFilter) {
                let dataFilterGroupBrand = (dataFilter.hasOwnProperty('groupBrand') ? dataFilter.groupBrand : []);
                for (let i = 0, len = groupBrand.length; i < len; ++i) {
                    if (groupBrand.length > 0) {
                        let selected = false;
                        for (let j = 0; j < dataFilterGroupBrand.length; j++) {
                            if (groupBrand[i]['Id'] === parseInt(dataFilterGroupBrand[j])) {
                                selected = true;
                            }
                        }
                        data.push({
                            id: groupBrand[i]['Id'],
                            text: groupBrand[i]['LongDesc'],
                            selected: selected
                        });
                    } else {
                        data.push({
                            id: groupBrand[i]['Id'],
                            text: groupBrand[i]['LongDesc']
                        });
                    }
                }
            } else {
                for (let i = 0, len = groupBrand.length; i < len; ++i) {
                    data.push({
                        id: groupBrand[i]['Id'],
                        text: groupBrand[i]['LongDesc']
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

                elGroupBrand.select2({
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
                        let selected = (elGroupBrand.val() || []).length;
                        let total = $('option', elGroupBrand).length;
                        return "Selected " + selected + " of " + total;
                    },
                    data: data
                });

                $('[aria-controls="select2-filter_groupbrand-container"]').addClass('form-select form-select-sm');
            });
        },
        complete: function () {

        },
        error: function (jqXHR) {
            console.log(jqXHR.responseText);
        }
    });
}

$('.export-excel').on('click', function () {
    let categoryType = $(this).attr('data-type');
    let categoryId = $(this).attr('data-category');

    let filter_period = elPeriod.val();
    let filter_category = JSON.stringify(categoryId);
    let filter_subcategory = JSON.stringify(elSubCategory.val());
    let filter_channel = JSON.stringify(elChannel.val());
    let filter_subchannel = JSON.stringify(elSubChannel.val());
    let filter_account = JSON.stringify(elAccount.val());
    let filter_subaccount = JSON.stringify(elSubAccount.val());
    let filter_distributor = JSON.stringify(elDistributor.val());
    let filter_groupbrand = JSON.stringify(elGroupBrand.val());
    let filter_subactivity_type = JSON.stringify(elSubActivityType.val());
    let filter_subactivity = JSON.stringify(elSubActivity.val());
    let filter_activity = JSON.stringify(elActivity.val());

    // Filter Sub Category
    let filter_subcategory_text = [];
    $.each($("#filter_subcategory option:selected"), function () {
        filter_subcategory_text.push($(this).text());
    });
    let text_subcategory = filter_subcategory_text.join(", ");

    // Filter Channel Get Text
    let filter_channel_text = [];
    $.each($("#filter_channel option:selected"), function () {
        filter_channel_text.push($(this).text());
    });
    let text_channel = filter_channel_text.join(", ");

    // Filter Sub Channel Get Text
    let filter_subchannel_text = [];
    $.each($("#filter_subchannel option:selected"), function () {
        filter_subchannel_text.push($(this).text());
    });
    let text_subchannel = filter_subchannel_text.join(", ");

    // Filter Account Get Text
    let filter_account_text = [];
    $.each($("#filter_account option:selected"), function () {
        filter_account_text.push($(this).text());
    });
    let text_account = filter_account_text.join(", ");

    // Filter Sub Account Get Text
    let filter_subaccount_text = [];
    $.each($("#filter_subaccount option:selected"), function () {
        filter_subaccount_text.push($(this).text());
    });
    let text_subaccount = filter_subaccount_text.join(", ");

    // Filter Distributor Get Text
    let filter_distributor_text = [];
    $.each($("#filter_distributor option:selected"), function () {
        filter_distributor_text.push($(this).text());
    });
    let text_distributor = filter_distributor_text.join(", ");

    // Filter Group Brand Get Text
    let filter_groupbrand_text = [];
    $.each($("#filter_groupbrand option:selected"), function () {
        filter_groupbrand_text.push($(this).text());
    });
    let text_groupbrand = filter_groupbrand_text.join(", ");

    // Filter Sub Activity Type Get Text
    let filter_subactivity_type_text = [];
    $.each($("#filter_subactivity_type option:selected"), function () {
        filter_subactivity_type_text.push($(this).text());
    });
    let text_subactivity_type = filter_subactivity_type_text.join(", ");

    // Filter Sub Activity Get Text
    let filter_subactivity_text = [];
    $.each($("#filter_subactivity option:selected"), function () {
        filter_subactivity_text.push($(this).text());
    });
    let text_subactivity = filter_subactivity_text.join(", ");

    // Filter Activity Get Text
    let filter_activity_text = [];
    $.each($("#filter_activity option:selected"), function () {
        filter_activity_text.push($(this).text());
    });
    let text_activity = filter_activity_text.join(", ");


    let a = document.createElement("a");
    a.href = "/budget/tt-console/export-xls?period=" + filter_period + "&channel=" + filter_channel + "&channelText=" + text_channel +
        "&category=" + filter_category + "&categoryText=" + $(this).text() +
        "&subCategory=" + filter_subcategory + "&subCategoryText=" + text_subcategory +
        "&subChannel=" + filter_subchannel + "&channelText=" + text_subchannel +
        "&account=" + filter_account + "&accountText=" + text_account +
        "&subAccount=" + filter_subaccount + "&subAccountText=" + text_subaccount +
        "&distributor=" + filter_distributor + "&distributorText=" + text_distributor +
        "&groupBrand=" + filter_groupbrand + "&groupBrandText=" + text_groupbrand +
        "&subActivityType=" + filter_subactivity_type + "&subActivityTypeText=" + text_subactivity_type +
        "&activity=" + filter_activity + "&subActivityTypeText=" + text_activity +
        "&subActivity=" + filter_subactivity + "&subActivityText=" + text_subactivity +
        "&typeDataText=" + categoryType;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('.download-template').on('click', function () {
    let category = $(this).attr('data-shortdesc');
    let categoryId = $(this).attr('data-category');

    let filter_period = elPeriod.val();
    let filter_category = JSON.stringify(categoryId);
    let filter_subcategory = JSON.stringify(elSubCategory.val());
    let filter_channel = JSON.stringify(elChannel.val());
    let filter_subchannel = JSON.stringify(elSubChannel.val());
    let filter_account = JSON.stringify(elAccount.val());
    let filter_subaccount = JSON.stringify(elSubAccount.val());
    let filter_distributor = JSON.stringify(elDistributor.val());
    let filter_groupbrand = JSON.stringify(elGroupBrand.val());
    let filter_subactivity_type = JSON.stringify(elSubActivityType.val());
    let filter_subactivity = JSON.stringify(elSubActivity.val());
    let filter_activity = JSON.stringify(elActivity.val());

    let a = document.createElement("a");
    if (category === 'RC') {
        a.href = "/budget/tt-console/download-template-rc?period=" + filter_period + "&category=" + filter_category + "&subCategory=" + filter_subcategory + "&channel=" + filter_channel + "&subChannel=" + filter_subchannel + "&account=" + filter_account +
            "&subAccount=" + filter_subaccount + "&distributor=" + filter_distributor + "&groupBrand=" + filter_groupbrand + "&subActivityType=" + filter_subactivity_type + "&subActivity=" + filter_subactivity + "&activity=" + filter_activity;
    } else {
        a.href = "/budget/tt-console/download-template-dc?period=" + filter_period + "&category=" + filter_category + "&subCategory=" + filter_subcategory + "&channel=" + filter_channel +
            "&distributor=" + filter_distributor + "&groupBrand=" + filter_groupbrand + "&subActivityType=" + filter_subactivity_type + "&subActivity=" + filter_subactivity + "&activity=" + filter_activity;
    }

    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('.upload-template').on('click', function () {
    let category = $(this).attr('data-shortdesc');
    if (category === 'RC') {
        window.location.href = "/budget/tt-console/upload-form-rc";
    } else {
        window.location.href = "/budget/tt-console/upload-form-dc";
    }
});

$('#btn_reset_filter').on('click', async function () {
    $('#form_budget_tt_console_filter')[0].reset();
    await elPeriod.val('2025');
    await dialerObject.setValue(2025);
    await elCategory.val('').trigger('change');
    await elChannel.val('').trigger('change');
    await elDistributor.val('').trigger('change');
    await elGroupBrand.val('').trigger('change');

    await elAccount.val('').trigger('change');
    await elSubAccount.val('').trigger('change');
    await elSubActivityType.val('').trigger('change');
    await elActivity.val('').trigger('change');
    await elSubActivity.val('').trigger('change');

    await dt_budget_tt_console.ajax.url('/budget/tt-console/list/paginate/filter?period=' + elPeriod.val()).load();
    await $('#filter_count_badge').html('0');
    await $('#filter_close').trigger('click');
    await localStorage.removeItem('ttConsoleState');
});

const loadDropdownFilter = () => {
    return new Promise(async (resolve) => {
        //<editor-fold desc="Load Filter Sub Activity Type by Category From dataFilter">
        let categoriesSelected = [];
        if (dataFilter.hasOwnProperty('category')) {
            categoriesSelected = dataFilter.category;
            await getListSubActivityType(JSON.stringify(categoriesSelected));
        }
        //</editor-fold>

        //<editor-fold desc="Load Filter Sub Channel by channel From dataFilter">
        let channelSelected = [];
        if (dataFilter.hasOwnProperty('channel')) {
            channelSelected = dataFilter.channel;
            await getListSubChannel(JSON.stringify(channelSelected));
        }
        //</editor-fold>

        //<editor-fold desc="Load Filter Account by Sub Channel From dataFilter">
        let subChannelSelected = [];
        if (dataFilter.hasOwnProperty('subChannel')) {
            subChannelSelected = dataFilter.subChannel;
            await getListAccount(JSON.stringify(subChannelSelected));
        }
        //</editor-fold>

        //<editor-fold desc="Load Filter Sub Account by Account From dataFilter">
        let accountSelected = [];
        if (dataFilter.hasOwnProperty('subAccount')) {
            accountSelected = dataFilter.account;
            await getListSubAccount(JSON.stringify(accountSelected));
        }
        //</editor-fold>

        //<editor-fold desc="Load Filter Sub Category by Category & Sub Activity Type From dataFilter">
        let subActivityTypeSelected = [];
        if (dataFilter.hasOwnProperty('subActivityType')) {
            subActivityTypeSelected = dataFilter.subActivityType;
            await getListSubCategory(JSON.stringify(categoriesSelected), JSON.stringify(subActivityTypeSelected));
        }
        //</editor-fold>

        //<editor-fold desc="Load Filter Activity by Category, Sub Activity Type & Sub Category From dataFilter">
        let subCategorySelected = [];
        if (dataFilter.hasOwnProperty('subCategory')) {
            subCategorySelected = dataFilter.subCategory;
            await getListActivity(JSON.stringify(categoriesSelected), JSON.stringify(subActivityTypeSelected), JSON.stringify(subCategorySelected));
        }
        //</editor-fold>

        //<editor-fold desc="Load Filter Activity by Category, Sub Activity Type & Sub Category From dataFilter">
        let activitySelected = [];
        if (dataFilter.hasOwnProperty('activity')) {
            activitySelected = dataFilter.activity;
            await getListSubActivity(JSON.stringify(categoriesSelected), JSON.stringify(subActivityTypeSelected), JSON.stringify(activitySelected));
        }
        //</editor-fold>

        resolve();
    });
}

const updateLocalStorage = (param) => {
    let currentState = JSON.parse(localStorage.getItem('ttConsoleState')) || {};
    if (dataFilter) {
        let lengthParam = currentState[param].length;
        if (currentState.hasOwnProperty(param)) {
            if (lengthParam > 0) {
                currentState[param] = [];
            }
        }
    }

    localStorage.setItem('ttConsoleState', JSON.stringify(currentState));
}

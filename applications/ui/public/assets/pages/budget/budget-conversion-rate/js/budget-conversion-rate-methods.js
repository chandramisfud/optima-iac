'use strict';

let validator, dt_budget_conversion_rate, dialerObject, arrChecked = [], sumAmount = 0, checkFlag = [];
let swalTitle = "Budget [Conversion Rate]";
let elDtBudgetConversionRate = $('#dt_budget_conversion_rate');
heightContainer = 285;

$(document).ready(function () {
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

    getListChannel();
    getListGroupBrand();

    dt_budget_conversion_rate = elDtBudgetConversionRate.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/budget/ss-input/conversion-rate/list/paginate/filter?period=' + $('#filter_period').val() + '&channel=' + $('#filter_channel').val() + '&groupBrand=' + $('#filter_group_brand').val(),
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[1, 'ASC']],
        columnDefs: [
            {
                targets: 0,
                title: 'Period',
                data: 'period',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Channel',
                data: 'channel',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Sub Channel',
                data: 'subChannel',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Brand',
                data: 'groupBrand',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Jan',
                data: 'm1',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 5,
                title: 'Feb',
                data: 'm2',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 6,
                title: 'Mar',
                data: 'm3',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 7,
                title: 'Apr',
                data: 'm4',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 8,
                title: 'May',
                data: 'm5',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 9,
                title: 'Jun',
                data: 'm6',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 10,
                title: 'Jul',
                data: 'm7',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 11,
                title: 'Aug',
                data: 'm8',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 12,
                title: 'Sep',
                data: 'm9',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 13,
                title: 'Oct',
                data: 'm10',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 14,
                title: 'Nov',
                data: 'm11',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
            },
            {
                targets: 15,
                title: 'Dec',
                data: 'm12',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data,2);
                    } else {
                        return formatMoney(0,2);
                    }
                }
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
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#dt_budget_conversion_rate_search').on('keyup', function() {
    dt_budget_conversion_rate.search(this.value).draw();
});

$('#filter_channel').on('change', async function () {
    let elSubChannel = $('#filter_sub_channel');
    let values = $(this).val();
    elSubChannel.empty();
    if (values.length > 0) await getListSubChannel(JSON.stringify($(this).val()));
    elSubChannel.val('').trigger('change');
});

$('#dt_budget_conversion_rate_view').on('click', function (){
    let btn = document.getElementById('dt_budget_conversion_rate_view');
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify($('#filter_channel').val());
    let filter_sub_channel = JSON.stringify($('#filter_sub_channel').val());
    let filter_group_brand = JSON.stringify($('#filter_group_brand').val());

    let url = "/budget/ss-input/conversion-rate/list/paginate/filter?period=" + filter_period + "&channel=" + filter_channel + "&subChannel=" + filter_sub_channel + "&groupBrand=" + filter_group_brand;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_budget_conversion_rate.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

const getListChannel = () => {
    $.ajax({
        url         : "/budget/ss-input/conversion-rate/list/channel",
        type        : "GET",
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
            let elSelectChannel = $('#filter_channel');

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
                    data: data
                });

                $('[aria-controls="select2-filter_channel-container"]').addClass('form-select form-select-sm');
            });
        },
        complete: function() {

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(jqXHR.responseText);
        }
    });
}

const getListSubChannel = (channel) => {
    $.ajax({
        url         : "/budget/ss-input/conversion-rate/list/sub-channel",
        type        : "GET",
        dataType    : 'json',
        data        : {channel: channel},
        async       : true,
        success: function(result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j){
                data.push({
                    id: result.data[j]['subChannelId'],
                    text: result.data[j]['subChannelLongDesc']
                });
            }
            let elSelectSubChannel = $('#filter_sub_channel');

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
                elSelectSubChannel.select2({
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
                        let selected = (elSelectSubChannel.val() || []).length;
                        let total = $('option', elSelectSubChannel).length;
                        return "Selected " + selected + " of " + total;
                    },
                    data: data
                });

                $('[aria-controls="select2-filter_sub_channel-container"]').addClass('form-select form-select-sm');
            });
        },
        complete: function() {

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(jqXHR.responseText);
        }
    });
}

const getListGroupBrand = () => {
    $.ajax({
        url         : "/budget/ss-input/conversion-rate/list/groupBrand",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j){
                data.push({
                    id: result.data[j].Id,
                    text: result.data[j].LongDesc
                });
            }
            let elSelectGroupBrand = $('#filter_group_brand');

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
                    data: data
                });

                $('[aria-controls="select2-filter_group_brand-container"]').addClass('form-select form-select-sm');
            });
        },
        complete: function() {

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(jqXHR.responseText);
        }
    });
}

$('#btn_export_excel').on('click', function() {
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify($('#filter_channel').val());
    let filter_sub_channel = JSON.stringify($('#filter_sub_channel').val());
    let filter_group_brand = JSON.stringify($('#filter_group_brand').val());

    // Filter Channel Get Text
    let filter_channel_text= [];
    $.each($("#filter_channel option:selected"), function(){
        filter_channel_text.push($(this).text());
    });
    let text_channel = filter_channel_text.join(", ")

    // Filter Sub Channel Get Text
    let filter_sub_channel_text= [];
    $.each($("#filter_sub_channel option:selected"), function(){
        filter_sub_channel_text.push($(this).text());
    });
    let text_sub_channel = filter_sub_channel_text.join(", ")

    // Filter Group Brand Get Text
    let filter_group_brand_text= [];
    $.each($("#filter_group_brand option:selected"), function(){
        filter_group_brand_text.push($(this).text());
    });
    let text_group_brand = filter_group_brand_text.join(", ")

    let a = document.createElement("a");
    a.href = "/budget/ss-input/conversion-rate/export-xls?period=" + filter_period + "&channel=" + filter_channel + "&channelText=" + text_channel +
        "&subChannel=" + filter_sub_channel + "&subChannelText=" + text_sub_channel + "&groupBrand=" + filter_group_brand + "&groupBrandText=" + text_group_brand;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_csv').on('click', function() {
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify($('#filter_channel').val());
    let filter_sub_channel = JSON.stringify($('#filter_sub_channel').val());
    let filter_group_brand = JSON.stringify($('#filter_group_brand').val());

    // Filter Channel Get Text
    let filter_channel_text= [];
    $.each($("#filter_channel option:selected"), function(){
        filter_channel_text.push($(this).text());
    });
    let text_channel = filter_channel_text.join(", ")

    // Filter Sub Channel Get Text
    let filter_sub_channel_text= [];
    $.each($("#filter_sub_channel option:selected"), function(){
        filter_sub_channel_text.push($(this).text());
    });
    let text_sub_channel = filter_sub_channel_text.join(", ")

    // Filter Group Brand Get Text
    let filter_group_brand_text= [];
    $.each($("#filter_group_brand option:selected"), function(){
        filter_group_brand_text.push($(this).text());
    });
    let text_group_brand = filter_group_brand_text.join(", ")

    let a = document.createElement("a");
    a.href = "/budget/ss-input/conversion-rate/export-csv?period=" + filter_period + "&channel=" + filter_channel + "&channelText=" + text_channel +
        "&subChannel=" + filter_sub_channel + "&subChannelText=" + text_sub_channel + "&groupBrand=" + filter_group_brand + "&groupBrandText=" + text_group_brand;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn-download-template').on('click', function() {
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify($('#filter_channel').val());
    let filter_sub_channel = JSON.stringify($('#filter_sub_channel').val());
    let filter_group_brand = JSON.stringify($('#filter_group_brand').val());

    let a = document.createElement("a");
    a.href = "/budget/ss-input/conversion-rate/download-template?period=" + filter_period + "&channel=" + filter_channel + "&subChannel=" + filter_sub_channel + "&groupBrand=" + filter_group_brand;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn-upload').on('click', function() {
    window.location.href = "/budget/ss-input/conversion-rate/upload-form";
});

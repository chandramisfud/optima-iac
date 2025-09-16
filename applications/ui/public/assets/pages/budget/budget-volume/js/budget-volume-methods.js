'use strict';

let validator, dt_budget_volume, dialerObject, arrChecked = [], sumAmount = 0, checkFlag = [];
let swalTitle = "Budget [SS Input]";
let elDtBudgetVolume = $('#dt_budget_volume');
heightContainer = 355;
let elFilterSubChannel = $('#filter_subchannel');
let elFilterAccount = $('#filter_account');
let elFilterSubAccount = $('#filter_subaccount');

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

    //<editor-fold desc="Initial Dropdown Sub Channel">
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
        elFilterSubChannel.select2({
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
                let selected = (elFilterSubChannel.val() || []).length;
                let total = $('option', elFilterSubChannel).length;
                return "Selected " + selected + " of " + total;
            },
            data: [{id:'', text:''}]
        });

        $('[aria-controls="select2-filter_subchannel-container"]').addClass('form-select form-select-sm');
    });
    //</editor-fold>
    //<editor-fold desc="Initial Dropdown Account">
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
        elFilterAccount.select2({
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
                let selected = (elFilterAccount.val() || []).length;
                let total = $('option', elFilterAccount).length;
                return "Selected " + selected + " of " + total;
            },
            data: [{id:'', text:''}]
        });

        $('[aria-controls="select2-filter_account-container"]').addClass('form-select form-select-sm');
    });
    //</editor-fold>
    //<editor-fold desc="Initial Dropdown Sub Account">
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
        elFilterSubAccount.select2({
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
                let selected = (elFilterSubAccount.val() || []).length;
                let total = $('option', elFilterSubAccount).length;
                return "Selected " + selected + " of " + total;
            },
            data: [{id:'', text:''}]
        });

        $('[aria-controls="select2-filter_subaccount-container"]').addClass('form-select form-select-sm');
    });
    //</editor-fold>
    getListChannel();
    getListRegion();
    getListGroupBrand();

    dt_budget_volume = elDtBudgetVolume.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/budget/ss-input/volume/list/paginate/filter?period=' + $('#filter_period').val() + '&channel=' + $('#filter_channel').val() + '&subChannel=' + elFilterSubChannel.val() + '&account=' + elFilterAccount.val() + '&subAccount=' + elFilterSubAccount.val() + '&region=' + $('#filter_region').val() + '&groupBrand=' + $('#filter_groupbrand').val(),
            type: 'get',
        },
        fixedColumns: {
            left: 7,
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
                title: 'Account',
                data: 'account',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Sub Account',
                data: 'subAccount',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Region',
                data: 'region',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Brand',
                data: 'groupBrand',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
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
                targets: 8,
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
                targets: 9,
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
                targets: 10,
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
                targets: 11,
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
                targets: 12,
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
                targets: 13,
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
                targets: 14,
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
                targets: 15,
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
                targets: 16,
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
                targets: 17,
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
                targets: 18,
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
            {
                targets: 19,
                title: 'FY',
                data: 'fy',
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
                targets: 20,
                title: 'Jan',
                data: 'rate1',
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
                targets: 21,
                title: 'Feb',
                data: 'rate2',
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
                targets: 22,
                title: 'Mar',
                data: 'rate3',
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
                targets: 23,
                title: 'Apr',
                data: 'rate4',
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
                targets: 24,
                title: 'May',
                data: 'rate5',
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
                targets: 25,
                title: 'Jun',
                data: 'rate6',
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
                targets: 26,
                title: 'Jul',
                data: 'rate7',
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
                targets: 27,
                title: 'Aug',
                data: 'rate8',
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
                targets: 28,
                title: 'Sep',
                data: 'rate9',
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
                targets: 29,
                title: 'Oct',
                data: 'rate10',
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
                targets: 30,
                title: 'Nov',
                data: 'rate11',
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
                targets: 31,
                title: 'Dec',
                data: 'rate12',
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
                targets: 32,
                title: 'Jan',
                data: 'value1',
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
                targets: 33,
                title: 'Feb',
                data: 'value2',
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
                targets: 34,
                title: 'Mar',
                data: 'value3',
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
                targets: 35,
                title: 'Apr',
                data: 'value4',
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
                targets: 36,
                title: 'May',
                data: 'value5',
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
                targets: 37,
                title: 'Jun',
                data: 'value6',
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
                targets: 38,
                title: 'Jul',
                data: 'value7',
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
                targets: 39,
                title: 'Aug',
                data: 'value8',
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
                targets: 40,
                title: 'Sep',
                data: 'value9',
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
                targets: 41,
                title: 'Oct',
                data: 'value10',
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
                targets: 42,
                title: 'Nov',
                data: 'value11',
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
                targets: 43,
                title: 'Dec',
                data: 'value12',
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
                targets: 44,
                title: 'FY',
                data: 'valueFY',
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

$('#btn-upload').on('click', function() {
    window.location.href = "/budget/ss-input/volume/upload-form";
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#filter_channel').on('change', async function () {
    let values = $(this).val();
    elFilterSubChannel.empty();
    if (values.length > 0) await getListSubChannel(JSON.stringify($(this).val()));
    elFilterSubChannel.val('').trigger('change');
});

elFilterSubChannel.on('change', async function () {
    let values = $(this).val();

    elFilterAccount.empty();
    if (values.length > 0) await getListAccount(JSON.stringify($(this).val()));
    elFilterAccount.val('').trigger('change');
});

elFilterAccount.on('change', async function () {
    let values = $(this).val();

    elFilterSubAccount.empty();
    if (values.length > 0) await getListSubAccount(JSON.stringify($(this).val()));
    elFilterSubAccount.val('').trigger('change');
});

$('#dt_budget_volume_search').on('keyup', function() {
    dt_budget_volume.search(this.value).draw();
});

$('#dt_budget_volume_view').on('click', function (){
    let btn = document.getElementById('dt_budget_volume_view');
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify($('#filter_channel').val());
    let filter_subchannel = JSON.stringify(elFilterSubChannel.val());
    let filter_account = JSON.stringify(elFilterAccount.val());
    let filter_subaccount = JSON.stringify(elFilterSubAccount.val());
    let filter_region = JSON.stringify($('#filter_region').val());
    let filter_groupbrand = JSON.stringify($('#filter_groupbrand').val());

    let url = '/budget/ss-input/volume/list/paginate/filter?period=' + filter_period + '&channel=' + filter_channel + '&subChannel=' + filter_subchannel + '&account=' + filter_account + '&subAccount=' + filter_subaccount + '&region=' + filter_region + '&groupBrand=' + filter_groupbrand;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_budget_volume.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

const getListChannel = () => {
    $.ajax({
        url         : "/budget/ss-input/volume/list/channel",
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
            console.log(errorThrown);
        }
    });
}

const getListSubChannel = (channelId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/ss-input/volume/list/sub-channel/channel-id",
            type        : "GET",
            dataType    : 'json',
            data        : {channelId: channelId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
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
                    elFilterSubChannel.select2({
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
                            let selected = (elFilterSubChannel.val() || []).length;
                            let total = $('option', elFilterSubChannel).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_subchannel-container"]').addClass('form-select form-select-sm');
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject();
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListAccount = (subChannelId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/ss-input/volume/list/account/sub-channel-id",
            type        : "GET",
            dataType    : 'json',
            data        : {subChannelId: subChannelId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
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
                    elFilterAccount.select2({
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
                            let selected = (elFilterAccount.val() || []).length;
                            let total = $('option', elFilterAccount).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_account-container"]').addClass('form-select form-select-sm');
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject();
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListSubAccount = (accountId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/budget/ss-input/volume/list/sub-account/account-id",
            type        : "GET",
            dataType    : 'json',
            data        : {accountId: accountId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
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
                    elFilterSubAccount.select2({
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
                            let selected = (elFilterSubAccount.val() || []).length;
                            let total = $('option', elFilterSubAccount).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_subaccount-container"]').addClass('form-select form-select-sm');
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject();
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListRegion = () => {
    $.ajax({
        url         : "/budget/ss-input/volume/list/region",
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
            let elSelectRegion = $('#filter_region');

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
                elSelectRegion.select2({
                    placeholder: 'Select a Region',
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
                        let selected = (elSelectRegion.val() || []).length;
                        let total = $('option', elSelectRegion).length;
                        return "Selected " + selected + " of " + total;
                    },
                    data: data
                });

                $('[aria-controls="select2-filter_region-container"]').addClass('form-select form-select-sm');
            });
        },
        complete: function() {
            return resolve();
        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(errorThrown);
            return reject();
        }
    });
}

const getListGroupBrand = () => {
    $.ajax({
        url         : "/budget/ss-input/volume/list/groupBrand",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j){
                data.push({
                    id: result.data[j]['Id'],
                    text: result.data[j]['LongDesc']
                });
            }
            let elSelectGroupBrand = $('#filter_groupbrand');

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

                $('[aria-controls="select2-filter_groupbrand-container"]').addClass('form-select form-select-sm');
            });
        },
        complete: function() {

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(errorThrown);
        }
    });
}

$('#btn-download-template').on('click', function() {
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify($('#filter_channel').val());
    let filter_subchannel = JSON.stringify(elFilterSubChannel.val());
    let filter_account = JSON.stringify(elFilterAccount.val());
    let filter_subaccount = JSON.stringify(elFilterSubAccount.val());
    let filter_groupbrand = JSON.stringify($('#filter_groupbrand').val());

    let a = document.createElement("a");
    a.href = '/budget/ss-input/volume/download-template?period=' + filter_period + '&channel=' + filter_channel + '&subChannel=' + filter_subchannel + '&account=' + filter_account + '&subAccount=' + filter_subaccount + '&groupBrand=' + filter_groupbrand;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_excel').on('click', function() {
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify($('#filter_channel').val());
    let filter_subchannel = JSON.stringify(elFilterSubChannel.val());
    let filter_account = JSON.stringify(elFilterAccount.val());
    let filter_subaccount = JSON.stringify(elFilterSubAccount.val());
    let filter_region = JSON.stringify($('#filter_region').val());
    let filter_groupbrand = JSON.stringify($('#filter_groupbrand').val());

    // Filter Channel Get Text
    let filter_channel_text= [];
    $.each($("#filter_channel option:selected"), function(){
        filter_channel_text.push($(this).text());
    });
    let text_channel = filter_channel_text.join(", ");

    // Filter Sub Channel Get Text
    let filter_subchannel_text= [];
    $.each($("#filter_subchannel option:selected"), function(){
        filter_subchannel_text.push($(this).text());
    });
    let text_subchannel = filter_subchannel_text.join(", ");

    // Filter Account Get Text
    let filter_account_text= [];
    $.each($("#filter_account option:selected"), function(){
        filter_account_text.push($(this).text());
    });
    let text_account = filter_account_text.join(", ");

    // Filter Sub Account Get Text
    let filter_subaccount_text= [];
    $.each($("#filter_subaccount option:selected"), function(){
        filter_subaccount_text.push($(this).text());
    });
    let text_subaccount = filter_subaccount_text.join(", ")

    // Filter Region Get Text
    let filter_region_text= [];
    $.each($("#filter_region option:selected"), function(){
        filter_region_text.push($(this).text());
    });
    let text_region = filter_region_text.join(", ");

    // Filter Group Brand Get Text
    let filter_group_brand_text= [];
    $.each($("#filter_group_brand option:selected"), function(){
        filter_group_brand_text.push($(this).text());
    });
    let text_group_brand = filter_group_brand_text.join(", ");

    let a = document.createElement("a");
    a.href = '/budget/ss-input/volume/export-xls?period=' + filter_period + '&channel=' + filter_channel + '&channelText=' + text_channel + '&subChannel=' + filter_subchannel + '&subChannelText=' + text_subchannel + '&account=' + filter_account + '&accountText=' + text_account + '&subAccount=' + filter_subaccount + '&subAccountText=' + text_subaccount + '&groupBrand=' + filter_groupbrand + '&groupBrandText=' + text_group_brand + '&region=' + filter_region + '&regionText=' + text_region;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_csv').on('click', function() {
    let filter_period = $('#filter_period').val();
    let filter_channel = JSON.stringify($('#filter_channel').val());
    let filter_subchannel = JSON.stringify(elFilterSubChannel.val());
    let filter_account = JSON.stringify(elFilterAccount.val());
    let filter_subaccount = JSON.stringify(elFilterSubAccount.val());
    let filter_region = JSON.stringify($('#filter_region').val());
    let filter_groupbrand = JSON.stringify($('#filter_groupbrand').val());

    // Filter Channel Get Text
    let filter_channel_text= [];
    $.each($("#filter_channel option:selected"), function(){
        filter_channel_text.push($(this).text());
    });
    let text_channel = filter_channel_text.join(", ");

    // Filter Sub Channel Get Text
    let filter_subchannel_text= [];
    $.each($("#filter_subchannel option:selected"), function(){
        filter_subchannel_text.push($(this).text());
    });
    let text_subchannel = filter_subchannel_text.join(", ");

    // Filter Account Get Text
    let filter_account_text= [];
    $.each($("#filter_account option:selected"), function(){
        filter_account_text.push($(this).text());
    });
    let text_account = filter_account_text.join(", ");

    // Filter Sub Account Get Text
    let filter_subaccount_text= [];
    $.each($("#filter_subaccount option:selected"), function(){
        filter_subaccount_text.push($(this).text());
    });
    let text_subaccount = filter_subaccount_text.join(", ")

    // Filter Region Get Text
    let filter_region_text= [];
    $.each($("#filter_region option:selected"), function(){
        filter_region_text.push($(this).text());
    });
    let text_region = filter_region_text.join(", ");

    // Filter Group Brand Get Text
    let filter_group_brand_text= [];
    $.each($("#filter_group_brand option:selected"), function(){
        filter_group_brand_text.push($(this).text());
    });
    let text_group_brand = filter_group_brand_text.join(", ");

    let a = document.createElement("a");
    a.href = '/budget/ss-input/volume/export-csv?period=' + filter_period + '&channel=' + filter_channel + '&channelText=' + text_channel + '&subChannel=' + filter_subchannel + '&subChannelText=' + text_subchannel + '&account=' + filter_account + '&accountText=' + text_account + '&subAccount=' + filter_subaccount + '&subAccountText=' + text_subaccount + '&groupBrand=' + filter_groupbrand + '&groupBrandText=' + text_group_brand + '&region=' + filter_region + '&regionText=' + text_region;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

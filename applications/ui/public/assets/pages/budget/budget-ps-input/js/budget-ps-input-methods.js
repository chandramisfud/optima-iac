'use strict';

let dt_budget_ps_input, dialerObject;
let swalTitle = "Budget [PS Input]";
let elDtBudgetPSInput = $('#dt_budget_ps_input');
heightContainer = 290;

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

    getListFilter();

    dt_budget_ps_input = elDtBudgetPSInput.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/budget/ss-input/ps-input/list/paginate/filter?period=' + $('#filter_period').val() + '&distributor=' + $('#filter_distributor').val() + '&groupBrand=' + $('#filter_group_brand').val(),
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
                title: 'Distributor',
                data: 'distributor',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Brand',
                data: 'groupBrand',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
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
                targets: 4,
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
                targets: 5,
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
                targets: 6,
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
                targets: 7,
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
                targets: 8,
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
                targets: 9,
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
                targets: 10,
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
                targets: 11,
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
                targets: 12,
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
                targets: 13,
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
                targets: 14,
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
                targets: 15,
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
        ],
        initComplete: function () {

        },
        drawCallback: function () {

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
            showLoaderOnConfirm: false,
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

$('#dt_budget_ps_input_search').on('keyup', function() {
    dt_budget_ps_input.search(this.value, false, false).draw();
});

$('#dt_budget_ps_input_view').on('click', function (){
    let btn = document.getElementById('dt_budget_ps_input_view');
    let filter_period = $('#filter_period').val();
    let filter_distributor = JSON.stringify($('#filter_distributor').val());
    let filter_group_brand = JSON.stringify($('#filter_group_brand').val());

    let url = "/budget/ss-input/ps-input/list/paginate/filter?period=" + filter_period + "&distributor=" + filter_distributor + "&groupBrand=" + filter_group_brand;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_budget_ps_input.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

const getListFilter = () => {
    $.ajax({
        url         : "/budget/ss-input/ps-input/list/filter",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            if (!result['error']) {
                let data = result['data'];
                let distributorList = data['distributor'];
                let groupBrandList = data['grpBrand'];

                //<editor-fold desc="Dropdown Distributor">
                let dataDistributorList = [];
                for (let i = 0, len = distributorList.length; i < len; ++i) {
                    dataDistributorList.push({
                        id: distributorList[i]['id'],
                        text: distributorList[i]['longDesc']
                    });
                }
                let elSelectDistributor = $('#filter_distributor');

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
                    elSelectDistributor.select2({
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
                            let selected = (elSelectDistributor.val() || []).length;
                            let total = $('option', elSelectDistributor).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: dataDistributorList
                    });

                    $('[aria-controls="select2-filter_distributor-container"]').addClass('form-select form-select-sm');
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
                        data: dataGroupBrandList
                    });

                    $('[aria-controls="select2-filter_group_brand-container"]').addClass('form-select form-select-sm');
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

$('#btn_export_excel').on('click', function() {
    let filter_period = $('#filter_period').val();
    let filter_distributor = JSON.stringify($('#filter_distributor').val());
    let filter_group_brand = JSON.stringify($('#filter_group_brand').val());

    // Filter Distributor Get Text
    let filter_distributor_text= [];
    $.each($("#filter_distributor option:selected"), function(){
        filter_distributor_text.push($(this).text());
    });
    let text_distributor = filter_distributor_text.join(", ")

    // Filter Group Brand Get Text
    let filter_group_brand_text= [];
    $.each($("#filter_group_brand option:selected"), function(){
        filter_group_brand_text.push($(this).text());
    });
    let text_group_brand = filter_group_brand_text.join(", ")

    let a = document.createElement("a");
    a.href = "/budget/ss-input/ps-input/export-xls?period=" + filter_period + "&distributor=" + filter_distributor + "&distributorText=" + text_distributor + "&groupBrand=" + filter_group_brand + "&groupBrandText=" + text_group_brand;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_csv').on('click', function() {
    let filter_period = $('#filter_period').val();
    let filter_distributor = JSON.stringify($('#filter_distributor').val());
    let filter_group_brand = JSON.stringify($('#filter_group_brand').val());

    // Filter Distributor Get Text
    let filter_distributor_text= [];
    $.each($("#filter_distributor option:selected"), function(){
        filter_distributor_text.push($(this).text());
    });
    let text_distributor = filter_distributor_text.join(", ")

    // Filter Group Brand Get Text
    let filter_group_brand_text= [];
    $.each($("#filter_group_brand option:selected"), function(){
        filter_group_brand_text.push($(this).text());
    });
    let text_group_brand = filter_group_brand_text.join(", ")

    let a = document.createElement("a");
    a.href = "/budget/ss-input/ps-input/export-csv?period=" + filter_period + "&distributor=" + filter_distributor + "&distributorText=" + text_distributor + "&groupBrand=" + filter_group_brand + "&groupBrandText=" + text_group_brand;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn-download-template').on('click', function() {
    let filter_period = $('#filter_period').val();
    let filter_distributor = JSON.stringify($('#filter_distributor').val());
    let filter_group_brand = JSON.stringify($('#filter_group_brand').val());

    let a = document.createElement("a");
    a.href = "/budget/ss-input/ps-input/download-template?period=" + filter_period + "&distributor=" + filter_distributor + "&groupBrand=" + filter_group_brand;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn-upload').on('click', function() {
    window.location.href = "/budget/ss-input/ps-input/upload-form";
});

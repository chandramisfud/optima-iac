'use strict';

let dt_dn_ready_to_pay;
let swalTitle = "DN Ready To Pay";
heightContainer = 285;
let dataFilter, url_datatable;
let dialerObject;
let elFilterPeriod = $('#filter_period');
let elFilterCategory = $('#filter_category');
let elFilterEntity = $('#filter_entity');
let elFilterDistributor = $('#filter_distributor');
let arrDistributor = [];

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    if (localStorage.getItem('financeDnReadyToPayState')) {
        dataFilter = JSON.parse(localStorage.getItem('financeDnReadyToPayState'));

        url_datatable = `/fin-rpt/dn-ready-to-pay/list/paginate/filter?period=${dataFilter.period}&categoryId=${(dataFilter.category ?? "")}&entityId=${(dataFilter.entity ?? "")}&distributorId=${(dataFilter.distributor ?? "")}`;
    } else {
        url_datatable = '/fin-rpt/dn-ready-to-pay/list/paginate/filter?period=' + elFilterPeriod.val();
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
            elFilterCategory.val(dataFilter.category).trigger('change.select2');
            elFilterEntity.val(dataFilter.entity).trigger('change.select2');
            let distributorDropdown = [{id:'', text:''}];
            for (let i=0; i<arrDistributor.length; i++) {
                if (parseInt(dataFilter.entity) === arrDistributor[i]['entityId']) {
                    distributorDropdown.push({
                        id: arrDistributor[i]['distributorId'],
                        text: arrDistributor[i]['distributorDesc'],
                    })
                }
            }
            elFilterDistributor.select2({
                placeholder: "Select a Distributor",
                width: '100%',
                data: distributorDropdown
            });
            elFilterDistributor.val(dataFilter.distributor).trigger('change.select2');
        } else {
            elFilterPeriod.val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_dn_ready_to_pay = $('#dt_dn_ready_to_pay').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[0, 'asc']],
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
                title: 'Distributor',
                data: 'distributorDesc',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 1,
                title: 'DN Number',
                data: 'refId',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'Promo ID',
                data: 'promoRefId',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'DN Description',
                data: 'activityDesc',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'Last Status',
                data: 'lastStatus',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 5,
                title: 'Validate by Danone On',
                data: 'validateByDanoneOn',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 6,
                title: 'Fee (%)',
                data: 'feePct',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 7,
                title: 'Fee Amount',
                data: 'feeAmount',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 8,
                title: 'DPP',
                data: 'dpp',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 9,
                title: 'PPN (%)',
                data: 'ppnPct',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 10,
                title: 'PPN Amount',
                data: 'ppnAmt',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 11,
                title: 'PPH (%)',
                data: 'pphPct',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 12,
                title: 'PPH Amount',
                data: 'pphAmt',
                className: 'align-middle text-nowrap text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data, 0);
                    } else {
                        return "0";
                    }
                }
            },
            {
                targets: 13,
                title: 'Internal Doc. Number',
                data: 'intDocNo',
                className: 'align-middle text-nowrap'
            },
            {
                targets: 14,
                title: 'Tax Level',
                data: 'taxLevel',
                className: 'align-middle text-nowrap',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $('#dt_dn_ready_to_pay_search').on('keyup', function () {
        dt_dn_ready_to_pay.search(this.value, false, false).draw();
    });

});

$('#dt_dn_ready_to_pay_view').on('click', function (){
    let btn = document.getElementById('dt_dn_ready_to_pay_view');
    let filter_period = (elFilterPeriod.val()) ?? "";
    let filter_category = (elFilterCategory.val()) ?? "";
    let filter_entity = (elFilterEntity.val()) ?? "";
    let filter_distributor = (elFilterDistributor.val()) ?? "";

    let url = `/fin-rpt/dn-ready-to-pay/list/paginate/filter?period=${filter_period}&categoryId=${filter_category}&entityId=${filter_entity}&distributorId=${filter_distributor}`;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_dn_ready_to_pay.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
        let data_filter = {
            period: filter_period,
            category: filter_category,
            entity: filter_entity,
            distributor: filter_distributor,
        };

        localStorage.setItem('financeDnReadyToPayState', JSON.stringify(data_filter));
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_category = (elFilterCategory.val()) ?? "";
    let filter_entity = (elFilterEntity.val()) ?? "";
    let filter_distributor = (elFilterDistributor.val()) ?? "";

    let data_category = elFilterCategory.select2('data');
    let category = 'ALL';
    if (data_category.length > 0) {
        if (data_category[0].text !== "") {
            category = data_category[0].text
        }
    }

    let data_entity = elFilterEntity.select2('data');
    let entity = 'ALL';
    if (data_entity.length > 0) {
        if (data_entity[0].text !== "") {
            entity = data_entity[0].text
        }
    }

    let data_distributor = elFilterDistributor.select2('data');
    let distributor = 'ALL';
    if (data_distributor.length > 0) {
        if (data_distributor[0].text !== "") {
            distributor = data_distributor[0].text
        }
    }

    let url = `/fin-rpt/dn-ready-to-pay/export-xls?period=${filter_period}&categoryId=${filter_category}&entityId=${filter_entity}&distributorId=${filter_distributor}
                &category=${category}&entity=${entity}&distributor=${distributor}`;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_csv').on('click', function() {
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_category = (elFilterCategory.val()) ?? "";
    let filter_entity = (elFilterEntity.val()) ?? "";
    let filter_distributor = (elFilterDistributor.val()) ?? "";

    let data_category = elFilterCategory.select2('data');
    let category = 'ALL';
    if (data_category.length > 0) {
        if (data_category[0].text !== "") {
            category = data_category[0].text
        }
    }

    let data_entity = elFilterEntity.select2('data');
    let entity = 'ALL';
    if (data_entity.length > 0) {
        if (data_entity[0].text !== "") {
            entity = data_entity[0].text
        }
    }

    let data_distributor = elFilterDistributor.select2('data');
    let distributor = 'ALL';
    if (data_distributor.length > 0) {
        if (data_distributor[0].text !== "") {
            distributor = data_distributor[0].text
        }
    }

    let url = `/fin-rpt/dn-ready-to-pay/export-csv?period=${filter_period}&categoryId=${filter_category}&entityId=${filter_entity}&distributorId=${filter_distributor}
                &category=${category}&entity=${entity}&distributor=${distributor}`;
    window.open(url, '_blank');
});

elFilterEntity.on('change', async function () {
    let entityId = $(this).val();
    elFilterDistributor.empty();
    let distributorDropdown = [{id:'', text:''}];
    if (entityId) {
        for (let i=0; i<arrDistributor.length; i++) {
            if (parseInt(entityId) === arrDistributor[i]['entityId']) {
                distributorDropdown.push({
                    id: arrDistributor[i]['distributorId'],
                    text: arrDistributor[i]['distributorDesc'],
                })
            }
        }
    }
    $('#filter_distributor').select2({
        placeholder: "Select a Distributor",
        width: '100%',
        data: distributorDropdown
    });
});

const getListFilter = () => {
    return new Promise((resolve) => {
        $.ajax({
            url         : "/fin-rpt/dn-ready-to-pay/list/filter",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result['error']) {
                    let category = result['data']['category'];
                    arrDistributor = result['data']['entityDistributor'];
                    let entity = [...new Map(arrDistributor.map(item => [item['entityId'], item])).values()];

                    // Category
                    let categoryDropdown = [{id:'', text:''}];
                    for (let i = 0; i < category.length; i++) {
                        categoryDropdown.push({
                            id: category[i]['categoryId'],
                            text: category[i]['categoryLongDesc']
                        });
                    }
                    $('#filter_category').select2({
                        placeholder: "Select a Category",
                        width: '100%',
                        data: categoryDropdown
                    });

                    // Entity
                    let entityDropdown = [{id:'', text:''}];
                    for (let i = 0; i < entity.length; i++) {
                        entityDropdown.push({
                            id: entity[i]['entityId'],
                            text: entity[i]['entityDesc']
                        });
                    }
                    $('#filter_entity').select2({
                        placeholder: "Select an Entity",
                        width: '100%',
                        data: entityDropdown
                    });
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

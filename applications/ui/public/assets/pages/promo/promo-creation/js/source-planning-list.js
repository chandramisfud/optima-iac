'use strict';

let dt_source_planning_list;
let elDtSourcePlanningList = $('#dt_source_planning_list');

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getFilterEntity();

    dt_source_planning_list = elDtSourcePlanningList.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        scrollY: "40vh",
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Promo Planning ID',
                data: 'id',
                width: 100,
                visible: false,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 1,
                title: 'Promo Planning ID',
                data: 'refId',
                width: 100,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 2,
                title: 'Entity',
                data: 'entityLongDesc',
                width: 150,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 3,
                title: 'Distributor',
                data: 'distributorLongDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 4,
                title: 'Activity Description',
                data: 'activityDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 5,
                title: 'TS Code',
                data: 'tsCoding',
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 6,
                title: 'Promo Start',
                data: 'startPromo',
                className: 'text-nowrap align-middle cursor-pointer',
                render: function (data) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 7,
                title: 'Promo End',
                data: 'endPromo',
                className: 'text-nowrap align-middle cursor-pointer',
                render: function (data) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 8,
                title: 'Investment',
                data: 'investment',
                width: 200,
                className: 'text-nowrap align-middle text-end cursor-pointer',
                render: function (data) {
                    return formatMoney(data,0)
                }
            },
            {
                targets: 9,
                title: 'Initiator Notes',
                data: 'initiator_notes',
                className: 'text-nowrap align-middle cursor-pointer',
            },

        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    elDtSourcePlanningList.on( 'dblclick', 'tr', async function () {
        let data = dt_source_planning_list.row( this ).data();
        blockUIModal.block();
        clearBudgetSource();
        let getData = await getDataPlanningSource(data.id);
        calcPromo();
        validator.revalidateField('promoPlanningRefId');
        validator.resetField('subCategoryId');
        validator.resetField('activityId');
        validator.resetField('subActivityId');
        validator.resetField('activityDesc');
        validator.resetField('investment');
        validator.resetField('channelId');
        validator.resetField('subChannelId');
        validator.resetField('accountId');
        validator.resetField('subAccountId');
        blockUIModal.release();
        if (getData) {
            $('#modal_source_planning_list').modal('hide');
        } else {
            Swal.fire({
                title: "Promo Planning Source",
                text: "Failed get data detail",
                icon: "error",
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });

});

$('#dt_source_planning_list_search').on('keyup', function () {
    dt_source_planning_list.search(this.value, false, false).draw();
});

$('#dt_source_planning_list_view').on('click', function () {
    let btn = document.getElementById('dt_source_planning_list_view');
    let period = ($('#period').val() ?? "");
    let entityId = ($('#filter_entity').val() ?? "");
    let distributorId = ($('#filter_distributor').val() ?? "");
    let url = "/promo/creation/list/source-planning?period=" + period + "&entityId="+entityId + "&distributorId="+distributorId;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_source_planning_list.clear().draw();
    dt_source_planning_list.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#filter_entity').on('change', async function () {
    blockUIModal.block();
    let elFilterDistributor = $('#filter_distributor');
    elFilterDistributor.empty();
    if ($(this).val() !== "" && $(this).val() !== null) {
        await getFilterDistributor($(this).val());
    }
    elFilterDistributor.val('').trigger('change.select2');
    blockUIModal.release();
});

const getFilterEntity = () => {
    $.ajax({
        url         : "/promo/creation/list/entity",
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
            $('#filter_entity').select2({
                placeholder: "Select an Entity",
                width: '100%',
                data: data
            });
        },
        complete: function() {

        },
        error: function (jqXHR)
        {
            console.log(jqXHR);
        }
    });
}

const getFilterDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/list/distributor",
            type        : "GET",
            data        : {entityId: entityId},
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
                $('#filter_distributor').select2({
                    placeholder: "Select a Distributor",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDataPlanningSource = (promoPlanId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/creation/data/promo-planning-id",
            type: "GET",
            data: {promoPlanningId: promoPlanId},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;

                    tsCoding = values.promoPlanningHeader.tsCoding;
                    promoPlanningRefId = values.promoPlanningHeader.refId;
                    promoPlanningId = promoPlanId;
                    entityId = values.promoPlanningHeader.entityId;
                    entityShortDesc = values.promoPlanningHeader.entityShortDesc;
                    distributorId = values.promoPlanningHeader.distributorId;
                    categoryId = values.promoPlanningHeader.categoryId;
                    subCategoryId = values.promoPlanningHeader.subCategoryId;
                    activityId = values.promoPlanningHeader.activityId;
                    subActivityId = values.promoPlanningHeader.subActivityId;

                    $('#period').val(new Date(values.promoPlanningHeader.startPromo).getFullYear());
                    $('#activityDesc').val(values.promoPlanningHeader.activityDesc);
                    $('#initiatorNotes').val(values.promoPlanningHeader.initiator_notes);
                    $('#baselineSales').val(values.promoPlanningHeader.normalSales);
                    $('#incrementSales').val(values.promoPlanningHeader.incrSales);
                    $('#investment').val(values.promoPlanningHeader.investment);
                    $('#totalInvestment').val(values.promoPlanningHeader.investment);
                    $('#totalSales').val((parseFloat(values.promoPlanningHeader.incrSales) + parseFloat(values.promoPlanningHeader.normalSales)).toString());
                    $('#roi').val(values.promoPlanningHeader.roi);
                    $('#costRatio').val(values.promoPlanningHeader.costRatio);
                    $('#startPromo').val(formatDate(values.promoPlanningHeader.startPromo)).flatpickr({
                        altFormat: "d-m-Y",
                        altInput: true,
                        allowInput: true,
                        dateFormat: "Y-m-d",
                        disableMobile: "true",
                    });
                    $('#endPromo').val(formatDate(values.promoPlanningHeader.endPromo)).flatpickr({
                        altFormat: "d-m-Y",
                        altInput: true,
                        allowInput: true,
                        dateFormat: "Y-m-d",
                        disableMobile: "true",
                    });

                    for (let i = 0; i < values.channels.length; i++) {
                        if (values.channels[i].flag) channelId = values.channels[i].id;
                    }
                    for (let i = 0; i < values.subChannels.length; i++) {
                        if (values.subChannels[i].flag) subChannelId = values.subChannels[i].id;
                    }
                    for (let i = 0; i < values.accounts.length; i++) {
                        if (values.accounts[i].flag) accountId = values.accounts[i].id;
                    }
                    for (let i = 0; i < values.subAccounts.length; i++) {
                        if (values.subAccounts[i].flag) subAccountId = values.subAccounts[i].id;
                    }

                    await $('#subCategoryId').val(subCategoryId).trigger('change.select2');
                    $('#tsCode').val(values.promoPlanningHeader.tsCoding);
                    $('#entity').val(values.promoPlanningHeader.entityLongDesc);
                    entityShortDesc = values.promoPlanningHeader.entityShortDesc;
                    $('#distributor').val(values.promoPlanningHeader.distributorLongDesc);
                    $('#promoPlanningRefId').val(promoPlanningRefId);


                    await getActivity(subCategoryId);
                    await $('#activityId').val(activityId).trigger('change.select2');

                    await getSubActivity(activityId);
                    $('#subActivityId').val(subActivityId).trigger('change.select2');

                    await getInvestmentType(subActivityId);

                    $('#channelId').val(channelId).trigger('change.select2');

                    await getSubChannel(channelId)
                    await $('#subChannelId').val(subChannelId).trigger('change.select2');

                    await getAccount(subChannelId);
                    await $('#accountId').val(accountId).trigger('change.select2');

                    await getSubAccount(accountId)
                    $('#subAccountId').val(subAccountId).trigger('change.select2');

                    //Attribute Region
                    arr_region = [];
                    let longDescRegion = '';
                    for (let i = 0; i < values.regions.length; i++) {
                        if (values.regions[i].flag) {
                            arr_region.push(values.regions[i].id);
                            longDescRegion += '<span className="fw-bold text-sm-left">' + values.regions[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        }
                    }
                    $('#card_list_region').html(longDescRegion);

                    //Attribute Brand
                    arr_brand = [];
                    let longDescBrand = '';
                    for (let i = 0; i < values.brands.length; i++) {
                        if (values.brands[i].flag) {
                            arr_brand.push(values.brands[i].id);
                            longDescBrand += '<span className="fw-bold text-sm-left">' + values.brands[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        }
                    }
                    $('#card_list_brand').html(longDescBrand);

                    //Attribute SKU
                    arr_sku = [];
                    let longDescSKU = '';
                    for (let i = 0; i < values.skus.length; i++) {
                        if (values.skus[i].flag) {
                            arr_sku.push(values.skus[i].id);
                            longDescSKU += '<span className="fw-bold text-sm-left">' + values.skus[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        }
                    }
                    $('#card_list_sku').html(longDescSKU);

                    // Attribuate Mechanism
                    let nourut = 0;

                    dt_mechanism.clear().draw();
                    arr_mechanism = [];
                    for (let i = 0; i < values.mechanisms.length; i++) {
                        nourut += 1;
                        let dMechanism = [];
                        dMechanism["no"] = nourut;
                        dMechanism["mechanism"] = values.mechanisms[i].mechanism;
                        dMechanism["notes"] = values.mechanisms[i].notes;
                        dMechanism["productId"] = values.mechanisms[i].productId;
                        dMechanism["product"] = values.mechanisms[i].product;
                        dMechanism["brandId"] = values.mechanisms[i].brandId;
                        dMechanism["brand"] = values.mechanisms[i].brand;
                        dMechanism["mechanismId"] = values.mechanisms[i].mechanismId;
                        let mechanism = {};
                        mechanism.id = values.mechanisms[i].mechanismId;
                        mechanism.mechanism = values.mechanisms[i].mechanism;
                        mechanism.notes = values.mechanisms[i].notes;
                        mechanism.productId = values.mechanisms[i].productId;
                        mechanism.product = values.mechanisms[i].product;
                        mechanism.brandId = values.mechanisms[i].brandId;
                        mechanism.brand = values.mechanisms[i].brand;

                        arr_mechanism.push(mechanism);

                        dt_mechanism.row.add(dMechanism).draw();
                    }
                    return resolve(true);
                } else {
                    return reject(false);
                }
            },
            complete: function () {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

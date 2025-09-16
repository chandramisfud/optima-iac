'use strict';

let dt_promo_creation;
let swalTitle = "Promo Creation";
heightContainer = 280;
let dataFilter, url_datatable;
let elDtPromoCreation = $('#dt_promo_creation');
let dialerObject;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    if (localStorage.getItem('promoCreationState')) {
        dataFilter = JSON.parse(localStorage.getItem('promoCreationState'));

        url_datatable = '/promo/creation/list/paginate/filter?period=' + dataFilter.period +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId + "&categoryId=" + (dataFilter.categoryId ?? "0");
    } else {
        url_datatable = '/promo/creation/list/paginate/filter?period=' + $('#filter_period').val();
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListCategory(), getListEntity()]).then(async function () {
        if (dataFilter) {
            $('#filter_period').val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));
            await $('#filter_category').val(dataFilter.categoryId).trigger('change.select2');
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_promo_creation = elDtPromoCreation.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        saveState: true,
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'promoId',
                width: 20,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full) {
                    let popMenu;
                    if (full['isCancelLocked'] || full['isClose']) {
                        popMenu =  `<div class="me-0">
                                        <a class="btn show menu-dropdown disabled" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-bs-placement="right" >
                                            <i class="la la-cog fs-3 text-optima text-hover-optima"></i>
                                        </a>
                                    </div>`;
                    } else {
                        popMenu =  `<div class="me-0">
                                        <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-bs-placement="right" >
                                            <i class="la la-cog fs-3 text-optima text-hover-optima"></i>
                                        </a>
                                        <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">
                                            <a class="dropdown-item text-start edit-record" href="/promo/creation/form?method=update&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}"><i class="fa fa-edit fs-6"></i> Edit Data</a>
                                            <a class="dropdown-item text-start req-cancel-record" href="/promo/creation/form-cancel-request?method=cancelrequest&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}"><i class="fa fa-undo fs-6"></i> Cancel Request</a>
                                        </div>
                                    </div>`;
                        if (!full['isOldPromo']) {
                            if (full['editable']) {
                                if (!full['reconciled']) {
                                    popMenu =  `<div class="me-0">
                                        <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-bs-placement="right" >
                                            <i class="la la-cog fs-3 text-optima text-hover-optima"></i>
                                        </a>
                                        <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">
                                            <a class="dropdown-item text-start edit-record" href="/promo/creation/form?method=update&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}"><i class="fa fa-edit fs-6"></i> Edit Data</a>
                                            <a class="dropdown-item text-start req-cancel-record" href="/promo/creation/form-cancel-request?method=cancelrequest&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}"><i class="fa fa-undo fs-6"></i> Cancel Request</a>
                                            <a class="dropdown-item text-start duplicate-promo-record cursor-pointer" href="/promo/creation/form?method=duplicate&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}"><i class="fa fa-paste fs-6"></i> Duplicate Promo ID</a>
                                        </div>
                                    </div>`;
                                } else {
                                    popMenu =  `<div class="me-0">
                                        <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-bs-placement="right" >
                                            <i class="la la-cog fs-3 text-optima text-hover-optima"></i>
                                        </a>
                                        <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">
                                            <a class="dropdown-item text-start edit-record" href="/promo/creation/form?method=update&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}&recon=1"><i class="fa fa-edit fs-6"></i> Edit Data</a>
                                            <a class="dropdown-item text-start req-cancel-record" href="/promo/creation/form-cancel-request?method=cancelrequest&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}&recon=1"><i class="fa fa-undo fs-6"></i> Cancel Request</a>
                                            <a class="dropdown-item text-start duplicate-promo-record cursor-pointer" href="/promo/creation/form?method=duplicate&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}"><i class="fa fa-paste fs-6"></i> Duplicate Promo ID</a>
                                        </div>
                                    </div>`;
                                }
                            } else {
                                popMenu =  `<div class="me-0">
                                        <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-bs-placement="right" >
                                            <i class="la la-cog fs-3 text-optima text-hover-optima"></i>
                                        </a>
                                        <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">
                                            <a class="dropdown-item text-start req-cancel-record" href="/promo/creation/form-cancel-request?method=cancelrequest&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}"><i class="fa fa-undo fs-6"></i> Cancel Request</a>
                                            <a class="dropdown-item text-start duplicate-promo-record cursor-pointer" href="/promo/creation/form?method=duplicate&promoId=${full['promoId']}&c=${full['categoryShortDesc']}&old=${(full['isOldPromo'] ? 1 : 0)}"><i class="fa fa-paste fs-6"></i> Duplicate Promo ID</a>
                                        </div>
                                    </div>`;
                            }
                        }
                    }
                    return popMenu;
                }
            },
            {
                targets: 1,
                data: 'refId',
                width: 120,
                title: 'Promo ID',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 250,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'Promo Start',
                data: 'startPromo',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 4,
                title: 'Promo End',
                data: 'endPromo',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 5,
                title: 'Cost',
                data: 'investment',
                width: 100,
                className: 'align-middle text-end',
                render: function (data) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 6,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 7,
                title: 'Recon Status',
                data: 'reconciledStatus',
                width: 100,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 8,
                title: 'Approval Recon Status',
                data: 'approvalReconStatus',
                width: 100,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    return data ?? "";
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        let strMessage = e.jqXHR['responseJSON']['message']  ?? "Please contact your vendor";
        Swal.fire({
            text: strMessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}

        });
    };

});

$('#dt_promo_creation_search').on('keyup', function () {
    dt_promo_creation.search(this.value).draw();
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    let elDistributor = $('#filter_distributor');
    elDistributor.empty();
    if ($(this).val()) await getListDistributor($(this).val());
    blockUI.release();
    elDistributor.val('').trigger('change');
});

$('#dt_promo_creation_view').on('click', function (){
    let btn = document.getElementById('dt_promo_creation_view');
    let filter_period = ($('#filter_period').val() ?? "");
    let filter_category = ($('#filter_category').val() ?? "");
    let filter_entity = ($('#filter_entity').val() ?? "");
    let filter_distributor = ($('#filter_distributor').val() ?? "");
    let filter_activity_start = ($('#filter_activity_start').val() ?? "");
    let filter_activity_end = ($('#filter_activity_end').val() ?? "");
    let url = "/promo/creation/list/paginate/filter?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&categoryId=" + filter_category;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_creation.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
        let data_filter = {
            period: $('#filter_period').val(),
            activityStart: $('#filter_activity_start').val(),
            activityEnd: $('#filter_activity_end').val(),
            categoryId: ($('#filter_category').val() ?? ""),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? ""),
        };

        localStorage.setItem('promoCreationState', JSON.stringify(data_filter));
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        console.log(xhr)
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('.record-create').on('click', function() {
    let c = $(this).attr('data-category');
    checkFormAccess('create_rec', '', `/promo/creation/form?c=${c}`, '');
});

$('#btn_generate').on('click', function() {
    checkFormAccess('create_rec', '', `/promo/creation/form-generate`, '');
});

$('#btn_export_excel').on('click', function() {
    let elCategory = $('#filter_category');
    let elEntity = $('#filter_entity');
    let elDistributor = $('#filter_distributor');
    let dataCategory = elCategory.select2('data');
    let text_category = ( (dataCategory[0].id !== "") ? dataCategory[0].text : 'ALL' );
    let data = elEntity.select2('data');
    let text_entity = ( (data[0].id !== "") ? data[0].text : 'ALL' );
    let text_distributor;
    let dataDist = elDistributor.select2('data');
    if (dataDist.length > 0) {
        if (dataDist[0].id !== "") {
            text_distributor = dataDist[0].text
        } else {
            text_distributor ='ALL'
        }
    } else {
        text_distributor ='ALL'
    }

    let filter_period = ($('#filter_period').val() ?? "");
    let filter_category = (elCategory.val() ?? "0");
    let filter_entity = (elEntity.val() ?? "0");
    let filter_distributor = (elDistributor.val() ?? "0");
    let filter_activity_start = ($('#filter_activity_start').val() ?? "");
    let filter_activity_end = ($('#filter_activity_end').val() ?? "");
    let url = "/promo/creation/export-xls?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&entity=" + text_entity + "&distributor=" + text_distributor
        + "&categoryId=" + filter_category + "&category=" + text_category;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const getListCategory = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/list/category",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j]['Id'],
                        text: result.data[j]['categoryLongDesc']
                    });
                }
                $('#filter_category').select2({
                    placeholder: "Select a Category",
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

const getListEntity = () => {
    return new Promise((resolve, reject) => {
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
                        text: result.data[j].shortDesc + " - " + result.data[j].longDesc,
                        longDesc: result.data[j].longDesc
                    });
                }
                $('#filter_entity').select2({
                    placeholder: "Select an Entity",
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

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/creation/list/distributor",
            type        : "GET",
            dataType    : 'json',
            data        : {entityId: entityId},
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

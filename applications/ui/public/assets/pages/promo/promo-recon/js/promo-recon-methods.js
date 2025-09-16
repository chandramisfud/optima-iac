'use strict';

let dt_promo_recon, elDtPromoRecon = $('#dt_promo_recon');
let swalTitle = "Promo Reconciliation";
heightContainer = 320;
let dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#filter_activity_start, #filter_activity_end').flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    if (localStorage.getItem('promoReconState')) {
        dataFilter = JSON.parse(localStorage.getItem('promoReconState'));

        url_datatable = '/promo/recon/list?period=' + dataFilter.period +
            "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId + "&startFrom=" + dataFilter.activityStart +
            "&startTo=" + dataFilter.activityEnd + "&categoryId=" + (dataFilter.categoryId ?? "0");
    } else {
        url_datatable = '/promo/recon/list?period=' + $('#filter_period').val() + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end;
    }

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListCategory(), getListEntity(), getListChannel()]).then(async function () {
        if (dataFilter) {
            $('#filter_period').val(dataFilter.period);
            dialerObject.setValue(parseInt(dataFilter.period));
            await $('#filter_category').val(dataFilter.categoryId).trigger('change.select2');
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
            $('#filter_activity_start').flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(dataFilter.activityStart)
            });
            $('#filter_activity_end').flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
                defaultDate: new Date(dataFilter.activityEnd)
            });
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_promo_recon = elDtPromoRecon.DataTable({
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
                data: 'promoId',
                width: 20,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full) {
                    let popMenu;
                    if (full.isCancelLocked || full.isClose) {
                        popMenu =  '\
                            <div class="me-0">\
                                <a class="btn show menu-dropdown disabled" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-bs-placement="right">\
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                                </a>\
                            </div>\
                        ';
                    } else {
                        popMenu = '\
                                <div class="me-0">\
                                    <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start" data-bs-placement="right" >\
                                        <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                                    </a>\
                                    <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                        <a class="dropdown-item text-start edit-record" href="/promo/recon/form?method=update&promoId=' + full.promoId  + '&c=' + full.categoryShortDesc + '"><i class="fa fa-edit fs-6"></i> Edit Data</a>\
                                        <a class="dropdown-item text-start req-cancel-record" href="/promo/recon/form-cancel-request?method=cancelrequest&promoId=' + full.promoId  + '&c=' + full.categoryShortDesc + '"><i class="fa fa-undo fs-6"></i> Cancel Request</a>\
                                    </div>\
                                </div>\
                            ';
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
                title: 'Account',
                data: 'accountDesc',
                width: 90,
                className: 'align-middle',
                createdCell: function (cell, data) {
                    let $cell = $(cell);

                    if (data) {
                        $(cell).contents().wrapAll('<div class="content"></div>');
                        let $content = $cell.find(".content");
                        if (data.length > 45)
                            $(cell).append($('<button class="btn btn-clean-more">Read more</button>'));
                        let $btn = $(cell).find("button");

                        $content.css({
                            "padding": "0",
                            "height": "20px",
                            "width": "300px",
                            "overflow": "hidden"
                        })
                        $cell.data("isLess", true);

                        $btn.click(function () {
                            let isLess = $cell.data("isLess");
                            $content.css("height", isLess ? "auto" : "20px")
                            $(this).text(isLess ? "Read less" : "Read more")
                            $cell.data("isLess", !isLess)
                        })
                    }
                }
            },
            {
                targets: 6,
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 90,
                className: 'align-middle',
                createdCell: function (cell, data) {
                    let $cell = $(cell);

                    if (data) {
                        $(cell).contents().wrapAll('<div class="content"></div>');
                        let $content = $cell.find(".content");
                        if (data.length > 45)
                            $(cell).append($('<button class="btn btn-clean-more">Read more</button>'));
                        let $btn = $(cell).find("button");

                        $content.css({
                            "padding": "0",
                            "height": "20px",
                            "width": "300px",
                            "overflow": "hidden"
                        })
                        $cell.data("isLess", true);

                        $btn.click(function () {
                            let isLess = $cell.data("isLess");
                            $content.css("height", isLess ? "auto" : "20px")
                            $(this).text(isLess ? "Read less" : "Read more")
                            $cell.data("isLess", !isLess)
                        })
                    }
                }
            },
            {
                targets: 7,
                title: 'Investment',
                data: 'investment',
                width: 100,
                className: 'align-middle text-end',
                render: function (data) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 8,
                title: 'Initiator Notes',
                data: 'initiator_notes',
                width: 200,
                className: 'align-middle text-nowrap'
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}

        });
    };
});

$('#dt_promo_recon_search').on('keyup', function () {
    dt_promo_recon.search(this.value).draw();
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

$('#filter_period').on('change', function() {
    let period = this.value;
    let startDate = formatDate(new Date(period,0,1));
    let endDate = formatDate(new Date(period, 11, 31));
    $('#filter_activity_start').val(startDate);
    $('#filter_activity_end').val(endDate);
    $('#filter_activity_start, #filter_activity_end').flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });
});

$('#filter_activity_start').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_end.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(startDate)
        });
    }
});

$('#filter_activity_end').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_start.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(endDate)
        });
    }
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    let elDistributor = $('#filter_distributor');
    elDistributor.empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    elDistributor.val('').trigger('change');
});

$('#dt_promo_recon_view').on('click', function (){
    let btn = document.getElementById('dt_promo_recon_view');
    let filter_period = ($('#filter_period').val() ?? "");
    let filter_category = ($('#filter_category').val() ?? "");
    let filter_entity = ($('#filter_entity').val() ?? "");
    let filter_distributor = ($('#filter_distributor').val() ?? "");
    let filter_channel = ($('#filter_channel').val() ?? "");
    let filter_activity_start = ($('#filter_activity_start').val() ?? "");
    let filter_activity_end = ($('#filter_activity_end').val() ?? "");
    let url = "/promo/recon/list?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel + "&categoryId="+ filter_category;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_recon.ajax.url(url).load(function () {
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

        localStorage.setItem('promoReconState', JSON.stringify(data_filter));
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let elCategory = $('#filter_category');
    let elEntity = $('#filter_entity');
    let elDistributor = $('#filter_distributor');
    let dataCategory = elCategory.select2('data');
    let text_category = ( (dataCategory[0].id !== "") ? dataCategory[0].text : 'ALL' );
    let text_entity;
    let data = elEntity.select2('data');
    (data[0].id !== "") ? text_entity = data[0].text : text_entity ='ALL';
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
    let url = "/promo/recon/export-xls?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
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
            url         : "/promo/recon/list/category",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].Id,
                        text: result.data[j].categoryLongDesc
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
            url         : "/promo/recon/list/entity",
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
            url         : "/promo/recon/list/distributor",
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

const getListChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon/list/channel",
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
                $('#filter_channel').select2({
                    placeholder: "Select a Channel",
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

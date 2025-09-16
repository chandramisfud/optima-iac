'use strict';

var dt_promo_recon_send_back;
var swalTitle = "Promo Send Back Reconciliation";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    let dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });
    dialerObject.setValue(new Date().getFullYear());

    getListEntity();
    getListCategory();

    dt_promo_recon_send_back = $('#dt_promo_recon_send_back').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/promo/recon-send-back/list?period=' + $('#filter_period').val(),
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: true,
        ordering: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[6, 'asc']],
        columnDefs: [
            {
                targets: 0,
                data: 'promoId',
                width: 20,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start edit-record" href="/promo/recon-send-back/form?method=update&id=' + full.promoId + '&c=' + full.categoryShortDesc + '"><i class="fa fa-edit fs-6"></i> Edit Data</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'Promo ID',
                data: 'promoNumber',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 200,
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Promo Start',
                data: 'startPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatDate(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 4,
                title: 'Promo End',
                data: 'endPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatDate(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 5,
                title: 'Send Back Date',
                data: 'approveOn',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatDate(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 6,
                title: 'Send Back Notes',
                data: 'approvalNotes',
                width: 100,
                className: 'align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
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

$('#dt_promo_recon_send_back_search').on('keyup', function () {
    dt_promo_recon_send_back.search(this.value).draw();
});

$('#dt_promo_recon_send_back_view').on('click', function (){
    let btn = document.getElementById('dt_promo_recon_send_back_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_category = ($('#filter_category').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/promo/recon-send-back/list?period=" + filter_period + "&categoryId=" + filter_category + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&categoryId=" + filter_category;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_recon_send_back.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let text_category = "";
    let dataCategory = $('#filter_category').select2('data');
    if (dataCategory.length > 0) {
        if (dataCategory[0].id !== "") {
            text_category = dataCategory[0].text
        } else {
            text_category ='ALL'
        }
    } else {
        text_category ='ALL'
    }

    let text_entity = "";
    let data = $('#filter_entity').select2('data');
    (data[0].id !== "") ? text_entity = data[0].text : text_entity ='ALL';
    let text_distributor = "";
    let dataDist = $('#filter_distributor').select2('data');
    if (dataDist.length > 0) {
        if (dataDist[0].id !== "") {
            text_distributor = dataDist[0].text
        } else {
            text_distributor ='ALL'
        }
    } else {
        text_distributor ='ALL'
    }

    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/promo/recon-send-back/export-xls?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&entity=" + text_entity + "&distributor=" + text_distributor + "&category=" + text_category;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
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
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

const getListCategory = () => {
    $.ajax({
        url         : "/promo/recon-send-back/list/category",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j){
                data.push({
                    id: result.data[j].Id,
                    text: result.data[j].categoryLongDesc,
                });
            }
            $('#filter_category').select2({
                placeholder: "Select a Category",
                width: '100%',
                data: data
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

const getListEntity = () => {
    $.ajax({
        url         : "/promo/recon-send-back/list/entity",
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

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(jqXHR.responseText);
        }
    });
}

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/recon-send-back/list/distributor",
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
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

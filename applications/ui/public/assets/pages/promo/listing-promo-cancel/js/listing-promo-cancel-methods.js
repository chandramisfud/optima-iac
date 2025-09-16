'use strict';

var dt_listing_promo_cancel;
var swalTitle = "Listing Promo Cancel";
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
        min: 1900,
        step: 1,
    });
    dialerObject.setValue(new Date().getFullYear());

    getListEntity();

    dt_listing_promo_cancel = $('#dt_listing_promo_cancel').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/promo/listing-promo-cancel/list?period=' + $('#filter_period').val(),
            type: 'get',
        },
        processing: true,
        serverSide: false,
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
                title: 'Promo ID',
                data: 'refId',
                width: 100,
                orderable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Last Status',
                data: 'lastStatus',
                width: 300,
                orderable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 200,
                orderable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'TS Code',
                data: 'tsCoding',
                width: 100,
                orderable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Allocation',
                data: 'allocation',
                width: 100,
                orderable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Investment',
                data: 'investment',
                width: 100,
                orderable: false,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 6,
                title: 'Cancel Notes',
                data: 'cancelNotes',
                width: 100,
                orderable: false,
                className: 'text-nowrap align-middle',
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

    $("#dt_listing_promo_cancel").on('click', '.view-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_listing_promo_cancel.row(tr).data();

        let url = '/promo/listing-promo-cancel/form?method=view&id=' + trdata.promoId;
        window.location.href = url;
    });
});

$('#dt_listing_promo_cancel_search').on('keyup', function () {
    dt_listing_promo_cancel.search(this.value, false, false).draw();
});

$('#dt_listing_promo_cancel_view').on('click', function (){
    let btn = document.getElementById('dt_listing_promo_cancel_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/promo/listing-promo-cancel/list?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_listing_promo_cancel.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
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
    let url = "/promo/listing-promo-cancel/export-xls?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&entity=" + text_entity + "&distributor=" + text_distributor;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

const getListEntity = () => {
    $.ajax({
        url         : "/promo/listing-promo-cancel/list/entity",
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
            url         : "/promo/listing-promo-cancel/list/distributor",
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

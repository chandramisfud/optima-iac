'use strict';

var dt_listing_promo_distributor;
var swalTitle = "Listing Promo Distributor";
heightContainer = 320;
var dataFilter, url_datatable;
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#filter_create_start, #filter_create_end, #filter_activity_start, #filter_activity_end').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListCategory(), getListEntity()]).then(async function () {
        $('#filter_period').val(new Date().getFullYear());
        dialerObject.setValue(new Date().getFullYear());
    });

    dt_listing_promo_distributor = $('#dt_listing_promo_distributor').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url:'/dn/rpt-listing-promo-distributor/list/paginate/filter?period=' + $('#filter_period').val() + "&startFrom=" + $('#filter_activity_start').val()  + "&startTo=" + $('#filter_activity_end').val() + "&createFrom=" + $('#filter_create_start').val()  + "&createTo=" + $('#filter_create_end').val() + "&entityId=0",
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
                data: 'promoNumber',
                title: 'Promo ID',
                className: 'text-nowrap align-middle'
            },
            {
                targets: 1,
                title: 'Initiator',
                data: 'initiator',
                width: 100,
                className: 'text-nowrap align-middle'
            },
            {
                targets: 2,
                title: 'Sub Account',
                data: 'subAccountDesc',
                className: 'text-nowrap align-middle'
            },
            {
                targets: 3,
                title: 'Activity Description',
                data: 'activityDesc',
                className: 'text-nowrap align-middle'
            },
            {
                targets: 4,
                title: 'Start Promo',
                data: 'startPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 5,
                title: 'End Promo',
                data: 'endPromo',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 6,
                title: 'Creation Date',
                data: 'createOn',
                width: 90,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 7,
                title: 'Status',
                data: 'lastStatus',
                className: 'text-nowrap align-middle'
            },
            {
                targets: 8,
                title: 'Status Date',
                data: 'lastStatusDate',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 9,
                title: 'DN Claim',
                data: 'dnClaim',
                width: 180,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

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

//Activity
$('#filter_activity_start').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_end.val(el_start.val());
    }
});

$('#filter_activity_end').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_start.val(el_end.val());
    }
});

//Create From
$('#filter_create_start').on('change', function () {
    let el_start = $('#filter_create_start');
    let el_end = $('#filter_create_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_end.val(el_start.val());
    }
});

$('#filter_create_end').on('change', function () {
    let el_start = $('#filter_create_start');
    let el_end = $('#filter_create_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_start.val(el_end.val());
    }
});

$('#filter_period').on('change', function() {
    let period = this.value;
    var startDate = formatDate(new Date(period,0,1));
    var endDate = formatDate(new Date(period, 11, 31));
    $('#filter_activity_start').val(startDate);
    $('#filter_activity_end').val(endDate);
    $('#filter_create_start').val(startDate);
    $('#filter_create_end').val(endDate);
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2015) {
        $(this).val("2015");
        dialerObject.setValue(2015);
    }
});

$('#dt_listing_promo_distributor_search').on('keyup', function () {
    dt_listing_promo_distributor.search(this.value).draw();
});

$('#dt_listing_promo_distributor_view').on('click', function (){
    let btn = document.getElementById('dt_listing_promo_distributor_view');
    let filter_period = ($('#filter_period').val() ?? "");
    let filter_entity = ($('#filter_entity').val() ?? 0);
    let filter_category = ($('#filter_category').val() ?? 0);
    let filter_activity_start = ($('#filter_activity_start').val() ?? "");
    let filter_activity_end = ($('#filter_activity_end').val() ?? "");
    let filter_create_start = ($('#filter_create_start').val() ?? "");
    let filter_create_end = ($('#filter_create_end').val() ?? "");
    let url = "/dn/rpt-listing-promo-distributor/list/paginate/filter?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&categoryId=" + filter_category + "&entityId=" + filter_entity + "&createFrom=" + filter_create_start + "&createTo=" + filter_create_end;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_listing_promo_distributor.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let text_entity = "";
    let data_entity = $('#filter_entity').select2('data');
    (data_entity[0].id !== "") ? text_entity = data_entity[0].longDesc : text_entity ='ALL';

    let filter_category = ($('#filter_category').val() ?? 0);
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let filter_create_start = ($('#filter_create_start').val()) ?? "";
    let filter_create_end = ($('#filter_create_end').val()) ?? "";
    let filter_entity = ($('#filter_entity').val() ?? 0);
    let url = "/dn/rpt-listing-promo-distributor/export-xls?period=" + filter_period + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end
        + "&createFrom=" + filter_create_start + "&createTo=" + filter_create_end + "&categoryId=" + filter_category + "&entityId=" + filter_entity + "&entity=" + text_entity;
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
            url         : "/dn/rpt-listing-promo-distributor/list/category",
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
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/rpt-listing-promo-distributor/list/entity",
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
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

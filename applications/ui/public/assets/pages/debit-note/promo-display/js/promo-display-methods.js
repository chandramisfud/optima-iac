'use strict';

var dt_promo_display;
var swalTitle = "Promo Display";
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

    dt_promo_display = $('#dt_promo_display').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/promo-display/list/paginate/filter?period=' + $('#filter_period').val(),
            type: 'get',
        },
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
                render: function (data, type, full, meta) {
                    let popMenu = '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start view-record" href="/dn/promo-display/form?method=view&id=' + full.promoId + '&c=' + full.categoryShortDesc + '"><i class="fa fa-edit fs-6"></i> View Data</a>\
                            </div>\
                        </div>\
                    ';
                    return popMenu;
                }
            },
            {
                targets: 1,
                title: 'Promo ID',
                data: 'refId',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Last Status',
                data: 'lastStatus',
                width: 300,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'TS Code',
                data: 'tsCoding',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Allocation',
                data: 'allocation',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Investment',
                data: 'investment',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
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

$('#dt_promo_display_search').on('keyup', function () {
    dt_promo_display.search(this.value).draw();
});

$('#dt_promo_display_view').on('click', function (){
    let btn = document.getElementById('dt_promo_display_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let url = "/dn/promo-display/list/paginate/filter?period=" + filter_period + "&entityId=" + filter_entity;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_display.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

const getListEntity = () => {
    $.ajax({
        url         : "/dn/promo-display/list/entity",
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

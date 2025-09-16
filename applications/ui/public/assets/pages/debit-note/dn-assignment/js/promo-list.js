'use strict';

var dt_promo_list;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListFilterEntity();
    getListFilterChannel();

    dt_promo_list = $('#dt_promo_list').DataTable({
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
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 100,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 1,
                title: 'Ref ID',
                data: 'refId',
                width: 100,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 2,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 3,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 100,
                className: 'text-nowrap align-middle cursor-pointer',
            },
            {
                targets: 4,
                title: 'Allocation',
                data: 'allocation',
                width: 100,
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });
});

$('#dt_promo_list_search').on('keyup', function() {
    dt_promo_list.search(this.value).draw();
});

$('#dt_promo_list').on( 'dblclick', 'tr', function () {
    var data = dt_promo_list.row( this ).data();

    $('#promoRefId').val(data.refId);
    $('#entityId').val(data.entityName);
    $('#entityAddress').val(data.entityAddress);
    $('#entityUp').val(data.entityUp);

    let startPromo = formatDate(data.startPromo);
    let endPromo = formatDate(data.endPromo);
    let period = startPromo + ' s/d ' + endPromo;
    $('#period').val(period.toString());

    entityId = data.entityId;
    promoId = data.promoId;

    $('#modal_list_promo').modal('hide');
});

$('#dt_promo_list_view').on('click',  function () {
    let filter_period = ($('#year').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_channel = ($('#filter_channel').val()) ?? "";

    if(filter_entity=="" || filter_entity==null) { filter_entity = "0"; }
    if(filter_channel=="" || filter_channel==null) { filter_channel = "0"; }

    dt_promo_list.clear().draw()
    let url = "/dn/assignment/get-data/promo?period=" +  filter_period + "&subAccountId="+ accountId + "&entityId=" + filter_entity + "&channelId=" + filter_channel;
    dt_promo_list.ajax.url(url).load();
});

const getListFilterEntity = () => {
    $.ajax({
        url         : "/dn/creation/list/entity",
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

const getListFilterChannel = () => {
    $.ajax({
        url         : "/dn/creation/list/channel",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j){
                data.push({
                    id: result.data[j].id,
                    text: result.data[j].longDesc,
                });
            }
            $('#filter_channel').select2({
                placeholder: "Select a Channel",
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

'use strict';

var dt_dn_multi_print, dt_dn_multi_print_list;
var swalTitle = "Multi Print Debit Note";
heightContainer = 350;
var dataFilter, url_datatable;

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
    let dialerObject = new KTDialer(dialerElement, {
        min: 2015,
        step: 1,
    });

    Promise.all([getListSubAccount()]).then(async function () {
        $('#filter_period').val(new Date().getFullYear());
        dialerObject.setValue(new Date().getFullYear());
    });

    dt_dn_multi_print = $('#dt_dn_multi_print').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-5'i><'col-sm-6'p>>",
        order: [[1, 'asc']],
        ajax: {
            url:'/dn/multi-print/list/paginate/filter?period=' + $('#filter_period').val() + "&subAccountId=" + $('#filter_subaccount').val(),
            type: 'get',
        },
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
                data: 'id',
                visible: false,
            },
            {
                targets: 1,
                data: 'refId',
                width: 170,
                title: 'DN Number',
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'DN Description',
                data: 'activityDesc',
                width: 350,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'DPP',
                data: 'dpp',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 4,
                title: 'Category',
                data: 'dnCategory',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 5,
                title: 'Promo Id',
                data: 'promoRefId',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Over Budget Status',
                data: 'overBudgetStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Initiator',
                data: 'createBy',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 8,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'align-middle'
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_dn_multi_print_list = $('#dt_dn_multi_print_list').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        order: [[1, 'asc']],
        processing: true,
        serverSide: false,
        paging: false,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                visible: false,
            },
            {
                targets: 1,
                data: 'refId',
                width: 170,
                title: 'DN Number',
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'DN Description',
                data: 'activityDesc',
                width: 350,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'DPP',
                data: 'dpp',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0);
                }
            },
            {
                targets: 4,
                title: 'Category',
                data: 'dnCategory',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 5,
                title: 'Promo Id',
                data: 'promoRefId',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Over Budget Status',
                data: 'overBudgetStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Initiator',
                data: 'createBy',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 8,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'align-middle'
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    // $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
    //     let strmessage = e.jqXHR.responseJSON.message
    //     if(strmessage==="") strmessage = "Please contact your vendor"
    //     Swal.fire({
    //         text: strmessage,
    //         icon: "warning",
    //         buttonsStyling: !1,
    //         confirmButtonText: "OK",
    //         customClass: {confirmButton: "btn btn-optima"},
    //         closeOnConfirm: false,
    //         showLoaderOnConfirm: false,
    //         closeOnClickOutside: false,
    //         closeOnEsc: false,
    //         allowOutsideClick: false,
    //     });
    // };
});

$('#dt_dn_multi_print_search').on('keyup', function () {
    dt_dn_multi_print.search(this.value).draw();
});

$('#dt_dn_multi_print_list_search').on('keyup', function () {
    dt_dn_multi_print_list.search(this.value).draw();
});

$('#dt_dn_multi_print_view').on('click', function (){
    let btn = document.getElementById('dt_dn_multi_print_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_subaccount = (($('#filter_subaccount').val() == "") ? 0 : $('#filter_subaccount').val());
    let url = "/dn/multi-print/list/paginate/filter?period=" + filter_period + "&subAccountId=" + filter_subaccount ;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_dn_multi_print.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#dt_dn_multi_print').on( 'dblclick', 'tr', function () {
    let data = dt_dn_multi_print.row(this).data();

    let idx = dt_dn_multi_print_list
        .columns(0)
        .data()
        .eq(0)
        .indexOf(data.id);

    if (idx === -1) {
        let dbresult = []

        dbresult["id"] = data.id
        dbresult["refId"] = data.refId
        dbresult["activityDesc"] = data.activityDesc
        dbresult["dpp"] = data.dpp
        dbresult["dnCategory"] = data.dnCategory
        dbresult["promoRefId"] = data.promoRefId
        dbresult["overBudgetStatus"] = data.overBudgetStatus
        dbresult["createBy"] = data.createBy
        dbresult["lastStatus"] = data.lastStatus

        dt_dn_multi_print_list.row.add(dbresult).draw();
    };
});

$('#dt_dn_multi_print_list').on( 'dblclick', 'tr', function () {
    var tr = $(this).closest('tr');

    let trindex = dt_dn_multi_print_list.row(tr).index();
    dt_dn_multi_print_list.row(trindex).remove().draw();
});

$('#btn_print').on('click', function () {
    let rowData = dt_dn_multi_print_list
        .rows()
        .data();
    let dataSend = []
    $.each(rowData, function (index, rowId) {
        dataSend.push(rowId.id)
    });
    if (dataSend.length > 0) {
        let url = "/dn/multi-print/print-pdf?id=" + encodeURIComponent(JSON.stringify(dataSend));

        let a = document.createElement("a");
        a.href = url;
        let evt = document.createEvent("MouseEvents");
        //the tenth parameter of initMouseEvent sets ctrl key
        evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
            true, false, false, false, 0, null);
        a.dispatchEvent(evt);
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please select one or more items",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

const getListSubAccount = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/multi-print/list/sub-account",
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
                $('#filter_subaccount').select2({
                    placeholder: "Select a Sub Account",
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

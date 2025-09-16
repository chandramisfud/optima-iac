'use strict';

var dt_invoice_creation;
var swalTitle = "Invoice Creation";
heightContainer = 280;
var dataFilter, url_datatable;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#filter_date').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    getListEntity();

    dt_invoice_creation = $('#dt_invoice_creation').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[2, 'asc']],
        ajax: {
            url: '/dn/invoice/list?filter_date=' + $('#filter_date').val() + '&entityId=' + $('#filter_entity').val(),
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
                width: 20,
                title: '',
                orderable: false,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-150px w-md-150px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start edit-record" href="/dn/invoice/form?method=update&id=' + data + '"><i class="fa fa-edit fs-6"></i> Edit Invoice</a>\
                                <a class="dropdown-item text-start print-record" href="/dn/invoice/print-pdf?&id=' + data + '" target="_blank"><i class="fa fa-print fs-6"></i> Print Invoice</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'Invoice Number',
                data: 'refId',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'Invoice Date',
                data: 'invoiceDate',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 3,
                title: 'Description',
                data: 'invoiceDesc',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'DPP',
                data: 'dpp',
                width: 170,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0);
                }
            },
            {
                targets: 5,
                title: 'PPN',
                data: 'ppn',
                width: 170,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0);
                }
            },
            {
                targets: 6,
                title: 'Amount',
                data: 'invoiceAmount',
                width: 170,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data,0);
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

$('#dt_invoice_creation_search').on('keyup', function () {
    dt_invoice_creation.search(this.value).draw();
});

$('#btn_create').on('click', function () {
    checkFormAccess('create_rec', '', '/dn/invoice/form', '')
});

$('#dt_invoice_creation_view').on('click', function (){
    let btn = document.getElementById('dt_invoice_creation_view');

    let filter_date = ($('#filter_date').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? 0;

    let url = "/dn/invoice/list?filter_date=" + filter_date + '&entityId=' + filter_entity;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_invoice_creation.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/invoice/list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].shortDesc + ' - ' +result.data[j].longDesc,
                        longDesc: result.data[j].longDesc,
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

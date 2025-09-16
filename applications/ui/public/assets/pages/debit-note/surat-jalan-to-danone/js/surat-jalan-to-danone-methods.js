'use strict';

var dt_surat_jalan;
var swalTitle = "Surat Jalan";
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

    dt_surat_jalan = $('#dt_surat_jalan').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[2, 'asc']],
        ajax: {
            url: '/dn/surat-jalan-danone/list?senddate=' + $('#filter_date').val(),
            type: 'get',
        },
        processing: true,
        serverSide: false,
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
                width: 5,
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
                                <a class="dropdown-item text-start print-record" href="/dn/surat-jalan-danone/print-pdf?&id=' + data + '" target="_blank"><i class="fa fa-print fs-6"></i> Print Surat Jalan</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                width: 5,
                className: 'dt-control align-middle',
                orderable: false,
                data: null,
                defaultContent: '',
            },
            {
                targets: 2,
                title: 'SP Number',
                data: 'refId',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Distributor',
                data: 'distributorDesc',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Entity',
                data: 'entityDesc',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Create On',
                data: 'createOn',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDate(data);
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

    $('#dt_surat_jalan tbody').on('click', 'td.dt-control', function () {
        let tr = $(this).closest('tr');
        let row = dt_surat_jalan.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        } else {
            // Open this row
            row.child(format(row.data().dnId[0])).show();
            tr.addClass('shown');
        }
    });
});

$('#dt_surat_jalan_search').on('keyup', function () {
    dt_surat_jalan.search(this.value).draw();
});

$('#dt_surat_jalan_view').on('click', function (){
    let btn = document.getElementById('dt_surat_jalan_view');

    let filter_date = ($('#filter_date').val()) ?? "";
    let url = "/dn/surat-jalan-danone/list?senddate=" + filter_date;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_surat_jalan.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

const format = (d) => {
    return '<table class="table table-sm table-condensed" style="margin-left:20px;">' +
        '<thead><tr>' +
        '<td>Debit Note</td><td>Promo Number</td><td>Account</td><td>Activity</td><td>Memorial Doc No</td><td>Total Claim</td>' +
        '</tr></thead>' +
        '<tr>' +
        '<td>' + d.dnNumber + '</td><td>' + d.promoNumber + '</td><td>' + d.accountDesc + '</td><td>' + d.activityDesc + '</td><td>' + d.memDocNo + '</td><td>' + formatMoney(d.totalClaim,0) +
        '</tr>' +
        '<tr><td></td><td></td><td></td><td></td><td></td><td></td></tr>' +
        '</table>';
}

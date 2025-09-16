'use strict';

var dt_dn_reassignment;
var swalTitle = "Debit Note [Reassignment]";
heightContainer = 280;

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_dn_reassignment = $('#dt_dn_reassignment').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/reassignment/list',
            type: 'GET',
        },
        processing: true,
        serverSide: true,
        paging: true,
        ordering: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                data: 'id',
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
                                <a class="dropdown-item text-start reassignment-record" href="/dn/reassignment/form?method=update&id=' + data + '"><i class="fa fa-edit fs-6"></i> DN Reassignment</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'DN Number',
                data: 'refId',
                width: 150,
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'Sub Account',
                data: 'subAccount',
                width: 120,
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 120,
                className: 'align-middle',
            },
            {
                targets: 4,
                title: 'DN Description',
                data: 'activityDesc',
                width: 400,
                className: 'align-middle',
            },
            {
                targets: 5,
                title: 'DPP',
                data: 'dpp',
                width: 170,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 6,
                title: 'Last Status',
                data: 'lastStatus',
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
});

$('#dt_dn_reassignment_search').on('keyup', function () {
    dt_dn_reassignment.search(this.value).draw();
});

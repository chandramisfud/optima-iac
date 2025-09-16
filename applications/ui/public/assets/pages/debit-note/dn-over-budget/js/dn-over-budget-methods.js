'use strict';

var dt_dn_over_budget;
var swalTitle = "Debit Note [Refresh]";
heightContainer = 280;
let el_dt_dn_over_budget = $('#dt_dn_over_budget');

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_dn_over_budget = el_dt_dn_over_budget.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/over-budget/list',
            type: 'GET',
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
                            <div class="menu menu-sub menu-sub-dropdown w-200px w-md-200px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start refresh-record" href="#"><i class="fa fa-edit fs-6"></i> DN Refresh Over Budget</a>\
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
                title: 'Promo ID',
                data: 'promoRefId',
                width: 120,
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Activity',
                data: 'activityDesc',
                width: 400,
                className: 'align-middle',
            },
            {
                targets: 4,
                title: 'Total Claim',
                data: 'totalClaim',
                width: 100,
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
                targets: 5,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 6,
                title: 'Last Update',
                data: 'lastUpdate',
                width: 100,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 7,
                title: 'Over Budget Status',
                data: 'overBudgetStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    el_dt_dn_over_budget.on('click', '.refresh-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_dn_over_budget.row(tr).data();

        Swal.fire({
            title: swalTitle,
            text: 'Are you sure to refresh ' + trdata.refId + '?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#AAAAAA',
            allowOutsideClick: false,
            allowEscapeKey: false,
            cancelButtonText: 'No, cancel',
            confirmButtonText: "Yes, refresh it",
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                let formData = new FormData();
                formData.append('promoId', trdata.promoId);
                $.get('/refresh-csrf').done(function(data) {
                    $('meta[name="csrf-token"]').attr('content', data)
                    $.ajaxSetup({
                        headers: {
                            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                        }
                    });
                    $.ajax({
                        url: '/dn/over-budget/refresh',
                        data: formData,
                        type: 'POST',
                        async: true,
                        dataType: 'JSON',
                        cache: false,
                        contentType: false,
                        processData: false,
                        beforeSend: function () {
                            blockUI.block();
                        },
                        success: function (result) {
                            if (!result.error) {
                                Swal.fire({
                                    title: swalTitle,
                                    text: "Data has been refresh",
                                    icon: "success",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    confirmButtonText: "OK",
                                    customClass: {confirmButton: "btn btn-optima"}
                                }).then(function () {
                                    dt_dn_over_budget.ajax.reload();
                                });
                            } else {
                                Swal.fire({
                                    title: swalTitle,
                                    text: result.message,
                                    icon: "warning",
                                    buttonsStyling: !1,
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    confirmButtonText: "OK",
                                    customClass: {confirmButton: "btn btn-optima"}
                                });
                            }
                        },
                        complete: function () {
                            blockUI.release();
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(errorThrown)
                            Swal.fire({
                                title: swalTitle,
                                text: textStatus,
                                icon: "error",
                                buttonsStyling: !1,
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                confirmButtonText: "OK",
                                customClass: {confirmButton: "btn btn-optima"}
                            });
                        }
                    });
                });
            }
        });
    });
    $('#dt_dn_over_budget_search').on('keyup', function () {
        dt_dn_over_budget.search(this.value).draw();
    });

});

'use strict';

var dt_confirm_paid, arrChecked = [], sumAmount = 0;
var swalTitle = "Confirm Paid";
heightContainer = 305;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_confirm_paid = $('#dt_confirm_paid').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/paid/list',
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 10,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center align-middle',
                checkboxes: {
                    'selectRow': false,
                },
            },
            {
                targets: 1,
                title: 'Dn Number',
                data: 'refId',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'DN Description',
                data: 'activityDesc',
                width: 500,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'DPP',
                data: 'dpp',
                width: 150,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatMoney(data,0);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 5,
                title: 'Last Update',
                data: 'lastUpdate',
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
        ],
        initComplete: function (settings, json) {
            $('#dt_confirm_paid_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_confirm_paid.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        arrChecked.push({
                            id: data[i].id,
                            totalClaim: parseFloat(data[i].totalClaim)
                        });
                    }
                } else {
                    arrChecked = [];
                }
                sumAmount = 0;
                if (arrChecked.length > 0) {
                    for (let i = 0; i < arrChecked.length; i++) {
                        sumAmount += parseFloat(arrChecked[i].totalClaim);
                    }
                }
                $('#tot').html(formatMoney(sumAmount.toString(), 0));
                $('#count').html(arrChecked.length);
            });
        },
        drawCallback: function (settings, json) {

        },
    });

    $('.dt-footer').html("<div>Total Selected : <span id='count'>0</span>&nbsp&nbsp&nbsp Total Claim : <span id='tot'>0</span></div>");

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        console.log(e.jqXHR);
        let strmessage = e.jqXHR.responseJSON.message
        if (strmessage === "") strmessage = "Please contact your vendor"
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

    $('#dt_confirm_paid').on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_confirm_paid.row(this.closest('tr')).data();
        let totalClaim = rows.totalClaim;
        if (this.checked) {
            arrChecked.push({
                id: rows.id,
                totalClaim: parseFloat(rows.totalClaim)
            });
            sumAmount += parseFloat(totalClaim);
        } else {
            sumAmount -= parseFloat(totalClaim);
            let index = arrChecked.findIndex(p => p.id === rows.id);
            if (index > -1) {
                arrChecked.splice(index, 1);
            }
        }
        $('#tot').html(formatMoney(sumAmount.toString(), 0));
        $('#count').html(arrChecked.length);
    });
});

$('#dt_confirm_paid_search').on('keyup', function() {
    dt_confirm_paid.search(this.value).draw();
});

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    let dataRowsChecked = [];
    let rows_selected = dt_confirm_paid.column(0).checkboxes.selected();
    $.each(rows_selected, function (index, value) {
        dataRowsChecked.push({
            dnid: value,
        });
    });

    if (dataRowsChecked.length > 0) {
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;

        Swal.fire({
            title: 'Payment Date',
            html: '<div class="col-lg-12 col-12">\
                        <input type="text" class="form-control form-control-sm" name="paymentDate" id="paymentDate" placeholder="Payment Date" value="" autocomplete="off"/>\
                    </div>',
            showCancelButton: true,
            allowOutsideClick: false,
            allowEscapeKey: false,
            cancelButtonText: 'Cancel',
            confirmButtonText: 'OK',
            reverseButtons: true,
            didOpen: function () {
                $('#paymentDate').flatpickr({
                    altFormat: "Y-m-d",
                    dateFormat: "Y-m-d",
                    disableMobile: "true",
                });
            },
        }).then((result) => {
            if (result.isConfirmed) {
                var paymentDate = $('#paymentDate').val()
                if(paymentDate === "") {
                    Swal.fire({
                        title: "Please input payment date",
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        e.setAttribute("data-kt-indicator", "off");
                        e.disabled = !1;
                    });
                } else {
                    let formData = new FormData();
                    formData.append('dnId', JSON.stringify(dataRowsChecked));
                    formData.append('paymentDate', paymentDate);
                    let url = "/dn/paid/submit";

                    $.ajax({
                        url         : url,
                        data        : formData,
                        type        : 'POST',
                        async       : true,
                        dataType    : 'JSON',
                        cache       : false,
                        contentType : false,
                        processData : false,
                        beforeSend: function() {
                            e.setAttribute("data-kt-indicator", "on");
                            e.disabled = !0;
                        },
                        success: function(result, status, xhr, $form) {
                            if (!result.error) {
                                Swal.fire({
                                    text: result.message,
                                    icon: "success",
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"}
                                }).then(function () {
                                    dt_confirm_paid.ajax.reload();
                                });
                            } else {
                                Swal.fire({
                                    title: swalTitle,
                                    text: result.message,
                                    icon: "error",
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"}
                                });
                            }
                        },
                        complete: function() {
                            e.setAttribute("data-kt-indicator", "off");
                            e.disabled = !1;
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(jqXHR.message)
                            Swal.fire({
                                title: swalTitle,
                                text: "Failed to save data, an error occurred in the process",
                                icon: "error",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            });
                        }
                    });
                }
            } else {
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
            }
        })
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

const resetForm = () => {
    arrChecked = [];
    sumAmount = 0;
    if (arrChecked.length > 0) {
        for (let i = 0; i < arrChecked.length; i++) {
            sumAmount += parseFloat(arrChecked[i].totalClaim);
        }
    }
    $('#tot').html(formatMoney(sumAmount.toString(), 0));
    $('#count').html(arrChecked.length);
}

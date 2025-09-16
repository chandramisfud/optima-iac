'use strict';

let dt_send_to_ho, arrChecked = [], sumAmount = 0;
let swalTitle = "Debit Note [Send To HO]";
let elDtSendToHO = $('#dt_send_to_ho');
heightContainer = 305;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    dt_send_to_ho = elDtSendToHO.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/send-to-ho/list',
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
                render: function (data, type, full) {
                    if (type === 'display') {
                        if (full.docCount === 0) {
                            data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off" disabled>'
                        } else {
                            data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off">';
                        }
                    }
                    return data;
                }
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
                title: 'Activity',
                data: 'activityDesc',
                width: 500,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Memorial Doc. No',
                data: 'memDocNo',
                width: 150,
                className: 'text-nowrap align-middle'
            },
            {
                targets: 5,
                title: 'Internal Doc. No',
                data: 'intDocNo',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Total Claim',
                data: 'totalClaim',
                width: 200,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data) {
                        return formatMoney(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 7,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Last Update',
                data: 'lastUpdate',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 9,
                title: 'Tax Level',
                data: 'materialNumber',
                width: 300,
                className: 'text-nowrap align-middle',
                render: function ( data, type, full) {
                    if(data == null){
                        return ''
                    }else{
                        return data + ' - ' + full.taxLevel
                    }

                }
            },
            {
                targets: 10,
                title: 'Remark by Sales',
                data: 'remarkSales',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 11,
                title: 'Sales Validation Status',
                data: 'salesValidationStatus',
                width: 200,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function () {
            $('#dt_send_to_ho_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_send_to_ho.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        if (data[i].docCount !== 0) {
                            arrChecked.push({
                                id: data[i].id,
                                totalClaim: parseFloat(data[i].totalClaim)
                            });
                        }
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

    $.fn.dataTable.ext.errMode = function (e) {
        console.log(e.jqXHR);
        let strMessage = e.jqXHR['responseJSON']['message'];
        if (strMessage === "") strMessage = "Please contact your vendor"
        Swal.fire({
            text: strMessage,
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

    elDtSendToHO.on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_send_to_ho.row(this.closest('tr')).data();
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

$('#dt_send_to_ho_search').on('keyup', function() {
    dt_send_to_ho.search(this.value).draw();
});

$('#btn_submit').on('click', async function () {
    let e = document.querySelector("#btn_submit");
    let dataRowsChecked = [];
    $.each(arrChecked, function (index, value) {
        dataRowsChecked.push({
            dnid: value.id,
        });
    });
    let messageError = "";
    if (dataRowsChecked.length > 0) {
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        blockUI.block();
        let error = false;
        for (let i=1; i <= dataRowsChecked.length; i++) {
            let dataRow = dataRowsChecked[i-1];
            let arrDN = [dataRow];
            let res_method = await submit(arrDN);
            if (res_method.error) {
                error = true;
                messageError = res_method.message;
                break;
            }
            if (!res_method.error) {
                messageError = res_method.message;
                if (i === dataRowsChecked.length) {
                    let formData = new FormData();
                    formData.append('dnid', JSON.stringify(dataRowsChecked));
                    let url = "/dn/send-to-ho/submit-sj";
                    $.ajax({
                        type        : 'POST',
                        url         : url,
                        data        : formData,
                        dataType    : "JSON",
                        async       : true,
                        cache       : false,
                        contentType : false,
                        processData : false,
                        success: function (result) {
                            if (!result.error) {
                                blockUI.release();
                                e.setAttribute("data-kt-indicator", "off");
                                e.disabled = !1;
                                Swal.fire({
                                    text: result['message'],
                                    icon: "success",
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"}
                                }).then(function () {
                                    $('#tot').html(0);
                                    $('#count').html(0);
                                    dt_send_to_ho.ajax.reload();
                                });
                            } else {
                                Swal.fire({
                                    title: swalTitle,
                                    text: result['message'],
                                    icon: "warning",
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: { confirmButton: "btn btn-optima" }
                                }).then(function () {
                                    dt_send_to_ho.ajax.reload();
                                });
                            }
                        },
                        complete: function() {

                        },
                        error: function (jqXHR) {
                            if (jqXHR.status === 403) {
                                Swal.fire({
                                    title: swalTitle,
                                    text: jqXHR['responseJSON'].message,
                                    icon: "warning",
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: { confirmButton: "btn btn-primary" }
                                }).then(function () {
                                    window.location.href = '/login-page';
                                });
                            }
                        }
                    });
                }
            }
        }
        if (error) {
            blockUI.release();
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
            Swal.fire({
                title: swalTitle,
                text: messageError,
                icon: "warning",
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: { confirmButton: "btn btn-optima" }
            }).then(function () {
                dt_send_to_ho.ajax.reload();
            });
        }
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

const submit = (arrDN) => {
    return new Promise(function (resolve) {
        let formData = new FormData();
        formData.append('dnid', JSON.stringify(arrDN));
        let url = "/dn/send-to-ho/update";
        $.ajax({
            type        : 'POST',
            url         : url,
            data        : formData,
            dataType    : "JSON",
            async       : true,
            cache       : false,
            contentType : false,
            processData : false,
            success: function (result) {
                return resolve(result);
            },
            complete: function() {

            },
            error: function (jqXHR) {
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR['responseJSON'].message,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-primary" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                return resolve(false);
            }
        });
    });
}

const getListEntity = () => {
    $.ajax({
        url         : "/tools/xml-payment-reset/list/entity",
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
        error: function (jqXHR)
        {
            console.log(jqXHR.responseText);
        }
    });
}

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

'use strict';

var dt_received_by_danone, arrChecked = [], sumAmount = 0;
var swalTitle = "Debit Note [Received By Danone]";
heightContainer = 305;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListEntity()

    dt_received_by_danone = $('#dt_received_by_danone').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/received-by-danone/list',
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
                data: 'id',
                width: 10,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center align-middle',
                render: function (data, type, full, meta) {
                    return '\
                            <div class="dropdown">\
                                <a href="/dn/received-by-danone/form?id=' + data + '" class="btn btn-optima btn-sm btn-clean" title="Reject" ">\
                                    <i class="la la-mail-reply"></i>\
                                </a>\
                            </div>\
                        ';
                },
            },
            {
                targets: 2,
                title: 'DN Number',
                data: 'refId',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Activity',
                data: 'activityDesc',
                width: 500,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Total Claim',
                data: 'totalClaim',
                width: 170,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatMoney(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 6,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Last Update',
                data: 'lastUpdate',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 8,
                title: 'SP No',
                data: 'sp_principal',
                width: 170,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: 'TaxLevel',
                data: 'materialNumber',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data === null || data === "" || full.taxLevel === null) {
                        return ''
                    } else {
                        return data + ' - ' + full.taxLevel
                    }
                }
            },
            {
                targets: 10,
                title: 'Remark by Sales',
                data: 'remarkSales',
                width: 100,
                className: 'text-nowrap align-middle'
            },
            {
                targets: 11,
                title: 'Sales Validation Status',
                data: 'salesValidationStatus',
                width: 100,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function (settings, json) {
            $('#dt_received_by_danone_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_received_by_danone.rows( { filter : 'applied'} ).data();
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

    $('#dt_received_by_danone').on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_received_by_danone.row(this.closest('tr')).data();
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

$('#dt_received_by_danone_search').on('keyup', function() {
    dt_received_by_danone.search(this.value).draw();
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_received_by_danone_view').on('click', function (){
    let btn = document.getElementById('dt_received_by_danone_view');

    let filter_entity = ($('#filter_entity').val()) ?? 0;
    let filter_distributor = ($('#filter_distributor').val()) ?? 0;

    let url = "/dn/received-by-danone/list?entityId=" + filter_entity + '&distributorId=' + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_received_by_danone.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_submit').on('click', async function () {
    let e = document.querySelector("#btn_submit");
    let dataRowsChecked = [];
    let rows_selected = dt_received_by_danone.column(0).checkboxes.selected();
    $.each(rows_selected, function (index, value) {
        dataRowsChecked.push({
            dnid: value,
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
                    blockUI.release();
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    Swal.fire({
                        text: messageError,
                        icon: "success",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_received_by_danone.ajax.reload();
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
                dt_received_by_danone.ajax.reload();
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
        let payment_date = formatDate(new Date());
        let formData = new FormData();
        formData.append('dnid', JSON.stringify(arrDN));
        formData.append('payment_date', payment_date);
        let url = "/dn/received-by-danone/update";
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
        url         : "/dn/received-by-danone/list/entity",
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

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/received-by-danone/list/distributor/entity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {entityId: entityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_distributor').select2({
                    placeholder: "Select a Distributor",
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

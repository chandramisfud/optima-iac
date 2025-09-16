'use strict';

var dt_invoice_notif, arrChecked = [], sumAmount = 0;
var swalTitle = "Invoice Notification";
heightContainer = 305;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListEntity()

    dt_invoice_notif = $('#dt_invoice_notif').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/invoice-notif/list',
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
                            <a href="/dn/invoice-notif/form?id=' + data + '" class="btn btn-optima btn-sm btn-clean" title="Reject" ">\
                                <i class="la la-mail-reply"></i>\
                            </a>\
                        </div>\
                    ';
                },
            },
            {
                targets: 2,
                title: 'Dn Number',
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
                title: 'DN Description',
                data: 'activityDesc',
                width: 500,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'DPP',
                data: 'dpp',
                width: 150,
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
                    if (data) {
                        return formatDate(data)
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 8,
                title: 'TaxLevel',
                data: 'materialNumber',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return data + ' - ' + full.taxLevel;
                    } else {
                       return "";
                    }
                }
            },
        ],
        initComplete: function (settings, json) {
            $('#dt_invoice_notif_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_invoice_notif.rows( { filter : 'applied'} ).data();
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

    $('#dt_invoice_notif').on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_invoice_notif.row(this.closest('tr')).data();
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

$('#dt_invoice_notif_search').on('keyup', function() {
    dt_invoice_notif.search(this.value).draw();
});

$('#dt_invoice_notif_view').on('click', function (){
    let btn = document.getElementById('dt_invoice_notif_view');
    let filter_entity = (($('#filter_entity').val() == "" || $('#filter_entity').val() == null) ? 0 : $('#filter_entity').val());
    let filter_distributor = (($('#filter_distributor').val() == "" || $('#filter_distributor').val() == null) ? 0 : $('#filter_distributor').val());

    let url = "/dn/invoice-notif/list?entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_invoice_notif.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#btn_submit').on('click', async function () {
    let e = document.querySelector("#btn_submit");
    if (arrChecked.length > 0) {
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        $('#d_progress').removeClass('d-none');
        blockUI.block();
        let perc = 0;
        let error = true;
        for (let i=1; i <= arrChecked.length; i++) {
            let dataRow = arrChecked[i-1];
            let res_method = await submit(dataRow.id);
            if (!res_method) {
                error = false;
                break;
            }
            if (res_method) {
                perc = ((i / arrChecked.length) * 100).toFixed(0);
                $('#text_progress').text(perc.toString() + '%');
                let progress_import = $('#progress_bar');
                progress_import.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());
                if (i === arrChecked.length) {
                    progress_import.css('width', '0%').attr('aria-valuenow', '0');
                    blockUI.release();
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    $('#d_progress').addClass('d-none');
                    Swal.fire({
                        title: swalTitle,
                        text: 'DN Validated',
                        icon: "success",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_invoice_notif.ajax.reload();
                        resetForm();
                    });
                }
            }
        }
        if (!error) {
            let progress_import = $('#progress_bar');
            progress_import.css('width', '0%').attr('aria-valuenow', '0');
            blockUI.release();
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
            $('#d_progress').addClass('d-none');
            Swal.fire({
                title: swalTitle,
                text: "Validate Failed",
                icon: "warning",
                allowOutsideClick: false,
                allowEscapeKey: false,
                confirmButtonText: "OK",
                customClass: { confirmButton: "btn btn-optima" }
            });
        }
    } else {
        blockUI.release();
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
        $('#d_progress').addClass('d-none');
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

const submit = (id) =>  {
    return new Promise(function (resolve, reject) {
        let dataRowsChecked = [];
        dataRowsChecked.push({
            dnid: id,
        });
        let formData = new FormData();
        formData.append('dnid', JSON.stringify(dataRowsChecked));
        $.ajax({
            type        : 'POST',
            url         : "/dn/invoice-notif/update",
            data        : formData,
            dataType    : "JSON",
            async       : true,
            cache       : false,
            contentType : false,
            processData : false,
            success: function (result) {
                if (!result.error) {
                    return resolve(true);
                } else {
                    return resolve(false);
                }
            },
            complete: function() {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR.responseJSON.message,
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
        url         : "/dn/invoice-notif/list/entity",
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
            url         : "/dn/invoice-notif/list/distributor/entity-id",
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

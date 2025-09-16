'use strict';

let dt_promo_cancel_request, arrChecked = [], dialerObject;
let swalTitle = "Promo Cancel Request";
heightContainer = 290;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });
    dialerObject.setValue(new Date().getFullYear());

    getListEntity();

    dt_promo_cancel_request = $('#dt_promo_cancel_request').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/promo/cancel-request/list?period=' + $('#filter_period').val(),
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: true,
        ordering: false,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'PromoId',
                width: 20,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center',
                checkboxes: {
                    'selectRow': false
                },
                render: function (data, type, full, meta) {
                    data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off">';
                    return data;
                },
            },
            {
                targets: 1,
                title: '',
                data: 'PromoId',
                searchable: false,
                width: 10,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    let startYear = new Date(full['StartPromo']).getFullYear();
                    return `
                        <div class="me-0">
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>
                            </a>
                            <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">
                                <a class="dropdown-item text-start cancel-record" href="/promo/cancel-request/form-cancel?id=${full['PromoId']}&c=${full['categoryShortDesc']}&sy=${startYear}&recon=${full['reconciled'] ? 1 : 0}"><i class="fa fa-edit fs-6"></i> Cancel Promo</a>
                            </div>
                        </div>
                    `;
                }
            },
            {
                targets: 2,
                title: 'Req ID',
                data: 'RequestId',
                width: 60,
                className: 'text-nowrap align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'Req Date',
                data: 'RequestDate',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data, type, full, meta) {
                    return formatDate(data) ?? "";
                }
            },
            {
                targets: 4,
                title: 'Promo ID',
                data: 'RefId',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Activity Description',
                data: 'ActivityDesc',
                width: 200,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 6,
                title: 'TS Code',
                data: 'TsCoding',
                width: 200,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 7,
                title: 'Aging',
                data: 'AgingApproval',
                width: 50,
                className: 'align-middle text-end text-nowrap',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0) ?? "";
                }
            },
            {
                targets: 8,
                title: 'Start Promo',
                data: 'StartPromo',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data, type, full, meta) {
                    return formatDate(data) ?? "";
                }
            },
            {
                targets: 9,
                title: 'End Promo',
                data: 'EndPromo',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data, type, full, meta) {
                    return formatDate(data) ?? "";
                }
            },
            {
                targets: 10,
                title: 'Initiator',
                data: 'Initiator',
                width: 100,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 11,
                title: 'Investment',
                data: 'Investment',
                width: 100,
                className: 'align-middle text-end text-nowrap',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0) ?? "";
                }
            },
            {
                targets: 12,
                title: 'Cancel Reason',
                data: 'CancelReason',
                width: 100,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 13,
                title: 'Promo Planning ID',
                data: 'PromoPlanId',
                width: 100,
                visible: false,
                className: 'align-middle text-nowrap',
            },
        ],
        initComplete: function( settings, json ) {
            $('#dt_promo_cancel_request_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_promo_cancel_request.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        arrChecked.push({
                            promoId: data[i].PromoId,
                            promoPlanId : data[i].PromoPlanId,
                            initiator : data[i].Initiator
                        });
                    }
                } else {
                    arrChecked = [];
                }
            });
        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            title: swalTitle,
            text: strmessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            closeOnConfirm: false,
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,
        });
    };

    $('#dt_promo_cancel_request').on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_promo_cancel_request.row(this.closest('tr')).data();
        if (this.checked) {
            arrChecked.push({
                promoId: rows.PromoId,
                refId: rows.RefId,
                promoPlanId : rows.PromoPlanId,
                initiator : rows.Initiator
            });
        } else {
            let index = arrChecked.findIndex(p => p.PromoId === rows.PromoId);
            if (index > -1) {
                arrChecked.splice(index, 1);
            }
        }
    });
});

$('#dt_promo_cancel_request_search').on('keyup', function() {
    dt_promo_cancel_request.search(this.value, false, false).draw();
});

$('#dt_promo_cancel_request_view').on('click', function (){
    let btn = document.getElementById('dt_promo_cancel_request_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/promo/cancel-request/list?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_cancel_request.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#btn_approve').on('click', async function () {
    let messageError = "";
    let e = document.querySelector("#btn_approve");
    if (arrChecked.length > 0) {
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        $('#d_progress').removeClass('d-none');
        blockUI.block();
        let perc = 0;
        let error = false;
        for (let i=1; i <= arrChecked.length; i++) {
            let dataRow = arrChecked[i-1];
            let res_method = await approve(dataRow.promoId, dataRow.refId, dataRow.promoPlanId, dataRow.initiator, '');
            if (res_method.error) {
                error = true;
                messageError = res_method.message;
                break;
            }
            if (!res_method.error) {
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
                        text: 'Complete',
                        icon: "success",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_promo_cancel_request.ajax.reload();
                        resetForm();
                    });
                }
            }
        }
        if (error) {
            let progress_import = $('#progress_bar');
            progress_import.css('width', '0%').attr('aria-valuenow', '0');
            blockUI.release();
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
            $('#d_progress').addClass('d-none');
            Swal.fire({
                title: swalTitle,
                text: messageError,
                icon: "warning",
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: { confirmButton: "btn btn-optima" }
            }).then(function () {
                dt_promo_cancel_request.ajax.reload();
                resetForm();
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

$('#btn_sendback').on('click', async function () {
    let messageError = "";
    let e = document.querySelector("#btn_sendback");
    if (arrChecked.length > 0) {
        Swal.fire({
            title: 'Sendback Notes',
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            confirmButtonText: 'Sendback',
            allowOutsideClick: false,
            allowEscapeKey: false,
        }).then(async function (result) {
            if (result.isConfirmed) {
                let notes = result.value;

                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
                $('#d_progress').removeClass('d-none');
                blockUI.block();
                let perc = 0;
                let error = false;
                for (let i = 1; i <= arrChecked.length; i++) {
                    let dataRow = arrChecked[i - 1];
                    let res_method = await sendBack(dataRow.promoId, dataRow.refId, dataRow.promoPlanId, dataRow.initiator, notes);
                    if (res_method.error) {
                        error = true;
                        messageError = res_method.message;
                        break;
                    }
                    if (!res_method.error) {
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
                                text: 'Complete',
                                icon: "success",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(function () {
                                dt_promo_cancel_request.ajax.reload();
                                resetForm();
                            });
                        }
                    }
                }
                if (error) {
                    let progress_import = $('#progress_bar');
                    progress_import.css('width', '0%').attr('aria-valuenow', '0');
                    blockUI.release();
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                    $('#d_progress').addClass('d-none');
                    Swal.fire({
                        title: swalTitle,
                        text: messageError,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_promo_cancel_request.ajax.reload();
                        resetForm();
                    });
                }
            }
        });
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

const approve = (promoId, promoRefId, promoPlanId, initiator, notes) =>  {
    return new Promise(function (resolve, reject) {
        let formData = new FormData();
        formData.append('promoId', promoId);
        formData.append('promoRefId', promoRefId);
        formData.append('promoPlanningId', promoPlanId);
        formData.append('createdBy', initiator);
        formData.append('notes', notes);

        $.ajax({
            type        : 'POST',
            url         : "/promo/cancel-request/approve",
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

const sendBack = (promoId, promoRefId, promoPlanId, initiator, notes) =>  {
    return new Promise(function (resolve, reject) {
        let formData = new FormData();
        formData.append('promoId', promoId);
        formData.append('promoRefId', promoRefId);
        formData.append('promoPlanningId', promoPlanId);
        formData.append('createdBy', initiator);
        formData.append('notes', notes);
        $.ajax({
            type        : 'POST',
            url         : "/promo/cancel-request/reject",
            data        : formData,
            dataType    : "JSON",
            async       : true,
            cache       : false,
            contentType : false,
            processData : false,
            success: function (result) {
                return resolve(result);
            },
            complete: function(result) {

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
        url         : "/promo/cancel-request/list/entity",
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
            url         : "/promo/cancel-request/list/distributor",
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
}

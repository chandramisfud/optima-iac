'use strict';

var dt_budget_approval, arrChecked = [], sumAmount = 0;
var swalTitle = "Budget Approval";
heightContainer = 295;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    let dialerObject = new KTDialer(dialerElement, {
        min: 1900,
        step: 1,
    });
    dialerObject.setValue(new Date().getFullYear());

    getListEntity();
    getListChannel();

    dt_budget_approval = $('#dt_budget_approval').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/budget/approval/list/paginate/filter?period=' + $('#filter_period').val(),
            type: 'get',
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
        order: [[7, 'desc']],
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 25,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center',
                checkboxes: {
                    'selectRow': false
                },
            },
            {
                targets: 1,
                title: 'Period',
                data: 'periode',
                width: 80,
                className: 'align-middle',
            },
            {
                targets: 2,
                title: 'Ref ID',
                data: 'refId',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Description',
                data: 'longDesc',
                width: 300,
                className: 'align-middle',
            },
            {
                targets: 4,
                title: 'Amount',
                data: 'budgetAmount',
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
                title: 'Distributor',
                data: 'distributorName',
                width: 250,
                className: 'align-middle'
            },
            {
                targets: 6,
                title: 'Owner',
                data: 'ownerId',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 7,
                title: 'Status Approval',
                data: 'statusApproval',
                width: 110,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    if (data ==='AP2') {
                        return 'Approved';
                    } else {
                        return 'Waiting Approval';
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {
            $('#dt_budget_approval_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_budget_approval.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        arrChecked.push({
                            id: data[i].id,
                            budgetAmount: parseFloat(data[i].budgetAmount)
                        });
                    }
                } else {
                    arrChecked = [];
                }
                sumAmount = 0;
                if (arrChecked.length > 0) {
                    for (let i = 0; i < arrChecked.length; i++) {
                        sumAmount += parseFloat(arrChecked[i].budgetAmount);
                    }
                }
                $('#tot').html(formatMoney(sumAmount.toString(), 0));
                $('#count').html(arrChecked.length);
            });
        },
        drawCallback: function( settings, json ) {

        },
    });

    $('.dt-footer').html("<div>Total Selected : <span id='count'>0</span>&nbsp&nbsp&nbsp Total Amount : <span id='tot'>0</span></div>");

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

    $('#dt_budget_approval').on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_budget_approval.row(this.closest('tr')).data();
        let budgetAmount = rows.budgetAmount;
        if (this.checked) {
            arrChecked.push({
                id: rows.id,
                budgetAmount: parseFloat(rows.budgetAmount)
            });
            sumAmount += parseFloat(budgetAmount);
        } else {
            sumAmount -= parseFloat(budgetAmount);
            let index = arrChecked.findIndex(p => p.id === rows.id);
            if (index > -1) {
                arrChecked.splice(index, 1);
            }
        }
        $('#tot').html(formatMoney(sumAmount.toString(), 0));
        $('#count').html(arrChecked.length);
    });
});

$('#dt_budget_approval_search').on('keyup', function() {
    dt_budget_approval.search(this.value).draw();
});

$('#dt_budget_approval_view').on('click', function (){
    let e = document.getElementById('dt_budget_approval_view');
    let period = ($('#filter_period').val()) ?? "";
    let entityId = ($('#filter_entity').val()) ?? "";
    let distributorId = ($('#filter_distributor').val()) ?? "";
    let channelId = ($('#filter_channel').val()) ?? "";
    let url = "/budget/approval/list/paginate/filter?period=" + period + "&entityId=" + entityId + "&distributorId=" + distributorId + "&channelId=" + channelId;
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    $('#btn_approve').attr('disabled', true);
    $('#btn_unapprove').attr('disabled', true);
    dt_budget_approval.ajax.url(url).load(function () {
        resetForm();

        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
        $('#btn_approve').attr('disabled', false);
        $('#btn_unapprove').attr('disabled', false);
    });
});

$('#btn_export_excel').on('click', function() {
    let text_entity = "";
    let data = $('#filter_entity').select2('data');
    (data[0].id !== "") ? text_entity = data[0].longDesc : text_entity ='ALL';
    let period = ($('#filter_period').val()) ?? "";
    let entityId = ($('#filter_entity').val()) ?? "";
    let distributorId = ($('#filter_distributor').val()) ?? "";
    let channelId = ($('#filter_channel').val()) ?? "";
    let url = "/budget/approval/export-xls?period=" + period + "&entityId=" + entityId + "&distributorId=" + distributorId + "&channelId=" + channelId + "&entity=" + text_entity;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#btn_approve').on('click', async function () {
    let e = document.querySelector("#btn_approve");
    if (arrChecked.length > 0) {
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        $('#d_progress').removeClass('d-none');
        blockUI.block();
        let perc = 0;
        let error = true;
        for (let i=1; i <= arrChecked.length; i++) {
            let dataRow = arrChecked[i-1];
            let res_method = await approve(dataRow.id);
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
                        text: 'Complete',
                        icon: "success",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_budget_approval.ajax.reload();
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
                text: "Approve Failed",
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

$('#btn_unapprove').on('click', async function () {
    let e = document.querySelector("#btn_unapprove");
    if (arrChecked.length > 0) {
        Swal.fire({
            title: 'Please input reason for budget unapproval',
            input: 'text',
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            showLoaderOnConfirm: true,
            confirmButtonText: 'Unapprove',
            customClass: {
                validationMessage: 'my-validation-message'
            },
            preConfirm: async (value) => {
                if (!value) {
                    Swal.showValidationMessage(
                        'Reason must be enter'
                    )
                } else {
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                    $('#d_progress').removeClass('d-none');
                    blockUI.block();
                    let perc = 0;
                    let error = true;
                    let note = value;
                    for (let i=1; i <= arrChecked.length; i++) {
                        let dataRow = arrChecked[i-1];
                        let res_method = await unApprove(dataRow.id, note);
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
                                    text: 'Complete',
                                    icon: "success",
                                    confirmButtonText: "OK",
                                    customClass: {confirmButton: "btn btn-optima"}
                                }).then(function () {
                                    dt_budget_approval.ajax.reload();
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
                            text: "Unapprove Failed",
                            icon: "warning",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            confirmButtonText: "OK",
                            customClass: { confirmButton: "btn btn-optima" }
                        });
                    }
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

const approve = (id) =>  {
    return new Promise(function (resolve, reject) {
        let formData = new FormData();
        formData.append('id', id);
        $.ajax({
            type        : 'POST',
            url         : "/budget/approval/approve",
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

const unApprove = (id, note) =>  {
    return new Promise(function (resolve, reject) {
        let formData = new FormData();
        formData.append('id', id);
        formData.append('notes', note);
        $.ajax({
            type        : 'POST',
            url         : "/budget/approval/unapprove",
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
        url         : "/budget/approval/list/entity",
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
            url         : "/budget/approval/list/distributor/entity-id",
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

const getListChannel = () => {
    $.ajax({
        url         : "/budget/approval/list/channel",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j){
                data.push({
                    id: result.data[j].id,
                    text: result.data[j].longDesc
                });
            }
            $('#filter_channel').select2({
                placeholder: "Select a Channel",
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

const resetForm = () => {
    arrChecked = [];
    sumAmount = 0;
    if (arrChecked.length > 0) {
        for (let i = 0; i < arrChecked.length; i++) {
            sumAmount += parseFloat(arrChecked[i].budgetAmount);
        }
    }
    $('#tot').html(formatMoney(sumAmount.toString(), 0));
    $('#count').html(arrChecked.length);
}

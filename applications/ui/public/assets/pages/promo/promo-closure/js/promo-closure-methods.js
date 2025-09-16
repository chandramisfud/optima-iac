'use strict';

var dt_promo_closure, arrChecked = [], dataFilter, url_datatable;
var swalTitle = "Promo Closure";
heightContainer = 350;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#filter_activity_start, #filter_activity_end').flatpickr({
        altFormat: "d-m-Y",
        altInput: true,
        allowInput: true,
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    if (localStorage.getItem('promoClosureState')) {
        dataFilter = JSON.parse(localStorage.getItem('promoClosureState'));

        url_datatable = '/promo/closure/list?entityId=' + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId +
            "&channelId=" + dataFilter.channelId + "&startFrom=" + dataFilter.activityStart + "&startTo=" + dataFilter.activityEnd + "&remainingBudget=" + dataFilter.remainingBudget;
    } else {

        let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
        let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
        url_datatable = '/promo/closure/list?startFrom=' + filter_activity_start + '&startTo=' + filter_activity_end + '&remainingBudget=all';
    }

    getListEntity().then(async function () {
        await getListChannel();
        if (dataFilter) {
            let startDate = new Date(dataFilter.activityStart);
            let endDate = new Date(dataFilter.activityEnd);
            $('#filter_activity_start').flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
            }).setDate(startDate);
            $('#filter_activity_end').flatpickr({
                altFormat: "d-m-Y",
                altInput: true,
                allowInput: true,
                dateFormat: "Y-m-d",
                disableMobile: "true",
            }).setDate(endDate);
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await $('#filter_channel').val(dataFilter.channelId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
            await $('#remaining_budget').val(dataFilter.remainingBudget).trigger('change.select2');
        } else {
            $('#filter_period').val(new Date().getFullYear());
            dialerObject.setValue(new Date().getFullYear());
        }
    });

    dt_promo_closure = $('#dt_promo_closure').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: url_datatable,
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
                data: 'promoId',
                width: 20,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center',
                checkboxes: {
                    'selectRow': false
                },
                render: function (data, type, full, meta) {
                    if (full.closureStatus) {
                        data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off" disabled>';
                    } else {
                        data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off">';
                    }
                    return data;
                },
            },
            {
                targets: 1,
                title: 'Promo ID',
                data: 'promoNumber',
                width: 10,
                className: 'text-nowrap align-middle',
                render: function (data, type, full) {
                    let url;
                    let startYear = new Date(full['startPromo']).getFullYear();
                    if(full.closureStatus) {
                        url = '/promo/closure/form?m=open&id=' + full.promoId + '&sy=' + startYear + '&recon=' + (full.reconciled ? '1' : '0') + '&c=' + full.categoryShortDesc;
                    } else {
                        url = '/promo/closure/form?m=close&id=' + full.promoId + '&sy=' + startYear + '&recon=' + (full.reconciled ? '1' : '0') + '&c=' + full.categoryShortDesc;
                    }
                    return '<a class="text-primary cursor-pointer" href="'+url+'">' + data + '</a>'
                }
            },
            {
                targets: 2,
                title: 'Initiator',
                data: 'initiator',
                width: 60,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 60,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 60,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Start Promo',
                data: 'startPromo',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    return formatDate(data) ?? "";
                }
            },
            {
                targets: 6,
                title: 'End Promo',
                data: 'endPromo',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data, type, full, meta) {
                    return formatDate(data) ?? "";
                }
            },
            {
                targets: 7,
                title: 'Stastus',
                data: 'promoStatus',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Status Date',
                data: 'statusdate',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data, type, full, meta) {
                    if (data === null) {
                        return data = "";
                    } else {
                        return formatDate(data) ?? "";
                    }
                }
            },
            {
                targets: 9,
                title: 'Investment',
                data: 'investment',
                width: 100,
                className: 'align-middle text-end text-nowrap',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0) ?? "";
                }
            },
            {
                targets: 10,
                title: 'DN Claim',
                data: 'dnClaim',
                width: 100,
                className: 'align-middle text-end text-nowrap',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0) ?? "";
                }
            },
            {
                targets: 11,
                title: 'DN Paid',
                data: 'dnPaid',
                width: 100,
                className: 'align-middle text-end text-nowrap',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0) ?? "";
                }
            },
            {
                targets: 12,
                title: 'Last DN Creation Date',
                data: 'lastDNCreationDate',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data, type, full, meta) {
                    return formatDate(data) ?? "";
                }
            },
            {
                targets: 13,
                title: 'Remaining',
                data: 'remainingInvestment_DN',
                width: 100,
                className: 'align-middle text-end text-nowrap',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0) ?? "";
                }
            },
            {
                targets: 14,
                title: 'Closure Status',
                data: 'closureStatus',
                width: 100,
                className: 'align-middle text-nowrap',
                render: function (data, type, full, meta) {
                    if (data) {
                        return 'Closed'
                    } else {
                        return 'Open'
                    }
                }
            },
        ],
        initComplete: function( settings, json ) {
            $('#dt_promo_closure_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_promo_closure.rows( { filter : 'applied'} ).data();
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        arrChecked.push({
                            promoId: data[i].promoId,
                            balance: data[i].remainingInvestment_DN,
                            investment: data[i].investment,
                        });
                    }
                } else {
                    for (let i = 0; i < data.length; i++) {
                        let index = arrChecked.findIndex(p => p.promoId === data[i].promoId);
                        if (index > -1) {
                            arrChecked.splice(index, 1);
                        }
                    }
                }
                let sumInvestment = 0;
                let sumBalance = 0
                if (arrChecked.length > 0) {
                    for (let i = 0; i < arrChecked.length; i++) {
                        sumBalance += parseFloat(arrChecked[i].balance);
                        sumInvestment += parseFloat(arrChecked[i].investment);
                    }
                }
                $('#totBalance').html(formatMoney(sumBalance.toString(), 0));
                $('#totInvestment').html(formatMoney(sumInvestment.toString(), 0));
                $('#count').html(arrChecked.length);
            });
        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $('.dt-footer').html("<div>Total Selected : <span id='count'>0</span>&nbsp&nbsp&nbsp Total Balance : <span id='totBalance'>0</span>&nbsp&nbsp&nbsp Total Investment : <span id='totInvestment'>0</span></div>");

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

    $('#dt_promo_closure').on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_promo_closure.row(this.closest('tr')).data();
        if (this.checked) {
            arrChecked.push({
                promoId: rows.promoId,
                balance: rows.remainingInvestment_DN,
                investment: rows.investment,
            });
        } else {
            let index = arrChecked.findIndex(p => p.promoId === rows.promoId);
            if (index > -1) {
                arrChecked.splice(index, 1);
            }
        }
        let sumInvestment = 0;
        let sumBalance = 0
        if (arrChecked.length > 0) {
            for (let i = 0; i < arrChecked.length; i++) {
                sumBalance += parseFloat(arrChecked[i].balance);
                sumInvestment += parseFloat(arrChecked[i].investment);
            }
        }
        $('#totBalance').html(formatMoney(sumBalance.toString(), 0));
        $('#totInvestment').html(formatMoney(sumInvestment.toString(), 0));
        $('#count').html(arrChecked.length);
    });

    $("#dt_promo_closure").on('click', '.closure-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_promo_closure.row(tr).data();
        let url;
        if(trdata.closureStatus) {
            url = '/promo/closure/form?m=open&id=' + trdata.promoId + '&c=' + trdata.categoryShortDesc;
        } else {
            url = '/promo/closure/form?m=close&id=' + trdata.promoId + '&c=' + trdata.categoryShortDesc;
        }
        window.location.href = url;
    });
});

$('#filter_activity_start').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val());
    let endDate = new Date(el_end.val());
    if (startDate > endDate) {
        el_end.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(startDate)
        });
    }
});

$('#filter_activity_end').on('change', function () {
    let el_start = $('#filter_activity_start');
    let el_end = $('#filter_activity_end');
    let startDate = new Date(el_start.val()).getTime();
    let endDate = new Date(el_end.val()).getTime();
    if (startDate > endDate) {
        el_start.flatpickr({
            altFormat: "d-m-Y",
            altInput: true,
            allowInput: true,
            dateFormat: "Y-m-d",
            disableMobile: "true",
            defaultDate: new Date(endDate)
        });
    }
});

$('#dt_promo_closure_search').on('keyup', function() {
    dt_promo_closure.search(this.value, false, false).draw();
});

$('#dt_promo_closure_view').on('click', function (){
    let btn = document.getElementById('dt_promo_closure_view');
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let filter_channel = ($('#filter_channel').val()) ?? "";
    let remainingBudget = ($('#remaining_budget').val()) ?? "";
    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";

    let url = "/promo/closure/list?entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel
        + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end + "&remainingBudget=" + remainingBudget;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_closure.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? ""),
            channelId: ($('#filter_channel').val() ?? ""),
            activityStart: $('#filter_activity_start').val(),
            activityEnd: $('#filter_activity_end').val(),
            remainingBudget: ($('#remaining_budget').val() ?? ""),
        };

        localStorage.setItem('promoClosureState', JSON.stringify(data_filter));
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let text_entity = "";
    let text_channel = "";
    let data = $('#filter_entity').select2('data');
    (data[0].id !== "") ? text_entity = data[0].text : text_entity ='ALL';
    let text_distributor = "";
    let dataDist = $('#filter_distributor').select2('data');
    if (dataDist.length > 0) {
        if (dataDist[0].id !== "") {
            text_distributor = dataDist[0].text
        } else {
            text_distributor ='ALL'
        }
    } else {
        text_distributor ='ALL'
    }
    let dataChannel = $('#filter_channel').select2('data');
    (dataChannel[0].id !== "") ? text_channel = dataChannel[0].text : text_channel ='ALL';

    let filter_entity = ($('#filter_entity').val()) ?? "0";
    let filter_distributor = ($('#filter_distributor').val()) ?? "0";
    let filter_channel = ($('#filter_channel').val()) ?? "";
    let remainingBudget = ($('#remaining_budget').val()) ?? "";

    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let url = "/promo/closure/export-xls?entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel
        + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end + "&remainingBudget=" + remainingBudget
        + "&entity=" + text_entity + "&distributor=" + text_distributor+ "&channel=" + text_channel;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_download_template').on('click', function() {
    let text_entity = "";
    let text_distributor = "";
    let text_channel = "";
    let data = $('#filter_entity').select2('data');
    (data[0].id !== "") ? text_entity = data[0].text : text_entity ='ALL';
    let dataDist = $('#filter_distributor').select2('data');
    (dataDist[0].id !== "") ? text_distributor = dataDist[0].text : text_distributor ='ALL';
    let dataChannel = $('#filter_channel').select2('data');
    (dataChannel[0].id !== "") ? text_channel = dataChannel[0].text : text_channel ='ALL';

    let filter_entity = ($('#filter_entity').val()) ?? "0";
    let filter_distributor = ($('#filter_distributor').val()) ?? "0";
    let filter_channel = ($('#filter_channel').val()) ?? "";
    let remainingBudget = ($('#remaining_budget').val()) ?? "";

    let filter_activity_start = ($('#filter_activity_start').val()) ?? "";
    let filter_activity_end = ($('#filter_activity_end').val()) ?? "";
    let url = "/promo/closure/download-template?entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&channelId=" + filter_channel
        + "&startFrom=" + filter_activity_start + "&startTo=" + filter_activity_end + "&remainingBudget=" + remainingBudget
        + "&entity=" + text_entity + "&distributor=" + text_distributor+ "&channel=" + text_channel;

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

$('#btn-upload').on('click', function() {
    window.location.href = "/promo/closure/upload-form";
});

$('#btn_submit').on('click', async function () {
    let messageError = "";
    let e = document.querySelector("#btn_submit");
    if (arrChecked.length > 0) {
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        $('#d_progress').removeClass('d-none');
        blockUI.block();
        let perc = 0;
        let error = false;
        for (let i=1; i <= arrChecked.length; i++) {
            let dataRow = arrChecked[i-1];
            let res_method = await closePromo(dataRow.promoId);
            if (res_method.error) {
                error = true;
                messageError = res_method.doc + ' ' + res_method.message;
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
                        dt_promo_closure.ajax.reload();
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
                dt_promo_closure.ajax.reload();
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

const closePromo = (promoId) =>  {
    return new Promise(function (resolve, reject) {
        let formData = new FormData();
        formData.append('promoId', promoId);
        $.ajax({
            type        : 'POST',
            url         : "/promo/closure/close",
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

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/closure/list/entity",
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

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/closure/list/distributor",
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
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/closure/list/channel",
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
    let sumInvestment = 0;
    let sumBalance = 0
    if (arrChecked.length > 0) {
        for (let i = 0; i < arrChecked.length; i++) {
            sumBalance += parseFloat(arrChecked[i].balance);
            sumInvestment += parseFloat(arrChecked[i].investment);
        }
    }
    $('#totBalance').html(formatMoney(sumBalance.toString(), 0));
    $('#totInvestment').html(formatMoney(sumInvestment.toString(), 0));
    $('#count').html(arrChecked.length);
}

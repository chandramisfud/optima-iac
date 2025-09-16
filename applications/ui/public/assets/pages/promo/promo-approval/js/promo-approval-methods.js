'use strict';

let dt_promo_approval, arrChecked = [];
let swalTitle = "Promo Approval";
let elDtPromoApproval = $('#dt_promo_approval');
heightContainer = 290;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListEntity();
    getListCategory();

    dt_promo_approval = elDtPromoApproval.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/promo/approval/list',
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
        order: [[2, 'asc']],
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
                render: function (data, type, full) {
                    if (type === 'display' && full.investment <= 10000000) {
                        data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off">';
                    } else {
                        data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off" disabled>'
                    }

                    return data;
                },
            },
            {
                targets: 1,
                title: '',
                data: 'promoId',
                searchable: false,
                orderable: false,
                width: 10,
                className: 'align-middle',
                render: function (data, type, full) {
                    let startYear = new Date(full['startPromo']).getFullYear();
                    return `<div class="me-0">
                                <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>
                                </a>
                                <div class="menu menu-sub menu-sub-dropdown w-175px w-md-175px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">
                                    <a class="dropdown-item text-start approve-record" href="/promo/approval/form-approve?id=${full['promoId']}&c=${full['categoryShortDesc']}&sy=${startYear}"><i class="fa fa-edit fs-6"></i> Select Approval Action</a>
                                </div>
                        </div>`;
                }
            },
            {
                targets: 2,
                title: 'Req ID',
                data: 'requestId',
                width: 60,
                className: 'text-nowrap align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'Req Date',
                data: 'requestDate',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    return formatDate(data) ?? "";
                }
            },
            {
                targets: 4,
                title: 'Promo ID',
                data: 'refId',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Promo Start',
                data: 'startPromo',
                width: 80,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    return formatDate(data) ?? "";
                }
            },
            {
                targets: 6,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 200,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 7,
                title: 'Aging',
                data: 'agingApproval',
                width: 50,
                className: 'align-middle text-end text-nowrap',
                render: function (data, type, full) {
                    switch (full['descstatus']) {
                        case 'Precaution':
                            return formatMoney(data, 0) + '&nbsp;<i class="fa fa-exclamation-circle" style="color:green"></i>';
                        case 'Critical':
                            return formatMoney(data, 0) + '&nbsp;<i class="fa fa-exclamation-circle" style="color:red"></i>';
                        case 'Warning':
                            return formatMoney(data, 0) + '&nbsp;<i class="fa fa-exclamation-circle" style="color:yellow"></i>';
                    }
                }
            },
            {
                targets: 8,
                title: 'Initiator',
                data: 'initiator',
                width: 100,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 9,
                title: 'Cost',
                data: 'investment',
                width: 100,
                className: 'align-middle text-end text-nowrap',
                render: function (data) {
                    return formatMoney(data, 0) ?? "";
                }
            },
            {
                targets: 10,
                title: 'Creation Date',
                data: 'createOn',
                width: 100,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    return formatDateTime(data) ?? "";
                }
            },
        ],
        initComplete: function(  ) {
            $('#dt_promo_approval_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_promo_approval.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        if (data[i].investment <= 10000000) {
                            arrChecked.push({
                                promoId: data[i]['promoId'],
                                categoryShortDescEnc: data[i]['categoryShortDesc'],
                                yearPromo: new Date(data[i]['startPromo']).getFullYear()
                            });
                        }
                    }
                } else {
                    arrChecked = [];
                }
            });
        },
        drawCallback: function() {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        let strMessage = e.jqXHR['responseJSON'].message
        if(strMessage==="") strMessage = "Please contact your vendor"
        Swal.fire({
            title: swalTitle,
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

    elDtPromoApproval.on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_promo_approval.row(this.closest('tr')).data();
        if (this.checked) {
            arrChecked.push({
                promoId: rows['promoId'],
                categoryShortDescEnc: rows['categoryShortDesc'],
                yearPromo: new Date(rows['startPromo']).getFullYear()
            });
        } else {
            let index = arrChecked.findIndex(p => p.promoId === rows.promoId);
            if (index > -1) {
                arrChecked.splice(index, 1);
            }
        }
    });
});

$('#dt_promo_approval_search').on('keyup', function() {
    dt_promo_approval.search(this.value, false, false).draw();
});

$('#dt_promo_approval_view').on('click', function (){
    let btn = document.getElementById('dt_promo_approval_view');
    let filter_category = ($('#filter_category').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/promo/approval/list?entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&categoryId=" + filter_category;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_promo_approval.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    let elFilterDistributor = $('#filter_distributor');
    elFilterDistributor.empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    elFilterDistributor.val('').trigger('change');
});

$('#btn_approve').on('click', async function () {
    let messageError = "";
    let e = document.querySelector("#btn_approve");
    if (arrChecked.length > 0) {
        let elProgress = $('#d_progress');
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        elProgress.removeClass('d-none');
        blockUI.block();
        let perc = 0;
        let error = false;
        for (let i=1; i <= arrChecked.length; i++) {
            let dataRow = arrChecked[i-1];
            let res_method = await approve(dataRow.promoId, dataRow.categoryShortDescEnc, dataRow.yearPromo);
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
                    elProgress.addClass('d-none');
                    Swal.fire({
                        title: swalTitle,
                        text: 'Approval Complete',
                        icon: "success",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_promo_approval.ajax.reload();
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
            elProgress.addClass('d-none');
            Swal.fire({
                title: swalTitle,
                text: messageError,
                icon: "warning",
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: { confirmButton: "btn btn-optima" }
            }).then(function () {
                dt_promo_approval.ajax.reload();
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
            showLoaderOnConfirm: true,
            allowOutsideClick: false,
            allowEscapeKey: false,
            inputValidator: (value) => {
                if (!value) {
                    return 'Please enter sendback notes'
                }
            }
        }).then(async function (result) {
            if (result.isConfirmed) {
                let elProgress = $('#d_progress');
                let notes = result.value;
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
                elProgress.removeClass('d-none');
                blockUI.block();
                let perc = 0;
                let error = false;
                for (let i = 1; i <= arrChecked.length; i++) {
                    let dataRow = arrChecked[i - 1];
                    let res_method = await sendBack(dataRow.promoId, dataRow.categoryShortDescEnc, notes);
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
                            elProgress.addClass('d-none');
                            Swal.fire({
                                title: swalTitle,
                                text: 'Send back Complete',
                                icon: "success",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(function () {
                                dt_promo_approval.ajax.reload();
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
                    elProgress.addClass('d-none');
                    Swal.fire({
                        title: swalTitle,
                        text: messageError,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_promo_approval.ajax.reload();
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

const approve = (promoId, categoryShortDescEnc, yearPromo) =>  {
    return new Promise(function (resolve) {
        let formData = new FormData();
        formData.append('promoId', promoId);
        formData.append('categoryShortDescEnc', categoryShortDescEnc);
        formData.append('sy', yearPromo);
        let url = "/promo/approval/approve";

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

const sendBack = (promoId, categoryShortDescEnc, notes) =>  {
    return new Promise(function (resolve) {
        let formData = new FormData();
        formData.append('promoId', promoId);
        formData.append('categoryShortDescEnc', categoryShortDescEnc);
        formData.append('notes', notes);
        let url = "/promo/approval/send-back";

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

const getListCategory = () => {
    $.ajax({
        url         : "/promo/approval/list/category",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let data = [];
            for (let j = 0, len = result.data.length; j < len; ++j){
                data.push({
                    id: result.data[j].Id,
                    text: result.data[j]['categoryLongDesc'],
                });
            }
            $('#filter_category').select2({
                placeholder: "Select a Category",
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

const getListEntity = () => {
    $.ajax({
        url         : "/promo/approval/list/entity",
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

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/approval/list/distributor",
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
            error: function (jqXHR)
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

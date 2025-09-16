'use strict';

let dt_promo_cycle1, dt_promo_cycle2;
let elDtPromoCycle1 = $('#dt_promo_cycle1');
let elDtPromoCycle2 = $('#dt_promo_cycle2');
let arrCycle1Checked = [], arrCycle2Checked = [];
let swalTitle = "Resend Email Approval";
heightContainer = 330;

let targetCycle1 = document.querySelector(".card_form1");
let blockUICycle1 = new KTBlockUI(targetCycle1, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetCycle2 = document.querySelector(".card_form2");
let blockUICycle2 = new KTBlockUI(targetCycle2, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {

    dt_promo_cycle1 = elDtPromoCycle1.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-lg-1'l><'col-lg-5'i><'col-lg-6'p>>",
        ajax: {
            url: "/resend-email-approval/list?cycle=1",
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
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 20,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center',
                checkboxes: {
                    'selectRow': false
                },
                render: function () {
                    return '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off">';
                },
            },
            {
                targets: 1,
                title: 'Promo Ref Id',
                data: 'RefId',
                width: 150,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'Approver User',
                data: 'UserApprover',
                width: 100,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'Approver Name',
                data: 'UserApproverName',
                width: 200,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'Email Recipient',
                data: 'email',
                width: 200,
                className: 'align-middle text-nowrap',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {
            $('#dt_promo_cycle1_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_promo_cycle1.rows( { filter : 'applied'} ).data();
                arrCycle1Checked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        arrCycle1Checked.push({
                            promoId: data[i]['id'],
                            promoRefId: data[i]['RefId'],
                            UserApprover: data[i]['UserApprover'],
                            UserApproverName: data[i]['UserApproverName'],
                            email: data[i]['email'],
                            period: data[i]['period']
                        });
                    }
                } else {
                    arrCycle1Checked = [];
                }
            });
        },
    });

    dt_promo_cycle2 = elDtPromoCycle2.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-lg-1'l><'col-lg-5'i><'col-lg-6'p>>",
        ajax: {
            url: "/resend-email-approval/list?cycle=2",
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
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 20,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center',
                checkboxes: {
                    'selectRow': false
                },
                render: function () {
                    return '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off">';
                },
            },
            {
                targets: 1,
                title: 'Promo Ref Id',
                data: 'RefId',
                width: 150,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'Approver User',
                data: 'UserApprover',
                width: 100,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'Approver Name',
                data: 'UserApproverName',
                width: 200,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'Email Recipient',
                data: 'email',
                width: 200,
                className: 'align-middle text-nowrap',
            },
        ],
        initComplete: function (settings, json) {
            $('#dt_promo_cycle2_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_promo_cycle2.rows( { filter : 'applied'} ).data();
                arrCycle2Checked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        arrCycle2Checked.push({
                            promoId: data[i]['id'],
                            promoRefId: data[i]['RefId'],
                            UserApprover: data[i]['UserApprover'],
                            UserApproverName: data[i]['UserApproverName'],
                            email: data[i]['email'],
                            period: data[i]['period']
                        });
                    }
                } else {
                    arrCycle2Checked = [];
                }
            });
        },
        drawCallback: function (settings, json) {

        },
    });

    elDtPromoCycle1.on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_promo_cycle1.row(this.closest('tr')).data();
        if (this.checked) {
            arrCycle1Checked.push({
                promoId: rows['id'],
                promoRefId: rows['RefId'],
                UserApprover: rows['UserApprover'],
                UserApproverName: rows['UserApproverName'],
                email: rows['email'],
                period: rows['period']
            });
        } else {
            let index = arrCycle1Checked.findIndex(p => p.promoId === rows.id);
            if (index > -1) {
                arrCycle1Checked.splice(index, 1);
            }
        }
    });

    elDtPromoCycle2.on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_promo_cycle2.row(this.closest('tr')).data();
        if (this.checked) {
            arrCycle2Checked.push({
                promoId: rows['id'],
                promoRefId: rows['RefId'],
                UserApprover: rows['UserApprover'],
                UserApproverName: rows['UserApproverName'],
                email: rows['email'],
                period: rows['period']
            });
        } else {
            let index = arrCycle2Checked.findIndex(p => p.promoId === rows.id);
            if (index > -1) {
                arrCycle2Checked.splice(index, 1);
            }
        }
    });
});

$('#dt_promo_cycle1_search').on('keyup', function() {
    dt_promo_cycle1.search(this.value).draw();
});

$('#dt_promo_cycle2_search').on('keyup', function() {
    dt_promo_cycle2.search(this.value).draw();
});

$('#btn_send_cycle1').on('click', async function () {
    let messageError = '';
    let e = document.querySelector("#btn_send_cycle1");
    let elProgress = $('#d_progress_cycle1');
    if (arrCycle1Checked.length > 0) {
        Swal.fire({
            title: swalTitle,
            text: 'Please enter key',
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            confirmButtonText: 'Submit',
            showLoaderOnConfirm: true,
            allowOutsideClick: false,
            allowEscapeKey: false,
            inputValidator: (value) => {
                if (!value) {
                    return 'Please enter key'
                }
            }
        }).then(async function (result) {
            if (result.isConfirmed) {
                let validKey = await checkKey(result.value);
                if (validKey) {
                    blockUICycle1.block();
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                    elProgress.removeClass('d-none');

                    let perc = 0;
                    let error = false;
                    for (let i=1; i <= arrCycle1Checked.length; i++) {
                        let dataRow = arrCycle1Checked[i-1];
                        let res_method = await sendEmailCycle1(dataRow);
                        if (res_method.error) {
                            error = true;
                            messageError = res_method.message;
                            break;
                        }
                        if (!res_method.error) {
                            perc = ((i / arrCycle1Checked.length) * 100).toFixed(0);
                            $('#text_progress_cycle1').text(perc.toString() + '%');
                            let progress_import = $('#progress_bar_cycle1');
                            progress_import.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());
                            if (i === arrCycle1Checked.length) {
                                progress_import.css('width', '0%').attr('aria-valuenow', '0');
                                blockUICycle1.release();
                                e.setAttribute("data-kt-indicator", "off");
                                e.disabled = !1;
                                elProgress.addClass('d-none');
                                Swal.fire({
                                    title: swalTitle,
                                    text: 'Send Email Success',
                                    icon: "success",
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"}
                                }).then(function () {
                                    dt_promo_cycle1.ajax.reload();
                                    arrCycle1Checked = [];
                                });
                            }
                        }
                    }
                    if (error) {
                        let progress_import = $('#progress_bar_cycle1');
                        progress_import.css('width', '0%').attr('aria-valuenow', '0');
                        blockUICycle1.release();
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
                            dt_promo_cycle1.ajax.reload();
                            arrCycle1Checked = [];
                        });
                    }
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: "Key is not valid!",
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            }
        });
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please select one or more items for Promo Cycle 1",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

$('#btn_send_cycle2').on('click', async function () {
    let messageError = '';
    let e = document.querySelector("#btn_send_cycle2");
    let elProgress = $('#d_progress_cycle2');
    if (arrCycle2Checked.length > 0) {
        Swal.fire({
            title: swalTitle,
            text: 'Please enter key',
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            showCancelButton: true,
            confirmButtonText: 'Submit',
            showLoaderOnConfirm: true,
            allowOutsideClick: false,
            allowEscapeKey: false,
            inputValidator: (value) => {
                if (!value) {
                    return 'Please enter key'
                }
            }
        }).then(async function (result) {
            if (result.isConfirmed) {
                let validKey = await checkKey(result.value);
                if (validKey) {
                    blockUICycle2.block();
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                    elProgress.removeClass('d-none');

                    let perc = 0;
                    let error = false;
                    for (let i=1; i <= arrCycle2Checked.length; i++) {
                        let dataRow = arrCycle2Checked[i-1];
                        let res_method = await sendEmailCycle2(dataRow);
                        if (res_method.error) {
                            error = true;
                            messageError = res_method.message;
                            break;
                        }
                        if (!res_method.error) {
                            perc = ((i / arrCycle2Checked.length) * 100).toFixed(0);
                            $('#text_progress_cycle2').text(perc.toString() + '%');
                            let progress_import = $('#progress_bar_cycle2');
                            progress_import.css('width', perc.toString() + '%').attr('aria-valuenow', perc.toString());
                            if (i === arrCycle2Checked.length) {
                                progress_import.css('width', '0%').attr('aria-valuenow', '0');
                                blockUICycle2.release();
                                e.setAttribute("data-kt-indicator", "off");
                                e.disabled = !1;
                                elProgress.addClass('d-none');
                                Swal.fire({
                                    title: swalTitle,
                                    text: 'Send Email Success',
                                    icon: "success",
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"}
                                }).then(function () {
                                    dt_promo_cycle2.ajax.reload();
                                    arrCycle2Checked = [];
                                });
                            }
                        }
                    }
                    if (error) {
                        let progress_import = $('#progress_bar_cycle2');
                        progress_import.css('width', '0%').attr('aria-valuenow', '0');
                        blockUICycle2.release();
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
                            dt_promo_cycle2.ajax.reload();
                            arrCycle2Checked = [];
                        });
                    }
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: "Key is not valid!",
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            }
        });
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please select one or more items for Promo Cycle 2",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

const checkKey = (key) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/resend-email-approval/confirm-key",
            type: "GET",
            dataType: 'json',
            data: {key: key},
            async: true,
            success: function (result) {
                return resolve(result);
            },
            complete: function () {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                return resolve(false);
            }
        });
    });
}

const sendEmailCycle1 = (data) =>  {
    return new Promise(function (resolve, reject) {
        let formData = new FormData();
        formData.append('promoId', data['promoId']);
        formData.append('promoRefId', data['promoRefId']);
        formData.append('UserApprover', data['UserApprover']);
        formData.append('UserApproverName', data['UserApproverName']);
        formData.append('email', data['email']);
        formData.append('cycle', '1');
        formData.append('period', data['period']);
        $.ajax({
            type        : 'POST',
            url         : "/resend-email-approval/send-email-cycle1",
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

const sendEmailCycle2 = (data) =>  {
    return new Promise(function (resolve, reject) {
        let formData = new FormData();
        formData.append('promoId', data['promoId']);
        formData.append('promoRefId', data['promoRefId']);
        formData.append('UserApprover', data['UserApprover']);
        formData.append('UserApproverName', data['UserApproverName']);
        formData.append('email', data['email']);
        formData.append('cycle', '2');
        formData.append('period', data['period']);
        $.ajax({
            type        : 'POST',
            url         : "/resend-email-approval/send-email-cycle2",
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

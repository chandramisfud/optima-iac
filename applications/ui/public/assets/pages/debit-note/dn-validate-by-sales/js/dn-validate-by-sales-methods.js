'use strict';

var dt_validate_by_sales, validator, arrChecked = [], sumAmount = 0, checkFlag = [];
var swalTitle = "Debit Note [Validate By Sales]";
heightContainer = 305;

$(document).ready(function () {
    $('#form-upload').hide();
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    let dialerObject = new KTDialer(dialerElement, {
        min: 2015,
        step: 1,
    });

    Promise.all([getListEntity()]).then(async function () {
        $('#filter_period').val(new Date().getFullYear());
        dialerObject.setValue(new Date().getFullYear());
    });

    validator = FormValidation.formValidation(document.querySelector("#form_upload_validate"), {
        fields: {
            file: {
                selector: '[data-stripe="file"]',
                validators: {
                    notEmpty: {
                        message: 'Choose file..',
                    },
                },
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

    dt_validate_by_sales = $('#dt_validate_by_sales').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/validate-by-sales/list?period=' + $('#filter_period').val() + '&entityId=' + $('#filter_entity').val() + '&distributorId=' + $('#filter_distributor').val(),
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
                width: 20,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center align-middle',
                checkboxes: {
                    'selectRow': false,
                },
                createdCell:  function (td, cellData, rowData, row, col){
                    if (checkFlag.length > 0) {
                        if(checkFlag.includes(rowData.id)){
                            this.api().cell(td).checkboxes.select();
                        }else{
                            this.api().cell(td).checkboxes.deselect();
                            if ((rowData.promoId === 0 || rowData.isOverBudget === false) && rowData.id === 0) {
                                td.children[0].disabled = true;
                            }
                        }
                    }
                },
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        if ((full.promoId === 0 || full.isOverBudget) && full.isDNPromo) {
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
                data: 'id',
                width: 20,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center align-middle',
                render: function (data, type, full, meta) {
                    let div = '';
                    if ((full.promoId === 0 || full.isOverBudget) && full.isDNPromo) {
                        div = '';
                    } else {
                        div = '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-125px w-md-125px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start btn-approval" href="/dn/validate-by-sales/form?method=approve&id=' + data + '"><i class="fa fa-check fs-6"></i> DN Approval</a>\
                            </div>\
                        </div>\
                        ';
                    }

                    if (full['subAccount'] === 'All Sub Account') {
                        if (full['isOverBudget']) {
                            div = '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-125px w-md-125px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start btn-approval" href="/dn/validate-by-sales/form?method=reject&id=' + data + '"><i class="fa fa-check fs-6"></i> DN Reject</a>\
                            </div>\
                        </div>\
                        ';
                        } else {
                            div = '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-125px w-md-125px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start btn-approval" href="/dn/validate-by-sales/form?method=approve&id=' + data + '"><i class="fa fa-check fs-6"></i> DN Approval</a>\
                            </div>\
                        </div>\
                        ';
                        }

                    }
                    return div;
                }
            },
            {
                targets: 2,
                title: 'DN Number',
                data: 'refId',
                width: 120,
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
                width: 170,
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
                targets: 6,
                title: 'Sub Account',
                data: 'subAccount',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Last Status',
                data: 'statusSales',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Remaining Balance',
                data: 'remaining',
                width: 100,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return '0';
                    } else {
                        return formatMoney(data,0);
                    }
                }
            },
            {
                targets: 9,
                title: 'Over Budget Status',
                data: 'overBudgetStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 10,
                title: 'Sales Validation Status',
                data: 'salesValidationStatus',
                width: 170,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 11,
                title: 'Remark by Sales',
                data: 'statusSalesNotes',
                width: 170,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 12,
                title: 'Finance Validation Status',
                data: 'financeValidationStatus',
                width: 170,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function (settings, json) {
            $('#dt_validate_by_sales_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_validate_by_sales.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        if ((data[i].promoId === 0 || data[i].isOverBudget) && data[i].isDNPromo) {

                        } else {
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
            KTMenu.createInstances();
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

    $('#dt_validate_by_sales').on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_validate_by_sales.row(this.closest('tr')).data();
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

$('#dt_validate_by_sales_search').on('keyup', function() {
    dt_validate_by_sales.search(this.value).draw();
});

$('#btn-upload').on('click', function() {
    $('#form-upload').toggle();
});

$('#dt_validate_by_sales_view').on('click', function (){
    let btn = document.getElementById('dt_validate_by_sales_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_entity = (($('#filter_entity').val() == "") ? 0 : $('#filter_entity').val());
    let filter_distributor = (($('#filter_distributor').val() == "") ? 0 : $('#filter_distributor').val());

    let url = "/dn/validate-by-sales/list?period=" + filter_period + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_validate_by_sales.ajax.url(url).load(function () {
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
                        dt_validate_by_sales.ajax.reload();
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
        let formData = new FormData();
        formData.append('dnid', id);
        formData.append('approvalStatusCode', 'validate_by_sales');
        $.ajax({
            type        : 'POST',
            url         : "/dn/validate-by-sales/submit",
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

$('#btn_upload').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            let formData = new FormData($('#form_upload_validate')[0]);
            let url = '/dn/validate-by-sales/upload-xls';

            $.ajax({
                url: url,
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                // enctype: "multipart/form-data",
                beforeSend: function () {
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                },
                success: function (result, status, xhr, $form) {
                    if (!result.error) {
                        Swal.fire({
                            title: 'Files Uploaded!',
                            icon: "success",
                            confirmButtonText: "Ok",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            dt_validate_by_sales.clear().draw();
                            let el_header = $('#dt_validate_by_sales_wrapper #dt-checkbox-header');
                            let data = [];
                            if (el_header[0].checked === false) {
                                if (result.data) {
                                    if (result.data.length > 0) {
                                        result.data.forEach(function(el, index) {
                                            if (el.checkFlag === true) {
                                                checkFlag.push(
                                                    el.id
                                                );
                                                data.push({
                                                    id: el.id,
                                                    checkFlag: el.checkFlag,
                                                    refId: el.refId,
                                                    promoRefId: el.promoRefId,
                                                    activityDesc: el.activityDesc,
                                                    dpp: el.dpp,
                                                    sp_principal: el.sp_ho,
                                                    lastStatus: el.lastStatus,
                                                    remaining: el.remaining,
                                                    isDNPromo: el.isDNPromo,
                                                    isOverBudget: el.isOverBudget,
                                                    materialNumber: el.materialNumber,
                                                    taxLevel: el.taxLevel,
                                                    salesValidationStatus: el.salesValidationStatus,
                                                    financeValidationStatus: el.financeValidationStatus ?? '',
                                                    totalClaim: el.totalClaim,
                                                    entityId: el.entityId,
                                                    promoId: el.promoId,
                                                    subAccount: el.subAccount ?? '',
                                                    statusSalesNotes: el.statusSalesNotes ?? '',
                                                    statusSales: el.statusSales ?? ''
                                                });
                                            } else {
                                                data.push({
                                                    id: el.id,
                                                    checkFlag: el.checkFlag,
                                                    refId: el.refId,
                                                    promoRefId: el.promoRefId,
                                                    activityDesc: el.activityDesc,
                                                    dpp: el.dpp,
                                                    sp_principal: el.sp_ho,
                                                    lastStatus: el.lastStatus,
                                                    remaining: el.remaining,
                                                    isDNPromo: el.isDNPromo,
                                                    isOverBudget: el.isOverBudget,
                                                    materialNumber: el.materialNumber,
                                                    taxLevel: el.taxLevel,
                                                    salesValidationStatus: el.salesValidationStatus,
                                                    financeValidationStatus: el.financeValidationStatus ?? '',
                                                    totalClaim: el.totalClaim,
                                                    entityId: el.entityId,
                                                    promoId: el.promoId,
                                                    subAccount: el.subAccount ?? '',
                                                    statusSalesNotes: el.statusSalesNotes ?? '',
                                                    statusSales: el.statusSales ?? ''
                                                });
                                            }
                                        });
                                    }
                                }
                                dt_validate_by_sales.rows.add(data).draw();
                            }
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "Ok",
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                },
                complete: function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.message)
                    Swal.fire({
                        title: swalTitle,
                        text: "Files Upload Failed",
                        icon: "error",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        } else {
            Swal.fire({
                title: swalTitle,
                text: 'Choose file...',
                icon: "warning",
                confirmButtonText: "Ok",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

const getListEntity = () => {
    $.ajax({
        url         : "/dn/validate-by-sales/list/entity",
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
            url         : "/dn/validate-by-sales/list/distributor/entity-id",
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

$('#btn_download').on('click', function() {
    const current_url = window.location.href;
    const url_object = new URL(current_url);
    const protocol = url_object.protocol;
    const domain = url_object.hostname;
    const port = url_object.port;

    var url;
    if (port == "") {
        url = protocol + "//" + domain + "/" + 'assets/media/templates/Template_DN_Upload_Validation.xlsx';
    } else {
        url = protocol + "//" + domain + ":" + port + "/" + 'assets/media/templates/Template_DN_Upload_Validation.xlsx';
    }
    fetch(url)
        .then(resp => resp.blob())
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            // the filename you want
            let name_file = 'Template DN Validation.xlsx';
            a.download = name_file;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        });
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

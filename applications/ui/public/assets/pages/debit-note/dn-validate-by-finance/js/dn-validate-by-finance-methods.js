'use strict';

let validator, dt_validate_by_finance, arrChecked = [], sumAmount = 0, checkFlag = [];
let swalTitle = "Debit Note [Validate By Finance]";
let elDtValidateByFinance = $('#dt_validate_by_finance');
heightContainer = 305;

$(document).ready(function () {
    $('#form-upload').hide();
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
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

    getListEntity()

    dt_validate_by_finance = elDtValidateByFinance.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/validate-by-finance/list?entityId=' + $('#filter_entity').val() + '&distributorId=' + $('#filter_distributor').val(),
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
                createdCell:  function (td, cellData, rowData){
                    if (checkFlag.length > 0) {
                        if (checkFlag.includes(rowData.id)) {
                            this.api().cell(td).checkboxes.select();
                        } else {
                            this.api().cell(td).checkboxes.deselect();
                            if (!rowData['tickable']) {
                                td.children[0].disabled = true;
                            }
                        }
                    }
                },
                render: function (data, type, full) {
                    if (type === 'display') {
                        if (!full['tickable']) {
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
                render: function (data, type, full) {
                    if (((full.promoId === 0 || full.isOverBudget !== 0) && full.isDNPromo) || full.lastStatus === 'validate_by_finance') {
                        return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-125px w-md-125px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start btn-reject" href="/dn/validate-by-finance/form?method=reject&id=' + data + '"><i class="fa fa-check fs-6"></i> DN Reject</a>\
                            </div>\
                        </div>\
                    ';
                    } else {
                        return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-125px w-md-125px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start btn-approval" href="/dn/validate-by-finance/form?method=approve&id=' + data + '"><i class="fa fa-check fs-6"></i> DN Approval</a>\
                            </div>\
                        </div>\
                    ';
                    }
                }
            },
            {
                targets: 2,
                title: 'VAT Expired',
                data: 'id',
                width: 20,
                className: 'text-nowrap align-middle',
                render: function (data, type, full) {
                    let checked="";
                    if (type === 'display') {
                        if (full.dnvatExpired) {
                            checked = "checked";
                        } else {
                            checked = "";
                        }
                        return '\
                        <span class="form-check form-switch form-check-custom form-check-success form-check-solid">\
                            <label><input type="checkbox" class="form-check-input vat-expired" ' + checked + ' data-toggle="toggle">\
                            </label>\
                        </span>\
                    ';
                    }
                    return data;
                },
            },
            {
                targets: 3,
                title: 'DN Number',
                data: 'refId',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'DN Description',
                data: 'activityDesc',
                width: 500,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'DPP',
                data: 'dpp',
                width: 170,
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
                title: 'SP No',
                data: 'sp_principal',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: 'Remaining Balance',
                data: 'remaining',
                width: 170,
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    if (data == null) {
                        return '0';
                    } else {
                        return formatMoney(data,0);
                    }
                }
            },
            {
                targets: 10,
                title: 'Over Budget Status',
                data: 'overBudgetStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 11,
                title: 'TaxLevel',
                data: 'materialNumber',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full) {
                    if (data === null || data === "" || full.taxLevel === null) {
                        return ''
                    } else {
                        return data + ' - ' + full.taxLevel
                    }
                }
            },
            {
                targets: 12,
                title: 'Sales Validation Status',
                data: 'salesValidationStatus',
                width: 170,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 13,
                title: 'Total Claim',
                data: 'totalClaim',
                width: 170,
                visible: false,
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
                targets: 14,
                title: 'entityId',
                data: 'entityId',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 15,
                title: 'promoId',
                data: 'promoId',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 16,
                title: 'isDNPromo',
                data: 'isDNPromo',
                visible: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 17,
                data: 'materialNumber',
                visible: false,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function () {
            $('#dt_validate_by_finance_wrapper').on('change', 'thead th #dt-checkbox-header',  function () {
                let data = dt_validate_by_finance.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        if (data[i]['tickable']) {
                            arrChecked.push({
                                id: data[i].id,
                                entityId: data[i].entityId,
                                promoId: data[i].promoId,
                                isDNPromo: data[i].isDNPromo,
                                taxLevel: data[i].materialNumber,
                                totalClaim: parseFloat(data[i].totalClaim),
                                wHTType: data[i].whtType,
                                statusPPH: data[i].statusPPH,
                                pphPct: data[i].pphPct,
                                pphAmt: data[i].pphAmt
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
        drawCallback: function () {
            KTMenu.createInstances();
        },
    });

    $('.dt-footer').html("<div>Total Selected : <span id='count'>0</span>&nbsp&nbsp&nbsp Total Claim : <span id='tot'>0</span></div>");

    $.fn.dataTable.ext.errMode = function (e) {
        console.log(e.jqXHR);
        let strMessage = e.jqXHR['responseJSON'].message
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

    elDtValidateByFinance.on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_validate_by_finance.row(this.closest('tr')).data();
        let totalClaim = rows.totalClaim;
        if (this.checked) {
            arrChecked.push({
                id: rows.id,
                entityId: rows.entityId,
                promoId: rows.promoId,
                isDNPromo: rows.isDNPromo,
                taxLevel: rows.materialNumber,
                totalClaim: parseFloat(rows.totalClaim),
                wHTType: rows.whtType,
                statusPPH: rows.statusPPH,
                pphPct: rows.pphPct,
                pphAmt: rows.pphAmt
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

    elDtValidateByFinance.on('change', 'tbody td .vat-expired', function () {
        let set = $(this).closest($(this).parents('tr')).find('td .vat-expired');
        let checked = $(this).is(':checked');

        let data = dt_validate_by_finance.row($(this).parents('tr')).data();

        let Id = data.id;
        let url;
        let setVATExpired;
        if (checked) {
            url = "/dn/validate-by-finance/vat-expired/update";
            setVATExpired = 1;
        } else {
            url = "/dn/validate-by-finance/vat-expired/update";
            setVATExpired = 0;
        }

        let formData = new FormData();
        formData.append('id', Id);
        formData.append('VATExpired', setVATExpired);

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
                set.disabled = !0;
            },
            success: function(result) {
                if (!result.error) {
                    set.prop('checked', setVATExpired)
                } else {
                    if (setVATExpired === 1){
                        set.prop('checked', 0)
                    } else {
                        set.prop('checked', 1)
                    }
                }
            },
            complete: function() {
                set.disabled = !1;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown)
                Swal.fire({
                    title: swalTitle,
                    text: "Failed to update VAT Expired, an error occurred in the process",
                    icon: "error",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        });
    });
});

$('#dt_validate_by_finance_search').on('keyup', function() {
    dt_validate_by_finance.search(this.value).draw();
});

$('#dt_validate_by_finance_view').on('click', function (){
    let btn = document.getElementById('dt_validate_by_finance_view');
    let elFilterEntity = $('#filter_entity');
    let elFilterDistributor = $('#filter_distributor');
    let filter_entity = ((elFilterEntity.val() === "") ? 0 : elFilterEntity.val());
    let filter_distributor = ((elFilterDistributor.val() === "") ? 0 : elFilterDistributor.val());

    let url = "/dn/validate-by-finance/list?entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_validate_by_finance.ajax.url(url).load(function () {
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

$('#btn_submit').on('click', async function () {
    let e = document.querySelector("#btn_submit");
    if (arrChecked.length > 0) {
        e.setAttribute("data-kt-indicator", "on");
        e.disabled = !0;
        let elProgress = $('#d_progress');
        elProgress.removeClass('d-none');
        blockUI.block();
        let perc = 0;
        let error = true;
        let message='';
        for (let i=1; i <= arrChecked.length; i++) {
            let dataRow = arrChecked[i-1];
            let res_method = await submit(dataRow.id, dataRow.entityId, dataRow.promoId, dataRow.isDNPromo, dataRow.taxLevel, dataRow.wHTType, dataRow.statusPPH, dataRow.pphPct, dataRow.pphAmt);
            message = res_method.message;
            if (res_method.error) {
                error = false;
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
                        text: 'Complete',
                        icon: "success",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_validate_by_finance.ajax.reload();
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
            elProgress.addClass('d-none');
            Swal.fire({
                title: swalTitle,
                text: message,
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

const submit = (id, entityId, promoId, isDnPromo, taxLevel, wHTType, statusPPH, pphPct, pphAmt) =>  {
    return new Promise(function (resolve) {
        let formData = new FormData();
        formData.append('dnid', id);
        formData.append('approvalStatusCode', 'validate_by_finance');
        formData.append('entityId', entityId);
        formData.append('promoId', promoId);
        formData.append('isDNPromo', isDnPromo);
        formData.append('taxLevel', taxLevel);
        formData.append('whtType', wHTType);
        formData.append('statusPPH', statusPPH);
        formData.append('pphPct', pphPct);
        formData.append('pphAmt', pphAmt);
        $.ajax({
            type        : 'POST',
            url         : "/dn/validate-by-finance/submit",
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
                console.log(errorThrown)
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
        url         : "/dn/validate-by-finance/list/entity",
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
            console.log(errorThrown);
        }
    });
}

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/validate-by-finance/list/distributor/entity-id",
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
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

$('#btn_download').on('click', function() {
    let url = '/assets/media/templates/Template_DN_Upload_Validation.xlsx';
    fetch(url)
        .then(resp => resp.blob())
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            // the filename you want
            a.download = 'Template DN Validation.xlsx';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        });
});


$('#btn-upload').on('click', function() {
    $('#form-upload').toggle();
});

$('#btn_upload').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            let formData = new FormData($('#form_upload_validate')[0]);
            let url = '/dn/validate-by-finance/upload-xls';
            $.ajax({
                url: url,
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function () {
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                },
                success: function (result) {
                    if (!result.error) {
                        Swal.fire({
                            title: 'Files Uploaded!',
                            icon: "success",
                            confirmButtonText: "Ok",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            dt_validate_by_finance.clear().draw();
                            let el_header = $('#dt_validate_by_finance_wrapper #dt-checkbox-header');
                            let data = [];
                            if (el_header[0].checked === false) {
                                if (result.data) {
                                    if (result.data.length > 0) {
                                        result.data.forEach(function(el) {
                                            if (el.checkFlag === true) {
                                                checkFlag.push(
                                                    el.id
                                                );
                                                data.push({
                                                    id: el.id,
                                                    checkFlag: el['checkFlag'],
                                                    refId: el['refId'],
                                                    promoRefId: el['promoRefId'],
                                                    activityDesc: el['activityDesc'],
                                                    dpp: el['dpp'],
                                                    sp_principal: el['sp_ho'],
                                                    lastStatus: el['lastStatus'],
                                                    remaining: el['remaining'],
                                                    isDNPromo: el['isDNPromo'] ?? '',
                                                    isOverBudget: el['isOverBudget'],
                                                    materialNumber: el['materialNumber'],
                                                    taxLevel: el['taxLevel'],
                                                    salesValidationStatus: el['salesValidationStatus'],
                                                    totalClaim: el['totalClaim'],
                                                    entityId: el['entityId'],
                                                    promoId: el['promoId'],
                                                    vatExpired: el['vatExpired'],
                                                    dnvatExpired: el['dnvatExpired'],
                                                    docCount: 1

                                                });
                                            } else {
                                                data.push({
                                                    id: el['id'],
                                                    checkFlag: el['checkFlag'],
                                                    refId: el['refId'],
                                                    promoRefId: el['promoRefId'],
                                                    activityDesc: el['activityDesc'],
                                                    dpp: el['dpp'],
                                                    sp_principal: el['sp_ho'],
                                                    lastStatus: el['lastStatus'],
                                                    remaining: el['remaining'],
                                                    isDNPromo: el['isDNPromo'] ?? '',
                                                    isOverBudget: el['isOverBudget'],
                                                    materialNumber: el['materialNumber'],
                                                    taxLevel: el['taxLevel'],
                                                    salesValidationStatus: el['salesValidationStatus'],
                                                    totalClaim: el['totalClaim'],
                                                    entityId: el['entityId'],
                                                    promoId: el['promoId'],
                                                    vatExpired: el['vatExpired'],
                                                    dnvatExpired: el['dnvatExpired'],
                                                    docCount: 0
                                                });
                                            }
                                        });
                                    }
                                }
                                dt_validate_by_finance.rows.add(data).draw();
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
                    console.log(errorThrown)
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

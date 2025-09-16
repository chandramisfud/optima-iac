'use strict';

let swalTitle = "Promo Cancel Request";
let url_str = new URL(window.location.href);
let method = url_str.searchParams.get("method");
let promoId = url_str.searchParams.get("id");
let categoryShortDescEnc = url_str.searchParams.get("c");
let statusApprovalCode;

let promoRefId, createdBy, groupBrandId, categoryId, subCategoryId, activityId, subActivityId, subActivityTypeId, distributorId, channelId, subChannelId, accountId, subAccountId;

let elDtSKU = $('#dt_sku');
let elDtMechanism = $('#dt_mechanism');
let elDtMechanismList = $('#dt_mechanism_list');
let dt_mechanism, dt_mechanism_list, dt_sku;

(function (window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

//<editor-fold desc="Loading Animation">
let targetHeader = document.querySelector(".card_header");
let blockUIHeader = new KTBlockUI(targetHeader, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetMechanism = document.querySelector(".card_mechanism");
let blockUIMechanism = new KTBlockUI(targetMechanism, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetBudget = document.querySelector(".card_budget");
let blockUIBudget = new KTBlockUI(targetBudget, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetRegion = document.querySelector(".card_region");
let blockUIRegion = new KTBlockUI(targetRegion, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetSKU = document.querySelector(".card_sku");
let blockUISKU = new KTBlockUI(targetSKU, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetAttachment = document.querySelector(".card_attachment");
let blockUIAttachment = new KTBlockUI(targetAttachment, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});
//</editor-fold>

$(document).ready(function () {
    $('form').submit(false);

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#remainingBudget, #totalCost, #baseline, #totalSales, #cost");

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
        suffix: ' %'
    }).mask("#uplift, #salesContribution, #storesCoverage, #redemptionRate, #roi, #cr");

    dt_mechanism = elDtMechanism.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: false,
        scrollY: "40vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                width: 10,
                data: 'no',
                orderable: false,
                className: 'text-nowrap text-center align-top',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
            {
                title: 'Mechanism',
                targets: 1,
                data: 'mechanism',
                className: 'align-top',
            },
            {
                title: 'Notes',
                targets: 2,
                data: 'notes',
                className: 'align-top',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
        footerCallback: function (row, data) {

        }
    });

    dt_mechanism_list = elDtMechanismList.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "40vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: '',
                targets: 0,
                data: 'no',
                width: 10,
                orderable: false,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                width: 150,
                data: 'mechanism',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                width: 120,
                data: 'notes',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 4,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 5,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 6,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 7,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 8,
                width: 120,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
            {
                targets: 9,
                width: 150,
                data: 'mechanism',
                className: 'text-nowrap align-middle deleted_cell',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
        createdRow: function (row, data, dataIndex) {
            $('td:eq(1)', row).attr('colspan', '5');
            $('td:eq(2)', row).attr('colspan', '4');
            $('.deleted_cell', row).remove();
            dt_mechanism_list.rows().every(function (rowIdx, tableLoop, rowLoop) {
                if (dataIndex === rowIdx) {
                    let incrSales = (Math.round(data['baseline']) * (data['uplift'] / 100));
                    let roi = Math.round(((incrSales - data['cost']) / data['cost']) * 100);
                    if (!isFinite(roi)) {
                        roi= 0;
                    }

                    this.child(
                        $(`
                        <tr>
                            <td style="width: 0"></td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['baseline'] ?? 0), 0)}</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['uplift'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['totalSales'] ?? 0), 0)}</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['salesContribution'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['storesCoverage'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['redemptionRate'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['cr'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((roi ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%">${formatMoney((data['cost'] ?? 0), 0)}</td>
                        </tr>
                    `)
                    ).show();
                }
            });
        }
    });

    dt_sku = elDtSKU.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "27vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'SKU',
                data: 'skuDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $('#dt_sku_search').on('keyup', function () {
        dt_sku.search(this.value, false, false).draw();
    });

    blockUI.block();
    blockUIHeader.block();
    blockUIMechanism.block();
    blockUIBudget.block();
    blockUIRegion.block();
    blockUISKU.block();
    blockUIAttachment.block();
    disableButtonSave();

    getData(promoId).then(async function () {
        blockUI.release();
        blockUIHeader.release();
        blockUIMechanism.release();
        blockUIBudget.release();
        blockUIRegion.release();
        blockUISKU.release();
        blockUIAttachment.release();
        enableButtonSave();
    });
});

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#review_file_label_' + row);
        let url = file_host + "/assets/media/promo/" + promoId + "/row" + row + "/" + attachment.val();
        if (attachment.val() !== "") {
            fetch(url)
                .then((resp) => {
                    if (resp.ok) {
                        resp.blob().then(blob => {
                            const url_blob = window.URL.createObjectURL(blob);
                            const a = document.createElement('a');
                            a.style.display = 'none';
                            a.href = url_blob;
                            a.download = $('#review_file_label_' + row).val();
                            document.body.appendChild(a);
                            a.click();
                            window.URL.revokeObjectURL(url_blob);
                        })
                            .catch(e => {
                                console.log(e);
                                Swal.fire({
                                    text: "Download attachment failed",
                                    icon: "warning",
                                    buttonsStyling: !1,
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"},
                                });
                            });
                    } else {
                        Swal.fire({
                            title: "Download Attachment",
                            text: "Attachment not found",
                            icon: "warning",
                            buttonsStyling: !1,
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"},
                        });
                    }
                });
        } else {
            Swal.fire({
                title: "Download Attachment",
                text: "Attachment not found",
                icon: "warning",
                buttonsStyling: !1,
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-optima"},
                allowOutsideClick: false,
            });
        }
    }
});

$('.btn_view').on('click', function () {
    let row = $(this).val();
    let attachment = $('#review_file_label_' + row);
    let file_name = attachment.val();
    let title = $('#txt_info_method').text();
    let preview_path =  "/promo/cancel-request/preview-attachment?i="+promoId+"&r="+row+"&fileName="+encodeURIComponent(file_name)+"&t="+title;
    let w = window.innerWidth;
    let h = window.innerHeight;
    window.open(preview_path, "promoWindow", "location=1,status=1,scrollbars=1,width=" + w + ",height=" + h);
});

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    let formData = new FormData($('#form_promo_cancel')[0]);
    formData.append('promoId', promoId);
    formData.append('notes', $('#notes').val());
    formData.append('promoRefId', promoRefId);
    formData.append('createdBy', createdBy);

    let url = '/promo/cancel-request/approve';
    blockUI.block();
    blockUIHeader.block();
    blockUIMechanism.block();
    blockUIBudget.block();
    blockUIRegion.block();
    blockUISKU.block();
    blockUIAttachment.block();
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;

    $.get('/refresh-csrf').done(function(data) {
        let elMeta = $('meta[name="csrf-token"]');
        elMeta.attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': elMeta.attr('content')
            }
        });
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
            },
            success: function(result) {
                if (!result.error) {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "success",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        window.location.href = '/promo/cancel-request';
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete: function() {
                blockUI.release();
                blockUIHeader.release();
                blockUIMechanism.release();
                blockUIBudget.release();
                blockUIRegion.release();
                blockUISKU.release();
                blockUIAttachment.release();
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown)
                Swal.fire({
                    title: swalTitle,
                    text: "Failed to save data, an error occurred in the process",
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

$('#btn_reject').on('click', function () {
    let e = document.querySelector("#btn_reject");
    let formData = new FormData($('#form_promo_cancel')[0]);
    formData.append('promoId', promoId);
    formData.append('notes', $('#notes').val());
    formData.append('promoRefId', promoRefId);
    formData.append('createdBy', createdBy);

    let url = '/promo/cancel-request/reject';
    blockUI.block();
    blockUIHeader.block();
    blockUIMechanism.block();
    blockUIBudget.block();
    blockUIRegion.block();
    blockUISKU.block();
    blockUIAttachment.block();
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;

    $.get('/refresh-csrf').done(function(data) {
        let elMeta = $('meta[name="csrf-token"]');
        elMeta.attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': elMeta.attr('content')
            }
        });
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

            },
            success: function(result) {
                if (!result.error) {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "success",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        window.location.href = '/promo/cancel-request';
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "error",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete: function() {
                blockUI.release();
                blockUIHeader.release();
                blockUIMechanism.release();
                blockUIBudget.release();
                blockUIRegion.release();
                blockUISKU.release();
                blockUIAttachment.release();
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown)
                Swal.fire({
                    title: swalTitle,
                    text: "Failed to save data, an error occurred in the process",
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

const getData = (pId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/promo/cancel-request/recon/id",
            type: "GET",
            data: {id: pId},
            dataType: "JSON",
            success: async function (result) {
                if (!result['error']) {
                    let data = result['data'];
                    let promo = data['promo'];

                    groupBrandId = promo['groupBrandId'];
                    categoryId = promo['categoryId'];
                    subCategoryId = promo['subCategoryId'];
                    activityId = promo['activityId'];
                    subActivityId = promo['subActivityId'];
                    subActivityTypeId = promo['subActivityTypeId'];
                    distributorId = promo['distributorId'];
                    channelId = promo['channelId'];
                    subChannelId = promo['subChannelId'];
                    accountId = promo['accountId'];
                    subAccountId = promo['subAccountId'];
                    promoRefId = promo['refId'];
                    createdBy = promo['createBy'];
                    $('#txt_info_method').text('View Promo ID ' + promo['refId']);

                    $('#notes').val(promo['cancelReason']);

                    $('#entityLongDesc').val(promo['entityLongDesc']);
                    $('#groupBrandLongDesc').val(promo['groupBrandLongDesc']);
                    $('#subCategoryLongDesc').val(promo['subCategoryLongDesc']);
                    $('#activityLongDesc').val(promo['activityLongDesc']);
                    $('#subActivityLongDesc').val(promo['subActivityLongDesc']);
                    $('#activityDesc').val(promo['activityDesc']);
                    $('#startPromo').val(formatDateOptima(promo['startPromo']));
                    $('#endPromo').val(formatDateOptima(promo['endPromo']));
                    $('#period').val(promo['period']);
                    $('#distributorLongDesc').val(promo['distributorLongDesc']);
                    $('#channelDesc').val(promo['channelDesc']);
                    $('#subChannelDesc').val(promo['subChannelDesc']);
                    $('#accountDesc').val(promo['accountDesc']);
                    $('#subAccountDesc').val(promo['subAccountDesc']);


                    $('#budgetSourceName').val(promo['budgetName']);
                    $('#remainingBudget').val((promo['remainingBudget'] ? formatMoney(promo['remainingBudget'],0) : 0));
                    $('#totalCost').val(formatMoney(promo['cost'],0));

                    // set data region
                    let arrSetRegion = [];
                    for (let i=0; i<data['region'].length; i++) {
                        arrSetRegion.push(data['region'][i]['regionDesc']);
                    }
                    let regionText = (arrSetRegion.join(', ') ?? '-');
                    $('#txtInfoRegion').text(regionText);

                    dt_sku.rows.add(data['sku']).draw();

                    if (data['mechanism'].length > 0) {
                        for (let i=0; i<data['mechanism'].length; i++) {
                            data['mechanism'][i]['no'] = i+1;
                        }
                    }

                    if (promo['mechanismInputMethod']) {
                        $('#tblMechanismFreeText').addClass('d-none');
                        $('#tblMechanismList').removeClass('d-none');
                        dt_mechanism_list.rows.add(data['mechanism']).draw();
                        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                    } else {
                        dt_mechanism.rows.add(data['mechanism']).draw();
                        $('#tblMechanismList').addClass('d-none');
                        $('#tblMechanismFreeText').removeClass('d-none');
                        $('#mechanismFreeText').removeClass('d-none');

                        if (data['mechanism'].length > 0) {
                            $('#baseline').val(data['mechanism'][0]['baseline']);
                            $('#uplift').val(data['mechanism'][0]['uplift']);
                            $('#totalSales').val(data['mechanism'][0]['totalSales']);
                            $('#salesContribution').val(data['mechanism'][0]['salesContribution']);
                            $('#storesCoverage').val(data['mechanism'][0]['storesCoverage']);
                            $('#redemptionRate').val(data['mechanism'][0]['redemptionRate']);
                            $('#cr').val(data['mechanism'][0]['cr']);
                            $('#cost').val(data['mechanism'][0]['cost']);
                            roiFormula();
                        }
                    }

                    if (data['attachment']) {
                        data['attachment'].forEach((item) => {
                            if (item['docLink'] === 'row' + parseInt(item['docLink'].replace('row', ''))) {
                                $('#review_file_label_' + parseInt(item['docLink'].replace('row', ''))).val(item.fileName);
                                $('#btn_download' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                                $('#btn_view' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                            }
                        });
                    }
                }
            },
            complete: function () {
                return resolve();
            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return 0;
            }
        });
    }).catch((e) => {
        console.log(e);
        return 0;
    });
}

const roiFormula = () => {
    let elBaseline = $('#baseline');
    let elUplift = $('#uplift');
    let elRoi = $('#roi');
    let elCost = $('#cost');

    let uplift = parseFloat((elUplift.val() === '' ? '0' : elUplift.val()).replace(/,/g, ''));
    let baseline = parseFloat((elBaseline.val() === '' ? '0' : elBaseline.val()).replace(/,/g, ''));
    let cost = parseFloat((elCost.val() === '' ? '0' : elCost.val()).replace(/,/g, ''));

    let incrSales = (baseline * (uplift / 100));
    let roi = Math.round(((incrSales - cost) / cost) * 100);
    if (!isFinite(roi)) {
        elRoi.val(0);
    } else {
        elRoi.val(roi);
    }
}

'use strict';

let swalTitle = "Promo Approval Reconciliation";
let url_str = new URL(window.location.href);
let method = url_str.searchParams.get("method");
let promoId = url_str.searchParams.get("id");
let sy = url_str.searchParams.get("sy");
let categoryShortDescEnc = url_str.searchParams.get("c");
let approvalCycle = url_str.searchParams.get("cycle");
let statusApprovalCode;

let groupBrandId, categoryId, subCategoryId, activityId, subActivityId, subActivityTypeId, distributorId, channelId, subChannelId, accountId, subAccountId;

let validator;

let elDtSKU = $('#dt_sku');
let elDtMechanismCalculator = $('#dt_mechanism_calculator');
let elDtMechanism = $('#dt_mechanism');
let elDtMechanismList = $('#dt_mechanism_list');
let dt_mechanism_calculator, dt_mechanism, dt_mechanism_list, dt_sku;

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

//<editor-fold desc="Document on load">
$(document).ready(function () {
    $('form').submit(false);

    FormValidation.validators.v_notes = function () {
        return {
            validate: function () {
                let approvalAction = $('#approvalStatusCode').val();
                let notes = $('#notes').val();
                if (approvalAction === "TP3" && notes === "") {
                    return {
                        valid: false,
                        message: "This field is required"
                    }
                } else {
                    return {
                        valid: true,
                        message: ""
                    }
                }
            }
        }
    };

    validator = FormValidation.formValidation(document.getElementById('form_promo_approval'), {
        fields: {
            approvalStatusCode: {
                validators: {
                    notEmpty: {
                        message: "This field is required"
                    },
                }
            },
            notes: {
                validators: {
                    v_notes: {

                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
        }
    });

    Inputmask({
        alias: "numeric",
        allowMinus: true,
        autoGroup: true,
        groupSeparator: ",",
    }).mask("#remainingBudget, #totalCost");

    dt_mechanism_calculator = elDtMechanismCalculator.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "5vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Baseline',
                data: 'baseline',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
            {
                targets: 1,
                title: 'Uplift',
                data: 'uplift',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data,0) + '%';
                }
            },
            {
                targets: 2,
                title: 'Total Sales',
                data: 'totalSales',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
            {
                targets: 3,
                title: 'Sales Contribution',
                data: 'salesContribution',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data,0) + '%';
                }
            },
            {
                targets: 4,
                title: 'Stores Coverage',
                data: 'storesCoverage',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data,0) + '%';
                }
            },
            {
                targets: 5,
                title: 'Redemption Rate',
                data: 'redemptionRate',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data,0) + '%';
                }
            },
            {
                targets: 6,
                title: 'CR',
                data: 'cr',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data,2) + '%';
                }
            },
            {
                targets: 7,
                title: 'ROI',
                data: 'roi',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data,0) + '%';
                }
            },
            {
                targets: 8,
                title: 'Cost',
                data: 'cost',
                className: 'text-nowrap align-middle text-end',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    dt_mechanism = elDtMechanism.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: false,
        scrollY: "25vh",
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
                className: 'align-top text-end',
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
        scrollY: "38vh",
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
                className: 'text-nowrap align-middle text-end',
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
                            <td style="text-align: right; width: 11.11%; color: #3747c7;">${formatMoney((data['baseline'] ?? 0), 0)}</td>
                            <td style="text-align: right; width: 11.11%; color: #3747c7;">${formatMoney((data['uplift'] ?? 0), 0)}%</td>
                            <td style="text-align: right; width: 11.11%; color: #3747c7;">${formatMoney((data['totalSales'] ?? 0), 0)}</td>
                            <td style="text-align: right; width: 11.11%; color: #3747c7;">${formatMoney((data['salesContribution'] ?? 0), 0)}%</td>
                            <td style="text-align: right; width: 11.11%; color: #3747c7;">${formatMoney((data['storesCoverage'] ?? 0), 0)}%</td>
                            <td style="text-align: right; width: 11.11%; color: #3747c7;">${formatMoney((data['redemptionRate'] ?? 0), 0)}%</td>
                            <td style="text-align: right; width: 11.11%; color: #3747c7;">${formatMoney((data['cr'] ?? 0), 2)}%</td>
                            <td style="text-align: right; width: 11.11%; color: #3747c7;">${formatMoney((roi ?? 0), 0)}%</td>
                            <td style="text-align: right; width: 11.11%; color: #3747c7;">${formatMoney((data['cost'] ?? 0), 0)}</td>
                        </tr><tr><td>&nbsp;</td></tr>
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

    blockUIHeader.block();
    blockUIMechanism.block();
    blockUIBudget.block();
    blockUIRegion.block();
    blockUISKU.block();
    blockUIAttachment.block();
    disableButtonSave();

    getData(promoId).then(async function () {
        blockUIHeader.release();
        blockUIMechanism.release();
        blockUIBudget.release();
        blockUIRegion.release();
        blockUISKU.release();
        blockUIAttachment.release();
        enableButtonSave();
    });
});
//</editor-fold>

$('#sticky_notes').on('click', function () {
    let elModal = $('#modal_notes');
    if (elModal.hasClass('show')) {
        elModal.modal('hide');
    } else {
        elModal.modal('show');
    }
});

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let formData = new FormData($('#form_promo_approval')[0]);
            formData.append('promoId', promoId);
            formData.append('sy', sy);
            formData.append('categoryShortDescEnc', 'RC');
            let elApprovalStatusCode = $('#approvalStatusCode');
            let url = '';
            if (approvalCycle === '1') {
                url = '/promo/approval/approve';
                if (elApprovalStatusCode.val() === "TP3") {
                    url = '/promo/approval/send-back';
                }
            }

            if (approvalCycle === '2') {
                url = '/promo/approval/approve-recon';
                if (elApprovalStatusCode.val() === "TP3") {
                    url = '/promo/approval/send-back-recon';
                }
            }
            $.get('/refresh-csrf').done(function(data) {
                let elMeta = $('meta[name="csrf-token"]');
                elMeta.attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': elMeta.attr('content')
                    }
                });
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
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
                                window.location.href = '/promo/approval';
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
        }
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
    let preview_path =  "/promo/approval/preview-attachment?i="+promoId+"&r="+row+"&fileName="+encodeURIComponent(file_name)+"&t="+title;
    let w = window.innerWidth;
    let h = window.innerHeight;
    window.open(preview_path, "promoWindow", "location=1,status=1,scrollbars=1,width=" + w + ",height=" + h);
});

const getData = (pId) => {
    return new Promise((resolve) => {
        $.ajax({
            url: "/promo/approval/recon/id",
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
                    $('#txt_info_method').text('Approval Promo ID ' + promo['refId']);

                    $('#txt_sendback_notes').text(promo['sendBackNotesDate']);
                    $('#sendback_notes').text(promo['sendBackNotes']);

                    let dataApprovalAction = [];
                    if (data['promoStatus']) {
                        for (let j = 0, len = data['promoStatus'].length; j < len; ++j){
                            dataApprovalAction.push({
                                id: data['promoStatus'][j]['statusCode'],
                                text: data['promoStatus'][j]['statusDesc'],
                            });
                        }
                    }
                    $('#approvalStatusCode').select2({
                        placeholder: "Select Approval Action",
                        width: '100%',
                        data: dataApprovalAction
                    });

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
                        $('#tblMechanismCalculatorFreeText').addClass('d-none');
                        $('#tblMechanismFreeText').addClass('d-none');
                        $('#tblMechanismList').removeClass('d-none');
                        dt_mechanism_list.rows.add(data['mechanism']).draw();
                        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
                    } else {
                        dt_mechanism.rows.add(data['mechanism']).draw();
                        $('#tblMechanismCalculatorFreeText').removeClass('d-none');
                        $('#tblMechanismList').addClass('d-none');
                        $('#tblMechanismFreeText').removeClass('d-none');
                        $('#mechanismFreeText').removeClass('d-none');

                        if (data['mechanism'].length > 0) {
                            dt_mechanism_calculator.rows.add(data['mechanism']).draw();
                            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
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

                    $('#notes_message').html(promo['createBy'] + ' <span class="ms-2 text-gray-700">'+ (promo['modifiedOn'] ? formatDateOptima(promo['modifiedOn']) : formatDateOptima(promo['createOn'])) +'</span><div class="row ms-2 text-gray-700">' + promo['notes'] + '</div>');

                    statusApprovalCode = promo['statusApprovalCode'];

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

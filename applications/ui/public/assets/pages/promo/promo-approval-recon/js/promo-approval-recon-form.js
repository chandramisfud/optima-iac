'use strict';

let swalTitle = "Promo Approval Reconcile";
let dt_mechanism, id, validator;

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    id = url_str.searchParams.get("id");

    blockUI.block();
    Promise.all([ getData(id) ]).then(async () => {
        blockUI.release();
    });

    const v_notes = function () {
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
    }

    FormValidation.validators.v_notes = v_notes;

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

    dt_mechanism = $('#dt_mechanism').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "30vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                data: 'mechanismId',
                width: 50,
                orderable: false,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                title: 'Mechanism',
                data: 'mechanism',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Notes',
                data: 'notes',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $('.modal-header').on('mousedown', function (e) {
        $('.modal-dialog').draggable("disable");
    }).on('mouseover', function () {
        $('.modal-header').css('cursor', 'default');
        $('.modal-title').css('cursor', 'default');
    });
});

$('#sticky_notes').on('click', function () {
    let elModal = $('#modal_notes');
    if (elModal.hasClass('show')) {
        elModal.modal('hide');
    } else {
        elModal.modal('show');
    }
});

$('#btn_list_dn_claim').on('click', function () {
    let e = document.querySelector("#btn_list_dn_claim");
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    $('#txt_title_list_dn').text('Debit Note Claimed List');
    dt_list_dn.clear().draw();
    dt_list_dn.ajax.url("/promo/approval-recon/list/dn-claimed?id=" + id).load(function () {
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
        $('#modal_list_dn').modal('show');
    });
});

$('#btn_list_dn_paid').on('click', function () {
    let e = document.querySelector("#btn_list_dn_paid");
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    $('#txt_title_list_dn').text('Debit Note Paid List');
    dt_list_dn.clear().draw()
    dt_list_dn.ajax.url("/promo/approval-recon/list/dn-paid?id=" + id).load(function () {
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
        $('#modal_list_dn').modal('show');
    });
});

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let formData = new FormData($('#form_promo_approval')[0]);
            formData.append('promoId', id);
            formData.append('categoryShortDescEnc', 'RC');
            let url = '/promo/approval-recon/approve';
            if ($('#approvalStatusCode').val() === "TP3") {
                url = '/promo/approval-recon/send-back';
            }
            $.get('/refresh-csrf').done(function(data) {
                $('meta[name="csrf-token"]').attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
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
                        e.setAttribute("data-kt-indicator", "on");
                        e.disabled = !0;
                    },
                    success: function(result, status, xhr, $form) {
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
                                window.location.href = '/promo/approval-recon';
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
                        console.log(jqXHR.message)
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
        let url = file_host + "/assets/media/promo/" + id + "/row" + row + "/" + attachment.val();
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
    let preview_path =  "/promo/approval-recon/preview-attachment?i="+id+"&r="+row+"&fileName="+encodeURIComponent(file_name)+"&t="+title;
    let w = window.innerWidth;
    let h = window.innerHeight;
    window.open(preview_path, "promoWindow", "location=1,status=1,scrollbars=1,width=" + w + ",height=" + h);
});

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/approval-recon/data/id",
            type: "GET",
            data: {id:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;
                    let channels = [], subChannels = [], accounts = [], subAccounts = [], regions = [], brands = [], skus = [], attachments = [];
                    let header = values.promoHeader;
                    if (header === null) {
                        return Swal.fire({
                            title: swalTitle,
                            text: "This promo does not have promo planning",
                            icon: "warning",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.location.href = '/promo/approval-recon';
                        });
                    }
                    regions = values.regions;
                    brands = values.brands;
                    skus = values.skus;
                    channels = values.channels;
                    subChannels = values.subChannels;
                    accounts = values.accounts;
                    subAccounts = values.subAccounts;

                    $('#txt_info_method').html('Promo ID '  + header.refId);

                    let dataApprovalAction = [];
                    for (let j = 0, len = values.listApprovalStatus.length; j < len; ++j){
                        dataApprovalAction.push({
                            id: values.listApprovalStatus[j].statusCode,
                            text: values.listApprovalStatus[j].statusDesc,
                        });
                    }
                    $('#approvalStatusCode').select2({
                        placeholder: "Select Approval Action",
                        width: '100%',
                        data: dataApprovalAction
                    });

                    $('#txt_senback_notes').text(header.sendback_notes_date);
                    $('#sendback_notes').text(header.sendback_notes);

                    // Detail
                    $('#period').val(new Date(header.startPromo).getFullYear());
                    $('#allocationRefId').val(header.allocationRefId);
                    $('#activityLongDesc').val(header.activityLongDesc);
                    $('#subActivityLongDesc').val(header.subActivityLongDesc);
                    $('#activityDesc').val(header.activityDesc);
                    $('#startPromo').val(formatDateOptima(header.startPromo) ?? "");
                    $('#endPromo').val(formatDateOptima(header.endPromo) ?? "");
                    $('#subCategoryDesc').val(header.subCategoryLongDesc);
                    $('#principalName').val(header.principalName);
                    $('#allocationDesc').val(header.allocationDesc);
                    $('#tsCoding').val(header.tsCoding);
                    $('#budgetAmount').val(formatMoney(header.budgetAmount, 0) ?? "0");
                    $('#remainingBudget').val(formatMoney(header.remainingBudget, 0) ?? "0");
                    $('#initiator_notes').val(header.initiator_notes);

                    // current
                    $('#planNormalSales').val(formatMoney(header.normalSales, 0) ?? "0");
                    $('#planIncrSales').val(formatMoney(header.incrSales, 0) ?? "0");
                    $('#planTotalSales').val(formatMoney(header.normalSales + header.incrSales, 0) ?? "0");
                    $('#planInvestment').val(formatMoney(header.investment, 0) ?? "0");
                    $('#planRoi').val(formatMoney(header.roi, 2) ?? "0");
                    $('#planCostRatio').val(formatMoney(header.costRatio, 2) ?? "0");

                    // Previous
                    $('#normalSales').val(formatMoney(header.lastNormalSales, 0) ?? "0");
                    $('#incrSales').val(formatMoney(header.lastIncrSales, 0) ?? "0");
                    $('#prevTotSales').val(formatMoney(header.lastTotSales, 0) ?? "0");
                    $('#investment').val(formatMoney(header.lastInvestment, 0) ?? "0");
                    $('#roi').val(formatMoney(header.lastRoi, 2) ?? "0");
                    $('#costRatio').val(formatMoney(header.lastCostRatio, 2) ?? "0");
                    $('#totalClaim').val(formatMoney(header['totalClaim'], 0) ?? "0");
                    $('#totalPaid').val(formatMoney(header['totalPaid'], 0) ?? "0");

                    // Detail Attribute
                    if (channels.length > 0) {
                        channels.forEach(function (el) {
                            if (el.flag) $('#channel').val(el.longDesc);
                        });
                    }

                    if (subChannels.length > 0) {
                        subChannels.forEach(function (el) {
                            if (el.flag) $('#subChannels').val(el.longDesc);
                        });
                    }

                    if (accounts.length > 0) {
                        accounts.forEach(function (el) {
                            if (el.flag) $('#accounts').val(el.longDesc);
                        });
                    }

                    if (subAccounts.length > 0) {
                        subAccounts.forEach(function (el) {
                            if (el.flag) $('#subAccounts').val(el.longDesc);
                        });
                    }

                    dt_mechanism.rows.add(values.mechanism).draw();

                    // Region
                    if (regions.length > 0) {
                        let longDescRegion = '';
                        regions.forEach(function (el) {
                            if (el.flag) longDescRegion += '<span class="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        });
                        $('#card_list_region').html(longDescRegion);
                    }

                    // Brand
                    if (brands.length > 0) {
                        let longDescBrand = '';
                        brands.forEach(function (el) {
                            if (el.flag) longDescBrand += '<span class="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        });
                        $('#card_list_brand').html(longDescBrand);
                    }

                    // SKU
                    if (skus.length > 0) {
                        let longDescSKU = '';
                        skus.forEach(function (el) {
                            if (el.flag) longDescSKU += '<span class="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        });
                        $('#card_list_sku').html(longDescSKU);
                    }

                    // Attachment
                    if (values.attachments) {
                        values.attachments.forEach((item, index, arr) => {
                            if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).val(item.fileName);
                                $('#btn_download' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                                $('#btn_view' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                            }
                        });
                    }

                    // Notes
                    $('#notes_message').html(header.createBy + ' <span class="ms-2 text-gray-700">'+ (formatDate(header.modifiedOn) ?? "") +'</span><div class="row ms-2 text-gray-700">' + header.notes + '</div>');
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete:function(){
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

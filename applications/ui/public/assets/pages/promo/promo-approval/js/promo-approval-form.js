'use strict';

var swalTitle = "Promo Approval";
var dt_mechanism, promoId, validator;

$(document).ready(function () {
    let url_str = new URL(window.location.href);
    promoId = url_str.searchParams.get("id");

    blockUI.block();
    Promise.all([ getData(promoId) ]).then(async () => {
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

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let formData = new FormData($('#form_promo_approval')[0]);
            formData.append('promoId', promoId);
            formData.append('categoryShortDescEnc', 'RC');
            let url = '/promo/approval/approve';
            if ($('#approvalStatusCode').val() === "TP3") {
                url = '/promo/approval/send-back';
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

const getData = (promoId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/approval/data/id",
            type: "GET",
            data: {id: promoId},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    if (values.promoHeader === null) {
                        return Swal.fire({
                            title: swalTitle,
                            text: "This promo does not have promo planning",
                            icon: "warning",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.location.href = '/promo/approval';
                        });
                    } else {
                        $('#txt_info_method').text('Edit ' + (values.promoHeader.refId ?? ""));

                        $('#txt_senback_notes').text(values.promoHeader.sendback_notes_date);
                        $('#sendback_notes').text(values.promoHeader.sendback_notes);

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

                        // header
                        $('#period').val(new Date(values.promoHeader.startPromo).getFullYear());
                        $('#promoPlanRefId').val(values.promoHeader.promoPlanRefId);
                        $('#allocationRefId').val(values.promoHeader.allocationRefId);
                        $('#allocationDesc').val(values.promoHeader.allocationDesc);
                        $('#activityLongDesc').val(values.promoHeader.activityLongDesc);
                        $('#subActivityLongDesc').val(values.promoHeader.subActivityLongDesc);
                        $('#activityDesc').val(values.promoHeader.activityDesc);
                        $('#startPromo').val(formatDateOptima(values.promoHeader.startPromo) ?? "");
                        $('#endPromo').val(formatDateOptima(values.promoHeader.endPromo) ?? "");
                        $('#subCategoryDesc').val(values.promoHeader.subCategoryDesc);
                        $('#principalName').val(values.promoHeader.principalName);
                        $('#tsCoding').val(values.promoHeader.tsCoding);
                        $('#budgetAmount').val(formatMoney(values.promoHeader.budgetAmount, 0) ?? "0");
                        $('#remainingBudget').val(formatMoney(values.promoHeader.remainingBudget, 0) ?? "0");
                        $('#initiatorNotes').val(values.promoHeader.initiator_notes);

                        // Promo Planning
                        $('#txt_promoPlanRefId').text("Promo Planning - " + values.promoHeader.promoPlanRefId);
                        $('#planNormalSales').val(formatMoney(values.promoHeader.planNormalSales, 0) ?? "0");
                        $('#planIncrSales').val(formatMoney(values.promoHeader.planIncrSales, 0) ?? "0");
                        $('#planTotalSales').val(formatMoney(values.promoHeader.planTotSales, 0) ?? "0");
                        $('#planInvestment').val(formatMoney(values.promoHeader.planInvestment, 0) ?? "0");
                        $('#planRoi').val(formatMoney(values.promoHeader.planRoi, 2) ?? "0");
                        $('#planCostRatio').val(formatMoney(values.promoHeader.planCostRatio, 2) ?? "0");

                        // Promo Creation
                        $('#normalSales').val(formatMoney(values.promoHeader.normalSales, 0) ?? "0");
                        $('#incrSales').val(formatMoney(values.promoHeader.incrSales, 0) ?? "0");
                        $('#prevTotSales').val(formatMoney(values.promoHeader.prevTotSales, 0) ?? "0");
                        $('#investment').val(formatMoney(values.promoHeader.investment, 0) ?? "0");
                        $('#roi').val(formatMoney(values.promoHeader.roi, 2) ?? "0");
                        $('#costRatio').val(formatMoney(values.promoHeader.costRatio, 2) ?? "0");

                        // Detail Attribute
                        if (values.channels.length > 0) {
                            values.channels.forEach(function (el) {
                                if (el.flag) $('#channel').val(el.longDesc);
                            });
                        }

                        if (values.subChannels.length > 0) {
                            values.subChannels.forEach(function (el) {
                                if (el.flag) $('#subChannels').val(el.longDesc);
                            });
                        }

                        if (values.accounts.length > 0) {
                            values.accounts.forEach(function (el) {
                                if (el.flag) $('#accounts').val(el.longDesc);
                            });
                        }

                        if (values.subAccounts.length > 0) {
                            values.subAccounts.forEach(function (el) {
                                if (el.flag) $('#subAccounts').val(el.longDesc);
                            });
                        }

                        dt_mechanism.rows.add(values.mechanism).draw();

                        // Region
                        if (values.regions.length > 0) {
                            let longDescRegion = '';
                            values.regions.forEach(function (el) {
                                if (el.flag) longDescRegion += '<span class="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                            });
                            $('#card_list_region').html(longDescRegion);
                        }

                        // Brand
                        if (values.brands.length > 0) {
                            let longDescBrand = '';
                            values.brands.forEach(function (el) {
                                if (el.flag) longDescBrand += '<span class="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                            });
                            $('#card_list_brand').html(longDescBrand);
                        }

                        // SKU
                        if (values.skus.length > 0) {
                            let longDescSKU = '';
                            values.skus.forEach(function (el) {
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
                        $('#notes_message').html(values.promoHeader.createBy + ' <span class="ms-2 text-gray-700">'+ (formatDate(values.promoHeader.modifiedOn) ?? "") +'</span><div class="row ms-2 text-gray-700">' + values.promoHeader.notes + '</div>');
                    }
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
            complete: function () {
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


'use strict';

let swalTitle = "Promo Cancel Request";
let id, validator, createBy;
let c, groupBrandId, promoId, promoRefId, promoPlanningId, categoryId, subCategoryId, listChannel = [], listSubChannel = [], listSubActivityType = [];


$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    promoId = url_str.searchParams.get("id");

    Promise.all([ blockUI.block() ]).then(async () => {
        await getData(promoId)
        blockUI.release();
    });

});

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/cancel-request/data/id",
            type: "GET",
            data: {promoId: id},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
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
                            window.location.href = '/promo/approval';
                        });
                    }

                    $('#txt_info_method').html('View '  + header.refId);

                    categoryId = header.categoryId;
                    subCategoryId = header.subCategoryId;
                    promoRefId = header.refId;
                    promoPlanningId = header.promoPlanId;
                    createBy = header.createBy;

                    // Set Channel
                    let strChannel = "";
                    if (values.channels) {
                        let arrChannels = [];
                        for (let i = 0; i < values.channels.length; i++) {
                            if (values.channels[i].flag) {
                                arrChannels.push(values.channels[i].longDesc);
                            }
                        }
                        if(arrChannels.length > 0) {
                            strChannel += arrChannels.reduce((text, value, i, array) => text + (i < array.length - 1 ? ', ' : ', ') + value);
                        }
                    }
                    $('#channel').val(strChannel);

                    // Set Brand
                    if (values.groupBrand) {
                        for (let i = 0; i < values.groupBrand.length; i++) {
                            if (values.groupBrand[i].flag) {
                                groupBrandId = values.groupBrand[i].id;
                                $('#brand').val(values.groupBrand[i].longDesc);
                            }
                        }
                    }

                    $('#period').val(new Date(header.startPromo).getFullYear());
                    $('#allocationRefId').val(header.allocationRefId);
                    $('#allocationDesc').val(header.allocationDesc);
                    $('#entity').val(header.principalName);
                    $('#distributor').val(header.distributorName);
                    $('#subCategory').val(header.subCategoryLongDesc);
                    $('#budgetDeployed').val(formatMoney(header.budgetAmount,  0));
                    $('#budgetRemaining').val(formatMoney(header.remainingBudget, 0));
                    $('#subActivity').val(header.subActivityLongDesc);
                    $('#startPromo').val(formatDateOptima(header.startPromo) ?? "");
                    $('#endPromo').val(formatDateOptima(header.endPromo) ?? "");
                    $('#initiatorNotes').val(header.initiator_notes);
                    $('#notes').val(values.promoHeader.cancelReason);

                    // Promo Creation
                    $('#investment').val(formatMoney(header.investment, 0) ?? "0");

                    // Attribute Mechanism
                    if (values.mechanisms) {
                        for (let i = 0; i < values.mechanisms.length; i++) {
                            $('#mechanism1').val(values.mechanisms[i].mechanism);
                        }
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
                    $('#notes_message').html(header.createBy + ' <span class="ms-2 text-gray-700">'+ (formatDate(header.createOn) ?? "") +'</span><div class="row ms-2 text-gray-700">' + header.notes + '</div>');
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

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    let formData = new FormData($('#form_promo_approval')[0]);
    formData.append('promoId', promoId);
    formData.append('promoPlanningId', promoPlanningId);
    formData.append('notes', $('#notes').val());
    formData.append('promoRefId', promoRefId);
    formData.append('createdBy', createBy);

    let url = '/promo/cancel-request/approve';
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
                    }).then(function () {
                        window.location.href = '/promo/cancel-request';
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
});

$('#btn_reject').on('click', function () {
    let e = document.querySelector("#btn_reject");
    let formData = new FormData($('#form_promo_approval')[0]);
    formData.append('promoId', promoId);
    formData.append('promoPlanningId', promoPlanningId);
    formData.append('notes', $('#notes').val());
    formData.append('promoRefId', promoRefId);
    formData.append('createdBy', createBy);

    let url = '/promo/cancel-request/reject';
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
                    }).then(function () {
                        window.location.href = '/promo/cancel-request';
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
                            a.download = attachment.val();
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
                                    customClass: {confirmButton: "btn btn-optima"},
                                    closeOnConfirm: false,
                                    showLoaderOnConfirm: false,
                                    closeOnClickOutside: false,
                                    closeOnEsc: false,
                                    allowOutsideClick: false,
                                });
                            });
                    } else {
                        Swal.fire({
                            title: "Download Attachment",
                            text: "Attachment not found",
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
                    }
                });
        }
    }
});

$('.btn_view').on('click', function () {
    let row = $(this).val();
    let attachment = $('#review_file_label_' + row);
    let file_name = attachment.val();
    let title = $('#txt_info_method').text();
    let preview_path =  "/promo/display/preview-attachment?i="+promoId+"&r="+row+"&fileName="+encodeURIComponent(file_name)+"&t="+title;
    let w = window.innerWidth;
    let h = window.innerHeight;
    window.open(preview_path, "promoWindow", "location=1,status=1,scrollbars=1,width=" + w + ",height=" + h);
});

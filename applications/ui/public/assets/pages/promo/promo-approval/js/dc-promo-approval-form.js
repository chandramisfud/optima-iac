'use strict';

let swalTitle = "Promo Approval";
let id, validator;
let c, groupBrandId, categoryId, subCategoryId, listChannel = [], listSubChannel = [], listSubActivityType = [];

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    id = url_str.searchParams.get("id");

    blockUI.block();
    Promise.all([ getData(id) ]).then(async () => {
        await getSubActivityType(categoryId).then(function () {
            if (listSubActivityType) {
                for (let i = 0; i < listSubActivityType.length; i++) {
                    if (listSubActivityType[i].id == subCategoryId) {
                        $('#subCategory').val(listSubActivityType[i].text);
                    }
                }
            }
        });
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
            formData.append('promoId', id);
            formData.append('categoryShortDescEnc', 'DC');
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
    let preview_path =  "/promo/approval/preview-attachment?i="+id+"&r="+row+"&fileName="+encodeURIComponent(file_name)+"&t="+title;
    let w = window.innerWidth;
    let h = window.innerHeight;
    window.open(preview_path, "promoWindow", "location=1,status=1,scrollbars=1,width=" + w + ",height=" + h);
});

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/approval/data/id",
            type: "GET",
            data: {id:id},
            dataType: "JSON",
            success: async function (result) {
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
                    categoryId = header.categoryId;
                    subCategoryId = header.subCategoryId;

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
                    $('#budgetDeployed').val(formatMoney(header.budgetAmount, 0));
                    $('#budgetRemaining').val(formatMoney(header.remainingBudget, 0));
                    $('#subActivity').val(header.subActivityLongDesc);
                    $('#startPromo').val(formatDateOptima(header.startPromo) ?? "");
                    $('#endPromo').val(formatDateOptima(header.endPromo) ?? "");
                    $('#initiatorNotes').val(header.initiator_notes);

                    // Promo Creation
                    $('#investment').val(formatMoney(header.investment, 0) ?? "0");

                    // Attribute Mechanism
                    if (values.mechanism) {
                        for (let i = 0; i < values.mechanism.length; i++) {
                            $('#mechanism1').val(values.mechanism[i].mechanism);
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

const getSubActivityType = (categoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/promo/approval/list/sub-activity-type",
            type        : "GET",
            data        : {CategoryId: categoryId},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                for (let j = 0, len = result.data.length; j < len; ++j){
                    listSubActivityType.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
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

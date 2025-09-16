'use strict';

let swalTitle = "Promo Display";
let id, validator;
let c, groupBrandId, promoId, categoryId, subCategoryId, listChannel = [], listSubChannel = [], listSubActivityType = [];

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    promoId = url_str.searchParams.get("id");

    blockUI.block();
    Promise.all([ getData(promoId) ]).then(async () => {
        blockUI.release();
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
    let fileName = attachment.val();
    let preview_path = '/dn/promo-display/view-file?promoId=' + promoId + "&docLink=row" + row + "&fileName=" + encodeURIComponent(fileName);

    window.open(preview_path, "_blank");
});

const getData = (promoId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dn/promo-display/get-data/id",
            type: "GET",
            data: {promoId:promoId},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;
                    let header = values.promoHeader;

                    $('#txt_info_method').html('View '  + header.refId);

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
                        if(arrChannels.length > 0){
                            strChannel += arrChannels.reduce((text, value, i, array) => text + (i < array.length - 1 ? ', ' : ', ') + value);
                        }
                    }
                    $('#channel').val(strChannel);

                    // Set Brand
                    if (values.groupBrand) {
                        for (let i = 0; i < values.groupBrand.length; i++) {
                            if (values.groupBrand[i].flag) {
                                $('#brand').val(values.groupBrand[i].longDesc);
                            }
                        }
                    }

                    $('#period').val(new Date(header.startPromo).getFullYear());
                    $('#allocationRefId').val(header.allocationRefId);
                    $('#allocationDesc').val(header.allocationDesc);
                    $('#entity').val(header.principalName);
                    $('#distributor').val(header.distributorName);
                    $('#subCategory').val(header.subCategoryDesc);
                    $('#budgetDeployed').val(formatMoney(header.budgetAmount,  0));
                    $('#budgetRemaining').val(formatMoney(header.remainingBudget, 0));
                    $('#subActivity').val(header.subActivityLongDesc);
                    $('#startPromo').val(formatDateOptima(header.startPromo) ?? "");
                    $('#endPromo').val(formatDateOptima(header.endPromo) ?? "");
                    $('#initiatorNotes').val(header.initiator_notes);

                    // Promo Creation
                    $('#investment').val(formatMoney(header.investment, 0) ?? "0");
                    $('#investmentBfrClose').val(formatMoney(header.investmentBfrClose, 0) ?? "0");
                    $('#investmentClosedBalance').val(formatMoney(header.investmentClosedBalance, 0) ?? "0");

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

$('#btn_export_pdf').on('click', function() {
    window.open( "/dn/promo-display/export-pdf?id=" + promoId , "_blank");
});

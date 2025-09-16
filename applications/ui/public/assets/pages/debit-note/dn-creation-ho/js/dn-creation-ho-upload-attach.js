'use strict';

var swalTitle = "Debit Note";
var id, method, arrTaxLevel = [], listEntity = [];
var dnId=0, promoId=0, isDNPromo=1, startPromo, endPromo, entityId, subAccountId, subAccountDesc, taxLevelId, stsPPN, stsPPH;

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    id = url_str.searchParams.get("id");

    blockUI.block();
    Promise.all([getListTaxLevel()]).then(async () => {
        await getData(id);
        blockUI.release();
    });

});

$('#isDNPromo').on('change', function () {
    if(this.checked) {
        console.log('check')
        isDNPromo = 1;
        $('#isDNPromo').val("1");
    } else {
        console.log('uncheck')
        isDNPromo = 0;
        $('#isDNPromo').val("0");
    }
});

$('.btn_delete').on('click', function () {
    let row = this.value;
    let data = $('#review_file_label_' + row).text();
    let file = document.getElementById('attachment' + row).files[0];
    swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this file",
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it',
        cancelButtonText: 'No, cancel',
        reverseButtons: true,
        allowOutsideClick: false,
        allowEscapeKey: false,
    }).then(function (result) {
        if(result.value){
            if (data !== ''){
                // let form_data;
                let formData = new FormData($('#form_dn')[0]);

                if (method == 'update') {
                    formData.append('dnId', id);
                    formData.append('mode', 'edit');
                } else if(method == 'upload') {
                    formData.append('dnId', id);
                    formData.append('mode', 'uploadattach');
                } else {
                    formData.append('dnId', dnId);
                }
                formData.append('row', 'row' + row);
                formData.append('fileName', data);
                formData.append('file', file);

                $.ajax({
                    url         : '/dn/creation-ho/delete-attachment',
                    data        : formData,
                    type        : 'POST',
                    async       : true,
                    dataType    : 'JSON',
                    cache       : false,
                    contentType : false,
                    processData : false,
                    success: function(result, status, xhr, $form) {
                        let swalType;
                        if (!result.error) {
                            swalType = 'success';
                            $('#review_file_label_' + row).removeClass('form-control-solid-bg');
                            $('#attachment' + row).attr('disabled', false);
                            $('#review_file_label_' + row).text('Choose File').attr('title', '');
                            // $('#attachment' + row).val('');
                            $('#info' + row).addClass('d-none');
                            $('#offset' + row).removeClass('d-none');
                        } else {
                            swalType = 'error';
                        }
                        Swal.fire({
                            title: 'File Deleted',
                            text: result.message,
                            icon: swalType,
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
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
            }
        }
    });
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");
    let formData = new FormData($('#form_dn')[0]);
    let sellpoint = [];
    sellpoint.push($("#sellingPoint").val());
    formData.append('sellpoint', JSON.stringify(sellpoint));
    formData.append('entity', entityId);
    formData.append('accountDesc', subAccountDesc);
    formData.append('accountId', subAccountId);
    formData.append('promoId', promoId);
    formData.append('taxLevel', taxLevelId);
    formData.append('statusPPN', stsPPN);
    formData.append('statusPPH', stsPPH);

    let url;
    if (method) {
        url = '/dn/creation-ho/update';
        formData.append('id', id);
        formData.append('mode', 'uploadattach');
    } else {
        url = '/dn/creation-ho/save';
    }

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
                    text: 'Update Debet Note ID ' + result.refId + ' has been successfuly',
                    icon: "success",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    window.location.href = '/dn/creation-ho';
                });
            } else {
                if (result.status === 666) { //data double
                    let strMsgH = '<table class="text-start">\
                            <tr><td>DN Number</td><td>:</td><td class="ps-2">' + result.refId + '</td></tr>\
                            <tr><td>Distributor</td><td>:</td><td class="ps-2">' + result.distributor + '</td></tr>\
                            <tr><td>Entity</td><td>:</td><td class="ps-2">' + result.entity + '</td></tr>'
                    let strMsgF = '</table>'
                    let strMsgActivityDesc = "";
                    if(result.activitdesc !== "none"){
                        strMsgActivityDesc = '<tr><td>DN Description</td><td>:</td><td>' + result.activitdesc + '</td></tr>'
                    }
                    let strMsgIntDocNo = "";
                    if(result.intdocno !== "none"){
                        strMsgIntDocNo = '<tr><td>Internal Doc No</td><td>:</td><td>' + result.intdocno + '</td></tr>'
                    }
                    let strMsg = strMsgH + strMsgActivityDesc + strMsgIntDocNo + strMsgF
                    Swal.fire({
                        title: result.message,
                        html: strMsg,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                } else {
                    Swal.fire({
                        title: 'Warning',
                        text: result.message,
                        icon: "warning",
                        confirmButtonText: "Confirm",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
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

$('#attachment1, #attachment2, #attachment3, #attachment4, #attachment5, #attachment6').on('change', function () {
    let row = $(this).attr('data-row');
    let elLabel = $('#review_file_label_' + row);
    let oldNameFile = elLabel.text();
    let fileName = (this.files.length > 0) ? this.files[0].name : 'Choose File';
    elLabel.text(fileName).attr('title', fileName);
    if (this.files[0].size > 10000000) {
        swal.fire({
            type: 'warning',
            title: 'Warning',
            text: 'Maximum file size 10Mb',
            showConfirmButton: true,
            confirmButtonText: 'Confirm',
            allowOutsideClick: false,
        });
        elLabel.text(oldNameFile);
    } else if (checkNameFile(this.value)) {
        swal.fire({
            type: 'warning',
            title: 'Warning',
            text: 'File name has special characters /\:*<>?|#%" \n. These are not allowed\n',
            showConfirmButton: true,
            confirmButtonText: 'Confirm',
            allowOutsideClick: false,
        });
        elLabel.text(oldNameFile);
    } else {
        upload_file($(this), row);
    }
});

function checkNameFile(value) {
    var fullPath = value;
    if (fullPath) {
        var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
        var filename = fullPath.substring(startIndex);
        if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
            filename = filename.substring(1);
        }
    }
    var format = /[/\:*"<>?|#%]/;
    return format.test(filename);
}

const upload_file =  (el, row) => {
    let form_data = new FormData();
    let file = document.getElementById('attachment'+row).files[0];
    form_data.append('dnId', id);
    form_data.append('file', file);
    form_data.append('row', 'row'+row);
    form_data.append('docLink', el.val());
    form_data.append('mode', 'uploadattach');

    $.ajax({
        url: "/dn/creation-ho/upload",
        type: "POST",
        dataType: "JSON",
        data: form_data,
        cache: false,
        processData: false,
        contentType: false,
        async: true,
        beforeSend: function () {
            blockUI.block();
        },
        success: function (result) {
            let swalType = '';
            if (!result.error) {
                swalType = 'success';
                $('#info' + row).removeClass('d-none');
                $('#info' + row).html('<span class="badge badge-circle badge-success ms-2"><i class="fa fa-check text-white"></i></span>');
            } else {
                swalType = 'error';
                $('#info' + row).removeClass('d-none');
                $('#info' + row).html('<span class="badge badge-circle badge-danger ms-2"><i class="fa fa-times text-white"></i></span>');
            }
            $('#offset'+row).addClass('d-none');
            Swal.fire({
                title: 'File Upload',
                text: result.message,
                icon: swalType,
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        },
        complete: function () {
            blockUI.release();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#info' + row).html('<span class="badge badge-circle badge-danger ms-2"><i class="fa fa-times text-white"></i></span>');
        }
    });
}

const getData = (id) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/creation-ho/get-data/id",
            type: "GET",
            data: {id: id},
            dataType: 'json',
            async: true,
            success: async function (result) {
                if (!result.error) {
                    let value = result.data;
                    dnId = value.id;
                    entityId = value.entityId;
                    subAccountId = value.accountId;
                    subAccountDesc = value.accountDesc;
                    promoId = value.promoId;

                    $('#txt_info_method').text('Upload Attachment ' + value.refId + ' | ' + value.lastStatus);
                    $('#period').val(value.periode);
                    if( value.periode < 2024){
                        $('#text-warning').removeClass('d-none');
                        $('#trade-promo').removeClass('d-none');
                    }
                    $('#accountDesc').val(value.accountDesc);
                    if (value.sellpoint) {
                        if (value.sellpoint.length > 0) {
                            value.sellpoint.forEach(function(el) {
                                if (el.flag) {
                                    return $('#sellingPoint').val(el.sellpoint).trigger('change');
                                }
                            });
                        }
                    }
                    $('#promoRefId').val(value.promoRefId);
                    $('#entityLongDesc').val(value.entityLongDesc);
                    $('#entityAddress').val(value.entityAddress);
                    $('#entityUp').val(value.entityUp);
                    $('#activityDesc').val(value.activityDesc);
                    $('#feeDesc').val(value.feeDesc);
                    $('#dnCreator').val(value.dnCreator);
                    selectTaxLevel(value.taxLevel);
                    await matchTaxLevelFixing(value.taxLevel);
                    await matchTaxLevel(value.taxLevel);
                    $('#whtType').val(value.whtType);
                    $('#deductionDate').val(formatDate(value.deductionDate));
                    $('#memDocNo').val(value.memDocNo);
                    $('#intDocNo').val(value.intDocNo);
                    $('#periode').val((value.startPromo) ? value.startPromo + " to " + value.endPromo : '');
                    $('#dnAmount').val(formatMoney(value.dnAmount, 0));
                    $('#feePct').val(formatMoney(value.feePct, 0));
                    $('#feeAmount').val(formatMoney(value.feeAmount, 0));
                    $('#dpp').val(formatMoney(value.dpp, 0));
                    $('#statusPPN').val(selectPPN(value.statusPPN));
                    stsPPN = value.statusPPN;
                    $('#ppnPct').val(formatMoney(value.ppnPct, 0));
                    $('#ppnAmt').val(formatMoney(value.ppnAmt, 0));
                    $('#statusPPH').val(selectPPH(value.statusPPH));
                    stsPPH = value.statusPPH;
                    $('#pphPct').val(formatMoney(value.pphPct, 0));
                    $('#pphAmount').val(formatMoney(value.pphAmt, 0));
                    $('#total').val(formatMoney(value.totalClaim, 0));

                    if(value.isDNPromo == true){
                        isDNPromo = 1;
                        $('#isDNPromo').prop("checked", true)
                        $('#isDNPromo').attr("disabled", true)
                    } else {
                        isDNPromo = 0;
                        $('#isDNPromo').prop("checked", false)
                        $('#isDNPromo').attr("disabled", true)
                    }

                    $('#fpNumber').val(value.fpNumber);
                    $('#fpDate').val((value.fpNumber) ? formatDate(value.fpDate) : "");
                    if (value.dnattachment) {
                        value.dnattachment.forEach((item, index, arr) => {
                            if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).text(item.fileName).attr('title', item.fileName);
                                $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).addClass('form-control-solid-bg');
                                $('#attachment' + parseInt(item.docLink.replace('row', ''))).attr('disabled', true);
                            }
                        });
                    }
                }
            },
            complete: function(result) {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                if (jqXHR.status == 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR.responseJSON.message,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-optima" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                return reject(jqXHR.responseText);
            },
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListTaxLevel = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url         : "/dn/creation-ho/list/tax-level",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                arrTaxLevel = result.data;
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

const selectTaxLevel = (materialNumber) => {
    arrTaxLevel.forEach(function(el) {
        if (el.materialNumber === materialNumber) {
            return $('#taxLevel').val(el.materialNumber + " - " + el.description);
        }
    });
}

const matchTaxLevelFixing = (str) => {
    arrTaxLevel.forEach(function(el) {
        if (el.materialNumber + " - " + el.description === str) {
            taxLevelId = el.materialNumber;
            return $('#taxLevel').val(el.materialNumber + " - " + el.description);
        }
    });
}

const matchTaxLevel = (str) => {
    arrTaxLevel.forEach(function(el) {
        if (el.materialNumber === str) {
            taxLevelId = el.materialNumber;
            return $('#taxLevel').val(el.materialNumber + " - " + el.description);
        }
    });
}

const selectPPN = (str) => {
    switch (str) {
        case 'PPN DN AMOUNT': return 'PPN - DN Amount';
        case 'PPN FEE' : return 'PPN - Fee';
        case 'PPN DPP' : return 'PPN - DPP';

        case 'PPN - DN AMOUNT': return 'PPN - DN Amount';
        case 'PPN - FEE' : return 'PPN - Fee';
        case 'PPN - DPP' : return 'PPN - DPP';
        case '' : return '';
    }
}

const selectPPH = (str) => {
    switch (str) {
        case 'DN Amount PPH': return 'PPH - DN Amount';
        case 'FEE PPH' : return 'PPH - Fee';
        case 'DPP PPH' : return 'PPH - DPP';

        case 'DN - Amount PPH': return 'PPH - DN Amount';
        case 'FEE - PPH' : return 'PPH - Fee';
        case 'DPP - PPH' : return 'PPH - DPP';
        case '' : return '';
    }
}

const isJsonString = (str) => {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}

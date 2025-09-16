'use strict';

let swalTitle = "Debit Note";
let elPeriod = $('#period');
let id, method, dialerObject, validator, arrTaxLevel = [], listEntity = [];
let dnId=0, promoId=0, isDNPromo=1, lastStatus, isCancel, dnRefId, startPromo, endPromo, entityId, refId, taxLevelId, dnWhtType, whtType = "";

(function(window, document, $) {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#deductionDate, #fpDate').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        digits: 0,
        groupSeparator: ",",
    }).mask("#dnAmount, #feeAmount, #dpp, #ppnAmount, #pphAmount, #total, #feePct");

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        digits: 4,
        groupSeparator: ",",
    }).mask("#ppnPct, #pphPct");

})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    id = url_str.searchParams.get("id");

    if (method === 'update') {
        blockUI.block();
        disableButtonSave();
        Promise.all([getListWHTType(), getListSubAccount(), getListSellingPoint(), getListEntity()]).then(async () => {
            $('#download-zip').removeClass('d-none');
            await getData(id);
            await getDataWHTTypePromo();

            // Select WHT Type
            if (dnWhtType !== "") {
                $('#whtType').val(dnWhtType).trigger('change.select2');
            } else {
                if (whtType !== "") {
                    $('#whtType').val(whtType).trigger('change.select2');
                } else {
                    $('#whtType').val("").trigger('change.select2');
                }
            }

            let elStatusPPN = $("#statusPPN");
            let elPpnPct = $("#ppnPct");
            let elStatusPPH = $("#statusPPH");
            let elPphPct = $("#pphPct");
            // Event Driven PPH
            if (dnWhtType !== "") {
                if (dnWhtType === "Non WHT Object") {
                    elStatusPPH.attr("readonly", "readonly");
                    elPphPct.attr("readonly", "readonly");
                    elStatusPPH.val('').trigger('change.select2');
                    elPphPct.val(0);
                } else {
                    elStatusPPH.attr("readonly", false);
                    elPphPct.attr("readonly", false);
                }
            } else {
                if(whtType === 'Non WHT Object') {
                    elStatusPPH.attr("readonly", "readonly");
                    elPphPct.attr("readonly", "readonly");
                } else {
                    elStatusPPH.attr("readonly", false);
                    elPphPct.attr("readonly", false);
                }
            }

            await getDataTaxLevel(entityId, $('#whtType').val());
            $('#taxLevel').val(taxLevelId).trigger('change.select2');

            enableButtonSave();
            if (isCancel) {
                disableButtonSave();
                $('.btn_delete').attr('disabled', true);
                $('#btn_search_promo').attr('disabled', true);
                $('#download_all').attr('disabled', true);
                Swal.fire({
                    title: swalTitle,
                    text: 'Debit Note ' + dnRefId + ' can not edit, because this debit note is cancel',
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    window.location.href = '/dn/creation-ho';
                });
            } else if (lastStatus === 'send_to_dist_ho' || lastStatus === 'created') {
                enableButtonSave();
                blockUI.release();
            } else {
                disableButtonSave();
                $('.btn_delete').attr('disabled', true);
                $('#btn_search_promo').attr('disabled', true);
                $('#download_all').attr('disabled', true);
                Swal.fire({
                    title: swalTitle,
                    text: 'Debit Note ' + dnRefId + ' can not edit, because this debit note status is ' + lastStatus,
                    icon: "warning",
                    buttonsStyling: !1,
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    window.location.href = '/dn/creation-ho';
                });
            }
            if ($('#period').val() < 2024) {
                $('#text-warning').removeClass('d-none');
                $('#trade-promo').removeClass('d-none');
            }
            blockUI.release();
        });
    } else if (method === 'view') {
        blockUI.block();
        disableButtonSave();
        Promise.all([getListWHTType(), getListTaxLevel() ]).then(async () => {
            await getData(id);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        blockUI.block();
        disableButtonSave();
        Promise.all([getListWHTType(), getListSubAccount(), getListSellingPoint(), getListEntity()]).then(async () => {
            $('#btn_search_promo').removeClass('d-none');
            $('#isDNPromo').attr('disabled',true);
            enableButtonSave();
            blockUI.release();
        });
    }

    const v_fp_number = function () {
        return {
            validate: function () {
                let ppnAmount = parseFloat($('#ppnAmount').val().toString().replace(/,/g, ''));
                if (ppnAmount !== 0 && $('#fpNumber').val() === '') {
                    return {
                        valid: false,
                        message: 'Please fill in FP Number'
                    }
                } else {
                    return {
                        valid: true
                    }
                }
            }
        }
    }

    const v_fp_date = function () {
        return {
            validate: function () {
                let ppnAmount = parseFloat($('#ppnAmount').val().toString().replace(/,/g, ''));
                if (ppnAmount !== 0 && $('#fpNumber').val() === '') {
                    return {
                        valid: false,
                        message: 'Please fill in FP Date'
                    }
                } else {
                    return {
                        valid: true
                    }
                }
            }
        }
    }

    FormValidation.validators.v_fp_number = v_fp_number;
    FormValidation.validators.v_fp_date = v_fp_date;

    validator = FormValidation.formValidation(document.getElementById('form_dn'), {
        fields: {
            period: {
                validators: {
                    notEmpty: {
                        message: "This field is required"
                    },
                }
            },
            subAccountId: {
                validators: {
                    notEmpty: {
                        message: "Enter a Sub Account"
                    },
                }
            },
            activityDesc: {
                validators: {
                    notEmpty: {
                        message: "Enter a Description"
                    },
                    stringLength: {
                        max: 255,
                        message: 'Enter max 255 characters',
                    }
                }
            },
            entityId: {
                validators: {
                    notEmpty: {
                        message: "Enter an Entity"
                    },
                }
            },
            dnAmount: {
                validators: {
                    notEmpty: {
                        message: "This field is required"
                    },
                    greaterThan: {
                        min: 1,
                        message: "Can not be zero of DN Amount"
                    }
                }
            },
            memDocNo: {
                validators: {
                    stringLength: {
                        max: 20,
                        message: 'Enter max 20 characters',
                    }
                }
            },
            intDocNo: {
                validators: {
                    stringLength: {
                        max: 50,
                        message: 'Enter max 50 characters',
                    }
                }
            },
            feeDesc: {
                validators: {
                    stringLength: {
                        max: 255,
                        message: 'Enter max 255 characters',
                    }
                }
            },
            fpNumber: {
                validators: {
                    v_fp_number: {

                    }
                }
            },
            fpDate: {
                validators: {
                    v_fp_date: {

                    }
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

});

elPeriod.on('blur', function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

elPeriod.on('change', function () {
    if($(this).val() > 2023){
        let elIsDNPromo = $('#isDNPromo');
        isDNPromo = 1;
        elIsDNPromo.val("1");

        elIsDNPromo.attr('disabled',true);
        elIsDNPromo.prop("checked", true)

        $('#subAccountId').val('').trigger('change');
        $('#promoRefId').val('');
        validator.addField('subAccountId', {
            validators: {
                notEmpty: {
                    message: "Enter a Sub Account"
                },
            }
        });
        $('#subAccountID-iconRequired').addClass('required');
        $('#btn_search_promo').removeClass('d-none');

        $('#text-warning').addClass('d-none');
        $('#trade-promo').addClass('d-none');
        promoId=0;
        if(method !== 'update'){
            entityId = 0;
            $('#entityId').val('').trigger('change');
        }
    } else {
        $('#isDNPromo').attr('disabled',false);
        $('#text-warning').removeClass('d-none');
        $('#trade-promo').removeClass('d-none');

        promoId=0;
        if(method !== 'update'){
            entityId = 0;
            $('#entityId').val('').trigger('change');
        }
        $('#subAccountId').val('').trigger('change');
        $('#promoRefId').val('');
    }
});

$('#btn_search_promo').on('click',  function () {
    let filter_period = ($('#period').val()) ?? "";
    let filter_entity = ($('#entityId').val()) ?? "";
    let account_id = ($('#subAccountId').val()) ?? "";

    if(filter_entity === "" || filter_entity==null) { filter_entity = "0"; }
    if(account_id === "" || account_id==null) { account_id = "0"; }

    dt_promo_list.clear().draw()

    let url = "/dn/creation-ho/get-data/promo?period=" +  filter_period + "&subAccountId=" + account_id + "&entityId=" + filter_entity + "&channelId=0";
    dt_promo_list.ajax.url(url).load();

    if (method !== 'update') {
        $('#filter_entity').val('').trigger('change');
    } else {
        $('#filter_entity').val(filter_entity).trigger('change');
    }
    $('#filter_channel').val('').trigger('change');
    $('#modal_list_promo').modal('show');
});

$('#promoRefId').on('change', async function () {
    if ($(this).val() === "") {
        $('#whtType').val('').trigger('change');
    } else {
        await getDataWHTTypePromo();
        $('#whtType').val(whtType).trigger('change');
    }
});

$('#subAccountId').on('change', function () {
    let elPromoRefId = $('#promoRefId');
    promoId=0;
    elPromoRefId.val('');
    $('#periode').val('');
    if(method !== 'update'){
        $("#entityId").attr("readonly", false);
    }
});

$('#entityId').on('change', async function () {
    blockUI.block();
    let data = $('#entityId').select2('data')
    $("#entityUp").val(data[0].entityUp)
    $("#entityAddress").val(data[0].entityAddress);

    blockUI.release();
});

$('#whtType').on('change', async function () {
    let elStatusPPH = $("#statusPPH");
    let elPphPct = $("#pphPct");
    let elEntity = $('#entityId');
    let elTaxLevel = $('#taxLevel');

    if ($(this).val() === "WHT No Deduct") {
        $('#icon_info_wht').removeClass('d-none');
    } else {
        $('#icon_info_wht').addClass('d-none');
    }

    elTaxLevel.empty();
    await getDataTaxLevel(elEntity.val(), $(this).val());
    entityId = elEntity.val();
    elTaxLevel.val('').trigger('change');

    if($(this).val() === 'Non WHT Object'){
        elStatusPPH.attr("readonly", "readonly");
        elPphPct.attr("readonly", "readonly");
        elStatusPPH.val('').trigger('change');
        elPphPct.val(0);
    } else {
        elStatusPPH.attr("readonly", false);
        elPphPct.attr("readonly", false);
    }
    calculation();
});

$('#taxLevel').on('change', function () {
    if ($(this).val()) {
        let data = $('#taxLevel').select2('data');
        let ppn = parseFloat(data[0].ppn.toString().replace(/,/g, ''));
        let pph = parseFloat(data[0].pph.toString().replace(/,/g, ''));

        $('#ppnPct').val(ppn);
        $('#pphPct').val(pph);
        if ($('#statusPPN').val() !== '') {
            calculation();
        }
        taxLevelId = $(this).val();
    }
});

$('#isDNPromo').on('change', function () {
    if(this.checked) {
        isDNPromo = 1;
        $('#isDNPromo').val("1");

        $('#subAccountId').val('').trigger('change');
        $('#promoRefId').val('');
        if(method !== 'update'){
            $("#entityId").attr("readonly", false);
        }
        validator.addField('subAccountId', {
            validators: {
                notEmpty: {
                    message: "Enter a Sub Account"
                },
            }
        });
        $('#subAccountID-iconRequired').addClass('required');
    } else {
        isDNPromo = 0;
        $('#isDNPromo').val("0");

        $('#subAccountId').val('').trigger('change');
        $('#promoRefId').val('');
        if(method !== 'update'){
            $("#entityId").attr("readonly", false);
        }
        validator.removeField('subAccountId');
        $('#subAccountID-iconRequired').removeClass('required');
        promoId = 0;
    }
    $('#btn_search_promo').toggleClass('d-none');
});

$('#info_wht_type').on('mouseenter', function () {
    $('.tooltip_custom .tooltip_text').css('visibility', 'visible').css('opacity', 1);
}).on('mouseleave', function() {
    $('.tooltip_custom .tooltip_text').css('visibility', 'hidden').css('opacity', 0);
});

$('#dnAmount').on('keyup', function () {
    calculation();
}).on('change', function () {
    if ($(this).val() === "") {
        $(this).val('0');
        calculation();
    }
});

$('#feePct').on('keyup', function () {
    calculation();
}).on('change', function () {
    if ($(this).val() === "") {
        $(this).val('0');
        calculation();
    }
});

$('#statusPPN').on('change', function () {
    calculation();
});

$('#ppnPct').on('keyup', function () {
    if($('#statusPPN').val() !== '') {
        calculation();
    }
}).on('change', function () {
    if ($(this).val() === "") {
        $(this).val('0');
        calculation();
    }
});

$('#statusPPH').on('change', function () {
    calculation();
});

$('#pphPct').on('keyup', function () {
    if($('#statusPPH').val() !== '') {
        calculation();
    }
}).on('change', function () {
    if ($(this).val() === "") {
        $(this).val('0');
        calculation();
    }
});

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#attachment' + row);
        let url = file_host + "/assets/media/debitnote/" + id + "/row" + row + "/" + attachment.val();
        if (attachment.val() !== "") {
            fetch(url).then((resp) => {
                if (resp.ok) {
                    resp.blob().then(blob => {
                        const url_blob = window.URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.style.display = 'none';
                        a.href = url_blob;
                        a.download = $('#attachment' + row).val();
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

$('#download_all').on('click', function () {
    let url = "/dn/creation-ho/download-zip?id=" + id;
    let xmlhttp = new XMLHttpRequest();
    xmlhttp.open('GET', url, true);
    xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");

    xmlhttp.onreadystatechange = function(result) {
        let target = result.target;
        if(target.readyState === 4 && target.status === 200) {
            if (isJsonString(target.response)) {
                let response = JSON.parse(target.response);
                if (response.error){
                    Swal.fire({
                        title: "Download ZIP",
                        text: "File " + id + ".zip does not exist",
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
                } else {
                    window.location = url
                }
            } else {
                window.location = url
            }
        }
    }
    xmlhttp.send();
});

$('#btn_save').on('click', function () {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save");
            let formData = new FormData($('#form_dn')[0]);
            let sellpoint = [];
            let subAccount = $('#subAccountId').select2('data');
            sellpoint.push($("#sellingPointId").val());
            formData.append('sellpoint', JSON.stringify(sellpoint));
            formData.append('entity', entityId);
            if(subAccount.length > 0){
                formData.append('accountDesc', subAccount[0].text);
                formData.append('accountId', subAccount[0].id);
            }
            formData.append('promoId', promoId);
            formData.append('isDnPromo', isDNPromo);

            let attachment1 = $('#attachment1').val().split('\\').pop();
            let attachment2 = $('#attachment2').val().split('\\').pop();
            let attachment3 = $('#attachment3').val().split('\\').pop();
            let attachment4 = $('#attachment4').val().split('\\').pop();
            let attachment5 = $('#attachment5').val().split('\\').pop();
            let attachment6 = $('#attachment6').val().split('\\').pop();

            formData.append('file_name_attachment1', attachment1.toString());
            formData.append('file_name_attachment2', attachment2.toString());
            formData.append('file_name_attachment3', attachment3.toString());
            formData.append('file_name_attachment4', attachment4.toString());
            formData.append('file_name_attachment5', attachment5.toString());
            formData.append('file_name_attachment6', attachment6.toString());

            let url;
            if (method) {
                url = '/dn/creation-ho/update';
                formData.append('id', id);
                formData.append('taxLevelId', taxLevelId);
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
                success: function(result) {
                    if (!result['error']) {
                        let methodMessage;
                        if (method === 'update') {
                            methodMessage = 'Update Debet Note ID ';
                        } else {
                            methodMessage = 'Save Debet Note ID ';
                        }
                        Swal.fire({
                            title: swalTitle,
                            text: methodMessage + result['refId'] + ' has been successfuly',
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.location.href = '/dn/creation-ho';
                        });
                    } else {
                        if (result['status'] === 666) { //data double
                            let strMsgH = '<table class="text-start">\
                                    <tr><td>DN Number</td><td>:</td><td class="ps-2">' + result['refId'] + '</td></tr>\
                                    <tr><td>Distributor</td><td>:</td><td class="ps-2">' + result['distributor'] + '</td></tr>\
                                    <tr><td>Entity</td><td>:</td><td class="ps-2">' + result['entity'] + '</td></tr>'
                            let strMsgF = '</table>'
                            let strMsgActivityDesc = "";
                            if(result['activitdesc'] !== "none"){
                                strMsgActivityDesc = '<tr><td>DN Description</td><td>:</td><td class="ps-2">' + result['activitdesc'] + '</td></tr>'
                            }
                            let strMsgIntDocNo = "";
                            if(result['intdocno'] !== "none"){
                                strMsgIntDocNo = '<tr><td>Internal Doc No</td><td>:</td><td class="ps-2">' + result['intdocno'] + '</td></tr>'
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
                            }).then(function () {
                                window.location.href = '/dn/creation-ho';
                            });
                        }
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
        } else {
            if (isDNPromo === 0) {
                $('#subAccountId').removeClass('is-invalid');
            }

            let form = document.getElementById("form_dn");
            let elements = form.elements;
            let txt = "";
            for (let i = 0, len = elements.length; i < len; ++i) {
                let el_class = elements[i];
                if (el_class.classList.contains('is-invalid')) {
                    txt += el_class.nextElementSibling.innerText + "<br/>";
                }
            }
            Swal.fire({
                title: "Data not valid",
                html: txt,
                icon: "warning",
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
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
                let formData = new FormData($('#form_dn')[0]);

                if (method === 'update') {
                    formData.append('dnId', id);
                    formData.append('mode', 'edit');
                } else if(method === 'upload') {
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
                    success: function(result) {
                        let swalType;
                        if (!result.error) {
                            swalType = 'success';
                            let elReviewFileLabel = $('#review_file_label_' + row);
                            elReviewFileLabel.removeClass('form-control-solid-bg');
                            $('#attachment' + row).attr('disabled', false);
                            elReviewFileLabel.text('Choose File').attr('title', '');
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
            }
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
            icon: 'warning',
            title: 'Warning',
            text: 'Maximum file size 10Mb',
            showConfirmButton: true,
            confirmButtonText: 'Confirm',
            allowOutsideClick: false,
        });
        elLabel.text(oldNameFile);
    } else if (checkNameFile(this.value)) {
        swal.fire({
            icon: 'warning',
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
    let fullPath = value;
    if (fullPath) {
        let startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
        var filename = fullPath.substring(startIndex);
        if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
            filename = filename.substring(1);
        }
    }
    let format = /[/:*"<>?|#%]/;
    return format.test(filename);
}

const upload_file =  (el, row) => {
    let form_data = new FormData();
    let file = document.getElementById('attachment' + row).files[0];
    if (method === 'update'){
        form_data.append('dnId', id);
        form_data.append('mode', 'edit');
    }else if(method === 'upload') {
        form_data.append('dnId', id);
        form_data.append('mode', 'uploadattach');
    } else {
        form_data.append('dnId', dnId);
    }
    form_data.append('file', file);
    form_data.append('row', 'row' + row);
    form_data.append('docLink', el.val());

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
            let swalType;
            $('#offset' + row).addClass('d-none');
            if (!result.error) {
                swalType = 'success';
                $('#info' + row).removeClass('d-none').removeClass('badge-danger').addClass('badge-success').html('<i class="fa fa-check text-white"></i>');
            } else {
                swalType = 'error';
                $('#info' + row).removeClass('d-none').removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
            }
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
            console.log(errorThrown)
            $('#info' + row).removeClass('d-none').removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
        }
    });
}

const calculation = () => {
    let elPpnAmount = $('#ppnAmount');
    let dnAmount = parseFloat(document.getElementById("dnAmount").value.replace(/,/g, ''));

    // Calculate Fee
    let feePct = parseFloat(document.getElementById("feePct").value.replace(/,/g, ''));
    let feeAmount = (dnAmount * feePct) / 100;
    $('#feeAmount').val(formatMoney(feeAmount,0));


    // Calculate DPP
    let dpp = dnAmount + feeAmount;
    $('#dpp').val(formatMoney(dpp,0));

    // Calculate PPN
    let statusPPN = $('#statusPPN').val();
    let ppnPerc = $('#ppnPct').val();
    let ppn = 0;
    switch (statusPPN) {
        case 'PPN DN AMOUNT':
            ppn = (dnAmount * ppnPerc) / 100;
            break;
        case 'PPN FEE':
            ppn = (feeAmount * ppnPerc) / 100;
            break;
        case 'PPN DPP':
            ppn = (dpp * ppnPerc) / 100;
            break;
    }
    $('#ppnpersen').val(ppnPerc);
    elPpnAmount.val(formatMoney(ppn,0));

    // Calculate PPH
    let statusPPH = $('#statusPPH').val();
    let pphPerc = $('#pphPct').val();
    let pph = 0;
    switch (statusPPH) {
        case 'FEE PPH':
            pph = (feeAmount * pphPerc) / 100;
            break;
        case 'DPP PPH':
            pph = (dpp * pphPerc) / 100;
            break;
        case 'DN Amount PPH':
            pph = (dnAmount * pphPerc) / 100;
            break;
    }
    $("#pphAmount").val(formatMoney(pph,0));

    let wht = $('#whtType').val();
    let total;
    if (wht === "WHT No Deduct") {
        total = dpp + ppn;
    } else {
        total = dpp + ppn - pph;
    }
    $("#total").val(formatMoney(total,0));

    if(elPpnAmount.val() !== "0" || elPpnAmount.val() !== '') {
        validator.revalidateField('fpNumber');
        validator.revalidateField('fpDate');
    }
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

                    refId = value['refId'];

                    if(method === 'update'){
                        $('#txt_info_method').text('Edit ' + value['refId'] + ' | ' + value['lastStatus']);
                    } else {
                        $('#txt_info_method').text('Display ' + value.refId + ' | ' + value['lastStatus']);
                    }
                    $('#period').val(value['periode']);
                    $('#accountDesc').val(value['accountDesc']);
                    if (value['sellpoint']) {
                        if (value['sellpoint'].length > 0) {
                            value['sellpoint'].forEach(function(el) {
                                if (el.flag) {
                                    return $('#sellingPointId').val(el['sellpoint']).trigger('change');
                                }
                            });
                        }
                    }
                    $('#subAccountId').val(value['accountId']).trigger('change.select2');

                    $('#entityId').val(value['entityId']).trigger('change.select2');
                    $("#entityId").attr("readonly", "readonly");
                    dnRefId = value['refId'];
                    entityId = value['entityId'];
                    promoId = value['promoId'];
                    lastStatus = value['lastStatus'];
                    ((value['lastStatus'] === 'cancelled_by_dist') ? isCancel = true : isCancel= false);
                    $('#promoRefId').val(value['promoRefId']);
                    $('#entityLongDesc').val(value['entityLongDesc']);
                    $('#entityAddress').val(value['entityAddress']);
                    $('#entityUp').val(value['entityUp']);
                    $('#activityDesc').val(value['activityDesc']);

                    $('#feeDesc').val(value['feeDesc']);
                    $('#dnCreator').val(value['dnCreator']);

                    dnWhtType = value['whtType'];
                    taxLevelId = value['taxLevel'];

                    if (dnWhtType === "WHT No Deduct") {
                        $('#icon_info_wht').removeClass('d-none');
                    } else {
                        $('#icon_info_wht').addClass('d-none');
                    }

                    $('#deductionDate').val(formatDate(value['deductionDate']));
                    $('#memDocNo').val(value['memDocNo']);
                    $('#intDocNo').val(value['intDocNo']);
                    $('#periode').val((value['startPromo']) ? value['startPromo'] + " to " + value['endPromo'] : '');
                    $('#dnAmount').val(formatMoney(value['dnAmount'], 0));
                    $('#feePct').val(formatMoney(value['feePct'], 0));
                    $('#feeAmount').val(formatMoney(value['feeAmount'], 0));
                    $('#dpp').val(formatMoney(value['dpp'], 0));
                    $('#statusPPN').val(value['statusPPN']).trigger('change.select2');
                    $('#ppnPct').val(formatMoney(value['ppnPct'], 0));
                    $('#ppnAmount').val(formatMoney(value['ppnAmt'], 0));
                    $('#statusPPH').val(value['statusPPH']).trigger('change.select2');
                    $('#pphPct').val(formatMoney(value['pphPct'], 0));
                    $('#pphAmount').val(formatMoney(value['pphAmt'], 0));

                    let total;
                    if (value['whtType'] === "WHT No Deduct") {
                        total = parseFloat(value['dpp']) + parseFloat(value['ppnAmt']);
                    } else {
                        total = parseFloat(value['dpp']) + parseFloat(value['ppnAmt']) - parseFloat(value['pphAmt']);
                    }
                    $('#total').val(formatMoney(total.toString(), 0));

                    let elIsDNPromo = $('#isDNPromo');
                    if(value.periode > 2023) {
                        isDNPromo = 1;
                        elIsDNPromo.prop("checked", true);
                        elIsDNPromo.attr("disabled", true);
                        validator.addField('subAccountId', {
                            validators: {
                                notEmpty: {
                                    message: "Enter a Sub Account"
                                },
                            }
                        });
                        $('#subAccountID-iconRequired').addClass('required');
                        $('#btn_search_promo').removeClass('d-none');
                    } else {
                        if(value['isDNPromo'] === true || value['isDNPromo'] === "true"){
                            isDNPromo = 1;
                            elIsDNPromo.prop("checked", true)
                            $('#btn_search_promo').removeClass('d-none');
                            elIsDNPromo.attr("disabled", false)
                            validator.addField('subAccountId', {
                                validators: {
                                    notEmpty: {
                                        message: "Enter a Sub Account"
                                    },
                                }
                            });
                            $('#subAccountID-iconRequired').addClass('required');
                        } else {
                            isDNPromo = 0;
                            elIsDNPromo.prop("checked", false)
                            elIsDNPromo.attr("disabled", false)
                            validator.removeField('subAccountId');
                            $('#subAccountID-iconRequired').removeClass('required');
                        }
                    }

                    $('#fpNumber').val(value['fpNumber']);
                    $('#fpDate').val((value['fpNumber']) ? formatDate(value['fpDate']) : "");

                    if (value['dnattachment']) {
                        value['dnattachment'].forEach((item) => {
                            if (item['docLink'] === 'row' + parseInt(item['docLink'].replace('row', ''))) {
                                let elReviewFileLabel = $('#review_file_label_' + parseInt(item['docLink'].replace('row', '')));
                                elReviewFileLabel.text(item['fileName']).attr('title', item['fileName']);
                                elReviewFileLabel.addClass('form-control-solid-bg');
                                $('#attachment' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', true);
                            }
                        });
                    }
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: errorThrown,
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

const getDataWHTTypePromo = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/creation-ho/get-data/wht-type/promo-id",
            type: "GET",
            data: {promoId: promoId},
            dataType: 'json',
            async: true,
            success: async function (result) {
                if (!result.error) {
                    let value = result.data;

                    whtType = value['whtType'];
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: errorThrown,
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

const getListSubAccount = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/creation-ho/list/subaccount",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j]['id'],
                        text: result.data[j]['longDesc']
                    });
                }
                $('#subAccountId').select2({
                    placeholder: "Select a Sub Account",
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

const getListSellingPoint = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/creation-ho/list/sellingpoint",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j]['refId'],
                        text: result.data[j]['longDesc']
                    });
                }
                $('#sellingPointId').select2({
                    placeholder: "Select a Selling Point",
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

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/creation-ho/list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j]['id'],
                        text: result.data[j]['longDesc'],
                        longDesc: result.data[j]['longDesc'],
                        entityUp: result.data[j]['entityUp'],
                        entityAddress: result.data[j]['entityAddress'],
                    });
                }
                listEntity = data;
                $('#entityId').select2({
                    placeholder: "Select an Entity",
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

const getDataTaxLevel = (entityId, whtType) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/creation-ho/get-data/tax-level/entity-id",
            type        : "GET",
            data        : {entityId: entityId, whtType: whtType},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (result.data.length > 0) arrTaxLevel = result.data;
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j]['taxLevel'],
                        text: result.data[j]['taxLevel'] + ' - ' +result.data[j]['description'],
                        ppn: (result.data[j]['ppnPct'] ?? 0),
                        pph: (result.data[j]['pphPct'] ?? 0),
                    });
                }
                $('#taxLevel').select2({
                    placeholder: "Select a Tax Level",
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
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListWHTType = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/creation-ho/list/wht-type",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j],
                        text: result.data[j]
                    });
                }
                $('#whtType').select2({
                    placeholder: "Select a WHT Type",
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

const selectTaxLevel = (materialNumber) => {
    arrTaxLevel.forEach(function(el) {
        if (el.materialNumber === materialNumber) {
            return $('#taxLevel').val(el.materialNumber + " - " + el.description);
        }
    });
}

const matchTaxLevel = (str) => {
    arrTaxLevel.forEach(function(el) {
        if (el.materialNumber + " - " + el.description === str) {
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
        case '' : return '';
    }
}

const selectPPH = (str) => {
    switch (str) {
        case 'DN Amount PPH': return 'PPH - DN Amount';
        case 'FEE PPH' : return 'PPH - Fee';
        case 'DPP PPH' : return 'PPH - DPP';
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

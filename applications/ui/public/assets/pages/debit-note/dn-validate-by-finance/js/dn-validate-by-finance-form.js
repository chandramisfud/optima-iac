'use strict';

let swalTitle = "Validation by Finance";
let id, validator, promoId=0, isDNPromo=true, entityId, method, statusValidation = [], arrTaxLevel = [];
let dt_mechanism, refId, categoryShortDesc;
let whtTypeBefore, whtTypePromo, allowValidate = false;
let taxLevelId;

let targetAttachment = document.querySelector(".card_attachment");
let blockUIAttachment = new KTBlockUI(targetAttachment, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

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
    Promise.all([getListWHTType(), getListSubAccount()]).then(async () => {
        await getData(id).then(function () {
            if (promoId !== 0 || promoId !== "0") {
                getDataPromo(promoId)
            }
        });
        $('#detailPromoDC').addClass('d-none');
        $('#detailPromoRC').addClass('d-none');
        blockUI.release();
    });

    if (method === 'reject') {
        statusValidation.push({
            id: 'rejected_by_finance',
            text: 'Rejected'
        });
    } else {
        statusValidation.push({
            id: 'validate_by_finance',
            text: 'Validated'
        });
        statusValidation.push({
            id: 'rejected_by_finance',
            text: 'Rejected'
        });
        $('#is-dn-promo').removeClass('d-none')
    }

    const v_taxLevel = function () {
        return {
            validate: function () {
                if (method === 'approve') {
                    let elTaxLevel = $('#taxLevel');
                    if(elTaxLevel.val() === null || elTaxLevel.val() === "") {
                        return {
                            valid: false,
                            message: 'This field is required'
                        }
                    } else {
                        return {
                            valid: true
                        }
                    }
                }
            }
        }
    }

    FormValidation.validators.v_notes = function () {
        return {
            validate: function () {
                if ($('#approvalStatusCode').val() === 'rejected_by_finance') {
                    let elNotes = $('#notes');
                    if (elNotes.val() === null || elNotes.val() === "") {
                        return {
                            valid: false,
                            message: 'This field is required'
                        }
                    } else {
                        return {
                            valid: true
                        }
                    }
                }
            }
        }
    };
    FormValidation.validators.v_taxLevel = v_taxLevel;
    validator = FormValidation.formValidation(document.getElementById('form_validate_by_finance'), {
        fields: {
            approvalStatusCode: {
                validators: {
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
            notes: {
                validators: {
                    v_notes: {

                    }
                }
            },
            subAccountId: {
                validators: {
                    notEmpty: {
                        message: "Enter a Sub Account"
                    },
                }
            },
            taxLevel: {
                validators: {
                    v_taxLevel: {

                    }
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
        });



    $('#approvalStatusCode').select2({
        placeholder: "Select validation status",
        minimumResultsForSearch: -1,
        allowClear: true,
        data: statusValidation
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
        scrollY: "20vh",
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
});

$('#isDNPromo').on('change', function () {
    if(this.checked) {
        isDNPromo = true;
        $('#isDNPromo').val("1");

        $('#subAccountId').val('').trigger('change');
        $('#promoRefId').val('');
        $('#entityId').val('');
        validator.addField('subAccountId', {
            validators: {
                notEmpty: {
                    message: "Enter a Sub Account"
                },
            }
        });
        $('#subAccountID-iconRequired').addClass('required');
    } else {
        isDNPromo = false;
        $('#isDNPromo').val("0");

        $('#subAccountId').val('').trigger('change');
        $('#promoRefId').val('');
        $('#entityId').val('');

        validator.removeField('subAccountId');
        $('#subAccountID-iconRequired').removeClass('required');
        promoId = 0;
    }
    $('#btn_search_promo').toggleClass('d-none');
});

$('#btn_search_promo').on('click',  function () {
    let filter_period = ($('#year').val()) ?? "";

    $("#filter_entity").val(entityId).trigger('change').attr("readonly", "readonly");
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let account_id = ($('#subAccountId').val()) ?? "";

    if(account_id === "" || account_id==null) { account_id = "0"; }

    dt_promo_list.clear().draw()
    let url = "/dn/creation-ho/get-data/promo?period=" +  filter_period + "&subAccountId=" + account_id + "&entityId=" + filter_entity + "&channelId=0";
    dt_promo_list.ajax.url(url).load();

    $('#filter_channel').val('').trigger('change');
    $('#modal_list_promo').modal('show');
});

$('#promoRefId').on('change', async function () {
    if ($(this).val() === "") {
        $('#whtType').val('').trigger('change');
    } else {
        await getDataWHTTypePromo();
        $('#whtType').val(whtTypePromo).trigger('change');
    }
});

$('#whtType').on('change', async function () {
    let elTaxLevel = $('#taxLevel');

    if ($(this).val() === "WHT No Deduct") {
        $('#icon_info_wht').removeClass('d-none');
    } else {
        $('#icon_info_wht').addClass('d-none');
    }

    elTaxLevel.empty();
    await getDataTaxLevel(entityId, $(this).val());
    elTaxLevel.val(taxLevelId).trigger('change.select2');
    if (elTaxLevel.val() !== taxLevelId) {
        elTaxLevel.val('').trigger('change');

        taxLevelId = '';
    }

    if($(this).val() === "Non WHT Object") {
        $("#statusPPH").val('').trigger('change');
        $("#pphPct").val(0);
    }

    if (whtTypeBefore === "WHT No Deduct" && $(this).val() === "Non WHT Object") {
        $('#statusPPH').attr('readonly', true);
        $('#pphPct').attr('readonly', true);
        allowValidate = true;
    } else if (whtTypeBefore === "Non WHT Object" && $(this).val() === "WHT No Deduct") {
        $('#statusPPH').attr('readonly', false);
        $('#pphPct').attr('readonly', false);
        allowValidate = true;
    } else if (whtTypeBefore === "Non WHT Object" && $(this).val() === "Non WHT Object") {
        $('#statusPPH').attr('readonly', true);
        $('#pphPct').attr('readonly', true);
        allowValidate = true;
    } else if (whtTypeBefore === "" && $(this).val() === "WHT Deduct") {
        $('#statusPPH').attr('readonly', true);
        $('#pphPct').attr('readonly', true);
        allowValidate = false;
    } else if (whtTypeBefore === "" && $(this).val() === "WHT No Deduct") {
        $('#statusPPH').attr('readonly', false);
        $('#pphPct').attr('readonly', false);
        allowValidate = true;
    } else if (whtTypeBefore === "" && $(this).val() === "Non WHT Object") {
        $('#statusPPH').attr('readonly', true);
        $('#pphPct').attr('readonly', true);
        allowValidate = true;
    } else if (whtTypeBefore === "WHT Deduct" && $(this).val() === "WHT Deduct" && $('#pphAmt').val() !== '0') {
        allowValidate = true;
    } else if (whtTypeBefore === "WHT No Deduct" && $(this).val() === "WHT No Deduct") {
        allowValidate = true;
    } else if (whtTypeBefore === "WHT Deduct" && $(this).val() === "WHT Deduct" && $('#pphAmt').val() === '0') {
        allowValidate = true;
    } else {
        allowValidate = false;
        $('#statusPPH').attr('readonly', true);
        $('#pphPct').attr('readonly', true);
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
        calculation();
    } else {
        $('#ppnPct').val(0);
        $('#pphPct').val(0);
        calculation();
    }
});

$('#info_wht_type').on('mouseenter', function () {
    $('.tooltip_custom .tooltip_text').css('visibility', 'visible').css('opacity', 1);
}).on('mouseleave', function() {
    $('.tooltip_custom .tooltip_text').css('visibility', 'hidden').css('opacity', 0);
});

$('#statusPPH').on('change', function () {
    calculation();
});

$('#pphPct').on('keyup', function () {
    if($('#statusPPH').val() !== '') {
        calculation();
    }
});

$('#btn_submit').on('click', function () {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            if (allowValidate === false && $('#approvalStatusCode').val() === "validate_by_finance") {
                return Swal.fire({
                    title: "Can't Validate DN",
                    text: 'Please reject to distributor because there is a change in WHT Type from ' + (whtTypeBefore === "" ? "---" : whtTypeBefore) + ' to ' + $('#whtType').val(),
                    icon: "warning",
                    confirmButtonText: "Confirm",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
            let e = document.querySelector("#btn_submit");
            let formData = new FormData($('#form_validate_by_finance')[0]);
            formData.append('dnid', id);
            formData.append('isDNPromo', isDNPromo);
            formData.append('promoId', promoId);

            let url = '/dn/validate-by-finance/submit';
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
                    if (!result.error) {
                        Swal.fire({
                            text: result.message,
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.location.href = '/dn/validate-by-finance';
                        });
                    } else {
                        Swal.fire({
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "Confirm",
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
        } else {
            Swal.fire({
                title: "Data not valid",
                icon: "warning",
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");

    let originalInvoice = $('#Original_Invoice_from_retailers').is(':checked');
    let taxInvoice = $('#Tax_Invoice').is(':checked');
    let promotionAgreement = $('#Promotion_Agreement_Letter').is(':checked');
    let tradingTerm = $('#Trading_Term').is(':checked');
    let salesData = $('#Sales_Data').is(':checked');
    let copyMailer = $('#Copy_of_mailer').is(':checked');
    let copyPhoto = $('#Copy_of_photo_doc').is(':checked');
    let listTransfer = $('#List_of_Transfer').is(':checked');

    let formData = new FormData();
    formData.append('dnid', id);
    formData.append('original_Invoice_from_retailers', originalInvoice);
    formData.append('tax_Invoice', taxInvoice);
    formData.append('promotion_Agreement_Letter', promotionAgreement);
    formData.append('trading_Term', tradingTerm);
    formData.append('sales_Data', salesData);
    formData.append('copy_of_Mailer', copyMailer);
    formData.append('copy_of_Photo_Doc', copyPhoto);
    formData.append('list_of_Transfer', listTransfer);

    let url = '/dn/validate-by-finance/save';
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
            if (!result.error) {
                Swal.fire({
                    text: result.message,
                    icon: "success",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            } else {
                Swal.fire({
                    text: result.message,
                    icon: "warning",
                    confirmButtonText: "Confirm",
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

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#attachment' + row);
        let url = file_host + "/assets/media/debitnote/" + id + "/row" + row + "/" + attachment.val();
        if (attachment.val() !== "") {
        fetch(url)
            .then((resp) => {
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

$('.btn_download_promo').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#attachmentPromo' + row);
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
                            a.download = $('#attachmentPromo' + row).val();
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

$('.btn_view').on('click', function() {
    let row = $(this).val();
    let attachment = $('#attachment' + row).val();
    window.location.href = '/dn/validate-by-finance/view?id=' + id + '&row=' + row + '&fileName=' + encodeURIComponent(attachment);
});

$('#download_all').on('click', function () {
    let url = "/dn/validate-by-finance/download-zip?id=" + id;
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
                        text: "File " + refId + ".zip does not exist",
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
                    window.location = url;
                }
            } else {
                window.location = url;
            }
        }
    }
    xmlhttp.send();
});

$('.btn_download_dc').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#review_file_label_' + row);
        let url = file_host + "/assets/media/promo/" + promoId + "/row" + row + "/" + attachment.text();
        if (attachment.text() !== "") {
            fetch(url)
            .then((resp) => {
                if (resp.ok) {
                    resp.blob().then(blob => {
                        const url_blob = window.URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.style.display = 'none';
                        a.href = url_blob;
                        a.download = attachment.text();
                        document.body.appendChild(a);
                        a.click();
                        window.URL.revokeObjectURL(url_blob);
                    })
                    .catch(e => {
                        console.log(e);
                        Swal.fire({
                            title: "Download Attachment",
                            text: "Download attachment failed",
                            icon: "warning",
                            buttonsStyling: !1,
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
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
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        }
    }
});

const getData = (id) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/validate-by-finance/get-data/id",
            type: "GET",
            data: {id: id},
            dataType: 'json',
            async: true,
            success: async function (result) {
                if (!result.error) {
                    let value = result.data;
                    promoId = value['promoId'];

                    refId = value['refId'];
                    categoryShortDesc = value['categoryShortDesc'];
                    entityId = value['entityId'];
                    taxLevelId = value['taxLevel'];

                    if(method === 'approve') {
                        $('#txt_info_method').text('DN Approval ' + value['refId'] + ' | ' + value['lastStatus']);
                    } else {
                        $('#txt_info_method').text('DN Reject ' + value['refId'] + ' | ' + value['lastStatus']);
                    }
                    $('#year').val(value['periode']);
                    $('#subAccountId').val(value['accountId']).trigger('change');
                    if (value['sellpoint']) {
                        if (value['sellpoint'].length > 0) {
                            value['sellpoint'].forEach(function(el) {
                                if (el['flag']) {
                                    return $('#sellingPoint').val(el['longDesc']);
                                }
                            });
                        }
                    }

                    $("#filter_entity").attr("readonly", "readonly");
                    $('#btn-save-document-completeness').removeClass('d-none');

                    whtTypeBefore = value['whtType'];
                    $('#whtType').val(value['whtType']).trigger('change.select2');

                    await getDataTaxLevel(value['entityId'], value['whtType']);
                    $('#taxLevel').val(value['taxLevel']).trigger('change');

                    $('#promoRefId').val(value['promoRefId']);
                    $('#entityLongDesc').val(value['entityLongDesc']);
                    $('#entityAddress').val(value['entityAddress']);
                    $('#entityUp').val(value['entityUp']);
                    $('#activityDesc').val(value['activityDesc']);
                    $('#feeDesc').val(value['feeDesc']);
                    $('#dnCreator').val(value['dnCreator']);
                    $('#deductionDate').val(formatDate(value['deductionDate']));
                    $('#memDocNo').val(value['memDocNo']);
                    $('#intDocNo').val(value['intDocNo']);
                    $('#periode').val((value['startPromo']) ? value['startPromo'] + " to " + value['endPromo'] : '');
                    $('#dnAmount').val(formatMoney(value['dnAmount'], 0));
                    $('#feePct').val(formatMoney(value['feePct'], 0));
                    $('#feeAmount').val(formatMoney(value['feeAmount'], 0));
                    $('#dpp').val(formatMoney(value['dpp'], 0));
                    $('#statusPPN').val(selectPPN(value['statusPPN']));
                    $('#ppnPct').val(formatMoney(value['ppnPct'], 0));
                    $('#ppnAmt').val(formatMoney(value['ppnAmt'], 0));
                    $('#statusPPH').val(value['statusPPH']).trigger('change');
                    $('#pphPct').val(formatMoney(value['pphPct'], 0));
                    $('#pphAmt').val(formatMoney(value['pphAmt'], 0));

                    let total;
                    if (value['whtType'] === "WHT No Deduct") {
                        total = parseFloat(value['dpp']) + parseFloat(value['ppnAmt']);
                    } else {
                        total = parseFloat(value['dpp']) + parseFloat(value['ppnAmt']) - parseFloat(value['pphAmt']);
                    }
                    $('#total').val(formatMoney(total.toString(), 0));

                    if(value.periode > 2023) {
                        isDNPromo = true;
                        let elIsDNPromo = $('#isDNPromo');
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
                        if(value.isDNPromo === true){
                            isDNPromo = true;
                            let elIsDNPromo = $('#isDNPromo');
                            elIsDNPromo.prop("checked", true);
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
                            let elIsDNPromo = $('#isDNPromo');
                            isDNPromo = false;
                            elIsDNPromo.prop("checked", false)
                            elIsDNPromo.attr("disabled", false)
                            validator.removeField('subAccountId');
                            $('#subAccountID-iconRequired').removeClass('required');
                        }
                    }

                    $('#fpNumber').val(value.fpNumber);
                    $('#fpDate').val((value.fpNumber) ? formatDate(value.fpDate) : "");

                    if (value['dnDocCompletenessHeader']) {
                        value['dnDocCompletenessHeader'].forEach((item) => {
                            $('#Original_Invoice_from_retailers').prop("checked", item['original_Invoice_from_retailers']);
                            $('#Tax_Invoice').prop("checked", item['tax_Invoice']);
                            $('#Promotion_Agreement_Letter').prop("checked", item['promotion_Agreement_Letter']);
                            $('#Trading_Term').prop("checked", item['trading_Term']);
                            $('#Sales_Data').prop("checked", item['sales_Data']);
                            $('#Copy_of_mailer').prop("checked", item['copy_of_Mailer']);
                            $('#Copy_of_photo_doc').prop("checked", item['copy_of_Photo_Doc']);
                            $('#List_of_Transfer').prop("checked", item['list_of_Transfer']);
                        });
                    }

                    if(method === 'reject'){
                        $('#Original_Invoice_from_retailers').attr("disabled", true);
                        $('#Tax_Invoice').attr("disabled", true);
                        $('#Promotion_Agreement_Letter').attr("disabled", true);
                        $('#Trading_Term').attr("disabled", true);
                        $('#Sales_Data').attr("disabled", true);
                        $('#Copy_of_mailer').attr("disabled", true);
                        $('#Copy_of_photo_doc').attr("disabled", true);
                        $('#List_of_Transfer').attr("disabled", true);
                    }

                    if (value['dnattachment']) {
                        value['dnattachment'].forEach((item) => {
                            if (item['docLink'] === 'row' + parseInt(item['docLink'].replace('row', ''))) $('#attachment' + parseInt(item['docLink'].replace('row', ''))).val(item['fileName']);
                        });
                    }

                    if (value['whtType'] === "WHT No Deduct") {
                        $('#icon_info_wht').removeClass('d-none');
                    } else {
                        $('#icon_info_wht').addClass('d-none');
                    }

                    if (value['whtType'] === "WHT Deduct" && $('#pphAmt').val() !== "0") {
                        allowValidate = true;
                    } else if (value['whtType'] === "WHT No Deduct") {
                        allowValidate = true;
                    } else if (value['whtType'] === "Non WHT Object") {
                        allowValidate = true;
                    } else {
                        allowValidate = false;
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
                        text: jqXHR['responseJSON'].message,
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

const getDataPromo = (id) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/validate-by-finance/get-data/promo/id",
            type: "GET",
            data: {id: id},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {

                    if (categoryShortDesc === 'DC'){
                        $('#detailPromoDC').removeClass('d-none');

                        let values = result.data;
                        let promoHeader = values['promoHeader'];

                        if (promoHeader) {
                            $('#period').val(new Date(promoHeader['startPromo']).getFullYear());
                            $('#entityDesc').val(promoHeader['principalName']);
                            $('#groupBrandDescDC').val(promoHeader['groupBrandDesc']);
                            $('#distributorNameDC').val(promoHeader['distributorName']);
                            $('#subCategoryDescDC').val(promoHeader['subCategoryDesc']);
                            $('#subActivityLongDescDC').val(promoHeader['subActivityLongDesc']);
                            $('#channelDescDC').val(promoHeader['channelDesc']);
                            $('#allocationRefIdDC').val(promoHeader['allocationRefId']);
                            $('#startPromoDC').val(formatDateOptima(promoHeader['startPromo']));
                            $('#endPromoDC').val(formatDateOptima(promoHeader['endPromo']));

                            if (values['mechanisms'].length > 0) {
                                for (let i = 0; i < values['mechanisms'].length; i++) {
                                    $('#mechanismDC').val(values['mechanisms'][i].mechanism);
                                }
                            }

                            $('#investmentDC').val(formatMoney(promoHeader['investment'], 0));
                            $('#initiatorNotesDC').val(promoHeader['initiator_notes']);

                            $('#allocationDescDC').val(promoHeader['allocationDesc']);
                            $('#budgetDeployedDC').val(formatMoney(promoHeader['budgetAmount'], 0));
                            $('#budgetRemainingDC').val(formatMoney(promoHeader['remainingBudget'], 0));
                        }
                        if (values['attachments']) {
                            let fileSource = "";
                            values['attachments'].forEach((item) => {
                                if (item['docLink'] === 'row' + parseInt(item['docLink'].replace('row', ''))) {
                                    let elLabel = $('#review_file_label_' + parseInt(item['docLink'].replace('row', '')));
                                    elLabel.text(item['fileName']).attr('title', item['fileName']);
                                    $('#btn_download_dc' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                                    fileSource = file_host + "/assets/media/promo/" + promoId + "/" + item['docLink'] + "/" + item.fileName;
                                    const fileInput = document.querySelector('#attachment' + parseInt(item['docLink'].replace('row', '')));
                                    fileInput.dataset.file = fileSource;
                                }
                            });
                        }
                    } else {
                        $('#detailPromoRC').removeClass('d-none');
                        let value = result.data;

                        if (value['promoHeader']) {
                            let year = new Date(value['promoHeader']['startPromo']);
                            $('#yearPromo').val(year.getFullYear());
                            $('#promoPlanRefId').val(value['promoHeader']['promoPlanRefId']);
                            $('#allocationRefId').val(value['promoHeader']['allocationRefId']);
                            $('#allocationDesc').val(value['promoHeader']['allocationDesc']);
                            $('#activityLongDesc').val(value['promoHeader']['activityLongDesc']);
                            $('#subActivityLongDesc').val(value['promoHeader']['subActivityLongDesc']);
                            $('#startPromo').val(formatDateOptima(value['promoHeader']['startPromo']));
                            $('#endPromo').val(formatDateOptima(value['promoHeader']['endPromo']));

                            $('#tsCoding').val(value['promoHeader']['tsCoding']);
                            $('#subCategoryDesc').val(value['promoHeader']['subCategoryDesc']);
                            $('#principalName').val(value['promoHeader']['principalName']);
                            $('#distributorname').val(value['promoHeader']['distributorName']);
                            $('#budgetAmount').val(formatMoney(value['promoHeader']['budgetAmount'], 0));
                            $('#remainingBudget').val(formatMoney(value['promoHeader']['remainingBudget'], 0));
                            $('#initiator').val(value['promoHeader']['initiator']);
                            $('#initiator_notes').val(value['promoHeader']['initiator_notes']);
                            $('#investmentTypeDesc').val(value['promoHeader']['investmentTypeDesc']);

                            $('#normalSales').val(formatMoney(value['promoHeader']['normalSales'],0));
                            $('#incrSales').val(formatMoney(value['promoHeader']['incrSales'],0));
                            $('#investment').val(formatMoney(value['promoHeader']['investment'],0));
                            $('#investmentBfrClose').val(formatMoney(value['promoHeader']['investmentBfrClose'],0));
                            $('#investmentClosedBalance').val(formatMoney(value['promoHeader']['investmentClosedBalance'],0));

                            $('#totSales').val(formatMoney((value['promoHeader']['normalSales']) + (value['promoHeader']['incrSales']),0));
                            $('#totInvestment').val(formatMoney(value['promoHeader']['investment'],0));
                            $('#roi').val(formatMoney(value['promoHeader']['roi'],0));
                            $('#costRatio').val(formatMoney(value['promoHeader']['costRatio'],0));
                        }

                        if (value['channels']) {
                            if (value['channels'].length > 0) {
                                value['channels'].forEach(function(el) {
                                    if (el['flag']) {
                                        $('#channelDesc').val(el['longDesc']);
                                    }
                                });
                            }
                        }

                        if (value['subChannels']) {
                            if (value['subChannels'].length > 0) {
                                value['subChannels'].forEach(function(el) {
                                    if (el.flag) {
                                        $('#subchannelDesc').val(el.longDesc);
                                    }
                                });
                            }
                        }

                        if (value['accounts']) {
                            if (value['accounts'].length > 0) {
                                value['accounts'].forEach(function(el) {
                                    if (el['flag']) {
                                        $('#accountDesc').val(el['longDesc']);
                                    }
                                });
                            }
                        }

                        if (value['subAccounts']) {
                            if (value['subAccounts'].length > 0) {
                                value['subAccounts'].forEach(function(el) {
                                    if (el['flag']) {
                                        $('#subaccountDesc').val(el['longDesc']);
                                    }
                                });
                            }
                        }

                        let longDescRegion='';
                        if (value['regions']) {
                            if (value['regions'].length > 0) {
                                value['regions'].forEach(function(el) {
                                    if (el['flag']) {
                                        longDescRegion += '<span class="fw-bold text-sm-left">' + el['longDesc'] + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                    }
                                });
                                $('#card_list_region').html(longDescRegion);
                            }
                        }

                        let longDescBrand='';
                        if (value['brands']) {
                            if (value['brands'].length > 0) {
                                value['brands'].forEach(function(el) {
                                    if (el['flag']) {
                                        longDescBrand += '<span class="fw-bold text-sm-left">' + el['longDesc'] + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                    }
                                });
                                $('#card_list_brand').html(longDescBrand);
                            }
                        }

                        let longDescSku='';
                        if (value['skus']) {
                            if (value['skus'].length > 0) {
                                value['skus'].forEach(function(el) {
                                    if (el['flag']) {
                                        longDescSku += '<span class="fw-bold text-sm-left">' + el['longDesc'] + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                    }
                                });
                                $('#card_list_sku').html(longDescSku);
                            }
                        }

                        if(value['mechanism']){
                            dt_mechanism.rows.add(value['mechanism']).draw();
                        }

                        if (value['attachments']) {
                            value['attachments'].forEach((item) => {
                                if (item['docLink'] === 'row' + parseInt(item['docLink'].replace('row', ''))) $('#attachmentPromo' + parseInt(item['docLink'].replace('row', ''))).val(item['fileName']);
                            });
                        }
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
                        text: jqXHR['responseJSON'].message,
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
            url         : "/dn/received-by-danone/list/tax-level",
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
            url         : "/dn/validate-by-finance/list/wht-type",
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

const getListSubAccount = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/creation/list/subaccount",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
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

const getDataTaxLevel = (entityId, whtType) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/creation/get-data/tax-level/entity-id",
            type        : "GET",
            data        : {entityId: entityId, whtType: whtType},
            dataType    : 'json',
            async       : true,
            success: function(result) {
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

const getDataWHTTypePromo = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/validate-by-finance/get-data/wht-type/promo-id",
            type: "GET",
            data: {promoId: promoId},
            dataType: 'json',
            async: true,
            success: async function (result) {
                if (!result.error) {
                    let value = result.data;

                    whtTypePromo = value['whtType'];
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

const selectTaxLevel = (materialNumber) => {
    arrTaxLevel.forEach(function(el) {
        if (el.materialNumber === materialNumber) {
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

const calculation = () => {
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
    let ppn = 0;
    let ppnPerc = $('#ppnPct').val();
    switch (statusPPN) {
        case 'PPN - DN Amount':
            ppn = dnAmount * (ppnPerc / 100);
            break;
        case 'PPN - Fee':
            ppn = feeAmount * (ppnPerc / 100);
            break;
        case 'PPN - DPP':
            ppn = dpp * (ppnPerc / 100);
            break;
    }
    $("#ppnAmt").val(formatMoney(ppn,0));

    // Calculate PPH
    let statusPPH = $('#statusPPH').val();
    let pphPerc = $('#pphPct').val();
    let pph = 0;
    switch (statusPPH) {
        case 'FEE PPH':
            pph = feeAmount * (pphPerc / 100);
            break;
        case 'DPP PPH':
            pph = dpp * (pphPerc / 100);
            break;
        case 'DN Amount PPH':
            pph = dnAmount * (pphPerc / 100);
            break;
    }
    $("#pphAmt").val(formatMoney(pph,0));

    let wht = $('#whtType').val();
    let total;
    if (wht === "WHT No Deduct") {
        total = dpp + ppn;
    } else {
        total = dpp + ppn - pph;
    }
    $("#total").val(formatMoney(total,0));
}

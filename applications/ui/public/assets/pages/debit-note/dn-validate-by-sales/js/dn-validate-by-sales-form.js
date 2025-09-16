'use strict';

let swalTitle = "Debit Note [Validate By Sales]";
var id, validator, promoId=0, isDNPromo=1, method, statusValidation = [], arrTaxLevel = [];
var dt_mechanism, refId, categoryShortDesc;

var target = document.querySelector(".card_form_attachments");
var blockUIAttachments = new KTBlockUI(target, {
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
    Promise.all([getListSubAccount()]).then(async () => {
        await getData(id).then(function () {

            statusValidation.push({
                id: 'validate_by_sales',
                text: 'Validated'
            });

            if(categoryShortDesc == 'DC'){
                statusValidation.push({
                    id: 'rejected_by_sales',
                    text: 'Rejected'
                });
            } else {
                statusValidation.push({
                    id: 'rejected_by_sales',
                    text: 'Remarks by Sales'
                });
            }

            if(method === 'reject'){
                statusValidation = [{
                    id: 'rejected_by_sales',
                    text: 'Rejected'
                }];
            }

            $('#approvalStatusCode').select2({
                placeholder: "Select validation status",
                minimumResultsForSearch: -1,
                allowClear: true,
                data: statusValidation
            });

            if (promoId !== 0 || promoId !== "0") {
                getDataPromo(promoId)
            }
        });
        $('#detailPromoDC').addClass('d-none');
        $('#detailPromoRC').addClass('d-none');
        blockUI.release();
    });

    const v_notes = function () {
        return {
            validate: function () {
                if ($('#approvalStatusCode').val() === 'rejected_by_sales') {
                    if($('#notes').val() === null || $('#notes').val() === "") {
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

    FormValidation.validators.v_notes = v_notes;

    validator = FormValidation.formValidation(document.getElementById('form_validate_by_sales'), {
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
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
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
        scrollY: "20vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                data: 'mechanismId',
                width: 5,
                orderable: false,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                title: 'Mechanism',
                width: 200,
                data: 'mechanism',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Notes',
                width: 200,
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

$('#approvalStatusCode').on('change', function() {
    if (this.value === 'rejected_by_sales'){
        validator.revalidateField('notes');
    }
});

$('#info_wht_type').on('mouseenter', function () {
    $('.tooltip_custom .tooltip_text').css('visibility', 'visible').css('opacity', 1);
}).on('mouseleave', function() {
    $('.tooltip_custom .tooltip_text').css('visibility', 'hidden').css('opacity', 0);
});

$('#notes').on('keyup', function() {
    if ($('#approvalStatusCode').val() === 'rejected_by_sales'){
        validator.revalidateField('notes');
    }
});

$('#btn_submit').on('click', function () {
    validator.validate().then(function (status) {
        if (status == "Valid") {
            let e = document.querySelector("#btn_submit");
            let formData = new FormData($('#form_validate_by_sales')[0]);
            formData.append('dnid', id);

            formData.delete('attachment7');
            formData.delete('attachment8');
            formData.delete('attachment9');

            let url = '/dn/validate-by-sales/submit';
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
                        let message = 'DN Validated';
                        if(method === 'reject') message = 'DN Rejected';
                        Swal.fire({
                            text: message,
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.location.href = '/dn/validate-by-sales';
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

$('#attachment7, #attachment8, #attachment9').on('change', function () {
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
                let formData = new FormData($('#form_validate_by_sales')[0]);
                formData.append('dnId', id);
                formData.append('mode', 'uploadattach');
                formData.append('row', 'row' + row);
                formData.append('fileName', data);
                formData.append('file', file);

                $.ajax({
                    url         : '/dn/validate-by-sales/delete-attachment',
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

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#review_file_label_' + row);
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

$('#download_all').on('click', function () {
    let url = "/dn/validate-by-sales/download-zip?id=" + id;
    let xmlhttp = new XMLHttpRequest(),  self = this;
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

const upload_file =  (el, row) => {
    let form_data = new FormData();
    let file = document.getElementById('attachment' + row).files[0];
    form_data.append('dnId', id);
    form_data.append('mode', 'uploadattach');
    form_data.append('file', file);
    form_data.append('row', 'row' + row);
    form_data.append('docLink', el.val());

    $.ajax({
        url: "/dn/creation/upload",
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
            $('#info' + row).removeClass('d-none').removeClass('badge-success').addClass('badge-danger').html('<i class="fa fa-times text-white"></i>');
        }
    });
}

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

$('.btn_preview').on('click', function () {
    let row = $(this).val();
    let attachment = $('#review_file_label_' + row);
    let file_name = attachment.val();
    if (parseInt(row) > 6) {
        file_name = attachment.text();
    }
    let title = $('#txt_info_method').text();
    window.location.href = "/dn/validate-by-sales/preview-attachment?i=" + id + "&r=" + row + "&fileName=" + encodeURIComponent(file_name) + "&t=" + title;
});

$('.btn_download_dc').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#review_file_label_dc_' + row);
        let id = ((!promoId || promoId === "0") ? uuid : promoId);
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
            url: "/dn/validate-by-sales/get-data/id",
            type: "GET",
            data: {id: id},
            dataType: 'json',
            async: true,
            success: async function (result) {
                if (!result.error) {
                    let value = result.data;
                    promoId = value.promoId;
                    refId = value.refId;
                    categoryShortDesc = value.categoryShortDesc;

                    $('#txt_info_method').text('DN Approval ' + value.refId + ' | ' + value.lastStatus);
                    $('#year').val(value.periode);
                    $('#accountDesc').val(value.accountDesc);
                    if (value.sellpoint) {
                        if (value.sellpoint.length > 0) {
                            value.sellpoint.forEach(function(el) {
                                if (el.flag) {
                                    return $('#sellingPoint').val(el.longDesc);
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
                    $('#taxLevel').val(value.taxLevel);
                    $('#whtType').val(value.whtType);

                    if (value['whtType'] === "WHT No Deduct") {
                        $('#icon_info_wht').removeClass('d-none');
                    } else {
                        $('#icon_info_wht').addClass('d-none');
                    }

                    $('#dnCreator').val(value.dnCreator);
                    $('#deductionDate').val(formatDate(value.deductionDate));
                    $('#memDocNo').val(value.memDocNo);
                    $('#intDocNo').val(value.intDocNo);
                    $('#periode').val((value.startPromo) ? value.startPromo + " to " + value.endPromo : '');
                    $('#dnAmount').val(formatMoney(value.dnAmount, 0));
                    $('#feePct').val(formatMoney(value.feePct, 0));
                    $('#feeAmount').val(formatMoney(value.feeAmount, 0));
                    $('#dpp').val(formatMoney(value.dpp, 0));
                    $('#statusPPN').val(selectPPN(value.statusPPN));
                    $('#ppnPct').val(formatMoney(value.ppnPct, 0));
                    $('#ppnAmt').val(formatMoney(value.ppnAmt, 0));
                    $('#statusPPH').val(selectPPH(value.statusPPH));
                    $('#pphPct').val(formatMoney(value.pphPct, 0));
                    $('#pphAmt').val(formatMoney(value.pphAmt, 0));
                    $('#total').val(formatMoney(value.totalClaim, 0));

                    if(value.isDNPromo == true){
                        isDNPromo = 1;
                        $('#isDNPromo').prop("checked", true)
                        $('#btn_search_promo').attr('disabled', false)
                    } else {
                        isDNPromo = 0;
                        $('#isDNPromo').prop("checked", false)
                        $('#btn_search_promo').attr('disabled', true)
                    }

                    $('#fpNumber').val(value.fpNumber);
                    $('#fpDate').val((value.fpNumber) ? formatDate(value.fpDate) : "");

                    if (value.channelId === 5) {
                        $('#list_transfer').removeClass("d-none");
                    }

                    if (value.dnDocCompletenessHeader) {
                        value.dnDocCompletenessHeader.forEach((item, index, arr) => {
                            $('#Original_Invoice_from_retailers').prop("checked", item.original_Invoice_from_retailers);
                            $('#Tax_Invoice').prop("checked", item.tax_Invoice);
                            $('#Promotion_Agreement_Letter').prop("checked", item.promotion_Agreement_Letter);
                            $('#Trading_Term').prop("checked", item.trading_Term);
                            $('#Sales_Data').prop("checked", item.sales_Data);
                            $('#Copy_of_mailer').prop("checked", item.copy_of_Mailer);
                            $('#Copy_of_photo_doc').prop("checked", item.copy_of_Photo_Doc);
                            $('#List_of_Transfer').prop("checked", item.list_of_Transfer);

                        });
                    }
                    $('#Original_Invoice_from_retailers').attr('disabled', true);
                    $('#Tax_Invoice').attr("disabled", true);
                    $('#Promotion_Agreement_Letter').attr("disabled", true);
                    $('#Trading_Term').attr("disabled", true);
                    $('#Sales_Data').attr("disabled", true);
                    $('#Copy_of_mailer').attr("disabled", true);
                    $('#Copy_of_photo_doc').attr("disabled", true);
                    $('#List_of_Transfer').attr("disabled", true);

                    if (value.dnattachment) {
                        value.dnattachment.forEach((item, index, arr) => {
                            if (item.docLink === ('row' + parseInt(item.docLink.replace('row', '')))) {
                                if (parseInt(item.docLink.replace('row', '')) > 6) {
                                    $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).text(item.fileName);
                                } else {
                                    $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).val(item.fileName).attr('title', item.fileName);
                                }
                                $('#btn_download_' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                                $('#btn_view_' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                            } else {
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

const getDataPromo = (promoId) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/validate-by-sales/get-data/promo/id",
            type: "GET",
            data: {id: promoId},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {

                    if (categoryShortDesc == 'DC'){
                        $('#detailPromoDC').removeClass('d-none');

                        let values = result.data;
                        let promoHeader = values.promoHeader;

                        if (promoHeader) {
                            $('#period').val(new Date(promoHeader.startPromo).getFullYear());
                            $('#entityDesc').val(promoHeader.principalName);
                            $('#groupBrandDescDC').val(promoHeader.groupBrandDesc);
                            $('#distributorNameDC').val(promoHeader.distributorName);
                            $('#subCategoryDescDC').val(promoHeader.subCategoryDesc);
                            $('#subActivityLongDescDC').val(promoHeader.subActivityLongDesc);
                            $('#channelDescDC').val(promoHeader.channelDesc);
                            $('#allocationRefIdDC').val(promoHeader.allocationRefId);
                            $('#startPromoDC').val(formatDateOptima(promoHeader.startPromo));
                            $('#endPromoDC').val(formatDateOptima(promoHeader.endPromo));

                            if (values.mechanisms.length > 0) {
                                for (let i = 0; i < values.mechanisms.length; i++) {
                                    $('#mechanismDC').val(values.mechanisms[i].mechanism);
                                }
                            }

                            $('#investmentDC').val(formatMoney(promoHeader.investment, 0));
                            $('#initiatorNotesDC').val(promoHeader.initiator_notes);

                            $('#allocationDescDC').val(promoHeader.allocationDesc);
                            $('#budgetDeployedDC').val(formatMoney(promoHeader.budgetAmount, 0));
                            $('#budgetRemainingDC').val(formatMoney(promoHeader.remainingBudget, 0));
                        }

                        if (values.attachments) {
                            let fileSource = "";
                            values.attachments.forEach((item, index, arr) => {
                                if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                    let elLabel = $('#review_file_label_dc_' + parseInt(item.docLink.replace('row', '')));
                                    elLabel.text(item.fileName).attr('title', item.fileName);
                                    elLabel.addClass('form-control-solid-bg');
                                    $('#attachment' + parseInt(item.docLink.replace('row', ''))).attr('disabled', true);
                                    $('#btn_download_dc' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                                    fileSource = file_host + "/assets/media/promo/" + promoId + "/" + item.docLink + "/" + item.fileName;
                                    const fileInput = document.querySelector('#attachment' + parseInt(item.docLink.replace('row', '')));
                                    fileInput.dataset.file = fileSource;
                                }
                            });
                        }
                    } else {
                        $('#detailPromoRC').removeClass('d-none');
                        let value = result.data;

                        if (value.promoHeader) {
                            let year = new Date(value.promoHeader.startPromo);
                            $('#yearPromo').val(year.getFullYear());
                            $('#promoPlanRefId').val(value.promoHeader.promoPlanRefId);
                            $('#allocationRefId').val(value.promoHeader.allocationRefId);
                            $('#allocationDesc').val(value.promoHeader.allocationDesc);
                            $('#activityLongDesc').val(value.promoHeader.activityLongDesc);
                            $('#subActivityLongDesc').val(value.promoHeader.subActivityLongDesc);
                            $('#startPromo').val(formatDateOptima(value.promoHeader.startPromo));
                            $('#endPromo').val(formatDateOptima(value.promoHeader.endPromo));

                            $('#tsCoding').val(value.promoHeader.tsCoding);
                            $('#subCategoryDesc').val(value.promoHeader.subCategoryDesc);
                            $('#principalName').val(value.promoHeader.principalName);
                            $('#distributorname').val(value.promoHeader.distributorName);
                            $('#budgetAmount').val(formatMoney(value.promoHeader.budgetAmount, 0));
                            $('#remainingBudget').val(formatMoney(value.promoHeader.remainingBudget, 0));
                            $('#initiator').val(value.promoHeader.initiator);
                            $('#initiator_notes').val(value.promoHeader.initiator_notes);
                            $('#investmentTypeDesc').val(value.promoHeader.investmentTypeDesc);

                            $('#normalSales').val(formatMoney(value.promoHeader.normalSales,0));
                            $('#incrSales').val(formatMoney(value.promoHeader.incrSales,0));
                            $('#investment').val(formatMoney(value.promoHeader.investment,0));
                            $('#investmentBfrClose').val(formatMoney(value.promoHeader.investmentBfrClose,0));
                            $('#investmentClosedBalance').val(formatMoney(value.promoHeader.investmentClosedBalance,0));

                            $('#totSales').val(formatMoney((value.promoHeader.normalSales) + (value.promoHeader.incrSales),0));
                            $('#totInvestment').val(formatMoney(value.promoHeader.investment,0));
                            $('#roi').val(formatMoney(value.promoHeader.roi,0));
                            $('#costRatio').val(formatMoney(value.promoHeader.costRatio,0));
                        }

                        if (value.channels) {
                            if (value.channels.length > 0) {
                                value.channels.forEach(function(el) {
                                    if (el.flag) {
                                        $('#channelDesc').val(el.longDesc);
                                    }
                                });
                            }
                        }

                        if (value.subChannels) {
                            if (value.subChannels.length > 0) {
                                value.subChannels.forEach(function(el) {
                                    if (el.flag) {
                                        $('#subchannelDesc').val(el.longDesc);
                                    }
                                });
                            }
                        }

                        if (value.accounts) {
                            if (value.accounts.length > 0) {
                                value.accounts.forEach(function(el) {
                                    if (el.flag) {
                                        $('#accountDesc').val(el.longDesc);
                                    }
                                });
                            }
                        }

                        if (value.subAccounts) {
                            if (value.subAccounts.length > 0) {
                                value.subAccounts.forEach(function(el) {
                                    if (el.flag) {
                                        $('#subaccountDesc').val(el.longDesc);
                                    }
                                });
                            }
                        }

                        let longDescRegion='';
                        if (value.regions) {
                            if (value.regions.length > 0) {
                                value.regions.forEach(function(el) {
                                    if (el.flag) {
                                        longDescRegion += '<span className="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                    }
                                });
                                $('#card_list_region').html(longDescRegion);
                            }
                        }

                        let longDescBrand='';
                        if (value.brands) {
                            if (value.brands.length > 0) {
                                value.brands.forEach(function(el) {
                                    if (el.flag) {
                                        longDescBrand += '<span className="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                    }
                                });
                                $('#card_list_brand').html(longDescBrand);
                            }
                        }

                        let longDescSku='';
                        if (value.skus) {
                            if (value.skus.length > 0) {
                                value.skus.forEach(function(el) {
                                    if (el.flag) {
                                        longDescSku += '<span className="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                    }
                                });
                                $('#card_list_sku').html(longDescSku);
                            }
                        }

                        dt_mechanism.rows.add(value.mechanism).draw();

                        if (value.attachments) {
                            value.attachments.forEach((item, index, arr) => {
                                if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) $('#attachmentPromo' + parseInt(item.docLink.replace('row', ''))).val(item.fileName);
                            });
                        }
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
                console.log(jqXHR.responseText);
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

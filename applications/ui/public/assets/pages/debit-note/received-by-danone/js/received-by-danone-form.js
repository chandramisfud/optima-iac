'use strict';

var swalTitle = "Debet Note [Reject]";
var id, validator, arrTaxLevel = [];

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
    id = url_str.searchParams.get("id");

    blockUI.block();
    Promise.all([getListTaxLevel()]).then(async () => {
        await getData(id);
        blockUI.release();
    });

    validator = FormValidation.formValidation(document.getElementById('form_reject'), {
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
                    notEmpty: {
                        message: "This field is required."
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
        });

});

$('#btn_reject').on('click', function () {
    validator.validate().then(function (status) {
        if (status == "Valid") {
            let e = document.querySelector("#btn_reject");
            let formData = new FormData($('#form_reject')[0]);
            formData.append('dnid', id);

            let url = '/dn/received-by-danone/reject';
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
                            text: result.message,
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(function () {
                            window.location.href = '/dn/received-by-danone';
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

$('#download_all').on('click', function () {
    let url = "/dn/creation/download-zip?id=" + id;
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

const getData = (id) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/received-by-danone/get-data/id",
            type: "GET",
            data: {id: id},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {
                    let value = result.data;

                    $('#txt_info_method').text('Display ' + value.refId + ' | ' + value.lastStatus);
                    $('#year').val(value.periode);
                    $('#notes').val(value.notes ?? '');
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
                    $('#dnCreator').val(value.dnCreator);
                    selectTaxLevel(value.taxLevel);
                    $('#whtType').val(value.whtType);
                    $('#deductionDate').val(formatDate(value.deductionDate));
                    $('#memDocNo').val(value.memDocNo);
                    $('#intDocNo').val(value.intDocNo);
                    $('#period').val((value.startPromo) ? value.startPromo + " to " + value.endPromo : '');
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

                    $('#fpNumber').val(value.fpNumber);
                    $('#fpDate').val((value.fpNumber) ? formatDate(value.fpDate) : "");

                    if (value.dnattachment) {
                        value.dnattachment.forEach((item, index, arr) => {
                            if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) $('#attachment' + parseInt(item.docLink.replace('row', ''))).val(item.fileName);
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

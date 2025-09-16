'use strict';

var swalTitle = "Late Promo Creation Configuration";

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    Inputmask({
        alias: "numeric",
        allowMinus: false,
        autoGroup: true,
        digits: 0,
        groupSeparator: ",",
    }).mask("#config_501, #config_502");

    blockUI.block();
    Promise.all([getDataConfig()]).then(function () {
        blockUI.release();
    });
});

$('#config_501').on('keyup', function () {
    let field = $('#config_501');
    if (field.val() === "") {
        field.val('0');
    }
});

$('#config_502').on('keyup', function () {
    let field = $('#config_502');
    if (field.val() === "") {
        field.val('0');
    }
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");
    let data = [{
        id: "501",
        daysfrom: $('#config_501').val()
    },{
        id: "502",
        daysfrom: $('#config_502').val()
    }];
    let formData = new FormData();
    formData.append('configList', JSON.stringify(data));
    let url = "/configuration/late-promo-creation/update";
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
                    window.location.href = '/configuration/late-promo-creation';
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

const getDataConfig = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/late-promo-creation/get-data",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result.error) {
                    let values = result.data;
                    for (let i=0; i<values.length; i++) {
                        if (values[i].id === 501) {
                            $('#config_501').val(values[i].days);
                        } else if (values[i].id === 502) {
                            $('#config_502').val(values[i].days);
                        }
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

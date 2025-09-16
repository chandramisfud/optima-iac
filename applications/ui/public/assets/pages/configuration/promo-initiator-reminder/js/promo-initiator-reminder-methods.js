'use strict';

var swalTitle = "Promo Initiator Reminder Configuration";

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    $('#promo_plan_creation_q1, #promo_plan_creation_q2, #promo_plan_creation_q3, #promo_plan_creation_q4, #promo_recon').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    blockUI.block();
    Promise.all([getDataConfig()]).then(function () {
        blockUI.release();
    });
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");
    let data = [{
        id: 401,
        datereminder: $('#promo_plan_creation_q1').val()
    },{
        id: 402,
        datereminder: $('#promo_plan_creation_q2').val()
    },{
        id: 403,
        datereminder: $('#promo_plan_creation_q3').val()
    },{
        id: 404,
        datereminder: $('#promo_plan_creation_q4').val()
    },{
        id: 410,
        datereminder: $('#promo_recon').val()
    }];
    let formData = new FormData();
    formData.append('configList', JSON.stringify(data));
    let url = "/configuration/promo-initiator-reminder/update";
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
                    window.location.href = '/configuration/promo-initiator-reminder';
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
            url         : "/configuration/promo-initiator-reminder/get-data",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result.error) {
                    let values = result.data;
                    for (let i=0; i<values.length; i++) {
                        if (values[i].id === 401) {
                            $('#promo_plan_creation_q1').val(formatDate(values[i].datereminder)).flatpickr({
                                defaultDate: formatDate(values[i].datereminder)
                            });
                        } else if (values[i].id === 402) {
                            $('#promo_plan_creation_q2').val(formatDate(values[i].datereminder)).flatpickr({
                                defaultDate: formatDate(values[i].datereminder)
                            });
                        } else if (values[i].id === 403) {
                            $('#promo_plan_creation_q3').val(formatDate(values[i].datereminder)).flatpickr({
                                defaultDate: formatDate(values[i].datereminder)
                            });
                        } else if (values[i].id === 404) {
                            $('#promo_plan_creation_q4').val(formatDate(values[i].datereminder)).flatpickr({
                                defaultDate: formatDate(values[i].datereminder)
                            });
                        } else if (values[i].id === 410) {
                            $('#promo_recon').val(formatDate(values[i].datereminder)).flatpickr({
                                defaultDate: formatDate(values[i].datereminder)
                            });
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

'use strict';

var validator, method, profitcenterid, description;
var swalTitle = "Master Profit Center";

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
    profitcenterid = url_str.searchParams.get("profitcenterid");

    if (method === 'update') {
        Promise.all([ blockUI.block(), disableButtonSave() ]).then(async () => {
            await getData(profitcenterid);
            $('#profitcenter').addClass('form-control-solid-bg');
            $('#profitcenter').addClass('form-control-solid-bg');
            $('#profitcenter').attr('readonly', true);
            $('#txt_info_method').text('Edit Profit Center ' + description);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ blockUI.block(), disableButtonSave() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_profitcenter');

    validator = FormValidation.formValidation(form, {
        fields: {
            profitcenter: {
                validators: {
                    notEmpty: {
                        message: "Profit Center must be enter"
                    },
                    stringLength: {
                        max: 10,
                        message: 'Profit Center must be less than 10 characters',
                    },
                    regexp: {
                        regexp: /^\d+$/,
                        message: 'Profit Center must be numeric'
                    }
                }
            },
            description: {
                validators: {
                    notEmpty: {
                        message: "Description must be enter"
                    },
                    stringLength: {
                        max: 50,
                        message: 'Description must be less than 50 characters',
                    }
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })
});

$('#btn_back').on('click', function() {
    window.location.href = '/master/profit-center';
});

$('#btn_save_exit').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save_action");
            save(true, e);
        }
    });
});

$('#btn_save_add').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save_action");
            save(false, e);
        }
    });
});

$('#btn_save').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_save");
            save(true, e);
        }
    });
});

const save = (exit, e) => {
    let formData = new FormData($('#form_profitcenter')[0]);
    let url = '/master/profit-center/save';
    if (method === "update") {
        formData.append('id', profitcenterid);
        url = '/master/profit-center/update';
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
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        if (exit) {
                            window.location.href = '/master/profit-center';
                        } else {
                            let form = document.querySelectorAll("#form_profitcenter input[type=text]");
                            formReset(form);
                        }
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "error",
                        confirmButtonText: "OK",
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
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        });
    });
}

const getData = (profitcenterid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/profit-center/data/id",
            type: "GET",
            data: {id:profitcenterid},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#profitcenter').val(values.profitCenter);
                    $('#description').val(values.profitCenterDesc);
                    description = values.profitCenterDesc;
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
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
        return reject(e);
    });
}

const formReset = (elements) => {
    for (var i = 0, len = elements.length; i < len; ++i) {
        elements[i].value = "";
    }
}

'use strict';

var validator;
var swalTitle = "Mapping Material Tax Level";
let elPpnPct = $('#ppnPct');
let elPphPct = $('#pphPct');

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
        alias: "decimal",
        radixPoint: ".",
        allowMinus: false,
        autoGroup: true,
        onfocus: "",
        groupSeparator: ",",
    }).mask("#ppnPct, #pphPct");

    validator =  FormValidation.formValidation(document.getElementById('form_mapping_tax_level'), {
        fields: {
            materialNumber: {
                validators: {
                    notEmpty: {
                        message: "Material Number must be enter"
                    },
                }
            },
            description: {
                validators: {
                    notEmpty: {
                        message: "Description must be enter"
                    },
                }
            },
            whT_Type: {
                validators: {
                    notEmpty: {
                        message: "WHT Type must be enter"
                    },
                }
            },
            whT_Code: {
                validators: {
                    notEmpty: {
                        message: "WHT Code must be enter"
                    },
                }
            },
            purpose: {
                validators: {
                    notEmpty: {
                        message: "Purpose must be enter"
                    },
                }
            },
            entityId: {
                validators: {
                    notEmpty: {
                        message: "Entity must be enter"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
        }
    });

    blockUI.block();
    disableButtonSave();
    Promise.all([getListEntity()]).then(function () {
        blockUI.release();
        enableButtonSave();
    });
});

elPpnPct.on('change', function () {
    if ($(this).val() === "") {
        $(this).val("0");
    }
});

elPpnPct.on('click', function () {
    $(this).select();
});

elPphPct.on('change', function () {
    if ($(this).val() === "") {
        $(this).val("0");
    }
});

elPphPct.on('click', function () {
    $(this).select();
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

const save = (exit, e) => {
    let formData = new FormData($('#form_mapping_tax_level')[0]);
    let data = $('#entityId').select2('data');
    let entity = ((data[0].id !== "") ? data[0].text : '');
    formData.append('entity', entity);

    let url = '/mapping/tax-level/save';
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
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
                    if (exit) {
                        window.location.href = '/mapping/tax-level';
                    } else {
                        let form = document.querySelectorAll("#form_mapping_tax_level input[type=text]");
                        formReset(form);
                    }
                });
            } else {
                Swal.fire({
                    title: swalTitle,
                    text: result.message,
                    icon: "warning",
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
}

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/tax-level/list/entity",
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
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const formReset = (elements) => {
    for (let i = 0, len = elements.length; i < len; ++i) {
        elements[i].value = "";
    }
}


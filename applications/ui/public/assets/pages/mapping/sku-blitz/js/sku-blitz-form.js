'use strict';

var validator;
var swalTitle = "Mapping SKU Blitz";

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    validator =  FormValidation.formValidation(document.getElementById('form_mapping_sku_blitz'), {
        fields: {
            entityId: {
                validators: {
                    notEmpty: {
                        message: "Entity must be enter"
                    },
                }
            },
            brandId: {
                validators: {
                    notEmpty: {
                        message: "Brand must be enter"
                    },
                }
            },
            skuId: {
                validators: {
                    notEmpty: {
                        message: "SKU must be enter"
                    },
                }
            },
            sapCode: {
                validators: {
                    notEmpty: {
                        message: "Blitz SKU Code must be enter"
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

$('#btn_back').on('click', function() {
    window.location.href = '/mapping/sku-blitz';
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
    let formData = new FormData($('#form_mapping_sku_blitz')[0]);
    let url = '/mapping/sku-blitz/save';
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
                    if (exit) {
                        window.location.href = '/mapping/sku-blitz';
                    } else {
                        let form = document.querySelectorAll("#form_mapping_sku_blitz input[type=text]");
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

$('#entityId').on('change', async function () {
    validator.resetForm(true);
});

$('#entityId').on('change', async function () {
    blockUI.block();
    $('#brandId').empty();
    if ($(this).val() !== "") await getListBrand($(this).val());
    blockUI.release();
    $('#brandId').val('').trigger('change');
    validator.resetForm(true);
});

$('#brandId').on('change', async function () {
    blockUI.block();
    $('#skuId').empty();
    if ($(this).val() !== "") await getListSKU($(this).val());
    blockUI.release();
    $('#skuId').val('').trigger('change');
    validator.resetForm(true);
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
    $.ajax({
            url         : "/mapping/sku-blitz/list/entity",
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

const getListBrand = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/sku-blitz/list/brand/entity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {entityId: entityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#brandId').select2({
                    placeholder: "Select a Brand",
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

const getListSKU = (brandId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/sku-blitz/list/sku/brand-id",
            type        : "GET",
            dataType    : 'json',
            data        : {brandId: brandId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#skuId').select2({
                    placeholder: "Select a SKU",
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
    for (var i = 0, len = elements.length; i < len; ++i) {
        elements[i].value = "";
    }
}

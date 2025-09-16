'use strict';

var validator, method, skuid, refId, entityid, brandid;
var swalTitle = "Master SKU";

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
    skuid = url_str.searchParams.get("skuid");
    blockUI.block();
    if (method === 'update') {
        Promise.all([ disableButtonSave(), getEntity() ]).then(async () => {
            await getData(skuid);
            await getBrand(entityid);
            await $('#brand').val(brandid).trigger('change');
            $('#txt_info_method').text('Edit SKU ' + refId);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ disableButtonSave(), getEntity() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_sku');

    validator = FormValidation.formValidation(form, {
        fields: {
            entity: {
                validators: {
                    notEmpty: {
                        message: "Entity must be enter"
                    },
                }
            },
            brand: {
                validators: {
                    notEmpty: {
                        message: "Brand must be enter"
                    },
                }
            },
            shortDesc: {
                validators: {
                    stringLength: {
                        max: 10,
                        message: 'Short desc must be less than 10 characters',
                    }
                }
            },
            longDesc: {
                validators: {
                    notEmpty: {
                        message: "Long desc must be enter"
                    },
                    stringLength: {
                        max: 50,
                        message: 'Long desc must be less than 50 characters',
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

$('#entity').on('change', async function () {
    blockUI.block();
    $('#brand').empty();
    if ($(this).val() !== "") await getBrand($(this).val());
    blockUI.release();
    $('#brand').val('').trigger('change');
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
    let formData = new FormData($('#form_sku')[0]);
    let url = '/master/sku/save';
    if (method === "update") {
        formData.append('id', skuid);
        url = '/master/sku/update';
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
                    text: result.message,
                    icon: "success",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    if (exit) {
                        window.location.href = '/master/sku';
                    } else {
                        let form = document.querySelectorAll("#form_sku input[type=text]");
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
}

const getData = (skuid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/sku/data/id",
            type: "GET",
            data: {id:skuid},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#refId').val(values.refId);
                    $('#entity').val(values.principalId).trigger('change.select2');
                    $('#shortDesc').val(values.shortDesc);
                    $('#longDesc').val(values.longDesc);
                    refId = values.refId;
                    entityid = values.principalId;
                    brandid = values.brandId;
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

const getEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/sku/get-list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#entity').select2({
                    placeholder: "Select a Entity",
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
        return reject(e);
    });
}

const getBrand = (entityid) => {
    var data = [];
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/sku/get-data/brand/entity-id",
            type        : "GET",
            data        : {PrincipalId:entityid},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#brand').select2({
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
        return reject(e);
    });
}

const formReset = (elements) => {
    for (var i = 0, len = elements.length; i < len; ++i) {
        elements[i].value = "";
    }
}

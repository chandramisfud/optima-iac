'use strict';

var validator, method, sellingpointid, regionid, refId;
var swalTitle = "Form Selling Point";

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
    sellingpointid = url_str.searchParams.get("sellingpointid");
    blockUI.block();
    disableButtonSave();
    if (method === 'update') {
        Promise.all([ getRegion(), getProfitCenter() ]).then(async () => {
            await getData(sellingpointid);
            $('#txt_info_method').text('Edit Selling Point ' + refId);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ getRegion(), getProfitCenter() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_sellingpoint');

    validator = FormValidation.formValidation(form, {
        fields: {
            refId: {
                validators: {
                    notEmpty: {
                        message: "RefId must be enter"
                    },
                    stringLength: {
                        max: 10,
                        message: 'RefId must be less than 10 characters',
                    },
                    regexp: {
                        regexp: /^\d+$/,
                        message: 'Refid must be numeric'
                    }
                }
            },
            region: {
                validators: {
                    notEmpty: {
                        message: "Region must be enter"
                    },
                }
            },
            areaCode: {
                validators: {
                    stringLength: {
                        max: 3,
                        message: 'Area code must be less than 3 characters',
                    }
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

$('#btn_back').on('click', function() {
    window.location.href = '/master/selling-point';
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
    let formData = new FormData($('#form_sellingpoint')[0]);
    let url = '/master/selling-point/save';
    if (method === "update") {
        formData.append('id', sellingpointid);
        url = '/master/selling-point/update';
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
                            window.location.href = '/master/selling-point';
                        } else {
                            let form = document.querySelectorAll("#form_sellingpoint input[type=text]");
                            formReset(form);
                        }
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
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

const getData = (sellingpointid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/selling-point/data/id",
            type: "GET",
            data: {id:sellingpointid},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#refId').val(values.refId);
                    $('#region').val(values.regionId).trigger('change');
                    $('#areaCode').val(values.areaCode);
                    $('#profitcenter').val(values.profitCenter).trigger('change');
                    $('#shortDesc').val(values.shortDesc);
                    $('#longDesc').val(values.longDesc);
                    refId = values.refId;
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

const getRegion = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/selling-point/get-list/region",
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
                $('#region').select2({
                    placeholder: "Select a Region",
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

const getProfitCenter = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/selling-point/get-list/profit-center",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].profitCenter,
                        text: result.data[j].profitCenterDesc
                    });
                }
                $('#profitcenter').select2({
                    placeholder: "Select a Profit Center",
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

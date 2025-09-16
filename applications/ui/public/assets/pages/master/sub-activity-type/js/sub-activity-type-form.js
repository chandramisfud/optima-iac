'use strict';

var validator, method, activityTypeId, refId;
var swalTitle = "Sub Activity Type Promo";

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
    activityTypeId = url_str.searchParams.get("activitytypeid");
    blockUI.block();
    if (method === 'update') {
        Promise.all([ disableButtonSave() ]).then(async () => {
            await getData(activityTypeId);
            $('#txt_info_method').text('Edit Sub Activity Type ' + refId);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ disableButtonSave() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_subactivitytype');

    validator = FormValidation.formValidation(form, {
        fields: {
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
    window.location.href = '/master/sub-activity-type';
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
    let formData = new FormData($('#form_subactivitytype')[0]);
    let url = '/master/sub-activity-type/save';
    if (method === "update") {
        formData.append('id', activityTypeId);
        url = '/master/sub-activity-type/update';
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
                            window.location.href = '/master/sub-activity-type';
                        } else {
                            let form = document.querySelectorAll("#form_subactivitytype input[type=text]");
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

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/sub-activity-type/data/id",
            type: "GET",
            data: {id:id},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#refId').val(values.refId);
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

const formReset = (elements) => {
    for (var i = 0, len = elements.length; i < len; ++i) {
        elements[i].value = "";
    }
}

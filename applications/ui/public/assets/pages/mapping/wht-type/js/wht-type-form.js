'use strict';

let validator, method, id;
let swalTitle = "Mapping WHT Type";

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

    validator =  FormValidation.formValidation(document.getElementById('form_mapping_wht_type'), {
        fields: {
            distributor: {
                validators: {
                    notEmpty: {
                        message: "Distributor must be enter"
                    },
                }
            },
            subActivity: {
                validators: {
                    notEmpty: {
                        message: "Sub Activity must be enter"
                    },
                }
            },
            subAccount: {
                validators: {
                    notEmpty: {
                        message: "Sub Account must be enter"
                    },
                }
            },
            WHTType: {
                validators: {
                    notEmpty: {
                        message: "WHT Type must be enter"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
        }
    });

    if (method === 'update') {
        blockUI.block();
        disableButtonSave();
        Promise.all([getListDistributor(), getListSubActivity(), getListSubAccount(), getListWHTType()]).then(async () => {
            await getData(id);
            blockUI.release();
            enableButtonSave();
        });
    } else {
        blockUI.block();
        disableButtonSave();
        Promise.all([getListDistributor(), getListSubActivity(), getListSubAccount(), getListWHTType()]).then(function () {
            blockUI.release();
            enableButtonSave();
        });
    }
});

$('#btn_back').on('click', function() {
    window.location.href = '/mapping/wht-type';
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
            save(false, e);
        }
    });
});

const save = (exit, e) => {
    let formData = new FormData($('#form_mapping_wht_type')[0]);
    let url;
    if(method === 'update'){
        url = '/mapping/wht-type/update';
        formData.append('id', id);
        formData.append('whtType', $('#WHTType').val());
    } else {
        url = '/mapping/wht-type/save';
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
        success: function(result) {
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
                    if(method === 'update'){
                        window.location.href = '/mapping/wht-type';
                    } else {
                        if (exit) {
                            window.location.href = '/mapping/wht-type';
                        } else {
                            let form = document.querySelectorAll("#form_mapping_wht_type input[type=text]");
                            formReset(form);
                        }
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
            console.log(errorThrown)
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

const getListDistributor = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/mapping/wht-type/list/distributor",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                $('#distributor').select2({
                    width: '100%',
                    data: result.data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    });
}

const getListSubActivity = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/mapping/wht-type/list/sub-activity",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                $('#subActivity').select2({
                    width: '100%',
                    data: result.data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    });
}

const getListSubAccount = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/mapping/wht-type/list/sub-account",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                $('#subAccount').select2({
                    width: '100%',
                    data: result.data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    });
}

const getListWHTType = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/mapping/wht-type/list/wht-type",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                $('#WHTType').select2({
                    width: '100%',
                    data: result.data
                });
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(errorThrown);
                return reject(jqXHR.responseText);
            }
        });
    });
}

const getData = (id) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/mapping/wht-type/get-data/id",
            type: "GET",
            data: {id: id},
            dataType: 'json',
            async: true,
            success: async function (result) {
                if (!result.error) {
                    let value = result.data[0];

                    $('#distributor').val(value['distributor']).trigger('change');
                    $('#subActivity').val(value['subActivity']).trigger('change');
                    $('#subAccount').val(value['subAccount']).trigger('change');

                    $('#distributor').attr('readonly', 'readonly');
                    $('#subActivity').attr('readonly', 'readonly');
                    $('#subAccount').attr('readonly', 'readonly');

                    $('#WHTType').val(value['WHTType']).trigger('change');
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
                if (jqXHR.status === 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: errorThrown,
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

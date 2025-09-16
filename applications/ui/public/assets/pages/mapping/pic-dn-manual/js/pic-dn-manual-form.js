'use strict';

var  validator;
var swalTitle = "Mapping PIC DN Manual  ";

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    validator =  FormValidation.formValidation(document.getElementById('form_mapping_pic_dn_manual'), {
        fields: {
            channelId: {
                validators: {
                    notEmpty: {
                        message: "Channel must be enter"
                    },
                }
            },
            subChannelId: {
                validators: {
                    notEmpty: {
                        message: "Sub Channel must be enter"
                    },
                }
            },
            accountId: {
                validators: {
                    notEmpty: {
                        message: "Account must be enter"
                    },
                }
            },
            subAccountId: {
                validators: {
                    notEmpty: {
                        message: "Sub Account must be enter"
                    },
                }
            },
            pic1: {
                validators: {
                    notEmpty: {
                        message: "PIC 1 must be enter"
                    },
                }
            },
            pic2: {
                validators: {
                    notEmpty: {
                        message: "PIC 2 must be enter"
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
    Promise.all([getListChannel(), getListUserProfile()]).then(function () {
        blockUI.release();
        enableButtonSave();
    });
});

$('#btn_back').on('click', function() {
    window.location.href = '/mapping/pic-dn-manual';
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
    let formData = new FormData($('#form_mapping_pic_dn_manual')[0]);
    let url = '/mapping/pic-dn-manual/save';
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
                        window.location.href = '/mapping/pic-dn-manual';
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

$('#channelId').on('change', async function () {
    blockUI.block();
    $('#subChannelId').empty();
    if ($(this).val() !== "") await getListSubChannel($(this).val());
    blockUI.release();
    $('#subChannelId').val('').trigger('change');
    validator.resetForm(true);
});

$('#subChannelId').on('change', async function () {
    blockUI.block();
    $('#accountId').empty();
    if ($(this).val() !== "") await getListAccount($(this).val());
    blockUI.release();
    $('#accountId').val('').trigger('change');
    validator.resetForm(true);
});

$('#accountId').on('change', async function () {
    blockUI.block();
    $('#subAccountId').empty();
    if ($(this).val() !== "") await getListSubAccount($(this).val());
    blockUI.release();
    $('#subAccountId').val('').trigger('change');
    validator.resetForm(true);
});

$('#subAccountId').on('change', async function () {
    validator.resetForm(true);
});

const getListChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/pic-dn-manual/list/channel",
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
                $('#channelId').select2({
                    placeholder: "Select a Channel",
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

const getListSubChannel = (channelId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/pic-dn-manual/list/sub-channel/channel-id",
            type        : "GET",
            dataType    : 'json',
            data        : {channelId: channelId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#subChannelId').select2({
                    placeholder: "Select a Sub Channel",
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

const getListAccount = (subChannelId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/pic-dn-manual/list/account/sub-channel-id",
            type        : "GET",
            dataType    : 'json',
            data        : {subChannelId: subChannelId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#accountId').select2({
                    placeholder: "Select an Account",
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

const getListSubAccount = (accountId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/pic-dn-manual/list/sub-account/account-id",
            type        : "GET",
            dataType    : 'json',
            data        : {accountId: accountId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#subAccountId').select2({
                    placeholder: "Select a Sub Account",
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


const getListUserProfile = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/mapping/pic-dn-manual/list/user-profile",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].id
                    });
                }
                $('#pic1').select2({
                    placeholder: "Select a PIC 1",
                    width: '100%',
                    data: data
                });
                $('#pic2').select2({
                    placeholder: "Select a PIC 2",
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

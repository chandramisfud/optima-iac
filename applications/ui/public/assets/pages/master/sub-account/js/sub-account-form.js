'use strict';

var validator, method, subaccountid, subaccountRefId, subChannelId, accountId;
var swalTitle = "Sub Account";

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
    subaccountid = url_str.searchParams.get("subaccountid");
    blockUI.block();
    disableButtonSave();
    if (method === 'update') {
        Promise.all([ getChannel() ]).then(async () => {
            await getData(subaccountid);
            await getSubChannel($('#channel').val());
            $('#subchannel').val(subChannelId).trigger('change.select2');
            await getAccount($('#subchannel').val());
            $('#account').val(accountId).trigger('change.select2');
            $('#txt_info_method').text('Edit Sub Account ' + subaccountRefId);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ getChannel() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_subaccount');

    validator = FormValidation.formValidation(form, {
        fields: {
            channel: {
                validators: {
                    notEmpty: {
                        message: "Channel must be enter"
                    },
                }
            },
            subchannel: {
                validators: {
                    notEmpty: {
                        message: "Subchannel must be enter"
                    },
                }
            },
            account: {
                validators: {
                    notEmpty: {
                        message: "Account must be enter"
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

$('#channel').on('change', async function () {
    blockUI.block();
    $('#subchannel').empty();
    if ($(this).val() !== "") await getSubChannel($(this).val());
    blockUI.release();
    $('#subchannel').val('').trigger('change');
});

$('#subchannel').on('change', async function () {
    blockUI.block();
    $('#account').empty();
    if ($(this).val() !== "") await getAccount($(this).val());
    blockUI.release();
    $('#account').val('').trigger('change');
});

$('#btn_back').on('click', function() {
    window.location.href = '/master/sub-account';
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
    let formData = new FormData($('#form_subaccount')[0]);
    let url = '/master/sub-account/save';
    if (method === "update") {
        formData.append('id', subaccountid);
        url = '/master/sub-account/update';
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
                            window.location.href = '/master/sub-account';
                        } else {
                            let form = document.querySelectorAll("#form_subaccount input[type=text]");
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

const getData = (subaccountid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/sub-account/data/id",
            type: "GET",
            data: {id:subaccountid},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#refId').val(values.refId);
                    $('#channel').val(values.channelId).trigger('change.select2');
                    $('#shortDesc').val(values.shortDesc);
                    $('#longDesc').val(values.longDesc);
                    subaccountRefId = values.refId;
                    subChannelId = values.subChannelId;
                    accountId = values.accountId;
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
    });
}

const getChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/sub-account/get-list/channel",
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
                $('#channel').select2({
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
        return reject(e);
    });
}

const getSubChannel = (channelid) => {
    var data = [];
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/sub-account/get-data/sub-channel/channel-id",
            type        : "GET",
            data        : {ChannelId:channelid},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#subchannel').select2({
                    placeholder: "Select a Subchannel",
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

const getAccount = (subchannelid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/sub-account/get-data/account/sub-channel-id",
            type        : "GET",
            data        : {SubChannelId:subchannelid},
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
                $('#account').select2({
                    placeholder: "Select a Account",
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

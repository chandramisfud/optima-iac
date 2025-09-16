'use strict';

var validator, method, userlevel;
var swalTitle = "User Group Rights";

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
        alias: "numeric",
        allowMinus: false,
        autoGroup: false,
        digits: 0,
    }).mask("#userlevel");

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    userlevel = url_str.searchParams.get("userlevel");

    if (method === 'update') {
        blockUI.block();
        disableButtonSave();
        Promise.all([getGroupMenu()]).then(async () => {
            await getData(userlevel);
            $('#userlevel').attr('readOnly', true);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        blockUI.block();
        disableButtonSave();
        Promise.all([getGroupMenu()]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_groupuserrights');

    validator = FormValidation.formValidation(form, {
        fields: {
            userlevel: {
                validators: {
                    notEmpty: {
                        message: "The User Group Rights ID must be numbered"
                    },
                    stringLength: {
                        max: 15,
                        message: 'User Group Rights ID must be less than 15 characters',
                    }
                }
            },
            usergroupid: {
                validators: {
                    notEmpty: {
                        message: "User Group Menu must be enter"
                    },
                }
            },
            levelname: {
                validators: {
                    notEmpty: {
                        message: "User Group Rights Name must be enter"
                    },
                    stringLength: {
                        max: 100,
                        message: 'User Group Rights Name must be less than 100 characters',
                    }
                }
            }
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })

    $(form.querySelector('[name="usergroupid"]')).on('change', function () {
        validator.revalidateField('usergroupid');
    });
});

$('#btn_back').on('click', function() {
    window.location.href = '/useraccess/group-rights';
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
    let formData = new FormData($('#form_groupuserrights')[0]);
    let url = '/useraccess/group-rights/save';
    if (method === "update") {
        url = '/useraccess/group-rights/update';
    }
    $.get('/refresh-csrf').done(function(data) {
        $('meta[name="csrf-token"]').attr('content', data)
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            }
        });
        $.ajax({
            url: url,
            data: formData,
            type: 'POST',
            async: true,
            dataType: 'JSON',
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function () {
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
            },
            success: function (result, status, xhr, $form) {
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
                            window.location.href = '/useraccess/group-rights';
                        } else {
                            let form = document.querySelectorAll("#form_groupuserrights input[type=text]");
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
            complete: function () {
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
}

const getData = (userlevel) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/group-rights/get-data/id",
            type: "GET",
            data: {userlevel:userlevel},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#txt_info_method').text('Edit Data');
                    $('#userlevel').val(values.userlevel);
                    $('#usergroupid').val(values.usergroupid).trigger('change');
                    $('#levelname').val(values.levelname);

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
    });
}

const getGroupMenu = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/useraccess/group-rights/get-data/usergroupmenu",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].userGroupId,
                        text: result.data[j].userGroupName
                    });
                }
                $('#usergroupid').select2({
                    placeholder: "Select a User Group Menu",
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

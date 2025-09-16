'use strict';

var validator, method, usergroupid, usergroupname;
var swalTitle = "User Group Menu";

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
    usergroupid = url_str.searchParams.get("usergroupid");

    if (method === 'update') {
        Promise.all([ blockUI.block(), disableButtonSave(), getGroupMenuPermission() ]).then(async () => {
            await getData(usergroupid);
            $('#usergroupid').attr('readOnly', true);
            $('#txt_info_method').text('Edit User Group ' + usergroupname);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ blockUI.block(), disableButtonSave(), getGroupMenuPermission() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_groupuser');

    validator = FormValidation.formValidation(form, {
        fields: {
            usergroupid: {
                validators: {
                    notEmpty: {
                        message: "User Group ID must be input"
                    },
                    stringLength: {
                        max: 15,
                        message: 'User Group ID must be less than 15 characters',
                    }
                }
            },
            usergroupname: {
                validators: {
                    notEmpty: {
                        message: "User Group Name must be enter"
                    },
                    stringLength: {
                        max: 100,
                        message: 'User Group Name must be less than 100 characters',
                    }
                }
            },
            groupmenupermission: {
                validators: {
                    notEmpty: {
                        message: "Group Menu Permission must be enter"
                    },
                }
            }
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });


    $(form.querySelector('[name="groupmenupermission"]')).on('change', function () {
        validator.revalidateField('groupmenupermission');
    });

});

$('#btn_back').on('click', function() {
    window.location.href = '/useraccess/group-menu';
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
    let formData = new FormData($('#form_groupuser')[0]);
    let url = '/useraccess/group-menu/save';
    if (method === "update") {
        url = '/useraccess/group-menu/update';
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
                            window.location.href = '/useraccess/group-menu';
                        } else {
                            let form = document.querySelectorAll("#form_groupuser input[type=text]");
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

const getData = (usergroupid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/group-menu/get-data/id",
            type: "GET",
            data: {usergroupid:usergroupid},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#usergroupid').val(values.usergroupid);
                    $('#usergroupname').val(values.usergroupname);
                    $('#groupmenupermission').val(values.groupmenupermission).trigger('change');
                    usergroupname = values.usergroupname;
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
    });
}

const getGroupMenuPermission = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/useraccess/group-menu/get-data/groupmenupermission",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].name
                    });
                }
                $('#groupmenupermission').select2({
                    placeholder: "Select a Group Menu Permission",
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

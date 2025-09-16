'use strict';

var  usergroupid, menuid, method;
var swalTitle = "User Group Rights Configuration";

var target = document.querySelector(".card_menu");
var blockUI = new KTBlockUI(target, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

var targetRights = document.querySelector(".card_rights");
var blockUIRights = new KTBlockUI(targetRights, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

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


    blockUI.block();
    Promise.all([getDataUserGroupById(usergroupid), getDataUserLevelbyUserGroupId(usergroupid)]).then(function() {
        blockUI.release();
    });

});

$('#usergrouprights').on('change', async function () {
    $('#card_rights').addClass('d-none');
    if ($(this).val() === "") {
        $('#tree-container').jstree('destroy');
    } else {
        blockUI.block();
        await getMenuByUserLevel($(this).val());
        blockUI.release();
    }
});

$('#btn_back').on('click', function() {
    window.location.href = '/useraccess/group-menu';
});

$('#btn_save').on('click', function() {
    let e = document.querySelector("#btn_save");
    let formData = new FormData($('#form_user_rights')[0]);
    formData.append('usergroupid', usergroupid);
    formData.append('userlevel', $('#usergrouprights').val());
    formData.append('menuid', menuid);
    let url = "/useraccess/group-menu/save-grouprights-config";

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
                blockUI.block();
                blockUIRights.block();
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
                        $('#card_rights').addClass('d-none');
                    });
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "error",
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
                blockUI.release();
                blockUIRights.release();
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
});

const getDataUserGroupById = (usergroupid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/group-menu/get-data/id",
            type: "GET",
            data: {usergroupid: usergroupid},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    $('#txt_info_method').text("User Group Rights Configuration " + result.data.usergroupname);
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
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject();
            }
        });
    });
}

const getDataUserLevelbyUserGroupId = (usergroupid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/group-menu/list/userlevel/usergroupid",
            type: "GET",
            data: {usergroupid: usergroupid},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let data = [];
                    for (let j = 0, len = result.data.length; j < len; ++j) {
                        data.push({
                            id: result.data[j].userlevel,
                            text: result.data[j].levelname
                        });
                    }
                    $('#usergrouprights').select2({
                        placeholder: "Select a User Group Rights",
                        width: '100%',
                        data: data
                    });
                }
            },
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject();
            }
        });
    });
}

const getMenuByUserLevel = (userlevelid) => {
    return new Promise((resolve, reject) => {
        $('#tree-container').jstree('destroy');
        $("#tree-container").on('before_open.jstree', function (e, data) {
            data.node.state.disabled && $("#tree-container").jstree().close_node(data.node);
        }).jstree({
            'plugins': ['wholerow', 'themes', 'ui'],
            "core": {
                "check_callback": true,
                'data': {
                    'url': function (node) {
                        return '/useraccess/group-menu/list/menu/userlevel?userlevelid=' + userlevelid;
                    },
                    'data': function (node) {
                        return {
                            'parent': node.id
                        };
                    },
                    "plugins": ['wholerow'],
                    "dataType": "json" // needed only if you do not supply JSON headers
                }

            },
        });
        $("#tree-container").on("select_node.jstree", async function(evt, data){
            $('#title_hak_akses').text("Configuration Menu " + data.node.text);
            blockUI.block();
            $('#card_rights').addClass('d-none');
            await getUserRights(usergroupid, $('#usergrouprights').val(), data.node.id);
            menuid = data.node.id;
            blockUI.release();
        });
        return resolve();
    });
}

const getUserRights = (usergroupid, userlevelid, menuid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/group-menu/get-data/userrights",
            type: "GET",
            data: {
                usergroupid: usergroupid,
                userlevelid: userlevelid,
                menuid: menuid
            },
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    let checkbox_create = $('#create_rec');
                    let checkbox_update = $('#update_rec');
                    let checkbox_delete = $('#delete_rec');
                    let checkbox_approve = $('#approve_rec');

                    checkbox_create.prop('checked', false);
                    checkbox_update.prop('checked', false);
                    checkbox_update.prop('checked', false);
                    checkbox_approve.prop('checked', false);
                    if (values.crud === 1 && values.approve === 0) {
                        $('#card_rights').removeClass('d-none');

                        checkbox_approve.attr('checked', false).attr('disabled', true);

                        (values.c === 1) ? checkbox_create.attr('disabled', false).prop('checked', true) : checkbox_create.attr('disabled', false).prop('checked', false);
                        (values.u === 1) ? checkbox_update.attr('disabled', false).prop('checked', true) : checkbox_update.attr('disabled', false).prop('checked', false);
                        (values.d === 1) ? checkbox_delete.attr('disabled', false).prop('checked', true) : checkbox_delete.attr('disabled', false).prop('checked', false);
                    } else if (values.approve === 1 && values.crud === 0) {
                        $('#card_rights').removeClass('d-none');

                        checkbox_create.attr('disabled', true);
                        checkbox_update.attr('disabled', true);
                        checkbox_delete.attr('disabled', true);
                        checkbox_approve.attr('disabled', false).prop('checked', true).attr('disabled', true);
                    } else if (values.approve === 1 && values.crud === 1) {

                        checkbox_create.attr('disabled', false);
                        checkbox_update.attr('disabled', false);
                        checkbox_delete.attr('disabled', false);
                        checkbox_approve.attr('disabled', false).prop('checked', true).attr('disabled', true);

                        (values.c === 1) ? checkbox_create.attr('checked', true) : checkbox_create.prop('checked', false);
                        (values.u === 1) ? checkbox_update.attr('checked', true) : checkbox_update.prop('checked', false);
                        (values.d === 1) ? checkbox_delete.attr('checked', true) : checkbox_delete.prop('checked', false);
                    }
                }
            },
            complete: function () {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject();
            }
        });
    });
}

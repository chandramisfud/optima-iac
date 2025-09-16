'use strict';

var  usergroupid, method;
var swalTitle = "User Group Menu Configuration";

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

    getDataUserGroupById(usergroupid)
    getMenuByUserGroup(usergroupid);

});

$('#btn_back').on('click', function() {
    window.location.href = '/useraccess/group-menu';
});

$('#btn_save').on('click', function () {
    let selected = $('#tree-container').jstree(true).get_selected();
    let selectedNode= $('#tree-container').jstree(true).get_selected('full',true);
    let data = [];
    for (let i = 0, len = selected.length; i < len; ++i){
        let rec = [];
        rec["usergroupid"]  = usergroupid;
        rec["menuid"]       = selected[i];
        data.push(assign({}, rec));
    }

    for (let i = 0, len = selectedNode.length; i < len; ++i){
        let rec = [];
        rec["usergroupid"]  = usergroupid;
        rec["menuid"]       = selectedNode[i].parent;
        data.push(assign({}, rec));
        if (selectedNode[i].parents.length > 0) {
            for (let j=0; j<selectedNode[i].parents.length; j++) {
                if (selectedNode[i].parents[j] !== '#') {
                    rec["usergroupid"]  = usergroupid;
                    rec["menuid"]       = selectedNode[i].parents[j];
                    data.push(assign({}, rec));
                }
            }
        }
    }

    let dobel = [];
    for (let i = 0, len = data.length; i < len; ++i){
        dobel.push(data[i].menuid);

    }
    let dataFilter = [];
    $.each(dobel, function(i, el){
        if($.inArray(el, dataFilter) === -1) dataFilter.push(el);
    });
    let result = [];
    for (let i = 0, len = dataFilter.length; i < len; ++i) {
        let rec = [];
        rec["usergroupid"]  = usergroupid;
        rec["menuid"]       = dataFilter[i];
        result.push(Object.assign({}, rec));
    }

    let e = document.querySelector("#btn_save");
    let formData = new FormData();
    formData.append('usergroupid', usergroupid);
    formData.append('userRightArrays', JSON.stringify(result));
    let url = '/useraccess/group-menu/save-groupmenu-config';

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
                        window.location.href = '/useraccess/group-menu';
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
    $.ajax({
        url: "/useraccess/group-menu/get-data/id",
        type: "GET",
        data: {usergroupid:usergroupid},
        dataType: "JSON",
        success: function (result) {
            if (!result.error) {
                $('#txt_info_method').text("Change Menu " + result.data.usergroupname);
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

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.responseText);
        }
    });
}

const getMenuByUserGroup = (usergroupid) => {
    $("#tree-container").on('before_open.jstree', function(e, data) {
        data.node.state.disabled && $("#tree-container").jstree().close_node(data.node);
    })
    .jstree({
        'plugins': ['wholerow', 'checkbox', 'themes', 'ui', 'real_checkboxes'],
        "core" : {
            "check_callback": true,
            'data' : {
                'url': function(node) {
                    return '/useraccess/group-menu/list/menu/usergroupid?usergroupid=' + usergroupid;
                },
                'data': function(node) {
                    return {
                        'parent': node.id
                    };
                },
                "plugins" : [ 'wholerow', 'checkbox' ],
                "dataType" : "json" // needed only if you do not supply JSON headers
            }

        }
    });
}

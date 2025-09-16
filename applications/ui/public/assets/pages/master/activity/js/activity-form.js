'use strict';

var validator, method, activityid, activityRefId, subCategoryid;
var swalTitle = "Master Activity Promo";

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
    activityid = url_str.searchParams.get("activityid");
    if (method === 'update') {
        blockUI.block();
        disableButtonSave();
        Promise.all([getCategory() ]).then(async () => {
            await getData(activityid);
            await getSubCategory( $('#category').val());
            $('#subcategory').val(subCategoryid).trigger('change');
            $('#txt_info_method').text('Edit Sub Account ' + activityRefId);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        Promise.all([ blockUI.block(), disableButtonSave(), getCategory() ]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_activity');

    validator = FormValidation.formValidation(form, {
        fields: {
            category: {
                validators: {
                    notEmpty: {
                        message: "Category must be enter"
                    },
                }
            },
            subcategory: {
                validators: {
                    notEmpty: {
                        message: "Subcategory must be enter"
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

$('#category').on('change', async function () {
    let val = $(this).val();
    if(!method){
        blockUI.block();
    }
    if (val === "") {
        $('#subcategory')
            .empty()
            .append('<option selected="selected" value=""></option>');
    } else {
        $('#subcategory')
            .empty()
            .append('<option selected="selected" value=""></option>');
        await getSubCategory(val);
    }
    blockUI.release();
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
    let formData = new FormData($('#form_activity')[0]);
    let url = '/master/activity/save';
    if (method === "update") {
        formData.append('id', activityid);
        url = '/master/activity/update';
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
                            window.location.href = '/master/activity';
                        } else {
                            let form = document.querySelectorAll("#form_activity input[type=text]");
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

const getData = (activityid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/master/activity/data/id",
            type: "GET",
            data: {id:activityid},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#refId').val(values.refId);
                    $('#category').val(values.categoryId).trigger('change.select2');
                    $('#shortDesc').val(values.shortDesc);
                    $('#longDesc').val(values.longDesc);
                    subCategoryid = values.subCategoryId
                    activityRefId = values.refId;
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

const getCategory= () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/activity/get-list/category",
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
                $('#category').select2({
                    placeholder: "Select a Category",
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

const getSubCategory = (categoryid) => {
    var data = [];
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/activity/get-data/sub-category/category-id",
            type        : "GET",
            data        : {CategoryId:categoryid},
            dataType    : 'json',
            async       : true,
            success: function(result) {
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#subcategory').select2({
                    placeholder: "Select a Sub Category",
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
